using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNet.Identity.SQL.FrontWeb
{
    public class MembershipRequestApproval
    {
        public string MembershipId { get; set; }

        public string ApprovalId { get; set; }

        public string RealName { get; set; }

        public string VIN { get; set; }

        public string IdentityNumber { get; set; }

        public string PhoneNumber { get; set; }

        public string SubmitTime { get; set; }

        public string Status { get; set; }

        public string StatusName { get; set; }

        public string IsPay { get; set; }

        public string PayNumber { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public decimal Amount { get; set; }
        public string DealerId { get; set; }
        public string No { get; set; }

        public string PaperWork {
            get; set; }
    }
}
