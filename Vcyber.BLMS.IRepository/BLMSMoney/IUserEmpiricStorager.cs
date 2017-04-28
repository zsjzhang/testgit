using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.Entity.SelectCondition;

namespace Vcyber.BLMS.IRepository
{
    /// <summary>
    /// 用户经验值操作
    /// </summary>
    public interface IUserEmpiricStorager
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
        IEnumerable<UserEmpiricRecord> SelectList(string userId, PageData pageData, out int total);

        /// <summary>
        /// 获取用户总经验值
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        int TotalValue(string userId);

        /// <summary>
        /// 扣除用户经验值
        /// </summary>
        /// <param name="id">经验值Id</param>
        /// <param name="userId">用户Id</param>
        /// <param name="value">需要扣除的经验值</param>
        /// <returns></returns>
        bool SubValue(int id, string userId, int value);

        /// <summary>
        /// 统计用户获得某种经验值的次数
        /// </summary>
        /// <param name="rule">经验值规则</param>
        /// <param name="userId">用户Id</param>
        /// <param name="condition"></param>
        /// <returns></returns>
        int Count(EEmpiricRule rule, string userId, UserEmpiricCondition condition);
    }
}
