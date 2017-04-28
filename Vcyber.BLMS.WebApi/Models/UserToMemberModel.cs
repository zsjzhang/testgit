using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vcyber.BLMS.WebApi.Models
{
    public class UserToMemberModel
    {
        public string UserId { get; set; }

        /// <summary>
        /// 1: 个人客户 2:集团客户
        /// </summary>
        public int CustomerType { get; set; }

        public string IdentityNumber { get; set; }

        public string VIN { get; set; }

        public string DealerId { get; set; }

        public string PayNumber { get; set; }

        /// <summary>
        /// 1:4S店申请 2:支付码
        /// </summary>
        public int ActivityType { get; set; }
    }
}