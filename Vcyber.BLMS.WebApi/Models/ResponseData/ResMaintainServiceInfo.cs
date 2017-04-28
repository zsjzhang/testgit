using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vcyber.BLMS.WebApi.Models.ResponseData
{
    /// <summary>
    /// 5种车辆养护产品实体
    /// </summary>
    public class ResMaintainServiceInfo
    {
        /// <summary>
        /// 卡券类型GUID
        /// </summary>
        public string CardType { get; set; }

        /// <summary>
        /// 卡券名字
        /// </summary>
        public string CardName { get; set; }


        /// <summary>
        /// 卡券logo
        /// </summary>
        public string CardLogoUrl
        {
            set;
            get;
        }

        /// <summary>
        /// 投放活动名称
        /// </summary>
        public string ActivityType { get; set; }
    }
}