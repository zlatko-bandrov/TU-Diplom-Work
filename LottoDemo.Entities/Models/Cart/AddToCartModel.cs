using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace LottoDemo.Entities.Models.Cart
{
    public class AddToCartModel
    {
        public int LotteryGameId { get; set; }

        public IEnumerable<byte> DrawBallsTicket1 { get; set; }
        public IEnumerable<byte> DrawBallsTicket2 { get; set; }
        public IEnumerable<byte> DrawBallsTicket3 { get; set; }

        public IEnumerable<byte> BonusBallsTicket1 { get; set; }
        public IEnumerable<byte> BonusBallsTicket2 { get; set; }
        public IEnumerable<byte> BonusBallsTicket3 { get; set; }

        public void LoadValues(NameValueCollection postedForm)
        {
            string fieldValue = postedForm["DrawBallsTicket1"];
            this.DrawBallsTicket1 = JsonConvert.DeserializeObject<IEnumerable<byte>>(fieldValue);
            if (this.DrawBallsTicket1 == null)
            {
                this.DrawBallsTicket1 = new List<byte>();
            }

            fieldValue = postedForm["DrawBallsTicket2"];
            this.DrawBallsTicket2 = JsonConvert.DeserializeObject<IEnumerable<byte>>(fieldValue);
            if (this.DrawBallsTicket2 == null)
            {
                this.DrawBallsTicket2 = new List<byte>();
            }

            fieldValue = postedForm["DrawBallsTicket3"];
            this.DrawBallsTicket3 = JsonConvert.DeserializeObject<IEnumerable<byte>>(fieldValue);
            if (this.DrawBallsTicket3 == null)
            {
                this.DrawBallsTicket3 = new List<byte>();
            }

            fieldValue = postedForm["BonusBallsTicket1"];
            this.BonusBallsTicket1 = JsonConvert.DeserializeObject<IEnumerable<byte>>(fieldValue);
            if (this.BonusBallsTicket1 == null)
            {
                this.BonusBallsTicket1 = new List<byte>();
            }

            fieldValue = postedForm["BonusBallsTicket2"];
            this.BonusBallsTicket2 = JsonConvert.DeserializeObject<IEnumerable<byte>>(fieldValue);
            if (this.BonusBallsTicket2 == null)
            {
                this.BonusBallsTicket2 = new List<byte>();
            }

            fieldValue = postedForm["BonusBallsTicket3"];
            this.BonusBallsTicket3 = JsonConvert.DeserializeObject<IEnumerable<byte>>(fieldValue);
            if (this.BonusBallsTicket3 == null)
            {
                this.BonusBallsTicket3 = new List<byte>();
            }
        }
    }
}
