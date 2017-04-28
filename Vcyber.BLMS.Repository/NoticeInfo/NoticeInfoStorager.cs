using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.Entity.NoticeInfos;
using Vcyber.BLMS.IRepository;

namespace Vcyber.BLMS.Repository
{
    public class NoticeInfoStorager : INoticeInfoStorager
    {
        public int AddNoticeInfo(BLMS.Entity.NoticeInfos.NoticeInfos notice)
        {
            string sql = "Insert into NoticeInfo(Title,Summary,CreateTime,IsDisplay) values(@Title,@Summary,@CreateTime,@IsDisplay);SELECT @@identity";
            return DbHelp.Execute(sql, new { @Title = notice.Title, @Summary = notice.Summary, @CreateTime = DateTime.Now, @IsDisplay = notice.IsDisplay });
        }

        public int UpdateNoticeInfo(BLMS.Entity.NoticeInfos.NoticeInfos notice)
        {
            string sql = "Update NoticeInfo set Title=@Title,Summary=@Summary,UpdateTime=@UpdateTime,IsDisplay=@IsDisplay where Id=@Id";
            return DbHelp.Execute(sql, new { @Id = notice.Id, @Title = notice.Title, @Summary = notice.Summary, @UpdateTime = DateTime.Now, @IsDisplay = notice.IsDisplay });
        }

        public int DelNoticeInfoByID(int id)
        {
            //string sql = "delete from NoticeInfo where Id=@Id";
            string sql = "Update NoticeInfo set IsDeleted=1 where Id=@Id";
            return DbHelp.Execute(sql, new { @Id = id });
        }

        public BLMS.Entity.NoticeInfos.NoticeInfos GetNoticeInfoById(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BLMS.Entity.NoticeInfos.NoticeInfos> GetNoticeInfos(object queryObj, int pageIndex, int pageSize, out int totalCount)
        {
            string sql = " SELECT top {0} * FROM NoticeInfo WHERE isDisplay=1 and isDeleted=0 and id not in (select top {1} id from NoticeInfo WHERE isDisplay=1 and isDeleted=0 ORDER BY CreateTime desc) ORDER BY CreateTime desc ";
            sql = string.Format(sql, pageSize, pageIndex);
            var totalsql = string.Format("Select count(1) from NoticeInfo WHERE isDisplay=1 and isDeleted=0 ");
            totalCount = DbHelp.ExecuteScalar<int>(totalsql);
            return DbHelp.Query<NoticeInfos>(sql);
        }

        public IEnumerable<BLMS.Entity.NoticeInfos.NoticeInfos> GetNoticeInfos(object queryObj)
        {
            throw new NotImplementedException();
        }

        public BLMS.Entity.NoticeInfos.NoticeInfos GetNewNoticeInfo()
        {
            string sql = " select top 1 * from NoticeInfo where isDisplay=1 and isDeleted=0 order by createTime desc ";
            return DbHelp.QueryOne<NoticeInfos>(sql);
        }
    }
}
