using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vcyber.BLMS.WebApi.Models.ResponseData
{
    public class ResMembershipApproval
    {
        /// <summary>
        /// 会员积分
        /// </summary>
        public string Point
        {
            get;
            set;
        }

        /// <summary>
        /// 会员编号
        /// </summary>
        public string BlueMembership_Id { get; set; }
        /// <summary>
        /// 会员卡号
        /// </summary>
        public string BlueMembership_No { get; set; }

        #region add by wangchunrong 2016-09-04
        /// <summary>
        /// 会员等级
        /// </summary>
        public string Mlevel { get; set; }

        /// <summary>
        ///会员失效时间
        /// </summary>
        public string MLevelInvalidDate { get; set; }

        /// <summary>
        /// 会员生效日期
        /// </summary>
        public string MLevelBeginDate { get; set; }

        /// <summary>
        /// 蓝宾会员是否
        /// </summary>
        public string BlueMembership_YN { get; set; }

        /// <summary>
        /// 等级认证时间
        /// </summary>
        public string AuthenticationTime { get; set; }
        #endregion
    }
}