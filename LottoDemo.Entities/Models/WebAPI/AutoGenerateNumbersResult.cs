using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LottoDemo.Entities.Models.WebAPI
{
    public class AutoGenerateNumbersResult
    {
        public AutoGenerateNumbersResult()
        {
            this.BallsList = new List<byte>();
            this.BonusBallsList = new List<byte>();
        }

        [JsonProperty("ballsList")]
        public IEnumerable<byte> BallsList { get; set; }

        [JsonProperty("bonusBallsList")]
        public IEnumerable<byte> BonusBallsList { get; set; }
    }
}
