using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Models;
using Umbraco.Core.Models.Membership;

namespace LottoDemo.Entities.Models
{
    public class CustomerProfileModel
    {
        public string Gender { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string County { get; set; }
        public string PostalCode { get; set; }
        public string MobilePhone { get; set; }
        public string WorkPhone { get; set; }
        public string Email { get; set; }

        public void ParseFromContent(IMember member)
        {
            if (member != null)
            {
                this.FirstName = member.GetValue("FirstName").ToString();
                this.LastName = member.GetValue("LastName").ToString();
                this.Gender = member.GetValue("Gender").ToString();
                this.DateOfBirth = (DateTime)member.GetValue("DateOfBirth");
                this.AddressLine1 = member.GetValue("AddressLine1").ToString();
                this.AddressLine2 = member.GetValue("AddressLine2").ToString();
                this.City = member.GetValue("City").ToString();
                this.County = member.GetValue("County").ToString();
                this.PostalCode = member.GetValue("PostalCode").ToString();
                this.MobilePhone = member.GetValue("MobilePhone").ToString();
                this.WorkPhone = member.GetValue("WorkPhone").ToString();
                this.Email = member.Email;
            }
        }
    }
}