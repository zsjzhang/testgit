using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    public class UserSecurityInfo
    {
        /// <summary>
        /// 手机绑定
        /// </summary>
        public bool PhoneNumberConfirmed { get; set; }

        /// <summary>
        /// 实名认证
        /// </summary>
        public bool IdentityConfirmed { get; set; }

        /// <summary>
        /// 邮箱绑定
        /// </summary>
        public bool EmailConfirmed { get; set; }

        /// <summary>
        /// 密保问题
        /// </summary>
        public bool SecurityQuestion { get; set; }

        /// <summary>
        /// 安全等级
        /// </summary>
        public string SecurityLevel { get; set; }
    }
}
