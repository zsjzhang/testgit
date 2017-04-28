using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Senparc.Weixin.MP.CommonAPIs;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Security.Cryptography;
using Vcyber.BLMS.Common;

namespace Vcyber.BLMS.Repository
{
    public class WXCardRepository
    {
        public bool SendWXCard(SendCard cardModel)
        {
            TimeSpan ts = DateTime.Now - DateTime.Parse("1970-01-01 00:00:00");
            var tsStr = ts.Ticks.ToString();

            cardModel.wxcard.card_ext.timestamp = tsStr;
            cardModel.wxcard.card_ext.signature = HBCommon.GetSHA1(tsStr);

            var token = WeChatHelper.GetAccessToken();

            var responseStr = POST_HttpWebRequestHTML("UTF-8", string.Format("https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token={0}", token), JsonConvert.SerializeObject(cardModel));

            ReturnObj re = JsonConvert.DeserializeObject<ReturnObj>(responseStr);
            if (re != null && re.errcode == 0)
            {
                return true;
            }
            else
                return false;
        }

        public bool ValideWxCardCode(string cardid, string code, out string openid, out string error)
        {
            error = "";
            openid = "";
            var token = WeChatHelper.GetAccessToken();
            WxCardValidePara para = new WxCardValidePara { code = code, card_id = cardid, check_consume = true };
            var resultValue = POST_HttpWebRequestHTML("UTF-8", string.Format("https://api.weixin.qq.com/card/code/get?access_token={0}", token), JsonConvert.SerializeObject(para));
            WxCardValidateResultModel validResult = JsonConvert.DeserializeObject<WxCardValidateResultModel>(resultValue);
            if (validResult == null)
            {
                LogService.Instance.Error("后台验证：卡券Code无效: 方法名：ValidateCardCode；验证CODE结果转换失败，返回字符串：" + resultValue);
                return false;
            }
            else if (validResult.errcode != 0)
            {
                LogService.Instance.Info("后台验证：方法名：ValidateCardCode；验证CODE结果转换失败，返回字符串：" + resultValue);
                openid = validResult.openid;
                return false;
            }
            else
            {
                error = validResult.card.can_consume ? "可以核销" : "不可以核销";
                openid = validResult.openid;
                return true;
            }
        }

        public bool WxCardHX(string code, out string openid, out string cardId, out string error)
        {
            openid = "";
            cardId = "";
            error = "";
            bool result = false;
            var _msg = "HXWXCard--";
            var token = WeChatHelper.GetAccessToken();
            HXCardInfo hxCard = new HXCardInfo() { code = code };
            string resultValue = POST_HttpWebRequestHTML("UTF-8", string.Format("https://api.weixin.qq.com/card/code/consume?access_token={0}", token), JsonConvert.SerializeObject(hxCard));
            var model = JsonConvert.DeserializeObject<CardHXReturn>(resultValue);
            if (model == null)
            {   //转换出错
                _msg += "；转换出错，resultValue=" + resultValue;
            }
            else if (model != null && model.errcode == 0)
            {   //成功
                result = true;
                openid = model.openid;
                cardId = model.card.card_id;
            }
            else
            {   //核销失败
                _msg += "；转换出错，resultValue=" + resultValue;
                error = model != null ? model.errmsg : "";
            }
            return result;
        }

        /// <summary>
        /// 将网址类容转换成文本字符串 post请求
        /// </summary>
        /// <param name="data">要post的数据</param>
        /// <param name="url">目标url</param>
        /// <returns>服务器响应</returns>
        public string POST_HttpWebRequestHTML(string encodingName,
                                                      string htmlUrl,
                                                      string postData)
        {
            string html = string.Empty;

            try
            {
                Encoding encoding = Encoding.GetEncoding(encodingName);

                byte[] bytesToPost = encoding.GetBytes(postData);

                WebRequest webRequest = WebRequest.Create(htmlUrl);
                HttpWebRequest httpRequest = webRequest as System.Net.HttpWebRequest;

                httpRequest.Method = "POST";
                httpRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.2; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
                //httpRequest.ContentType = "application/json";
                httpRequest.ContentType = "text/xml";
                httpRequest.ContentLength = bytesToPost.Length;
                httpRequest.Timeout = 15000;
                httpRequest.ReadWriteTimeout = 15000;
                Stream requestStream = httpRequest.GetRequestStream();
                requestStream.Write(bytesToPost, 0, bytesToPost.Length);
                requestStream.Close();

                HttpWebResponse httpWebResponse = (HttpWebResponse)httpRequest.GetResponse();
                Stream responseStream = httpWebResponse.GetResponseStream();
                StreamReader streamReader = new StreamReader(responseStream, encoding);

                html = streamReader.ReadToEnd();
            }
            catch (WebException ex)
            {
                throw new Exception(ex.Message);
            }

            return html;
        }

        public string GetPhoneNumber(string openid_new)
        {
            string sql = @"
select wx.Tel from WXBind wx
where wx.OpenId=@OpenId 
";
            return DbHelp.ExecuteScalar<string>(sql, new { @OpenId = openid_new });
        }
    }

    public class ReturnObj
    {
        public int errcode { get; set; }

        public string errmsg { get; set; }

    }

    public class WeChatHelper
    {
        static WeChatHelper()
        {
            RegistWxApp();
        }

        private static void RegistWxApp()
        {
            try
            {
                var appid = ConfigurationManager.AppSettings["WeChatAppID"]; //"wx8ba75eb0ebbb764b";//
                var weChatAppSecret = ConfigurationManager.AppSettings["WeChatAppSecret"];
                if (!AccessTokenContainer.CheckRegistered(appid))
                {
                    AccessTokenContainer.Register(appid, weChatAppSecret);
                }
            }
            catch (Exception ex)
            {
                //Loger.Error(string.Format("注册全局app失败，请注意web.config是否正确，需要配置项WeChatAppID和WeChatAppSecret，详细错误{0}", ex));
            }
        }

        /// <summary>
        /// 获取Access_token
        /// </summary>
        /// <returns></returns>
        public static String GetAccessToken()
        {
            var token = "";
            string appid = ConfigurationManager.AppSettings["WeChatAppID"]; //"wx8ba75eb0ebbb764b";//
            //string weChatAppSecret = ConfigurationManager.AppSettings["WeChatAppSecret"]; 

            for (int i = 0; i < 3; i++)
            {
                try
                {
                    var accessTokenResult = AccessTokenContainer.GetTokenResult(appid);
                    var menu = Senparc.Weixin.MP.CommonAPIs.CommonApi.GetMenu(accessTokenResult.access_token);
                    if (menu != null && menu.menu != null && menu.menu.button != null && menu.menu.button.Count > 0)
                    {
                        token = accessTokenResult.access_token;
                    }
                    else
                    {
                        var temp = AccessTokenContainer.GetTokenResult(appid, true);
                        token = temp.access_token;
                    }

                    break;
                }
                catch (Exception ex)
                {
                    if (i >= 2)
                    {
                        RegistWxApp();
                    }

                    //token = AccessTokenContainer.TryGetToken(appid, weChatAppSecret);
                    var temp = AccessTokenContainer.GetTokenResult(appid, true);
                    token = temp.access_token;
                }
            }

            return token;
        }
    }

    /// <summary>
    /// 抢红包相关方法
    /// </summary>
    public static class HBCommon
    {
        private static char[] constant =   
      {   
        '0','1','2','3','4','5','6','7','8','9',  
        'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z',   
        'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z'   
      };

        private static char[] constantNumber = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

        /// <summary>
        /// 获取指定长度的随机数（由数字，大小写字母组成）
        /// </summary>
        /// <param name="Length">随机数长度</param>
        /// <returns></returns>
        public static string GetRandomStr(int Length)
        {
            System.Text.StringBuilder newRandom = new System.Text.StringBuilder();
            Random rd = new Random();
            for (int i = 0; i < Length; i++)
            {
                newRandom.Append(constant[rd.Next(62)]);
            }
            return newRandom.ToString();
        }

        /// <summary>
        /// 随机获取指定长度的一串数字的字符串
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static String GetRandomNumber(int length)
        {
            System.Text.StringBuilder newRandom = new System.Text.StringBuilder();
            Random rd = new Random();
            for (int i = 0; i < length; i++)
            {
                newRandom.Append(constantNumber[rd.Next(10)]);
            }
            return newRandom.ToString();
        }

        /// <summary>
        /// MD5
        /// </summary>
        /// <param name="encypStr"></param>
        /// <param name="charset"></param>
        /// <returns></returns>
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

        /// <summary>
        /// SHA1加密
        /// </summary>
        /// <param name="encypStr"></param>
        /// <returns></returns>
        public static string GetSHA1(string encypStr)
        {
            return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(encypStr, "SHA1");
        }
    }

    /// <summary>
    /// 卡券核销 结果
    /// </summary>
    public class CardHXReturn
    {
        public long errcode { get; set; }

        public string errmsg { get; set; }

        public string openid { get; set; }

        public Card card { get; set; }
    }

    public class Card
    {
        public string card_id { get; set; }
    }

    public class HXCardInfo
    {
        public string code { get; set; }
    }

    public class WxCardValidePara
    {
        public string card_id { get; set; }

        public string code { get; set; }

        public bool check_consume { get; set; }

    }

    public class WxCardValidateResultModel
    {
        public int errcode { get; set; }

        public string errmsg { get; set; }

        public WxCardValidate_Card card { get; set; }

        public string openid { get; set; }

    }
    public class WxCardValidate_Card
    {
        public string card_id { get; set; }

        public long begin_time { get; set; }

        public long end_time { get; set; }

        public bool can_consume { get; set; }

        public string user_card_status { get; set; }
    }

    public class SendCard
    {
        public string touser { get; set; }

        /// <summary>
        /// wxcard
        /// </summary>
        public string msgtype { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public WXCard wxcard { get; set; }
    }

    public class WXCard
    {
        public string card_id { get; set; }

        public Card_ext card_ext { get; set; }
    }

    public class Card_ext
    {
        public string code { get; set; }

        public string openid { get; set; }

        public string timestamp { get; set; }

        public string signature { get; set; }
    }
}
