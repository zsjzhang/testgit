using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;

namespace Vcyber.BLMS.IRepository
{
    public interface IBlueBeanRuleStorager
    {
        /// <summary>
        /// 获取某个蓝豆规则
        /// </summary>
        /// <param name="ruleType">规则类型</param>
        /// <returns></returns>
        BlueBeanRule SelecRuletOne(EBRuleType ruleType);

        /// <summary>
        /// 获取用户蓝豆说明
        /// </summary>
        /// <param name="ruleId">规则Id</param>
        /// <returns></returns>
        IEnumerable<BlueBeanRuleUser> SelectRuleUserOne(string ruleId);
    }
}
