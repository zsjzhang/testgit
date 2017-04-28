using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;

using Vcyber.BLMS.Application;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.IRepository;

namespace Vcyber.BLMS.Domain
{
    /// <summary>
    /// 用户经验值业务逻辑
    /// </summary>
    public class UserEmpiricApp : IUserEmpiricApp
    {
        #region ==== 构造函数 ====

        public UserEmpiricApp() { }

        #endregion

        #region ==== 公共方法 ====

        /// <summary>
        /// 添加用户经验记录
        /// </summary>
        /// <param name="data"></param>
        public void Add(UserEmpiricRecord data)
        {
            _DbSession.UserEmpiricStorager.Add(data);
        }

        /// <summary>
        /// 获取有效的用户经验记录信息
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="pageData"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public IEnumerable<UserEmpiricRecord> GetList(string userId, PageData pageData, out int total)
        {
            return _DbSession.UserEmpiricStorager.SelectList(userId, pageData, out total);
        }

        /// <summary>
        /// 获取用户总经验值
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int TotalValue(string userId)
        {
            return 0;
        }



        #endregion
    }
}
