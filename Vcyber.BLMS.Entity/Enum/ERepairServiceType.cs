using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Common;

namespace Vcyber.BLMS.Entity.Enum
{
    public enum ERepairServiceType
    { 
        /// <summary>
        /// 100日上门服务
        /// </summary>
        [Display(Name = "上门关怀服务")]
        [EnumDescribe("上门关怀服务")]

        _100dayCare = 0,

        /// <summary>
        /// 3年9次免检
        /// </summary>
        [Display(Name = "3年9次免检服务")]
        [EnumDescribe("3年9次免检服务")]
        _3year9time = 1,

        /// <summary>
        /// Home 2 Home取送车
        /// </summary>
        [Display(Name = "免费取送车服务")]
        [EnumDescribe("免费取送车服务")]
        Home2home = 2,

        /// <summary>
        /// 1对1服务
        /// </summary>
        [Display(Name = "一对一专享服务")]
        [EnumDescribe("一对一专享服务")]
        _11Servie = 3,
        /// <summary>
        /// 预约维保
        /// </summary>
        [Display(Name = "预约维保")]
        [EnumDescribe("预约维保")]
        BookMaintain = 4
    }
}
