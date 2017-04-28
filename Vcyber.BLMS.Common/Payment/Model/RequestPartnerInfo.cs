using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Common.Payment.Model
{
    public class RequestPartnerInfo
    {
        /// <summary>
        /// 财付通PC商户号
        /// </summary>
        public static string TenCode
        {
            get 
            {
                return GetInfoByAppSetting("TenCode");
            }
        }
        /// <summary>
        /// 财付通PC商户key
        /// </summary>
        public static string TenKey
        {
            get
            {
                return GetInfoByAppSetting("TenKey");
            }
        }
        /// <summary>
        /// 财付通PC商户证书Path
        /// </summary>
        public static string TenCert
        {
            get
            {
                return GetInfoByAppSetting("TenCert");
            }
        }
        /// <summary>
        /// 返回的Url
        /// </summary>
        public static string ReturnUrlForWeb(PaymentType type)
        {
            var url = GetInfoByAppSetting("ReturnUrlForWeb");
            return string.Format("{0}?paymentType = {1}", url, type.ToString());
        }
        /// <summary>
        /// 回调的地址
        /// </summary>
        public static string NotifyUrl(PaymentType type)
        {
            var url = GetInfoByAppSetting("PaymentNotifyUrl");
            return string.Format("{0}?paymentType = {1}", url, type.ToString());
        }
        /// <summary>
        /// 获取appsetting信息
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>value</returns>
        private static string GetInfoByAppSetting(string key) 
        {
            var value = ConfigurationManager.AppSettings[key];
            if (value == null)
            {
                value = string.Empty;
            }
            return value;
        }
    }
}
