using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Common.Payment.Model.ResponseRefund
{
    public class ResponseRefund : ResponseBase
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

        public string transaction_id { get; set; }
        public string out_trade_no { get; set; }
        public string out_refund_no { get; set; }
        public string refund_id { get; set; }
        public string refund_channel { get; set; }        
        public int refund_fee { get; set; }        
        public int settlement_refund_fee { get; set; }       
        public int total_fee { get; set; }       
        public int settlement_total_fee { get; set; }       
        public string fee_type { get; set; }       
        public int cash_fee { get; set; }       
        public int cash_refund_fee { get; set; }        
    }
    public class xml : ResponseRefund { }
}
