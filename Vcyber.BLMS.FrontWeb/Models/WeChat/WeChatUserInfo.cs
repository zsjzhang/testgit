using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vcyber.BLMS.FrontWeb.Models.WeChat
{
    /// <summary>
    /// 微信用户信息
    /// </summary>
    public class WeChatUserInfo
    {
        public String openid { get; set; }

        public String nickname { get; set; }

        public String sex { get; set; }

        public String province { get; set; }

        public String city { get; set; }

        public String country { get; set; }

        public String headimgurl { get; set; }


        #region 以下是调用接口api.weixin.qq.com/cgi-bin/user/info 才会出现

        /// <summary>
        /// 是否关注标识；1-关注，0-没关注
        /// </summary>
        public int subscribe { get; set; }


        public long subscribe_time { get; set; }

        public String remark { get; set; }

        public int groupid { get; set; }

        #endregion
    }
}