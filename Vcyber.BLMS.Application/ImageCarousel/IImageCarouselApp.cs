using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.Application
{
   public interface IImageCarouselApp
   {
       IEnumerable<ImageCarousel> GetImageCarouselList(int? approveStatus, int? type, int? start, int? count, out int totalCount);
       ImageCarousel GetImageCarouselById(int id);

       bool Delete(int id,string name);
       bool Update(ImageCarousel data);

       int Create(ImageCarousel data);
       int GetMaxPriority();
       int GetMinPriority();
       bool UpdatePriority(int id, int priority, string operatorName);
       bool ApprovedImageCarousel(int id, string userId, string userName, int status, string memo);
       IEnumerable<ImageCarousel> GetImageCarousel(int status, int pageIndex, int pageSize, out int totalCount);
   }
}
