using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.IRepository;

namespace Vcyber.BLMS.Repository
{
    /// <summary>
    /// 交易流水操作
    /// </summary>
    public class TradeFlowStorager : ITradeFlowStorager
    {
        #region ==== 构造函数 ====

        public TradeFlowStorager() { }

        #endregion

        #region ==== 公共方法 ====

        /// <summary>
        /// 添加交易流水
        /// </summary>
        /// <param name="data"></param>
        public void Add(Tradeflow data, ETradeType type)
        {
            data.TradeType = type.ToInt32();

            StringBuilder sql = new StringBuilder();
            sql.Append(" insert into tradeflow(UserId,FlowCode,TradeType,tradeintegral,trademoney,orderId,rebackId,remark,CreateTime,");
            sql.Append(" UpdateTime,BlueBean)");
            sql.Append(" values(@UserId,@FlowCode,@TradeType,@tradeintegral,@trademoney,@orderId,@rebackId,@remark,@CreateTime,");
            sql.Append(" @UpdateTime,@BlueBean)");
            DbHelp.Execute(sql.ToString(), data);
        }

        /// <summary>
        /// 获取用户交易流水
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IEnumerable<Tradeflow> SelectList(string userId)
        {
            string sql = "select * from tradeflow where UserId=@UserId";
            return DbHelp.Query<Tradeflow>(sql, new { UserId = userId });
        }

        /// <summary>
        /// 获取用户交易流水
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="pageData">分页数据</param>
        /// <param name="total">数据总个数</param>
        /// <returns></returns>
        public IEnumerable<Tradeflow> SelectList(string userId, PageData pageData, out int total)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(" select count(1) from tradeflow where tradeflow.UserId=@UserId");
            total = DbHelp.ExecuteScalar<int>(sql.ToString(), new { UserId = userId });

            sql.Clear();

            sql.AppendFormat("  select top {0} * from tradeflow where tradeflow.UserId=@UserId and tradeflow.ID not in(", pageData.Size);
            sql.AppendFormat(" select top {0} tradeflow.ID from tradeflow where tradeflow.UserId=@UserId ", pageData.Index);
            sql.Append(" order by tradeflow.CreateTime desc) order by tradeflow.CreateTime desc");
            return DbHelp.Query<Tradeflow>(sql.ToString(), new { UserId = userId });
        }

        #endregion
    }
}
