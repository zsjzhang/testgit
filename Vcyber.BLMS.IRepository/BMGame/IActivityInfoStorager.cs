using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.IRepository
{
    public interface IActivityInfoStorager
    {
        /// <summary>
        /// 根据编号获取活动详情
        /// </summary>
        /// <param name="id">活动编号</param>
        /// <returns></returns>
        ActivityInfo GetActivityInfoByID(int id);
        /// <summary>
        /// 查询单条活动记录
        /// </summary>
        /// <param name="name">活动名称</param>
        /// <returns></returns>
        ActivityInfo GetActivityInfoByName(string name);
        /// <summary>
        /// 获取所有活动记录
        /// </summary>
        /// <returns></returns>
        IEnumerable<ActivityInfo> GetActivityInfoAll();

        /// <summary>
        /// 添加活动记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int AddActivityInfo(ActivityInfo entity);

        /// <summary>
        /// 修改活动记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int UpdateActivityInfo(ActivityInfo entity);

        /// <summary>
        /// 获取活动编号
        /// </summary>
        /// <returns></returns>
        IEnumerable<int> GetDistinctActivityId();

        /// <summary>
        /// 结束活动
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int EndActivityInfo(int id);
    }
}
