using System.Collections.Generic;

namespace LottoDemo.Entities.Models.User
{
    public class UserGameHistory
    {
        public UserGameHistory()
        {
            Tickets = new List<UserTicketHistory>();
        }

        public string GameName { get; set; }
        public decimal Jackpot { get; set; }
        public List<UserTicketHistory> Tickets { get; set; }
    }
}
