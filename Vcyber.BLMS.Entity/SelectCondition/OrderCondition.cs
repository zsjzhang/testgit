using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Vcyber.BLMS.Entity.Enum;

namespace Vcyber.BLMS.Entity.SelectCondition
{
    /// <summary>
    /// 订单查询条件
    /// </summary>
    public class OrderCondition
    {
        #region ==== 构造函数 ====

        public OrderCondition() { }

        public OrderCondition(string oid, string phone, string startTime, string endTime, int tradeState = -1)
        {
            this.OrderID = oid;

            this.Phone = phone;

            DateTime value;

            if (!DateTime.TryParse(startTime, out value))
            {
                this.StartTime = null;
            }
            else
            {
                this.StartTime = value;
            }

            DateTime value2;

            if (!DateTime.TryParse(endTime, out value2))
            {
                this.EndTime = null;
            }
            else
            {
                this.EndTime = value2;
            }

            if (tradeState == -1)
            {
                this.TradeState = null;
            }
            else
            {
                this.TradeState = (ETradeState)tradeState;
            }
        }

        #endregion

        #region ==== 公共属性 ====

        /// <summary>
        /// 订单Id
        /// </summary>
        public string OrderID { get; set; }

        /// <summary>
        /// 下单开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 下单结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 交易类型
        /// </summary>
        public ETradeState? TradeState { get; set; }

        public string Phone { get; set; }

        #endregion

        #region ==== 公共方法 ====

        public string ToWhere()
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(" 1=1 ");

            if (!string.IsNullOrEmpty(this.OrderID))
            {
                sql.AppendFormat(" and orders.OrderID like '%{0}%' ",this.OrderID);
            }

            if (!string.IsNullOrEmpty(this.Phone))
            {
                sql.AppendFormat(" and orders.UserId = (select id from membership where username = '{0}')", this.Phone);
            }

            if (this.StartTime!=null)
            {
                sql.AppendFormat(" and orders.Createtime>='{0}' ",this.StartTime);
            }

            if (this.EndTime!=null)
            {
                sql.AppendFormat(" and orders.Createtime<='{0}' ",this.EndTime);
            }

            if (this.TradeState!=null)
            {
                sql.AppendFormat(" and orders.TradeState={0} ",((ETradeState)this.TradeState).ToInt32());
            }

            return sql.ToString();
        }

        #endregion
    }
}
