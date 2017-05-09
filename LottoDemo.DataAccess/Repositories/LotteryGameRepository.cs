using LottoDemo.DataAccess;
using LottoDemo.Repositories.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LottoDemo.Repositories.Repositories
{
    public class LotteryGameRepository : GenericRepository<LotteryGame>
    {
        public LotteryGameRepository(LotteryDemoDBEntities context)
            : base(context)
        {
        }

        public LotteryGame GetByCMSID(Guid gameCMSId)
        {
            return this.Get(g => g.CMSID.Equals(gameCMSId));
        }
    }
}