using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LottoDemo.Entities.Models.ResultsAndWinnings
{
    public class DrawWinningTier
    {
        public string TierName { get; set; }
        public decimal WinningPerPerson { get; set; }
        public int NumberOfWinners { get; set; }
    }
}
