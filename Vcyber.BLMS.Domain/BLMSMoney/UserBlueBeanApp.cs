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
    /// <summary>
    /// 用户蓝豆业务
    /// </summary>
    public class UserBlueBeanApp : IUserBlueBeanApp
    {
        #region ==== 构造函数 ====

        public UserBlueBeanApp() { }

        #endregion

        #region ==== 公共方法 ====

        /// <summary>
        /// 添加用户蓝豆
        /// </summary>
        /// <param name="data"></param>
        public void Add(UserblueBean data)
        {
            _DbSession.UserBlueBeanStorager.Add(data);
        }

        /// <summary>
        /// 获取用户总蓝豆
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int GetTotalBlueBean(string userId)
        {
            return 0;
        }

        /// <summary>
        /// 获取用户全部蓝豆记录
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IEnumerable<UserblueBean> GetAll(string userId)
        {
            return _DbSession.UserBlueBeanStorager.SelectAll(userId);
        }

        /// <summary>
        /// 获取用户有效蓝豆
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IEnumerable<UserblueBean> GetList(string userId)
        {
            return _DbSession.UserBlueBeanStorager.SelectList(userId);
        }

        /// <summary>
        /// 获取用户蓝豆
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IEnumerable<UserblueBean> GetAll(string userId,PageData pageData,out int total)
        {
            return _DbSession.UserBlueBeanStorager.SelectAll(userId,pageData,out total);
        }
        #endregion

    }
}
