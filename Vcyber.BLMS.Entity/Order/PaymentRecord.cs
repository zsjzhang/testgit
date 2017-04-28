using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    public class PaymentRecord
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 微信分配的公众账号ID（企业号corpid即为此appId）
        /// </summary>
        public string AppId { get; set; }
        /// <summary>
        /// 此参数为微信用户在商户对应appid下的唯一标识。
        /// </summary>
        public string OpenId { get; set; }
        /// <summary>
        /// 微信支付分配的商户号
        /// </summary>
        public string MchId { get; set; }
        /// <summary>
        /// 经销商编号
        /// </summary>
        public string DealerCode { get; set; }
        /// <summary>
        /// 付款状态
        /// </summary>
        public string State { get; set; }
        /// <summary>
        /// 交易类型
        /// </summary>
        public string TradeType { get; set; }
        /// <summary>
        /// 终端IP
        /// </summary>
        public string SpbillCreateIp { get; set; }
        /// <summary>
        /// 标价币种默认人民币：CNY
        /// </summary>
        public string FeeType { get; set; }
        /// <summary>
        /// 标价金额
        /// </summary>
        public int TotalFee { get; set; }
        /// <summary>
        /// 商户订单号
        /// </summary>
        public string OutTradeNo { get; set; }
        /// <summary>
        /// 附加数据
        /// </summary>
        public string Attach { get; set; }
        /// <summary>
        /// 商品详情
        /// </summary>
        public string Detail { get; set; }
        /// <summary>
        /// 商品描述
        /// </summary>
        public string Body { get; set; }
        /// <summary>
        /// 错误代码描述
        /// </summary>
        public string ErrCodeDes { get; set; }
        /// <summary>
        /// 错误代码
        /// </summary>
        public string ErrCode { get; set; }
        /// <summary>
        /// 业务结果
        /// </summary>
        public string ResultCode { get; set; }
        /// <summary>
        /// 返回信息
        /// </summary>
        public string ReturnMsg { get; set; }
        /// <summary>
        /// 返回状态码
        /// </summary>
        public string ReturnCode { get; set; }
        /// <summary>
        /// 是否关注公众账号
        /// </summary>
        public string IsSubscribe { get; set; }
        /// <summary>
        /// 付款银行
        /// </summary>
        public string BankType { get; set; }
        /// <summary>
        /// 应结订单金额
        /// </summary>
        public int SettlemenTotalFee { get; set; }
        /// <summary>
        /// 现金支付金额
        /// </summary>
        public int CashFee { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 支付完成时间
        /// </summary>
        public string TimeEnd { get; set; }
        /// <summary>
        /// 微信支付订单号
        /// </summary>
        public string TransactionId { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId { get; set; }
    }
}
