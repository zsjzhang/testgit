using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.IRepository;

namespace Vcyber.BLMS.Repository
{
    /// <summary>
    /// 蓝豆规则操作
    /// </summary>
    public class BlueBeanRuleStorager : IBlueBeanRuleStorager
    {
        #region ==== 构造函数 ====

        public BlueBeanRuleStorager() { }

        #endregion

        #region ==== 公共方法 ====

        /// <summary>
        /// 获取某个蓝豆规则
        /// </summary>
        /// <param name="ruleType">规则类型</param>
        /// <returns></returns>
        public BlueBeanRule SelecRuletOne(EBRuleType ruleType)
        {
            string sql = "select * from blueBeanRule where blueBeanRule.Type=@Type and blueBeanRule.DateState=@DateState";
            var ruleData = DbHelp.QueryOne<BlueBeanRule>(sql, new { Type = ruleType.ToInt32(), DateState =EDataStatus.NoDelete.ToInt32()});

            if (ruleData!=null)
            {
                ruleData.RuleUsers = this.SelectRuleUserOne(ruleData.Id);
            }

            return ruleData;
        }

        /// <summary>
        /// 获取用户蓝豆说明
        /// </summary>
        /// <param name="ruleId">规则Id</param>
        /// <returns></returns>
        public IEnumerable<BlueBeanRuleUser> SelectRuleUserOne(string ruleId)
        {
            string sql = "select * from blueBeanRuleUser where blueBeanRuleUser.RuleId=@RuleId and blueBeanRuleUser.DateState=@DateState";
            return DbHelp.Query<BlueBeanRuleUser>(sql, new { RuleId = ruleId, DateState=EDataStatus.NoDelete.ToInt32() });
        }

        #endregion
    }
}
