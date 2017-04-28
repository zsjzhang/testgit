using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.Application
{
    /// <summary>
    /// 用户经验值业务逻辑
    /// </summary>
    public interface IUserEmpiricApp
    {
        /// <summary>
        /// 添加用户经验记录
        /// </summary>
        /// <param name="data"></param>
        void Add(UserEmpiricRecord data);

        /// <summary>
        /// 获取有效的用户经验记录信息
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="pageData"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        IEnumerable<UserEmpiricRecord> GetList(string userId, PageData pageData, out int total);

        /// <summary>
        /// 获取用户总经验值
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        int TotalValue(string userId);
    }
}
