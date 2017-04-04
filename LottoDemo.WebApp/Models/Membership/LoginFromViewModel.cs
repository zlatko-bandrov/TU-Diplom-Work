using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LottoDemo.WebApp.Models.Membership
{
    public class LoginFromViewModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string Country { get; set; }

        public string MobilePhone { get; set; }

        public string MemberEmail { get; set; }

        public string MemberPassword { get; set; }
    }
}