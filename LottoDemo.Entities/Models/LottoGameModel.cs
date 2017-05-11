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
        public LottoGameModel(IPublishedContent contentItem = null)
        {
            this.GameSettings = new LottoGameSettings(contentItem);
            this.TicketBoxSettings = new TicketBoxSettings(this.GameSettings);
            this.NextDrawingDate = DateTime.Now.AddMinutes(15);
        }

        public int Id { get; set; }
        public Guid GameKey { get; set; }
        public decimal Jackpot { get; set; }
        public CurrencyModel JackpotCurrency { get; set; }
        public DateTime NextDrawingDate { get; set; }

        public TicketBoxSettings TicketBoxSettings { get; set; }
        public LottoGameSettings GameSettings { get; set; }
    }
}
