using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vcyber.BLMS.WebApi.Models
{
    /// <summary>
    /// 商品收货地址
    /// </summary>
    public class TradeAddressV
    {
        /// <summary>
        /// 收货人姓名
        /// </summary>
        public string receiveName { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string phone { get; set; }
       
        /// <summary>
        /// 省市县中文 数据格式（xxx,xxx,xxx）
        /// </summary>
        public string pCC { get; set; }

        /// <summary>
        /// 省Id
        /// </summary>
        public int provincesId { get; set; }

        /// <summary>
        /// 市Id
        /// </summary>
        public int cityId { get; set; }

        /// <summary>
        /// 区县Id
        /// </summary>
        public int areaId { get; set; }

        /// <summary>
        /// 邮编
        /// </summary>
        public string zipCode { get; set; }

        /// <summary>
        /// 详细地址
        /// </summary>
        public string detail { get; set; }
    }
}