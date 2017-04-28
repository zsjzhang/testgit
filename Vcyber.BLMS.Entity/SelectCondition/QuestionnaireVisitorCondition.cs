using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity.Enum;

namespace Vcyber.BLMS.Entity
{
    public class QuestionnaireVisitorCondition
    {

        #region ==== 私有字段 ====
        private StringBuilder where = new StringBuilder(" 1 = 1");

        private bool isLock = false;
        #endregion

        #region ==== 构造函数 ====

        public QuestionnaireVisitorCondition() { }
        #endregion

        #region ==== 公共属性 ====

        /// <summary>
        /// 调查开始时间
        /// </summary>
        public DateTime? BeginTime { get; set; }

        /// <summary>
        /// 调查结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 是否为前一天
        /// </summary>
        public bool IsPrevDay { get; set; }

        /// <summary>
        /// 问卷id
        /// </summary>
        public int? QuestionnaireId { get; set; }

        /// <summary>
        /// 年龄段
        /// </summary>
        public string Age { get; set; }

        /// <summary>
        /// 学历
        /// </summary>
        public string Education { get; set; }

        /// <summary>
        /// 会员等级
        /// </summary>
        public int? MemberLevel { get; set; }

        public int index { get; set; }

        public int size { get; set; }
        #endregion

        #region ==== 公共方法 ====

        public string ToWhere()
        {
            //if (!this.isLock)
            //{
            this.where = new StringBuilder(" 1 = 1");
            if (this.IsPrevDay)
            {
                this.where.AppendLine(" and DateDiff(dd,v.CreateTime,getdate())=1");
            }
            else
            {
                if (this.BeginTime != null)
                {
                    this.where.AppendFormat(" and v.CreateTime >= '{0}'", this.BeginTime);
                }
                if (this.EndTime != null)
                {
                    this.where.AppendFormat(" and v.CreateTime < '{0}'", DateTime.Parse(this.EndTime.ToString()).AddDays(1));
                }
            }
            if (this.QuestionnaireId != null)
            {
                this.where.AppendFormat(" and v.QuestionnaireId = {0}", this.QuestionnaireId);
            }
            if (!string.IsNullOrEmpty(this.Age))
            {
                this.where.AppendFormat(" and v.Age = '{0}'", this.Age);
            }
            if (!string.IsNullOrEmpty(this.Education))
            {
                this.where.AppendFormat(" and v.Education = '{0}'", this.Education);
            }
            //if (this.MemberLevel != null && this.MemberLevel == Convert.ToInt32(EQuestionnaireMemberLevel.Visitor))
            this.where.AppendLine(" and v.IsMember = 0");

            //}
            //this.isLock = true;
            return this.where.ToString();
        }

        public string ToMemberWhere()
        {
            //if (!this.isLock)
            //{
            where = new StringBuilder(" 1 = 1");
            if (this.IsPrevDay)
            {
                this.where.AppendLine(" and DateDiff(dd,mfqm.CreateTime,getdate())=1");
            }
            else
            {
                if (this.BeginTime != null)
                {
                    this.where.AppendFormat(" and mfqm.CreateTime >= '{0}'", this.BeginTime);
                }
                if (this.EndTime != null)
                {
                    this.where.AppendFormat(" and mfqm.CreateTime < '{0}'", DateTime.Parse(this.EndTime.ToString()).AddDays(1));
                }
            }
            if (this.QuestionnaireId != null)
            {
                this.where.AppendFormat(" and qv.QuestionnaireId = {0}", this.QuestionnaireId);
            }
            if (!string.IsNullOrEmpty(this.Age))
            {
                this.where.AppendFormat(" and qv.Age = '{0}'", this.Age);
            }
            if (!string.IsNullOrEmpty(this.Education))
            {
                this.where.AppendFormat(" and qv.Education = '{0}'", this.Education);
            }
            if (this.MemberLevel != null)
            {
                EQuestionnaireMemberLevel ml = (EQuestionnaireMemberLevel)this.MemberLevel;
                switch (ml)
                {
                    case EQuestionnaireMemberLevel.OneMember:
                        this.where.AppendFormat(" and mfqm.MemberId in (select ms.Id from Membership ms where ms.MLevel = 1 and qv.IsMember = 1)");
                        break;
                    case EQuestionnaireMemberLevel.TwoMember:
                        this.where.AppendFormat(" and mfqm.MemberId in (select ms.Id from Membership ms where ms.MLevel = 2 and ms.[no] is null)");
                        break;
                    case EQuestionnaireMemberLevel.ThreeMember:
                        this.where.AppendFormat(" and mfqm.MemberId in (select ms.Id from Membership ms where ms.MLevel = 3 and ms.[No] is null)");
                        break;
                    case EQuestionnaireMemberLevel.SjMember:
                        this.where.AppendFormat(" and mfqm.MemberId in (select ms.Id from Membership ms where ms.MLevel = 3 and ms.[No] is not null)");
                        break;
                    default: break;
                }
            }
            this.where.AppendLine(" and mfqm.[State] = 2");

            //}
            //this.isLock = true;
            return this.where.ToString();
        }
        #endregion
    }
}
