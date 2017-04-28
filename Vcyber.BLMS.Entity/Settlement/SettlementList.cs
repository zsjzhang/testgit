using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Vcyber.BLMS.Entity
{
    /// <summary>
    /// 
    /// </summary>
   public class SettlementList
    {
        /// <summary>
        /// 主键ID
        /// </summary>
      public string Id { get; set; }

        /// <summary>
        /// 结算状态
        /// </summary>
        public string SettlementState { get; set; }
        /// <summary>
        /// 区域
        /// </summary>
        public string Area { get; set; }

        /// <summary>
        /// 办事处
        /// </summary>
        public string Region { get; set; }

        /// <summary>
        /// 店代码
        /// </summary>
        public string DealerId { get; set; }

        /// <summary>
        /// 积分获取总量
        /// </summary>
        public string RewardPoints { get; set; }

        /// <summary>
        /// 积分兑换总量
        /// </summary>
        public string PointCost { get; set; }

        /// <summary>
        /// 已结算金额
        /// </summary>
        public string SettlementY { get; set; }

        /// <summary>
        /// 未结算金额
        /// </summary>
        public string SettlementN { get; set; }


    }
}
