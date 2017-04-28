using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Common;

namespace Vcyber.BLMS.Entity.Enum
{
    /// <summary>
    /// 交易状态
    /// </summary>
    public enum ETradeState
    {
        /// <summary>
        /// 交易中
        /// </summary>
        [EnumDescribe("待付款")]
        JYZ = 1,

        /// <summary>
        /// 交易完成
        /// </summary>
        [EnumDescribe("交易完成")]
        JYWC = 2,

        /// <summary>
        /// 部分退款
        /// </summary>
        [EnumDescribe("部分退款")]
        BFTK = 3,

        /// <summary>
        /// 全部退款
        /// </summary>
        [EnumDescribe("全部退款")]
        WBTK = 4,

        /// <summary>
        /// 交易取消
        /// </summary>
        [EnumDescribe("交易取消")]
        JYQX = 5,

        /// <summary>
        /// 待发货
        /// </summary>
        [EnumDescribe("待发货")]
        DFH = 17,

        /// <summary>
        /// 已发货
        /// </summary>
        [EnumDescribe("已发货")]
        YFH = 6
    }
}
