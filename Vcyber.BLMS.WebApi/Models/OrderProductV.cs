using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vcyber.BLMS.WebApi.Models
{
    /// <summary>
    /// 用户兑换商品信息
    /// </summary>
    public class OrderProductV
    {
        /// <summary>
        /// 商品名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 兑换积分
        /// </summary>
        public int credit { get; set; }

        /// <summary>
        /// 兑换蓝豆
        /// </summary>
        public int BlueBean { get; set; }

        /// <summary>
        /// 兑换时间
        /// </summary>
        public string date { get; set; }
    }
}