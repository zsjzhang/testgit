using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.IRepository;

namespace Vcyber.BLMS.Domain
{
    public class NoticeInfosApp : INoticeInfosApp
    {
        public int AddNoticeInfo(Entity.NoticeInfos.NoticeInfos notice)
        {
            return _DbSession.NoticeInfoStorager.AddNoticeInfo(notice);
        }

        public int UpdateNoticeInfo(Entity.NoticeInfos.NoticeInfos notice)
        {
            return _DbSession.NoticeInfoStorager.UpdateNoticeInfo(notice);
        }

        public int DelNoticeInfoByID(int id)
        {
            return _DbSession.NoticeInfoStorager.DelNoticeInfoByID(id);
        }

        public Entity.NoticeInfos.NoticeInfos GetNoticeInfoById(int id)
        {
            return _DbSession.NoticeInfoStorager.GetNoticeInfoById(id);
        }

        public IEnumerable<Entity.NoticeInfos.NoticeInfos> GetNoticeInfos(object queryObj, int pageIndex, int pageSize, out int totalCount)
        {
            return _DbSession.NoticeInfoStorager.GetNoticeInfos(queryObj, pageIndex, pageSize, out totalCount);
        }

        public IEnumerable<Entity.NoticeInfos.NoticeInfos> GetNoticeInfos(object queryObj)
        {
            return _DbSession.NoticeInfoStorager.GetNoticeInfos(queryObj);
        }

        public Entity.NoticeInfos.NoticeInfos GetNewNoticeInfo()
        {
            return _DbSession.NoticeInfoStorager.GetNewNoticeInfo();
        }
    }
}
