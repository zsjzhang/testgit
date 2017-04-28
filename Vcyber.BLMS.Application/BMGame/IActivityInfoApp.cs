using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.Application
{
    public interface IActivityInfoApp
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
        List<ActivityInfo> GetActivityInfoAll();

        /// <summary>
        /// 添加活动记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool AddActivityInfo(ActivityInfo entity);

        /// <summary>
        /// 修改活动记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool UpdateActivityInfo(ActivityInfo entity);

        List<int> GetDistinctActivityId();
        bool EndActivityInfo(int id);

        /// <summary>
        /// 抽奖（根据活动的中奖率）
        /// </summary>
        /// <param name="activityId">活动ID</param>
        /// <param name="userId">用户ID</param>        
        /// <returns>中奖的奖品</returns>
        PrizesInfo NormalDraw(int activityId, string userId, string source = "blms_web");
        /// <summary>
        /// 抽奖（感恩季活动）
        /// </summary>
        /// <param name="activityId">活动ID</param>
        /// <param name="userId">用户ID</param>        
        /// <returns>中奖的奖品</returns>
        PrizesInfo ThankDraw(int activityId, string userId, string source = "blms_web");
    }
}
