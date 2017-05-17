using LottoDemo.BusinessLogic.Services;
using LottoDemo.Common.Services;
using LottoDemo.Entities.Models;
using System;
using System.Configuration;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading;

namespace TestConsoleApp
{
    class Program : IDisposable
    {
        private static Timer ServiceTimer;

        private static HttpClient _httpClient = new HttpClient();

        protected static Guid GetLottoGameKey
        {
            get
            {
                string value = ConfigurationManager.AppSettings["LotteryGameKey"];
                return Guid.Parse(value);
            }
        }

        protected static string GetWebApiUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["webAPIUrl"];
            }
        }

        protected static int DrawingsCount
        {
            get
            {
                CultureInfo formatProvider = CultureInfo.InvariantCulture;
                int drawingCount = int.Parse(ConfigurationManager.AppSettings["NumberOfDrawings"], formatProvider);

                return drawingCount;
            }
        }

        protected static string LotteryGameName
        {
            get
            {
                return ConfigurationManager.AppSettings["LottoGameName"];
            }
        }

        protected static LottoGameSettings GameSettings { get; set; }


        static void Main(string[] args)
        {
            try
            {
                if (!InitGameSettings())
                {
                    Console.ReadLine();
                    return;
                }

                LottoDrawService.WriteLogMessage("Start the number generation service.\n");

                LottoDrawService drawingService = new LottoDrawService(GameSettings, GetLottoGameKey, DrawingsCount);
                AutoResetEvent autoEvent = new AutoResetEvent(false);
                ServiceTimer = new Timer(drawingService.ExecuteLottoDraw, autoEvent, TimeSpan.Zero, TimeSpan.FromMinutes(drawingService.DrawingTimeInterval));
                autoEvent.WaitOne();

                LottoDrawService.WriteLogMessage("\nDestroying timer.");
                ServiceTimer.Dispose();

                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Log.Error(MethodBase.GetCurrentMethod().DeclaringType, ex.Message, ex);
            }
            finally
            {
                if (ServiceTimer != null)
                {
                    ServiceTimer.Dispose();
                }
                if (_httpClient != null)
                {
                    _httpClient.Dispose();
                }
            }
        }

        public void Dispose()
        {
            if (ServiceTimer != null)
            {
                ServiceTimer.Dispose();
            }
        }

        private static bool InitGameSettings()
        {
            string webApiMethod = string.Format("umbraco/Api/LotteryGame/GetGameSettings?gameKey={0}", GetLottoGameKey);
            _httpClient.BaseAddress = new Uri(GetWebApiUrl);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            LottoDrawService.WriteLogMessage("Start getting the game settings.");

            HttpResponseMessage response = _httpClient.GetAsync(webApiMethod).Result;

            if (response.IsSuccessStatusCode)
            {
                GameSettings = response.Content.ReadAsAsync<LottoDemo.Entities.Models.LottoGameSettings>().Result;
                LottoDrawService.WriteLogMessage("Game settins were retrieved successfully from the CMS.");
                return true;
            }
            else
            {
                LottoDrawService.WriteLogMessage(new Exception("Error Code" + response.StatusCode + " : Message - " + response.ReasonPhrase));
                return false;
            }
        }
    }
}