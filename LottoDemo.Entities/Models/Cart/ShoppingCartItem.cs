using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LottoDemo.Entities.Models.Cart
{
    public class ShoppingCartItem : LotteryGameCartItem
    {
        public string LotteryGameUrl { get; set; }
        public int LottoGameId { get; set; }
        public int TicketsCount { get; set; }
    }
}
