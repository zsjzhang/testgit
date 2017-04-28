using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Common.Payment.Model
{
    [DataContract(Name = "xml")]
    public class RequestQuery
    {
        [DataMember(Name = "appid")]
        public string AppId { get; set; }
        [DataMember(Name = "mch_id")]
        public string MchId { get; set; }
        [DataMember(Name = "out_trade_no")]
        public string OutTradeNo { get; set; }
        [DataMember(Name = "nonce_str")]
        public string NonceStr { get; set; }
        [DataMember(Name = "sign")]
        public string Sign { get; set; }
        [DataMember(Name = "sign_type")]
        public string SignType { get; set; }
    }
}
