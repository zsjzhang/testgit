using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Common.Payment.Model.ReponseCreate
{
    public class ResponseCreate
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
        /// 公众账号ID
        /// </summary>
        public string appid { get; set; }
        /// <summary>
        /// 商户号	
        /// </summary>
        public string mch_id { get; set; }
        /// <summary>
        /// 设备号	
        /// </summary>
        public string device_info { get; set; }
        /// <summary>
        /// 随机字符串	
        /// </summary>
        public string nonce_str { get; set; }
        /// <summary>
        /// 签名	
        /// </summary>
        public string sign { get; set; }
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
        /// 交易类型	
        /// </summary>
        public string trade_type { get; set; }
        /// <summary>
        /// 预支付交易会话标识	
        /// </summary>
        public string prepay_id { get; set; }
        /// <summary>
        /// 二维码链接	
        /// </summary>        
        public string code_url { get; set; }
        /// <summary>
        /// 密钥
        /// </summary>
        public string key { get; set; }
        /// <summary>
        /// 商家的自定义单号
        /// </summary>
        public string out_trade_no { get; set; }
    }
    public class xml : ResponseCreate { }
}
