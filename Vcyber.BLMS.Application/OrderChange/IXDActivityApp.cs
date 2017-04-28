using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.Application
{
    public interface IXDActivityApp
    {
        XDActivity GetXDActivityByActivityId(int activityId);
        /// <summary>
        /// 根据活动类型获取活动列表
        /// </summary>
        /// <param name="activityType">(1：预约试驾  2：预约置换)</param>
        /// <returns></returns>
        IEnumerable<XDActivity> GetXDActivityList(int activityType, int activityId = 0);
        void UpdateActivityLotteryBalanceCount(int activityId);

        //后台置换活动调用方法 =======================================================================
        //后台置换活动方法========================================
        IEnumerable<Entity.XDActivity> SelectStatus(string dealer, int? status, int? approveStatus, int? display, int? isHot, int pageIndex, int pageSize, out int totalCount);

        Entity.XDActivity GetXDActivityById(int id);

        int AddXDActivity(Entity.XDActivity activities);


        bool UpdateActivities(XDActivity activities);

        bool updateActivitiesStatus(int id, string userId, int status);

        bool DeleteActivities(int id, string name);


    }
}
