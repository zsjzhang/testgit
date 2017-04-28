using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity.Enum
{
    using System.ComponentModel.DataAnnotations;

    using Vcyber.BLMS.Common;

    /// <summary>
    /// 服务单类型
    /// </summary>
    public enum EBMServiceType
    {
        /// <summary>
        /// 上门关怀服务
        /// </summary>
        [Display(Name = "上门关怀服务")]
        [EnumDescribe("BM-上门关怀服务")]
        Care = 0,

        /// <summary>
        /// 三年九次免检服务
        /// </summary>
        [Display(Name = "三年九次免检服务")]
        [EnumDescribe("BM-三年九次免检服务")]
        FreeCheck = 1,

        /// <summary>
        /// 上门取送车服务
        /// </summary>
        [Display(Name = "上门取送车服务")]
        [EnumDescribe("BM-上门取送车服务")]
        Home2Home = 2,

        /// <summary>
        /// 一对一专属服务
        /// </summary>
        [Display(Name = "一对一专属服务")]
        [EnumDescribe("BM-专属预约维保服务")]
        SpecialMaintain = 3,

        /// <summary>
        /// 普通预约维保服务
        /// </summary>
        [Display(Name = "普通预约维保服务")]
        [EnumDescribe("BM-普通预约维保服务")]
        CommonMaintain = 4,

        [Display(Name = "长途旅行关怀服务")]
        [EnumDescribe("BM-长途旅行关怀服务")]
        LongDistanceTravel = 5,

        /// <summary>
        /// 领动上市活动
        /// </summary>
        [Display(Name = "领动上市活动")]
        [EnumDescribe("BM-领动上市活动")]
        LingDong = 6,

        /// <summary>
        /// 领动上市活动预约维保
        /// </summary>
        [Display(Name = "悦纳上市活动预约维保")]
        [EnumDescribe("BM-悦纳上市活动预约维保")]
        Yuena = 7,
    }
}
