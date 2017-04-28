using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vcyber.BLMS.Entity
{
    using Vcyber.BLMS.Entity.Enum;

    public class AutoRegisterViewModel
    {
        /// <summary>
        /// 手机号码
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
         public string Captcha { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
         public string NickName { get; set; }

        /// <summary>
        /// 会员类型(1:非车主，2：车主，3：银卡会员)
        /// </summary>
         public MembershipType MType { get; set; }

        /// <summary>
        /// 身份证号
        /// </summary>
         public string Identity { get; set; }

        /// <summary>
        /// Vin
        /// </summary>
         public string VIN { get; set; }
    }
}