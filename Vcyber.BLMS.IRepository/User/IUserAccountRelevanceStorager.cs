using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.IRepository
{
    /// <summary>
    /// 用户 商城账户关联操作
    /// </summary>
    public interface IUserAccountRelevanceStorager
    {
        #region ==== public method ====

        /// <summary>
        /// 添加账户关联
        /// </summary>
        /// <param name="data"></param>
        void Add(UserAccountRelevance data);

        /// <summary>
        /// 车音通用户是否关联某个商城账户
        /// </summary>
        /// <param name="mallCode">商城Code</param>
        /// <param name="mallUserCode"></param>
        /// <returns>true:已经关联</returns>
        bool IsRelevance(string mallCode, string mallUserCode);

        /// <summary>
        /// 获取车音通用户关联商城账户信息
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        IEnumerable<UserAccountRelevance> SelectList(int userId);

        /// <summary>
        /// 删除用户的某个商城关联账户
        /// </summary>
        /// <param name="relevanceId">关联账户Id</param>
        /// <returns></returns>
        bool Delete(int relevanceId);

        #endregion
    }
}
