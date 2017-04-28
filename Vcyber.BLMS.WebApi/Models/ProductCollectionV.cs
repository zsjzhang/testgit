using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vcyber.BLMS.WebApi.Models
{
    /// <summary>
    /// 商城列表
    /// </summary>
    public class ProductCollectionV
    {
        /// <summary>
        /// 商品类型
        /// </summary>
        public int type { get; set; }

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
        /// 商品集合节点
        /// </summary>
        public List<ProductV> gifts { get; set; }
    }
}