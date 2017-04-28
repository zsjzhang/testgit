using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vcyber.BLMS.ManageWeb.Models
{
    public class MemberShipRequestFailedModel
    {
        public int Id { get; set; }
        public string UserId { get;set; }
        public string UserName { get; set;}
        public string RequestTime { get; set; }

        public string IdentityNumber { get; set; }

        public string VIN { get; set; }

        public int Status { get; set; }

        public string StatusValue
        {
            get
            {
                switch (this.Status)
                {
                    case 1:
                        return "未处理";
                    case 2:
                        return "已处理";
                    default:
                        return "";

                }
            }

        }

        public string OperationTime { get; set; }
        public string Operator { get; set; }
        public string PayNumber { get; set; }

        public string IsPay { get; set; }

        public string CarCategory { get; set; }
    }
}