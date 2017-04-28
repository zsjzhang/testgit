using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Common;
namespace Vcyber.BLMS.Entity.Enum
{
    public enum EMLevelType
    {

        [EnumDescribe("请选择")]
        [Display(Name = "请选择")]
        ALL = -1,
        /// <summary>
        /// 普卡
        /// </summary>
        [Display(Name = "普卡")]
        MLevelType10 = 10,

        /// <summary>
        /// 银卡
        /// </summary>
        [Display(Name = "银卡")]
        MLevelType11 = 11,
       
        /// <summary>
        /// 金卡
        /// </summary>
        [Display(Name = "金卡")]
        MLevelType12 = 12,
    }
}
