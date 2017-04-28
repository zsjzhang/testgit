using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.IRepository;

namespace Vcyber.BLMS.Domain
{
   public class ApproveRecordApp:IApproveRecordApp
    {
       public bool UpdateStatus(ApproveRecord record) 
       {
           if(record !=null)
           {
               
                   return _DbSession.ApproveRecordStorager.CreateApproveRecord(record);
              
           }
           return true;
       }

       public IEnumerable<ApproveRecord> GetApproveRecordList(int itemId, string itemType)
       {
           return _DbSession.ApproveRecordStorager.GetApproveRecordList(itemId, itemType);
       }

       public ApproveRecord GetLatestRecord(int itemId, string itemType) 
       {
           return _DbSession.ApproveRecordStorager.GetLatestRecord(itemId,itemType);
       }
    }
}
