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
    public class ImageCarouselApp : IImageCarouselApp
    {
        public IEnumerable<ImageCarousel> GetImageCarouselList(int? approveStatus,int? type, int? start,int? count, out int totalCount)
        {
            return _DbSession.ImageCarouselStorager.GetImageCarouselList(approveStatus, type, start, count,out totalCount);
        }

        public ImageCarousel GetImageCarouselById(int id)
        {
            return _DbSession.ImageCarouselStorager.GetImgCarouselById(id);
        }

        public bool Delete(int id,string name)
        {
            return _DbSession.ImageCarouselStorager.DeleteImgCarousel(id,name);
        }

        public bool Update(ImageCarousel data)
        {
            return _DbSession.ImageCarouselStorager.UpdateImgCarousel(data);
        }

        public int Create(ImageCarousel data)
        {
            return _DbSession.ImageCarouselStorager.CreateImgCarousel(data);
        }

        public int GetMaxPriority()
        {
            return _DbSession.ImageCarouselStorager.GetMaxPriority();
        }

        public int GetMinPriority()
        {
            return _DbSession.ImageCarouselStorager.GetMinPriority();
        }

        public bool ApprovedImageCarousel(int id, string userId, string userName, int status, string memo)
        {
            var approve = new ApproveRecord();
            approve.ItemId = id;
            approve.ItemType = ((int) EApproveType.ImageCarousel).ToString();
            approve.IsApproval = status;
            approve.ApprovalMemo = memo;
            approve.OperatorId = userId;
            approve.OperatorName = userName;
            _AppContext.ApproveRecordApp.UpdateStatus(approve);
            return _DbSession.ImageCarouselStorager.ApprovedImageCarousel(id, userId, status);
        }

        public bool UpdatePriority(int id, int priority, string operatorName)
        {
            return _DbSession.ImageCarouselStorager.UpdatePriority(id, priority, operatorName);
        }

        public IEnumerable<ImageCarousel> GetImageCarousel(int status, int pageIndex, int pageSize, out int totalCount)
        {
            return _DbSession.ImageCarouselStorager.GetImageCarousel(status, pageIndex, pageSize, out totalCount);

        }


    }
}
