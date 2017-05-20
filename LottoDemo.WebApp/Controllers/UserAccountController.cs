using LottoDemo.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Umbraco.Web.Mvc;

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
                    return RedirectToRoute("/");
                }

                ViewBag.MemberProfile = this.GetMemberProfile();

                return View("RenderDetailsForm", this.CurrentPage);
            }
            catch (Exception ex)
            {
                this.WriteErrorMessage(ex);
            }

            return RedirectToCurrentUmbracoPage();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateCustomerProfile(CustomerProfileModel model)
        {
            try
            {
                this.UpdateProfile(model);
            }
            catch (Exception ex)
            {
                this.WriteErrorMessage(ex);
            }

            return CurrentUmbracoPage();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateCustomerPassword(MemberPasswordChangeModel model)
        {
            try
            {
                this.UpdatePassword(model);
                ViewBag.ErrorsList = new List<string> { "Test Transfer 1", "Test Transfer 2" };
            }
            catch (Exception ex)
            {
                this.WriteErrorMessage(ex);
            }

            return CurrentUmbracoPage();
        }

        private MembershipUser GetCurrentUser()
        {
            var user = System.Web.Security.Membership.GetUser();
            return user;
        }

        private CustomerProfileModel GetMemberProfile()
        {
            CustomerProfileModel model = new CustomerProfileModel();

            var user = this.GetCurrentUser();
            if (user != null)
            {
                var member = Services.MemberService.GetByEmail(user.Email);
                model.ParseFromContent(member);
            }

            return model;
        }

        private void UpdatePassword(MemberPasswordChangeModel profile)
        {

        }

        private void UpdateProfile(CustomerProfileModel model)
        {

        }
    }
}