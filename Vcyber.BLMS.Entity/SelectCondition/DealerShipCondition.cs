using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Common;

namespace Vcyber.BLMS.Entity
{
    /// <summary>
    /// 经销商查询条件
    /// </summary>
    public class DealerShipCondition : Condition
    {
        #region ==== 私有字段 ====

        private StringBuilder sql = new StringBuilder(" 1=1 ");

        private bool isLock = false;

        #endregion

        #region ==== 构造函数 ====

        public DealerShipCondition()
        {

        }

        #endregion

        #region ==== 公共属性 ====

        /// <summary>
        /// 经销商表示
        /// </summary>
        public string DealerId { get; set; }

        /// <summary>
        /// 经销商名称
        /// </summary>
        public string DealerName { get; set; }

        public int Index { get; set; }

        public int Size { get; set; }
        public int? Istestserver { get; set; }
        public int? IsDingChe { get; set; }
        public int? IsWeibao { get; set; }
        public int? IsDel { get; set; }


        #endregion

        #region ==== 接口实现 ====

        public string ToWhere()
        {
            if (this.isLock==false)
            {
                if (!string.IsNullOrEmpty(this.DealerId))
                {
                    this.sql.AppendFormat(" and DealerId='{0}'",FilterStr.FilterSql(this.DealerId.Trim()));
                }

                if (!string.IsNullOrEmpty(this.DealerName))
                {
                    this.sql.AppendFormat(" and Name like '%{0}%'", FilterStr.FilterSql(this.DealerName.Trim()));
                }

                if (this.IsDel.HasValue)
                {
                    this.sql.AppendFormat(" and IsDel = '{0}'", FilterStr.FilterSql(this.IsDel.ToString()));
                }
                if (this.Istestserver.HasValue)
                {
                    this.sql.AppendFormat(" and Istestserver = '{0}'", FilterStr.FilterSql(this.Istestserver.ToString()));
                }
                if (this.IsDingChe.HasValue)
                {
                    this.sql.AppendFormat(" and IsDingChe = '{0}'", FilterStr.FilterSql(this.IsDingChe.ToString()));
                }
                if (this.IsWeibao.HasValue)
                {
                    this.sql.AppendFormat(" and IsWeibao = '{0}'", FilterStr.FilterSql(this.IsWeibao.ToString()));
                }

                this.isLock = true;
            }

            return this.sql.ToString();
        }

        #endregion
    }
}
