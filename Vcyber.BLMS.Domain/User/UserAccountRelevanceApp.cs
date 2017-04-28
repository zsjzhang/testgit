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
    /// 用户账户关联 业务逻辑
    /// </summary>
    public class UserAccountRelevanceApp : IUserAccountRelevanceApp
    {
        #region ==== public constructor ====

        public UserAccountRelevanceApp() { }

        #endregion

        #region ==== public method ====

        /// <summary>
        /// 用户关联 商城账户
        /// </summary>
        /// <param name="data"></param>
        public void UserRelevance(UserAccountRelevance data)
        {
            _DbSession.UserAccountRelevanceStorager.Add(data);
        }

        /// <summary>
        /// 车音通用户是否关联某个商城账户
        /// </summary>
        /// <param name="mallCode">商城Code</param>
        /// <param name="mallUserCode"></param>
        /// <returns>true:已经关联</returns>
        public bool IsRelevance(string mallCode, string mallUserCode)
        {
            return _DbSession.UserAccountRelevanceStorager.IsRelevance(mallCode, mallUserCode);
        }

        /// <summary>
        /// 获取用户关联商城账户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IEnumerable<UserAccountRelevance> GetList(int userId)
        {
            return _DbSession.UserAccountRelevanceStorager.SelectList(userId);
        }

        /// <summary>
        /// 删除关联账户信息
        /// </summary>
        /// <param name="relevanceId"></param>
        /// <returns></returns>
        public bool Delete(int relevanceId)
        {
            return _DbSession.UserAccountRelevanceStorager.Delete(relevanceId);
        }

        #endregion
    }
}
