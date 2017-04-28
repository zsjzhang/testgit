using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.IRepository;

namespace Vcyber.BLMS.Repository
{
    public class ServiceCardBatchStorager : IServiceCardBatchStorager
    {
        public IEnumerable<ServiceCardBatch> SelectServiceCardBatchList()
        {
            string sql = "SELECT * FROM ServiceCardBatch";

            return DbHelp.Query<ServiceCardBatch>(sql);
        }

        /// <summary>
        /// 是存在相同的卡卷名称
        /// </summary>
        /// <param name="batchName"></param>
        /// <returns></returns>
        public bool IsExist(string batchName)
        {
            string sql = "select COUNT(1) from ServiceCardBatch where BatchName=@Name";
            return DbHelp.ExecuteScalar<int>(sql, new { Name = batchName }) > 0;
        }

        public bool AddServiceCardBatch(ServiceCardBatch batch)
        {
            string sql = @"INSERT INTO ServiceCardBatch(TypeCode,BatchNo,BatchName,BatchQty,BatchPrice,BatchTotalMoney
                                ,CreateTime,ValidateTime,ConsumeMoney,DearlerId) 
                            VALUES(@TypeCode,@BatchNo,@BatchName,@BatchQty,@BatchPrice,@BatchTotalMoney
                                ,getdate(),@ValidateTime,@ConsumeMoney,@DearlerId)";

            return DbHelp.Execute(sql, new
            {
                @TypeCode = batch.TypeCode,
                @BatchNo = batch.BatchNo,
                @BatchName = batch.BatchName,
                @BatchQty = batch.BatchQty,
                @BatchPrice = batch.BatchPrice,
                @BatchTotalMoney = batch.BatchTotalMoney,
                @ValidateTime = batch.ValidateTime,
                @ConsumeMoney = batch.ConsumeMoney,
                @DearlerId = batch.DearlerId
            }) > 0;
        }

        public string GetServiceCardBatchNo()
        {
            string sql = "SELECT dbo.GetServiceCardBatchNo()";

            return DbHelp.ExecuteScalar<string>(sql);
        }

        /// <summary>
        /// 服务卡使用情况统计
        /// </summary>
        /// <param name="condition">统计条件</param>
        /// <returns></returns>
        public ServiceCarUseStatistics serviceCardStatistics(Condition condition)
        {
            StringBuilder sql = new StringBuilder();

            sql.Append(" select sum(case when tempTable.Status=1 then 1 else 0 end) as WF,");
            sql.Append(" sum(case when tempTable.Status=2 then 1 else 0 end) as YF,");
            sql.Append(" sum(case when tempTable.Status=3 then 1 else 0 end) as YHX,");
            sql.Append(" sum(case when tempTable.Status=4 then 1 else 0 end) as YJS");
            sql.Append(" from (");
            sql.Append(" select sc.Status from ServiceCard sc");
            sql.Append(" where sc.BatchNo in(select scb.BatchNo");
            sql.Append(" from ServiceCardBatch scb ");
            sql.AppendFormat(" where {0}", condition.ToWhere());
            sql.Append(" ) ) as tempTable");

            return DbHelp.QueryOne<ServiceCarUseStatistics>(sql.ToString());
        }

        /// <summary>
        /// 查询服务卡批次信息
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="pageData">分页信息</param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public IEnumerable<ServiceCardBatch> findCondition(Condition condition, PageData pageData, out int totalCount)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(" select COUNT(1) from ServiceCardBatch scb");
            sql.AppendFormat(" where {0}", condition.ToWhere());
            totalCount = DbHelp.ExecuteScalar<int>(sql.ToString());

            sql.Clear();

            sql.AppendFormat(" select top {0} * from ServiceCardBatch scb ", pageData.Size);
            sql.AppendFormat(" where {0}", condition.ToWhere());
            sql.AppendFormat(" and scb.BatchId not in(select top {0} scb.BatchId ", pageData.Index);
            sql.Append(" from ServiceCardBatch scb  ");
            sql.AppendFormat(" where {0}", condition.ToWhere());
            sql.Append(" order by scb.CreateTime desc) ");
            sql.Append(" order by scb.CreateTime desc ");
            return DbHelp.Query<ServiceCardBatch>(sql.ToString());
        }

        /// <summary>
        /// 查询服务卡批次信息
        /// </summary>
        /// <param name="batchNo">批次编号</param>
        /// <returns></returns>
        public ServiceCardBatch FindBatchOne(string batchNo)
        {
            string sql = "select * from ServiceCardBatch where BatchNo=@BatchNo";
            return DbHelp.QueryOne<ServiceCardBatch>(sql, new { BatchNo=batchNo });
        }

        #region ==== 服务卡类型 ====

        /// <summary>
        /// 获取服务卡类型
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ServiceCardType> CardTypeAll()
        {
            string sql = "select * from ServiceCardType";
            return DbHelp.Query<ServiceCardType>(sql);
        }

        /// <summary>
        /// 根据服务卡类型Code,查找服务卡类型
        /// </summary>
        /// <param name="typeCode"></param>
        /// <returns></returns>
        public ServiceCardType CardTypeOne(string typeCode)
        {
            string sql = "select * from ServiceCardType where ServiceCardType.TypeCode=@Code";
            return DbHelp.QueryOne<ServiceCardType>(sql, new { Code=typeCode });
        }

        #endregion
    }
}
