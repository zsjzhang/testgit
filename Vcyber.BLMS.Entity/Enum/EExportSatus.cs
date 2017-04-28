using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Common;

namespace Vcyber.BLMS.Entity.Enum
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// 数据导出状态
    /// </summary>
    public enum EExportSatus
    {
        /// <summary>
        /// 未导出
        /// </summary>
        [Display(Name = "未导出")]
        No = 0,

        /// <summary>
        /// 已导出
        /// </summary>
        [Display(Name = "已导出")]
        Yes=1,

        /// <summary>
        /// 已导出
        /// </summary>
        [Display(Name = "全部")]
        All = -1

        
    }
}
