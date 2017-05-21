using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LottoDemo.Entities.Models
{
    public class LottoDrawStatistic
    {
        public LottoDrawStatistic()
        {
            Table = new Dictionary<Tuple<byte, byte>, DrawStatisticItem>();
        }
        public Dictionary<Tuple<byte, byte>, DrawStatisticItem> Table { get; protected set; }
    }
}
