using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.Entity.SelectCondition;
using Vcyber.BLMS.IRepository;


namespace Vcyber.BLMS.Repository
{
    /// <summary>
    /// 日志操作
    /// </summary>
    public class LogOperatorStorager : ILogOperatorStorager
    {
        #region ==== 构造函数 ====

        public LogOperatorStorager()
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
            StringBuilder sql = new StringBuilder();
            sql.Append("INSERT INTO OperationRecord (Type,SourceId,OperateItem,OriginalValue,CurrentValue,OperateTime,");
            sql.Append("OperaterId,OperaterName,Remark) VALUES(@Type,@SourceId,@OperateItem,@OriginalValue,@CurrentValue,@OperateTime,");
            sql.Append("@OperaterId,@OperaterName,@Remark)");
            data.Type = (int)type;
            data.OperateTime = DateTime.Now;
            DbHelp.Execute(sql.ToString(), data);
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
        public IEnumerable<OperatorLog> Select(string sourceId, string mallCode, ELogType type, int pageIndex, int pageSize, out int totalCount)
        {
            string sql = "SELECT * FROM OperationRecord WHERE SourceId=@SourceId AND Type=@Type ORDER BY OperateTime ASC LIMIT @PageIndex,@PageSize";
            totalCount = DbHelp.ExecuteScalar<int>("Select count(*) from operatorlog WHERE SourceId=@SourceId AND Type=@Type", new { @SourceId = sourceId, @Type = (int)type });
            return DbHelp.Query<OperatorLog>(sql, new { @SourceId = sourceId, @Type = (int)type, @PageIndex = pageIndex, @PageSize = pageSize });
        }

        /// <summary>
        /// 搜索日志操作信息
        /// </summary>
        /// <param name="para">搜索参数</param>
        /// <param name="totalCount">搜索总个数</param>
        /// <returns></returns>
        public IEnumerable<OperatorLog> Select(LogSearchParamater para, out int totalCount)
        {
            StringBuilder countSql = new StringBuilder();
            countSql.Append(" SELECT count(*) FROM OperationRecord ");
            countSql.AppendFormat(" WHERE OperaterName LIKE '%{0}%' AND OperateTime>=@StartTime AND OperateTime<=@EndTime", para.Name);
            totalCount = DbHelp.ExecuteScalar<int>(countSql.ToString(), para);

            StringBuilder sql = new StringBuilder();
            sql.Append(" SELECT * FROM operatorlog ");
            sql.AppendFormat(" WHERE OperaterName LIKE '%{0}%' AND OperateTime>=@StartTime AND OperateTime<=@EndTime", para.Name);
            sql.Append(" ORDER BY operatorlog.OperateTime ASC LIMIT @PageIndex,@PageSize");
            return DbHelp.Query<OperatorLog>(sql.ToString(), para);
        }

        public IEnumerable<OperatorLog> Select(string sourceId, ELogType type, int pageIndex, int pageSize, out int totalCount)
        {
            string sql = "SELECT top {0} * FROM OperationRecord WHERE SourceId ='{2}' and Type={3} and id not in (select top {1} id from OperationRecord WHERE SourceId ='{2}' and Type={3} ORDER BY OperateTime desc) ORDER BY OperateTime desc";
            sql = string.Format(sql, pageSize, pageIndex, sourceId, (int)type);
            var totalsql = string.Format("Select count(*) from OperationRecord WHERE SourceId ='{0}' and Type={1}", sourceId, (int)type);
            totalCount = DbHelp.ExecuteScalar<int>(totalsql);
            return DbHelp.Query<OperatorLog>(sql);

        }

        #endregion
    }
}
