using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Common;

namespace Vcyber.BLMS.Entity.Enum
{
    /// <summary>
    /// 蓝豆规则会员等级
    /// </summary>
    public enum EBUserGrade
    {
        /// <summary>
        /// 一星
        /// </summary>
       //[EnumDescribe("一星")]
       // YX=1,

        /// <summary>
        /// 二星
        /// </summary>
       //[EnumDescribe("二星")]
       // EX=2,

        /// <summary>
        /// 三星
        /// </summary>
        //[EnumDescribe("三星")]
        //SX=3

        /// <summary>
        /// 注册用户
        /// </summary>
        [EnumDescribe("注册用户")]
        YX = 1,
        /// <summary>
        /// 普卡会员
        /// </summary>
        [EnumDescribe("普卡")]
        pk = 10,
        /// <summary>
        /// 普卡会员
        /// </summary>
        [EnumDescribe("金卡")]
        jk = 12,
        /// <summary>
        /// 普卡会员
        /// </summary>
        [EnumDescribe("银卡")]
        yk = 11,
    }
}
