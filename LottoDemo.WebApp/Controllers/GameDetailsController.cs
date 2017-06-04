using LottoDemo.BusinessLogic.Games;
using LottoDemo.BusinessLogic.Services;
using LottoDemo.Entities.Models;
using LottoDemo.Entities.Models.Cart;
using System;
using System.Linq;
using System.Web.Mvc;
using Umbraco.Web.Mvc;

namespace LottoDemo.WebApp.Controllers
{
    public class GameDetailsController : SurfaceController
    {
        public LotteryGameService GameService = new LotteryGameService();

        public LottoUserService LottoUserService = new LottoUserService();

        public ActionResult RenderHeader()
        {
            try
            {
                LottoGameModel lottoGame = this.GetLottoGameModel();
                if (lottoGame != null)
                {
                    ViewBag.TicketBoxSettings = lottoGame.TicketBoxSettings;
                    ViewBag.GameSettings = lottoGame.GameSettings;
                    ViewBag.JackpotCurrencyCode = "EUR";
                    ViewBag.NextDrawTime = lottoGame.NextDrawingTime;
                    ViewBag.NextDrawTimeLeft = lottoGame.NextDrawingTime - DateTime.Now;
                    ViewBag.GameJackpot = lottoGame.Jackpot.ToString("0,0");
                }
                return PartialView("GameDetails/_GameDetailsHeader", this.CurrentPage);
            }
            catch (Exception ex)
            {
                WriteErrorMessage(ex);
            }
            return CurrentUmbracoPage();
        }

        public ActionResult RenderTicketBoxes()
        {
            try
            {
                LottoGameModel lottoGame = GetLottoGameModel();
                if (lottoGame != null)
                {
                    ViewBag.TicketBoxSettings = lottoGame.TicketBoxSettings;
                    ViewBag.GameSettings = lottoGame.GameSettings;
                }
                return PartialView("GameDetails/_LotteryTicketBoxes", this.CurrentPage);
            }
            catch (Exception ex)
            {
                WriteErrorMessage(ex);
            }
            return CurrentUmbracoPage();
        }

        public ActionResult RenderCart()
        {
            try
            {
                LottoGameModel lottoGame = GetLottoGameModel();
                if (lottoGame != null)
                {
                    var shoppingCartItems = LottoUserService.GetAllCartTickets(User.Identity.Name, lottoGame, (string)CurrentPage.GetProperty("LotteryName").Value);
                    double totalPrice = 0;

                    var cartItem = shoppingCartItems.FirstOrDefault();
                    if (cartItem != null)
                    {
                        totalPrice = cartItem.TicketPrice * cartItem.Tickets.Count;
                    }

                    int? nextDrawId = GameService.TakeTheNextDrawID(lottoGame.Id);
                    ViewBag.LottoDrawId = nextDrawId.HasValue ? nextDrawId.Value : 0;
                    ViewBag.ShoppinCartItems = shoppingCartItems;
                    ViewBag.CartTotalPrice = totalPrice;
                }

                return PartialView("GameDetails/_LottoGameCart", CurrentPage);
            }
            catch (Exception ex)
            {
                WriteErrorMessage(ex);
            }
            return CurrentUmbracoPage();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddToCart(AddToCartModel model)
        {
            try
            {
                model.LoadValues(Request.Form);
                LottoUserService.AddNewLotteryTickets(this.User.Identity.Name, model);
            }
            catch (Exception ex)
            {
                WriteErrorMessage(ex);
            }
            return CurrentUmbracoPage();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteAllTickets()
        {
            try
            {
                LottoGameModel lottoGame = this.GetLottoGameModel();
                if (lottoGame != null && System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    LottoUserService.DeleteAllTicketsByGame(System.Web.HttpContext.Current.User.Identity.Name, lottoGame.Id);
                }
            }
            catch (Exception ex)
            {
                this.WriteErrorMessage(ex);
            }
            return CurrentUmbracoPage();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteSingleTicket(int lotteryTicketId)
        {
            try
            {
                if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    LottoUserService.DeleteSingleTicketByGame(System.Web.HttpContext.Current.User.Identity.Name, lotteryTicketId);
                }
            }
            catch (Exception ex)
            {
                this.WriteErrorMessage(ex);
            }
            return CurrentUmbracoPage();
        }

        private LottoGameModel GetLottoGameModel()
        {
            var contentData = Services.ContentService.GetById(CurrentPage.Id);
            LottoGameModel lottoGame = GameService.GetLottoGameModelByKey(contentData.Key, CurrentPage);
            lottoGame.NextDrawingTime = GameService.GetGameNextDrawTime(lottoGame.Id, TimeSpan.FromMinutes(lottoGame.GameSettings.DrawingTimeInterval));
            lottoGame.PreviousDrawingTime = GameService.GetLastCompletedDrawTime(contentData.Key);

            return lottoGame;
        }

        private void WriteErrorMessage(Exception ex)
        {
            this.Logger.Error(typeof(GameDetailsController), ex.Message, ex);
        }
    }
}