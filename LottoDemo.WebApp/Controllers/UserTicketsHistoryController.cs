using LottoDemo.BusinessLogic.Services;
using LottoDemo.Entities.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LottoDemo.WebApp.Controllers
{
    public class UserTicketsHistoryController : BaseController
    {
        private readonly LottoUserService UserService = new LottoUserService();

        public ActionResult UserTicketsList()
        {
            try
            {
                ViewBag.UserHistory = GetUserHistory();
            }
            catch (Exception ex)
            {
                Logger.Error(typeof(MemberLoginController), ex.ToString(), ex);
            }
            return View(CurrentPage);
        }

        private List<UserGameHistory> GetUserHistory()
        {
            var result = UserService.GetUserHistory(User.Identity.Name);
            foreach (var game in result)
            {
                var content = Services.ContentService.GetById(game.GameKey);
                decimal ticketPrice = content.GetValue<decimal>("TicketPrice");
                game.GameName = content.GetValue<string>("LotteryName");
                foreach (var ticket in game.Tickets)
                {
                    if (ticket.ProfitLost == 0)
                    {
                        ticket.ProfitLost = (-1) * ticketPrice;
                    }
                }
            }
            return result;
        }
    }
}