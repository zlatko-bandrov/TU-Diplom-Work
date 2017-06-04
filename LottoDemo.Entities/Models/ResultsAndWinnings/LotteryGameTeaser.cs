using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LottoDemo.Entities.Models.ResultsAndWinnings
{
    public class LotteryGameTeaser
    {
        public string GameLogoUrl { get; set; }
        public string GameUrl { get; set; }
        public string GameDisplayName { get; set; }
        public Guid GameKey { get; set; }
        public int GameDbId { get; set; }
        public DateTime NextDrawTime { get; set; }
        public DateTime PreviousDrawTime { get; set; }
        public double TicketPrice { get; set; }
        public decimal EstimatedJackpot { get; set; }

        public LottoDrawModel LastDraw { get; set; }
    }
}
