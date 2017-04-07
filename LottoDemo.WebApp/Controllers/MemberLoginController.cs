using LottoDemo.WebApp.Models.Membership;
using System.Web.Mvc;
using System.Web.Security;
using Umbraco.Web.Mvc;

namespace LottoDemo.WebApp.Controllers
{
    public class MemberLoginController : SurfaceController
    {
        private readonly string NewMemberGroupName = "Lottery Games";
        private readonly string NewMemberType = "Lotto Gamer";


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
        public ActionResult MemberRegistration(LoginFromViewModel model)
        {
            var memberService = this.Services.MemberService;

            if (memberService.Exists(model.MemberEmail))
            {
                var newMember = memberService.CreateWithIdentity(model.MemberEmail, model.MemberEmail, model.MemberPassword, this.NewMemberType);
                memberService.AssignRole(model.MemberEmail, this.NewMemberGroupName);

                // Set the new member data
                newMember.SetValue("firstName", model.FirstName);
                newMember.SetValue("lastName", model.LastName);
                newMember.SetValue("country", model.Country);
                newMember.SetValue("mobilePhone", model.MobilePhone);
                newMember.SetValue("personTitle", model.Title);

                memberService.Save(newMember);
            }
            else
            {
                //member exists
            }

            return RedirectToCurrentUmbracoPage();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MemberLogin(LoginFromViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(model.MemberEmail, model.MemberPassword))
                {
                    FormsAuthentication.SetAuthCookie(model.MemberEmail, false);
                    //UrlHelper urlHelper = new UrlHelper(HttpContext.Request.RequestContext);

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