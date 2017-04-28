using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity.Enum;

namespace Vcyber.BLMS.Entity
{
    /// <summary>
    /// DMS取消工单消费
    /// </summary>
    public class RequestUpdateConsuem
    {
        #region ==== 构造函数 ====

        public RequestUpdateConsuem() { }

        #endregion

        #region ==== 公共属性 ====

        /// <summary>
        /// 消费使用积分数量（取消工单的时候应该加上）
        /// </summary>
        public int  ConsumePoints { get; set; }

        ///// <summary>
        ///// 消费是返还的积分（取消工单的时候要减去）
        ///// </summary>
        //public int RewardPoints { get; set; }

        /// <summary>
        /// 应付金额（总费用）
        /// </summary>
        public string TotalCost { get; set; }

        /// <summary>
        /// DMS消费工单号
        /// </summary>
        public string DMSOrderNo { get; set; }

        ///// <summary>
        ///// 实际支付金额 
        ///// </summary>
        //public string PurchaseCost { get; set; }

        ///// <summary>
        ///// 使用积分抵扣的金额
        ///// </summary>
        //public string PointCost { get; set; }

        #endregion
    }

}
