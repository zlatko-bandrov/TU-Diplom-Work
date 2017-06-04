using System;
using System.Collections.Generic;

namespace LottoDemo.Entities.Models.User
{
    public class UserTicketHistory
    {
        public UserTicketHistory()
        {
            BallNumbers = new List<byte>();
            BonusBallNumbers = new List<byte>();
        }

        public List<byte> BallNumbers { get; set; }
        public List<byte> BonusBallNumbers { get; set; }
        public bool IsWinning { get; set; }
        public decimal ProfitLost { get; set; }
        public DateTime DrawTime { get; set; }
    }
}
