using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    public class IntegralCountReportInfo
    {
        /// <summary>
        /// 经销商
        /// </summary>
        public string DealerId { get; set; }

        /// <summary>
        /// 办事处
        /// </summary>
        public string Region { get; set; }

        /// <summary>
        /// 区域
        /// </summary>
        public string Area { get; set; }

        /// <summary>
        /// 经销商名称
        /// </summary>
        public string DealerName { get; set; }

        /// <summary>
        /// 积分获取总量
        /// </summary>
        public int RewardPoints { get; set; }

        /// <summary>
        /// 积分兑换总量
        /// </summary>
        public int ConsumePoints { get; set; }

        /// <summary>
        /// 积分兑换金额
        /// </summary>
        public int PointCost { get; set; }
        /// <summary>
        /// 结算开始日期
        /// </summary>
        public string DateStart { get; set; }
        /// <summary>
        /// 结算截止时间
        /// </summary>
        public string DateEnd { get; set; }
        /// <summary>
        /// 日期（结算日期区间）
        /// </summary>
        public string DateString { get; set; }


        /// <summary>
        /// 已结算金额
        /// </summary>
        public int SettlementY { get; set; }

        /// <summary>
        /// 未结算金额
        /// </summary>
        public int SettlementN { get; set; }


        public SettlementState SettlementState { get; set; }
    }
}
