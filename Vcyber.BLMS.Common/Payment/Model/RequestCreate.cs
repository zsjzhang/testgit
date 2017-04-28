using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Common.Payment.Model
{
    [DataContract(Name="xml")]
    /// <summary>
    /// 支付请求类
    /// </summary>
    public class RequestCreate : RequestBase
    {
        /// <summary>
        /// 生成财付通的请求参数
        /// </summary>
        /// <param name="partner">商户号</param>
        /// <param name="key">密钥</param>
        /// <param name="outTradeNo">商户订单号</param>
        /// <param name="body">商品描述</param>
        /// <param name="attach">附加数据，原样返回</param>
        /// <param name="returnUrl">交易完成后跳转的URL，需给绝对路径</param>
        /// <param name="notifyUrl">接收财付通通知的URL，需给绝对路径</param>
        /// <param name="totalFee">订单金额</param>
        /// <param name="transportFee">运费</param>
        /// <param name="productFee">商品金额</param>
        /// <param name="spbillCreateIp">创建IP</param>
        public RequestCreate(string mchId, string sign, string outTradeNo, string body, string attach, string notifyUrl, decimal totalFee, string spbillCreateIp) 
        {
            this.MchId = mchId;
            this.Sign = sign;
            this.OutTradeNo = outTradeNo;
            this.Body = body;
            this.Attach = attach;            
            this.NotifyUrl = notifyUrl;
            this.TotalFee = (int)(totalFee * 100);                        
            this.SpbillCreateIp = spbillCreateIp;            
        }
        /// <summary>
        /// 生成天猫的请求参数
        /// </summary>
        /// <param name="totalFee">订单金额</param>
        public RequestCreate(int totalFee)
        {
            this.TotalFee = totalFee;
        }
        public RequestCreate()
        {
        }
        [DataMember(Name = "appid")]
        public string AppId { get; set; }
        [DataMember(Name = "mch_id")]
        public string MchId { get; set; }
        [DataMember(Name = "device_info")]
        public string DeviceInfo { get; set; }
        [DataMember(Name = "nonce_str")]
        public string NonceStr { get; set; }
        [DataMember(Name = "sign")]
        public string Sign { get; set; }
        [DataMember(Name = "sign_type")]
        public string SignType { get; set; }
        [DataMember(Name = "body")]
        public string Body { get; set; }
        [DataMember(Name = "detail")]
        public string Detail { get; set; }
        [DataMember(Name = "attach")]
        public string Attach { get; set; }
        [DataMember(Name = "out_trade_no")]
        public string OutTradeNo { get; set; }
        [DataMember(Name = "fee_type")]
        public string FeeType { get; set; }
        /// <summary>
        /// 订单总金额，单位为分
        /// </summary>
        [DataMember(Name = "total_fee")]
        public int TotalFee { get; set; }
        [DataMember(Name = "spbill_create_ip")]
        public string SpbillCreateIp { get; set; }
        [DataMember(Name = "time_start")]
        public string TimeStart { get; set; }
        [DataMember(Name = "time_expire")]
        public string TimeExpire { get; set; }        
        public string GoodsTag { get; set; }
        [DataMember(Name = "notify_url")]
        public string NotifyUrl { get; set; }
        [DataMember(Name = "trade_type")]
        public string TradeType { get; set; }
        [DataMember(Name = "product_id")]
        public string ProductId { get; set; }        
        public string LimitPay { get; set; }
        [DataMember(Name = "openid")]
        public string OpenId { get; set; }
    }
}
