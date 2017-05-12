using LottoDemo.BusinessLogic.Games;
using LottoDemo.BusinessLogic.Services;
using LottoDemo.Entities.Models;
using LottoDemo.Entities.Models.Cart;
using System;
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
                this.WriteErrorMessage(ex);
            }

            return new EmptyResult();
        }

        public ActionResult RenderTicketBoxes()
        {
            try
            {
                LottoGameModel lottoGame = this.GetLottoGameModel(true);
                if (lottoGame != null)
                {
                    this.ViewBag.TicketBoxSettings = lottoGame.TicketBoxSettings;
                    this.ViewBag.GameSettings = lottoGame.GameSettings;
                }

                return View("GameDetails/_LotteryTicketBoxes", this.CurrentPage);
            }
            catch (Exception ex)
            {
                this.WriteErrorMessage(ex);
            }
            return new EmptyResult();
        }

        public ActionResult RenderCart()
        {
            try
            {
                LottoGameModel lottoGame = this.GetLottoGameModel();
                if (lottoGame != null)
                {
                    ViewBag.LotteryGameId = lottoGame.Id;
                }

                return View("GameDetails/_LottoGameCart", this.CurrentPage);
            }
            catch (Exception ex)
            {
                this.WriteErrorMessage(ex);
            }
            return new EmptyResult();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddToCart(AddToCartModel model)
        {
            try
            {
                model.LoadValues(Request.Form);

                var gamblerService = new GamblerService();
                gamblerService.AddNewLotteryTickets(this.User.Identity.Name, model);

                return RedirectToCurrentUmbracoPage();
            }
            catch (Exception ex)
            {
                this.WriteErrorMessage(ex);
            }

            return new EmptyResult();
        }

        private LottoGameModel GetLottoGameModel(bool loadDataFromContent = false)
        {
            var contentData = Services.ContentService.GetById(this.CurrentPage.Id);
            LottoGameModel lottoGame = this.GameService.GetLottoGameModelByKey(contentData.Key, this.CurrentPage);

            return lottoGame;
        }

        private void WriteErrorMessage(Exception ex)
        {
            this.Logger.Error(typeof(GameDetailsController), ex.Message, ex);
        }
    }
}