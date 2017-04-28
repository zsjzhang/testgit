using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.IRepository;

namespace Vcyber.BLMS.Domain
{
    public class MagazineApp : IMagazineApp
    {
        public IEnumerable<Magazine> GetMagazineList(int? approveStatus, int? year, int? month, string name, int start, int count, out int total)
        {
            return _DbSession.MagazineStorager.GetMagazineList(approveStatus,year, month, name, start, count, out total);
        }

        public Magazine GetMagazineById(int id)
        {
            return _DbSession.MagazineStorager.GetMagazineById(id);
        }

        public bool Delete(int id,string name)
        {
            return _DbSession.MagazineStorager.Delete(id,name);
        }

        public bool Update(Magazine data)
        {
            return _DbSession.MagazineStorager.Update(data);
        }

        public int Create(Magazine data)
        {
            return _DbSession.MagazineStorager.Create(data);
        }
        public IEnumerable<Magazine> GetMagazine(int status, int pageIndex, int pageSize, out int totalCount)
        {
            return _DbSession.MagazineStorager.GetMagazine(status, pageIndex, pageSize, out totalCount);

        }

        public bool ApprovedMagazine(int id, string userId, string userName, int status, string memo)
        {
            var approve = new ApproveRecord();
            approve.ItemId = id;
            approve.ItemType = ((int)EApproveType.Magazine).ToString();
            approve.IsApproval = status;
            approve.ApprovalMemo = memo;
            approve.OperatorId = userId;
            approve.OperatorName = userName;
            _AppContext.ApproveRecordApp.UpdateStatus(approve);
            return _DbSession.MagazineStorager.ApprovedMagazine(id, userId, status);
       
        }


        public IEnumerable<Magazine> GetMagazineAll()
        {
            return _DbSession.MagazineStorager.GetMagazineAll();
        }
    }
}
