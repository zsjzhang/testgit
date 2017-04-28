using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vcyber.BLMS.FrontWeb.Models.WeChat
{
    public class WeChatBaseToken
    {
        public String access_token { get; set; }

        public long expires_in { get; set; }
    }

    /// <summary>
    /// 从微信端获取token的实体
    /// </summary>
    public class WechatAccess_Token
    {
        public int ret { get; set; }

        public String msg { get; set; }
    }
}