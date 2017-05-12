using LottoDemo.BusinessLogic.LotteryLogic;
using LottoDemo.Common.Services;
using LottoDemo.DataAccess;
using LottoDemo.Entities.Helpers;
using LottoDemo.Repositories.UnitsOfWork;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace LottoDemo.BusinessLogic.Services
{
    public class GameDrawingService
    {
        private int iterationIndex;

        private int maxIterationsCount;

        internal virtual LottoGameUnitOfWork UnitOfWork { get; set; }

        public double DrawingTimeInterval
        {
            // TODO: Fix the drawing time interval
            get { return 5; }
        }

        public GameDrawingService(int maxNumberOfIterations = -1)
        {
            this.maxIterationsCount = maxNumberOfIterations;
            this.UnitOfWork = new LottoGameUnitOfWork();

            string gameName = ConfigurationManager.AppSettings["LottoGameName"];
            //var lottoGame = this.UnitOfWork.LotteryGameRepository.GetByCMSID(gameName);
        }

        public void ExecuteDrawing(Object stateInfo)
        {
            AutoResetEvent autoEvent = (AutoResetEvent)stateInfo;
            DateTime lottoDrawTime = DateTime.Now;
            this.iterationIndex++;

            var drawNumbersTask = Task.Run(() =>
            {
                // Generate lottery game drawing numbers
                List<byte> numbers = this.RunLottoDrawing();
                string infoMessage = string.Format("Drawing at {0}: {1}", lottoDrawTime.ToString("HH:mm:ss"), string.Join(", ", numbers));

                // Write some messages in the log files and in the Console
                Log.Info(MethodBase.GetCurrentMethod().DeclaringType, infoMessage);
                Console.WriteLine(infoMessage);

                // Create and save the lottery drawing in the database
                LottoDrawing lotteryDrawing = this.CreateAndSaveDrawing(lottoDrawTime, numbers);

                return lotteryDrawing;
            });
            drawNumbersTask.ContinueWith((antecedent) =>
            {
                LottoDrawing drawing = antecedent.Result;
                this.CalculateGameWinnings(drawing);
            },
            TaskContinuationOptions.OnlyOnRanToCompletion);

            try
            {
                drawNumbersTask.Wait();
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

        public IEnumerable<byte> GenerateDrawNumbers(byte MinValue, byte MaxValue, byte numbersCount)
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
            while (draw.Count < numbersCount);

            return draw;
        }

        private List<byte> RunLottoDrawing()
        {
            byte number = 0;
            List<byte> draw = new List<byte>();
            NumbersGenerator generator = new NumbersGenerator();
            // TODO: Fix the lottery drawing service; And use the method GenerateDrawNumbers
            //do
            //{
            //    number = (byte)generator.Next(this.GameSettings.MinimalBallNumber, this.GameSettings.MaximalBallNumber);
            //    if (!draw.Contains(number) && number > 0)
            //    {
            //        draw.Add(number);
            //    }
            //}
            //while (draw.Count < this.GameSettings.DrawingBallsCount);

            return draw;
        }

        private LottoDrawing CreateAndSaveDrawing(DateTime drawingDate, List<byte> numbers)
        {
            //var drawingNumbers = DataHelper.CreateNewDrawingNumber(numbers);

            var newDrawing = new LottoDrawing()
            {
                // TODO: Fix Lottery game id
                //LotteryGameID = this.GameSettings.LotteryGameID,
                //DrawingNumber = drawingNumbers,
                IsCalculated = false,
                DrawTime = drawingDate,
                CreationDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };

            this.UnitOfWork.DrawRepository.Add(newDrawing);
            this.UnitOfWork.CommitChanges();

            return newDrawing;
        }

        private void CalculateGameWinnings(LottoDrawing drawing)
        {
            // TODO: Game winnings calculation
        }
    }
}
