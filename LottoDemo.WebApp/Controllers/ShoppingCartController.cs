using LottoDemo.BusinessLogic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.Mvc;

namespace LottoDemo.WebApp.Controllers
{
    public class ShoppingCartController : SurfaceController
    {
        public LottoUserService UserService = new LottoUserService();

        public ActionResult RenderMainMenu()
        {
            decimal userBalance = 0;
            decimal totalCartValue = 0;

            var shoppingCartItems = UserService.GetAllCartTickets(User.Identity.Name, out userBalance, out totalCartValue);

            ViewBag.ShoppingCartItems = shoppingCartItems;
            ViewBag.UserBalance = string.Format("{0:0,0.00}", userBalance).Replace(",", " ");
            ViewBag.TotalCartPrice = string.Format("{0:0,0.00}", totalCartValue).Replace(",", " ");

            return View("MainMenu", this.CurrentPage);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteAllTickets(int lottoGameId)
        {
            try
            {
                UserService.DeleteAllTicketsByGame(System.Web.HttpContext.Current.User.Identity.Name, lottoGameId);

                return RedirectToCurrentUmbracoPage();
            }
            catch (Exception ex)
            {
                this.WriteErrorMessage(ex);
            }
            return new EmptyResult();
        }

        private void WriteErrorMessage(Exception ex)
        {
            this.Logger.Error(typeof(GameDetailsController), ex.Message, ex);
        }
    }
}