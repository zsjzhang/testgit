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
    /// 用户蓝豆操作
    /// </summary>
    public interface IUserBlueBeanStorager
    {
        #region ==== 公共方法 ====

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
        IEnumerable<UserblueBean> SelectAll(string userId);

        /// <summary>
        /// 分页获取用户蓝豆
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pageData"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        IEnumerable<UserblueBean> SelectAll(string userId, PageData pageData, out int total);

        /// <summary>
        /// 获取用户有效蓝豆
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        IEnumerable<UserblueBean> SelectList(string userId);

        /// <summary>
        /// 减去用户蓝豆
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool SubBlueBean(int id, string userId, int subValue);

        /// <summary>
        /// 统计用户获取蓝豆的次数
        /// </summary>
        /// <param name="ruleType">蓝豆规则类型</param>
        /// <param name="userId">用户Id</param>
        /// <param name="condition">统计条件</param>
        /// <returns></returns>
        int Count(EBRuleType ruleType, string userId, BlueBeanCondition condition);

        /// <summary>
        /// 清理用户过期蓝豆
        /// </summary>
        /// <param name="userId"></param>
        void CleanBlueBean(string userId);

        void CleanBlueBean(IEnumerable<UserblueBean>  blBeans);

        #endregion
    }
}
