using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;

namespace Vcyber.BLMS.IRepository
{
    /// <summary>
    /// 用户经验规则操作
    /// </summary>
    public interface IUserEmpiricRuleStorager
    {
        /// <summary>
        /// 获取经验值规则数据
        /// </summary>
        /// <param name="rule">经验值规则类型</param>
        /// <returns></returns>
        UserEmpiricRule SelectOne(EEmpiricRule rule);
    }
}
