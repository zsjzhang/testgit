using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity.Enum
{
    /// <summary>
    /// 交易类型
    /// </summary>
    public enum ETradeType
    {
        /// <summary>
        /// 订单支付
        /// </summary>
        OrderPay=1,

        /// <summary>
        /// 订单退款
        /// </summary>
        OrderReturn=2
    }
}
