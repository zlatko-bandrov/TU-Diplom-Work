using LottoDemo.BusinessLogic.Games;
using LottoDemo.Entities.Models;
using LottoDemo.Entities.Models.ResultsAndWinnings;
using LottoDemo.WebApp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Core.Models;
using Umbraco.Web.Mvc;

namespace LottoDemo.WebApp.Controllers
{
    public class LotteryGamesListController : SurfaceController
    {
        protected LotteryGameService GameService = new LotteryGameService();

        public ActionResult RenderLotteryList()
        {
            IPublishedContent model = null;
            try
            {
                model = this.Umbraco.TypedContent(ItemHelper.SettingsItem.GetValue<int>("lotteryListPage"));
                ViewBag.GamesList = GetGamesList();
            }
            catch (Exception ex)
            {
                Logger.Error(typeof(LotteryGamesListController), ex.Message, ex);
            }

            return PartialView("Games/_LotteryGamesList", model);
        }


        private List<LotteryGameTeaser> GetGamesList()
        {
            List<LotteryGameTeaser> gameList = new List<LotteryGameTeaser>();
            var listPageId = ItemHelper.SettingsItem.GetValue<int>("lotteryListPage");
            var listPage = Umbraco.TypedContent(listPageId);

            foreach (var game in listPage.Children)
            {
                var content = Umbraco.TypedContent(game.Id);
                Guid gameKey = Services.ContentService.GetById(game.Id).Key;
                LottoGameModel gameModel = GameService.GetLottoGameModelByKey(gameKey, content);

                var mediaUrl = Umbraco.Media((string)content.GetProperty("LotteryLogo").Value).umbracoFile.src;
                var gameTeaser = new LotteryGameTeaser
                {
                    GameKey = gameKey,
                    GameDbId = gameModel.Id,
                    EstimatedJackpot = gameModel.Jackpot,
                    GameDisplayName = gameModel.LottoGameName,
                    GameUrl = content.Url,
                    GameLogoUrl = mediaUrl,
                    TicketPrice = gameModel.GameSettings.TicketPrice,
                    NextDrawTime = GameService.GetGameNextDrawTime(gameModel.Id, TimeSpan.FromMinutes(gameModel.GameSettings.DrawingTimeInterval))
                };
                gameList.Add(gameTeaser);
            }

            return gameList;
        }
    }
}