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
    public class ServiceCardStorager : IServiceCardStorager
    {
        public IEnumerable<ServiceCard> SelectServiceCardList()
        {
            string sql = "SELECT * FROM ServiceCard";
            return DbHelp.Query<ServiceCard>(sql);
        }

        public bool AddServiceCard(ServiceCard card)
        {
            string sql = @"INSERT INTO ServiceCard(BatchNo,CardNo,Status,UserId,SendTime,UseTime,CreateTime) 
                            VALUES(@BatchNo,@CardNo,@Status,@UserId,@SendTime,@UseTime,GETDATE())";

            return DbHelp.Execute(sql, new
            {
                @BatchNo = card.BatchNo,
                @CardNo = card.CardNo,
                @Status = card.Status,
                @UserId = card.UserId,
                @SendTime = card.SendTime,
                @UseTime = card.UseTime
            }) > 0;
        }

        public string GetServiceCardNo()
        {
            string sql = "SELECT dbo.GetServiceCardNo()";

            return DbHelp.ExecuteScalar<string>(sql);
        }

        public bool SendServiceCard(string userId, string cardNo)
        {
            string sql = "UPDATE ServiceCard SET STATUS = 2,UserId = @UserId,SendTime = GETDATE() WHERE CardNo = @CardNo";

            return DbHelp.Execute(sql, new { @UserId = userId, @CardNo = cardNo }) > 0;
        }

        public bool UseServiceCard(string cardNo)
        {
            string sql = "UPDATE ServiceCard SET STATUS = 3,UseTime = GETDATE() WHERE CardNo = @CardNo";

            return DbHelp.Execute(sql, new { @CardNo = cardNo }) > 0;
        }

        public IEnumerable<ServiceCard> GetServiceCard(string batchNo, int qty)
        {
            string sql = "SELECT TOP @Qty * FROM ServiceCard WHERE STATUS = 1 AND BatchNo = @BatchNo";

            return DbHelp.Query<ServiceCard>(sql, new { @Qty = qty, @BatchNo = batchNo });
        }

        /// <summary>
        /// 查询服务卡
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="pageData"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public IEnumerable<ServiceCard> FindList(Condition condition, PageData pageData, out int totalCount)
        {
            StringBuilder sql = new StringBuilder();

            sql.AppendFormat(" select COUNT(1) from ServiceCard sc where {0}", condition.ToWhere());
            totalCount = DbHelp.ExecuteScalar<int>(sql.ToString());

            sql.Clear();

            sql.AppendFormat(" select top {0} scb.BatchName,scb.BatchNo,scb.BatchPrice,scb.ValidateTime,sc.*", pageData.Size);
            sql.Append(" from ServiceCard sc join ServiceCardBatch scb on sc.BatchNo=scb.BatchNo");
            sql.AppendFormat(" where {0} and sc.CardId not in(", condition.ToWhere());
            sql.AppendFormat(" select top {0} sc.CardId from ServiceCard sc ", pageData.Index);
            sql.AppendFormat(" where {0}", condition.ToWhere());
            sql.Append("  order by sc.CreateTime desc)");
            sql.Append(" order by sc.CreateTime desc");

            return DbHelp.Query<ServiceCard>(sql.ToString());
        }

        /// <summary>
        /// 查询服务卡信息
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="maxValue">查询最新服务卡的个数</param>
        /// <returns></returns>
        public IEnumerable<ServiceCard> FindList(Condition condition, int maxValue)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat(" select top {0} scb.BatchName,scb.BatchNo,scb.BatchPrice,scb.ValidateTime,sc.*", maxValue);
            sql.Append(" from ServiceCard sc join ServiceCardBatch scb on sc.BatchNo=scb.BatchNo");
            sql.AppendFormat(" where {0}", condition.ToWhere());
            sql.Append(" order by sc.CreateTime desc");
            return DbHelp.Query<ServiceCard>(sql.ToString());
        }

        /// <summary>
        /// 根据卡卷编号查询卡卷信息
        /// </summary>
        /// <param name="cardNo">卡卷编号</param>
        /// <returns></returns>
        public ServiceCard FindOne(string cardNo)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(" select sc.*,scb.ValidateTime,sct.TypeCode,sct.TypeName");
            sql.Append(" from ServiceCard sc join ServiceCardBatch scb on sc.BatchNo=scb.BatchNo ");
            sql.Append(" join ServiceCardType sct on scb.TypeCode=sct.TypeCode");
            sql.Append(" where sc.CardNo=@CardNo");
            return DbHelp.QueryOne<ServiceCard>(sql.ToString(), new { CardNo=cardNo });
        }

        public IEnumerable<ServiceCard> SelectServiceCardList(ServiceCardSearchCondition condition)
        {
            var sql = "SELECT * FROM ServiceCard ";

            return DbHelp.Query<ServiceCard>(sql);
        }
    }
}
