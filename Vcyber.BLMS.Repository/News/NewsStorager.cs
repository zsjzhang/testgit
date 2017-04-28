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
    public class NewsStorager : INewsStorager
    {
        public IEnumerable<News> Select(string title, int pageIndex, int pageSize, out int totalCount)
        {
            string sql = "SELECT top {0} * FROM News WHERE Title like '%{2}%' and IsDeleted=0 and id not in (select top {1} id from news WHERE Title like '%{2}%' and IsDeleted=0 ORDER BY CreateTime desc,Priority desc) ORDER BY CreateTime desc,Priority desc";
            sql = string.Format(sql, pageSize, pageIndex, title);
            var totalsql = string.Format("Select count(*) from News WHERE Title like '%{0}%' and IsDeleted=0", title);
            totalCount = DbHelp.ExecuteScalar<int>(totalsql);
            return DbHelp.Query<News>(sql);
        }

        public IEnumerable<News> Select(string title, int? isDisplay, int? approveStatus, string dealer, int pageIndex, int pageSize, out int totalCount)
        {
            StringBuilder conditionStr = new StringBuilder();
            if (approveStatus != null)
            {
                conditionStr.AppendFormat(" and IsApproved={0}", approveStatus.Value);
            }
            if (isDisplay != null)
            {
                conditionStr.AppendFormat(" and IsDisplay={0}", isDisplay.Value);
            }
            if (!string.IsNullOrEmpty(dealer))
            {
                conditionStr.AppendFormat(" and Dealer='{0}'", dealer);
            }

            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select count(1) from News where IsDeleted=0 and Title like '%{0}%' ", title);
            sql.Append(conditionStr);
            totalCount = DbHelp.ExecuteScalar<int>(sql.ToString());
            sql.Clear();

            sql.AppendFormat(" select top {0} * from News where IsDeleted=0 and Title like '%{1}%' ", pageSize, title);
            sql.Append(conditionStr);
            sql.Append(" and News.ID not in( ");
            sql.AppendFormat(" select top {0} News.ID from News where IsDeleted=0 and Title like '%{1}%' ", pageIndex, title);
            sql.Append(conditionStr);
            sql.Append(" order by CreateTime desc,Priority desc)order by CreateTime desc,Priority desc ");
            return DbHelp.Query<News>(sql.ToString());
        }

        public IEnumerable<News> Select1(string title, int? isDisplay, int? approveStatus, string dealer, int pageIndex, int pageSize, out int totalCount, string start, string end)
        {
            StringBuilder conditionStr = new StringBuilder();

            if (approveStatus != null)
            {

                conditionStr.AppendFormat(" and IsApproved=@IsApproved");
            }
            if (isDisplay != null)
            {
                conditionStr.AppendFormat(" and IsDisplay=@isDisplay");
            }
            if (dealer != null)
            {
                conditionStr.AppendFormat(" and Dealer=@Dealer");
            }

            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select count(1) from News where IsDeleted=0");
            if (!string.IsNullOrWhiteSpace(title))
            {
                sql.AppendFormat(" and  Title like @Title");
            }
            if (!string.IsNullOrWhiteSpace(start) || !string.IsNullOrWhiteSpace(end))
            {
                sql.AppendFormat(" and IsApproved=1 ");
            }
            if (!string.IsNullOrWhiteSpace(start))
            {
                sql.AppendFormat(" and ApprovedTime>=@ApprovedTime");
            }
            if (!string.IsNullOrWhiteSpace(end))
            {
                sql.AppendFormat(" and ApprovedTime<'{0}'", Convert.ToDateTime(end).AddDays(1).ToString("yyyy-MM-dd"));
            }
            sql.Append(conditionStr);
            totalCount = DbHelp.ExecuteScalar<int>(sql.ToString(), new
            {
                IsApproved = approveStatus.HasValue ? approveStatus.Value : 0,
                isDisplay = isDisplay.HasValue ? isDisplay.Value : 1,
                Dealer = dealer,
                Title = string.Format("%{0}%", title),
                ApprovedTime = start

            });
            sql.Clear();

            sql.AppendFormat(" select top {0} * from News where IsDeleted=0 ", pageSize);
            if (!string.IsNullOrWhiteSpace(title))
            {
                sql.AppendFormat(" and  Title like @Title");
            }
            if (!string.IsNullOrWhiteSpace(start) || !string.IsNullOrWhiteSpace(end))
            {
                sql.AppendFormat(" and IsApproved=1 ");
            }
            if (!string.IsNullOrWhiteSpace(start))
            {
                sql.AppendFormat(" and ApprovedTime>=@ApprovedTime");
            }
            if (!string.IsNullOrWhiteSpace(end))
            {
                sql.AppendFormat(" and ApprovedTime<'{0}'", Convert.ToDateTime(end).AddDays(1));
            }
            sql.Append(conditionStr);
            sql.Append(" and News.ID not in( ");
            sql.AppendFormat(" select top {0} News.ID from News where IsDeleted=0 and IsApproved=1 ", pageIndex);
            if (!string.IsNullOrWhiteSpace(title))
            {
                sql.AppendFormat(" and  Title like @Title");
            }
            if (!string.IsNullOrWhiteSpace(start))
            {
                sql.AppendFormat(" and ApprovedTime>= @ApprovedTime");
            }
            if (!string.IsNullOrWhiteSpace(end))
            {
                sql.AppendFormat(" and ApprovedTime<{0}", Convert.ToDateTime(end).AddDays(1).ToString("yyyy-MM-dd"));
            }
            sql.Append(conditionStr);
            sql.Append(" order by CreateTime desc,Priority desc)order by CreateTime desc,Priority desc ");
            return DbHelp.Query<News>(sql.ToString(), new
            {
                Title = string.Format("%{0}%", title),
                ApprovedTime = start

            });
        }

        /// <summary>
        /// 获取在首页显示的热点新闻（已经审批通过）
        /// </summary>
        /// <param name="title"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public IEnumerable<News> SelectHotNews(string title, int pageIndex, int pageSize, out int totalCount)
        {
            string sql = "SELECT top {0} * FROM News WHERE Title like '%{2}%' and IsDeleted=0 and IsHot=1 and IsDisplay=1 and IsApproved=1 and id not in (select top {1} id from news WHERE Title like '%{2}%' and IsDeleted=0 and IsHot=1 and IsDisplay=1 and IsApproved=1 ORDER BY CreateTime desc,Priority desc) ORDER BY CreateTime desc,Priority desc";
            sql = string.Format(sql, pageSize, pageIndex, title);
            var totalsql = string.Format("Select count(*) from News WHERE Title like '%{0}%'and IsDeleted=0 and IsHot=1 and IsDisplay=1 and IsApproved=1", title);
            totalCount = DbHelp.ExecuteScalar<int>(totalsql);
            return DbHelp.Query<News>(sql);
        }

        public News GetNewsById(int id)
        {
            string sql = "Select * From News WHERE Id=@Id";
            return DbHelp.QueryOne<News>(sql, new { @Id = id });
        }

        public int AddNews(News news)
        {
            string sql = "Insert into News(Title,MajorImageUrl,Content,IsDisplay,Priority,IsHot,CreateTime,CreateBy,UpdateTime,UpdateBy,IsApproved,IsDeleted,Summary,Dealer) values(@Title,@MajorImageUrl,@Content,@IsDisplay,@Priority,@IsHot,@CreateTime,@CreateBy,@UpdateTime,@UpdateBy,@IsApproved,@IsDeleted,@Summary,@Dealer);SELECT @@identity";
            return DbHelp.ExecuteScalar<int>(sql, new { @Title = news.Title, @MajorImageUrl = news.MajorImageUrl, @Content = news.Content, @IsDisplay = news.IsDisplay, @Priority = news.Priority, @IsHot = news.IsHot, @CreateTime = DateTime.Now, @CreateBy = news.CreateBy, @UpdateTime = DateTime.Now, news.UpdateBy, @IsDeleted = 0, @IsApproved = (int)EApproveStatus.NoBegin, news.Summary, news.Dealer });
        }
        public bool UpdateNews(News news)
        {
            string sql = "Update News set Title =@Title,MajorImageUrl=@MajorImageUrl,Content=@Content,IsDisplay=@IsDisplay,Priority=@Priority,IsHot=@IsHot,UpdateTime=@UpdateTime,UpdateBy=@UpdateBy, IsApproved=@IsApproved,ApprovedBy=@ApprovedBy,ApprovedTime=@ApprovedTime,Summary=@Summary where Id=@Id and IsDeleted=0";
            var approvestatus = news.IsApproved;
            var apporveBy = news.ApprovedBy;
            var approveTime = news.ApprovedTime;

            if (approvestatus != (int)EApproveStatus.NoBegin)
            {
                approvestatus = (int)EApproveStatus.NoBegin;
                apporveBy = news.UpdateBy;
                approveTime = DateTime.Now;
            }
            if (approveTime <= DateTime.MinValue)
            {
                approveTime = (DateTime)SqlDateTime.MinValue;
            }

            return DbHelp.Execute(sql, new { news.Title, news.MajorImageUrl, news.Content, news.IsDisplay, news.Priority, news.IsHot, @UpdateTime = DateTime.Now, @IsApproved = approvestatus, @ApprovedBy = apporveBy, @ApprovedTime = approveTime, news.UpdateBy, news.Summary, news.Id }) > 0;

        }

        public bool DeleteNews(int id, string name)
        {
            string sql = "Update News set IsDeleted = 1,IsDisplay=0,UpdateBy=@UpdateBy,UpdateTime=@UpdateTime where Id=@Id and IsDeleted=0";
            return DbHelp.Execute(sql, new { @Id = id, @UpdateBy = name, @UpdateTime = DateTime.Now }) > 0;

        }

        public bool ApprovedNews(int id, string userId, int status)
        {
            string sql = "Update News set IsApproved = @IsApproved,ApprovedBy=@ApprovedBy,ApprovedTime=@ApprovedTime,UpdateTime=@UpdateTime where Id=@Id and IsDeleted=0";
            return DbHelp.Execute(sql, new { @Id = id, @IsApproved = status, @ApprovedBy = userId, @ApprovedTime = DateTime.Now, @UpdateTime = DateTime.Now }) > 0;
        }

        public bool UpdateIsDisplay(int id, int status, string operatorName)
        {
            string sql = "Update News set IsDisplay = @IsDisplay,UpdateTime=@UpdateTime,UpdateBy=@UpdateBy where Id=@Id and IsDeleted=0";
            return DbHelp.Execute(sql, new { @Id = id, @IsDisplay = status, @UpdateBy = operatorName, @UpdateTime = DateTime.Now }) > 0;

        }

        public bool UpdateAllDisplay(int id, int priority, int dispaly, int isHot, string operatorName)
        {
            string sql = "Update News set Priority = @Priority,IsDisplay=@IsDisplay,IsHot = @IsHot ,UpdateTime=@UpdateTime,UpdateBy=@UpdateBy where Id=@Id and IsDeleted=0";
            return DbHelp.Execute(sql, new { @Id = id, @Priority = priority, @IsDisplay = dispaly, @IsHot = isHot, @UpdateBy = operatorName, @UpdateTime = DateTime.Now }) > 0;

        }

        public IEnumerable<News> GetNews(int status, int pageIndex, int pageSize, out int totalCount)
        {
            var whereStr = "";
            if (status >= 0)
            {
                whereStr = "IsApproved=" + status + " and";
            }
            string sql = "SELECT top {0} * FROM News WHERE " + whereStr +
                         " IsDeleted=0 and id not in (select top {1} id from news WHERE " + whereStr + " IsDeleted=0 ORDER BY CreateTime desc,Priority desc) ORDER BY CreateTime desc,Priority desc";
            sql = string.Format(sql, pageSize, pageIndex, status);
            var totalsql = string.Format("Select count(*) from News WHERE {0} IsDeleted=0", whereStr);
            totalCount = DbHelp.ExecuteScalar<int>(totalsql);
            return DbHelp.Query<News>(sql);
        }

    }
}
