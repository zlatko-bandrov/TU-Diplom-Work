using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LottoDemo.Entities.Models
{
    public class LottoDrawModel
    {
        public LottoDrawModel()
        {
            BallsList = new List<byte>();
            BonusBallsList = new List<byte>();
        }

        public int ID { get; set; }
        public List<byte> BallsList { get; set; }
        public List<byte> BonusBallsList { get; set; }
    }
}
