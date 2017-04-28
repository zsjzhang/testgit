using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;

namespace Vcyber.BLMS.IRepository
{
    /// <summary>
    /// 交易流水操作
    /// </summary>
    public interface ITradeFlowStorager
    {
        #region ==== 公共方法 ====

        /// <summary>
        /// 添加交易流水
        /// </summary>
        /// <param name="data"></param>
        void Add(Tradeflow data, ETradeType type);

        /// <summary>
        /// 获取用户交易流水
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        IEnumerable<Tradeflow> SelectList(string userId);

        /// <summary>
        /// 获取用户交易流水
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="pageData">分页数据</param>
        /// <param name="total">数据总个数</param>
        /// <returns></returns>
        IEnumerable<Tradeflow> SelectList(string userId, PageData pageData, out int total);

        #endregion
    }
}
