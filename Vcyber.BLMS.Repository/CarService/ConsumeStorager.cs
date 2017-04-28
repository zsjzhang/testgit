using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Common;

namespace Vcyber.BLMS.Repository.CarService
{
    using PetaPoco;
    using Vcyber.BLMS.Entity;
    using Vcyber.BLMS.Entity.CarService;
    using Vcyber.BLMS.Entity.Enum;
    using Vcyber.BLMS.Entity.Generated;
    using Vcyber.BLMS.IRepository;
    using Vcyber.BLMS.IRepository.CarService;
    using Vcyber.BLMS.Repository.Entity.Generated;

    public class ConsumeStorager : IConsumeStorager
    {
        public int Add(CSConsume entity)
        {
            return (int)PocoHelper.CurrentDb().Insert(entity);
        }

        public Page<CSConsume> QueryOrders(ConsumeQueryParamEntity entity, long page, long itemsPerPage)
        {
            return PocoHelper.CurrentDb().Page<CSConsume>(page, itemsPerPage, CSSqlBuilder.BuildSql4Consume(entity));
        }


        public IEnumerable<CSConsume> GetUserConsume(string userId, int pageIndex, int pageSize, out int total)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(" select count(1) from  CS_Consume   where userId=@userId  and ( ConsumeType=3 or ConsumeType=1 )");
            total = DbHelp.ExecuteScalar<int>(sql.ToString(), new { userId = userId });
            sql.Clear();
            sql.AppendFormat(@" select * from ( select  * ,  ROW_NUMBER ()  over ( order by  CreateTime  desc )   as  rows  from  CS_Consume   where userId =@userID  and (ConsumeType=3 or ConsumeType=1 )) page
   where      page.rows  >= {0}  and  page .rows <=  {1} ", (pageIndex - 1) * pageSize + 1, pageIndex * pageSize);
            return DbHelp.Query<CSConsume>(sql.ToString(), new { userId = userId });
        
              
        }

        public int UpdateStatus(int id, EPointApproveStatus status, string updateId, string updateName)
        {
            return PocoHelper.CurrentDb().Update<CSConsume>("set ApproveStatus=@0,UpdateId=@1, UpdateName=@2, UpdateTime=@3 where Id=@4", status, updateId, updateName, DateTime.Now, id);
        }


        public int UpdatePointStatus(int id, EPointStatus status, string updateId, string updateName)
        {
            return PocoHelper.CurrentDb().Update<CSConsume>("set PointStatus=@0,UpdateId=@1, UpdateName=@2, UpdateTime=@3 where Id=@4", status, updateId, updateName, DateTime.Now, id);
        }


        public CSConsume GetById(int id)
        {
            return PocoHelper.CurrentDb().Single<CSConsume>(id);
        }

        public int UpdatePaperOrder(int id, string paperOrder)
        {
            return PocoHelper.CurrentDb().Update<CSConsume>("set PaperOrder=@0 where id=@1", paperOrder, id);
        }

        public int UpdateConsume(CSConsume entity)
        {
            return PocoHelper.CurrentDb().Update(entity);
        }

        public void ReleasePoints()
        {
            //如果审核通过或者未审核时间超过24小时，并且上传了附件，则自动发放积分，同时更新积分发放状态
            throw new NotImplementedException();
        }

        public int BatchUpdateStatus(string ids, EPointApproveStatus status, string updateId, string updateName)
        {
            string[] idList = ids.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (idList.Length > 0)
            {
                //PocoHelper.CurrentDb().BeginTransaction();
                int updateCount = 0;
                if (status == EPointApproveStatus.Approved)
                {
                    BMDb db = BMDb.GetInstance();
                    using (var scope = db.GetTransaction())
                    {

                        updateCount = db.Execute(@"
update cs_consume
set ApproveStatus=@0,
    UpdateId=@1,
    UpdateName=@2, 
    UpdateTime=getdate(),
    /*RewardPoints = round((PartCost+MaterialCost-PointCost)TotalCost)*0.1,1),*/
    PointStatus = 1
where Id in (@3);

/*insert into userintegral
	(userId,integralSource,value,usevalue,datastate,remark,CreateTime)
select  UserId, '1', RewardPoints, 0, 0, '', getdate()
from cs_consume
where RewardPoints>0 and Id in (@3)*/;
", status, updateId, updateName, idList);
                        scope.Complete();
                    }
                    return updateCount;
                }

                return PocoHelper.CurrentDb().Execute(@"
update cs_consume
set ApproveStatus=@0,
    UpdateId=@1,
    UpdateName=@2, 
    UpdateTime=getdate()
where Id in (@3);
", status, updateId, updateName, idList);
            }
            return 0;
        }

        public void AddAndProcess(string userId, int id)
        {
          
            double discount=_DbSession.UserIntegralStorager.GetUserIntegralDiscount(userId);
            BMDb db = BMDb.GetInstance();
            using (var scope = db.GetTransaction())
            {

                db.Execute(string.Format(@"update cs_consume set RewardPoints = round((TotalCost)*{0},0),PointStatus = 1 
                                        where Id = @0;
                insert into userintegral (userId,integralSource,value,usevalue,datastate,remark,CreateTime,IntegralBeginDate,IntegralInvalidDate)
                select  UserId, '1', RewardPoints, 0, 0, '维保消费返积分', getdate(),CONVERT(varchar(12), getdate(), 111 ) ,CONVERT(varchar(12) , DATEADD(yyyy,2,getdate()), 111)
                from cs_consume
                where RewardPoints > 0 and Id = @0;
               insert into UserIntegralRecord (userId,integralSource,value,usevalue,datastate,remark,CreateTime,IntegralBeginDate,IntegralInvalidDate,ProductName)
                select  UserId, '2', ConsumePoints*(-1), 0, 0, '维保消费消耗积分', getdate(),CONVERT(varchar(12), getdate(), 111 ) ,CONVERT(varchar(12) , DATEADD(yyyy,2,getdate()), 111),DealerName
                from cs_consume
                where ConsumePoints > 0 and Id = @0;
               insert into UserIntegralRecord (userId,integralSource,value,usevalue,datastate,remark,CreateTime,IntegralBeginDate,IntegralInvalidDate,ProductName)
                select  UserId, '1', RewardPoints, 0, 0, '维保消费返积分', getdate(),CONVERT(varchar(12), getdate(), 111 ) ,CONVERT(varchar(12) , DATEADD(yyyy,2,getdate()), 111),DealerName
                from cs_consume
                where RewardPoints > 0 and Id = @0;
                ", discount), id);
                scope.Complete();
            }
        }

        /// <summary>
        /// 管理员 确认结算 （报表管理 特约店积分结算报表）
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="SettlementState"></param>
        public void Settlement(string dealerId, string startDate, string endDate, int SettlementState, string consumeType)
        {
            BMDb db = BMDb.GetInstance();           
            using (var scope = db.GetTransaction())
            {
                string sql = string.Format("UPDATE CS_Consume SET Settlement = 'Y' ,SettlementState={0} WHERE Settlement is null ", SettlementState);
                if (!string.IsNullOrEmpty(dealerId))
                {
                    sql += " AND DealerId = '" + dealerId + "'";
                }
                if (!string.IsNullOrEmpty(startDate))
                {
                    sql += " AND ConsumeDate >= '" + startDate + "-01'";
                }

                if (!string.IsNullOrEmpty(endDate))
                {
                    sql += " AND ConsumeDate < '" + DateTime.Parse(endDate.ToString()).AddMonths(1).AddSeconds(-1).ToString() + "'";   
                }

                db.Execute(sql);
                scope.Complete();
            }
            //在结算后把结算的数据插入到新表SettleDealerPoint中
            using (var scope = db.GetTransaction())
            {
                StringBuilder sql = new StringBuilder();

                IntegralCountReportCondition condition = new IntegralCountReportCondition();
                condition.ConsumeType = Convert.ToInt32(consumeType);
                condition.DealerId = dealerId;
                condition.StartTime = Convert.ToDateTime(startDate);
                condition.EndTime = Convert.ToDateTime(endDate);
                condition.SettlementState = SettlementState;
                if (condition.EndTime.HasValue)
                {
                    endDate = DateTime.Parse(condition.EndTime.ToString()).AddMonths(1).AddSeconds(-1).ToString();
                }


                sql.Append("insert SettleDealerPoint select d.Area,d.Region,d.DealerId,c.SettlementState,'" + condition.ConsumeType + "'as ConsumeType, c.ApproveStatus,d.Name,sum(rewardpoints) RewardPoints,sum(consumepoints) ConsumePoints,sum(case ConsumeType when 1 then 0 else PointCost end) PointCost,sum(case Settlement when 'Y' then pointcost else 0 end) SettlementY,sum(case Settlement when 'Y' then 0 else pointcost end) SettlementN,'" + condition.StartTime + "'as DateStart,'" + Convert.ToDateTime(endDate) + "'as DateEnd,'"+ DateTime.Now+"'as CreateTime");
                sql.Append(@" from CS_Consume c 
                        left join Membership m on c.userid = m.id
                        left join CS_CarDealerShip d on c.dealerid = d.dealerid ");
                sql.AppendFormat(" where {0}", condition.ToWhere());
                sql.Append(" group by d.area,d.region,d.dealerid,d.Name,c.ApproveStatus,c.SettlementState");

                db.Execute(sql.ToString(), condition);
                scope.Complete();
            }
        }


        /// <summary>
        /// 蓝缤会员消费积分明细
        /// </summary>
        /// <param name="userId">会员ID</param>
        /// <param name="bgDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <returns>蓝缤会员消费积分明细</returns>
        public IEnumerable<CSConsume> GetUserConsumeList(string userId, DateTime bgDate, DateTime endDate)
        {
            string sql =
                @"select * from CS_Consume   where  UserId=@UserId  {0} ";//AND CreateTime  between @bgDate and @endDate
            //时间段可以为空
            var whereExp = new StringBuilder();
            if (bgDate.Year != 9999 || endDate.Year != 9999)
            {
                whereExp.Append("AND CreateTime  between @bgDate and @endDate");
            }
            sql = string.Format(sql,whereExp.ToString());
            return DbHelp.Query<CSConsume>(sql, new { UserId = userId, bgDate = bgDate, endDate = endDate });
        }

        /// <summary>
        /// 蓝缤会员积分使用（消费/新增）明细
        /// </summary>
        /// <param name="userId">会员ID</param>
        /// <param name="bgDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <returns>蓝缤会员消费积分明细</returns>
        public IEnumerable<ResUserintegral> GetUserIntegraldetail(string userId, DateTime bgDate, DateTime endDate)
        {
            string sql =
                @" select value as Point,CreateTime as UpdateDate,ProductName as DealerId,case when value<0 then '1' else '0'  end as Type,remark from UserIntegralRecord 
				    where userId=@UserId and datastate=0 {0} order by CreateTime desc ";//AND CreateTime  between @bgDate and @endDate
            //时间段可以为空
            var whereExp = new StringBuilder();
            if (bgDate.Year != 9999 || endDate.Year != 9999)
            {
                whereExp.Append("AND CreateTime  between @bgDate and @endDate");
            }
            sql = string.Format(sql, whereExp.ToString());
            return DbHelp.Query<ResUserintegral>(sql, new { UserId = userId, bgDate = bgDate, endDate = endDate });
        }

        /// <summary>
        /// 根据用户名称查找用户ID
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        public string GetDealerIdByname(string Name)
        {
            string sql = @"select DealerId from CS_CarDealerShip where Name = @Name";
            return DbHelp.ExecuteScalar<string>(sql,new { Name = Name });
        }

        /// <summary>
        /// 查找一键入会返积分/经销商入会返积分的经销商ID
        /// </summary>
        /// <param name="Userid"></param>
        /// <returns></returns>
        public string GetDeaByUId(string Userid)
        {
            string sql = @"select CreatedPerson from Membership where Id = @Userid ";
            return DbHelp.ExecuteScalar<string>(sql,new { Userid = Userid });
        }

        public string GetDIdByUId(string Userid)
        {
            string sql = @"select DealerId from MembershipRequest where MembershipId = @Userid";
            return DbHelp.ExecuteScalar<string>(sql,new { Userid = Userid });
        }

        /// <summary>
        ///根据消费工单ID查找NO
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetOrderNoById(int id)
        {
            string sql = @"select OrderNo from CS_Consume where Id = @id";
            return DbHelp.ExecuteScalar<string>(sql, new { Id = id });
        }

        /// <summary>
        /// 根据DMS的消费单号查找工单信息
        /// </summary>
        /// <param name="DMSOrderNo"></param>
        /// <returns></returns>
        public Consuem GetUserConsumeByDmsNo(string DMSOrderNo)
        {
            string sql = @"select * from CS_Consume where DMSOrderNo = @DMSOrderNo";
            return DbHelp.QueryOne<Consuem>(sql, new { DMSOrderNo = DMSOrderNo });
        }

        /// <summary>
        /// DMS消费工单，只执行返还积分的操作
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="id"></param>
        public void AddAndProcessReward(string userId, int id)
        {

            double discount = _DbSession.UserIntegralStorager.GetUserIntegralDiscount(userId);
            BMDb db = BMDb.GetInstance();
            using (var scope = db.GetTransaction())
            {

                db.Execute(string.Format(@"update cs_consume set RewardPoints = round((TotalCost)*{0},0),PointStatus = 1 
                                        where Id = @0;
                insert into userintegral (userId,integralSource,value,usevalue,datastate,remark,CreateTime,IntegralBeginDate,IntegralInvalidDate)
                select  UserId, '1', RewardPoints, 0, 0, '维保消费返积分', getdate(),CONVERT(varchar(12), getdate(), 111 ) ,CONVERT(varchar(12) , DATEADD(yyyy,2,getdate()), 111)
                from cs_consume
                where RewardPoints > 0 and Id = @0;
               insert into UserIntegralRecord (userId,integralSource,value,usevalue,datastate,remark,CreateTime,IntegralBeginDate,IntegralInvalidDate,ProductName)
                select  UserId, '1', RewardPoints, 0, 0, '维保消费返积分', getdate(),CONVERT(varchar(12), getdate(), 111 ) ,CONVERT(varchar(12) , DATEADD(yyyy,2,getdate()), 111),DealerName
                from cs_consume
                where RewardPoints > 0 and Id = @0;
                ", discount), id);
                scope.Complete();
            }
        }
    }
}
