﻿using LottoDemo.BusinessLogic.LotteryLogic;
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

        internal virtual LotteryGameSetting GameSettings { get; private set; }

        internal virtual GameDrawingUnitOfWork UnitOfWork { get; set; }

        public double DrawingTimeInterval
        {
            get { return this.GameSettings.DrawingTimeInterval; }
        }

        public GameDrawingService(int maxNumberOfIterations = -1)
        {
            this.maxIterationsCount = maxNumberOfIterations;
            this.UnitOfWork = new GameDrawingUnitOfWork();

            string gameName = ConfigurationManager.AppSettings["LottoGameName"];
            var lottoGame = this.UnitOfWork.LotteryGameRepository.GetByName(gameName);
            this.GameSettings = this.UnitOfWork.GameSettingsRepository.Get(settings => settings.LotteryGameID == lottoGame.ID);
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

        private List<byte> RunLottoDrawing()
        {
            byte number = 0;
            List<byte> draw = new List<byte>();
            NumbersGenerator generator = new NumbersGenerator();
            do
            {
                number = (byte)generator.Next(this.GameSettings.MinimalBallNumber, this.GameSettings.MaximalBallNumber);
                if (!draw.Contains(number) && number > 0)
                {
                    draw.Add(number);
                }
            }
            while (draw.Count < this.GameSettings.DrawingBallsCount);

            return draw;
        }

        private LottoDrawing CreateAndSaveDrawing(DateTime drawingDate, List<byte> numbers)
        {
            var drawingNumbers = DataHelper.CreateNewDrawingNumber(numbers);

            var newDrawing = new LottoDrawing()
            {
                LotteryGameID = this.GameSettings.LotteryGameID,
                DrawingNumber = drawingNumbers,
                IsCalculated = false,
                DrawTime = drawingDate,
                CreationDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };

            this.UnitOfWork.LottoDrawingRepository.Add(newDrawing);
            this.UnitOfWork.CommitChanges();

            return newDrawing;
        }

        private void CalculateGameWinnings(LottoDrawing drawing)
        {
            // TODO:
        }
    }
}