using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.Entity.SelectCondition;

namespace Vcyber.BLMS.IRepository
{
    /// <summary>
    /// 日志操作
    /// </summary>
    public interface ILogOperatorStorager
    {
        #region ==== 公共方法 ====

        /// <summary>
        /// 添加操作日志
        /// </summary>
        /// <param name="data">信息</param>
        /// <param name="type">日志类型</param>
        void Add(OperatorLog data, ELogType type);

        /// <summary>
        /// 获取日志信息
        /// </summary>
        /// <param name="sourceId">日志关联数据源Id</param>
        /// <param name="mallCode">商城编号</param>
        /// <param name="type">日志类型</param>
        /// <param name="pageIndex">分页索引</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="totalCount">数据总个数</param>
        /// <returns></returns>
        IEnumerable<OperatorLog> Select(string sourceId, string mallCode, ELogType type, int pageIndex, int pageSize, out int totalCount);

        /// <summary>
        /// 搜索日志操作信息
        /// </summary>
        /// <param name="para">搜索参数</param>
        /// <param name="totalCount">搜索总个数</param>
        /// <returns></returns>
        IEnumerable<OperatorLog> Select(LogSearchParamater para, out int totalCount);

        IEnumerable<OperatorLog> Select(string sourceId, ELogType type, int pageIndex, int pageSize, out int totalCount);

        #endregion
    }
}
