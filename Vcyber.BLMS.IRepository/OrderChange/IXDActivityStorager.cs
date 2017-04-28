using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.IRepository
{
    public interface IXDActivityStorager
    {
        /// <summary>
        /// 根据活动ID，获取活动
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns></returns>
        XDActivity GetXDActivityByActivityId(int activityId);
        /// <summary>
        /// 根据活动类型获取活动列表
        /// </summary>
        /// <param name="activityType">(1：预约试驾  2：预约置换)</param>
        /// <returns></returns>
        IEnumerable<XDActivity> GetXDActivityList(int activityType, int activityId = 0);
        void UpdateActivityLotteryBalanceCount(int activityId);

        //后台置换活动方法========================================
        IEnumerable<XDActivity> Select(int? approveStatus, int? displayStatus, int? isHot, string dealer, int pageIndex, int pageSize, out int totalCount);

        IEnumerable<XDActivity> SelectNotStart(int? approveStatus, int? displayStatus, int? isHot, string dealer, int pageIndex, int pageSize, out int totalCount);

        IEnumerable<XDActivity> SelectInProgress(int? approveStatus, int? displayStatus, int? isHot, string dealer, int pageIndex, int pageSize, out int totalCount);

        IEnumerable<XDActivity> SelectFinished(int? approveStatus, int? displayStatus, int? isHot, string dealer, int pageIndex, int pageSize, out int totalCount);

        XDActivity GetXDActivityById(int id);

        int AddXDActivity(XDActivity activities);

        bool UpdateActivities(XDActivity activities);

        bool updateActivitiesStatus(int id, string userId, int status);

        bool DeleteActivities(int id, string name);
    }
}
