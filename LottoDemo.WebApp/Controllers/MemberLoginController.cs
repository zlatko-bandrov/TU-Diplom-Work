using LottoDemo.BusinessLogic.Services;
using LottoDemo.Common;
using LottoDemo.WebApp.Models.Membership;
using System;
using System.Net.Mail;
using System.Web.Mvc;
using System.Web.Security;
using Umbraco.Web.Mvc;

namespace LottoDemo.WebApp.Controllers
{
    public class MemberLoginController : SurfaceController
    {
        private readonly string NewMemberGroupName = "Lottery Games";
        private readonly string NewMemberType = "lottoGamer";

        public LottoUserService LottoUserService = new LottoUserService();

        public ActionResult RenderLoginDialog()
        {
            var model = new LoginFromViewModel();

            if (!ModelState.IsValid)
            {
                ViewBag.ShowErrors = true;
            }

            return PartialView("LoginForm/_LoginFormDialog", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MemberRegistration(LoginFromViewModel model)
        {
            try
            {
                if (!ValidateNewMemberData(model))
                {
                    return CurrentUmbracoPage();
                }

                var memberService = Services.MemberService;

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
                    LottoUserService.CreateNewUser(model.MemberEmail);

                    // Login the new member
                    if (Membership.ValidateUser(model.MemberEmail, model.MemberPassword))
                    {
                        FormsAuthentication.SetAuthCookie(model.MemberEmail, false);
                    }
                }
                else
                {
                    AddModelError(Constants.ERR_USERREG_USEREXISTS_KEY);
                }
            }
            catch (Exception ex)
            {
                this.Logger.Error(typeof(MemberLoginController), ex.ToString(), ex);
            }

            return CurrentUmbracoPage();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginFromViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (String.IsNullOrEmpty(model.MemberEmail))
                {
                    AddModelError(Constants.ERR_USERLOG_REQUIREDEMAIL_KEY);
                }
                if (String.IsNullOrEmpty(model.MemberPassword))
                {
                    AddModelError(Constants.ERR_USERLOG_PASSREQUIRED_KEY);
                }

                if (!String.IsNullOrEmpty(model.MemberEmail) && !String.IsNullOrEmpty(model.MemberPassword))
                {
                    if (Membership.ValidateUser(model.MemberEmail, model.MemberPassword))
                    {
                        //FormsAuthentication.SetAuthCookie(model.MemberEmail, true);
                        this.Members.Login(model.MemberEmail, model.MemberPassword);
                        return RedirectToUmbracoPage(1050);
                    }
                    else
                    {
                        ModelState.Clear();
                        AddModelError(Constants.ERR_USERLOG_NOTVALIDCRED_KEY);
                    }
                }
            }

            return CurrentUmbracoPage();
        }

        public ActionResult Logout()
        {
            Session.Clear();
            //FormsAuthentication.SignOut();
            this.Members.Logout();

            return Redirect("/");
        }

        public ActionResult ForgottenPassword(string userEmail)
        {
            if (string.IsNullOrEmpty(userEmail))
            {
                return CurrentUmbracoPage();
            }

            MembershipUser member = Membership.GetUser(userEmail);
            if (member == null)
            {
                return CurrentUmbracoPage();
            }
            string newPassword = member.ResetPassword();
            MailMessage email = new MailMessage("bravehart_1985@abv.bg", userEmail, "LottoDemo Password Recovery Email", string.Format("Your temporary password is: {0}", newPassword));

            try
            {
                SmtpClient smtp = new SmtpClient();
                smtp.Send(email);
            }
            catch (Exception ex)
            {
                Logger.Error(typeof(MemberLoginController), ex.ToString(), ex);
                throw ex;
            }

            return CurrentUmbracoPage();
        }

        private bool ValidateNewMemberData(LoginFromViewModel model)
        {
            bool result = true;
            if (string.IsNullOrWhiteSpace(model.FirstName))
            {
                AddModelError(Constants.ERR_USERREG_REQUIREDNAME_KEY);
                result = false;
            }
            if (string.IsNullOrWhiteSpace(model.LastName))
            {
                AddModelError(Constants.ERR_USERREG_REQUIREDFAMILY_KEY);
                result = false;
            }
            if (string.IsNullOrWhiteSpace(model.MemberEmail))
            {
                AddModelError(Constants.ERR_USERREG_REQUIREDEMAIL_KEY);
                result = false;
            }
            if (string.IsNullOrWhiteSpace(model.MemberPassword))
            {
                AddModelError(Constants.ERR_USERREG_PASSREQUIRED_KEY);
                result = false;
            }
            if (string.IsNullOrWhiteSpace(model.MobilePhone))
            {
                AddModelError(Constants.ERR_USERREG_PHONEREQUIRED_KEY);
                result = false;
            }

            try
            {
                DateTime date = new DateTime(model.BirthDateYear, model.BirthDateMonth, model.BirthDateDay);
                if (date.AddYears(18) > DateTime.Now.Date)
                {
                    AddModelError(Constants.ERR_USERREG_DATE18YEARS_KEY);
                    result = false;
                }
            }
            catch
            {
                AddModelError(Constants.ERR_USERREG_NONEVALIDDATE_KEY);
                result = false;
            }

            return result;
        }

        private void AddModelError(string errorCode)
        {
            ModelState.AddModelError(errorCode, Umbraco.GetDictionaryValue(errorCode));
        }
    }
}