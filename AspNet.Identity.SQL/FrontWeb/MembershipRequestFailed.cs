using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNet.Identity.SQL
{
    public class MembershipRequestFailed
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string RequestTime { get; set; }

        public string IdentityNumber { get; set; }

        public string VIN { get; set; }

        public int Status { get; set; }

        public string StatusValue { get; set; }

        public string OperationTime { get; set; }
        public string Operator { get; set; }
        public string PayNumber { get; set; }
        public string IsPay
        {
            get
            {
                return string.IsNullOrEmpty(PayNumber) ? "否" : "是";
            }
        }

        public string CarCategory { get; set; }
    }
}
