using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Vcyber.BLMS.Common;

namespace Vcyber.BLMS.Entity.Enum
{
    /// <summary>
    /// 服务类型
    /// </summary>
    public enum EOrderType
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
        FreeCheck=1,

        /// <summary>
        /// 上门取送车服务
        /// </summary>
        [Display(Name = "上门取送车服务")]
        [EnumDescribe("BM-上门取送车服务")]
        Home2Home=2,

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
        CommonMaintain=4,

        /// <summary>
        /// 线上订车服务
        /// </summary>
        [Display(Name = "线上订车服务")]
        [EnumDescribe("线上订车服务")]
        OrderCar=6,
        /// <summary>
        /// 预约试驾服务
        /// </summary>
        [Display(Name = "预约试驾服务")]
        [EnumDescribe("预约试驾服务")]
        TestDrive=7,

        /// <summary>
        /// 长途旅行关怀服务
        /// </summary>
        [Display(Name = "长途旅行关怀服务")]
        [EnumDescribe("长途旅行关怀服务")]
        LongDistanceTravel = 5
    }
}
