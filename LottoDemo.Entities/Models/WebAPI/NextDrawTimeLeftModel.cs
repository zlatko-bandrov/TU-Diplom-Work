using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LottoDemo.Entities.Models.WebAPI
{
    public class NextDrawTimeLeftModel
    {
        [JsonProperty("nextDrawTimeTicks")]
        public string NextDrawTimeTicks { get; set; }

        [JsonProperty("redirectUrl")]
        public string RedirectUrl { get; set; }
    }
}
