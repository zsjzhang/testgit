using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vcyber.BLMS.WebApi.Models
{
    /// <summary>
    /// 积分值
    /// </summary>
    public class IntegralValueV
    {
        #region ==== 构造函数 ====

        public IntegralValueV() { }

        #endregion

        #region ==== 公共属性 ====

        /// <summary>
        /// 总积分
        /// </summary>
        public int TotalValue { get; set; }

        #endregion
    }
}