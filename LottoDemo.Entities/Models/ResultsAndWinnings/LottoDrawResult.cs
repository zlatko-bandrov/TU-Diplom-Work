using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LottoDemo.Entities.Models.ResultsAndWinnings
{
    public class LottoDrawResult
    {
        public int DrawID { get; set; }
        public DateTime DrawDate { get; set; }
        public List<byte> BallsBumbers { get; set; }
        public List<byte> BonusBallsNumbers { get; set; }
        public List<DrawWinningTier> WinningTiers { get; set; }
    }
}
