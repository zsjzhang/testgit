using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vcyber.BLMS.WebApi.Models
{
    /// <summary>
    /// 购买商品集合
    /// </summary>
    public class OrderProductCollectionV
    {
        /// <summary>
        /// 结果记录总数
        /// </summary>
        public int totalrecord { get; set; }

        /// <summary>
        /// 当前返回记录数
        /// </summary>
        public int record { get; set; }

        /// <summary>
        /// 当前页
        /// </summary>
        public int page { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public int totalpage { get; set; }

        /// <summary>
        /// 购买商品集合
        /// </summary>
        public List<OrderProductV> results { get; set; }
    }
}