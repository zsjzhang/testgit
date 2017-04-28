using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.IRepository;

namespace Vcyber.BLMS.Repository
{
    public class ActivityInfoStorager : IActivityInfoStorager
    {
        /// <summary>
        /// 查询单条活动记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActivityInfo GetActivityInfoByID(int id)
        {
            StringBuilder sql = new StringBuilder();            
            if (id > 0)
            {
                sql.AppendFormat("SELECT * FROM dbo.ActivityInfo AS ai WHERE ai.Id = {0}", id);
            }
            else 
            {
                sql.AppendFormat("SELECT TOP 1 * FROM dbo.ActivityInfo AS ai ORDER BY ai.StartDate DESC", id);
            }
            return DbHelp.QueryOne<ActivityInfo>(sql.ToString());
        }
        /// <summary>
        /// 查询单条活动记录
        /// </summary>
        /// <param name="name">活动名称</param>
        /// <returns></returns>
        public ActivityInfo GetActivityInfoByName(string name)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("SELECT ai.CreateDate,ai.EndDate,ai.Id,ai.Intro,ai.Name,ai.StartDate,ai.UpdateDate FROM dbo.ActivityInfo AS ai WHERE ai.Name = '{0}'", name);
            return DbHelp.QueryOne<ActivityInfo>(sql.ToString());
        }
        /// <summary>
        /// 查询所有活动记录
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ActivityInfo> GetActivityInfoAll()
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select * from ActivityInfo ");
            return DbHelp.Query<ActivityInfo>(sql.ToString());
        }
        /// <summary>
        /// 添加活动记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int AddActivityInfo(ActivityInfo entity)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("insert into ActivityInfo(Name,Intro,EndDate,StartDate,UpdateDate) ");
            sql.Append("values(@Name ,@Intro ,@EndDate ,@StartDate ,@UpdateDate);SELECT @@identity");
            int id = DbHelp.ExecuteScalar<int>(sql.ToString(), entity);
            return id;
        }


        public int UpdateActivityInfo(ActivityInfo entity)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("Update ActivityInfo Set");
            sql.Append(" Name=@Name,Intro=@Intro,EndDate=@EndDate,StartDate=@StartDate,UpdateDate=@UpdateDate ");
            sql.Append("where Id=@Id");
            int id = DbHelp.Execute(sql.ToString(), entity);
            return id;
        }

        public IEnumerable<int> GetDistinctActivityId()
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select distinct(Id) from ActivityInfo");
            return DbHelp.Query<int>(sql.ToString());

        }
        /// <summary>
        /// 结束活动
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int EndActivityInfo(int id)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("Update ActivityInfo set EndDate=getdate(),UpdateDate=getdate() where Id={0}", id);
            return DbHelp.Execute(sql.ToString());
        }
    }
}
