using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;

namespace Vcyber.BLMS.Application.BLMSMoney
{
    /// <summary>
    /// 订单支付信息抽象类型
    /// </summary>
    public abstract class OrderPayInfoBase
    {
        #region ==== 公共属性 ====

        /// <summary>
        /// 用户Id
        /// </summary>
        public string userId { get; set; }

        /// <summary>
        /// 订单Id
        /// </summary>
        public string orderId { get; set; }

        #endregion

        /// <summary>
        /// 验证订单支付数据是否合法
        /// </summary>
        /// <param name="flowData"></param>
        /// <returns></returns>
        public abstract bool ValidatePayData(out Tradeflow flowData, out EPayType payType);
    }
}
