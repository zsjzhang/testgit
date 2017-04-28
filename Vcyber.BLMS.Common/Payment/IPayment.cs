using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Common.Payment.Model;
using Vcyber.BLMS.Common.Payment.Model.ReponseCreate;
using Vcyber.BLMS.Common.Payment.Model.Request;
using Vcyber.BLMS.Common.Payment.Model.Response;
using Vcyber.BLMS.Common.Payment.Model.ResponseNotify;
using Vcyber.BLMS.Common.Payment.Model.ResponseQuery;
using Vcyber.BLMS.Common.Payment.Model.ResponseRefund;
namespace Vcyber.BLMS.Common.Payment
{
    /// <summary>
    /// 支付接口
    /// </summary>
    public interface IPayment
    {
        /// <summary>
        /// 支付方法
        /// </summary>
        /// <param name="obj">
        /// obj.TotalFee:金额,obj.ReturnUrl:返回地址,obj.Partner:商户号,obj.OutTradeNo:单号,obj.NotifyUrl:通知URL,obj.Attach:附件,obj.Body:支付内容
        /// </param>
        ResponseCreate Create(RequestCreate obj);
        /// <summary>
        /// 退款
        /// 必填：商户号，财付通订单号，商户退款单号，总金额，退款金额，操作员，操作员密码
        /// </summary>
        ResponseRefund Refund(RequestRefund obj);
        /// <summary>
        /// 企业付款
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        ResponseGiveFund GiveFund(RequestGiveFund obj);

        /// <summary>
        /// 通知
        /// </summary>
        /// <param name="content">通知内容</param>
        /// <returns>通知实体</returns>
        ResponseNotify Notify(string content);
        /// <summary>
        /// 查询订单
        /// </summary>
        /// <param name="obj">查询条件</param>
        /// <returns>订单信息</returns>
        ResponseQuery Query(RequestQuery obj);
    }
}
