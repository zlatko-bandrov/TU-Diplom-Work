using LottoDemo.BusinessLogic.Extensions.LotteryGame;
using LottoDemo.BusinessLogic.Services;
using LottoDemo.DataAccess;
using LottoDemo.Entities.Models;
using LottoDemo.Entities.Models.Cart;
using LottoDemo.Repositories.UnitsOfWork;
using System;
using System.Linq;
using System.Collections.Generic;
using Umbraco.Core.Models;

namespace LottoDemo.BusinessLogic.Games
{
    public class LotteryGameService
    {
        public static LotteryGameService GetInstance()
        {
            return new LotteryGameService();
        }

        public LottoGameUnitOfWork GameUnitOfWork
        {
            get { return LottoGameUnitOfWork.GetInstance(); }
        }

        internal LotteryGame GetLotteryGameByKey(Guid gameUniqueId)
        {
            var lottoGame = GameUnitOfWork.GameRepository.GetLottoGameByKey(gameUniqueId);
            return lottoGame;
        }

        public LottoGameModel GetLottoGameModelByKey(Guid gameUniqueId, IPublishedContent contentItem = null)
        {
            var lottoGame = this.GetLotteryGameByKey(gameUniqueId);
            return lottoGame != null ? lottoGame.ToLottoGameModel(contentItem) : null;
        }

        public decimal GetGameJackpot(Guid gameUniqueId)
        {
            var lottoGame = this.GetLotteryGameByKey(gameUniqueId);

            return lottoGame != null ? lottoGame.Jackpot.Balance.Value : 0;
        }


        public DateTime GetNextDrawTime()
        {
            return DateTime.Now.AddMinutes(15);
        }

        public DateTime GetLastCompleteDrawDate()
        {
            return DateTime.MinValue;
        }


        public LottoDrawing CreateNewLottoDraw(DateTime drawingDate, List<byte> ballNumbers, Guid lottoGameKey)
        {
            var lottoGame = this.GameUnitOfWork.GameRepository.Get(g => g.GameKey.Equals(lottoGameKey));

            LottoDrawing newDrawing = new LottoDrawing()
            {
                LotteryGameID = lottoGame.ID,
                IsCalculated = false,
                DrawTime = drawingDate,
                CreationDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };

            byte position = 1;
            foreach (byte ballNumber in ballNumbers)
            {
                var newLotteryBall = new LotteryBall
                {
                    BallNumber = ballNumber,
                    IsBonusBall = false,
                    Position = position++
                };

                var newLottoDrawBalls = new LottoDrawingBall
                {
                    LottoDrawing = newDrawing,
                    LotteryBall = newLotteryBall
                };

                GameUnitOfWork.BallsRepository.Add(newLotteryBall);
                GameUnitOfWork.DrawBallsRelationsRepository.Add(newLottoDrawBalls);
            }

            GameUnitOfWork.DrawRepository.Add(newDrawing);
            GameUnitOfWork.CommitChanges();

            return newDrawing;
        }

        public void CalculateGameWinnings(LottoDrawing drawing)
        {
            // TODO: Game winnings calculation
        }
    }
}