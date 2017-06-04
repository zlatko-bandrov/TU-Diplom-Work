using LottoDemo.BusinessLogic.Games;
using LottoDemo.Entities.Models;
using LottoDemo.Entities.Models.ResultsAndWinnings;
using LottoDemo.WebApp.Helpers;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.Mvc;

namespace LottoDemo.WebApp.Controllers
{
    public class ResultsAndWinningsController : BaseController
    {
        private LotteryGameService GameService = new LotteryGameService();

        public ActionResult GamesResultsList()
        {
            try
            {
                int gameId = Session["CurrentGameID"] != null ? (int)Session["CurrentGameID"] : -1;
                if (gameId > 0)
                {
                    ViewBag.GameName = GetLottoGameName(Session["CurrentGameKey"] != null ? (Guid)Session["CurrentGameKey"] : Guid.Empty);
                    ViewBag.LottoDraws = GetAllDraws(gameId);
                    ViewBag.GameID = gameId;
                    return PartialView("GameResultsDetails", CurrentPage);
                }
                else
                {
                    var gamesList = GetGamesList();
                    ViewBag.GamesList = gamesList;
                }
            }
            catch (Exception ex)
            {
                WriteErrorMessage(ex);
            }
            return PartialView(CurrentPage);
        }

        [HttpPost]
        public ActionResult GameResultsDetails(int gameId, Guid gameKey)
        {
            try
            {
                Session["CurrentGameID"] = gameId;
                Session["CurrentGameKey"] = gameKey;
                RedirectToAction("GamesResultsList");
            }
            catch (Exception ex)
            {
                WriteErrorMessage(ex);
            }
            return CurrentUmbracoPage();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public PartialViewResult UpdateDrawResultView(UpdateDrawResultsModel model)
        {
            LottoDrawResult result = null;
            try
            {
                ViewBag.DrawTimeLabel = Umbraco.GetDictionaryValue("drawResultsFullDate");
                result = GetAllDraws(model.GameID, model.DrawID).FirstOrDefault();
            }
            catch (Exception ex)
            {
                WriteErrorMessage(ex);
            }
            return PartialView(result);
        }

        [HttpPost]
        public ActionResult BackToList()
        {
            Session.Remove("CurrentGameID");
            Session.Remove("CurrentGameKey");

            return RedirectToCurrentUmbracoPage();
        }

        private List<LottoDrawResult> GetAllDraws(int gameId, int drawId = -1)
        {
            var draws = GameService.GetAllCompletedDrawings(gameId, drawId);
            return draws;
        }

        private string GetLottoGameName(Guid gameKey)
        {
            string gameName = (string)Umbraco.TypedContent(gameKey).GetProperty("LotteryName").Value;
            return gameName;
        }

        private List<LotteryGameTeaser> GetGamesList()
        {
            List<LotteryGameTeaser> gameList = new List<LotteryGameTeaser>();

            var settingItem = ItemHelper.SettingsItem;
            var listPageId = settingItem.GetValue<int>("lotteryListPage");
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
                    PreviousDrawTime = GameService.GetLastCompletedDrawTime(gameModel.Id),
                    LastDraw = new LottoDrawModel()
                };

                var drawNumbers = GameService.GetLastDrawNumbers(gameModel.Id);
                gameTeaser.LastDraw.BallsList.AddRange(drawNumbers.Item1);
                gameTeaser.LastDraw.BonusBallsList.AddRange(drawNumbers.Item2);

                gameList.Add(gameTeaser);
            }

            return gameList;
        }
    }
}