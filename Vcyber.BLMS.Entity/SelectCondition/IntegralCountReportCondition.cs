using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity.Enum;

namespace Vcyber.BLMS.Entity
{
    /// <summary>
    /// 会员获取积分报表
    /// </summary>
    public class IntegralCountReportCondition
    {
        #region ==== 私有字段 ====

        private StringBuilder where = new StringBuilder(" 1=1 ");

        private bool isLock = false;

        private int defaultConsumeType = -1;

        #endregion

        #region ==== 构造函数 ====

        public IntegralCountReportCondition() { }

        #endregion

        #region ==== 公共属性 ====

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 办事处
        /// </summary>
        public string Region { get; set; }

        /// <summary>
        /// 经销商区域
        /// </summary>
        public string Area { get; set; }

        /// <summary>
        /// 店代码
        /// </summary>
        public string DealerId { get; set; }

        public int ApproveStatus { get; set; }

        public int SettlementState { get; set; }

        /// <summary>
        /// 消费类型（0：维修，1：保养，2：购车）
        /// </summary>
        public int ConsumeType
        {
            get { return defaultConsumeType; }
            set { defaultConsumeType = value; }
        }

        public int index { get; set; }

        public int size { get; set; }

        #endregion

        #region ==== 公共方法 ====

        public string ToWhere()
        {
            if (!this.isLock)
            {
                if (this.StartTime != null)
                {
                    this.where.AppendFormat(" and c.ConsumeDate >=@StartTime ");
                }

                if (this.EndTime != null)
                {
                    this.EndTime = DateTime.Parse(this.EndTime.ToString()).AddMonths(1).AddSeconds(-1);
                    this.where.AppendFormat(" and c.ConsumeDate <=@EndTime");
                }

                if (this.ConsumeType > -1)
                {
                    this.where.AppendFormat(" and c.ConsumeType =@ConsumeType ");
                }

                if (!string.IsNullOrEmpty(this.Region))
                {
                    this.Region = Region.Trim();
                    this.where.AppendFormat(" and d.region =@Region ");
                }

                if (!string.IsNullOrEmpty(this.Area))
                {
                    this.Area = this.Area.Trim();
                    this.where.AppendFormat(" and d.area =@Area ");
                }

                if (!string.IsNullOrEmpty(this.DealerId))
                {
                    this.DealerId = string.Format("%{0}%", this.DealerId);
                    this.where.AppendFormat(" and ( d.DealerId like @DealerId )");
                }

                //if (this.ApproveStatus > -1)
                //{
                //新增消费记录默认是自动审核，so，查询的时候条件也是自动审核
                //this.where.AppendFormat(" and c.ApproveStatus ='{0}' ", (int)EPointApproveStatus.AutoApproved);
                this.where.AppendFormat(" and c.ApproveStatus in('1','2')");
                // }

                if (this.SettlementState > 0)
                {
                    this.where.AppendFormat(" and c.SettlementState =@SettlementState");
                }

                this.isLock = true;
            }

            return this.where.ToString();
        }
        /// <summary>
        /// 经销商查询条件
        /// </summary>
        /// <returns></returns>
        public string ToDealerWhere()
        {
            if (!this.isLock)
            {
                if (this.StartTime != null)
                {
                    this.where.AppendFormat(" and c.DateStart =@StartTime ");
                }

                if (this.EndTime != null)
                {
                    this.EndTime = DateTime.Parse(this.EndTime.ToString()).AddMonths(1).AddSeconds(-1);
                    this.where.AppendFormat(" and c.DateEnd =@EndTime");
                }
                if (this.ConsumeType > -1)
                {
                    this.where.AppendFormat(" and c.ConsumeType =@ConsumeType ");
                }
                if (!string.IsNullOrEmpty(this.Region))
                {
                    this.Region = Region.Trim();
                    this.where.AppendFormat(" and c.region =@Region ");
                }

                if (!string.IsNullOrEmpty(this.Area))
                {
                    this.Area = this.Area.Trim();
                    this.where.AppendFormat(" and c.area =@Area ");
                }

                if (!string.IsNullOrEmpty(this.DealerId))
                {
                    this.DealerId = string.Format("%{0}%", this.DealerId);
                    this.where.AppendFormat(" and ( c.DealerId like @DealerId )");
                }

                //if (this.ApproveStatus > -1)
                //{
                //新增消费记录默认是自动审核，so，查询的时候条件也是自动审核
                //this.where.AppendFormat(" and c.ApproveStatus ='{0}' ", (int)EPointApproveStatus.AutoApproved);
                // }

                if (this.SettlementState > 0)
                {
                    this.where.AppendFormat(" and c.SettlementState =@SettlementState");
                }

                this.isLock = true;
            }

            return this.where.ToString();
        }

        #endregion
    }
}
