using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vcyber.BLMS.FrontWeb.Models.WeChat
{
    /// <summary>
    /// 微信请求code成功返回的
    /// </summary>
    public class SussCode
    {
        /// <summary>
        /// 网页授权接口调用凭证
        /// </summary>
        public String access_token { get; set; }

        /// <summary>
        /// 过期时间，秒
        /// </summary>
        public int expires_in { get; set; }

        /// <summary>
        /// 刷新 token
        /// </summary>
        public String refresh_token { get; set; }

        /// <summary>
        /// 用户唯一标识
        /// </summary>
        public String openid { get; set; }

        /// <summary>
        /// 用户授权的作用域
        /// </summary>
        public String scope { get; set; }


        public String unionid { get; set; }

    }
}