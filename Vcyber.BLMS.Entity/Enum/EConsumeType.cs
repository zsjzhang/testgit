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
    /// 消费类型
    /// </summary>
    //该枚举的值必须和EOrderMode保持统一
    public enum EConsumeType
    {



        [EnumDescribe("请选择")]
        [Display(Name = "请选择",Order=1)]
        ALL = -1,


        //[EnumDescribe("保修")]
        //[Display(Name = "保修")]
        //BX = 2,

        [EnumDescribe("定期保养")]
        [Display(Name = "定期保养",Order=2)]
        DQBY = 3,

        //[EnumDescribe("一般钣喷维修")]
        //[Display(Name = "一般钣喷维修")]
        //FPWX = 8,

        [EnumDescribe("事故车维修（普通）")]
        [Display(Name = "事故车维修（普通）",Order=3)]
        STCPT = 0,

        [EnumDescribe("钣喷")]
        [Display(Name = "钣喷",Order=4)]
        FPWX = 8,

        [EnumDescribe("首次保养")]
        [Display(Name = "首次保养",Order=5)]
        SCBY = 1,

        //[EnumDescribe("事故车维修（理赔）")]
        //[Display(Name = "事故车维修（理赔）")]
        //STCLP = 10,


        //[EnumDescribe("机修")]
        //[Display(Name = "机修")]
        //JX = 11,


        //[EnumDescribe("购车")]
        //[Display(Name = "购车")]
        //GC = 12,


        /// <summary>
        /// 购车
        /// </summary>
        [EnumDescribe("购车")]
        [Display(Name = "购车",Order=6)]
        Purchase = 2,

        [EnumDescribe("配件")]
        [Display(Name = "配件",Order=7)]
        Pj = 9,

        [EnumDescribe("精品")]
        [Display(Name = "精品",Order=8)]
        Jp = 10,


    }
}
