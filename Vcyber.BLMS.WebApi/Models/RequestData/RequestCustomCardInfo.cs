using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vcyber.BLMS.WebApi.Models.RequestData
{

    /// <summary>
    /// 查询卡券是否可用传入参数实体；
    /// </summary>
    public class RequestCustomCardInfo
    {
        /// <summary>
        /// 经销商Id
        /// </summary>
        public string DealerId { get; set; }

        /// <summary>
        /// 证件号码
        /// </summary>
        public string IdentityNumber { get; set; }

        ///// <summary>
        ///// 优惠券类型Guid
        ///// </summary>
        //public string CardType { get; set; }

        /// <summary>
        /// 优惠券号码（认证号码）
        /// </summary>
        public string CardNo { get; set; }

        /// <summary>
        ///   VIN 码
        /// </summary>
        public string Vin { get; set; }

    }
}