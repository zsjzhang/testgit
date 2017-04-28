using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AspNet.Identity.SQL.FrontWeb.QueryEntity;
using Omu.ValueInjecter;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.ManageWeb.Models
{
    public class MembershipQueryViewModel
    {
        public string Identity { get; set; }

        public string DealerId { get; set; }

        public string RealName { get; set; }

        public string NickName { get; set; }

        public string VIN { get; set; }

        public string MLevel { get; set; }

        public string Status { get; set; }

        public int Skip { get; set; }

        public int Count { get; set; }

        public bool IsSonata9 { get; set; }

        public DateTime? BeginTime { get; set; }
        public DateTime? EndTime { get; set; }

        public string IDCard { get; set; }

        public string PayNumber { get; set; }
        public string IsTmall { get; set; }

        public string UserType { get; set; }

        public string City { get; set; }

        public string Area { get; set; }

        public string CarCategory { get; set; }
        public int Gender { get; set; }
       public string PaperWork { get; set; }

       public string isComValue { get; set; }

       public DateTime? AuthenticationTimeStart { get; set; }

       public DateTime? AuthenticationTimeEnd { get; set; }

       public DateTime? BuyTimeStart { get; set; }

       public DateTime? BuyTimeEnd { get; set; }

       public string AuthenticationSource { get; set; }

       public string CreatedPerson { get; set; }
       public string No { get; set; }

        // public int value { get; set; }

        public FrontUserQueryEntity ToEntity()
        {
            var entity = new FrontUserQueryEntity();
            entity.InjectFrom(this);
            return entity;
        }
   
    }
}