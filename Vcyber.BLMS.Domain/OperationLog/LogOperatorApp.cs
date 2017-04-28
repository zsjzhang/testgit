using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.Entity.SelectCondition;
using Vcyber.BLMS.IRepository;

namespace Vcyber.BLMS.Domain
{
    /// <summary>
    /// 日志操作
    /// </summary>
    public class LogOperatorApp : ILogOperatorApp
    {
        #region ==== 私有字段 ====


        #endregion

        #region ==== 构造函数 ====

        public LogOperatorApp()
        { }

        #endregion

        #region ==== 公共方法 ====

        /// <summary>
        /// 添加操作日志
        /// </summary>
        /// <param name="data">信息</param>
        /// <param name="type">日志类型</param>
        public void Add(OperatorLog data, ELogType type)
        {
            if (data == null)
            {
                throw new NullReferenceException("操作日志信息不能为空！");
            }

            _DbSession.LogStorager.Add(data, type);
        }

        /// <summary>
        /// 获取日志信息
        /// </summary>
        /// <param name="sourceId">日志关联数据源</param>
        /// <param name="mallCode">商城编号</param>
        /// <param name="type">日志类型</param>
        /// <returns></returns>
        public IEnumerable<OperatorLog> GetLogInfo(string sourceId, string mallCode, ELogType type)
        {
            int totalCount;
            return this.GetLogInfo(sourceId, mallCode, type, 1, int.MaxValue, out totalCount);
        }

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
        public IEnumerable<OperatorLog> GetLogInfo(string sourceId, string mallCode, ELogType type, int pageIndex, int pageSize, out int totalCount)
        {
            totalCount = -1;
            return _DbSession.LogStorager.Select(sourceId, mallCode, type, pageIndex, pageSize, out totalCount);
        }

        /// <summary>
        /// 搜索日志操作信息
        /// </summary>
        /// <param name="para">搜索参数</param>
        /// <param name="totalCount">搜索总个数</param>
        /// <returns></returns>
        public IEnumerable<OperatorLog> GetLogInfo(LogSearchParamater para, out int totalCount)
        {
            return _DbSession.LogStorager.Select(para, out totalCount);
        }
        public IEnumerable<OperatorLog> Select(string sourceId, ELogType type, int pageIndex, int pageSize, out int totalCount)
        {
            return _DbSession.LogStorager.Select(sourceId, type, pageIndex, pageSize, out totalCount);
        }

        #endregion
    }
}
