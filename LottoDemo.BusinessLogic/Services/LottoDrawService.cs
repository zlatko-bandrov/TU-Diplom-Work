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

        private int _iterationIndex = 0;
        private static int _iterationsCount = -1;
        private static readonly HttpClient _httpClient = new HttpClient();

        private static readonly string _webApiUrl = ConfigurationManager.AppSettings["webAPIUrl"];
        public static string WebApiUrl
        {
            get { return _webApiUrl; }
        }

        private static Guid _lotteryGameKey;
        public static Guid LotteryGameKey
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

        public double DrawingTimeInterval
        {
            get { return GameSettings != null ? GameSettings.DrawingTimeInterval : 5; }
        }

        public static int DrawingsCount
        {
            get
            {
                if (_iterationsCount <= -1)
                {
                    CultureInfo formatProvider = CultureInfo.InvariantCulture;
                    _iterationsCount = int.Parse(ConfigurationManager.AppSettings["NumberOfDrawings"], formatProvider);
                }

                return _iterationsCount;
            }
        }

        public LottoGameSettings GameSettings { get; internal set; }

        public LotteryGameService GameService
        {
            get { return LotteryGameService.Instance; }
        }

        #endregion
        
        public void ExecuteLottoDraw(Object stateInfo)
        {
            AutoResetEvent autoEvent = (AutoResetEvent)stateInfo;
            DateTime lottoDrawTime = DateTime.Now;
            this._iterationIndex++;

            var drawNumbersTask = Task.Run(() =>
            {
                // Generate lottery game drawing numbers
                List<byte> numbers = this.RunLottoDrawing();

                string infoMessage = string.Format("Drawing at {0}: {1}", lottoDrawTime.ToString("HH:mm:ss"), string.Join(", ", numbers));
                WriteLogMessage(infoMessage);

                // Create and save the lottery drawing in the database
                LottoDrawing lotteryDrawing = GameService.CreateNewLottoDraw(lottoDrawTime, numbers, LotteryGameKey);

                return lotteryDrawing;
            });
            drawNumbersTask.ContinueWith((antecedent) =>
            {
                LottoDrawing drawing = antecedent.Result;
                GameService.CalculateLottoDrawWinnings(drawing);
            },
            TaskContinuationOptions.OnlyOnRanToCompletion);

            try
            {
                drawNumbersTask.Wait();
                if (this._iterationIndex == DrawingsCount)
                {
                    // Reset the counter and signal the waiting thread.
                    this._iterationIndex = 0;
                    autoEvent.Set();
                }
            }
            catch (Exception error)
            {
                Log.Error(MethodBase.GetCurrentMethod().DeclaringType, error.Message, error);
                this._iterationIndex = 0;
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

        private List<byte> RunLottoDrawing()
        {
            List<byte> draw = (List<byte>)GenerateDrawNumbers(1, GameSettings.GameMaxNumber, GameSettings.BallsCount);
            return draw;
        }

        public bool InitGameSettings()
        {
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

                return GameSettings != null;
            }
            else
            {
                LottoDrawService.WriteLogMessage(new Exception(string.Format("Error Code {0} : Message - {1}\n", response.StatusCode, response.ReasonPhrase)));
                return false;
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
