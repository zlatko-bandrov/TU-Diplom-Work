using LottoDemo.DataAccess;
using LottoDemo.Repositories.Generic;
using System;

namespace LottoDemo.Repositories.Repositories
{
    public class LotteryGameRepository : GenericRepository<LotteryGame>
    {
        public LotteryGameRepository(LotteryDemoDBEntities context)
            : base(context)
        {
        }

        public LotteryGame GetLottoGameByKey(Guid gameUniqueId)
        {
            return this.Get(g => g.GameKey.Equals(gameUniqueId));
        }

        public LotteryGame GetLottoGameById(int lottoGameId)
        {
            return this.Get(g => g.ID == lottoGameId);
        }
    }
}