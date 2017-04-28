using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vcyber.BLMS.WebApi.Models.RequestData
{
    /// <summary>
    /// 蓝缤会员积分明细传参实体
    /// </summary>
    public class RequestUserintegral
    {
        /// <summary>
        /// 经销商Id
        /// </summary>
        public string DealerId { get; set; }
        /// <summary>
        /// 开始日期
        /// </summary>
        public string BeginDate { get; set; }
        /// <summary>
        /// 结束日期
        /// </summary>
        public string EndDate { get; set; }

        /// <summary>
        /// 证件号码
        /// </summary>
        public string IdentityNumber { get; set; }


        /// <summary>
        /// 会员卡号
        /// </summary>
        public string BlueMembership_No { get; set; }

        
    }
}