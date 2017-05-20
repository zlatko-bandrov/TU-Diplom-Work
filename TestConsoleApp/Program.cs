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

        private static readonly string _lotteryGameName = ConfigurationManager.AppSettings["LottoGameName"];
        public static string LotteryGameName
        {
            get { return _lotteryGameName; }
        }

        static void Main(string[] args)
        {
            try
            {
                LottoDrawService drawingService = new LottoDrawService();
                if (!drawingService.InitGameSettings())
                {
                    Console.ReadLine();
                    return;
                }

                // Get current game last draw time

                LottoDrawService.WriteLogMessage(string.Format("Start the number generation service for '{0}'.\n", LotteryGameName));

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
            }
        }

        public void Dispose()
        {
            if (ServiceTimer != null)
            {
                ServiceTimer.Dispose();
            }
        }
    }
}