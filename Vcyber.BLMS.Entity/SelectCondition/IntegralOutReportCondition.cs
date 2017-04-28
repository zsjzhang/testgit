using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    /// <summary>
    /// 会员消耗积分报表条件
    /// </summary>
    public class IntegralOutReportCondition
    {
        #region ==== 私有字段 ====

        private StringBuilder where = new StringBuilder(" 1=1 ");

        private bool isLock = false;

        #endregion

        #region ==== 构造函数 ====

        public IntegralOutReportCondition() { }

        #endregion

        #region ==== 公共属性 ====

        /// <summary>
        /// 会员名称
        /// </summary>
        public string RealName { get; set; }

        /// <summary>
        /// 会员卡号
        /// </summary>
        public string No { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 经销商区域
        /// </summary>
        public string Area { get; set; }

        /// <summary>
        /// 店代码
        /// </summary>
        public string DealerId { get; set; }

        /// <summary>
        /// 订单形式
        /// </summary>
        public int OrderMode { get; set; }

        public int index { get; set; }

        public int size { get; set; }

        #endregion

        #region ==== 公共方法 ====

        public string ToWhere()
        {
            if (!this.isLock)
            {
                if (!string.IsNullOrEmpty(this.RealName))
                {
                    this.where.AppendFormat(" and m.RealName like '%{0}%' ", this.RealName.Trim());
                }

                if (!string.IsNullOrEmpty(this.Phone))
                {
                    this.where.AppendFormat(" and m.PhoneNumber='{0}' ", this.Phone.Trim());
                }

                if (!string.IsNullOrEmpty(this.No))
                {
                    this.where.AppendFormat(" and m.No='{0}' ", this.No.Trim());
                }

                if (this.StartTime != null)
                {
                    this.where.AppendFormat(" and o.CreateTime>='{0}' ", this.StartTime);
                }

                if (this.EndTime != null)
                {
                    this.where.AppendFormat(" and o.CreateTime<'{0}' ", DateTime.Parse(this.EndTime.ToString()).AddDays(1));
                }

                if (!string.IsNullOrEmpty(this.Area))
                {
                    this.where.AppendFormat(" and dealer.Area='{0}' ", this.Area.Trim());
                }

                if (!string.IsNullOrEmpty(this.DealerId))
                {
                    this.where.AppendFormat(" and dealer.DealerId like '%{0}%' ", this.DealerId.Trim());
                }

                if (this.OrderMode != -1 && this.OrderMode != 0)
                {
                    this.where.AppendFormat(" and o.Mode={0} ", this.OrderMode);
                }

                this.isLock = true;
            }

            return this.where.ToString();
        }

        #endregion
    }
}
