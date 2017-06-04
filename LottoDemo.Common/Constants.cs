using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LottoDemo.Common
{
    public class Constants
    {
        public static readonly string SettingsItemId = ConfigurationManager.AppSettings["SettingsItemID"];

        public static readonly string DefaultGameJackpot = ConfigurationManager.AppSettings["InitialLotteryJackpot"];

        public static readonly int DefaultDrawInterval = 10;
    }
}
