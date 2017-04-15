using LottoDemo.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace LottoDemo.WebApp.Helpers
{
    public class ItemHelper
    {
        public static int SettingsItemID
        {
            get
            {
                int temp = 0;
                int.TryParse(Common.Constants.SettingsItemId, out temp);

                return temp;
            }
        }

        public static int DefaultGameJackpot()
        {
            int temp = 0;
            if (int.TryParse(Common.Constants.SettingsItemId, out temp))
            {
                return temp;
            }

            return 1000000;
        }

        public static IContent SettingsItem
        {
            get
            {
                var settingsItem = ApplicationContext.Current.Services.ContentService.GetById(SettingsItemID);
                return settingsItem;
            }
        }
    }
}