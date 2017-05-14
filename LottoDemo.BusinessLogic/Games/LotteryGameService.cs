using LottoDemo.BusinessLogic.Extensions.LotteryGame;
using LottoDemo.BusinessLogic.Services;
using LottoDemo.DataAccess;
using LottoDemo.Entities.Models;
using LottoDemo.Entities.Models.Cart;
using LottoDemo.Repositories.UnitsOfWork;
using System;
using System.Collections.Generic;
using Umbraco.Core.Models;

namespace LottoDemo.BusinessLogic.Games
{
    public class LotteryGameService
    {
        private LottoGameUnitOfWork GameUnitOfWork = new LottoGameUnitOfWork();

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
    }
}