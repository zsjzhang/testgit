using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    /// <summary>
    /// 问卷日报信息
    /// </summary>
    public class QuestionnarieDayReportInfo
    {
        #region ==== 构造函数 ====
        public QuestionnarieDayReportInfo() { }
        #endregion

        #region ==== 公共属性 ====

        /// <summary>
        /// 索九会员数量
        /// </summary>
        public int SjCount { get; set; }

        /// <summary>
        /// 普通会员数量
        /// </summary>
        public int PtCount { get; set; }

        /// <summary>
        /// 非车主会员数量
        /// </summary>
        public int FcCount { get; set; }

        /// <summary>
        /// 游客数量
        /// </summary>
        public int VisitorCount { get; set; }
        #endregion
    }

    public class QuestionnarieDayReportInfoCS
    {
        public QuestionnarieDayReportInfoCS() { }

        /// <summary>
        /// 索九会员数量
        /// </summary>
        public int SjCount { get; set; }

        /// <summary>
        /// 普通会员数量
        /// </summary>
        public int PtCount { get; set; }

        /// <summary>
        /// 新增索九会员数量
        /// </summary>
        public int NewSjCount { get; set; }

        /// <summary>
        /// 新增普通车主会员数量
        /// </summary>
        public int NewPtCount { get; set; }
    }
}
