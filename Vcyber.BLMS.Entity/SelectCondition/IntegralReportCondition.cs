using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    /// <summary>
    /// 会员积分统计条件
    /// </summary>
    public class IntegralReportCondition
    {
        #region ==== 私有字段 ====

        private StringBuilder where = new StringBuilder(" 1=1 ");

        #endregion

        #region ==== 构造函数 ====

        public IntegralReportCondition() { }

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

        public int index { get; set; }

        public int size { get; set; }

        #endregion

        #region ==== 公共方法 ====

        public string ToWhere()
        {
            if (!string.IsNullOrEmpty(this.RealName))
            {
                this.RealName = string.Format("%{0}%", this.RealName);
                this.where.AppendFormat(" and m.RealName like @RealName ");
            }

            if (!string.IsNullOrEmpty(this.Phone))
            {

                this.where.AppendFormat(" and m.PhoneNumber=@Phone ");
            }

            if (!string.IsNullOrEmpty(this.No))
            {
                this.where.AppendFormat(" and m.No=@No ");
            }

            if (this.StartTime != null)
            {
                this.where.AppendFormat(" and m.CreateTime>=@StartTime ");
            }

            if (this.EndTime != null)
            {
                this.EndTime = DateTime.Parse(this.EndTime.ToString()).AddDays(1);
                this.where.AppendFormat(" and m.CreateTime< @EndTime ");
            }

            return this.where.ToString();
        }

        #endregion
    }
}
