using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity.SelectCondition
{
    /// <summary>
    /// 购物车金额统计
    /// </summary>
    public class ShoppingStatistics
    {
        #region ==== 构造函数 ====

        public ShoppingStatistics() { }

        #endregion

        #region ==== 公共属性 ====

        /// <summary>
        /// 
        /// </summary>
        public int TotalIntegral { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int TotalBlueBean { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int MyProperty { get; set; }

        #endregion
    }
}
