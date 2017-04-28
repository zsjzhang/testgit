using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using  Vcyber.BLMS.Entity.NoticeInfos;

namespace Vcyber.BLMS.IRepository
{
    /// <summary>
    /// 公告信息
    /// </summary>
    public interface INoticeInfoStorager
    {
        /// <summary>
        /// 新增公告信息
        /// </summary>
        /// <param name="notice"></param>
        /// <returns></returns>
        int AddNoticeInfo(NoticeInfos notice);

        /// <summary>
        /// 修改公告信息
        /// </summary>
        /// <param name="notice"></param>
        /// <returns></returns>
        int UpdateNoticeInfo(NoticeInfos notice);

        /// <summary>
        /// 删除公告信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int DelNoticeInfoByID(int id);

        /// <summary>
        /// 获取一条
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        NoticeInfos GetNoticeInfoById(int id);

        /// <summary>
        /// 获取最新一条
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        NoticeInfos GetNewNoticeInfo();

        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="queryObj"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        IEnumerable<NoticeInfos> GetNoticeInfos(object queryObj, int pageIndex, int pageSize, out int totalCount);

        /// <summary>
        /// 获取所有
        /// </summary>
        /// <param name="queryObj"></param>
        /// <returns></returns>
        IEnumerable<NoticeInfos> GetNoticeInfos(object queryObj);
    }
}
