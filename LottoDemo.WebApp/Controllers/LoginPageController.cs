using LottoDemo.WebApp.Models.Membership;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace LottoDemo.WebApp.Controllers
{
    public class LoginPageController : BaseController
    {
        public ActionResult LoginView()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MemberLogin(LoginFromViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(model.MemberEmail, model.MemberPassword))
                {
                    if (Members.Login(model.MemberEmail, model.MemberPassword))
                    {
                        return RedirectToUmbracoPage(1050);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "The username or password provided is incorrect.");
                }
            }

            return RedirectToCurrentUmbracoPage();
        }
    }
}