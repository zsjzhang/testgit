using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.IRepository
{
    public interface IImageCarouselStorager
    {
        /// <summary>
        /// 根据Id，获取一个轮播图片实体
        /// </summary>
        /// <param name="id">图片Id</param>
        /// <returns></returns>
        ImageCarousel GetImgCarouselById(int id);

        /// <summary>
        /// 获取轮播图片list
        /// </summary>
        /// <param name="approveStatus">审批状态</param>
        /// <param name="type">类型（首页/新闻等）</param>
        /// <param name="start">index</param>
        /// <param name="count">page size</param>
        /// <param name="totalCount">审批状态</param>
        /// <returns></returns>
        IEnumerable<ImageCarousel> GetImageCarouselList(int? approveStatus, int? type, int? start, int? count, out int totalCount);

        /// <summary>
        /// 创建一个轮播图片
        /// </summary>
        /// <param name="entity">图片实体</param>
        /// <returns></returns>
        int CreateImgCarousel(ImageCarousel entity);

        /// <summary>
        /// 更新一个轮播图片
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool UpdateImgCarousel(ImageCarousel entity);

        /// <summary>
        /// 通过Id，删除响应的轮播图片
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        bool DeleteImgCarousel(int Id, string name);
        int GetMaxPriority();
        int GetMinPriority();
        ImageCarousel GetPreEntity(ImageCarousel entity);
        ImageCarousel GetNextEntity(ImageCarousel entity);
        bool ApprovedImageCarousel(int id, string userId, int status);
        IEnumerable<ImageCarousel> GetImageCarousel(int status, int pageIndex, int pageSize, out int totalCount);
        bool UpdatePriority(int id, int priority, string operatorName);
    }
}
