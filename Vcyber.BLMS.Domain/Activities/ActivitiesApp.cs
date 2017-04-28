using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.IRepository;

namespace Vcyber.BLMS.Domain
{
    public class ActivitiesApp:IActivitiesApp
    {

        public IEnumerable<Activities> Select(string dealer,int? status,int pageIndex, int pageSize, out int totalCount)
        {
            return SelectStatus(dealer,status,(int)EApproveStatus.Approved,1,null,pageIndex,pageSize, out totalCount);
        }
        public IEnumerable<Activities> SelectHotActivitieses(string dealer, int? status, int pageIndex, int pageSize, out int totalCount)
        {
            return SelectStatus(dealer, status, (int)EApproveStatus.Approved, 1, 1, pageIndex, pageSize, out totalCount);
        }
        public IEnumerable<Activities> SelectStatus(string dealer,int? status,int? approveStatus,int? display,int? isHot,int pageIndex, int pageSize, out int totalCount)
        {
            if (status != null)
            {
                if ((status) == (int)EActivitiescsStatus.NoBegin)
                {
                    return _DbSession.ActivitiesStorager.SelectNotStart(approveStatus, display, isHot,dealer, pageIndex, pageSize, out totalCount);
                }
                if ((status) == (int)EActivitiescsStatus.InProcess)
                {
                    return _DbSession.ActivitiesStorager.SelectInProgress(approveStatus, display,isHot, dealer, pageIndex, pageSize, out totalCount);
                }
                if ((status) == (int)EActivitiescsStatus.Finished)
                {
                    return _DbSession.ActivitiesStorager.SelectFinished(approveStatus, display,isHot, dealer, pageIndex, pageSize, out totalCount);
                }
            }
            return _DbSession.ActivitiesStorager.Select(approveStatus, display,isHot, dealer, pageIndex, pageSize, out totalCount);
        
        }

        //public IEnumerable<Activities> SelectInProgress(string dealer,int pageIndex, int pageSize, out int totalCount)
        //{
        //    return _DbSession.ActivitiesStorager.Select(dealer,pageIndex, pageSize, out totalCount);
        //}
        public Activities GetActivitiesById(int id)
        {
            return _DbSession.ActivitiesStorager.GetActivitiesById(id);

        }
       public int AddActivities(Activities activities)
        {
            return _DbSession.ActivitiesStorager.AddActivities(activities);
 
        }
       public bool UpdateActivities(Activities activities)
       {
           return _DbSession.ActivitiesStorager.UpdateActivities(activities);
       }
       public bool DeleteActivities(int id,string name)
       {
           return _DbSession.ActivitiesStorager.DeleteActivities(id,name);
       }

       public bool ApprovedActivities(int id, string userId,string userName,int status,string memo) {
           //using (TransactionScope scope = new TransactionScope())
           //{
               var approve = new ApproveRecord();
               approve.ItemId = id;
               approve.ItemType = ((int)EApproveType.Activities).ToString();
               approve.IsApproval = status;
               approve.ApprovalMemo = memo;
               approve.OperatorId = userId;
               approve.OperatorName = userName;
               _AppContext.ApproveRecordApp.UpdateStatus(approve);
               return _DbSession.ActivitiesStorager.ApprovedActivities(id, userId ,status);
           //}
       }

        public IEnumerable<Activities> GetActivities(int status, int pageIndex, int pageSize, out int totalCount)
        {
            return _DbSession.ActivitiesStorager.GetActivities(status, pageIndex, pageSize, out totalCount);
        }

        public bool UpdateIsDisplay(int id, int status, string operatorName)
        {
            return _DbSession.ActivitiesStorager.UpdateIsDisplay(id, status, operatorName);
        }

        public bool UpdateAllDisplay(int id, int priority, int isDisplay, int isHot, string operatorName)
        {
            return _DbSession.ActivitiesStorager.UpdateAllDisplay(id, priority, isDisplay, isHot, operatorName);
        }



        public ReturnResult ServiceCardConsum(long cardId)
        {
            ReturnResult result = new ReturnResult { IsSuccess = true };

            //密钥
         

            //调用服务
            var data = _DbSession.ActivitiesStorager.CancelCusomCard(cardId);

            if (data.ret != 1)
            {
                result.IsSuccess = false;
                result.Message = data.msg;
            }
            else
            {
                result.Data = data;
            }

            return result;
        }
    }
}
