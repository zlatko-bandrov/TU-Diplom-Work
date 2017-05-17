using LottoDemo.BusinessLogic.Games;
using LottoDemo.BusinessLogic.LotteryLogic;
using LottoDemo.Common.Services;
using LottoDemo.DataAccess;
using LottoDemo.Entities.Models;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace LottoDemo.BusinessLogic.Services
{
    public class LottoDrawService
    {
        #region Properties

        private int iterationIndex;

        private int MaxIterationsCount;

        private LotteryGameService GameService
        {
            get { return LotteryGameService.GetInstance(); }
        }

        public double DrawingTimeInterval
        {
            get { return GameSettings.DrawingTimeInterval; }
        }

        public Guid GameKey { get; set; }

        public LottoGameSettings GameSettings { get; set; }

        #endregion

        public LottoDrawService(LottoGameSettings gameSettings, Guid gameKey, int maxNumberOfIterations = -1)
        {
            this.MaxIterationsCount = maxNumberOfIterations;
            this.GameSettings = gameSettings;
            this.GameKey = gameKey;
        }


        public void ExecuteLottoDraw(Object stateInfo)
        {
            AutoResetEvent autoEvent = (AutoResetEvent)stateInfo;
            DateTime lottoDrawTime = DateTime.Now;
            this.iterationIndex++;

            var drawNumbersTask = Task.Run(() =>
            {
                // Generate lottery game drawing numbers
                List<byte> numbers = this.RunLottoDrawing();

                string infoMessage = string.Format("Drawing at {0}: {1}", lottoDrawTime.ToString("HH:mm:ss"), string.Join(", ", numbers));
                WriteLogMessage(infoMessage);

                // Create and save the lottery drawing in the database
                LottoDrawing lotteryDrawing = GameService.CreateNewLottoDraw(lottoDrawTime, numbers, GameKey);

                return lotteryDrawing;
            });
            drawNumbersTask.ContinueWith((antecedent) =>
            {
                LottoDrawing drawing = antecedent.Result;
                GameService.CalculateGameWinnings(drawing);
            },
            TaskContinuationOptions.OnlyOnRanToCompletion);

            try
            {
                drawNumbersTask.Wait();
                if (this.iterationIndex == this.MaxIterationsCount)
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
