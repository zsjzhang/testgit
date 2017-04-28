using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Common;

namespace Vcyber.BLMS.Entity.Enum
{
    /// <summary>
    /// 支付状态
    /// </summary>
    public enum EPayState
    {
        /// <summary>
        /// 待支付
        /// </summary>
        ZFZ=11,

        /// <summary>
        /// 支付完成
        /// </summary>
        ZFWC=2
    }

    public enum  PageOrderType
    {
        /// <summary>
        /// 待付款
        /// </summary>
        ///
        [EnumDescribe("特付款")]
        AwaitPay = 1,

        /// <summary>
        /// 待收货
        /// </summary>
        [EnumDescribe("待收货")]
        AwaitRecive = 2,
        /// <summary>
        /// 全部
        /// </summary>
        [EnumDescribe("全部")]
        ALL = 3,
    }
}
