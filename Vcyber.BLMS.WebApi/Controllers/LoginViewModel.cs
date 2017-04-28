using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vcyber.BLMS.WebApi.Models
{
    public class LoginViewModel
    {
        /// <summary>
        /// 手机号
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        //public bool RememberMe { get; set; }
        /// <summary>
        ///短信验证码
        /// </summary>
        public string Captcha { get; set; }

        /// <summary>
        /// 数据来源(blms:前台网站；blms_web：手机app;blms_wechat：微信)
        /// </summary>
        public string CreatedPerson { get; set; }

    }
}