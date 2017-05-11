using LottoDemo.DataAccess;
using LottoDemo.Entities.Models;
using LottoDemo.Repositories.UnitsOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LottoDemo.BusinessLogic.Extensions.LotteryGame;
using Umbraco.Core.Models;

namespace LottoDemo.BusinessLogic.Games
{
    public class LotteryGameService
    {
        private LotteryGameUnitOfWork GameUnitOfWork = new LotteryGameUnitOfWork();

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
    }
}