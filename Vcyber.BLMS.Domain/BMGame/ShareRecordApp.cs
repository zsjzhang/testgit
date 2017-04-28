using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.IRepository;

namespace Vcyber.BLMS.Domain
{
    public class ShareRecordApp : IShareRecordApp
    {
        public List<ShareRecord> GetShareRecordsByActivity(int activityId)
        {
            return _DbSession.ShareRecordStorager.GetShareRecordsByActivity(activityId).ToList();
        }

        public List<ShareRecord> GetShareRecordsByActivity(int activityId, PageData page, out int totalNum)
        {
            return _DbSession.ShareRecordStorager.GetShareRecordsByActivity(activityId, page, out totalNum).ToList();
        }

        public bool AddShareRecord(ShareRecord entity)
        {
            return _DbSession.ShareRecordStorager.AddShareRecord(entity) > 0;
        }

        public List<ShareRecord> GetShareRecordsAll()
        {
            return _DbSession.ShareRecordStorager.GetShareRecordsAll().ToList();
        }

        public List<ShareRecord> GetShareRecordsAll(PageData page, out int totalNum)
        {
            return _DbSession.ShareRecordStorager.GetShareRecordsAll(page, out totalNum).ToList();
        }
    }
}
