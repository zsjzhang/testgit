using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.IRepository;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Repository.Entity.Generated;
using Vcyber.BLMS.Entity.Generated;

namespace Vcyber.BLMS.Repository
{
    public class ReportStorager : IReportStorager
    {
        public DataTable GetReport(DateTime startTime, DateTime endTime, string tableName)
        {
            try
            {
                DBHelper dbHelper = new DBHelper();

                SqlParameter[] paramsArray = new SqlParameter[2] { new SqlParameter("@BeginDay", startTime), new SqlParameter("@EndDay", endTime) };
                var data = dbHelper.ExecDataSetProc(tableName, paramsArray);
                return data.Tables[0];

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// 报表管理--按车型、区域统计会员入会数量
        /// </summary>
        /// <param name="CreateTimeStart">入会开始时间</param>
        /// <param name="CreateTimeEnd">入会结束时间</param>
        /// <param name="AuthenticationTimeStart">入会认证开始时间</param>
        /// <param name="AuthenticationTimeEnd">入会认证结束时间</param>
        /// <param name="BuyTimeStart">购车开始时间</param>
        /// <param name="BuyTimeEnd">购车结束时间</param>
        /// <param name="CarCategory">车型</param>
        /// <param name="tableName">存储过程名称</param>
        /// <returns></returns>
        public DataTable GetReport(DateTime? CreateTimeStart, DateTime? CreateTimeEnd, DateTime? AuthenticationTimeStart, DateTime? AuthenticationTimeEnd, DateTime? BuyTimeStart, DateTime? BuyTimeEnd, string CarCategory, string AccntType, string tableName)
        {
            try
            {
                DBHelper dbHelper = new DBHelper();

                SqlParameter[] paramsArray = new SqlParameter[8] { 
                    new SqlParameter("@CreateTimeStart", CreateTimeStart), 
                    new SqlParameter("@CreateTimeEnd", CreateTimeEnd),
                new SqlParameter("@AuthenticationTimeStart", AuthenticationTimeStart),
                new SqlParameter("@AuthenticationTimeEnd", AuthenticationTimeEnd),
                new SqlParameter("@BuyTimeStart", BuyTimeStart),
                new SqlParameter("@BuyTimeEnd", BuyTimeEnd),
                new SqlParameter("@AccntType", AccntType),                
                new SqlParameter("@CarCategory", CarCategory)
                };
                var data = dbHelper.ExecDataSetProc(tableName, paramsArray);
                return data.Tables[0];

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// 统计出库数量
        /// </summary>
        /// <param name="BuyTimeStart">购车起始时间</param>
        /// <param name="BuyTimeEnd">购车截至时间</param>
        /// <param name="CarCategory">车型</param>
        /// <param name="tableName">存储过程名称</param>
        /// <returns></returns>
        public DataTable GetReport(DateTime? BuyTimeStart, DateTime? BuyTimeEnd, string CarCategory, string tableName)
        {
            try
            {
                DBHelper dbHelper = new DBHelper();

                SqlParameter[] paramsArray = new SqlParameter[3] {                 
                new SqlParameter("@BuyTimeStart", BuyTimeStart),
                new SqlParameter("@BuyTimeEnd", BuyTimeEnd),
                new SqlParameter("@CarCategory", CarCategory)
                };
                var data = dbHelper.ExecDataSetProc(tableName, paramsArray);
                return data.Tables[0];

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 车型渠道认证统计
        /// </summary>
        /// <param name="AuthenticationTimeStart"></param>
        /// <param name="AuthenticationTimeEnd"></param>
        /// <param name="BuyTimeStart"></param>
        /// <param name="BuyTimeEnd"></param>
        /// <param name="CarCategory"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public DataTable GetAuthenticationSourceByCarCategory(DateTime? AuthenticationTimeStart, DateTime? AuthenticationTimeEnd, DateTime? BuyTimeStart, DateTime? BuyTimeEnd, string CarCategory, string tableName)
        {
            try
            {
                DBHelper dbHelper = new DBHelper();

                SqlParameter[] paramsArray = new SqlParameter[5] {                 
                new SqlParameter("@AuthenticationTimeStart", AuthenticationTimeStart),
                new SqlParameter("@AuthenticationTimeEnd", AuthenticationTimeEnd),
                new SqlParameter("@BuyTimeStart", BuyTimeStart),
                new SqlParameter("@BuyTimeEnd", BuyTimeEnd),
                new SqlParameter("@CarCategory", CarCategory)
                };
                var data = dbHelper.ExecDataSetProc(tableName, paramsArray);
                return data.Tables[0];

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable GetUserCarIntegralRecordValueSum(DateTime? BuyTimeStart, DateTime? BuyTimeEnd, string CarCategory, string tableName)
        {
            try
            {
                DBHelper dbHelper = new DBHelper();

                SqlParameter[] paramsArray = new SqlParameter[3] {                 
                
                new SqlParameter("@BuyTimeStart", BuyTimeStart),
                new SqlParameter("@BuyTimeEnd", BuyTimeEnd),
                new SqlParameter("@CarCategory", CarCategory)
                };
                var data = dbHelper.ExecDataSetProc(tableName, paramsArray);
                return data.Tables[0];

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable GetCommonReport(DateTime? CreateTimeStart, DateTime? CreateTimeEnd, string TimeType, string tableName)
        {
            try
            {
                DBHelper dbHelper = new DBHelper();

                SqlParameter[] paramsArray = new SqlParameter[3] {                 
                new SqlParameter("@CreateTimeStart", CreateTimeStart),
                new SqlParameter("@CreateTimeEnd", CreateTimeEnd),
                new SqlParameter("@TimeType", TimeType)
                };
                var data = dbHelper.ExecDataSetProc(tableName, paramsArray);
                return data.Tables[0];

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// 按车型，分时间维度 统计返厂次数
        /// </summary>
        /// <param name="CreateTimeStart">返厂开始时间</param>
        /// <param name="CreateTimeEnd">返厂结束时间【消费表的createtime】</param>
        /// <param name="TimeType">查询维度 年月日</param>
        /// <param name="CarCategory">车型</param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public DataTable GetCommonReport(DateTime? CreateTimeStart, DateTime? CreateTimeEnd, string TimeType, string CarCategory, string tableName)
        {
            try
            {
                DBHelper dbHelper = new DBHelper();

                SqlParameter[] paramsArray = new SqlParameter[4] {                 
                new SqlParameter("@CreateTimeStart", CreateTimeStart),
                new SqlParameter("@CreateTimeEnd", CreateTimeEnd),
                new SqlParameter("@TimeType", TimeType),
                new SqlParameter("@CarCategory", CarCategory)
                };
                var data = dbHelper.ExecDataSetProc(tableName, paramsArray);
                return data.Tables[0];

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// 按年月日统计[活动]的入会人数
        /// </summary>
        /// <param name="CreateTimeStart">参加活动的开始时间</param>
        /// <param name="CreateTimeEnd">参加活动的结束时间</param>
        /// <param name="TimeType">查询维度，年月日</param>
        /// <param name="ActivityId">活动ID</param>
        /// <param name="tableName">存储过程名称</param>
        /// <returns></returns>
        public DataTable GetMembershipCountByTimeAndActivity(DateTime? CreateTimeStart, DateTime? CreateTimeEnd, string TimeType, string ActivityId, string tableName)
        {
            try
            {
                DBHelper dbHelper = new DBHelper();

                SqlParameter[] paramsArray = new SqlParameter[4] {                 
                new SqlParameter("@CreateTimeStart", CreateTimeStart),
                new SqlParameter("@CreateTimeEnd", CreateTimeEnd),
                new SqlParameter("@TimeType", TimeType),
                new SqlParameter("@ActivityId", ActivityId)
                };
                var data = dbHelper.ExecDataSetProc(tableName, paramsArray);
                return data.Tables[0];

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// 查询中奖明细
        /// </summary>
        /// <param name="CreateTimeStart"></param>
        /// <param name="CreateTimeEnd"></param>
        /// <param name="ActivityId"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public DataTable GetWinningInfoDetailsByActivity(DateTime? CreateTimeStart, DateTime? CreateTimeEnd, string ActivityId, string tableName)
        {
            try
            {
                DBHelper dbHelper = new DBHelper();

                SqlParameter[] paramsArray = new SqlParameter[3] {                 
                new SqlParameter("@CreateTimeStart", CreateTimeStart),
                new SqlParameter("@CreateTimeEnd", CreateTimeEnd),                
                new SqlParameter("@ActivityId", ActivityId)
                };
                var data = dbHelper.ExecDataSetProc(tableName, paramsArray);
                return data.Tables[0];

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable GetCreatedPerson(string qType, string CreatedPerson, string tableName)
        {
            try
            {
                DBHelper dbHelper = new DBHelper();

                SqlParameter[] paramsArray = new SqlParameter[2] {                 
                new SqlParameter("@qType", qType),
                new SqlParameter("@CreatedPerson", CreatedPerson)
                };
                var data = dbHelper.ExecDataSetProc(tableName, paramsArray);
                return data.Tables[0];

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// 保存新的创建渠道到CreatedType
        /// </summary>
        /// <param name="qType"></param>
        /// <param name="CreatedPerson"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public int SaveCreatedType(string qType, string CreatedPerson, string tableName)
        {
            try
            {
                DBHelper dbHelper = new DBHelper();

                SqlParameter[] paramsArray = new SqlParameter[2] {                 
                new SqlParameter("@qType", qType),
                new SqlParameter("@CreatedPerson", CreatedPerson)
                };
                //int k = dbHelper.ExecNonQueryProc(tableName, paramsArray);
                int k = 0;
                var data = dbHelper.ExecDataSetProc(tableName, paramsArray);
                if (data.Tables[0].Rows[0][0].ToString()=="OK")
                {
                    k= 1;
                }
                return k;

            }
            catch (Exception ex)
            {
                return  -3;
            }
        }
 
        public DataTable GetReport(int activityId, string tableName)
        {
            try
            {
                DBHelper dbHelper = new DBHelper();

                SqlParameter[] paramsArray = new SqlParameter[1] { new SqlParameter("@ActivityId", activityId) };
                var data = dbHelper.ExecDataSetProc(tableName, paramsArray);
                return data.Tables[0];

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable GetReport(string tableName)
        {
            try
            {
                DBHelper dbHelper = new DBHelper();

                var data = dbHelper.ExecDataSetProc(tableName);
                return data.Tables[0];

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// 权益使用分析 查询  
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public DataTable GetEquity(string startTime, string endTime)
        {
            try
            {
                DBHelper dbHelper = new DBHelper();
                SqlParameter[] paramsArray = new SqlParameter[2] { new SqlParameter("@begintime", startTime), new SqlParameter("@endtime", endTime) };
                var data = dbHelper.ExecDataSetProc("Equity", paramsArray);
                return data.Tables[0];

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable CardReport(string name,string type)
        {
            try
            {
                DBHelper dbHelper = new DBHelper();
                SqlParameter[] paramsArray = new SqlParameter[2] { new SqlParameter("ActiveName", name), new SqlParameter("CardType", type) };
                var data = dbHelper.ExecDataSetProc("CardCoutStatis", paramsArray);
                return data.Tables[0];

            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public DataTable GetWeekReport(string startTime, string endTime)
        {
            try
            {
                DBHelper dbHelper = new DBHelper();
                SqlParameter[] paramsArray = new SqlParameter[2] { new SqlParameter("@bein", startTime), new SqlParameter("@end", endTime) };
                //var data = dbHelper.ExecDataSetProc("Proc_MembershipLevel", paramsArray);
                var data = dbHelper.ExecDataSetProc("MemshipResour", paramsArray);
                return data.Tables[0];

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        //会员积分消费报表
        public DataTable GetPointCostReport(string startTime, string endTime)
        {
            try
            {
                DBHelper dbHelper = new DBHelper();
                SqlParameter[] paramsArray = new SqlParameter[2] { new SqlParameter("@beginTime", startTime), new SqlParameter("@endTime", endTime) };
                var data = dbHelper.ExecDataSetProc("UseUserintegralReport", paramsArray);
                return data.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public IEnumerable<ServiceModel> ServiceUse(string Createtime, string RealName, string phoneNumber, string DataSource, string AirportName, string DealerId, string Status, string OrderType, int pageIndex, int pageSize, out int totalCount)
        {
            string strWhere = " WHERE 1=1  ";
            if (!string.IsNullOrEmpty(Createtime))
            {
                strWhere += " and CONVERT(char(10),CreateTime,120)=@Createtime ";
            }

            if (!string.IsNullOrEmpty(RealName))
            {
                strWhere += " and RealName like @RealName ";
            }

            if (!string.IsNullOrEmpty(phoneNumber))
            {
                strWhere += " and PhoneNumber=@phoneNumber ";
            }
            if (!string.IsNullOrEmpty(DataSource))
            {
                strWhere += " and DataSource=@DataSource ";
            }

            if (!string.IsNullOrEmpty(AirportName))
            {
                strWhere += " and AirportName like @AirportName ";
            }
            if (!string.IsNullOrEmpty(DealerId))
            {
                strWhere += " and DealerId=@DealerId ";
            }
            if (!string.IsNullOrEmpty(Status))
            {
                strWhere += " and Status=@Status ";
            }
            if (!string.IsNullOrEmpty(OrderType))
            {
                strWhere += " and OrderType=@OrderType ";
            }

            string sql = "SELECT top {0} * FROM(SELECT ROW_NUMBER() OVER(ORDER BY CreateTime DESC) as rowid,*  FROM( SELECT CS_SonataService.CreateTime,CASE CS_SonataService.OrderType WHEN 0 THEN '上门关怀服务'WHEN 1 THEN '3年9次免费检测服务'WHEN 2 THEN '免费取送车服务' WHEN 3 THEN '一对一专属服务' WHEN 5 THEN '长途旅行关怀服务' ELSE '' END as OrderType,Membership.RealName,Membership.PhoneNumber,Membership.IdentityNumber,CS_SonataService.VIN,CASE CS_SonataService.MaintainType WHEN 0 THEN '维修'WHEN 1 THEN '保养'WHEN 2 THEN '维保' ELSE '' END as MaintainType,CASE CS_SonataService.DataSource WHEN 'blms_web' THEN 'App' WHEN 'blms_wechat' THEN '微信' WHEN 'blms' THEN '网站' ELSE '' END as DataSource,null as SNCode,null as SendType,null as IsUse,null as UseTime,null as AirportName,null as UseAdd,CS_SonataService.DealerId,CS_CarDealerShip.Name,CS_CarDealerShip.Address,CASE CS_SonataService.Status WHEN 0 THEN '待受理' WHEN 1 THEN '系统已受理' WHEN 2 THEN '待特约店处理' WHEN 3 THEN '服务记录已完成' ELSE '' END as Status,CS_CarDealerShip.Area,CS_CarDealerShip.Region from CS_SonataService left join Membership on CS_SonataService.UserId=Membership.Id  left join CS_CarDealerShip on CS_SonataService.DealerId=CS_CarDealerShip.DealerId ";
            sql += " UNION ALL";
            sql += " SELECT SNCard.CreateTime,'35个机场候机尊享服务' as OrderType,Membership.RealName,Membership.PhoneNumber,Membership.IdentityNumber,null as VIN,'机场服务' as MaintainType,CASE SNCard.SendType WHEN 1 THEN '网站' WHEN 2 THEN 'App'ELSE '' END as DataSource,SNCard.SNCode,CASE SNCard.SendType WHEN 4 THEN '否' ELSE '是' END as SendType,CASE SNCard.Status WHEN 3 THEN '是' ELSE '否' END as IsUse,snusedrecord.UseTime,Airport.AirportName,snusedrecord.UseAdd,null as DealerId,null as Name,null as Address,CASE SNCard.Status WHEN 1 THEN '创建' WHEN 2 THEN '已下发' WHEN 3 THEN '已消费' ELSE '' END as Status,null as Area,null as Region  from SNCard left join Membership on SNCard.UserId=Membership.Id left join Airport on SNCard.AirportId=Airport.Id left join snusedrecord on SNCard.SNCode=snusedrecord.SNCode WHERE SNCard.Status>1)A " + strWhere + " )B where rowid NOT IN(SELECT TOP {1} rowid FROM(SELECT ROW_NUMBER() OVER(ORDER BY CreateTime DESC) as rowid,* FROM(";

            sql += "SELECT CS_SonataService.CreateTime,CASE CS_SonataService.OrderType WHEN 0 THEN '上门关怀服务'WHEN 1 THEN '3年9次免费检测服务'WHEN 2 THEN '免费取送车服务' WHEN 3 THEN '一对一专属服务' WHEN 5 THEN '长途旅行关怀服务' ELSE '' END as OrderType,Membership.RealName,Membership.PhoneNumber,CASE CS_SonataService.DataSource WHEN 'blms_web' THEN 'App' WHEN 'blms_wechat' THEN '微信' WHEN 'blms' THEN '网站' ELSE '' END as DataSource,null as AirportName,CS_SonataService.DealerId,CASE CS_SonataService.Status WHEN 0 THEN '待受理' WHEN 1 THEN '系统已受理' WHEN 2 THEN '待特约店处理' WHEN 3 THEN '服务记录已完成' ELSE '' END as Status from CS_SonataService left join Membership on CS_SonataService.UserId=Membership.Id  left join CS_CarDealerShip on CS_SonataService.DealerId=CS_CarDealerShip.DealerId ";
            sql += " UNION ALL ";
            sql += " SELECT SNCard.CreateTime,'35个机场候机尊享服务' as OrderType,Membership.RealName,Membership.PhoneNumber,CASE SNCard.SendType WHEN 1 THEN '网站' WHEN 2 THEN 'App'ELSE '' END as DataSource, Airport.AirportName,null as DealerId,CASE SNCard.Status WHEN 1 THEN '创建' WHEN 2 THEN '已下发' WHEN 3 THEN '已消费' ELSE '' END as Status from SNCard left join Membership on SNCard.UserId=Membership.Id left join Airport on SNCard.AirportId=Airport.Id left join snusedrecord on SNCard.SNCode=snusedrecord.SNCode WHERE SNCard.Status>1 ";
            sql += ")T " + strWhere + " )T1) ";
            sql = string.Format(sql, pageSize, pageIndex);

            string sql1 = "SELECT COUNT(1) FROM(SELECT CS_SonataService.CreateTime,CASE CS_SonataService.OrderType WHEN 0 THEN '上门关怀服务'WHEN 1 THEN '3年9次免费检测服务'WHEN 2 THEN '免费取送车服务' WHEN 3 THEN '一对一专属服务' WHEN 5 THEN '长途旅行关怀服务' ELSE '' END as OrderType,Membership.RealName,Membership.PhoneNumber,CASE CS_SonataService.DataSource WHEN 'blms_web' THEN 'App' WHEN 'blms_wechat' THEN '微信' WHEN 'blms' THEN '网站' ELSE '' END as DataSource,null as AirportName,CS_SonataService.DealerId,CASE CS_SonataService.Status WHEN 0 THEN '待受理' WHEN 1 THEN '系统已受理' WHEN 2 THEN '待特约店处理' WHEN 3 THEN '服务记录已完成' ELSE '' END as Status from CS_SonataService left join Membership on CS_SonataService.UserId=Membership.Id  left join CS_CarDealerShip on CS_SonataService.DealerId=CS_CarDealerShip.DealerId UNION ALL SELECT SNCard.CreateTime,'35个机场候机尊享服务' as OrderType,Membership.RealName,Membership.PhoneNumber,CASE SNCard.SendType WHEN 1 THEN '网站' WHEN 2 THEN 'App'ELSE '' END as DataSource,Airport.AirportName,null as DealerId,CASE SNCard.Status WHEN 1 THEN '创建' WHEN 2 THEN '已下发' WHEN 3 THEN '已消费' ELSE '' END as Status from SNCard left join Membership on SNCard.UserId=Membership.Id left join Airport on SNCard.AirportId=Airport.Id left join snusedrecord on SNCard.SNCode=snusedrecord.SNCode WHERE SNCard.Status>1)A "+ strWhere;
            //var totalsql = string.Format(sql1, strWhere);
            totalCount = DbHelp.ExecuteScalar<int>(sql1, new { @Createtime = Createtime, @RealName = "%" + RealName + "%", @phoneNumber = phoneNumber, @DataSource = DataSource, @AirportName = "%" + AirportName + "%", @DealerId = DealerId, @Status = Status, @OrderType = OrderType });
            return DbHelp.Query<ServiceModel>(sql, new { @Createtime = Createtime, @RealName = "%" + RealName + "%", @phoneNumber = phoneNumber, @DataSource = DataSource, @AirportName = "%" + AirportName + "%", @DealerId = DealerId, @Status = Status, @OrderType = OrderType/*, @pageSize = pageSize, @pageIndex = pageIndex*/ });
        }

        /// <summary>
        /// 查询会员信息
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="pageData"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public IEnumerable<MemberReportInfo> FindMemberList(MemberReportCondition condition, PageData pageData, out int totalCount)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(" select COUNT(1) ");
            sql.Append(" from Membership m left join IF_Customer cu on m.IdentityNumber=cu.IdentityNumber and cu.IdentityNumber != ''");
            sql.Append(" left join IF_Car car on cu.CustId=car.CustId left join CS_CarDealerShip dealer on car.DealerId=dealer.DealerId");
            sql.AppendFormat(" where {0}", condition.ToWhere());
            totalCount = DbHelp.ExecuteScalar<int>(sql.ToString());

            sql.Clear();

            sql.AppendFormat("  select top {0} m.RealName,m.Gender,m.NickName,m.Age,m.MLevel,m.IdentityNumber,m.PhoneNumber,m.No,m.IsPay,", pageData.Size);
            sql.Append(" car.CarCategory,car.VIN,car.BuyTime,dealer.DealerId,dealer.Name as DealerName,dealer.Area as BuyingArea,Region as BuyingRegion,");
            sql.Append(" case when m.JoinData IS NOT NULL then m.JoinData else case when m.CreateTime < m.UpdateTime then m.CreateTime else m.UpdateTime end end as RegisterTime,m.CreateTime as MemberTime,(select top 1 MembershipRequest.Status from MembershipRequest where MembershipRequest.MembershipId=m.Id order by MembershipRequest.CreateTime desc) as YKStatus,");
            sql.Append(" m.CreatedPerson as SDataSource,");
            sql.Append(" m.PayNumber,(select top 1 CS_CarDealerShip.Name from MembershipRequest join CS_CarDealerShip on MembershipRequest.DealerId=CS_CarDealerShip.DealerId where MembershipRequest.MembershipId=m.Id order by MembershipRequest.CreateTime desc) as PayDealerName,");
            sql.Append(" cu.City,cu.Address,cu.Email,m.Birthday,m.Interest");
            sql.Append(" from Membership m left join IF_Customer cu on m.IdentityNumber=cu.IdentityNumber and cu.IdentityNumber != ''");
            sql.Append("  left join IF_Car car on cu.CustId=car.CustId left join CS_CarDealerShip dealer on car.DealerId=dealer.DealerId");
            sql.AppendFormat(" where {0}  and m.Id not in(", condition.ToWhere());

            sql.AppendFormat("  select top {0} m.Id ", pageData.Index);
            sql.Append(" from Membership m left join IF_Customer cu on m.IdentityNumber=cu.IdentityNumber and cu.IdentityNumber != ''");
            sql.Append(" left join IF_Car car on cu.CustId=car.CustId left join CS_CarDealerShip dealer on car.DealerId=dealer.DealerId");
            sql.AppendFormat(" where {0} ", condition.ToWhere());
            sql.Append(" order by m.UpdateTime desc");
            sql.Append(" ) order by m.UpdateTime desc");
            return DbHelp.Query<MemberReportInfo>(sql.ToString());


            //return DbHelp.Query<MemberReportInfo>("proc_memberreport", new { condition = condition.ToWhere(), index = pageData.Index, size=pageData.Size },null,true,null,CommandType.StoredProcedure);
        }

        /// <summary>
        /// 查询会员消耗积分报表信息
        /// 积分兑换
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="pageData"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public IEnumerable<IntegralOutReportInfo> FindIntegralOutList(IntegralOutReportCondition condition, PageData pageData, out int totalCount)
        {
            StringBuilder sql = new StringBuilder();

            sql.Append(" select COUNT(distinct o.Createtime)");
            sql.Append(" from Membership m join orders o on m.Id=o.UserID and (o.Type=2 or o.Type=5) left join IF_Customer cus on m.IdentityNumber=cus.IdentityNumber");
            sql.Append(" left join IF_Car car on cus.CustId=car.CustId and (car.CarCategory = '索纳塔9' or car.CarCategory = '第九代索纳塔' or car.CarCategory = '全新途胜') left join CS_CarDealerShip dealer on car.DealerId=dealer.DealerId");
            sql.AppendFormat(" where  (o.TradeState=17 or o.TradeState=2) and {0}", condition.ToWhere());
            totalCount = DbHelp.ExecuteScalar<int>(sql.ToString());

            sql.Clear();

            sql.AppendFormat(" select distinct top {0} dealer.DealerId,dealer.Region,dealer.Area,dealer.Name as DealerName,m.No,m.RealName,m.PhoneNumber,m.Id", pageData.Size);
            sql.Append(" ,o.Mode as OrderMode,o.Integral as IntegralValue,o.Createtime");
            sql.Append(" from Membership m join orders o on m.Id=o.UserID and (o.Type=2 or o.Type=5) left join IF_Customer cus on m.IdentityNumber=cus.IdentityNumber");
            sql.Append(" left join IF_Car car on cus.CustId=car.CustId and (car.CarCategory = '索纳塔9' or car.CarCategory = '第九代索纳塔' or car.CarCategory = '全新途胜') left join CS_CarDealerShip dealer on car.DealerId=dealer.DealerId");
            sql.AppendFormat(" where (o.TradeState=17 or o.TradeState=2) and  {0} ", condition.ToWhere());
            sql.Append(" and o.Id not in(");
            sql.AppendFormat(" select top {0} o.Id", pageData.Index);
            sql.Append(" from Membership m join orders o on m.Id=o.UserID and (o.Type=2 or o.Type=5) left join IF_Customer cus on m.IdentityNumber=cus.IdentityNumber");
            sql.Append(" left join IF_Car car on cus.CustId=car.CustId and (car.CarCategory = '索纳塔9' or car.CarCategory = '第九代索纳塔' or car.CarCategory = '全新途胜') left join CS_CarDealerShip dealer on car.DealerId=dealer.DealerId");
            sql.AppendFormat(" where (o.TradeState=17 or o.TradeState=2) and  {0}", condition.ToWhere());
            sql.Append(" order by o.Createtime desc , dealer.Area, dealer.DealerId, m.RealName, o.Mode");
            sql.Append(" ) order by o.Createtime desc , dealer.Area, dealer.DealerId, m.RealName, o.Mode");
            return DbHelp.Query<IntegralOutReportInfo>(sql.ToString());
        }

        /// <summary>
        /// 查询会员获取的积分信息
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="pageData"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public IEnumerable<IntegralInputReportInfo> FindIntegralInputList(IntegralInputReportCondition condition, PageData pageData, out int totalCount)
        {
            StringBuilder sql = new StringBuilder();

            // sql.Append(" select COUNT(1) from (");
            sql.Append(" select COUNT(1) ");
            sql.Append(" from userintegral integral join Membership m on m.Id=integral.userId  join IF_Customer cus on m.IdentityNumber=cus.IdentityNumber");
            sql.Append("  join IF_Car car on cus.CustId=car.CustId   join CS_CarDealerShip dealer on car.DealerId=dealer.DealerId");
            sql.AppendFormat(" where {0}", condition.ToWhere1());
            //sql.Append(" ) as tempTable");
            totalCount = DbHelp.ExecuteScalar<int>(sql.ToString(),condition);

            sql.Clear();

            sql.AppendFormat(" select distinct top {0} dealer.DealerId,dealer.Region,dealer.Area,dealer.Name as DealerName,m.No,m.RealName,m.PhoneNumber,m.Id", pageData.Size);
            sql.Append(" ,integral.integralSource,integral.value as IntegralValue,integral.CreateTime");
            sql.Append(" from userintegral integral join Membership m on m.Id=integral.userId   join IF_Customer cus on m.IdentityNumber=cus.IdentityNumber");
            sql.Append("  join IF_Car car on cus.CustId=car.CustId    left join CS_CarDealerShip dealer on car.DealerId=dealer.DealerId");
            sql.AppendFormat(" where {0}", condition.ToWhere1());
            sql.Append(" and integral.ID > (");
            sql.AppendFormat(" select isnull(MAX(t1.ID),0) from( select top {0} integral.ID", pageData.Index);
            sql.Append(" from userintegral integral join Membership m on m.Id=integral.userId join IF_Customer cus on m.IdentityNumber=cus.IdentityNumber");
            sql.Append("  join IF_Car car on cus.CustId=car.CustId left join CS_CarDealerShip dealer on car.DealerId=dealer.DealerId");
            sql.AppendFormat(" where {0}", condition.ToWhere1());
            sql.Append(" order by integral.CreateTime ) as t1");
            sql.Append(" ) order by integral.CreateTime ");

            return DbHelp.Query<IntegralInputReportInfo>(sql.ToString(), condition);
        }

        /// <summary>
        /// 获取会员积分信息
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="pageData"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<IntegralReportInfo> FindIntegralList(IntegralReportCondition condition, PageData pageData, out int totalCount)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(" select count(1)");
            sql.Append(" from Membership m left join IF_Customer cus on m.IdentityNumber=cus.IdentityNumber and cus.IdentityNumber != ''");
            sql.Append(" left join IF_Car car on cus.CustId=car.CustId left join CS_CarDealerShip dealer on car.DealerId=dealer.DealerId");
            sql.AppendFormat(" where {0}", condition.ToWhere());
            totalCount = DbHelp.ExecuteScalar<int>(sql.ToString(), condition);

            sql.Clear();

            sql.Append("  select tempTable.*,(select SUM(userintegral.value) from userintegral where userintegral.userId=tempTable.Id and userintegral.integralSource=9 and DATEDIFF(MONTH,userintegral.CreateTime,GETDATE())=0) as HGCTotal");
            sql.Append("  ,(select SUM(userintegral.value) from userintegral where userintegral.userId=tempTable.Id and userintegral.integralSource=10 and DATEDIFF(MONTH,userintegral.CreateTime,GETDATE())=0) as HZHTotal");
            sql.Append("  ,(select SUM(userintegral.value) from userintegral where userintegral.userId=tempTable.Id and userintegral.integralSource=1 and DATEDIFF(MONTH,userintegral.CreateTime,GETDATE())=0) as HWBTotal");
            sql.Append("  ,(select SUM(orders.Integral) from orders where orders.UserID=tempTable.Id and orders.Mode=2 and DATEDIFF(MONTH,orders.Createtime,GETDATE())=0) as XWXTotal");
            sql.Append("  ,(select SUM(orders.Integral) from orders where orders.UserID=tempTable.Id and orders.Mode=3 and DATEDIFF(MONTH,orders.Createtime,GETDATE())=0) as XBYTotal");
            sql.Append("  ,(select SUM(orders.Integral) from orders where orders.UserID=tempTable.Id and orders.Mode=5 and DATEDIFF(MONTH,orders.Createtime,GETDATE())=0) as XJCTotal");
            sql.Append("  ,(select SUM(orders.Integral) from orders where orders.UserID=tempTable.Id and orders.Mode=1 and DATEDIFF(MONTH,orders.Createtime,GETDATE())=0) as XLPTotal");
            sql.Append("  ,(select SUM(userintegral.value-userintegral.usevalue) from userintegral where userintegral.userId=tempTable.Id and userintegral.datastate=1) as SXTotal");
            sql.Append("  from(");

            sql.AppendFormat("  select top {0} dealer.DealerId,dealer.Area,dealer.Name as DealerName,m.No,m.RealName,m.PhoneNumber,m.Id", pageData.Size);
            sql.Append("  from Membership m left join IF_Customer cus on m.IdentityNumber=cus.IdentityNumber and cus.IdentityNumber != ''");
            sql.Append("  left join IF_Car car on cus.CustId=car.CustId left join CS_CarDealerShip dealer on car.DealerId=dealer.DealerId");

            sql.AppendFormat("  where {0} ", condition.ToWhere());
            sql.Append("  and m.Id not in(");
            sql.AppendFormat("  select top {0} m.Id", pageData.Index);
            sql.Append("  from Membership m left join IF_Customer cus on m.IdentityNumber=cus.IdentityNumber and cus.IdentityNumber != ''");
            sql.Append("  left join IF_Car car on cus.CustId=car.CustId left join CS_CarDealerShip dealer on car.DealerId=dealer.DealerId");
            sql.AppendFormat("  where {0} ", condition.ToWhere());
            sql.Append("  order by m.UpdateTime desc");
            sql.Append("  ) order by m.UpdateTime desc");
            sql.Append(" ) as tempTable");

            var dataResult = DbHelp.Query<IntegralReportInfo>(sql.ToString(), condition);

            sql.Clear();

            sql.Append(" select (select SUM(userintegral.value) from userintegral where  userintegral.integralSource=9 and DATEDIFF(MONTH,userintegral.CreateTime,GETDATE())=0) as HGCTotal");
            sql.Append(" ,(select SUM(userintegral.value) from userintegral where  userintegral.integralSource=10 and DATEDIFF(MONTH,userintegral.CreateTime,GETDATE())=0) as HZHTotal");
            sql.Append(" ,(select SUM(userintegral.value) from userintegral where  userintegral.integralSource=1 and DATEDIFF(MONTH,userintegral.CreateTime,GETDATE())=0) as HWBTotal");
            sql.Append(" ,(select SUM(orders.Integral) from orders where  orders.Mode=2 and DATEDIFF(MONTH,orders.Createtime,GETDATE())=0) as XWXTotal");
            sql.Append(" ,(select SUM(orders.Integral) from orders where orders.Mode=3 and DATEDIFF(MONTH,orders.Createtime,GETDATE())=0) as XBYTotal");
            sql.Append(" ,(select SUM(orders.Integral) from orders where orders.Mode=5 and DATEDIFF(MONTH,orders.Createtime,GETDATE())=0) as XJCTotal");
            sql.Append(" ,(select SUM(orders.Integral) from orders where  orders.Mode=1 and DATEDIFF(MONTH,orders.Createtime,GETDATE())=0) as XLPTotal");
            sql.Append(" ,(select SUM(userintegral.value-userintegral.usevalue) from userintegral where  userintegral.datastate=1) as SXTotal");

            var dataOne = DbHelp.QueryOne<IntegralReportInfo>(sql.ToString());

            if (dataResult != null && dataResult.Count() > 0)
            {
                dataOne.No = "全部";
                dataOne.RealName = "全部";
                dataOne.PhoneNumber = "全部";

                List<IntegralReportInfo> tempResult = dataResult.ToList<IntegralReportInfo>();
                tempResult.Insert(0, dataOne);
                return tempResult;
            }

            return new List<IntegralReportInfo>(1);
        }


        /// <summary>
        /// 管理员查询积分结算
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public IEnumerable<IntegralCountReportInfo> FindIntegralCountList(IntegralCountReportCondition condition)
        {
            StringBuilder sql = new StringBuilder();
            //if(condition.EndTime.HasValue)
            //{
            //    condition.EndTime = condition.EndTime.Value.AddDays(1).AddSeconds(-1);
            //}
            string strEndDate =string .Empty ;
            if (condition .EndTime .HasValue )
            {
                strEndDate = DateTime.Parse(condition.EndTime.ToString()).AddMonths(1).AddSeconds(-1).ToString();
            }

            //sql.Append("select d.Area,d.Region,d.DealerId,c.SettlementState,c.ApproveStatus,d.Name,sum(rewardpoints) RewardPoints,sum(consumepoints) ConsumePoints,sum(case ConsumeType when 1 then 0 else PointCost end) PointCost,sum(case Settlement when 'Y' then pointcost else 0 end) SettlementY,sum(case Settlement when 'Y' then 0 else pointcost end) SettlementN,'" + condition.StartTime.ToString() + " ~ " + strEndDate + "' as DateString");
            sql.Append("select d.Area,d.Region,d.DealerId,d.Name,sum(rewardpoints) RewardPoints,sum(consumepoints) ConsumePoints,sum(PointCost) PointCost,sum(case Settlement when 'Y' then pointcost else 0 end) SettlementY,sum(case Settlement when 'Y' then 0 else pointcost end) SettlementN,'" + condition.StartTime.ToString() + " ~ " + strEndDate + "' as DateString ");
            sql.Append(@" from CS_Consume c 
                        left join Membership m on c.userid = m.id
                        left join CS_CarDealerShip d on c.dealerid = d.dealerid ");
            sql.AppendFormat(" where {0} ", condition.ToWhere());
            sql.Append(" group by d.area,d.region,d.dealerid,d.Name");

            return DbHelp.Query<IntegralCountReportInfo>(sql.ToString(), condition);
        }
        /// <summary>
        /// 验证数据是否重复（店代码、结算开始时间 结束时间等信息）
        /// </summary>
        /// <param name="dealerId">店代码</param>
        /// <param name="startTime">结算开始时间</param>
        /// <param name="endTime">结算结束时间</param>
        /// <returns>true:有重复</returns>
        public bool validateData(string dealerId, string startTime,string endTime)
        {
            var dateStart = Convert.ToDateTime(startTime);
            var dateEnd = DateTime.Parse(endTime.ToString()).AddMonths(1).AddSeconds(-1);
            string sql = "select COUNT(1) from SettleDealerPoint where DateStart = '" + dateStart + "' and DateEnd = '" + dateEnd + "' ";
            if (!string.IsNullOrEmpty(dealerId))
            {
                sql += " and DealerId = '" + dealerId + "'";
            }

            return DbHelp.ExecuteScalar<int>(sql) > 0;
        }
        /// <summary>
        /// 特约店积分结算核对 报表数据
        /// 在SettleDealerPoint表中查看积分结算核对报表 
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public IEnumerable<IntegralCountReportInfo> FindDealerCountList(IntegralCountReportCondition condition)
        {
            StringBuilder sql = new StringBuilder();
            //if(condition.EndTime.HasValue)
            //{
            //    condition.EndTime = condition.EndTime.Value.AddDays(1).AddSeconds(-1);
            //}
            string strEndDate = string.Empty;
            if (condition.EndTime.HasValue)
            {
                strEndDate = DateTime.Parse(condition.EndTime.ToString()).AddMonths(1).AddSeconds(-1).ToString();
            }
            sql.Append("select distinct c.Area,c.Region,c.DealerId,c.SettlementState,c.ApproveStatus,c.RewardPoints,c.ConsumePoints,c.PointCost,c.SettlementY,c.SettlementN,convert(varchar,c.DateStart,120) as DateStart,convert(varchar,c.DateEnd,120) as DateEnd, convert(varchar,DateStart,120)+' ~ '+convert(varchar,DateEnd,120) as DateString");
            sql.Append(@" from SettleDealerPoint c");
            sql.AppendFormat(" where {0}", condition.ToDealerWhere());
            return DbHelp.Query<IntegralCountReportInfo>(sql.ToString(), condition);
        }
        /// <summary>
        /// 会员积分结算 确认
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="SettlementState"></param>
        public void UpdateDealerSettlementSeate(string dealerId, string startDate, string endDate, int SettlementState)
        {
            BMDb db = BMDb.GetInstance();
            int approveStatus = 0;
            if (SettlementState == 1)
            {
                approveStatus = 1;
            }
            if (SettlementState == 3)
            {
                approveStatus = 2;
            }
            //更改 SettleDealerPoint 表 的结算状态
            using (var scope = db.GetTransaction())
            {

                string sql = string.Format("update SettleDealerPoint set SettlementState={0},ApproveStatus={1} where ", SettlementState, approveStatus);
                if (!string.IsNullOrEmpty(dealerId))
                {
                    sql += " DealerId = '" + dealerId + "'";
                }
                if (!string.IsNullOrEmpty(startDate))
                {
                    sql += " AND DateStart >= '" + Convert.ToDateTime(startDate) + "'";
                }

                if (!string.IsNullOrEmpty(endDate))
                {
                    sql += " AND DateEnd <= '" + DateTime.Parse(endDate.ToString()).AddMonths(1).AddSeconds(-1) + "'";
                }

                db.Execute(sql);
                scope.Complete();
            }
            // 更改 表的 结算状态
            using(var scope=db.GetTransaction())
            {
                string sql = string.Format("UPDATE CS_Consume SET  SettlementState={0},ApproveStatus={1} WHERE Settlement = 'Y' ", SettlementState, approveStatus);
                if(!string.IsNullOrEmpty(dealerId))
                {
                    sql += " and  DealerId='" + dealerId + "'";
                }
                if(!string.IsNullOrEmpty(startDate))
                {
                    sql += " and ConsumeDate >='" + Convert.ToDateTime(startDate) + "'";
                }
                if(!string.IsNullOrEmpty(endDate))
                {
                    sql += " and ConsumeDate<='" + DateTime.Parse(endDate.ToString()).AddMonths(1).AddSeconds(-1) + "'";
                }
                db.Execute(sql);
                scope.Complete();
            }
        }
        public IEnumerable<QuestionnaireVisitor> FindQuestionnaireVisitorList(QuestionnaireVisitorCondition condition, PageData pageData, out int totalCount)
        {
            StringBuilder sql = new StringBuilder();
            if (condition.MemberLevel == null)
            {
                sql.AppendLine("select count(v.Id) from CS_QuestionnaireVisitor v");
                sql.AppendFormat(" where {0}", condition.ToWhere());
                totalCount = DbHelp.ExecuteScalar<int>(sql.ToString());
                sql.Clear();

                sql.AppendFormat("select distinct top {0} * from CS_QuestionnaireVisitor v", pageData.Size);
                sql.AppendFormat(" where {0}", condition.ToWhere());
                sql.AppendLine(" and v.Id > (");
                sql.AppendFormat(" select isnull(MAX(t1.Id),0) from ( select top {0} v.Id", pageData.Index);
                sql.AppendFormat(" where {0}", condition.ToWhere());
                sql.AppendLine(" order by v.Id) as t1");
                sql.AppendLine(" ) order by v.Id");
                IEnumerable<QuestionnaireVisitor> qvList = DbHelp.Query<QuestionnaireVisitor>(sql.ToString());
                sql.Clear();

                sql.AppendLine("select count(qv.Id) from CS_MemberForQuestionnaireManage mfqm");
                sql.AppendLine(" left join CS_QuestionnaireVisitor qv on qv.Id = mfqm.ContactId");
                sql.AppendLine(" left join Membership ms on ms.Id = mfqm.MemberId");
                sql.AppendFormat(" where {0}", condition.ToMemberWhere());

                totalCount = totalCount + DbHelp.ExecuteScalar<int>(sql.ToString());
                sql.Clear();

                sql.AppendFormat("select distinct top {0} qv.*,ms.VIN,ms.IdentityNumber,ms.MLevel,ms.CreatedPerson from CS_MemberForQuestionnaireManage mfqm", pageData.Size);
                sql.AppendLine(" left join CS_QuestionnaireVisitor qv on qv.Id = mfqm.ContactId");
                sql.AppendLine(" left join Membership ms on ms.Id = mfqm.MemberId");
                sql.AppendFormat(" where {0}", condition.ToMemberWhere());
                sql.AppendLine(" and qv.Id > (");
                sql.AppendFormat(" select isnull(MAX(t1.Id),0) from ( select top {0} qv.Id", pageData.Index);
                sql.AppendLine("  from CS_MemberForQuestionnaireManage mfqm left join CS_QuestionnaireVisitor qv on qv.Id = mfqm.ContactId");
                sql.AppendLine(" left join Membership ms on ms.Id = mfqm.MemberId");
                sql.AppendFormat(" where {0}", condition.ToMemberWhere());
                sql.AppendLine(" order by qv.Id) as t1");
                sql.AppendLine(" ) order by qv.Id");

                IEnumerable<QuestionnaireVisitor> qmList = DbHelp.Query<QuestionnaireVisitor>(sql.ToString());

                List<QuestionnaireVisitor> result = new List<QuestionnaireVisitor>();
                result.AddRange(qvList);
                result.AddRange(qmList);
                return result.AsEnumerable();
                //qvList.
            }
            else
            {
                if (condition.MemberLevel == 0)
                {
                    sql.AppendLine("select count(v.Id) from CS_QuestionnaireVisitor v");
                    sql.AppendFormat(" where {0}", condition.ToWhere());
                    totalCount = DbHelp.ExecuteScalar<int>(sql.ToString());
                    sql.Clear();

                    sql.AppendFormat("select distinct top {0} * from CS_QuestionnaireVisitor v", pageData.Size);
                    sql.AppendFormat(" where {0}", condition.ToWhere());
                    sql.AppendLine(" and v.Id > (");
                    sql.AppendFormat(" select isnull(MAX(t1.Id),0) from ( select top {0} v.Id", pageData.Index);
                    sql.AppendFormat(" where {0}", condition.ToWhere());
                    sql.AppendLine(" order by v.Id) as t1");
                    sql.AppendLine(" ) order by v.Id");
                }
                else
                {
                    sql.AppendLine("select count(qv.Id) from CS_MemberForQuestionnaireManage mfqm");
                    sql.AppendLine(" left join CS_QuestionnaireVisitor qv on qv.Id = mfqm.ContactId");
                    sql.AppendLine(" left join Membership ms on ms.Id = mfqm.MemberId");
                    sql.AppendFormat(" where {0}", condition.ToMemberWhere());
                    totalCount = DbHelp.ExecuteScalar<int>(sql.ToString());
                    sql.Clear();

                    sql.AppendFormat("select distinct top {0} qv.*,ms.VIN,ms.IdentityNumber,ms.MLevel,ms.CreatedPerson from CS_MemberForQuestionnaireManage mfqm", pageData.Size);
                    sql.AppendLine(" left join CS_QuestionnaireVisitor qv on qv.Id = mfqm.ContactId");
                    sql.AppendLine(" left join Membership ms on ms.Id = mfqm.MemberId");
                    sql.AppendFormat(" where {0}", condition.ToMemberWhere());
                    sql.AppendLine(" and qv.Id > (");
                    sql.AppendFormat(" select isnull(MAX(t1.Id),0) from ( select top {0} qv.Id", pageData.Index);
                    sql.AppendLine("  from CS_MemberForQuestionnaireManage mfqm left join CS_QuestionnaireVisitor qv on qv.Id = mfqm.ContactId");
                    sql.AppendLine(" left join Membership ms on ms.Id = mfqm.MemberId");
                    sql.AppendFormat(" where {0}", condition.ToMemberWhere());
                    sql.AppendLine(" order by qv.Id) as t1");
                    sql.AppendLine(" ) order by qv.Id");
                }
                return DbHelp.Query<QuestionnaireVisitor>(sql.ToString());
            }
        }




        public int GetSjCount(DateTime day, DateTime endDay, int qId,bool isCsNew)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("select COUNT(Id) from Membership ms ");
            sql.AppendLine(" where");
            sql.AppendLine(" ms.MLevel=3");
            sql.AppendLine(" and ms.No is not null");
            if (isCsNew)
            {
                sql.AppendLine(" and ms.CreatedPerson = 'cs_questionnaire'");
            }
            sql.AppendFormat(" and ms.Id in (select MemberId from CS_MemberForQuestionnaireManage where QuestionnaireId = {0})", qId);
            sql.AppendFormat(" and ms.PhoneNumber in (select PhoneNumber from CS_QuestionnaireVisitor where CreateTime>='{0}' and CreateTime<'{1}')", day, endDay.AddDays(1));
            return DbHelp.ExecuteScalar<int>(sql.ToString());
        }

        public int GetPtCount(DateTime day, DateTime endDay, int qId, bool isCsNew)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("select COUNT(Id) from Membership ms ");
            sql.AppendLine(" where");
            sql.AppendLine(" ms.MLevel in (2,3)");
            sql.AppendLine(" and ms.No is null");
            if (isCsNew)
            {
                sql.AppendLine(" and ms.CreatedPerson = 'cs_questionnaire'");
            }
            sql.AppendFormat(" and ms.Id in (select MemberId from CS_MemberForQuestionnaireManage where QuestionnaireId = {0})", qId);
            sql.AppendFormat(" and ms.PhoneNumber in (select PhoneNumber from CS_QuestionnaireVisitor where CreateTime>='{0}' and CreateTime<'{1}')", day, endDay.AddDays(1));
            return DbHelp.ExecuteScalar<int>(sql.ToString());
        }

        public int GetFcCount(DateTime day, DateTime endDay, int qId)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("select COUNT(Id) from Membership ms ");
            sql.AppendLine(" where");
            sql.AppendLine(" ms.MLevel = 1");
            sql.AppendFormat(" and ms.Id in (select MemberId from CS_MemberForQuestionnaireManage where QuestionnaireId = {0})", qId);
            sql.AppendFormat(" and ms.PhoneNumber in (select PhoneNumber from CS_QuestionnaireVisitor where CreateTime>='{0}' and CreateTime<'{1}')", day, endDay.AddDays(1));
            return DbHelp.ExecuteScalar<int>(sql.ToString());
        }

        public int GetTotalCount(DateTime day, DateTime endDay, int qId)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("select COUNT(Id) from CS_QuestionnaireVisitor ");
            sql.AppendLine(" where");
            sql.AppendFormat("QuestionnaireId = {0}", qId);
            sql.AppendFormat(" and CreateTime>='{0}' and CreateTime<'{1}'", day, endDay.AddDays(1));
            return DbHelp.ExecuteScalar<int>(sql.ToString());
        }


        public IEnumerable<AnswerReportInfo> FindAnswerList(int questionnarieId)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("select csq.QContent QContent,x.oName OName,csq.Sort Sort,z.* from CS_Question csq");
            sql.AppendLine(" left join CS_Answer csa on csa.ParentId = csq.Id");
            sql.AppendLine(" cross apply (select * from GetPhoneNumber(csa.MemberId)) as z");
            sql.AppendLine(" cross apply  (select * from GetAllOption(csa.AContent,',',csq.[Type],csq.Id)) as x");
            sql.AppendFormat(" where csq.ParentId = {0} and csq.State = 1 and Name is not null", questionnarieId);
            return DbHelp.Query<AnswerReportInfo>(sql.ToString());
        }

        public IEnumerable<AnswerReportInfo> FindAnswerJzList(int questionnarieId)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("select csq.QContent QContent,x.QContent CQContent,y.oName OName,csq.Sort Sort,z.* from CS_Question csq");
            sql.AppendLine(" cross apply (select * from CS_Question csqs where csqs.ParentId = csq.Id and csqs.State = 1) as x");
            sql.AppendLine(" left join CS_Answer csa on csa.ParentId = x.Id");
            sql.AppendLine(" cross apply (select * from GetPhoneNumber(csa.MemberId)) as z");
            sql.AppendLine(" cross apply (select * from GetAllOption(csa.AContent,',',x.Type,x.Id)) as y");
            sql.AppendFormat(" where csq.ParentId = {0} and csq.State = 1 and Name is not null", questionnarieId);
            return DbHelp.Query<AnswerReportInfo>(sql.ToString());
        }

        public IEnumerable<AnswerReportInfo> FindAnswerListCs(int questionnarieId)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("select csq.QContent QContent,x.oName OName,csq.Sort Sort,ms.RealName Name,ms.IdentityNumber,ms.VIN,ms.PhoneNumber,ms.MLevel from CS_Question csq");
            sql.AppendLine(" left join CS_Answer csa on csa.ParentId = csq.Id");
            sql.AppendLine(" left join Membership ms on ms.Id = csa.MemberId");
            sql.AppendLine(" cross apply  (select * from GetAllOption(csa.AContent,',',csq.[Type],csq.Id)) as x");
            sql.AppendFormat(" where csq.ParentId = {0} and csq.State = 1", questionnarieId);
            return DbHelp.Query<AnswerReportInfo>(sql.ToString());
        }

        public IEnumerable<AnswerReportInfo> FindAnswerJzListCs(int questionnarieId)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("select csq.QContent QContent,y.oName OName,csq.Sort Sort,ms.RealName Name,ms.IdentityNumber,ms.VIN,ms.PhoneNumber,ms.MLevel from CS_Question csq");
            sql.AppendLine(" cross apply (select * from CS_Question csqs where csqs.ParentId = csq.Id and csqs.State = 1) as x");
            sql.AppendLine(" left join CS_Answer csa on csa.ParentId = csq.Id");
            sql.AppendLine(" left join Membership ms on ms.Id = csa.MemberId");
            sql.AppendLine(" cross apply  (select * from GetAllOption(csa.AContent,',',x.Type,x.Id)) as y");
            sql.AppendFormat(" where csq.ParentId = {0} and csq.State = 1", questionnarieId);
            return DbHelp.Query<AnswerReportInfo>(sql.ToString());
        }

        public IEnumerable<AnswerReport> FindAnswerListCSNew(int questionnarieId)
        {
            string sql = @"SELECT Q.Id,Q.QContent,A.MemberId,dbo.GetAnswer(A.AContent,Q.Type) AS Answer,M.UserName,M.MLevel,M.IdentityNumber,M.VIN,M.RealName
                            FROM CS_Question Q,CS_Answer A ,Membership M 
                            WHERE A.ParentId = Q.Id AND A.MemberId = M.Id AND Q.ParentId = {0} AND Q.State = 1 ORDER BY Q.Sort,A.MemberId";
            return DbHelp.Query<AnswerReport>(string.Format(sql, questionnarieId));
        }

        public IEnumerable<string> FindAnswerListMember(int questionnarieId)
        {
            string sql = @"SELECT DISTINCT(MemberId) FROM CS_Answer WHERE ParentId IN(SELECT Id FROM CS_Question WHERE ParentId = {0} AND State = 1)";
            return DbHelp.Query<string>(string.Format(sql, questionnarieId), commandTimeout : 10000000);
        }
        //积分下发报表
        public DataTable GetUserintegralReport(string startTime, string endTime)
        {
            try
            {
                DBHelper dbHelper = new DBHelper();
                SqlParameter[] paramsArray = new SqlParameter[2] { new SqlParameter("@beginTime", startTime), new SqlParameter("@endTime", endTime) };
                var data = dbHelper.ExecDataSetProc("GetUserintegralReport", paramsArray);
                return data.Tables[0];

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable GetMemberResourceReport(string startTime, string endTime)
        {
            try
            {
                DBHelper dbHelper = new DBHelper();
                SqlParameter[] paramsArray = new SqlParameter[2] { new SqlParameter("@begin", startTime), new SqlParameter("@end", endTime) };
                var data = dbHelper.ExecDataSetProc("MemshipResour", paramsArray);
                return data.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //获取经销商购车且入会的报表数据
        public DataTable GetMemberDearBuyCarReport(string startTime, string endTime, string tableName)
        {
            try
            {
                DBHelper dbHelper = new DBHelper();

                SqlParameter[] paramsArray = new SqlParameter[2] { new SqlParameter("@BeginDay", startTime), new SqlParameter("@EndDay", endTime) };
                var data = dbHelper.ExecDataSetProc(tableName, paramsArray);              
                  return data.Tables[0];       
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //入会率
        public DataTable GetMemberJoinDate(DateTime startTime, DateTime endTime, DateTime buyStartTime, DateTime buyEndTime, string region, string tableName)
        {
            try
            {
                DBHelper dbHelper = new DBHelper();

                SqlParameter[] paramsArray = new SqlParameter[5] { new SqlParameter("@BeginDay", startTime), new SqlParameter("@EndDay", endTime) ,new SqlParameter("@buyStartTime", buyStartTime),new SqlParameter("@buyEndTime", buyEndTime), new SqlParameter("@region", region) };
                var data = dbHelper.ExecDataSetProc(tableName, paramsArray);
                return data.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        //活动卡券已领取未核销
        public DataTable AcitivityCard_NoCancel(string  startTime, string  endTime, string tableName, string AcitivityName)
        {
            try
            {
                DBHelper dbHelper = new DBHelper();

                SqlParameter[] paramsArray = new SqlParameter[3] { new SqlParameter("@BeginDay", startTime), new SqlParameter("@EndDay", endTime), new SqlParameter("@AcitivityName", AcitivityName) };
                var data = dbHelper.ExecDataSetProc(tableName, paramsArray);
                return data.Tables[0];

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// 获取活动名称
        /// </summary>
        /// <param name="name">活动名称</param>
        /// <returns></returns>
        public IEnumerable<SCServiceCardType> GetScServiceActivitName()
        {
            string sql = "select ActivityType from CustomCardInfo group by ActivityType";
            return DbHelp.Query<SCServiceCardType>(sql);
        }
    }
}