using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Vcyber.BLMS.Entity.Enum;

namespace Vcyber.BLMS.Entity
{
    /// <summary>
    /// 蓝豆规则
    /// </summary>
    public class BlueBeanRule
    {
        #region ==== 构造函数 ====

        public BlueBeanRule() { }

        #endregion

        #region ==== 公共属性 ====

        /// <summary>
        /// 规则Guid
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 蓝豆规则类型
        /// </summary>
        public EBRuleType Type { get; set; }

        /// <summary>
        /// 获取方式；1:一次性；2：每日；3：每月;4:每次
        /// </summary>
        public int AcquireMode { get; set; }

        /// <summary>
        /// 最大获取次数
        /// </summary>
        public int MaxCount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 规程是否失效：0：启用；1：失效
        /// </summary>
        public int DateState { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        #endregion

        #region ==== 附件属性 ====

        /// <summary>
        /// 用户获取蓝豆说明
        /// </summary>
        public IEnumerable<BlueBeanRuleUser> RuleUsers { get; set; }

        #endregion
    }
}
