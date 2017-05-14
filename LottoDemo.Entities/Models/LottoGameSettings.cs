using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Models;

namespace LottoDemo.Entities.Models
{
    public class LottoGameSettings
    {
        public LottoGameSettings(IPublishedContent contentItem = null)
        {
            if (contentItem != null)
            {
                // Set game settings
                this.DrawingTimeInterval = byte.Parse(contentItem["DrawingTimeInterval"].ToString());
                this.TicketPrice = double.Parse(contentItem["TicketPrice"].ToString());
                this.GameMaxNumber = byte.Parse(contentItem["DrawNumbersMaximum"].ToString());
                this.BallsCount = byte.Parse(contentItem["DrawBallsCount"].ToString());
                this.BonusBallsCount = byte.Parse(contentItem["BonusBallsCount"].ToString());
                this.BonusBallMin = byte.Parse(contentItem["BonusBallMinimum"].ToString());
                this.BonusBallMax = byte.Parse(contentItem["BonusBallMaximum"].ToString());
            }
        }

        public byte DrawingTimeInterval { get; set; }
        public double TicketPrice { get; set; }
        public byte GameMaxNumber { get; set; }
        public byte BallsCount { get; set; }
        public byte BonusBallsCount { get; set; }
        public byte BonusBallMin { get; set; }
        public byte BonusBallMax { get; set; }
        
    }
}
