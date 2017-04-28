using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Common;

namespace Vcyber.BLMS.Entity.Enum
{
    using System.ComponentModel.DataAnnotations;

    public enum EPointApproveStatus
    {
        /// <summary>
        /// 未审核
        /// </summary>
        [Display(Name="未审核")]
        NoBegin = 0,

        /// <summary>
        /// 审核通过
        /// </summary>
        [Display(Name = "通过")]
        Approved = 1,

        /// <summary>
        /// 自动审核
        /// </summary>
        [Display(Name = "自动审核")]
        AutoApproved=2,

        /// <summary>
        /// 审核未通过
        /// </summary>
        [Display(Name = "未通过")]
        NotApproved = 3,

        /// <summary>
        /// 所有状态
        /// </summary>
        [Display(Name="所有")]
        All=-1
    }
}
