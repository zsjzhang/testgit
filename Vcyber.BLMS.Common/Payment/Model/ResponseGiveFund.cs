using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vcyber.BLMS.Common.Payment.Model.Response
{
    public class ResponseGiveFund
    {
        /// <summary>
        /// 返回状态码	
        /// </summary>
        public string return_code { get; set; }
        /// <summary>
        /// 返回信息	
        /// </summary>
        public string return_msg { get; set; }
        /// <summary>
        /// 商户appid	
        /// </summary>
        public string mch_appid { get; set; }
        /// <summary>
        /// 商户号	
        /// </summary>
        public string mchid { get; set; }
        /// <summary>
        /// 设备号	
        /// </summary>
        public string device_info { get; set; }
        /// <summary>
        /// 随机字符串	
        /// </summary>
        public string nonce_str { get; set; }
        /// <summary>
        /// 业务结果	
        /// </summary>
        public string result_code { get; set; }
        /// <summary>
        /// 错误代码	
        /// </summary>
        public string err_code { get; set; }
        /// <summary>
        /// 错误代码描述	
        /// </summary>
        public string err_code_des { get; set; }
        /// <summary>
        /// 商户订单号	
        /// </summary>
        public string partner_trade_no { get; set; }
        /// <summary>
        /// 微信订单号	
        /// </summary>
        public string payment_no { get; set; }
        /// <summary>
        /// 微信支付成功时间	
        /// </summary>
        public string payment_time { get; set; }
    }
    public class xml : ResponseGiveFund { }
}