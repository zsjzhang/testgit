using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tenpay;
using Vcyber.BLMS.Common.Payment.Model;
using Vcyber.BLMS.Common.Payment.Model.ReponseCreate;
using Vcyber.BLMS.Common.Payment.Model.Request;
using Vcyber.BLMS.Common.Payment.Model.Response;
using Vcyber.BLMS.Common.Payment.Model.ResponseNotify;
using Vcyber.BLMS.Common.Payment.Model.ResponseRefund;
namespace Vcyber.BLMS.Common.Payment
{
    /// <summary>
    /// 财付通支付类
    /// </summary>
    public class PaymentForTenwap : IPayment
    {
        /// <summary>
        /// 支付方法
        /// </summary>
        /// <param name="obj">
        /// obj.TotalFee:金额,obj.ReturnUrl:返回地址,obj.Partner:商户号,obj.OutTradeNo:单号,obj.NotifyUrl:通知URL,obj.Attach:附件,obj.Body:支付内容
        /// </param>
        public ResponseCreate Create(RequestCreate obj)
        {
            /* 商户号，上线时务必将测试商户号替换为正式商户号 */
            string partner = obj.MchId;
            //创建RequestHandler实例
            RequestHandler reqHandler = new RequestHandler(System.Web.HttpContext.Current);
            //初始化
            reqHandler.init();
            //设置密钥
            reqHandler.setKey(obj.Sign);
            reqHandler.setGateUrl("https://gw.tenpay.com/gateway/pay.htm");
            //-----------------------------
            //设置支付参数
            //-----------------------------
            reqHandler.setParameter("total_fee", obj.TotalFee.ToString());
            //用户的公网ip,测试时填写127.0.0.1,只能支持10分以下交易
            reqHandler.setParameter("spbill_create_ip", obj.SpbillCreateIp);
            reqHandler.setParameter("return_url", obj.NotifyUrl);
            reqHandler.setParameter("partner", obj.MchId);
            reqHandler.setParameter("out_trade_no", obj.OutTradeNo);
            reqHandler.setParameter("notify_url", obj.NotifyUrl);
            reqHandler.setParameter("attach", obj.Attach);
            reqHandler.setParameter("body", obj.Body);
            reqHandler.setParameter("bank_type", "");
            //系统可选参数
            reqHandler.setParameter("sign_type", obj.SignType);
            reqHandler.setParameter("service_version", obj.ServiceVersion);
            reqHandler.setParameter("input_charset", obj.InputCharset);
            reqHandler.setParameter("sign_key_index", obj.SignKeyIndex.ToString());
            //获取请求带参数的url
            string requestUrl = reqHandler.getRequestURL();
            return new ResponseCreate() { result_code = requestUrl };
        }

        public ResponseRefund Refund(RequestRefund obj)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 企业付款
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public ResponseGiveFund GiveFund(RequestGiveFund obj) 
        {
            throw new NotImplementedException();
        }


        public ResponseNotify Notify(string content)
        {
            throw new NotImplementedException();
        }

        public Model.ResponseQuery.ResponseQuery Query(RequestQuery obj)
        {
            throw new NotImplementedException();
        }
    }
}
