using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.Common.Web;
using Vcyber.BLMS.FrontWeb.Models.WeChat;
using System.ComponentModel;
using System.Web.Caching;
using Vcyber.BLMS.Application;

namespace Vcyber.BLMS.FrontWeb.Controllers.Musical
{
    /// <summary>
    /// 微信相关
    /// </summary>
    public class WeChatController : Controller
    {
        private String strState1 = "base";
        private String strState2 = "userinfo";
        private String strAppid = "wx8ba75eb0ebbb764b";
        private String strSecret = "db00357b1a0cff6494aedea9a081a69b";

        /// <summary>
        /// 微信用户授权（Scope为snsapi_userinfo）
        /// </summary>
        /// <param name="redirect"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public ActionResult Index(string code = "", string state = "", string source = "")
        {
            var redirect = "https://www.bluemembers.com.cn/WeChat/index?source=" + source;  // this.Request.Url.ToString();
            if (!String.IsNullOrEmpty(code) && !String.IsNullOrEmpty(state))
            {
                if (state.ToUpper().Equals(strState1.ToUpper()))
                {
                    //做用户是否关注公众号验证，做snsapi_userinfo用户授权
                    var tokenInfo = GetAccess_Token(code);



                    if (tokenInfo != null && !string.IsNullOrEmpty(tokenInfo.access_token))
                    {
                        var user = GetSignUserInfo(tokenInfo.openid);
                        if (user != null && user.subscribe == 1)
                        {
                            ViewData["provinceList"] = Vcyber.BLMS.Common.City.CityService.Instance.GetProvince();
                            ViewData["powerWinnerList"] = _AppContext.WinningInfoApp.GetWinningsByWhere(1, " Prizesid=10 and  UpdateTime >'2015-11-10'");
                            ViewData["piaoWinerList"] = _AppContext.WinningInfoApp.GetWinningsByWhere(1, " Prizesid=9 and  UpdateTime >'2015-11-10'");
                            ViewBag.source = source;
                            return View();
                        }
                    }
                    //未关注,走授权
                    var url = GetRedirectUrl(redirect, "snsapi_userinfo", strState2);
                    return Redirect(url);

                }
                else if (state.ToUpper().Equals(strState2.ToUpper()))
                {
                    return RedirectToAction("Follow", new { });
                }

                //未关注,走授权
                var returnUrl = GetRedirectUrl(redirect, "snsapi_userinfo", strState2);
                return Redirect(returnUrl);
            }
            else
            {
                //参数错误，请求用户授权     
                var url = GetRedirectUrl(redirect, "snsapi_base", strState1);
                return Redirect(url.ToString());
            }
        }


        public ActionResult test(string code = "", string state = "", string source = "")
        {
            var redirect = "https://www.bluemembers.com.cn/WeChat/index?source=" + source;
            if (!String.IsNullOrEmpty(code) && !String.IsNullOrEmpty(state))
            {
                if (state.ToUpper().Equals(strState1.ToUpper()))
                {
                    //做用户是否关注公众号验证，做snsapi_userinfo用户授权
                    var tokenInfo = GetAccess_Token(code);

                    if (tokenInfo == null || tokenInfo.access_token == null)
                    {
                        return Content("1、获取access_token失败");
                    }
                    var user = GetSignUserInfo(tokenInfo.openid);
                    if (user == null || String.IsNullOrEmpty(user.openid) || user.subscribe == 0)
                    {
                        if (user == null || String.IsNullOrEmpty(user.openid))
                        {
                            return Content("1、获取的用户信息为null");
                        }
                        //未关注,走授权
                        var url = GetRedirectUrl(redirect, "snsapi_userinfo", strState2);
                        return Redirect(url);
                    }
                    //if (user.subscribe==1)
                    //{      //已经关注,跳出流程，走活动页面
                    return Content("1、" + WebUtils.ObjToJson(user));
                    //}

                }
                else if (state.ToUpper().Equals(strState2.ToUpper()))
                {
                    //授权成功,走未关注
                    var tokenInfo = GetAccess_Token(code);
                    if (tokenInfo == null || tokenInfo.access_token == null)
                    {
                        return Content("2、获取access_token失败");
                    }
                    var userInfo = tokenInfo != null ? GetUserInfo(tokenInfo.access_token, tokenInfo.openid) : null;
                    if (userInfo == null || String.IsNullOrEmpty(userInfo.openid))
                    {
                        return Content("2、获取用户信息失败");
                    }
                    return Content("2、" + WebUtils.ObjToJson(userInfo));
                }

                //未关注,走授权
                var returnUrl = GetRedirectUrl(redirect, "snsapi_userinfo", strState2);
                return Redirect(returnUrl);
            }
            else
            {
                //参数错误，请求用户授权     
                var url = GetRedirectUrl(redirect, "snsapi_base", strState1);
                return Redirect(url.ToString());
            }
        }

        /// <summary>
        /// 关注页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Follow()
        {
            return View();
        }

        #region
        private String GetRedirectUrl(String backUrl, String scope, String state)
        {
            var responseType = "code";
            StringBuilder url = new StringBuilder();
            url.Append("https://open.weixin.qq.com/connect/oauth2/authorize?");
            url.AppendFormat("appid={0}", this.strAppid);
            url.AppendFormat("&redirect_uri={0}", HttpUtility.UrlEncode(backUrl));
            url.AppendFormat("&response_type={0}", responseType);
            url.AppendFormat("&scope={0}", scope);
            url.AppendFormat("&state={0}", state);
            url.AppendFormat("#wechat_redirect");

            return url.ToString();
        }

        /// <summary>
        /// 获取access_token
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private SussCode GetAccess_Token(String code)
        {
            if (String.IsNullOrEmpty(code))
            {
                return null;
            }
            StringBuilder url = new StringBuilder();
            url.Append("https://api.weixin.qq.com/sns/oauth2/access_token?");
            url.AppendFormat("appid={0}", this.strAppid);
            url.AppendFormat("&secret={0}", this.strSecret);
            url.AppendFormat("&code={0}", code);
            url.AppendFormat("&grant_type={0}", "authorization_code");

            var json = WebUtils.GET_WebRequestHTML("utf-8", url.ToString());
            if (String.IsNullOrEmpty(json))
            {   //请求失败
                return null;
            }
            return WebUtils.JsonToObj<SussCode>(json, null);
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="token"></param>
        /// <param name="openId"></param>
        /// <returns></returns>
        private WeChatUserInfo GetUserInfo(String token, String openId)
        {
            if (String.IsNullOrEmpty(token) || String.IsNullOrEmpty(openId))
            {
                return null;
            }
            StringBuilder url_user = new StringBuilder();
            url_user.Append("https://api.weixin.qq.com/sns/userinfo?");
            url_user.AppendFormat("access_token={0}&openid={1}&lang=zh_CN", token, openId);

            var json_user = WebUtils.GET_WebRequestHTML("utf-8", url_user.ToString());

            if (String.IsNullOrEmpty(json_user))
            {
                return null;
            }
            return WebUtils.JsonToObj<WeChatUserInfo>(json_user, null);

        }

        /// <summary>
        /// 获取带是否关注标识的用户信息
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        private WeChatUserInfo GetSignUserInfo(String openId)
        {
            if (String.IsNullOrEmpty(openId))
            {
                return null;
            }
            var token = GetBaseToken();
            var url = String.Format("https://api.weixin.qq.com/cgi-bin/user/info?access_token={0}&openid={1}", token, openId);
            var json = WebUtils.GET_WebRequestHTML("utf-8", url);
            if (String.IsNullOrEmpty(json))
            {
                return null;
            }
            var userInfo = WebUtils.JsonToObj<WeChatUserInfo>(json, null);
            return userInfo;
        }
        
        private string GetBaseToken()
        {
            string access_token = "";
            string getAccess_tokenUr = "https://www.bluemembers.com.cn/weixin/wechat/common/GetAccessToken?key=d317857f468c8972547df70da5b1ace5798058c6";
            var json_token = WebUtils.GET_WebRequestHTML("utf-8", getAccess_tokenUr);
            if (!string.IsNullOrEmpty(json_token))
            {
                try
                {
                    var str_token = WebUtils.JsonToObj<WechatAccess_Token>(json_token, null);
                    if (str_token!=null&& str_token.ret==1)
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

            /*
            var cookieName = "bluemember_WeChatBaseTokenHour";
            var baseToken = LocalCache.GetCache<TokenExpires>(cookieName);
            if (baseToken == null || string.IsNullOrEmpty(baseToken.Token) || baseToken.Expires < DateTime.Now)
            {   //为空，重新获取
                var url = string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", strAppid, strSecret);
                var json = WebUtils.GET_WebRequestHTML("utf-8", url);
                if (!string.IsNullOrEmpty(json))
                {
                    var wbt = WebUtils.JsonToObj<WeChatBaseToken>(json, null);
                    if (wbt != null && !String.IsNullOrEmpty(wbt.access_token))
                    {
                        DateTime _expires = DateTime.Now.AddMinutes(60);
                        baseToken = new TokenExpires() { Token = wbt.access_token, Expires = _expires };
                        LocalCache.PushCache<TokenExpires>(cookieName, baseToken, 60);
                    }
                }
            }
            return baseToken.Token;
             * */
        }

        #endregion

    }



    public class TokenExpires
    {
        public string Token { get; set; }
        public DateTime Expires { get; set; }
    }

    /// <summary>
    /// 本地缓存
    /// </summary>
    public static class LocalCache
    {
        private static System.Web.Caching.Cache webCache = HttpContext.Current == null ? null : HttpContext.Current.Cache;

        [DefaultValue(15)]
        public static int DefaultExpiredTime
        {
            get;
            set;
        }

        public static bool HasValue(string cacheKey)
        {
            if (webCache[cacheKey] != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsCachePool()
        {
            return webCache != null;
        }

        /// <summary>
        /// 从缓存中取数据
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        public static T GetCache<T>(string cacheKey)
        {
            var o = webCache.Get(cacheKey);
            return o != null ? (T)o : default(T);
        }

        public static void PushCache<T>(string cacheKey, T cachedata, int ExpiredTime)
        {
            if (cachedata == null)
            {
                RemoveCache(cacheKey);
            }
            else
            {
                webCache.Add(cacheKey, cachedata, null, DateTime.Now.AddMinutes(ExpiredTime), Cache.NoSlidingExpiration,
                             CacheItemPriority.Default, null);
            }
        }

        public static void PushCache<T>(string cacheKey, T cachedata, TimeSpan span)
        {
            if (cachedata == null)
            {
                RemoveCache(cacheKey);
            }
            else
            {
                webCache.Add(cacheKey, cachedata, null, DateTime.Now.Add(span), Cache.NoSlidingExpiration,
                             CacheItemPriority.Default, null);
            }
        }

        /// <summary>
        /// 加入缓存
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="cachedata"></param>
        public static void PushCache<T>(string cacheKey, T cachedata, System.Web.Caching.CacheDependency cacheDependency, DateTime expiration)
        {
            if (cachedata == null)
            {
                RemoveCache(cacheKey);
            }
            else
            {
                webCache.Add(cacheKey, cachedata, cacheDependency, expiration, Cache.NoSlidingExpiration,
                             CacheItemPriority.Default, null);
            }
        }

        /// <summary>
        /// 从缓存中移除一项
        /// </summary>
        /// <param name="cacheKey"></param>
        public static void RemoveCache(string cacheKey)
        {
            if (webCache[cacheKey] != null)
            {
                webCache.Remove(cacheKey);
            }
        }
    }
}