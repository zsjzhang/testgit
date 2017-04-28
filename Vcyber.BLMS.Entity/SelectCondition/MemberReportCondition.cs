using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    /// <summary>
    /// 会员报表查询条件
    /// </summary>
    public class MemberReportCondition
    {
        #region ==== 私有字段 ====

        private StringBuilder where = new StringBuilder(" 1=1 ");

        private bool isLock = false;

        #endregion

        #region ==== 构造函数 ====

        public MemberReportCondition() { }

        #endregion

        #region ==== 公共属性 ====


        /// <summary>
        /// 姓名
        /// </summary>
        public string RealName { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 身份证号
        /// </summary>
        public string IdentityNumber { get; set; }

        /// <summary>
        /// 会员等级
        /// </summary>
        public int Mlevel { get; set; }

        /// <summary>
        /// 店代码
        /// </summary>
        public string DearlerId { get; set; }

        /// <summary>
        /// 车种
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        /// 银卡申请状态
        /// </summary>
        public int YKStatus { get; set; }

        /// <summary>
        /// 入会方式
        /// </summary>
        public int InputMode { get; set; }

        /// <summary>
        /// 注册开始时间
        /// </summary>
        public DateTime? RigerStartTime { get; set; }

        /// <summary>
        /// 注册结束时间
        /// </summary>
        public DateTime? RigerEndTime { get; set; }

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

                if (!string.IsNullOrEmpty(this.PhoneNumber))
                {
                    this.where.AppendFormat(" and m.PhoneNumber='{0}' ", this.PhoneNumber);
                }

                if (!string.IsNullOrEmpty(this.IdentityNumber))
                {
                    this.where.AppendFormat(" and m.IdentityNumber='{0}' ", this.IdentityNumber);
                }

                if (!string.IsNullOrEmpty(this.DearlerId) && !this.DearlerId.Equals("0"))
                {
                    this.where.AppendFormat(" and dealer.DealerId='{0}' ", this.DearlerId);
                }

                if (!string.IsNullOrEmpty(this.CategoryName) && !this.CategoryName.Equals("0"))
                {
                    if (CategoryName == "第九代索纳塔")
                        this.where.AppendFormat(" and (car.CarCategory='{0}' or car.CarCategory='索纳塔9') ", this.CategoryName);
                    else if (CategoryName == "第八代索纳塔")
                        this.where.AppendFormat(" and (car.CarCategory='{0}' or car.CarCategory='索纳塔8') ", this.CategoryName);
                    else
                        this.where.AppendFormat(" and (car.CarCategory='{0}') ", this.CategoryName);
                }

                if (this.Mlevel != 0)
                {
                    if (this.Mlevel == 9)
                        this.where.Append(" and m.MLevel=3 and m.No is not null ");
                    else
                        this.where.AppendFormat(" and m.MLevel={0} ", this.Mlevel);
                }

                if (this.YKStatus != 0)
                {
                    if (this.YKStatus != 5)
                    {
                        this.where.AppendFormat(" and (select MAX(MembershipRequest.CreateTime) from MembershipRequest where MembershipRequest.MembershipId=m.Id and MembershipRequest.Status={0}) is not null", this.YKStatus);
                    }
                    else
                    {
                        this.where.Append(" and (select MAX(MembershipRequest.CreateTime) from MembershipRequest where MembershipRequest.MembershipId=m.Id ) is null");
                    }
                }

                if (this.InputMode != 0)
                {
                    if (this.InputMode == 2)
                        this.where.Append(" and m.CreatedPerson='blms_web' ");
                    else if (this.InputMode == 3)
                        this.where.Append(" and m.CreatedPerson='blms_wechat' ");
                    else if (this.InputMode == 4)
                    {
                        this.where.Append(" and LEFT(m.CreatedPerson,1)='D' ");
                    }
                    else
                    {
                        this.where.Append(" and LEFT(m.CreatedPerson,1)!='D' and m.CreatedPerson!='blms_web' and m.CreatedPerson!='blms_wechat' ");
                    }
                }

                if (this.RigerStartTime != null)
                {
                    this.where.AppendFormat(" and  m.CreateTime>='{0}' ", this.RigerStartTime);
                }

                if (this.RigerEndTime != null)
                {
                    this.where.AppendFormat(" and  m.CreateTime<'{0}' ", DateTime.Parse(this.RigerEndTime.ToString()).AddDays(1));
                }

                this.isLock = true;
            }

            return this.where.ToString();
        }

        #endregion
    }
}
