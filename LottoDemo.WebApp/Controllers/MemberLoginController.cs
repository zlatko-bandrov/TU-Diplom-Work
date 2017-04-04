using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.Mvc;
using LottoDemo.WebApp.Models.Membership;
using System.Web.Security;

namespace LottoDemo.WebApp.Controllers
{
    public class MemberLoginController : SurfaceController
    {
        public ActionResult RenderLoginDialog()
        {
            var model = new LoginFromViewModel();

            return PartialView("LoginForm/_LoginFormDialog", model);
        }

        public ActionResult RenderLogout()
        {
            return PartialView();
        }

        public ActionResult RenderMemberMenuActions()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginFromViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(model.MemberEmail, model.MemberPassword))
                {
                    FormsAuthentication.SetAuthCookie(model.MemberEmail, false);
                    UrlHelper urlHelper = new UrlHelper(HttpContext.Request.RequestContext);

                    return Redirect("/");
                }
                else
                {
                    ModelState.AddModelError("", "The username or password provided is incorrect.");
                }
            }

            return RedirectToCurrentUmbracoPage();
        }

        public ActionResult Logout()
        {
            Session.Clear();
            FormsAuthentication.SignOut();

            return RedirectToCurrentUmbracoPage();
        }
    }
}