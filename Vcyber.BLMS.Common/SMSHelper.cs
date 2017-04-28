using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Common
{
    public static class SMSHelper
    {
        public static SmsResponseMessageDTO SendSMS(SmsSendMessageDTO sms)
        {
            if (ConfigurationManager.AppSettings["SmsAppkey"] == null || ConfigurationManager.AppSettings["SmsToken"] == null)
            {
                return null;
            }

            //取得Appid和Token
            string appid = ConfigurationManager.AppSettings["SmsAppkey"];
            string token = ConfigurationManager.AppSettings["SmsToken"];

            //获取时间戳
            string timestamp = DateTime.Now.ToFileTime().ToString();

            //生成加密签名
            string sigunature = EncryptionLib.SignatureBuilder.BuildSignature(appid, token, timestamp);

            //请求地址
            string url = ConfigurationManager.AppSettings["SmsSend"].ToString();

            //添加头部信息
            Dictionary<string, string> heads = new Dictionary<string, string>();
            heads.Add("timestamp", timestamp);
            heads.Add("appid", appid);
            heads.Add("signature", sigunature);

            //构建请求实体
            string postData = Web.WebUtils.ObjToJson<SmsSendMessageDTO>(sms);

            //调用服务
            var result = Web.WebUtils.JsonToObj<SmsResponseMessageDTO>(Web.WebUtils.POST_HttpWebRequestHTML("utf-8", url, postData, heads), null);

            return result;
        }

        /// <summary>
        /// 获取指定位数的数字验证码
        /// </summary>
        /// <param name="number">位数</param>
        /// <returns>验证码</returns>
        public static string GetValidateCode(int number)
        {
            Random dom = new Random();

            string code = "";

            for (int i = 0; i < number; i++)
            {
                code += dom.Next(0, 9);
            }

            return code;
        }
    }

    public class SmsSendMessageDTO
    {
        public string Mobile { get; set; }

        public string SmsContent { get; set; }

        public SmsType Type { get; set; }
    }

    public class SmsResponseMessageDTO
    {
        public SmsSendMessageDTO Content { get; set; }

        public string Code { get; set; }

        public string Message { get; set; }
    }

    public enum SmsType
    {
        Captcha = 1,
        RemindSms = 2,
        CommandSms = 3
    }
}
