using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.Application
{
    public interface IShareResourcesApp
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="shareRes"></param>
        /// <returns></returns>
        int AddShareRes(ShareResources shareRes);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="shareRes"></param>
        /// <returns></returns>
        int UpdateShareRes(ShareResources shareRes);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int DelShareResByID(int id);

        /// <summary>
        /// 获取一条
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ShareResources GetShareResById(int id);

        /// <summary>
        /// 获取最新一条
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ShareResources GetNewShareRes();

        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="queryObj"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        IEnumerable<ShareResources> GetShareRes(object queryObj, int fileType, string category, int pageIndex, int pageSize, out int totalCount);

        /// <summary>
        /// 获取所有
        /// </summary>
        /// <param name="queryObj"></param>
        /// <returns></returns>
        IEnumerable<ShareResources> GetShareRes(object queryObj);
    }
}
