using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Vcyber.BLMS.Application;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.IRepository;

namespace Vcyber.BLMS.Domain
{
    /// <summary>
    /// 交易流水业务
    /// </summary>
    public class TradeFlowApp : ITradeFlowApp
    {
        #region ==== 构造函数 ====

        public TradeFlowApp() { }

        #endregion

        #region ==== 公共方法 ====

        /// <summary>
        /// 添加交易流水
        /// </summary>
        /// <param name="data"></param>
        public void Add(Tradeflow data, ETradeType type)
        {
            _DbSession.TradeFlowStorager.Add(data, type);
        }

        /// <summary>
        /// 获取用户交易流水
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IEnumerable<Tradeflow> GetList(string userId)
        {
            return _DbSession.TradeFlowStorager.SelectList(userId);
        }

        /// <summary>
        /// 获取用户交易流水
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="pageData">分页数据</param>
        /// <param name="total">数据总个数</param>
        /// <returns></returns>
        public IEnumerable<Tradeflow> GetList(string userId, PageData pageData, out int total)
        {
            return _DbSession.TradeFlowStorager.SelectList(userId, pageData, out total);
        }

        #endregion
    }
}
