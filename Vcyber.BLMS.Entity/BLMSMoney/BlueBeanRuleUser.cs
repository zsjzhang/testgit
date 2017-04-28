using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Vcyber.BLMS.Entity.Enum;

namespace Vcyber.BLMS.Entity
{
    /// <summary>
    /// 用户获取蓝豆说明
    /// </summary>
    public class BlueBeanRuleUser
    {
        #region ==== 构造函数 ====

        public BlueBeanRuleUser() { }

        #endregion

        #region ==== 公共属性 ====

        /// <summary>
        /// 说明Guid
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 蓝豆规则Id
        /// </summary>
        public string RuleId { get; set; }

        /// <summary>
        /// 会员等级：1：一星；2：二星；3：三星
        /// </summary>
        public EBUserGrade UserGrade { get; set; }

        /// <summary>
        /// 获取蓝豆值
        /// </summary>
        public int BlueBeanValue { get; set; }

        /// <summary>
        /// 说明是否失效：0：启用；1：失效
        /// </summary>
        public int DateState { get; set; }

        #endregion
    }
}
