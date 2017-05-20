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
        public LottoGameModel(IPublishedContent contentItem = null, DateTime? previousDraw = null, DateTime? nextDraw = null)
        {
            if (contentItem != null)
            {
                this.Jackpot = decimal.Parse(contentItem.GetProperty("Jackpot").Value.ToString());
                this.LottoGameName = contentItem.Name;
                this.LottoGameUrl = contentItem.Url;
            }
            this.GameSettings = new LottoGameSettings(contentItem, previousDraw, nextDraw);
            this.TicketBoxSettings = new TicketBoxSettings(this.GameSettings);

            this.PreviousDrawingTime = previousDraw.HasValue ? previousDraw.Value : DateTime.MinValue;
            this.NextDrawingTime = nextDraw.HasValue ? nextDraw.Value : DateTime.MaxValue;
        }

        public int Id { get; set; }
        public Guid GameKey { get; set; }
        public decimal Jackpot { get; set; }
        public CurrencyModel JackpotCurrency { get; set; }

        public DateTime NextDrawingTime { get; set; }
        public DateTime PreviousDrawingTime { get; set; }

        public TicketBoxSettings TicketBoxSettings { get; set; }
        public LottoGameSettings GameSettings { get; set; }

        public string LottoGameName { get; set; }
        public string LottoGameUrl { get; set; }
    }
}
