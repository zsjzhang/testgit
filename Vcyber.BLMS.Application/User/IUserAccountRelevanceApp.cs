using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.Application
{
    /// <summary>
    /// 用户 账户关联业务逻辑
    /// </summary>
    public interface IUserAccountRelevanceApp
    {
        #region ==== public method ====

        /// <summary>
        /// 用户关联 商城账户
        /// </summary>
        /// <param name="data"></param>
        void UserRelevance(UserAccountRelevance data);

        /// <summary>
        /// 车音通用户是否关联某个商城账户
        /// </summary>
        /// <param name="mallCode">商城Code</param>
        /// <param name="mallUserCode"></param>
        /// <returns>true:已经关联</returns>
        bool IsRelevance(string mallCode, string mallUserCode);

        /// <summary>
        /// 获取用户关联商城账户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        IEnumerable<UserAccountRelevance> GetList(int userId);

        /// <summary>
        /// 删除关联账户信息
        /// </summary>
        /// <param name="relevanceId"></param>
        /// <returns></returns>
        bool Delete(int relevanceId);

        #endregion
    }
}
