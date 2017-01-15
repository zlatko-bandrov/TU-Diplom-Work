using LottoDemo.DataAccess;
using LottoDemo.Repositories.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LottoDemo.Repositories.Repositories
{
    public class LottoDrawingRepository : GenericRepository<LottoDrawing>
    {
        public LottoDrawingRepository(LotteryDemoDBEntities context)
            : base(context)
        {
        }
    }
}
