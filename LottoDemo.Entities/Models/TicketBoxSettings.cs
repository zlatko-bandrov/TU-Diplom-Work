using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LottoDemo.Entities.Models
{
    public class TicketBoxSettings
    {
        public TicketBoxSettings(LottoGameSettings gameSettings = null)
        {
            if (gameSettings != null)
            {
                this.BallsCount = gameSettings.BallsCount;
                this.BonusBallsCount = gameSettings.BonusBallsCount;

                this.BonusBallMin = gameSettings.BonusBallMin;
                this.BonusBallMax = gameSettings.BonusBallMax;
                this.GameMaxNumber = gameSettings.GameMaxNumber;

                // Calculate rows
                if (gameSettings.GameMaxNumber > 0)
                {
                    double rows = (double)gameSettings.GameMaxNumber / this.Columns;
                    if (rows % 1 != 0)
                    {
                        rows += 1;
                    }
                    this.Rows = (byte)rows;
                }

                // Calculate bonus balls rows
                if (this.BonusBallMax > 0)
                {
                    double rows = (double)this.BonusBallMax / this.Columns;
                    if (rows % 1 != 0)
                    {
                        rows += 1;
                    }
                    this.BonusBallsRows = (byte)rows;
                }
            }
        }

        public byte Columns { get { return 10; } }
        public byte Rows { get; set; }

        public byte BallsCount { get; set; }
        public byte BonusBallsCount { get; set; }

        public byte GameMaxNumber { get; set; }
        public byte BonusBallsRows { get; set; }
        public byte BonusBallMin { get; set; }
        public byte BonusBallMax { get; set; }
    }
}
