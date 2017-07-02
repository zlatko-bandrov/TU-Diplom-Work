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
        public string PersonTitle { get; set; }
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
                this.FirstName = member.GetValue<string>("FirstName");
                this.LastName = member.GetValue<string>("LastName");
                this.PersonTitle = member.GetValue<string>("PersonTitle");
                this.DateOfBirth = (DateTime)member.GetValue("DateOfBirth");
                this.AddressLine1 = member.GetValue<string>("AddressLine1");
                this.AddressLine2 = member.GetValue<string>("AddressLine2");
                this.City = member.GetValue<string>("City");
                this.County = member.GetValue<string>("County");
                this.PostalCode = member.GetValue<string>("PostalCode");
                this.MobilePhone = member.GetValue<string>("MobilePhone");
                this.WorkPhone = member.GetValue<string>("WorkPhone");
                this.Email = member.Email;
            }
        }
    }
}