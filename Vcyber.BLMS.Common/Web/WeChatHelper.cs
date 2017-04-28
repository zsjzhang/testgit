using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Common.Web
{
    public class WeChatHelper
    {
        /// <summary>
        /// 从微信沾点获取Access_token
        /// </summary>
        /// <returns></returns>
        public static String GetWeChatBaseAccess_Token()
        {
            string access_token = "";
            string getAccess_tokenUr = "https://www.bluemembers.com.cn/weixin/wechat/common/GetAccessToken?key=d317857f468c8972547df70da5b1ace5798058c6";
            var json_token = WebUtils.GET_WebRequestHTML("utf-8", getAccess_tokenUr);
            if (!string.IsNullOrEmpty(json_token))
            {
                try
                {
                    var str_token = WebUtils.JsonToObj<WechatAccess_Token>(json_token, null);
                    if (str_token != null && str_token.ret == 1)
                    {
                        access_token = str_token.msg;
                    }
                }
                catch
                {
                    access_token = "";
                }
            }

            return access_token;
        }
    }
    /// <summary>
    /// 从微信端获取token的实体
    /// </summary>
    class WechatAccess_Token
    {
        public int ret { get; set; }

        public String msg { get; set; }
    }
}
