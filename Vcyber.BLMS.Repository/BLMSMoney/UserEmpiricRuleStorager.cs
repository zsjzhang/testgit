using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.SelectCondition;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.IRepository;

namespace Vcyber.BLMS.Repository
{
    /// <summary>
    /// 用户经验值规则操作
    /// </summary>
    public class UserEmpiricRuleStorager : IUserEmpiricRuleStorager
    {
        #region ==== 构造函数 ====

        public UserEmpiricRuleStorager() { }

        #endregion

        #region ==== 公共方法 ====

        /// <summary>
        /// 获取经验值规则数据
        /// </summary>
        /// <param name="rule">经验值规则类型</param>
        /// <returns></returns>
        public UserEmpiricRule SelectOne(EEmpiricRule rule)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(" select * from UserEmpiricRule where UserEmpiricRule.Type=@Type and UserEmpiricRule.DataState=@DataState");
            return DbHelp.QueryOne<UserEmpiricRule>(sql.ToString(), new { Type = rule.ToInt32(), DataState=EDataStatus.NoDelete.ToInt32() });
        }

        #endregion
    }
}
