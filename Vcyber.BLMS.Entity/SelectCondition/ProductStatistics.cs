using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity.SelectCondition
{
    /// <summary>
    /// 商品统计
    /// </summary>
    public class ProductStatistics
    {
        #region ==== 构造函数 ====

        public ProductStatistics() { }

        #endregion

        #region ==== 公共属性 ====

        /// <summary>
        /// 商品总个数
        /// </summary>
        public int ProductCount { get; set; }

        /// <summary>
        /// 下架商品个数
        /// </summary>
        public int XJCount { get; set; }

        /// <summary>
        /// 新品个数
        /// </summary>
        public int XPCount { get; set; }

        /// <summary>
        /// 热销商品个数
        /// </summary>
        public int RXCount { get; set; }

        #endregion
    }
}
