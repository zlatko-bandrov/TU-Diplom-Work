using LottoDemo.Common;
using LottoDemo.Entities.Models;
using System;
using System.Web.Mvc;
using System.Web.Security;

namespace LottoDemo.WebApp.Controllers
{
    public class UserAccountController : BaseController
    {
        public ActionResult RenderCustomerDetailsForm()
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return RedirectToUmbracoPage(Constants.HOME_PAGE_ID);
                }

                ViewBag.MemberProfile = this.GetMemberProfile();
            }
            catch (Exception ex)
            {
                this.WriteErrorMessage(ex);
            }

            return View("RenderDetailsForm", CurrentPage);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateCustomerProfile(CustomerProfileModel model)
        {
            try
            {
                UpdateProfile(model);
            }
            catch (Exception ex)
            {
                WriteErrorMessage(ex);
            }

            return CurrentUmbracoPage();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateCustomerPassword(MemberPasswordChangeModel model)
        {
            try
            {
                UpdatePassword(model);
            }
            catch (Exception ex)
            {
                WriteErrorMessage(ex);
            }

            return CurrentUmbracoPage();
        }

        private MembershipUser GetCurrentUser()
        {
            var user = Membership.GetUser();
            return user;
        }

        private CustomerProfileModel GetMemberProfile()
        {
            CustomerProfileModel model = new CustomerProfileModel();

            var user = GetCurrentUser();
            if (user != null)
            {
                var member = Services.MemberService.GetByEmail(user.Email);
                model.ParseFromContent(member);
            }

            return model;
        }

        private void UpdatePassword(MemberPasswordChangeModel profile)
        {
            if (String.IsNullOrWhiteSpace(profile.CurrentPassword))
            {
                AddModelError(Constants.ERR_ACCOUNT_OLDPASSREQUIRED_KEY);
            }
            if (String.IsNullOrWhiteSpace(profile.NewPassword))
            {
                AddModelError(Constants.ERR_ACCOUNT_NEWPASSREQUIRED_KEY);
            }
            if (String.IsNullOrWhiteSpace(profile.ConfirmPassword))
            {
                AddModelError(Constants.ERR_ACCOUNT_CONFIRMPASSREQUIRED_KEY);
            }

            if (!String.IsNullOrEmpty(profile.CurrentPassword)
                && !String.IsNullOrEmpty(profile.NewPassword)
                && String.Compare(profile.ConfirmPassword, profile.NewPassword) == 0)
            {
                var currentUser = GetCurrentUser();
                currentUser.ChangePassword(profile.CurrentPassword, profile.NewPassword);
            }
            else
            {
                ModelState.Clear();
                AddModelError(Constants.ERR_ACCOUNT_PASSNOTMATCH_KEY);
            }
        }

        private void UpdateProfile(CustomerProfileModel model)
        {
            var currentUser = GetCurrentUser();
            if (currentUser == null)
            {
                return;
            }

            var memberService = Services.MemberService;
            if (memberService != null)
            {
                var newMember = memberService.GetByEmail(currentUser.Email);

                // Set the new member data
                newMember.SetValue("firstName", model.FirstName);
                newMember.SetValue("lastName", model.LastName);
                newMember.SetValue("mobilePhone", model.MobilePhone);
                newMember.SetValue("personTitle", model.PersonTitle);

                newMember.SetValue("city", model.City);
                newMember.SetValue("county", model.County);
                newMember.SetValue("postalCode", model.PostalCode);
                newMember.SetValue("addressLine1", model.AddressLine1);
                newMember.SetValue("addressLine2", model.AddressLine2);
                newMember.SetValue("workPhone", model.WorkPhone);

                memberService.Save(newMember);
            }
        }

        private void AddModelError(string errorCode)
        {
            ModelState.AddModelError(errorCode, Umbraco.GetDictionaryValue(errorCode));
        }
    }
}