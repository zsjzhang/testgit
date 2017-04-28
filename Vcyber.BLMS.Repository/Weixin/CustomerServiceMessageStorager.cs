using PetaPoco;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.IRepository.Weixin;
using Vcyber.BLMS.Repository.Entity.Generated;

namespace Vcyber.BLMS.Repository.Weixin
{
    public class CustomerServiceMessageStorager : ICustomerServiceMessageStorager
    {
        /// <summary>
        /// 添加客户消息
        /// </summary>
        /// <param name="obj">客服消息</param>
        /// <returns>Id</returns>
        public int Add(CustomerServiceMessage obj)
        {
            var sql = Sql.Builder
                .Append("INSERT INTO dbo.WX_CustomerServiceMessage(Worker,OpenId,OperCode,Message,OperTime,CreateTime,Timestamp)")
                .Append("VALUES(@0,@1,@2,@3,@4,GETDATE(),@5)", obj.worker, obj.openid, obj.opercode, obj.text, obj.opertime, obj.timestamp);
            var id = (int)PocoHelper.CurrentDb().Execute(sql);
            return id;
        }
        /// <summary>
        /// 按时间查询并且按客服分组
        /// </summary>
        /// <param name="beginTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns>汇总记录</returns>
        public IEnumerable<CustomerServiceRecord> GroupByWorker(string beginTime, string endTime)
        {
            var sql = Sql.Builder
                .Append("SELECT Worker,SUM(ReceiveCount) AS ReceiveCount,SUM(ReplyCount) AS ReplyCount,SUM(ReceivePersons) AS ReceivePersons,")
                .Append("SUM(ReplyPersons) AS ReplyPersons,AVG(isnull(AvgCount,0)) AS Mins,")
                .Append("(SELECT TOP 1 Mins FROM dbo.WX_CustomerServiceRecord AS wcsr1 WHERE wcsr1.CreateDate = MIN(wcsr.CreateDate) AND wcsr1.Worker = wcsr.Worker) AS FirstMins,")
                .Append(string.Format("'{0}-{1}' AS BetweenTime FROM dbo.WX_CustomerServiceRecord AS wcsr", beginTime, endTime))
                .Append("WHERE CreateDate BETWEEN @0 AND @1 GROUP BY Worker", beginTime, endTime);
            return PocoHelper.CurrentDb().Query<CustomerServiceRecord>(sql);
        }
        /// <summary>
        /// 按时间汇总
        /// </summary>
        /// <param name="beginTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns></returns>
        public DataTable Total(string beginTime, string endTime)
        {
            //var sql = Sql.Builder
            //    .Append("SELECT SUM(ReceiveCount) AS ReceiveCount,SUM(ReplyCount) AS ReplyCount,SUM(ReceivePersons) AS ReceivePersons,")
            //    .Append(string.Format("SUM(ReplyPersons) AS ReplyPersons,SUM(Mins)/COUNT(0) AS Mins,'{0}-{1}' AS BetweenTime FROM dbo.WX_CustomerServiceRecord AS wcsr",beginTime,endTime))
            //    .Append("WHERE CreateDate BETWEEN @0 AND @1", beginTime, endTime);
            //return PocoHelper.CurrentDb().Query<CustomerServiceRecord>(sql);
            //return PocoHelper.CurrentDb().ExecuteScalar<DataTable>("EXEC WX_test1 @beginTime,@endTime", new { @beginTime = beginTime, @endTime = endTime });
            DBHelper dbHelper = new DBHelper();
            SqlParameter[] paramsArray = new SqlParameter[2] { new SqlParameter("@FromDate", beginTime), new SqlParameter("@EndDate", endTime) };
            var data = dbHelper.ExecDataSetProc("WX_PressureAnalysis", paramsArray);
            return data.Tables[0];

            //return PocoHelper.CurrentDb().ExecuteTable("EXEC WX_test1 @beginTime,@endTime", 6, new { @beginTime = beginTime, @endTime = endTime });
        }
        /// <summary>
        /// 生成统计汇总
        /// </summary>
        /// <param name="currDate">记录的时间</param>
        public void AddRecord(string currDate = "") 
        {
            if(string.IsNullOrEmpty(currDate))
            {
                currDate = DateTime.Now.Date.ToString();
            }
            PocoHelper.CurrentDb().Execute("EXEC WX_CustomerServiceTotal @0",currDate);
        }

        /// <summary>
        /// 微信多客服绩效统计
        /// </summary>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public DataTable PerformanceStatisticsList(string beginTime, string endTime)
        {
            DBHelper dbHelper = new DBHelper();
            SqlParameter[] paramsArray = new SqlParameter[2] { new SqlParameter("@FromDate", beginTime), new SqlParameter("@EndDate", endTime) };
            var data = dbHelper.ExecDataSetProc("WX_PerformanceStatistics", paramsArray);
            return data.Tables[0];
        }
    }
}
