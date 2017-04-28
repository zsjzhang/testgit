using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity.SelectCondition
{
    /// <summary>
    /// 操作日志搜索参数
    /// </summary>
    public class LogSearchParamater
    {
        #region ==== 私有字段 ====

        private int pageIndex;

        private int pageSize;

        private DateTime endTime;

        #endregion

        #region ==== 构造函数 ====

        public LogSearchParamater()
        { }

        #endregion

        #region ==== 公共属性 ====

        /// <summary>
        /// 操作者名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 操作开始时间
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 操作结束时间
        /// </summary>
        public DateTime EndTime { get { return this.endTime == DateTime.MinValue ? DateTime.MaxValue : this.endTime; } set { this.endTime = value; } }

        /// <summary>
        /// 分页索引
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 分页大小
        /// </summary>
        public int PageSize { get; set; }

        #endregion
    }
}
