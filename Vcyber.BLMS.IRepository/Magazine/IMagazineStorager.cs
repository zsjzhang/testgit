using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.IRepository
{
   public interface IMagazineStorager
   {
       IEnumerable<Magazine> GetMagazineList(int? approveStatus, int? year, int? month, string name, int start, int count, out int total);

       IEnumerable<Magazine> GetMagazineAll();
       
       Magazine GetMagazineById(int id);

       bool Delete(int id,string name);
       bool Update(Magazine data);

       int Create(Magazine data);
       IEnumerable<Magazine> GetMagazine(int? status, int pageIndex, int pageSize, out int totalCount);
       bool ApprovedMagazine(int id, string userId, int status);
   }
}
