using LottoDemo.BusinessLogic.Games;
using LottoDemo.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.Mvc;

namespace LottoDemo.WebApp.Controllers
{
    public class GameDetailsController : SurfaceController
    {
        private LotteryGameService GameService = new LotteryGameService();

        public ActionResult RenderHeader()
        {
            try
            {
                LottoGameModel lottoGame = GetLottoGameModel();
                if (lottoGame != null)
                {
                    this.ViewBag.GameJackpot = lottoGame.Jackpot.ToString("0");
                    this.ViewBag.JackpotCurrencyCode = lottoGame.JackpotCurrency.Code;
                }

                return View("GameDetails/_GameDetailsHeader", this.CurrentPage);
            }
            catch (Exception ex)
            {
                this.Logger.Error(typeof(GameDetailsController), ex.Message, ex);
            }

            return new EmptyResult();
        }

        public ActionResult RenderTicketBoxes()
        {
            try
            {
                LottoGameModel lottoGame = GetLottoGameModel();

                return View("GameDetails/_LotteryTicketBoxes", this.CurrentPage);
            }
            catch (Exception ex)
            {
                this.Logger.Error(typeof(GameDetailsController), ex.Message, ex);
            }
            return new EmptyResult();
        }

        public ActionResult RenderCart()
        {
            try
            {

                return View("GameDetails/_LottoGameCart", this.CurrentPage);
            }
            catch (Exception ex)
            {
                this.Logger.Error(typeof(GameDetailsController), ex.Message, ex);
            }
            return new EmptyResult();
        }


        private LottoGameModel GetLottoGameModel()
        {
            Guid gameKey = Services.ContentService.GetById(CurrentPage.Id).Key;
            LottoGameModel lottoGame = this.GameService.GetLottoGameModelByKey(gameKey);

            return lottoGame;
        }
    }
}