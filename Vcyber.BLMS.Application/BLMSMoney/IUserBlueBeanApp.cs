using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.Application
{
    /// <summary>
    /// 用户蓝豆业务
    /// </summary>
    public interface IUserBlueBeanApp
    {
        /// <summary>
        /// 添加用户蓝豆
        /// </summary>
        /// <param name="data"></param>
        void Add(UserblueBean data);

        /// <summary>
        /// 获取用户总蓝豆
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        int GetTotalBlueBean(string userId);

        /// <summary>
        /// 获取用户全部蓝豆记录
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        IEnumerable<UserblueBean> GetAll(string userId);

        /// <summary>
        /// 获取用户有效蓝豆
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        IEnumerable<UserblueBean> GetList(string userId);

        /// <summary>
        /// 获取用户有效积分
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        IEnumerable<UserblueBean> GetAll(string userId, PageData pageData, out int total);
    }
}
