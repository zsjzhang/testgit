using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    /// <summary>
    /// 用户交易流水
    /// </summary>
    public class Tradeflow
    {
        #region ==== 构造函数 ===

        public Tradeflow() { }

        #endregion

        #region ==== 公共属性 ====

        /// <summary>
        /// 
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 流水编号
        /// </summary>
        public string FlowCode { get; set; }

        /// <summary>
        /// 交易类型（1=订单支付、2=订单退款）
        /// </summary>
        public int TradeType { get; set; }

        /// <summary>
        /// 交易积分
        /// </summary>
        public int tradeintegral { get; set; }

        /// <summary>
        /// 蓝豆价
        /// </summary>
        public int BlueBean { get; set; }

        /// <summary>
        /// 交易金额
        /// </summary>
        public int trademoney { get; set; }

        /// <summary>
        /// 订单Id
        /// </summary>
        public string orderId { get; set; }

        /// <summary>
        /// 退款Id
        /// </summary>
        public string rebackId { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime UpdateTime { get; set; }


        #endregion
    }
}
