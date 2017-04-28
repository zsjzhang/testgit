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
    public class ShareRecordStorager : IShareRecordStorager
    {
        public IEnumerable<ShareRecord> GetShareRecordsByActivity(int activityId)
        {
            StringBuilder sql = new StringBuilder();
            if (activityId == 0)
            {
                sql.Append("select * from ShareRecord ");
            }
            else
            {
                sql.Append("select * from ShareRecord where ActivityId=" + activityId);
            }
            return DbHelp.Query<ShareRecord>(sql.ToString());
        }

        public IEnumerable<ShareRecord> GetShareRecordsByActivity(int activityId, PageData pageData, out int total)
        {
            StringBuilder sql = new StringBuilder();
            if (activityId > 0)
            {
                sql.AppendFormat(" select count(1) from ShareRecord where ActivityId=@activityId");
                total = DbHelp.ExecuteScalar<int>(sql.ToString(), new { @activityId = activityId });
                sql.Clear();

                sql.AppendFormat(" select top {0} * from ShareRecord where ActivityId=@ActivityId", pageData.Size);
                sql.Append(" and ShareRecord.Id not in( ");
                sql.AppendFormat(" select top {0} ShareRecord.Id from ShareRecord where ActivityId=@ActivityId order by ShareRecord.Id asc", pageData.Index);
                sql.Append(" )order by ShareRecord.Id asc ");
                return DbHelp.Query<ShareRecord>(sql.ToString(), new { ActivityId = activityId });
            }
            else
            {
                sql.Append(" select count(1) from ShareRecord ");
                total = DbHelp.ExecuteScalar<int>(sql.ToString());
                sql.Clear();

                sql.AppendFormat(" select top {0} * from ShareRecord", pageData.Size);
                sql.Append(" where ShareRecord.Id not in( ");
                sql.AppendFormat(" select top {0} ShareRecord.Id from ShareRecord order by ShareRecord.Id asc", pageData.Index);
                sql.Append(" )order by ShareRecord.Id asc ");
                return DbHelp.Query<ShareRecord>(sql.ToString());
            }
        }

        public int AddShareRecord(ShareRecord entity)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("Insert into ShareRecord(Source,ShareType,UserId,ActivityId) values(@Source,@ShareType,@UserId,@ActivityId);SELECT @@identity");
            return DbHelp.ExecuteScalar<int>(sql.ToString(), entity);
        }

        public IEnumerable<ShareRecord> GetShareRecordsAll()
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select * from ShareRecord ");
            return DbHelp.Query<ShareRecord>(sql.ToString());
        }

        public IEnumerable<ShareRecord> GetShareRecordsAll(PageData pageData, out int total)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(" select count(1) from ShareRecord ");
            total = DbHelp.ExecuteScalar<int>(sql.ToString());
            sql.Clear();

            sql.AppendFormat(" select top {0} * from ShareRecord", pageData.Size);
            sql.Append(" where ShareRecord.Id not in( ");
            sql.AppendFormat(" select top {0} ShareRecord.Id from ShareRecord order by ShareRecord.Id asc", pageData.Index);
            sql.Append(" )order by ShareRecord.Id asc ");
            return DbHelp.Query<ShareRecord>(sql.ToString());
        }
    }
}
