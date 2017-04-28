using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Vcyber.BLMS.FrontWeb.Controllers.BlowCar
{
    public class IdentityAuthController : Controller
    {
        private string AppSecret = "db00357b1a0cff6494aedea9a081a69b";
        private string AppId = "wx8ba75eb0ebbb764b";
        //
        // GET: /IdentityAuth/
        public ActionResult Auth(string url)
        {
            //第一步，获取token
            WeiXinAccessToken tokenTool = new WeiXinAccessToken(AppId);
            string _access_token = tokenTool.get_token(); 
            //第二步：获取ticket
            WeiXinTicket ticketTool = new WeiXinTicket();
            string _ticket = ticketTool.get_jsapi_ticket(_access_token);
            //第三步：获取signature
            string _timestamp = string.Empty;
            string _noncestr = string.Empty;
            WeiXinSignature _signatureTool = new WeiXinSignature();
            string _signature = _signatureTool.Get(url, _ticket, out _noncestr, out _timestamp);
            Response.AddHeader("Access-Control-Allow-Origin", "*");
            return Json(new { appId = AppId, timestamp = _timestamp, nonceStr = _noncestr, signature = _signature }, JsonRequestBehavior.AllowGet);
        }
    }

    public class WeiXinMaster
    {
        public int errcode { get; set; }
        public string errmsg { get; set; }
        public string expires_in { get; set; }
    }

    public class WeiXinAccessToken : WeiXinMaster
    {
        public string AppId { get; set; }
        public string access_token { get; set; }
        public string AppSecret
        {
            get
            {
                return "db00357b1a0cff6494aedea9a081a69b";
            }
        }
        //微信接口TOKEN请求URL
        private string TOKENURL = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}";

        public WeiXinAccessToken()
        {

        }

        public WeiXinAccessToken(string appid)
        {
            this.AppId = appid;
        }

        private string get_token(string appid, string appsecret)
        {
            string _get_token_url = string.Format(TOKENURL, appid, appsecret);
            string _result_str = HttpTool.HttpGet(_get_token_url, string.Empty);
            WeiXinAccessToken _result = JsonConvert.DeserializeObject<WeiXinAccessToken>(_result_str);
            if (_result != null && !string.IsNullOrEmpty(_result.access_token))
            {
                return _result.access_token;
            }
            return string.Empty;
        }

        public string get_token()
        {
            if (HttpContext.Current.Cache.Get("accesstoken") == null)
            {
                HttpContext.Current.Cache.Add("accesstoken", this.get_token(this.AppId, this.AppSecret), null, DateTime.Now.AddHours(2), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Normal, null);
            }
            return HttpContext.Current.Cache["accesstoken"].ToString();
        }
    }

    public class WeiXinTicket : WeiXinMaster
    {
        private string TICKETURL = "https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token={0}&type=jsapi";
        public string ticket { get; set; }
        public string get_jsapi_ticket(string token)
        {
            if (HttpContext.Current.Cache.Get("jsapiticket") == null)
            {
                HttpContext.Current.Cache.Add("jsapiticket", this.get_jsapi_ticket(token, null), null, DateTime.Now.AddHours(2), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Normal, null);
            }
            return HttpContext.Current.Cache["jsapiticket"].ToString();
        }

        private string get_jsapi_ticket(string token, params string[] param)
        {
            string _get_ticket_url = string.Format(TICKETURL, token);
            string _result_src = HttpTool.HttpGet(_get_ticket_url, string.Empty);
            WeiXinTicket _result = JsonConvert.DeserializeObject<WeiXinTicket>(_result_src);
            if (_result != null && _result.errcode == 0)
            {
                return _result.ticket;
            }
            return string.Empty;
        }
    }

    public class WeiXinSignature
    {

        public string Get(string url, string jsapi_ticket, out string noncestr, out string timestamp)
        {
            noncestr = WeiXinSignature.GetNoncestr();
            timestamp = WeiXinSignature.GetTimeStamp();

            SortedDictionary<string, object> sdcollection = new SortedDictionary<string, object>();
            sdcollection.Add("noncestr", noncestr);
            sdcollection.Add("jsapi_ticket", jsapi_ticket);
            sdcollection.Add("timestamp", timestamp);
            sdcollection.Add("url", url);

            StringBuilder builder = new StringBuilder();

            foreach (var keyName in sdcollection.Keys)
            {
                builder.Append(keyName + "=" + sdcollection[keyName].ToString()).Append("&");
            }
            string _result = builder.ToString();
            _result = _result.Substring(0, _result.Length - 1);
            return SecurityTool.GetMD5(_result, "UTF-8").ToUpper();
        }

        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <returns></returns>
        public static string GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }

        public static string GetNoncestr()
        {
            return "BLMS";
        }
    }

    public class HttpTool
    {
        public static string HttpPost(string Url, string postDataStr)
        {
            CookieContainer cookie = new CookieContainer();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = Encoding.UTF8.GetByteCount(postDataStr);
            request.CookieContainer = cookie;
            Stream myRequestStream = request.GetRequestStream();
            StreamWriter myStreamWriter = new StreamWriter(myRequestStream, Encoding.GetEncoding("gb2312"));
            myStreamWriter.Write(postDataStr);
            myStreamWriter.Close();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            response.Cookies = cookie.GetCookies(response.ResponseUri);
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }

        public static string HttpGet(string Url, string postDataStr)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url + (postDataStr == "" ? "" : "?") + postDataStr);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();
            return retString;
        }
    }

    public class SecurityTool
    {
        public static string GetMD5(string encypStr, string charset)
        {
            string retStr;
            MD5CryptoServiceProvider m5 = new MD5CryptoServiceProvider();

            //创建md5对象
            byte[] inputBye;
            byte[] outputBye;

            //使用GB2312编码方式把字符串转化为字节数组．
            try
            {
                inputBye = Encoding.GetEncoding(charset).GetBytes(encypStr);
            }
            catch (Exception ex)
            {
                inputBye = Encoding.GetEncoding("GB2312").GetBytes(encypStr);
            }
            outputBye = m5.ComputeHash(inputBye);

            retStr = System.BitConverter.ToString(outputBye);
            retStr = retStr.Replace("-", "").ToUpper();
            return retStr;
        }
    }
}