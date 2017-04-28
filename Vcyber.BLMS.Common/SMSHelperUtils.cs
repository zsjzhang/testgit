using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Vcyber.BLMS.Common
{
    public static class SMSHelperUtils
    {
        public static string SendSMS(SmsSendMessage sms)
        {
            //判断所需配置是否完整
            if (ConfigurationManager.AppSettings["SmsURL"] == null || ConfigurationManager.AppSettings["SmsUsr"] == null || ConfigurationManager.AppSettings["SmsPwd"] == null)
            {
                return null;
            }

            //取得入口地址和用户名、密码
            string SmsURL = ConfigurationManager.AppSettings["SmsURL"];
            string SmsUsr = ConfigurationManager.AppSettings["SmsUsr"];
            string SmsPwd = ConfigurationManager.AppSettings["SmsPwd"];

            //设定用户名、密码
            sms.usr = SmsUsr;
            sms.pwd = SmsPwd;

            //构建请求实体
            string postData = Web.WebUtils.ObjToJson<SmsSendMessage>(sms);

            SmsURL += "?usr=" + SmsUsr + "&pwd=" + SmsPwd + "&mobile=" + sms.mobile + "&msg=" + HttpUtility.UrlEncode(sms.msg,Encoding.GetEncoding("gbk"));

            //调用服务
            //var result = Web.WebUtils.POST_HttpWebRequestHTML("utf-8", SmsURL, postData, null);
            var result = Web.WebUtils.GET_WebRequestHTML("utf-8", SmsURL);

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

    public class SmsSendMessage
    {
        public string mobile { get; set; }

        public string msg { get; set; }

        public string usr { get; set; }

        public string pwd { get; set; }
    }
}
