using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Models;

namespace LottoDemo.Entities.Models
{
    public class LottoGameModel
    {
        public int Id { get; set; }
        public Guid GameKey { get; set; }
        public decimal Jackpot { get; set; }
        public CurrencyModel JackpotCurrency { get; set; }
        public DateTime NextDrawingDate { get; set; }

        public int DrawingTimeInterval { get; set; }
        public double TicketPrice { get; set; }
        public int DrawNumbersMaximum { get; set; }
        public int DrawBallsCount { get; set; }
        public int BonusBallsCount { get; set; }
        public int BonusBallMinimum { get; set; }
        public int BonusBallMaximum { get; set; }

        public void GetDataFromContent(IContent content)
        {

        }
    }
}
