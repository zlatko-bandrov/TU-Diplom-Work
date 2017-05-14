using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LottoDemo.Entities.Models.Cart
{
    public class LotteryGameCartItem
    {
        public LotteryGameCartItem()
        {
            this.Tickets = new List<LotteryTicketCartItem>();
        }

        public string LottoGameName { get; set; }
        public double TicketPrice { get; set; }
        public List<LotteryTicketCartItem> Tickets { get; set; }
    }
}
