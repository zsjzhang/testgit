using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    /// <summary>
    /// 服务卡批次查询条件
    /// </summary>
    public class ServiceCarBatchCondition : Condition
    {
        #region ==== 私有方法 ====

        private StringBuilder sql = new StringBuilder(" 1=1 ");

        private bool isWhere = false;

        #endregion

        #region ==== 构造函数 ====

        public ServiceCarBatchCondition() { }

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
        /// 服务卡批次号
        /// </summary>
        public string BatchNo { get; set; }

        public int Index { get; set; }

        public int Size { get; set; }

        #endregion

        #region ==== 公共方法 ===

        public string ToWhere()
        {
            if (this.isWhere==false)
            {
                if (!string.IsNullOrEmpty(this.BatchNo))
                {
                    this.sql.AppendFormat(" and scb.BatchNo='{0}' ", this.BatchNo);
                }

                if (this.StartTime != null)
                {
                    this.sql.AppendFormat(" and scb.CreateTime>='{0}'", this.StartTime);
                }

                if (this.EndTime != null)
                {
                    this.sql.AppendFormat(" and scb.CreateTime<='{0}'", this.EndTime);
                }

                this.isWhere = true;
            }

            return this.sql.ToString();
        }

        #endregion
    }
}
