﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.IRepository
{
    public interface IJoinActivityStorager
    {
        /// <summary>
        /// 获取活动的参加人员列表
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns></returns>
        IEnumerable<JoinActivity> GetJoinActivitiesByAId(int activityId);
        /// <summary>
        /// 获取人员的参与记录
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        IEnumerable<JoinActivity> GetJoinActivitiesByUId(string userId);
        /// <summary>
        /// 判断人员是否参加活动
        /// </summary>
        /// <param name="activityId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        bool IsUserJoinActivity(int activityId, string userId);
        /// <summary>
        /// 添加人员参与记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int AddJoinActivity(JoinActivity entity);
        /// <summary>
        /// 分页获取参加人员列表
        /// </summary>
        /// <param name="activityId"></param>
        /// <param name="pageData"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        IEnumerable<JoinActivity> GetJoinActivitiesAll(int activityId, PageData pageData, out int total);

        /// <summary>
        /// 修改人员参与信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int UpdateJoinActivity(JoinActivity entity);
        /// <summary>
        /// 根据Id获取报名信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        JoinActivity GetJoinActivityById(int id);
        /// <summary>
        /// 判断该微信号当天是否参加过抽奖
        /// </summary>
        /// <param name="activityId"></param>
        /// <param name="openId"></param>
        /// <returns></returns>
        bool IsUserJoinActivityByDay(int activityId, string openId);
        /// <summary>
        /// 根据活动和用户查询参加活动的数量
        /// </summary>
        int GetJoinActivityForCount(int activityId, string userId);
    }
}
