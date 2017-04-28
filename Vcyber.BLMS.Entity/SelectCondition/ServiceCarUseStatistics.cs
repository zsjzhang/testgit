using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    /// <summary>
    /// 服务卡使用统计
    /// </summary>
    public class ServiceCarUseStatistics
    {
        #region ==== 构造函数 ====

        public ServiceCarUseStatistics() { }

        #endregion

        #region ==== 公共属性 ====

        /// <summary>
        /// 未发放
        /// </summary>
        public int WF { get; set; }

        /// <summary>
        /// 已发放
        /// </summary>
        public int YF { get; set; }

        /// <summary>
        /// 已核销
        /// </summary>
        public int YHX { get; set; }

        /// <summary>
        /// 已结算
        /// </summary>
        public int YJS { get; set; }

        #endregion
    }
}
