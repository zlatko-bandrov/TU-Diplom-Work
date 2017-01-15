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
                double intervalInMinutes = double.Parse(ConfigurationManager.AppSettings["TimeIntervalInMinutes"], formatProvider);
                int numberOfIterations = int.Parse(ConfigurationManager.AppSettings["NumberOfDrawings"], formatProvider);

                NumberGenerationService drawingService = new NumberGenerationService(numberOfIterations);
                AutoResetEvent autoEvent = new AutoResetEvent(false);

                Log.Info(MethodBase.GetCurrentMethod().DeclaringType, "Start the number generation service.");
                ServiceTimer = new Timer(drawingService.ExecuteDrawing, autoEvent, TimeSpan.Zero, TimeSpan.FromMinutes(intervalInMinutes));
                autoEvent.WaitOne();

                Console.WriteLine("\nDestroying timer.");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Log.Error(MethodBase.GetCurrentMethod().DeclaringType, ex.Message, ex);
            }
            finally
            {
                ServiceTimer.Dispose();
            }
        }

        public void Dispose()
        {
            ServiceTimer.Dispose();
        }
    }

    public class NumberGenerationService
    {
        private int iterationIndex;
        private int maxIterationsCount;

        public NumberGenerationService(int maxNumberOfIterations = -1)
        {
            this.iterationIndex = 0;
            this.maxIterationsCount = maxNumberOfIterations;
        }

        public void ExecuteDrawing(Object stateInfo)
        {
            AutoResetEvent autoEvent = (AutoResetEvent)stateInfo;
            DateTime currentTime = DateTime.Now;
            this.iterationIndex++;

            var drawNumbersTask = Task.Run(() =>
            {
                List<int> numbers = this.DrawNumbers();
                return numbers;
            });
            var calculateWinningsTask = drawNumbersTask.ContinueWith((antecedent) =>
            {
                string infoMessage = string.Format("Drawing at {0}: {1}", currentTime.ToString("HH:mm:ss"), string.Join(", ", antecedent.Result));
                Console.WriteLine(infoMessage);
                Log.Info(MethodBase.GetCurrentMethod().DeclaringType, infoMessage);
            },
            TaskContinuationOptions.OnlyOnRanToCompletion);

            try
            {
                drawNumbersTask.Wait();
                calculateWinningsTask.Wait();

                if (this.iterationIndex == this.maxIterationsCount)
                {
                    // Reset the counter and signal the waiting thread.
                    this.iterationIndex = 0;
                    autoEvent.Set();
                }
            }
            catch (Exception error)
            {
                Log.Error(MethodBase.GetCurrentMethod().DeclaringType, error.Message, error);
                this.iterationIndex = 0;
                autoEvent.Set();
            }
        }

        private List<int> DrawNumbers()
        {
            int number = -1;
            List<int> draw = new List<int>();
            NumbersGenerator generator = new NumbersGenerator();
            do
            {
                number = generator.Next(1, 49);
                if (!draw.Contains(number) && number > 0)
                {
                    draw.Add(number);
                }
            }
            while (draw.Count < 6);

            return draw;
        }
    }
}