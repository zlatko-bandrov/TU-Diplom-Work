using LottoDemo.BusinessLogic.Services;
using LottoDemo.WebApp.Models.Membership;
using System;
using System.Web.Mvc;
using System.Web.Security;
using Umbraco.Web.Mvc;

namespace LottoDemo.WebApp.Controllers
{
    public class MemberLoginController : SurfaceController
    {
        private readonly string NewMemberGroupName = "Lottery Games";
        private readonly string NewMemberType = "lottoGamer";

        public ActionResult RenderLoginDialog()
        {
            var model = new LoginFromViewModel();

            return PartialView("LoginForm/_LoginFormDialog", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MemberRegistration(LoginFromViewModel model)
        {
            try
            {
                if (!this.ValidateNewMemberData(model))
                {
                    return RedirectToCurrentUmbracoPage();
                }

                var memberService = this.Services.MemberService;

                if (!memberService.Exists(model.MemberEmail))
                {
                    var newMember = memberService.CreateWithIdentity(model.MemberEmail, model.MemberEmail, model.MemberPassword, this.NewMemberType);
                    memberService.AssignRole(model.MemberEmail, this.NewMemberGroupName);

                    // Set the new member data
                    newMember.SetValue("firstName", model.FirstName);
                    newMember.SetValue("lastName", model.LastName);
                    newMember.SetValue("country", model.Country);
                    newMember.SetValue("mobilePhone", model.MobilePhone);
                    newMember.SetValue("personTitle", model.Title);
                    newMember.SetValue("dateOfBirth", new DateTime(model.BirthDateYear, model.BirthDateMonth, model.BirthDateDay));

                    newMember.IsApproved = false;
                    if (!String.IsNullOrWhiteSpace(model.FirstName) || !String.IsNullOrWhiteSpace(model.LastName))
                    {
                        newMember.Name = string.Format("{0} {1}", model.FirstName, model.LastName);
                    }
                    memberService.Save(newMember);

                    // To create new user in the lottery system database
                    GamblerService service = new GamblerService();
                    service.CreateNewUser(model.MemberEmail);

                    // Login the new member
                    if (Membership.ValidateUser(model.MemberEmail, model.MemberPassword))
                    {
                        FormsAuthentication.SetAuthCookie(model.MemberEmail, false);
                    }
                }
                else
                {
                    //member exists
                }
            }
            catch (Exception ex)
            {
                this.Logger.Error(typeof(MemberLoginController), ex.ToString(), ex);
            }

            return RedirectToCurrentUmbracoPage();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginFromViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(model.MemberEmail, model.MemberPassword))
                {
                    //FormsAuthentication.SetAuthCookie(model.MemberEmail, true);
                    this.Members.Login(model.MemberEmail, model.MemberPassword);
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
            //FormsAuthentication.SignOut();
            this.Members.Logout();

            return Redirect("/");
        }

        private bool ValidateNewMemberData(LoginFromViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.FirstName)
                || string.IsNullOrWhiteSpace(model.LastName)
                || string.IsNullOrWhiteSpace(model.MemberEmail)
                || string.IsNullOrWhiteSpace(model.MobilePhone))
            {
                return false;
            }
            return true;
        }
    }
}