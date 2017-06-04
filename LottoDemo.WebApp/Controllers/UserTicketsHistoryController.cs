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
            var result = new List<UserGameHistory>();

            return result;
        }
    }
}