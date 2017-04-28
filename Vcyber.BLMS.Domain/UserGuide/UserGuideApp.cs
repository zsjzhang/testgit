using System;
using System.Collections.Generic;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.IRepository;

namespace Vcyber.BLMS.Domain
{
    public class UserGuideApp:IUserGuideApp
    {
        public UserGuide GetUserGuideById(int id)
        {
            return _DbSession.UserGuideStorager.GetUserGuideById(id);
        }

        public IEnumerable<UserGuide> GetUserGuideList(string title, int? approveStatus, int start, int count,
            out int total)
        {
            return _DbSession.UserGuideStorager.GetUserGuideList(title,approveStatus,start,count,out total);
        }

        public IEnumerable<UserGuide> GetUserGuideList(int? approveStatus, int start, int count,
          out int total)
        {
            return _DbSession.UserGuideStorager.GetUserGuideList(approveStatus, start, count, out total);
        }
       
        public int Create(UserGuide entity)
        {
            if (entity==null)
            {
                return -1;
            }
            return _DbSession.UserGuideStorager.Create(entity);
        }

        public bool Update(UserGuide entity)
        {
            if (entity == null)
            {
                return false;
            }
            return _DbSession.UserGuideStorager.Update(entity);
        }

        public bool Delete(int Id,string name)
        {
            return _DbSession.UserGuideStorager.Delete(Id,name);
        }

        public bool UpdateDownloadTimes(int Id, int times,string name)
        {
            return _DbSession.UserGuideStorager.UpdateDownloadTimes(Id, times,name);
        }

        public IEnumerable<UserGuide> GetUserGuide(int status, int pageIndex, int pageSize, out int totalCount)
        {
            return _DbSession.UserGuideStorager.GetUserGuide(status, pageIndex, pageSize, out totalCount);
        }

        public bool UpdateIsDisplay(int Id, int status,string name)
        {
            return _DbSession.UserGuideStorager.UpdateIsDisplay(Id, status,name);
        }
        public bool ApprovedUserGuide(int id, string userId, string userName, int status, string memo)
        {
            var approve = new ApproveRecord();
            approve.ItemId = id;
            approve.ItemType = ((int)EApproveType.UserGuide).ToString();
            approve.IsApproval = status;
            approve.ApprovalMemo = memo;
            approve.OperatorId = userId;
            approve.OperatorName = userName;
            _AppContext.ApproveRecordApp.UpdateStatus(approve);
            return _DbSession.UserGuideStorager.ApprovedUserGuide(id, userId, status);

        }
    }
}
