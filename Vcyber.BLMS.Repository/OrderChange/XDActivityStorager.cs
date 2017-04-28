using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.IRepository;

namespace Vcyber.BLMS.Repository
{
    public class XDActivityStorager : IXDActivityStorager
    {
        public XDActivityStorager() { }
        public XDActivity GetXDActivityByActivityId(int activityId)
        {
            string sql = "SELECT * FROM XD_Activity WHERE ActivityId=@ActivityId AND IsValid=1;";
            return DbHelp.QueryOne<XDActivity>(sql, new { ActivityId = activityId});
        }
        /// <summary>
        ///  前台 获取置换活动列表
        /// </summary>
        /// <param name="activityType"></param>
        /// <param name="activityId"></param>
        /// <returns></returns>
        public IEnumerable<XDActivity> GetXDActivityList(int activityType,int activityId=0) 
        {
//            string sql = @"select xa.ActivityId,xa.ActivityName,xa.ActivityImage,xa.ActivitySubImage,xa.ActivityUrl,xa.ActivityStartTime,xa.ActivityCarType,xa.ActivityEndTime,xa.ActivityStatus,tmp.ActivityLotteryName from XD_Activity xa  left join (
//select ActivityId,
//STUFF(
//     ( 
//      SELECT ','+ LotteryName FROM XD_Lottery b WHERE b.ActivityId =a.ActivityId FOR XML PATH('')
//     ),1 ,1, '') ActivityLotteryName 
//from XD_Lottery  a
//group by ActivityId
//)tmp  on tmp.ActivityId=xa.ActivityId
//where xa.IsValid=1 and xa.ActivityType=@ActivityType {0} order by xa.CreaterTime desc";
            string sql = @"select xa.ActivityId,xa.ActivityName,xa.ActivityImage,xa.ActivitySubImage,xa.ActivityUrl,xa.ActivityStartTime,xa.ActivityCarType,xa.ActivityEndTime,xa.ActivityStatus,xa.ActivityContent,tmp.ActivityLotteryName from XD_Activity xa  left join (
            select ActivityId,
            STUFF(
                 ( 
                  SELECT ','+ LotteryName FROM XD_Lottery b WHERE b.ActivityId =a.ActivityId FOR XML PATH('')
                 ),1 ,1, '') ActivityLotteryName 
            from XD_Lottery  a
            group by ActivityId
            )tmp  on tmp.ActivityId=xa.ActivityId
            where xa.IsValid=1 and IsDelete=0 and xa.ActivityType=@ActivityType {0} order by xa.CreaterTime desc";
            if (activityId != 0)
            {
                sql = string.Format(sql, " and xa.ActivityId=" + activityId,"{0}");
            }
            else 
            {
                sql = string.Format(sql,"","{0}");
            }
            return DbHelp.Query<XDActivity>(sql, new { ActivityType = activityType });
        }
        public void UpdateActivityLotteryBalanceCount(int activityId)
        {
            string sql = "UPDATE XD_Activity SET LotteryBalanceCount=LotteryBalanceCount-1 WHERE ActivityId=@ActivityId";
            DbHelp.Execute(sql, new { ActivityId = activityId});
        }
        #region 后台使用方法
        /// ====================================================================================================================================================
        /// <summary>
        /// 查询所有活动
        /// </summary>
        /// <param name="approveStatus"></param>
        /// <param name="displayStatus"></param>
        /// <param name="isHot"></param>
        /// <param name="dealer"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public IEnumerable<XDActivity> Select(int? approveStatus, int? displayStatus, int? isHot, string dealer, int pageIndex, int pageSize, out int totalCount)
        {
            StringBuilder conditionStr = new StringBuilder();
            //if (!string.IsNullOrEmpty(dealer))
            //{
            //    conditionStr.Append("and Dealer=@dealer");
            //}

            if (approveStatus != null && approveStatus >= 0)
            {

                conditionStr.AppendFormat(" and ActivityStatus={0}", approveStatus);

            }

            if (displayStatus != null && displayStatus >= 0)
            {

                conditionStr.AppendFormat(" and IsDisplay={0}", displayStatus);

            }
            //if (isHot != null && isHot >= 0)
            //{

            //    conditionStr.AppendFormat(" and IsHot={0}", isHot);

            //}

            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select count(1) from XD_Activity where IsDelete=0 ");
            sql.Append(conditionStr);
            totalCount = DbHelp.ExecuteScalar<int>(sql.ToString(), new { @dealer = dealer });
            sql.Clear();

            sql.AppendFormat("SELECT top {0} * FROM XD_Activity WHERE IsDelete=0", pageSize);
            sql.Append(conditionStr);
            sql.AppendFormat(" and ActivityId not in (select top {0} ActivityId from XD_Activity WHERE IsDelete=0", pageIndex);
            sql.Append(conditionStr);
            sql.Append(" ORDER BY CreaterTime DESC) ORDER BY CreaterTime DESC");
            return DbHelp.Query<XDActivity>(sql.ToString(), new { @dealer = dealer });
        }
        /// <summary>
        /// 查询未开始的活动
        /// </summary>
        /// <param name="approveStatus"></param>
        /// <param name="displayStatus"></param>
        /// <param name="isHot"></param>
        /// <param name="dealer"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public IEnumerable<XDActivity> SelectNotStart(int? approveStatus, int? displayStatus, int? isHot, string dealer, int pageIndex, int pageSize, out int totalCount)
        {
            StringBuilder conditionStr = new StringBuilder();
            if (!string.IsNullOrEmpty(dealer))
            {
                conditionStr.AppendFormat("and Dealer='{0}'", dealer);
            }

            if (approveStatus != null && approveStatus >= 0)
            {

                conditionStr.AppendFormat(" and IsApproved={0}", approveStatus);

            }
            if (displayStatus != null && displayStatus >= 0)
            {

                conditionStr.AppendFormat(" and IsDisplay={0}", displayStatus);

            }

            if (isHot != null && isHot >= 0)
            {

                conditionStr.AppendFormat(" and IsHot={0}", isHot);

            }
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select count(1) from XD_Activity where IsDelete=0 and ActivityStartTime>'{0}'", DateTime.Now);
            sql.Append(conditionStr);
            totalCount = DbHelp.ExecuteScalar<int>(sql.ToString());
            sql.Clear();

            sql.AppendFormat("SELECT top {0} * FROM XD_Activity WHERE IsDelete=0 and ActivityStartTime>'{1}'", pageSize, DateTime.Now);
            sql.Append(conditionStr);
            sql.AppendFormat(" and ActivityId not in (select top {0} ActivityId from XD_Activity WHERE IsDelete=0 and ActivityStartTime>'{1}'", pageIndex, DateTime.Now);
            sql.Append(conditionStr);
            sql.Append("ORDER BY CreaterTime DESC) ORDER BY CreaterTime DESC");
            return DbHelp.Query<XDActivity>(sql.ToString());
        }
        /// <summary>
        /// 查询进行中的活动
        /// </summary>
        /// <param name="approveStatus"></param>
        /// <param name="displayStatus"></param>
        /// <param name="isHot"></param>
        /// <param name="dealer"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public IEnumerable<XDActivity> SelectInProgress(int? approveStatus, int? displayStatus, int? isHot, string dealer, int pageIndex, int pageSize, out int totalCount)
        {
            StringBuilder conditionStr = new StringBuilder();
            if (!string.IsNullOrEmpty(dealer))
            {
                conditionStr.AppendFormat("and Dealer='{0}'", dealer);
            }

            if (approveStatus != null && approveStatus >= 0)
            {

                conditionStr.AppendFormat(" and IsApproved={0}", approveStatus);

            }
            if (displayStatus != null && displayStatus >= 0)
            {

                conditionStr.AppendFormat(" and IsDisplay={0}", displayStatus);

            }

            //if (isHot != null && isHot >= 0)
            //{

            //    conditionStr.AppendFormat(" and IsHot={0}", isHot);

            //}

            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select count(1) from XD_Activity where IsDelete=0 and ActivityStartTime<='{0}' and ActivityEndTime>='{0}'", DateTime.Now);
            sql.Append(conditionStr);
            totalCount = DbHelp.ExecuteScalar<int>(sql.ToString());
            sql.Clear();

            sql.AppendFormat("SELECT top {0} * FROM XD_Activity WHERE IsDelete=0 and ActivityStartTime<='{1}' and ActivityEndTime>='{1}'", pageSize, DateTime.Now);
            sql.Append(conditionStr);
            sql.AppendFormat(" and ActivityId not in (select top {0} ActivityId from XD_Activity WHERE IsDelete=0 and ActivityStartTime<='{1}' and ActivityEndTime>='{1}'", pageIndex, DateTime.Now);
            sql.Append(conditionStr);
            sql.Append("ORDER BY CreaterTime DESC) ORDER BY CreaterTime DESC");
            return DbHelp.Query<XDActivity>(sql.ToString());
        }
        /// <summary>
        /// 查询已结束的活动
        /// </summary>
        /// <param name="approveStatus"></param>
        /// <param name="displayStatus"></param>
        /// <param name="isHot"></param>
        /// <param name="dealer"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public IEnumerable<XDActivity> SelectFinished(int? approveStatus, int? displayStatus, int? isHot, string dealer, int pageIndex, int pageSize, out int totalCount)
        {
            StringBuilder conditionStr = new StringBuilder();
            if (!string.IsNullOrEmpty(dealer))
            {
                conditionStr.AppendFormat("and Dealer='{0}'", dealer);
            }

            if (approveStatus != null && approveStatus >= 0)
            {

                conditionStr.AppendFormat(" and IsApproved={0}", approveStatus);

            }
            if (displayStatus != null && displayStatus >= 0)
            {

                conditionStr.AppendFormat(" and IsDisplay={0}", displayStatus);

            }
            if (isHot != null && isHot >= 0)
            {

                conditionStr.AppendFormat(" and IsHot={0}", isHot);

            }

            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select count(1) from XD_Activity where IsDelete=0 and ActivityEndTime<'{0}'", DateTime.Now);
            sql.Append(conditionStr);
            totalCount = DbHelp.ExecuteScalar<int>(sql.ToString());
            sql.Clear();

            sql.AppendFormat("SELECT top {0} * FROM XD_Activity WHERE IsDelete=0 and ActivityEndTime<'{1}'", pageSize, DateTime.Now);
            sql.AppendFormat(" and ActivityId not in (select top {0} ActivityId from XD_Activity WHERE IsDelete=0 and ActivityEndTime<'{1}'", pageIndex, DateTime.Now);
            sql.Append(conditionStr);
            sql.Append(" ORDER BY CreaterTime DESC)  ORDER BY CreaterTime DESC");
            return DbHelp.Query<XDActivity>(sql.ToString());
        }

        /// <summary>
        /// 根据ID查询活动
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public XDActivity GetXDActivityById(int id)
        {
            string sql = "Select * From XD_Activity WHERE ActivityId=@Id and IsDelete=0";
            return DbHelp.QueryOne<XDActivity>(sql, new { @Id = id });
        }
        /// <summary>
        /// 创建置换活动
        /// </summary>
        /// <param name="activities"></param>
        /// <returns></returns>
        public int AddXDActivity(XDActivity activities)
        {
            string sql = @"Insert into XD_Activity(
                        ActivityName,ActivityTitle,ActivityType,ActivityPutType,ActivityImage,ActivitySubImage,ActivityCarType,
                        ActivityUrl,ActivityStartTime,ActivityEndTime,ActivityStatus,IsValid,CreaterName,CreaterTime,UpdaterName,UpdaterTime,ActivityContent,IsDelete) 
                values(@ActivityName,@ActivityTitle,@ActivityType,@ActivityPutType,@ActivityImage,@ActivitySubImage,@ActivityCarType,
                       @ActivityUrl,@ActivityStartTime,@ActivityEndTime,@ActivityStatus,@IsValid,
                       @CreaterName,@CreaterTime,@UpdaterName,@UpdaterTime,@ActivityContent,@IsDelete);SELECT @@identity";

            if(activities.ActivityStartTime>DateTime.Now)
            {
                activities.ActivityStatus = 2;
            }
            else if (activities.ActivityEndTime < DateTime.Now)
            {
                activities.ActivityStatus = 1;
            }
            else
            {
                activities.ActivityStatus = 0;
            }
            return DbHelp.ExecuteScalar<int>(sql, new
            {
                activities.ActivityName,
                activities.ActivityTitle,
                @ActivityType = 2,
                @ActivityPutType = 2,
                activities.ActivityImage,
                activities.ActivitySubImage,
                @ActivityCarType="",
                @ActivityUrl="",
                activities.ActivityStartTime,
                activities.ActivityEndTime,
                activities.ActivityStatus,
                activities.IsValid,
                activities.CreaterName,
                @CreaterTime=DateTime.Now,
                activities.UpdaterName,
                @UpdaterTime = DateTime.Now,
                activities.ActivityContent,
                @IsDelete=0
                //@IsApproved = (int)EApproveStatus.NoBegin,
                
            });
        }
        /// <summary>
        /// 编辑 更改活动
        /// </summary>
        /// <param name="activities"></param>
        /// <returns></returns>
        public bool UpdateActivities(XDActivity activities)
        {
            string sql = "Update XD_Activity set ActivityName =@ActivityName,ActivityTitle=@ActivityTitle,ActivityType=@ActivityType,ActivityPutType=@ActivityPutType,ActivityImage=@ActivityImage," +
                         "ActivitySubImage=@ActivitySubImage," +
                         "ActivityStartTime = @ActivityStartTime, ActivityEndTime= @ActivityEndTime,IsValid=@IsValid,ActivityStatus=@ActivityStatus, " +
                         "UpdaterName=@UpdaterName,UpdaterTime=@UpdaterTime,ActivityContent=@ActivityContent where ActivityId=@Id";

            //var approvestatus = activities.IsValid;
            //var apporveBy = activities.CreaterName;
            //var approveTime = activities.CreaterTime;

            //if (approvestatus != (int)EApproveStatus.NoBegin)
            //{
            //    approvestatus = (int)EApproveStatus.NoBegin;
            //    //apporveBy = activities.UpdateBy;
            //    //approveTime = DateTime.Now;
            //}
            //if (approveTime <= DateTime.MinValue)
            //{
            //    approveTime = (DateTime)SqlDateTime.MinValue;
            //}
            if (activities.ActivityStartTime > DateTime.Now)
            {
                activities.ActivityStatus = 2;
            }
            else if (activities.ActivityEndTime < DateTime.Now)
            {
                activities.ActivityStatus = 1;
            }
            else
            {
                activities.ActivityStatus = 0;
            }
            return DbHelp.Execute(sql, new
            {
                activities.ActivityName,
                activities.ActivityTitle,
                @ActivityType = 2,
                @ActivityPutType = 2,
                activities.ActivityImage,
                activities.ActivitySubImage,                
                activities.ActivityStartTime,
                activities.ActivityEndTime,                
                activities.IsValid,   
                activities.ActivityStatus,
                activities.UpdaterName,
                @UpdaterTime = DateTime.Now,
                activities.ActivityContent,
                @id=activities.ActivityId
            }) > 0;

        }
        /// <summary>
        /// 更改活动状态 即 是否结束活动
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public bool updateActivitiesStatus(int id, string userId, int status)
        {
            string sql = "Update XD_Activity set ActivityStatus = @ActivityStatus,UpdaterName=@UpdaterName,UpdateTime=@UpdateTime where ActivityId=@Id and IsDelete=0";
            return DbHelp.Execute(sql, new { @Id = id, @ActivityStatus = status, @ApprovedBy = userId, @UpdateTime = DateTime.Now }) > 0;

        }
        /// <summary>
        /// 删除活动
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool DeleteActivities(int id, string name)
        {
            string sql = "Update XD_Activity set IsDelete=1,IsValid=0,ActivityStatus=0,UpdaterName=@UpdaterName,UpdaterTime=@UpdaterTime where ActivityId=@Id ";
            return DbHelp.Execute(sql, new { @Id = id, @UpdaterName = name, @UpdaterTime = DateTime.Now }) > 0;

        }

        #endregion



    }
}
