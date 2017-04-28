using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity.Enum;

namespace Vcyber.BLMS.Application
{
    /// <summary>
    /// 面包业务逻辑
    /// </summary>
    public interface IBreadApp
    {
        /// <summary>
        /// 蓝豆面包
        /// </summary>
        /// <param name="ruleType">篮豆规则类型</param>
        /// <param name="userId">用户Id</param>
        /// <param name="level">用户级别</param>
        /// <param name="blueBeanValue">获得的蓝豆值</param>
        /// <returns>true:成功</returns>
        bool BlueBeanBread(EBRuleType ruleType, string userId, MemshipLevel level, out int blueBeanValue);

        /// <summary>
        /// 经验值面包
        /// </summary>
        /// <param name="ruleType">经验值规则类型</param>
        /// <param name="userId"></param>
        /// <param name="empiricValue">获得的经验值</param>
        /// <returns>true:成功</returns>
        bool EmpiricBread(EEmpiricRule ruleType, string userId, out int empiricValue);

        /// <summary>
        /// 会员是否可以获取经验值面包
        /// </summary>
        /// <param name="ruleType"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        bool IsEmiricBread(EEmpiricRule ruleType, string userId, out int empiricValue);
    }
}
