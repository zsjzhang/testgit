using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vcyber.BLMS.WebApi.Models
{
    /// <summary>
    /// 交易商品
    /// </summary>
    public class TradeProductV
    {
        /// <summary>
        /// 商品id
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 兑换数量
        /// </summary>
        public int quantity { get; set; }
        /// <summary>
        /// 商品颜色
        /// </summary>
        public string ProductColor { get; set; }

        /// <summary>
        /// 商品类型
        /// </summary>
        public string ProductType { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName { get; set; }//add by wangchunrong 在积分明细记录表里添加礼品名称20161205
    }
}