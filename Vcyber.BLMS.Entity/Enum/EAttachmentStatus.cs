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
    public enum EAttachmentStatus
    {
        /// <summary>
        /// 未上传
        /// </summary>
        [Display(Name = "未上传")]
        ToDo = 0,
        /// <summary>
        /// 已上传
        /// </summary>
        [Display(Name = "已上传")]
        Done = 1,
        /// <summary>
        /// 全部
        /// </summary>
        [Display(Name = "全部")]
        All = 2
    }

  
}
