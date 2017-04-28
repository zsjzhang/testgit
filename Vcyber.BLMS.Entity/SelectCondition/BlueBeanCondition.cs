using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity.Enum;

namespace Vcyber.BLMS.Entity.SelectCondition
{
    /// <summary>
    /// 统计蓝豆获取次数 查询条件
    /// </summary>
    public class BlueBeanCondition
    {
        #region ==== 私有字段 ====

        private StringBuilder where = new StringBuilder(" 1=1 ");

        #endregion

        #region ==== 构造函数 ====

        public BlueBeanCondition(EAcquireMode mode)
        {
            DateTime currentTime = DateTime.Now;

            switch (mode)
            {
                case EAcquireMode.YCX: { this.where.Append(" and 1=1 "); this.StartTime = DateTime.MaxValue; this.EndTime = DateTime.MaxValue; }
                    break;
                case EAcquireMode.MR:
                    {
                        this.StartTime = currentTime.AddHours(-currentTime.Hour).AddMinutes(-currentTime.Minute);
                        this.EndTime = currentTime.AddHours(24 - currentTime.Hour).AddMinutes(60 - currentTime.Minute);
                    }
                    break;
                case EAcquireMode.MY:
                    {
                        this.StartTime = new DateTime(currentTime.Year, currentTime.Month, 1, 0, 0, 0);
                        this.EndTime = new DateTime(currentTime.Year, currentTime.Month, DateTime.DaysInMonth(currentTime.Year, currentTime.Month), 0, 0, 0);
                    }
                    break;
                case EAcquireMode.MC:
                    {
                        this.StartTime = currentTime.AddYears(1);
                        this.EndTime = currentTime.AddYears(1);
                    }
                    break;
                case EAcquireMode.SY:
                    {
                        this.StartTime = currentTime.AddDays(-60).AddHours(-currentTime.Hour).AddMinutes(-currentTime.Minute);
                        this.EndTime = currentTime;
                    }
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region ==== 公共属性 ====

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { get; private set; }


        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; private set; }

        #endregion

        #region ==== 公共方法 ====

        public string ToWhere()
        {
            if (this.EndTime != DateTime.MaxValue && this.StartTime != DateTime.MaxValue)
            {
                this.where.AppendFormat(" and userblueBean.CreateTime>='{0}' and userblueBean.CreateTime<='{1}' ", this.StartTime, this.EndTime);
            }

            return this.where.ToString();
        }

        #endregion
    }
}
