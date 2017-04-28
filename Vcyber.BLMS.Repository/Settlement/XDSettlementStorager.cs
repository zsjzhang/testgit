using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.IRepository;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Common;
using System.Web.Script.Serialization;

namespace Vcyber.BLMS.Repository
{
   public class XDSettlementStorager : IXDSettlementStorager
    {
        public IEnumerable<SettlementList> GetXDSettlementList(string createtime)
        {
            string sql = string.Empty;

            //          sql = @"select d.Area,d.Region,d.DealerId,c.SettlementState,c.ApproveStatus,d.Name,sum(rewardpoints) RewardPoints,
            //                   sum(consumepoints) ConsumePoints,sum(case ConsumeType when 1 then 0 else PointCost end) PointCost,
            //                   sum(case Settlement when 'Y' then pointcost else 0 end) SettlementY,sum(case Settlement when 'Y' then 0 else pointcost end) SettlementN,
            //                   ' ~ ' as DateString from CS_Consume c 
            //                  inner join Membership m on c.userid = m.id
            //                  inner join CS_CarDealerShip d on c.dealerid = d.dealerid  where  1=1  and c.ApproveStatus ='2' 
            //                  and c.CreateTime between @Stdarttime and @Endtime  
            //group by d.area,d.region,d.dealerid,d.Name,c.ApproveStatus,c.SettlementState";
                   sql = @"select distinct c.Id, c.Area,c.Region,c.DealerId,c.SettlementState,c.ApproveStatus,
	                         c.RewardPoints,c.ConsumePoints,c.PointCost,c.SettlementY,c.SettlementN,c.DateStart,c.DateEnd, 
	                         convert(varchar,DateStart,120)+' ~ '+convert(varchar,DateEnd,120) as DateString 
	                         from SettleDealerPoint c where  1=1  
	                          and c.SettlementState ='2' {0}";
            var whereExp = new StringBuilder();
            if (!string.IsNullOrEmpty(createtime))
            {
                whereExp.Append(" and c.CreateTime >= @CreateTime ");
            }
         
            sql = string.Format(sql, whereExp.ToString());

            return DbHelp.Query<SettlementList>(sql, new { @CreateTime = createtime });
        }

        public int UpdateXDSettlementStatus(string settlementstate, string id)
        {
            string sql = string.Empty;

             
            sql = @"update SettleDealerPoint set SettlementState = @SettlementState where Id = @ID";

           //sql = @"update SettleDealerPoint set SettlementState = case @SettlementState when '已确认' then 1 when '待复核' then 3 else @SettlementState end 
						 //where DealerId = @DealerId";

            //return DbHelp.Query<SettlementList>(sql, new { @SettlementState = settlementstate, @DealerId = dealerid });
            return DbHelp.Execute(sql, new { @SettlementState = settlementstate, @ID = id });
        }

        public bool InsertDMSIntegralList(DMSIntergralListV data)
        {
            string sql = string.Empty;
            //List<IntegraListV> Integral = new List<IntegraListV>();
            //List<DMSIntergralListV> Integral = new List<DMSIntergralListV>();
            bool bl = true;

            foreach (var item in data.IntegraList)
            {
                try
                {
                    #region
                    //string DealerId = data.DealerId;
                    //string FromTime = data.FromTime;
                    //string EndTime = data.EndTime;
                    //string TotalPoint = data.TotalPoint;
                    //decimal TotalMony = data.TotalMony;
                    //string ISAgree = data.ISAgree;
                    //string UserId = item.UserId;
                    //string UserName = item.UserName;
                    //string OrderNo = item.OrderNo;
                    //string Phone = item.Phone;
                    ////string DealerId = item.DealerId;
                    //string DealerName = item.DealerName;
                    //string ScheduleOrderNo = item.ScheduleOrderNo;
                    //string ConsumeType = item.ConsumeType;
                    //decimal PointCost = item.PointCost;
                    //decimal TotalCost = item.TotalCost;
                    //int ConsumePoints = item.ConsumePoints;
                    //int RewardPoints = item.RewardPoints;
                    //string PaperOrder = item.PaperOrder;
                    //int ApproveStatus = item.ApproveStatus;
                    //int PointStatus = item.PointStatus;
                    //DateTime ConsumeDate = item.ConsumeDate;
                    //DateTime CreateTime = item.CreateTime;
                    //DateTime UpdateTime = item.UpdateTime;
                    //string Comment = item.Comment;
                    //string IdentityNumber = item.IdentityNumber;
                    //string VIN = item.VIN;
                    #endregion
                    sql = @"insert into CS_DMS_Consume ([DealerId],[DMSOrderNo]
           ,[FromTime]
           ,[EndTime]
           ,[TotalPoint]
           ,[TotalMoney]
           ,[ISAgree]
           ,[UserId]
           ,[Phone]
           ,[ConsumeType]
           ,[PointCost]
           ,[TotalCost]
           ,[ConsumePoints]
           ,[RewardPoints]
           ,[ConsumeDate]
           ,[Comment]
           ,[IdentityNumber]
           ,[VIN]
           ,[CreateTime])
values (@DealerId,@DMSOrderNo,@FromTime,@EndTime,@TotalPoint,@TotalMoney,@ISAgree,@UserId,@Phone,@ConsumeType,
                       @PointCost,@TotalCost,@ConsumePoints,@RewardPoints,@ConsumeDate,@Comment,@IdentityNumber,@VIN,@CreateTime )";
                    DbHelp.Execute(sql, new
                    {
                        @DealerId = data.DealerId,
                        @DMSOrderNo = item.DMSOrderNo,
                        @FromTime = data.FromTime,
                        @EndTime = data.EndTime,
                        @TotalPoint = data.TotalPoint,
                        @TotalMoney = data.TotalMoney,
                        @ISAgree = data.ISAgree,
                        @UserId = item.BlueMembership_Id,
                        //@UserName = item.UserName,
                        //@OrderNo = item.OrderNo,
                        @Phone = item.Phone,
                        //@DealerName = item.DealerName,
                        //@ScheduleOrderNo = item.ScheduleOrderNo,
                        @ConsumeType = item.ConsumeType,
                        @PointCost = item.PointCost,
                        @TotalCost = item.TotalCost,
                        @ConsumePoints = item.ConsumePoints,
                        @RewardPoints = item.RewardPoints,
                        //@PaperOrder = item.PaperOrder,
                        //@ApproveStatus = item.ApproveStatus,
                        //@PointStatus = item.PointStatus,
                        @ConsumeDate = item.ConsumeDate,
                        @CreateTime = DateTime.Now,
                        //@UpdateTime = item.UpdateTime,
                        @Comment = data.Comment,
                        @IdentityNumber = item.IdentityNumber,
                        @VIN = item.VIN,
                    });
                }
                catch (Exception ex)
                {
                    log4net.LogManager.GetLogger("积分结算过程中，DMS的积分清单").Error(data.DealerId, ex);
                    bl = false;
                }

            }

            return bl;
        }

        //更改BM结算数据的状态
        public bool UpdateBMSettlementStatus(DMSIntergralListV data)
        {
            JavaScriptSerializer Serializer = new JavaScriptSerializer();
            var jsonData = Serializer.Serialize(data);
            string sql = string.Empty;
            string ISAgree = string.Empty;
            bool bl = true;
            try
                {
                    sql = @"update SettleDealerPoint set SettlementState = @ISAgree where DealerId = @DealerId and  DateStart = @DateStart and  DateEnd = @DateEnd";

                    if (data.ISAgree.ToLower() == "y")
                    {
                        ISAgree = "1";
                    }
                    else
                    {
                        ISAgree = "3";
                    }
                DbHelp.Execute(sql, new
                    {
                        @ISAgree = ISAgree,
                        @DealerId = data.DealerId,
                        @DateStart = data.FromTime,
                        @DateEnd = data.EndTime,
                    });
                }
                catch (Exception ex)
                {
                //BLMS.Common.LogService.Instance.Info("调用GetDMSIntegralList接口" + string.Format("积分结算过程中，DMS的积分清单（失败），方法：GetDMSIntegralList 传入参数：{0}||时间：{1}", jsonData, DateTime.Now.ToString()));
                log4net.LogManager.GetLogger("积分结算失败").Error(data.DealerId, ex);
                bl = false;
                }
            try
                {
                string sql1 = string.Empty;
                sql1 = @"update CS_Consume set SettlementState = @ISAgree where DealerId = @DealerId and  ConsumeDate >= @DateStart and  ConsumeDate < @DateEnd";

                    if (data.ISAgree.ToLower() == "y")
                    {
                        ISAgree = "1";
                    }
                    else
                    {
                        ISAgree = "3";
                    }
                DbHelp.Execute(sql1, new
                    {
                        @ISAgree = ISAgree,
                        @DealerId = data.DealerId,
                        @DateStart = data.FromTime,
                        @DateEnd = data.EndTime,
                        //@DateEnd = DateTime.Parse(data.EndTime.ToString()).AddMonths(1).AddSeconds(-1).ToString(),
                    });

                }
                catch (Exception ex)
                {
                    log4net.LogManager.GetLogger("积分结算失败").Error(data.DealerId, ex);
                    //BLMS.Common.LogService.Instance.Info("调用GetDMSIntegralList接口" + string.Format("积分结算过程中，DMS的积分清单（失败），方法：GetDMSIntegralList 传入参数：{0}||时间：{1}", jsonData, DateTime.Now.ToString()));
                    bl = false;
                }
            
            return bl;
        }
    }
}
