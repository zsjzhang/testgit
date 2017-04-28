using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Common;

namespace Vcyber.BLMS.Entity.Enum
{
    /// <summary>
    /// 订单支付类型
    /// </summary>
    public enum EPayType
    {
        /// <summary>
        /// 金钱
        /// </summary>
        [EnumDescribe("金钱")]
        Money = 1,

        /// <summary>
        /// 积分
        /// </summary>
        [EnumDescribe("积分")]
        Integral = 2,

        /// <summary>
        /// 奖品
        /// </summary>
        [EnumDescribe("奖品")]
        Prize = 3,

        /// <summary>
        /// 蓝豆
        /// </summary>
        [EnumDescribe("蓝豆")]
        BlueBean = 4,


        /// <summary>
        /// 积分+蓝豆
        /// </summary>
        [EnumDescribe("积分+蓝豆")]
        IntegralAndBlueBean=5
    }
}
