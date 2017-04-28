using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.IRepository;

namespace Vcyber.BLMS.Domain
{
    public class JoinActivityApp : IJoinActivityApp
    {
        public List<JoinActivity> GetJoinActivitiesByAId(int activityId)
        {
            return _DbSession.JoinActivityStorager.GetJoinActivitiesByAId(activityId).ToList();
        }

        public List<JoinActivity> GetJoinActivitiesByUId(string userId)
        {
            return _DbSession.JoinActivityStorager.GetJoinActivitiesByUId(userId).ToList();
        }

        public bool IsUserJoinActivity(int activityId, string userId)
        {
            return _DbSession.JoinActivityStorager.IsUserJoinActivity(activityId, userId);
        }

        public bool AddJoinActivity(JoinActivity entity)
        {
            int offsetnum = _DbSession.JoinActivityStorager.AddJoinActivity(entity);
            if (offsetnum > 0) return true;
            else return false;
        }


        public List<JoinActivity> GetJoinActivitiesAll(int activityId, PageData pageData, out int total)
        {
            return _DbSession.JoinActivityStorager.GetJoinActivitiesAll(activityId, pageData, out total).ToList();
        }

        public bool UpdateJoinActivity(JoinActivity entity)
        {
            int ret= _DbSession.JoinActivityStorager.UpdateJoinActivity(entity);
            return ret > 0;
        }


        public int AddJoinActivityNew(JoinActivity entity)
        {
            return _DbSession.JoinActivityStorager.AddJoinActivity(entity);
        }

        public JoinActivity GetJoinActivityById(int id)
        {
            return _DbSession.JoinActivityStorager.GetJoinActivityById(id);
        }


        public bool IsUserJoinActivityByDay(int activityId, string openId)
        {
            return _DbSession.JoinActivityStorager.IsUserJoinActivityByDay(activityId, openId);
        }

        /// <summary>
        /// 根据活动和用户查询参加活动的数量
        /// </summary>
        public int GetJoinActivityForCount(int activityId, string userId)
        {
            return _DbSession.JoinActivityStorager.GetJoinActivityForCount(activityId, userId);
        }
    }
}
