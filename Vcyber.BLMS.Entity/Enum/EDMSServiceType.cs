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
    /// 维修单服务类型
    /// </summary>
    public enum EDMSServiceType
    {
        /// <summary>
        /// 上门关怀服务
        /// </summary>
        [Display(Name = "DMS-上门关怀服务")]
        [EnumDescribe("上门关怀服务")]
        Care = 0,

        /// <summary>
        /// 三年九次免检
        /// </summary>
        [Display(Name = "DMS-三年九次免检服务")]
        [EnumDescribe("三年九次免检服")]
        FreeCheck=1,

        /// <summary>
        /// 上门取送车服务
        /// </summary>
        [Display(Name = "DMS-上门取送车服务")]
        [EnumDescribe("上门取送车服务")]
        Home2Home=2,

        /// <summary>
        /// 一对一专属服务
        /// </summary>
        [Display(Name = "BM-一对一专属服务")]
        [EnumDescribe("一对一专属服务")]
        One2One=3,
        /// <summary>
        /// 长途旅行关怀服务
        /// </summary>
        [Display(Name = "BM-长途旅行关怀服务")]
        [EnumDescribe("长途旅行关怀服务")]
        LongDistanceTravel = 4
    }
}
