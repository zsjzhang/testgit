using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    /// <summary>
    /// 操作日志
    /// </summary>
    public class OperatorLog
    {
        #region ==== 构造函数 ====

        public OperatorLog()
        { }

        #endregion

        #region ==== 公共属性 ====

        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 操作类型
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 日志关联数据源Id
        /// </summary>
        public string SourceId { get; set; }

        /// <summary>
        /// 操作项目名称
        /// </summary>
        public string OperateItem { get; set; }

        /// <summary>
        /// 原始状态
        /// </summary>
        public string OriginalValue { get; set; }

        /// <summary>
        /// 当前操作状态
        /// </summary>
        public string CurrentValue { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime OperateTime { get; set; }


        /// <summary>
        /// 操作人员
        /// </summary>
        public string OperaterId { get; set; }

        /// <summary>
        /// 操作人员
        /// </summary>
        public string OperaterName { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        #endregion
    }
}
