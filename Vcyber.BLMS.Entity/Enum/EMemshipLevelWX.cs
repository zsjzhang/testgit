using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Common;

namespace Vcyber.BLMS.Entity.Enum
{
    /// <summary>
    /// 账户级别
    /// </summary>
    public enum EMemshipLevelWX
    {
        /// <summary>
        /// 潜客账户
        /// </summary>
        [EnumDescribe("潜客账户")]
        QZ = 0,

        /// <summary>
        /// 索九会员
        /// </summary>
        [EnumDescribe("银卡会员")]
        SJ = 9,

        /// <summary>
        /// 一级账户
        /// </summary>
        [EnumDescribe("注册用户 ")]
        OneStar = 1,

        /// <summary>
        /// 二级账户
        /// </summary>
        //[EnumDescribe("二星账户")]
        //TwoStar = 2,

        /// <summary>
        /// 三级账户
        /// </summary>
        //[EnumDescribe("三星账户")]
        //ThreeStar = 3,

        /// <summary>
        /// 银卡会员
        /// </summary>
        [EnumDescribe("银卡账户")]
        ThreeStar1 = 11,


        /// <summary>
        /// 金卡会员
        /// </summary>
        [EnumDescribe("金卡账户")]
        ThreeStar = 12,
        /// <summary>
        /// 待激活银卡会员
        /// </summary>
        SilverNotActive=4,
        /// <summary>
        /// 银卡会员
        /// </summary>
        Silver=5
    }
}
