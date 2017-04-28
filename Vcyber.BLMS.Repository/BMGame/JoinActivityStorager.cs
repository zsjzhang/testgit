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
    public class JoinActivityStorager : IJoinActivityStorager
    {
        /// <summary>
        /// 获取活动的参加人员列表
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns></returns>
        public IEnumerable<JoinActivity> GetJoinActivitiesByAId(int activityId)
        {
            StringBuilder sql = new StringBuilder();
            if (activityId > 0)
            {
                sql.AppendFormat("select * from JoinActivity where ActivityId={0}", activityId);
            }
            else
            {
                sql.Append("select * from JoinActivity ");
            }
            return DbHelp.Query<JoinActivity>(sql.ToString());
        }
        /// <summary>
        /// 分页获取参加人员列表
        /// </summary>
        /// <param name="activityId"></param>
        /// <param name="pageData"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public IEnumerable<JoinActivity> GetJoinActivitiesAll(int activityId, PageData pageData, out int total)
        {
            StringBuilder sql = new StringBuilder();
            if (activityId > 0)
            {
                sql.AppendFormat(" select count(1) from JoinActivity where ActivityId=@activityId");
                total = DbHelp.ExecuteScalar<int>(sql.ToString(), new { @activityId = activityId });
                sql.Clear();

                sql.AppendFormat(" select top {0} * from JoinActivity where ActivityId=@ActivityId", pageData.Size);
                sql.Append(" and JoinActivity.Id not in( ");
                sql.AppendFormat(" select top {0} JoinActivity.Id from JoinActivity where ActivityId=@ActivityId order by JoinActivity.Id asc", pageData.Index);
                sql.Append(" )order by JoinActivity.Id asc ");
                return DbHelp.Query<JoinActivity>(sql.ToString(), new { @ActivityId = activityId });
            }
            else
            {
                sql.Append(" select count(1) from JoinActivity ");
                total = DbHelp.ExecuteScalar<int>(sql.ToString());
                sql.Clear();

                sql.AppendFormat(" select top {0} * from JoinActivity", pageData.Size);
                sql.Append(" where JoinActivity.Id not in( ");
                sql.AppendFormat(" select top {0} JoinActivity.Id from JoinActivity order by JoinActivity.Id asc", pageData.Index);
                sql.Append(" )order by JoinActivity.Id asc ");
                return DbHelp.Query<JoinActivity>(sql.ToString());
            }
        }
        /// <summary>
        /// 获取人员的参与记录
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IEnumerable<JoinActivity> GetJoinActivitiesByUId(string userId)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from JoinActivity where UserId='{0}'", userId);
            return DbHelp.Query<JoinActivity>(sql.ToString());
        }
        /// <summary>
        /// 判断人员是否参加活动
        /// </summary>
        /// <param name="activityId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool IsUserJoinActivity(int activityId, string userId)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select count(*) from JoinActivity where ActivityId={0} and UserId='{1}'", activityId, userId);
            int count = DbHelp.ExecuteScalar<int>(sql.ToString());
            if (count > 0) return true;
            else return false;
        }
        /// <summary>
        /// 判断当前微信端当日是否参加过活动（针对机场服务升级活动）
        /// </summary>
        /// <param name="activityId"></param>
        /// <param name="openId">微信端OpenId</param>
        /// <returns></returns>
        public bool IsUserJoinActivityByDay(int activityId, string openId)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select count(*) from JoinActivity where ActivityId={0} and Results1='{1}' and datediff(DAY,CreateDate,getdate())=0", activityId, openId);
            int count = DbHelp.ExecuteScalar<int>(sql.ToString());
            if (count > 0) return true;
            else return false;
        }

        /// <summary>
        /// 添加人员参与记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int AddJoinActivity(JoinActivity entity)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("Insert into JoinActivity(ActivityId,UserId,Name,Tel,Province,City,Area,Address,Email,DeviceType,Results1,Results2,Results3) ");
            sql.Append("values(@ActivityId,@UserId,@Name,@Tel,@Province,@City,@Area,@Address,@Email,@DeviceType,@Results1,@Results2,@Results3);SELECT @@identity");
            return DbHelp.ExecuteScalar<int>(sql.ToString(), entity);
        }


        /// <summary>
        /// 修改人员参与记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int UpdateJoinActivity(JoinActivity entity)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("Update JoinActivity set ActivityId=@ActivityId,UserId=@UserId,Name=@Name,Tel=@Tel,Province=@Province,City=@City,Area=@Area,Address=@Address,Email=@Email,DeviceType=@DeviceType,Results1=@Results1,Results2=@Results2,Results3=@Results3 ");
            sql.Append("where Id=@Id");
            return DbHelp.Execute(sql.ToString(), entity);
        }
        /// <summary>
        /// 根据Id获取报名信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JoinActivity GetJoinActivityById(int id)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select * from JoinActivity where id={0}", id);
            return DbHelp.QueryOne<JoinActivity>(sql.ToString());
        }
        /// <summary>
        /// 根据活动和用户查询参加活动的数量
        /// </summary>
        public int GetJoinActivityForCount(int activityId,string userId)
        {
            Sql sql = Sql.Builder.Append("SELECT COUNT(*) FROM dbo.JoinActivity AS ja WHERE ActivityId = @0 AND UserId = @1", activityId, userId);
            return PocoHelper.CurrentDb().ExecuteScalar<int>(sql);
        }
    }
}
