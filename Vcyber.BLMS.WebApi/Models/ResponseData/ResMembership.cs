using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vcyber.BLMS.WebApi.Models.ResponseData
{

    /// <summary>
    /// 
    /// </summary>
    public class ResMembership : BaseMembership
    {

        /// <summary>
        /// 会员生效时间
        /// </summary>
        public string MLevelBeginDate { get; set; }

        /// <summary>
        /// 会员失效日期
        /// </summary>
        public string MLevelInvalidDate { get; set; }

        /// <summary>
        /// 缴费金额
        /// </summary>
        public string Amount { get; set; }

        /// <summary>
        /// 等级认证时间
        /// </summary>
        public string AuthenticationTime { get; set; }

        ///// <summary>
        /////会员积分
        ///// </summary>
        //public string Point { get; set; }

    }
}