using LottoDemo.WebApp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.Mvc;

namespace LottoDemo.WebApp.Controllers
{
    public class LotteryGamesListController : SurfaceController
    {
        public ActionResult RenderLotteryList()
        {
            try
            {
                var settingItem = ItemHelper.SettingsItem;
                var listPageId = settingItem.GetValue<int>("lotteryListPage");
                var model = this.Umbraco.TypedContent(listPageId);

                this.ViewBag.LastestJackpot = 1000000;
                this.ViewBag.NextDraw = DateTime.Now.AddDays(5).ToLongTimeString();

                return PartialView("Games/_LotteryGamesList", model);
            }
            catch (Exception ex)
            {
                Logger.Error(typeof(LotteryGamesListController), ex.Message, ex);
            }

            return new EmptyResult();
        }
    }
}