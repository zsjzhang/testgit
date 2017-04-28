using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    /// <summary>
    /// 服务卡查询条件
    /// </summary>
    public class ServiceCardCondition:Condition
    {
        #region ==== 私有字段 ====

        private StringBuilder sql = new StringBuilder(" 1=1 ");

        private bool isWhere = false;

        #endregion

        #region ==== 构造函数 ====

        public ServiceCardCondition() { }

        #endregion

        #region ==== 公共属性 ====

        /// <summary>
        /// 使用开始时间
        /// </summary>
        public DateTime? UStartTime { get; set; }

        /// <summary>
        /// 使用结束时间
        /// </summary>
        public DateTime? UEndTime { get; set; }

        /// <summary>
        /// 经销商名称
        /// </summary>
        public string DealerName { get; set; }

        /// <summary>
        /// 服务卡状态
        /// </summary>
        public int Status { get; set; }

        public int Index { get; set; }

        public int Size { get; set; }

        #endregion

        #region ==== 公共方法 ====

        public string ToWhere()
        {
            if (this.isWhere==false)
            {

                if (this.Status>0)
                {
                    this.sql.AppendFormat(" and sc.Status={0}",this.Status);
                }

                if (this.UStartTime!=null)
                {
                    this.sql.AppendFormat(" and sc.UseTime>='{0}'", this.UStartTime);
                }

                if (this.UEndTime != null)
                {
                    this.sql.AppendFormat(" and sc.UseTime<='{0}'", this.UEndTime);
                }

                if (!string.IsNullOrEmpty(this.DealerName))
                {
                    this.sql.AppendFormat(" and sc.CardNo in(select scur.CardNo from ServiceCardUsedRecord scur join CS_CarDealerShip dealer on scur.DealerId=dealer.DealerId where dealer.Name like '%{0}%')", this.DealerName);
                }

                this.isWhere = true;
            }

            return this.sql.ToString();
        }

        #endregion

    }
}
