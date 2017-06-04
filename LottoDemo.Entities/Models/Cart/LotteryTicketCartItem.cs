using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LottoDemo.Entities.Models.Cart
{
    public class LotteryTicketCartItem
    {
        public List<byte> BallsNumbers { get; set; }
        public List<byte> BonusBallsNumbers { get; set; }
        public DateTime GameDrawTime { get; set; }
        public int LottoTicketId { get; set; }
        public int LottoDrawId { get; set; }
    }
}
