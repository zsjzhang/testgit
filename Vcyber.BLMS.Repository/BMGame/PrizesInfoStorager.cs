using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.IRepository;
using Vcyber.BLMS.Common;

namespace Vcyber.BLMS.Repository
{
    public class PrizesInfoStorager : IPrizesInfoStorager
    {
        /// <summary>
        /// 获取活动奖品列表
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns></returns>
        public IEnumerable<PrizesInfo> GetPrizesInfosByActivity(int activityId)
        {
            StringBuilder sql = new StringBuilder();
            if (activityId == 0)
            {
                sql.Append("select * from PrizesInfo");
            }
            else
            {
                sql.AppendFormat("select * from PrizesInfo where ActivityId={0}", activityId);
            }

            return DbHelp.Query<PrizesInfo>(sql.ToString());
        }
        /// <summary>
        /// 获取景区门票活动奖品列表
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns></returns>
        public IEnumerable<PrizesInfo> GetPrizesUsedNumByActivity(int activityId)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select p.Title,p.TotalNum,p.Id,(select count(0) from WinningInfo where PrizesId = p.id and CreateTime > '2017-03-07 00:00:00.000') as UsedNum from PrizesInfo p where ActivityId={0}", activityId);
            return DbHelp.Query<PrizesInfo>(sql.ToString());
        }
        /// <summary>
        /// 获取活动奖品列表
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns></returns>
        public IEnumerable<PrizesInfo> GetPrizesInfosByActivity(int activityId, PageData pageData, out int total)
        {
            StringBuilder sql = new StringBuilder();
            if (activityId > 0)
            {
                sql.AppendFormat(" select count(1) from PrizesInfo where ActivityId=@activityId");
                total = DbHelp.ExecuteScalar<int>(sql.ToString(), new { @activityId = activityId });
                sql.Clear();

                sql.AppendFormat(" select top {0} * from PrizesInfo where ActivityId=@ActivityId", pageData.Size);
                sql.Append(" and PrizesInfo.Id not in( ");
                sql.AppendFormat(" select top {0} PrizesInfo.Id from PrizesInfo where ActivityId=@ActivityId order by PrizesInfo.Id asc", pageData.Index);
                sql.Append(" )order by PrizesInfo.Id asc ");
                return DbHelp.Query<PrizesInfo>(sql.ToString(), new { ActivityId = activityId });
            }
            else
            {
                sql.Append(" select count(1) from PrizesInfo ");
                total = DbHelp.ExecuteScalar<int>(sql.ToString());
                sql.Clear();

                sql.AppendFormat(" select top {0} * from PrizesInfo", pageData.Size);
                sql.Append(" where PrizesInfo.Id not in( ");
                sql.AppendFormat(" select top {0} PrizesInfo.Id from PrizesInfo order by PrizesInfo.Id asc", pageData.Index);
                sql.Append(" )order by PrizesInfo.Id asc ");
                return DbHelp.Query<PrizesInfo>(sql.ToString());
            }
        }

        /// <summary>
        /// 添加奖品
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int AddPrizesInfo(PrizesInfo entity)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("Insert into PrizesInfo(ActivityId,Title,Price,TotalNum,UsedNum,PrizeLevel,PrizeFlag,Img,CyclesFlag,CyclesNum,CyclesUnuseNum,Rate,UpdateTime)");
            sql.AppendFormat("values(@ActivityId,@Title,@Price,@TotalNum,@UsedNum,@PrizeLevel,@PrizeFlag,@Img,@CyclesFlag,@CyclesNum,@CyclesUnuseNum,@Rate,@UpdateTime);SELECT @@identity");
            return DbHelp.ExecuteScalar<int>(sql.ToString(), entity);
        }

        public PrizesInfo GetPrizeInfoMode(int prizeId)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from PrizesInfo where Id={0}", prizeId);
            return DbHelp.QueryOne<PrizesInfo>(sql.ToString());
        }

        public Membership GetMembershipMode(string phone)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select MLevel from Membership where PhoneNumber='{0}'", phone);
            return DbHelp.QueryOne<Membership>(sql.ToString());
        }

        /// <summary>
        /// 获取有效的奖项
        /// 贾锡安2015-09-09 17:09:07
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns></returns>
        public List<PrizesInfo> GetPrizesInfoNotNull(int activityId)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat(@"select * from PrizesInfo where 
ActivityId={0} and ((TotalNum>UsedNum 
and ((CyclesUnuseNum>0 and CyclesFlag>0) or CyclesFlag=0)) or (TotalNum=0 and CyclesFlag=0))", activityId);
            return DbHelp.Query<PrizesInfo>(sql.ToString()).ToList();
        }
        /// <summary>
        /// 修改奖品
        /// 贾锡安2015-09-09 17:06:34
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool UpdatePrizesInfo(PrizesInfo entity)
        {
            string sql = @"update PrizesInfo set Title=@Title,Price=@Price,TotalNum=@TotalNum,UsedNum=@UsedNum,PrizeFlag=@PrizeFlag,Img=@Img,
CyclesFlag=@CyclesFlag,CyclesNum=@CyclesNum,CyclesUnuseNum=@CyclesUnuseNum,PrizeLevel=@PrizeLevel,Rate=@Rate,UpdateTime=@UpdateTime where Id=@Id;";
            return DbHelp.Execute(sql, new
            {
                @Title = entity.Title,
                @Price = entity.Price,
                @TotalNum = entity.TotalNum,
                @UsedNum = entity.UsedNum,
                @PrizeFlag = entity.PrizeFlag,
                @PrizeLevel = entity.PrizeLevel,
                @Img = entity.Img,
                @CyclesFlag = entity.CyclesFlag,
                @CyclesNum = entity.CyclesNum,
                @CyclesUnuseNum = entity.CyclesUnuseNum,
                @Rate = entity.Rate,
                @UpdateTime = DateTime.Now,
                @Id = entity.Id,
            }) > 0;
        }

        /// <summary>
        /// 奖品减库存
        /// </summary>
        /// <param name="id">奖品ID</param>
        public void PrizeMinus(int id) 
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("UPDATE dbo.PrizesInfo SET UsedNum = UsedNum+1 WHERE id = {0}", id);
            DbHelp.Execute(sql.ToString());
        }

        public Membership GetUserInfoById(string guid)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("Select * from Membership where Id='{0}'", guid);
            return DbHelp.QueryOne<Membership>(sql.ToString());
        }

        public int DelPrizesInfo(int id)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("delete from PrizesInfo where Id=@id");
            return DbHelp.Execute(sql.ToString(), new { @id = id });
        }

        public int GetCyclesUnuseNumById(int prizeId, int activityId)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select CyclesUnuseNum from PrizesInfo where ActivityId={0} and Id={1}", activityId, prizeId);
            return DbHelp.ExecuteScalar<int>(sql.ToString());
        }

        public bool CutDownPrizeCyclesUnuseNum(int prizeId, int activityId, int num)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("update PrizesInfo set CyclesUnuseNum=CyclesUnuseNum-{0} where ActivityId={1} and Id={2}", num, activityId, prizeId);
            return DbHelp.Execute(sql.ToString()) > 0;
        }
    }


}
