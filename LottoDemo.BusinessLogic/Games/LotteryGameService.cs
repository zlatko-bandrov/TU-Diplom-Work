using LottoDemo.DataAccess;
using LottoDemo.Entities.Models;
using LottoDemo.Repositories.UnitsOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LottoDemo.BusinessLogic.Extensions.LotteryGame;

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

        public LottoGameModel GetLottoGameModelByKey(Guid gameUniqueId)
        {
            var lottoGame = this.GetLotteryGameByKey(gameUniqueId);
            return lottoGame != null ? lottoGame.ToLottoGameModel() : null;
        }

        public decimal GetGameJackpot(Guid gameUniqueId)
        {
            var lottoGame = this.GetLotteryGameByKey(gameUniqueId);

            return lottoGame != null ? lottoGame.Jackpot.Balance.Value : 0;
        }
    }
}