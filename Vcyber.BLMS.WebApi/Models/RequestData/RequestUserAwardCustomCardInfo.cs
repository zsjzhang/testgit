using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vcyber.BLMS.WebApi.Models.RequestData
{
    /// <summary>
    /// 用户卡券使用传参实体；
    /// </summary>
    public class RequestUserAwardCustomCardInfo : RequestCustomCardInfo
    {
        /// <summary>
        /// 车型
        /// </summary>
        public string CarCategory { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string CustName { get; set; }



        /// <summary>
        /// 行驶里程
        /// </summary>
        public string MileAge { get; set; }


        /// <summary>
        /// 手机号码
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public string DMSOrderNo { get; set; }

    }
}