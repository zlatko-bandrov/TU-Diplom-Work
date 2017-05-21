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

        private readonly string _webApiUrl = ConfigurationManager.AppSettings["webAPIUrl"];
        public string WebApiUrl
        {
            get { return _webApiUrl; }
        }

        private Guid _lotteryGameKey;
        public Guid LotteryGameKey
        {
            get
            {
                if (_lotteryGameKey == Guid.Empty)
                {
                    _lotteryGameKey = Guid.Parse(ConfigurationManager.AppSettings["LotteryGameKey"]);
                }
                return _lotteryGameKey;
            }
        }

        public LottoGameSettings GameSettings { get; set; }

        public double DrawingTimeInterval { get { return GameSettings != null ? GameSettings.DrawingTimeInterval : 5; } }

        public int LottoGameId { get; set; }

        public DateTime NextDrawExecution { get; set; }

        public int NextLottoDrawID { get; set; }

        #endregion


        public void ExecuteLottoDraw(Object stateInfo)
        {
            AutoResetEvent autoEvent = (AutoResetEvent)stateInfo;
            var drawNumbersTask = Task.Run(() =>
            {
                // Generate lottery game drawing numbers
                LotteryGameService gameService = new LotteryGameService();
                List<byte> balls = null;
                List<byte> bonusBalls = null;
                this.RunLottoDrawing(out balls, out bonusBalls);

                // Update lotto draw in the database
                LottoDrawing currentLottoDraw = gameService.UpdateLottoDraw(NextLottoDrawID, balls, bonusBalls);

                string updateMessage = string.Format("Draw with ID={3} was executed at {0}, Balls: {1}, Bonnus Balls: {2}; Updating draw to database...\n",
                        NextDrawExecution.ToString("HH:mm:ss"), string.Join(", ", balls), string.Join(", ", bonusBalls), NextLottoDrawID);
                WriteLogMessage(updateMessage);

                PrepareNextDraw();

                return currentLottoDraw;
            });
            drawNumbersTask.ContinueWith((antecedent) =>
            {
                LotteryGameService gameService = new LotteryGameService();
                LottoDrawing executedDraw = antecedent.Result;
                WriteLogMessage("Start winnings calculation.\n");
                gameService.DoExecutedDrawWinnings(executedDraw);
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

        private void RunLottoDrawing(out List<byte> ballsList, out List<byte> bonusBallsList)
        {
            ballsList = (List<byte>)GenerateDrawNumbers(1, GameSettings.GameMaxNumber, GameSettings.BallsCount);
            bonusBallsList = (List<byte>)GenerateDrawNumbers(GameSettings.BonusBallMin, GameSettings.BonusBallMax, GameSettings.BonusBallsCount);
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

        public bool Start()
        {
            bool result = InitGameSettings();
            if (result)
            {
                PrepareNextDraw();
                result = NextDrawExecution > DateTime.Now;
            }
            return result;
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

        private void SaveNextDrawToDatabase(int lottoGameId, DateTime drawTime)
        {
            LotteryGameService gameService = new LotteryGameService();
            int nextDrawId = -1;
            gameService.SaveNewDrawToDatabase(lottoGameId, drawTime, out nextDrawId);
            NextLottoDrawID = nextDrawId;
        }

        private void PrepareNextDraw()
        {
            LotteryGameService gameService = new LotteryGameService();
            var nextGameDraw = gameService.TakeTheNextDraw(LottoGameId);
            NextDrawExecution = gameService.GetTheNextDrawTime(TimeSpan.FromMinutes(GameSettings.DrawingTimeInterval), nextGameDraw);
            if (nextGameDraw == null)
            {
                SaveNextDrawToDatabase(LottoGameId, NextDrawExecution);
            }
            else
            {
                NextLottoDrawID = nextGameDraw.ID;
            }
            WriteLogMessage(string.Format("Next draw (ID={1}) is at: {0}...\n", NextDrawExecution.ToString("dd/MMM/yyyy HH:mm:ss"), NextLottoDrawID));
        }
    }
}
