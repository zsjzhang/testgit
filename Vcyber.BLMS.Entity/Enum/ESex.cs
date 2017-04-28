using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity.Enum
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// 性别
    /// </summary>
    public enum ESex
    {
        /// <summary>
        /// 女
        /// </summary>
        [Display(Name = "女")]
        Female = 0,

        /// <summary>
        /// 男
        /// </summary>
        [Display(Name = "男")]
        Male = 1
    }
}
