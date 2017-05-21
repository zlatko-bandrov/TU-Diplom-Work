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
                LottoDrawService drawingService = new LottoDrawService();
                if (!drawingService.Start())
                {
                    Console.ReadLine();
                    return;
                }

                // Get current game last draw time
                LottoDrawService.WriteLogMessage(string.Format("Start the number generation service for '{0}'.\n", LotteryGameName));
                TimeSpan delayTime = drawingService.NextDrawExecution - DateTime.Now;

                LottoDrawService.WriteLogMessage(string.Format("Next draw is at: {0}...\n", drawingService.NextDrawExecution.ToString("dd/MMM/yyyy HH:mm:ss")));

                AutoResetEvent autoEvent = new AutoResetEvent(false);
                ServiceTimer = new Timer(drawingService.ExecuteLottoDraw, autoEvent, delayTime, TimeSpan.FromMinutes(drawingService.DrawingTimeInterval));
                autoEvent.WaitOne();

                LottoDrawService.WriteLogMessage("\nDestroying timer.");
                ServiceTimer.Dispose();

                Console.ReadLine();
            }
            catch (Exception ex)
            {
                LottoDrawService.WriteLogMessage(ex);
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