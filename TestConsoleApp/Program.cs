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

namespace TestConsoleApp
{
    class Program : IDisposable
    {
        private static Timer ServiceTimer;

        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("{0:h:mm:ss.fff} Creating timer.\n", DateTime.Now);

                System.Globalization.CultureInfo formatProvider = System.Globalization.CultureInfo.InvariantCulture;
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