using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LottoDemo.Entities.Models.WebAPI
{
    public class GameDrawSettings
    {
        [JsonProperty("ballsCount")]
        public byte DrawBallsCount { get; set; }

        [JsonProperty("bonusBallsCount")]
        public byte BonusBallsCount { get; set; }

        [JsonProperty("minBonusBallNumber")]
        public byte MinBonusBallNumber { get; set; }

        [JsonProperty("maxBonusBallNumber")]
        public byte MaxBonusBallNumber { get; set; }

        [JsonProperty("drawBallMaxNumber")]
        public byte DrawBallMaxNumber { get; set; }
    }
}
