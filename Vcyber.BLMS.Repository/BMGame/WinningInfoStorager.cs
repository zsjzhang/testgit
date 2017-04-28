using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.IRepository;
using Vcyber.BLMS.Common;
using PetaPoco;
using Vcyber.BLMS.Repository.Entity.Generated;

namespace Vcyber.BLMS.Repository
{
    public class WinningInfoStorager : IWinningInfoStorager
    {
        /// <summary>
        /// 获取指定活动的获奖名单
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns></returns>
        public IEnumerable<WinningInfo> GetWinningInfosByActivityId(int activityId)
        {
            StringBuilder sql = new StringBuilder();
            if (activityId == 0)
            {
                sql.Append("select * from WinningInfo");
            }
            else
            {
                sql.AppendFormat("select * from WinningInfo where ActivityId={0}", activityId);
            }
            return DbHelp.Query<WinningInfo>(sql.ToString());
        }
        /// <summary>
        /// 通过手机号判断用户是否获奖
        /// </summary>
        /// <param name="activityId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool IsWinningByUIdAndActicityId(int activityId, string userTel)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select count(*) from WinningInfo where ActivityId={0} and UserTel='{1}'", activityId, userTel);
            int count = DbHelp.ExecuteScalar<int>(sql.ToString());
            if (count > 0) return true;
            else return false;
        }

        /// <summary>
        /// 获取单条用户获奖信息
        /// </summary>
        /// <param name="activityId"></param>
        /// <param name="userTel"></param>
        /// <returns></returns>
        public WinningInfo GetWinningByTelAndActicityId(int activityId, string userTel)
        {
            var sql = Sql.Builder
                .Append("SELECT pi.PrizeLevel,pi.Title, wi.Id,wi.ActivityId,wi.UserId,wi.UserName,")
                .Append("wi.UserTel,wi.UserType,wi.Vin,wi.Province,wi.City,wi.Area,wi.Address,")
                .Append("wi.ImgAddress,wi.InvoiceNO,wi.State,wi.PrizesId,wi.UpdateTime,wi.CreateTime")
                .Append("FROM dbo.WinningInfo AS wi")
                .Append("INNER JOIN dbo.PrizesInfo AS pi ON pi.id = wi.PrizesId")
                .Append("WHERE wi.ActivityId = @0 AND wi.UserTel = @1", activityId, userTel);
            return PocoHelper.CurrentDb().Query<WinningInfo>(sql).FirstOrDefault();
        }
        /// <summary>
        /// 获取单条用户获奖信息by userid
        /// </summary>
        /// <param name="activityId">活动ID</param>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        public WinningInfo GetWinningByUserIdAndActicityId(int activityId, string userId)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select wi.*,pi.PrizeLevel,pi.Title from WinningInfo wi");
            sql.Append(" INNER JOIN dbo.PrizesInfo AS pi ON pi.Id = wi.PrizesId");
            sql.AppendFormat(" where wi.ActivityId={0} and wi.UserId='{1}'", activityId, userId);
            return DbHelp.QueryOne<WinningInfo>(sql.ToString());
        }
        public bool IsWinningByActivity(int activityId, string openId)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select Count(*) from WinningInfo where ActivityId={0} and InvoiceNO='{1}'", activityId, openId);
            return DbHelp.ExecuteScalar<int>(sql.ToString()) > 0;
        }

        /// <summary>
        /// 根据条件获取活动的获奖信息
        /// </summary>
        /// <param name="activityId"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public IEnumerable<WinningInfo> GetWinningsByWhere(int activityId, string where)
        {
            StringBuilder sql = new StringBuilder();
            if (where == null || where == "")
            {
                sql.AppendFormat("select * from WinningInfo where ActivityId={0}", activityId);
            }
            else
            {
                if (activityId == 0)
                {
                    sql.AppendFormat("select wi.Id,pi.PrizeLevel from WinningInfo AS wi INNER JOIN dbo.PrizesInfo AS pi ON pi.Id = wi.PrizesId where {0}", where);
                }
                else
                {
                    sql.AppendFormat("select * from WinningInfo where ActivityId={0} and {1}", activityId, where);                
                }                
            }

            return DbHelp.Query<WinningInfo>(sql.ToString());
        }
        public int GetWinningsCount(int activityId)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select count(id) from WinningInfo where ActivityId={0} and State = 1", activityId);
            return DbHelp.ExecuteScalar<int>(sql.ToString());
        }
        public IEnumerable<WinningInfo> GetWinningsList(string[] activityIds, string phonenumber)
        {
            StringBuilder sql = new StringBuilder();
            if (phonenumber == null || phonenumber == "")
            {
                sql.AppendFormat("select  m.PhoneNumber,b.Title  from WinningInfo as a  inner join PrizesInfo as b on a.PrizesId=b.id  inner join Membership m on a.UserId=m.Id where a.ActivityId ={0}", activityIds[0]);
            }
            else
            {
                sql.AppendFormat("select  m.PhoneNumber as PhoneNumber, b.Title  from WinningInfo as a  inner join PrizesInfo as b on a.PrizesId=b.id  inner join Membership m on a.UserId=m.Id where a.ActivityId ={0} and m.PhoneNumber='{1}'", activityIds[0], phonenumber);
            }
            return DbHelp.Query<WinningInfo>(sql.ToString());
        }
        /// <summary>
        /// 添加获奖记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int AddWinningInfo(WinningInfo entity)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("Insert into WinningInfo(ActivityId,UserId,UserName,UserTel,UserType,Vin,Province,City,Area,Address,ImgAddress,InvoiceNO,State,PrizesId,UpdateTime)");
            sql.Append("values(@ActivityId,@UserId,@UserName,@UserTel,@UserType,@Vin,@Province,@City,@Area,@Address,@ImgAddress,@InvoiceNO,@State,@PrizesId,@UpdateTime);SELECT @@identity");
            return DbHelp.ExecuteScalar<int>(sql.ToString(), entity);
        }

        /// <summary>
        /// 分页获取获奖名单
        /// </summary>
        /// <param name="activityId"></param>
        /// <param name="pageData"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public IEnumerable<WinningInfo> GetWinningInfoByActivityId(int activityId, PageData pageData, out int total)
        {
            StringBuilder sql = new StringBuilder();
            if (activityId == 0)
            {
                sql.Append(" select count(1) from WinningInfo");
                total = DbHelp.ExecuteScalar<int>(sql.ToString());
                sql.Clear();

                sql.AppendFormat(" select top {0} * from WinningInfo ", pageData.Size);
                sql.Append("where  WinningInfo.Id not in( ");
                sql.AppendFormat(" select top {0} WinningInfo.Id from WinningInfo order by WinningInfo.Id asc", pageData.Index);
                sql.Append(" )order by WinningInfo.Id asc ");
                return DbHelp.Query<WinningInfo>(sql.ToString());
            }
            else
            {
                sql.AppendFormat(" select count(1) from WinningInfo where ActivityId=@activityId");
                total = DbHelp.ExecuteScalar<int>(sql.ToString(), new { @activityId = activityId });
                sql.Clear();

                sql.AppendFormat(" select top {0} * from WinningInfo where ActivityId=@ActivityId", pageData.Size);
                sql.Append(" and WinningInfo.Id not in( ");
                sql.AppendFormat(" select top {0} WinningInfo.Id from WinningInfo where ActivityId=@ActivityId order by WinningInfo.Id asc", pageData.Index);
                sql.Append(" )order by WinningInfo.Id asc ");
                return DbHelp.Query<WinningInfo>(sql.ToString(), new { ActivityId = activityId });
            }

        }
        /// <summary>
        /// 根据ID获取中奖信息
        /// 贾锡安2015-09-09 11:00:11
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public WinningInfo GetWinningInfo(int id)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from WinningInfo where Id={0}", id);
            return DbHelp.QueryOne<WinningInfo>(sql.ToString());
        }


        /// <summary>
        /// 修改中奖纪录
        /// </summary>
        public bool UpdateForUserId(WinningInfo obj)
        {
            string sql = @"UPDATE dbo.WinningInfo SET UserName = @UserName,UserTel = @UserTel,Province = @Province,City = @City,[Address] = @Address,[State] = 1,UpdateTime = GETDATE()
                            WHERE ActivityId = @ActivityId AND UserID = @UserID AND PrizesId = @PrizesId";
            return DbHelp.Execute(sql, new
            {
                @ActivityId = obj.ActivityId,
                @UserId = obj.UserId,
                @UserName = obj.UserName,
                @UserTel = obj.UserTel,
                @Province = obj.Province,
                @City = obj.City,
                @Address = obj.Address,
                @PrizesId = obj.PrizesId              
            }) > 0;
        }
        /// <summary>
        /// 修改中奖纪录
        /// </summary>
        public bool UpdateForId(int id, string userName, string phone, string address)
        {
            var sql = Sql.Builder
                .Append("UPDATE dbo.WinningInfo SET UserName = @0,UserTel = @1,Address = @2,State = 1,Updatetime = GETDATE() WHERE id = @3", userName, phone, address, id);
            var flag = PocoHelper.CurrentDb().Execute(sql) > 0;
            return flag;
        }
        /// <summary>
        /// 修改中奖纪录
        /// 贾锡安2015-09-09 11:13:06
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool UpdateWinningInfo(WinningInfo entity)
        {
            string sql = @"update WinningInfo set ActivityId=@ActivityId,UserId=@UserId,UserName=@UserName,
UserTel=@UserTel,UserType=@UserType,Vin=@Vin,Province=@Province,City=@City,Area=@Area,Address=@Address,ImgAddress=@ImgAddress,InvoiceNO=@InvoiceNO,
State=@State,PrizesId=@PrizesId,UpdateTime=@UpdateTime where Id=@Id;";
            return DbHelp.Execute(sql, new
            {
                @ActivityId = entity.ActivityId,
                @UserId = entity.UserId,
                @UserName = entity.UserName,
                @UserTel = entity.UserTel,
                @UserType = entity.UserType,
                @Vin = entity.Vin,
                @Province = entity.Province,
                @City = entity.City,
                @Area = entity.Area,
                @Address = entity.Address,
                @ImgAddress = entity.ImgAddress,
                @InvoiceNO = entity.InvoiceNO,
                @State = entity.State,
                @PrizesId = entity.PrizesId,
                @UpdateTime = DateTime.Now,
                @Id = entity.Id,
            }) > 0;
        }


        public Membership GetMembershipByNameAndTel(string name, string tel)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from Membership where RealName={0} and PhoneNumber={1}", name, tel);
            return DbHelp.QueryOne<Membership>(sql.ToString());
        }
        /// <summary>
        /// 获取奖品的每天的使用量
        /// </summary>
        /// <param name="activityId">活动ID</param>        
        /// <returns>奖品使用量列表</returns>
        public IEnumerable<PrizesInfo> GetPrizeUse(int activityId)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT wi.PrizesId AS Id,COUNT(1) AS CyclesUnuseNum FROM dbo.WinningInfo AS wi ");
            sql.AppendFormat("WHERE wi.ActivityId = {0} AND DATEDIFF(day,wi.CreateTime,GETDATE()) = 0 ",activityId);
            sql.Append("GROUP BY wi.PrizesId");
            return DbHelp.Query<PrizesInfo>(sql.ToString());
        }
        /// <summary>
        /// 获取所中的记录
        /// </summary>
        /// <param name="activityId">活动ID</param>
        /// <param name="userId">用户ID</param>
        /// <param name="prizeId">奖品ID</param>
        /// <returns>中奖记录</returns>
        public WinningInfo GetWinPrize(int activityId, string userId, int prizeId)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("SELECT * FROM dbo.WinningInfo WHERE ActivityId = {0} AND UserID = '{1}'",activityId,userId,prizeId);
            if (prizeId > 0)
            {
                sql.AppendFormat(" AND PrizesId = {0}", prizeId);
            }
            return DbHelp.QueryOne<WinningInfo>(sql.ToString());
        }
    }
}
