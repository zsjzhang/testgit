using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.IRepository
{
    public interface IUserGuideStorager
    {
        UserGuide GetUserGuideById(int id);
        IEnumerable<UserGuide> GetUserGuideList(string title, int? approveStatus, int start, int count, out int total);
        IEnumerable<UserGuide> GetUserGuideList(int? approveStatus, int start, int count, out int total);
        int Create(UserGuide entity);
        bool Update(UserGuide entity);
        bool Delete(int Id, string name);
        bool UpdateIsDisplay(int Id, int status,string name);
        bool UpdateDownloadTimes(int Id, int times, string name);
        IEnumerable<UserGuide> GetUserGuide(int status, int pageIndex, int pageSize, out int totalCount);
        bool ApprovedUserGuide(int id, string userId, int status);
    }
}
