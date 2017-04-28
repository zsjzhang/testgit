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
    public class UserGuideStorager : IUserGuideStorager
    {
        /// <summary>
        /// 根据Id，获取一个手册实体
        /// </summary>
        /// <param name="id">手册Id</param>
        /// <returns></returns>
        public UserGuide GetUserGuideById(int id)
        {
            string sql = "SELECT * FROM UserGuide where Id = @Id and IsDeleted=@IsDeleted";
            return DbHelp.QueryOne<UserGuide>(sql, new { @Id = id, @IsDeleted = 0 });

        }

        /// <summary>
        /// 获取用户手册列表
        /// </summary>
        /// <param name="title"></param>
        /// <param name="approveStatus"></param>
        /// <param name="start"></param>
        /// <param name="count"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public IEnumerable<UserGuide> GetUserGuideList(string title, int? approveStatus, int start, int count, out int total)
        {
            var approveStr = "";
            if (approveStatus != null)
            {
                approveStr = string.Format("and IsApproved={0}", approveStatus.Value);
            }
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat(" select count(1) from UserGuide where IsDeleted=0 and Title like '%{0}%' ",title);
            sql.Append(approveStr);
            total = DbHelp.ExecuteScalar<int>(sql.ToString());
            sql.Clear();

            sql.AppendFormat(" select top {0} * from UserGuide where IsDeleted=0 and Title like '%{1}%' ", count, title);
            sql.Append(approveStr);
            sql.Append(" and UserGuide.ID not in( ");
            sql.AppendFormat(" select top {0} UserGuide.ID from UserGuide where IsDeleted=0 and Title like '%{1}%' ", start, title);
            sql.Append(approveStr);
            sql.Append(" order by UserGuide.CreateTime desc)order by UserGuide.CreateTime desc ");
            return DbHelp.Query<UserGuide>(sql.ToString());
        }

        public IEnumerable<UserGuide> GetUserGuideList(int? approveStatus, int start, int count, out int total)
        {
            var approveStr = "";
            if (approveStatus != null)
            {
                approveStr = string.Format("and IsApproved={0}", approveStatus.Value);
            }
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat(" select count(1) from UserGuide where IsDeleted=0 and IsDisplay=1 ");
            sql.Append(approveStr);
            total = DbHelp.ExecuteScalar<int>(sql.ToString());
            sql.Clear();

            sql.AppendFormat(" select top {0} * from UserGuide where IsDeleted=0 and IsDisplay=1 ", count);
            sql.Append(approveStr);
            sql.Append(" and UserGuide.ID not in( ");
            sql.AppendFormat(" select top {0} UserGuide.ID from UserGuide where IsDeleted=0 and IsDisplay=1 ", start);
            sql.Append(approveStr);
            sql.Append(" order by UserGuide.CreateTime desc)order by UserGuide.CreateTime desc ");
            return DbHelp.Query<UserGuide>(sql.ToString());
        }

        public int Create(UserGuide entity)
        {
            string sql = "Insert into UserGuide(Title,ImageUrl,LinkUrl,IsDisplay,Summary,CreateTime,CreateBy,UpdateTime,UpdateBy,IsDeleted,IsApproved,DownloadTimes) values" +
                         "(@Title,@ImageUrl,@LinkUrl,@IsDisplay,@Summary,@CreateTime,@CreateBy,@UpdateTime,@UpdateBy,@IsDeleted,@IsApproved,@DownloadTimes);SELECT @@identity";
            return DbHelp.ExecuteScalar<int>(sql, new
            {
                entity.Title,
                entity.ImageUrl,
                entity.LinkUrl,
                entity.IsDisplay,
                entity.Summary,
                @CreateTime = DateTime.Now,
                entity.CreateBy,
                @UpdateTime = DateTime.Now,
                entity.UpdateBy,
                @IsDeleted = 0,
                @IsApproved = (int)EApproveStatus.NoBegin,
                @DownloadTimes=0
            });

        }

        /// <summary>
        /// 更新一个手册
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Update(UserGuide entity)
        {
            string sql = "UPDATE UserGuide SET Title=@Title,ImageUrl=@ImageUrl,LinkUrl=@LinkUrl,Summary=@Summary,UpdateTime=@UpdateTime,UpdateBy=@UpdateBy," +
                             "IsDeleted=@IsDeleted,IsApproved=@IsApproved,ApprovedBy=@ApprovedBy,ApprovedTime=@ApprovedTime " +
                             "WHERE Id=@Id";
            var approvestatus = entity.IsApproved;
            var apporveBy = entity.ApprovedBy;
            var approveTime = entity.ApprovedTime;
            if (approvestatus != (int)EApproveStatus.NoBegin)
            {
                approvestatus = (int)EApproveStatus.NoBegin;
                apporveBy = entity.UpdateBy;
                approveTime = DateTime.Now;
            } 
            if (approveTime <= DateTime.MinValue)
            {
                approveTime = (DateTime)SqlDateTime.MinValue;
            }
            return DbHelp.Execute(sql, new
            {
                entity.Title,
                entity.ImageUrl,
                entity.LinkUrl,
                entity.Summary,
                entity.Id,
                entity.UpdateTime,
                entity.UpdateBy,
                entity.IsDeleted,
                @IsApproved = approvestatus,
                @ApprovedBy = apporveBy,
                @ApprovedTime = approveTime
            }) > 0;

        }

        /// <summary>
        ///删除一个手册
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool Delete(int Id,string name)
        {
            string sql = "Update UserGuide set IsDeleted=1,UpdateBy=@UpdateBy,UpdateTime=@UpdateTime WHERE Id=@Id AND IsDeleted=0";
            return DbHelp.Execute(sql, new { Id,@UpdateBy=name,@UpdateTime=DateTime.Now  }) > 0;

        }

        /// <summary>
        ///更新下载次数
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="times"></param>
        /// /// <returns></returns>
        public bool UpdateDownloadTimes(int Id, int times,string name)
        {
            string sql = "Update UserGuide set DownloadTimes=DownloadTimes+@DownloadTimes,UpdateBy=@UpdateBy,UpdateTime=@UpdateTime WHERE Id=@Id AND IsDeleted=0";
            return DbHelp.Execute(sql, new { Id, @DownloadTimes=times,@UpdateBy=name,@UpdateTime=DateTime.Now }) > 0;

        }

        /// <summary>
        ///更新下载次数
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="status"></param>
        /// /// <returns></returns>
        public bool UpdateIsDisplay(int Id, int status,string name)
        {
            string sql = "Update UserGuide set IsDisplay=@IsDisplay,UpdateBy=@UpdateBy,UpdateTime=@UpdateTime WHERE Id=@Id AND IsDeleted=0";
            return DbHelp.Execute(sql, new { Id, @IsDisplay = status, @UpdateBy = name, @UpdateTime = DateTime.Now }) > 0;

        }

        /// <summary>
        /// 获取电子手册审批列表
        /// </summary>
        /// <param name="status"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public IEnumerable<UserGuide> GetUserGuide(int status, int pageIndex, int pageSize, out int totalCount)
        {
            var whereStr = "";
            if (status >= 0)
            {
                whereStr = "IsApproved=" + status + " and";
            }
            string sql = "SELECT top {0} * FROM UserGuide WHERE " + whereStr +
                         " IsDeleted=0 and id not in (select top {1} id from UserGuide WHERE " + whereStr + " IsDeleted=0 ORDER BY CreateTime desc) ORDER BY CreateTime desc";
            sql = string.Format(sql, pageSize, pageIndex, status);
            var totalsql = string.Format("Select count(*) from UserGuide WHERE {0} IsDeleted=0", whereStr);
            totalCount = DbHelp.ExecuteScalar<int>(totalsql);
            return DbHelp.Query<UserGuide>(sql);
        }
        public bool ApprovedUserGuide(int id, string userId, int status)
        {
            string sql = "Update UserGuide set IsApproved = @IsApproved,ApprovedBy=@ApprovedBy,ApprovedTime=@ApprovedTime,UpdateTime=@UpdateTime where Id=@Id and IsDeleted=0";
            return DbHelp.Execute(sql, new { @Id = id, @IsApproved = status, @ApprovedBy = userId, @ApprovedTime = DateTime.Now, @UpdateTime = DateTime.Now }) > 0;
        }
    }
}
