using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LottoDemo.Entities.Models
{
    public class WinningTier
    {
        public int ID { get; set; }
        public byte BallsCount { get; set; }
        public byte BonnusBallCount { get; set; }
        public double TierPercent { get; set; }
        public decimal WinningPerPerson { get; set; }
    }
}
