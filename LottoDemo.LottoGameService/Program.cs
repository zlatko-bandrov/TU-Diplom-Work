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

        public static string LotteryGameName
        {
            get { return ConfigurationManager.AppSettings["LottoGameName"]; }
        }

        static void Main(string[] args)
        {
            try
            {
                Console.Title = string.Format("Lottery Game Service - {0}", LotteryGameName);
                LottoDrawService gameDrawService = new LottoDrawService();
                if (!gameDrawService.Start())
                {
                    LottoDrawService.WriteLogMessage("There was a problem launching the service. Check the log files for more information.\n");
                    Console.ReadLine();
                    return;
                }

                string message = string.Format("Starting the game service for '{0}'.\n", LotteryGameName);
                LottoDrawService.WriteLogMessage(message);

                TimeSpan timeInterval = gameDrawService.GetDrawInterval();
                AutoResetEvent autoEvent = new AutoResetEvent(false);
                ServiceTimer = new Timer(gameDrawService.RunGameDrawing, autoEvent, timeInterval, timeInterval);
                autoEvent.WaitOne();

                LottoDrawService.WriteLogMessage("\nDestroying timer..");
                ServiceTimer.Dispose();
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                LottoDrawService.WriteLogMessage(ex);
                Console.ReadLine();
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