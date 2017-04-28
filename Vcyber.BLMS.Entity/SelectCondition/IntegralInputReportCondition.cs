using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    /// <summary>
    /// 会员获取积分报表
    /// </summary>
    public class IntegralInputReportCondition
    {
        #region ==== 私有字段 ====

        private StringBuilder where = new StringBuilder(" 1=1 ");

        private bool isLock = false;

        #endregion

        #region ==== 构造函数 ====

        public IntegralInputReportCondition() { }

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

        /// <summary>
        /// 积分来源（方式）
        /// </summary>
        public string IntegralSource { get; set; }

        public string UserType { get; set; }

        public int index { get; set; }

        public int size { get; set; }

        #endregion

        #region ==== 公共方法 ====

        public string ToWhere()
        {
            if (!this.isLock)
            {
                if (!string.IsNullOrEmpty(this.UserType))
                {
                    this.where.AppendFormat(" and m.UserType = @UserType");
                }

                if (!string.IsNullOrEmpty(this.RealName))
                {
                    this.RealName = string.Format("%{0}%", this.RealName.Trim());
                    this.where.AppendFormat(" and m.NickName like @RealName");
                }

                if (!string.IsNullOrEmpty(this.Phone))
                {
                    this.Phone = this.Phone.Trim();
                    this.where.AppendFormat(" and m.PhoneNumber=@Phone");
                }

                if (!string.IsNullOrEmpty(this.No))
                {
                    this.No = this.No.Trim();
                    this.where.AppendFormat(" and m.No=@No");
                }

                if (this.StartTime != null)
                {
                    this.where.AppendFormat(" and integral.CreateTime>=@StartTime ");
                }

                if (this.EndTime != null)
                {
                    this.EndTime = DateTime.Parse(this.EndTime.ToString()).AddDays(1);
                    this.where.AppendFormat(" and integral.CreateTime<@EndTime");
                }

                if (!string.IsNullOrEmpty(this.Region))
                {
                    this.Region = this.Region.Trim();
                    this.where.AppendFormat(" and dealer.Region=@Region");
                }

                if (!string.IsNullOrEmpty(this.Area))
                {
                    this.Area = this.Area.Trim();
                    this.where.AppendFormat(" and dealer.Area=@Area");
                }

                if (!string.IsNullOrEmpty(this.DealerId))
                {
                    this.DealerId = string.Format("%{0}%", this.DealerId.Trim());
                    this.where.AppendFormat(" and dealer.DealerId like @DealerId ");
                }

                if (!string.IsNullOrEmpty(this.IntegralSource) && !this.IntegralSource.Equals("-1"))
                {
                    this.where.AppendFormat(" and integral.integralSource=@IntegralSource");
                }

                this.isLock = true;
            }

            return this.where.ToString();
        }


        public string ToWhere1()
        {
            if (!this.isLock)
            {
                if (!string.IsNullOrEmpty(this.UserType))
                {
                    this.where.AppendFormat(" and m.MLevel = @UserType");
                }

                if (!string.IsNullOrEmpty(this.RealName))
                {
                    this.RealName = string.Format("%{0}%", this.RealName.Trim());
                    this.where.AppendFormat(" and m.NickName like @RealName");
                }

                if (!string.IsNullOrEmpty(this.Phone))
                {
                    this.Phone = this.Phone.Trim();
                    this.where.AppendFormat(" and m.PhoneNumber=@Phone");
                }

                if (!string.IsNullOrEmpty(this.No))
                {
                    this.No = this.No.Trim();
                    this.where.AppendFormat(" and m.No=@No");
                }

                if (this.StartTime != null)
                {
                    this.where.AppendFormat(" and integral.CreateTime>=@StartTime ");
                }

                if (this.EndTime != null)
                {
                    this.EndTime = DateTime.Parse(this.EndTime.ToString()).AddDays(1);
                    this.where.AppendFormat(" and integral.CreateTime<@EndTime");
                }

                if (!string.IsNullOrEmpty(this.Region))
                {
                    this.Region = this.Region.Trim();
                    this.where.AppendFormat(" and dealer.Region=@Region");
                }

                if (!string.IsNullOrEmpty(this.Area))
                {
                    this.Area = this.Area.Trim();
                    this.where.AppendFormat(" and dealer.Area=@Area");
                }

                if (!string.IsNullOrEmpty(this.DealerId))
                {
                    this.DealerId = string.Format("%{0}%", this.DealerId.Trim());
                    this.where.AppendFormat(" and dealer.DealerId like @DealerId ");
                }

                if (!string.IsNullOrEmpty(this.IntegralSource) && !this.IntegralSource.Equals("-1"))
                {
                    this.where.AppendFormat(" and integral.integralSource=@IntegralSource");
                }

                this.isLock = true;
            }

            return this.where.ToString();
        }

        #endregion
    }
}
