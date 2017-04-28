using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Vcyber.BLMS.Entity.Enum
{
    /// <summary>
    /// 积分发放状态
    /// </summary>
    public enum EPointStatus
    {
        /// <summary>
        /// 未发放
        /// </summary>
        [Display(Name = "未发放")]
        ToDo = 0,
        /// <summary>
        /// 已发放
        /// </summary>
        [Display(Name = "已发放")]
        Done = 1,
        /// <summary>
        /// 全部
        /// </summary>
        [Display(Name = "全部")]
        All = 2
    }
}
