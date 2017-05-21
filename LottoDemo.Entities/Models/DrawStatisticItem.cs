using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LottoDemo.Entities.Models
{
    public class DrawStatisticItem
    {
        public WinningTier WinningTier { get; set; }
        public List<LotteryTicket> Tickets { get; set; }
    }
}
