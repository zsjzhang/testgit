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
    public class XDActivityApp : IXDActivityApp
    {
        public XDActivityApp() { }
        public XDActivity GetXDActivityByActivityId(int activityId)
        {
            return _DbSession.XDActivityStorager.GetXDActivityByActivityId(activityId);
        }
        public IEnumerable<Entity.XDActivity> GetXDActivityList(int activityType, int activityId = 0) 
        {
            return _DbSession.XDActivityStorager.GetXDActivityList(activityType,activityId);
        }

        public void UpdateActivityLotteryBalanceCount(int activityId)
        {
            _DbSession.XDActivityStorager.UpdateActivityLotteryBalanceCount(activityId);
        }


        //后台置换活动调用方法===============================================================================
        /// <summary>
        /// 查询置换活动  根据活动状态
        /// </summary>
        /// <param name="dealer"></param>
        /// <param name="status"></param>
        /// <param name="approveStatus"></param>
        /// <param name="display"></param>
        /// <param name="isHot"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public IEnumerable<Entity.XDActivity> SelectStatus(string dealer, int? status, int? approveStatus, int? display, int? isHot, int pageIndex, int pageSize, out int totalCount)
        {
            if (status != null)
            {
                if ((status) == (int)EPermuteActivitiescsStatus.NoBegin)
                {
                    return _DbSession.XDActivityStorager.SelectNotStart(approveStatus, display, isHot, dealer, pageIndex, pageSize, out totalCount);
                }
                if ((status) == (int)EPermuteActivitiescsStatus.InProcess)
                {
                    return _DbSession.XDActivityStorager.SelectInProgress(approveStatus, display, isHot, dealer, pageIndex, pageSize, out totalCount);
                }
                if ((status) == (int)EPermuteActivitiescsStatus.Finished)
                {
                    return _DbSession.XDActivityStorager.SelectFinished(approveStatus, display, isHot, dealer, pageIndex, pageSize, out totalCount);
                }
            }
            return _DbSession.XDActivityStorager.Select(approveStatus, display, isHot, dealer, pageIndex, pageSize, out totalCount);

        }
        /// <summary>
        /// 根据Id查询活动 编辑活动时获得活动数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Entity.XDActivity GetXDActivityById(int id)
        {
            return _DbSession.XDActivityStorager.GetXDActivityById(id);

        }
        /// <summary>
        /// 添加活动
        /// </summary>
        /// <param name="activities"></param>
        /// <returns></returns>
        public int AddXDActivity(Entity.XDActivity activities)
        {
            return _DbSession.XDActivityStorager.AddXDActivity(activities);

        }
        //编辑活动
        public bool UpdateActivities(Entity.XDActivity activities)
        {
            return _DbSession.XDActivityStorager.UpdateActivities(activities);
        }
        //删除活动
        public bool DeleteActivities(int id, string name)
        {
            return _DbSession.XDActivityStorager.DeleteActivities(id, name);
        }
        public  bool updateActivitiesStatus(int id, string userId, int status)
        {
            return _DbSession.XDActivityStorager.updateActivitiesStatus(id,userId,status);
        }

    }
}
