using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LottoDemo.Entities.Models
{
    public class LotteryTicket
    {
        public LotteryTicket(List<byte> ballNumbers, List<byte> bonusBallNumbers)
        {
            this.BallNumbers = ballNumbers != null ? ballNumbers : new List<byte>();
            this.BonnusBallNumbers = bonusBallNumbers != null ? bonusBallNumbers : new List<byte>();
            this.IsWinning = false;
        }

        public int ID { get; set; }
        public bool IsWinning { get; set; }
        public List<byte> BallNumbers { get; protected set; }
        public List<byte> BonnusBallNumbers { get; protected set; }
    }
}
