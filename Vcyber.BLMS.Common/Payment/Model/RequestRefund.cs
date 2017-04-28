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
    /// 退款请求Model
    /// </summary>
    public class RequestRefund : RequestBase
    {
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
        [DataMember(Name = "out_trade_no")]
        public string OutTradeNo { get; set; }
        [DataMember(Name = "out_refund_no")]
        public string OutRefundNo { get; set; }
        [DataMember(Name = "total_fee")]
        public int TotalFee { get; set; }
        [DataMember(Name = "refund_fee")]
        public int RefundFee { get; set; }
        [DataMember(Name = "refund_fee_type")]
        public string RefundFeeType { get; set; }
        [DataMember(Name = "op_user_id")]
        public string OpUserId { get; set; }
        [DataMember(Name = "refund_account")]
        public string RefundAccount { get; set; }        
    }
}
