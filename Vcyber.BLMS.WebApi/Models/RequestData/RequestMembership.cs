using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vcyber.BLMS.WebApi.Models.RequestData
{
    /// <summary>
    /// 经销商协助入会参数实体信息
    /// </summary>
    public class RequestMembership
    {
        /// <summary>
        /// 手机号码
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 证件类型（身份证 = 1, 护照 = 2,  军官证 = 3）
        /// </summary>
        public string PaperWork { get; set; }

        /// <summary>
        /// 证件号码
        /// </summary>
        public string IdentityNumber { get; set; }

        /// <summary>
        /// 经销商编号
        /// </summary>
        public string DealerId { get; set; }


        /// <summary>
        /// 是否已经交付  0：未支付；1已支付
        /// </summary>
        public string Agree { get; set; }

        /// <summary>
        /// 缴费金额
        /// </summary>
        public string Amount { get; set; }


        ///// <summary>
        ///// 昵称
        ///// </summary>
        //public string NickName { get; set; }

        /// <summary>
        ///  录入人ID
        /// </summary>
        //public string Operator { get; set; }



    }
}