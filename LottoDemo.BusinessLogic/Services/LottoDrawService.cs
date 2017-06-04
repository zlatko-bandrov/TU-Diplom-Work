using LottoDemo.BusinessLogic.Games;
using LottoDemo.BusinessLogic.LotteryLogic;
using LottoDemo.Common.Services;
using LottoDemo.DataAccess;
using LottoDemo.Entities.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace LottoDemo.BusinessLogic.Services
{
    public class LottoDrawService
    {
        #region Properties

        private static readonly HttpClient _httpClient = new HttpClient();

        private static readonly HttpClient _httpJackpotClient = new HttpClient();

        public readonly string WebApiUrl = ConfigurationManager.AppSettings["webAPIUrl"];

        public readonly Guid LotteryGameKey = Guid.Parse(ConfigurationManager.AppSettings["LotteryGameKey"]);

        public int LottoGameId { get; set; }

        public LottoGameSettings GameSettings { get; set; }

        public TimeSpan GetDrawInterval()
        {
            TimeSpan timeInterval =
                GameSettings != null
                ? TimeSpan.FromMinutes(GameSettings.DrawingTimeInterval)
                : LotteryGameService.DefaultDrawInterval;
            return timeInterval;
        }

        public DateTime NextDrawRunTime { get; set; }

        public int NextDrawID = -1;

        #endregion

        public bool Start()
        {
            bool result = InitGameSettings();
            if (result)
            {
                PrepareNextDraw();
                result = NextDrawRunTime > DateTime.Now;
            }
            return result;
        }

        public void RunGameDrawing(Object stateInfo)
        {
            AutoResetEvent autoEvent = (AutoResetEvent)stateInfo;
            var drawNumbersTask = Task.Run(() =>
            {
                int lottoDrawId = -1;
                try
                {
                    List<byte> balls = null;
                    List<byte> bonusBalls = null;
                    GenerateBallNumbers(out balls, out bonusBalls);

                    // Update lotto draw in the database

                    var service = new LotteryGameService();
                    lottoDrawId = service.UpdateGameDraw(NextDrawID, balls, bonusBalls);

                    string updateMessage =
                        string.Format("Draw with ID {3} was launched at {0}, Balls: {1}, Bonnus Balls: {2}; \nUpdating draw to the database...\n",
                            NextDrawRunTime.ToString("HH:mm:ss"), string.Join(", ", balls), string.Join(", ", bonusBalls), lottoDrawId);
                    WriteLogMessage(updateMessage);

                    // Prepare the next draw
                    PrepareNextDraw();
                }
                catch (Exception ex)
                {
                    WriteLogMessage(ex);
                    throw ex;
                }

                return lottoDrawId;
            });

            drawNumbersTask.ContinueWith((antecedent) =>
            {
                try
                {
                    LotteryGameService gameService = new LotteryGameService();
                    decimal newJackpot = GameSettings.Jackpot;
                    gameService.CalculateDrawWinnings(antecedent.Result, GameSettings, out newJackpot);
                    UpdateTheGameJackpot(LotteryGameKey, newJackpot);
                    WriteLogMessage(string.Format("Game winning calculations were finisehd for the draw with ID {0}.\n", antecedent.Result));
                }
                catch (Exception ex)
                {
                    WriteLogMessage(ex);
                    throw ex;
                }
            },
            TaskContinuationOptions.OnlyOnRanToCompletion);

            try
            {
                drawNumbersTask.Wait();
            }
            catch (Exception error)
            {
                WriteLogMessage(error);
                autoEvent.Set();
            }
        }

        public static IEnumerable<byte> GenerateDrawNumbers(byte MinValue, byte MaxValue, byte ballsCount)
        {
            byte ballNumber = 0;
            List<byte> draw = new List<byte>();
            NumbersGenerator generator = new NumbersGenerator();
            do
            {
                ballNumber = (byte)generator.Next(MinValue, MaxValue);
                if (!draw.Contains(ballNumber) && ballNumber > 0)
                {
                    draw.Add(ballNumber);
                }
            }
            while (draw.Count < ballsCount);
            return draw;
        }


        private void GenerateBallNumbers(out List<byte> ballsList, out List<byte> bonusBallsList)
        {
            ballsList = (List<byte>)GenerateDrawNumbers(1, GameSettings.GameMaxNumber, GameSettings.BallsCount);
            bonusBallsList = (List<byte>)GenerateDrawNumbers(GameSettings.BonusBallMin, GameSettings.BonusBallMax, GameSettings.BonusBallsCount);
        }

        private bool InitGameSettings()
        {
            LotteryGameService gameService = new LotteryGameService();
            string webApiMethod = string.Format("umbraco/Api/LotteryGame/GetGameSettings?gameKey={0}", LotteryGameKey);
            _httpClient.BaseAddress = new Uri(WebApiUrl);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            LottoDrawService.WriteLogMessage("Loading game settings...");
            HttpResponseMessage response = _httpClient.GetAsync(webApiMethod).Result;
            if (response.IsSuccessStatusCode)
            {
                GameSettings = response.Content.ReadAsAsync<LottoDemo.Entities.Models.LottoGameSettings>().Result;
                LottoDrawService.WriteLogMessage(
                    string.Format("\"{0}\" game settins were retrieved successfully from the CMS.\n",
                        GameSettings != null ? GameSettings.GameDisplayName : string.Empty));
                var game = gameService.GetLotteryGameByKey(LotteryGameKey);
                if (game != null)
                {
                    this.LottoGameId = game.ID;
                }
                return GameSettings != null && this.LottoGameId > 0;
            }
            else
            {
                LottoDrawService.WriteLogMessage(new Exception(string.Format("Error Code {0} : Message - {1}\n", response.StatusCode, response.ReasonPhrase)));
                return false;
            }
        }

        private void PrepareNextDraw()
        {
            LotteryGameService gameService = new LotteryGameService();
            var lottoDraw = gameService.TakeTheNextDraw(LottoGameId);
            if (lottoDraw == null)
            {
                NextDrawRunTime = gameService.GetGameNextDrawTime(GetDrawInterval());
                lottoDraw = gameService.SaveNewDrawToDatabase(LottoGameId, NextDrawRunTime);
                NextDrawID = lottoDraw.ID;
            }
            else
            {
                NextDrawRunTime = lottoDraw.DrawTime;
                NextDrawID = lottoDraw.ID;
            }
            WriteLogMessage(string.Format("Next draw (ID={1}) will launch at: {0}...\n", NextDrawRunTime.ToString("dd MMM yyyy HH:mm:ss", CultureInfo.InvariantCulture), NextDrawID));
        }

        private void UpdateTheGameJackpot(Guid gameKey, decimal newJackpot)
        {
            string webApiMethod = string.Format("umbraco/Api/LotteryGame/UpdateGameJackpot?gameKey={0}&newJackpot={1}", gameKey, newJackpot);
            WriteLogMessage(string.Format("Update game key {0} jackpot - EUR {1}...", gameKey, newJackpot.ToString("0,0")));
            HttpResponseMessage response = _httpClient.PutAsync(webApiMethod, null).Result;

            if (response.IsSuccessStatusCode)
            {
                GameSettings.Jackpot = newJackpot;
            }
        }

        public static void WriteLogMessage(string infoMessage)
        {
            Console.WriteLine(infoMessage);
            Log.Info(MethodBase.GetCurrentMethod().DeclaringType, infoMessage);
        }

        public static void WriteLogMessage(Exception ex)
        {
            Console.WriteLine(ex.Message);
            Log.Error(MethodBase.GetCurrentMethod().DeclaringType, ex.Message, ex);
        }
    }
}
