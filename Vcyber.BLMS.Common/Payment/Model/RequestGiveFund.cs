
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vcyber.BLMS.Common.Payment.Model.Request
{
    public class RequestGiveFund
    {
        private string _nonce_str = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 30);          
        /// <summary>
        /// 公众账号appid	
        /// </summary>
        public string mch_appid { get; set; }
        /// <summary>
        /// 商户号	
        /// </summary>
        public string mchid { get; set; }
        /// <summary>
        /// 随机字符串	
        /// </summary>
        public string nonce_str 
        {
            get { return _nonce_str; }
            set { _nonce_str = value; }
        }
        /// <summary>
        /// 签名
        /// </summary>
        public string sign { get; set; }
        /// <summary>
        /// 商户订单号	
        /// </summary>
        public string partner_trade_no { get; set; }
        /// <summary>
        /// 用户openid	
        /// </summary>
        public string openid { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public int amount { get; set; }
        /// <summary>
        /// 企业付款描述信息	
        /// </summary>
        public string desc { get; set; }
        /// <summary>
        /// Ip地址	
        /// </summary>
        public string spbill_create_ip { get; set; }
        /// <summary>
        /// 校验用户姓名选项NO_CHECK：不校验真实姓名 
        /// </summary>
        public string check_name { get; set; }
    }

    public class xml : RequestGiveFund 
    {
        
    }
}