using LottoDemo.BusinessLogic.LotteryLogic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Configuration;
using log4net;
using System.Reflection;
using LottoDemo.Common.Services;
using LottoDemo.BusinessLogic.Services;
using System.Globalization;
using System.Net.Http;
using LottoDemo.Entities.Models;
using System.Net.Http.Headers;

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

        protected static LottoGameSettings GameSettings { get; set; }


        static void Main(string[] args)
        {
            try
            {
                WriteMessage("Start getting the game settings.");

                InitGameSettings();



                Console.WriteLine("{0:h:mm:ss.fff} Creating timer.\n", DateTime.Now);

                CultureInfo formatProvider = CultureInfo.InvariantCulture;
                int numberOfIterations = int.Parse(ConfigurationManager.AppSettings["NumberOfDrawings"], formatProvider);

                GameDrawingService drawingService = new GameDrawingService(numberOfIterations);
                AutoResetEvent autoEvent = new AutoResetEvent(false);

                Log.Info(MethodBase.GetCurrentMethod().DeclaringType, "Start the number generation service.");
                ServiceTimer = new Timer(drawingService.ExecuteDrawing, autoEvent, TimeSpan.Zero, TimeSpan.FromMinutes(drawingService.DrawingTimeInterval));
                autoEvent.WaitOne();

                Console.WriteLine("\nDestroying timer.");
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

            HttpResponseMessage response = _httpClient.GetAsync(webApiMethod).Result;

            if (response.IsSuccessStatusCode)
            {
                GameSettings = response.Content.ReadAsAsync<LottoDemo.Entities.Models.LottoGameSettings>().Result;
                WriteMessage("Game settins were retrieved successfully from the CMS.");
                return true;
            }
            else
            {
                WriteMessage(new Exception("Error Code" + response.StatusCode + " : Message - " + response.ReasonPhrase));
                return false;
            }
        }

        private static void WriteMessage(string infoMessage)
        {
            Console.WriteLine(infoMessage);
            Log.Info(MethodBase.GetCurrentMethod().DeclaringType, infoMessage);
        }

        private static void WriteMessage(Exception ex)
        {
            Console.WriteLine(ex.Message);
            Log.Error(MethodBase.GetCurrentMethod().DeclaringType, ex.Message, ex);
        }
    }
}