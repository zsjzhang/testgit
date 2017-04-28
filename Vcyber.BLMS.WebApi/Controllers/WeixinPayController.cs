using AspNet.Identity.SQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.Common.Payment;
using Vcyber.BLMS.Common.Payment.Model;
using Vcyber.BLMS.Domain;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Common;
using Vcyber.BLMS.Entity.Member;
using Vcyber.BLMS.Entity.Weixin;
using Vcyber.BLMS.Common.Payment.Model.Request;
using Vcyber.BLMS.Common.Payment.Model.Response;
using System.Web.Http.Filters;
using Vcyber.BLMS.Common.Payment.Model.ReponseCreate;
using Vcyber.BLMS.Common.Payment.Model.ResponseRefund;
using Vcyber.BLMS.Domain.Common;
using Vcyber.BLMS.Common.Payment.Model.ResponseQuery;

namespace Vcyber.BLMS.WebApi.Controllers
{
    /// <summary>
    /// 微信支付
    /// </summary>
    public class WeixinPayController : ApiController
    {
        /// <summary>
        /// 在线支付（微信）
        /// </summary>
        /// <param name="mch_id">微信支付分配的商户号</param>
        /// <param name="body">商品简单描述</param>
        /// <param name="detail">商品详细列表，使用Json格式，传输签名前请务必使用CDATA标签将JSON文本串保护起来。例如：<![CDATA[{"goods_detail":[{"goods_id":"x25保养套餐","goods_name":"x25保养套餐","quantity":1,"price":1}]}]]></param>
        /// <param name="attach">附加数据，在查询API和支付通知中原样返回，该字段主要用于商户携带订单的自定义数据</param>
        /// <param name="out_trade_no">商户系统内部的订单号,32个字符内、可包含字母</param>
        /// <param name="total_fee">订单总金额，单位为分</param>
        /// <param name="spbill_create_ip">APP和网页支付提交用户端ip，Native支付填调用微信支付API的机器IP。</param>        
        /// <param name="notify_url">接收微信支付异步通知回调地址，通知url必须为直接可访问的url，不能携带参数。</param>
        /// <param name="trade_type">取值如下：JSAPI，NATIVE，APP</param>        
        /// <param name="product_id">trade_type=NATIVE，此参数必传。此id为二维码中包含的商品ID，商户自行定义。</param>        
        /// <param name="openid">trade_type=JSAPI，此参数必传，用户在商户appid下的唯一标识。</param>
        /// <returns>返回下单的结果</returns>
        [HttpPost]
        [Route("api/weixinpay/create")]
        public ResponseCreate Create(string mch_id, string body, string detail, string attach, string out_trade_no, int total_fee, string spbill_create_ip, string notify_url, string trade_type, string product_id, string openid)
        {
            var rsp = new ResponseCreate();
            rsp.return_code = "FAIL";
            try
            {
                if (string.IsNullOrEmpty(mch_id))
                {
                    rsp.return_msg = "商品号不能为空";
                }
                else if (string.IsNullOrEmpty(body))
                {
                    rsp.return_msg = "商品简单描述不能为空";
                }
                else if (string.IsNullOrEmpty(detail))
                {
                    rsp.return_msg = "商品详情不能为空";
                }
                else if (string.IsNullOrEmpty(out_trade_no))
                {
                    rsp.return_msg = "订单号不能为空";
                }
                else if (string.IsNullOrEmpty(spbill_create_ip))
                {
                    rsp.return_msg = "IP地址不能为空";
                }
                else if (string.IsNullOrEmpty(notify_url))
                {
                    rsp.return_msg = "通知地址不能为空";
                }
                else if (string.IsNullOrEmpty(trade_type))
                {
                    rsp.return_msg = "交易类型不能为空";
                }
                else if (string.IsNullOrEmpty(openid))
                {
                    rsp.return_msg = "OpenId不能为空";
                }
                else
                {
                    var req = new RequestCreate();
                    req.MchId = mch_id;
                    req.Body = body;
                    req.Detail = detail;
                    req.Attach = string.IsNullOrEmpty(attach) ? out_trade_no : attach;
                    req.OutTradeNo = out_trade_no;
                    req.TotalFee = total_fee;
                    req.SpbillCreateIp = spbill_create_ip;
                    req.NotifyUrl = notify_url;
                    req.TradeType = trade_type;
                    req.OpenId = openid;
                    req.ProductId = string.IsNullOrEmpty(product_id) ? DateTime.Now.ToString("yyyyMMddhhmmss") : product_id;
                    IPayment pay = PaymentFactory.GetInstance(PaymentType.weixin);
                    rsp = pay.Create(req);
                }
            }
            catch (Exception ex)
            {
                rsp.return_msg = "系统内部错误";
                log4net.LogManager.GetLogger("weixin-pay-create").Error(ex);
            }
            return rsp;
        }
        /// <summary>
        /// 退款（微信）
        /// </summary>
        /// <param name="mch_id">商户号</param>
        /// <param name="out_trade_no">订单号</param>
        /// <param name="out_refund_no">退款单号</param>
        /// <param name="total_fee">总金额</param>
        /// <param name="refund_fee">退款金额</param>
        /// <param name="refund_account">退款来源渠道</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/weixinpay/refund")]
        public ResponseRefund Refund(string mch_id,string out_trade_no, string out_refund_no, int total_fee, int refund_fee, string refund_account)
        {
            var rsp = new ResponseRefund();
            rsp.return_code = "FAIL";
            try
            {
                if (string.IsNullOrEmpty(mch_id))
                {
                    rsp.return_msg = "商户号不能为空";
                }
                else if (string.IsNullOrEmpty(out_trade_no))
                {
                    rsp.return_msg = "订单号不能为空";
                }
                else if (string.IsNullOrEmpty(out_refund_no))
                {
                    rsp.return_msg = "退款单号不能为空";
                }
                else
                {
                    var req = new RequestRefund();
                    req.MchId = mch_id;
                    req.OutTradeNo = out_trade_no;
                    req.OutRefundNo = out_refund_no;
                    req.TotalFee = total_fee;
                    req.RefundFee = refund_fee;
                    req.RefundAccount = string.IsNullOrEmpty(refund_account) ? "REFUND_SOURCE_UNSETTLED_FUNDS" : refund_account;
                    IPayment pay = PaymentFactory.GetInstance(PaymentType.weixin);
                    rsp = pay.Refund(req);
                }
            }
            catch (Exception ex)
            {
                rsp.return_msg = "系统内部错误";
                log4net.LogManager.GetLogger("weixin-pay-refund").Error(ex);                
            }
            return rsp;
        }

        /// <summary>
        /// 缴费获取积分
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="openId">微信用户Id</param>
        /// <param name="dealerId">经销商编码</param>
        /// <param name="notifyUrl">回调地址</param>
        /// <param name="spbillCreateIp">客户端IP</param>
        /// <returns>支付的结果</returns>
        [HttpPost]
        [Route("api/weixinpay/fee")]
        public IHttpActionResult Fee(string userId, string openId, string dealerId, string notifyUrl, string spbillCreateIp)
        {
            var rsp = new ResponseCreate();            
            rsp.return_code = "FAIL";
            try
            {
                log4net.LogManager.GetLogger("weixin-pay-fee").Info(string.Format("userId:{0},openId:{1},dealerId:{2}", userId, openId, dealerId));
                if (string.IsNullOrEmpty(dealerId))
                {
                    rsp.return_msg = "商户号不能为空";
                }
                else if (string.IsNullOrEmpty(userId))
                {
                    rsp.return_msg = "用户ID不能为空";
                }
                else
                {
                    var req = new RequestCreate();
                    //获取用户信息
                    var userStore = new FrontUserStore<FrontIdentityUser>();
                    var user = userStore.FindByIdAsync(userId).Result;
                    //获取用户的缴费类型（50|100）
                    var returnIntegralType = (int)_AppContext.CarServiceUserApp.GetReIntegralTypeByIdentity(user.IdentityNumber);
                    var totalFee = 0;
                    if (returnIntegralType != -1)
                    {
                        totalFee = returnIntegralType > 1 ? 50 : 100;
                    }
                    totalFee = totalFee * 100;                    
                    //获取用户所选的经销商
                    var dealerObj = _AppContext.DealerApp.GetDealerByDealerId(dealerId);
                    req.MchId = dealerObj.WeixinPayAccount;
                    req.OpenId = openId;
                    //测试用，测试人员可一分钱购买-begin                 
                    //switch (req.OpenId)
                    //{                            
                    //    case "omXbSjnzWpUMERX9QkwmYLIc2dNg"://自己的微信号
                    //        totalFee = 1;
                    //        req.MchId = "1233041902";                                                             
                    //        break;
                    //    case "omXbSjnVLFct_-BRSH2vFsMfG_CY":
                    //        totalFee = 1;
                    //        req.MchId = "1233041902";//晓辉                                                        
                    //        break;
                    //    case "omXbSjuVNzcOOU2AG8kr8lfYL6Hk":
                    //        totalFee = 1;
                    //        req.MchId = "1233041902";//春霞                                                        
                    //        break;
                    //}
                    switch (req.OpenId)
                    {
                        case "oBvaJxLBqAvZSBZR9fA94l9L_Rac"://本地测试时加的默认微信号
                        case "oYnulw7gqtmmIVlqTCpYyrhp-P8k"://自己的微信号
                            totalFee = 1;
                            req.MchId = "1233041902";
                            req.OpenId = "omXbSjnzWpUMERX9QkwmYLIc2dNg";
                            break;
                        case "oYnulwzXUJWQA9V2OAUi25DVRXAk":
                            totalFee = 1;
                            req.MchId = "1233041902";
                            req.OpenId = "omXbSjnVLFct_-BRSH2vFsMfG_CY";//晓辉                                                        
                            break;
                        case "oYnulw7QMIAajM0KHa-hlo7Y0ajc":
                            totalFee = 1;
                            req.MchId = "1233041902";
                            req.OpenId = "omXbSjuVNzcOOU2AG8kr8lfYL6Hk";//春霞                                                        
                            break;
                    }
                    //--end                    
                    req.Body = string.Format("缴费获取积分-{0}", dealerObj.Name); ;
                    req.Detail = "<![CDATA[{\"goods_detail\":[{\"goods_id\":\"缴费获取积分\",\"goods_name\":\"缴费获取积分\",\"quantity\":1,\"price\":" + totalFee + "}]}]]>";
                    req.Attach = "北京现代汽车有限公司";
                    req.OutTradeNo = IdGenerator.GetId(SequenceCategory.JF);
                    req.TotalFee = totalFee;
                    req.SpbillCreateIp = spbillCreateIp;
                    req.NotifyUrl = notifyUrl;
                    req.TradeType = "JSAPI";                    
                    req.ProductId = "2017011601";
                    IPayment pay = PaymentFactory.GetInstance(PaymentType.weixin);
                    rsp = pay.Create(req);
                    rsp.out_trade_no = req.OutTradeNo;
                    rsp.mch_id = req.MchId;
                    //创建付款记录
                    PaymentRecord paymentRecord = new PaymentRecord();
                    paymentRecord.AppId = rsp.appid;
                    paymentRecord.OpenId = req.OpenId;
                    paymentRecord.MchId = req.MchId;
                    paymentRecord.UserId = userId;
                    paymentRecord.DealerCode = dealerId;
                    paymentRecord.State = "创建";
                    paymentRecord.TradeType = req.TradeType;
                    paymentRecord.SpbillCreateIp = spbillCreateIp;
                    paymentRecord.FeeType = "CNY";
                    paymentRecord.TotalFee = req.TotalFee;
                    paymentRecord.OutTradeNo = req.OutTradeNo;
                    paymentRecord.Attach = req.Attach;
                    paymentRecord.Detail = req.Detail;
                    paymentRecord.Body = req.Body;
                    paymentRecord.ErrCodeDes = rsp.err_code_des;
                    paymentRecord.ErrCode = rsp.err_code;
                    paymentRecord.ResultCode = rsp.result_code;
                    paymentRecord.ReturnMsg = rsp.return_msg;
                    paymentRecord.ReturnCode = rsp.return_code;
                    paymentRecord.CreateTime = DateTime.Now;
                    _AppContext.PaymentRecordApp.Add(paymentRecord);
                }
            }
            catch (Exception ex)
            {
                rsp.return_msg = "系统内部错误";
                log4net.LogManager.GetLogger("weixin-pay-fee").Error(ex);
            }            
            return this.Ok(new ReturnObject("200", "success", rsp));
        }

        /// <summary>
        /// 支付成功的通知
        /// </summary>
        /// <param name="content">通知xml</param>
        /// <returns>结果</returns>
        [HttpPost]
        [Route("api/weixinpay/notifly")]
        public IHttpActionResult Notifly(string content)
        {
            PaymentRecord rsp = new PaymentRecord() { ReturnCode = "FAIL" };
            try
            {
                log4net.LogManager.GetLogger("weixin-pay-notifly").Info(string.Format("请求体:{0}", content));
                IPayment pay = PaymentFactory.GetInstance(PaymentType.weixin);
                if (!string.IsNullOrEmpty(content))
                {
                    var notifyObj = pay.Notify(content);
                    if (notifyObj != null && !string.IsNullOrEmpty(notifyObj.out_trade_no))
                    {
                        RequestQuery reqQuery = new RequestQuery();
                        reqQuery.MchId = notifyObj.mch_id;
                        reqQuery.OutTradeNo = notifyObj.out_trade_no;
                        var rspQuery = pay.Query(reqQuery);
                        //如果交易成功就返回付款信息
                        if (rspQuery != null)
                        {
                            //添加或完善付款单信息
                            PaymentRecord paymentRecord = new PaymentRecord();
                            paymentRecord.AppId = rspQuery.appid;
                            paymentRecord.MchId = reqQuery.MchId;
                            paymentRecord.State = rspQuery.trade_state;
                            paymentRecord.TradeType = rspQuery.trade_type;
                            paymentRecord.FeeType = rspQuery.fee_type;
                            paymentRecord.TotalFee = rspQuery.total_fee;
                            paymentRecord.OutTradeNo = reqQuery.OutTradeNo;
                            paymentRecord.Attach = rspQuery.attach;
                            paymentRecord.ErrCodeDes = rspQuery.err_code_des;
                            paymentRecord.ErrCode = rspQuery.err_code;
                            paymentRecord.ResultCode = rspQuery.result_code;
                            paymentRecord.ReturnMsg = rspQuery.return_msg;
                            paymentRecord.ReturnCode = rspQuery.return_code;
                            paymentRecord.IsSubscribe = rspQuery.is_subscribe;
                            paymentRecord.BankType = rspQuery.bank_type;
                            paymentRecord.SettlemenTotalFee = rspQuery.settlement_total_fee;
                            paymentRecord.CashFee = rspQuery.cash_fee;
                            paymentRecord.CreateTime = DateTime.Now;
                            paymentRecord.TimeEnd = rspQuery.time_end;
                            _AppContext.PaymentRecordApp.Save(paymentRecord);
                            rsp = _AppContext.PaymentRecordApp.Find(reqQuery.OutTradeNo);
                        }
                        else
                        {
                            rsp.ReturnMsg = "网络请求过程中发生错误";
                        }
                    }
                    else
                    {
                        rsp.ReturnMsg = "通知的消息体格式错误，请校验";
                    }
                }
                else
                {
                    rsp.ReturnMsg = "通知内容不能为空";
                }
            }
            catch (Exception ex)
            {
                rsp.ReturnMsg = "系统内部错误";
                log4net.LogManager.GetLogger("weixin-pay-notifly").Error(ex);
            }
            return this.Ok(new ReturnObject("200", "success", rsp));
        }
        /// <summary>
        /// 修改付款记录的状态
        /// </summary>
        /// <param name="mch_id">商户号</param>
        /// <param name="out_trade_No">商家的单号</param>
        /// <returns>结果</returns>
        [HttpPost]
        [Route("api/weixinpay/updatestate")]
        public IHttpActionResult UpdateState(string mch_id,string out_trade_No)
        {
            PaymentRecord rsp = new PaymentRecord() { ReturnCode = "FAIL" };
            try
            {
                log4net.LogManager.GetLogger("weixin-pay-updatestate").Info(string.Format("mch_id:{0},out_trade_no:{1}", mch_id, out_trade_No));
                IPayment pay = PaymentFactory.GetInstance(PaymentType.weixin);                
                if (string.IsNullOrEmpty(mch_id))
                {
                    rsp.ReturnMsg = "商户号不能为空";
                }
                else if (!string.IsNullOrEmpty(out_trade_No))
                {
                    RequestQuery reqQuery = new RequestQuery();
                    reqQuery.MchId = mch_id;
                    reqQuery.OutTradeNo = out_trade_No;
                    var rspQuery = pay.Query(reqQuery);
                    if (rspQuery != null)
                    {
                        //添加或完善付款单信息
                        PaymentRecord paymentRecord = new PaymentRecord();
                        paymentRecord.AppId = rspQuery.appid;
                        paymentRecord.MchId = reqQuery.MchId;
                        paymentRecord.State = rspQuery.trade_state;
                        paymentRecord.TradeType = rspQuery.trade_type;
                        paymentRecord.FeeType = rspQuery.fee_type;
                        paymentRecord.TotalFee = rspQuery.total_fee;
                        paymentRecord.OutTradeNo = reqQuery.OutTradeNo;
                        paymentRecord.Attach = rspQuery.attach;
                        paymentRecord.ErrCodeDes = rspQuery.err_code_des;
                        paymentRecord.ErrCode = rspQuery.err_code;
                        paymentRecord.ResultCode = rspQuery.result_code;
                        paymentRecord.ReturnMsg = rspQuery.return_msg;
                        paymentRecord.ReturnCode = rspQuery.return_code;
                        paymentRecord.IsSubscribe = rspQuery.is_subscribe;
                        paymentRecord.BankType = rspQuery.bank_type;
                        paymentRecord.SettlemenTotalFee = rspQuery.settlement_total_fee;
                        paymentRecord.CashFee = rspQuery.cash_fee;
                        paymentRecord.CreateTime = DateTime.Now;
                        paymentRecord.TimeEnd = rspQuery.time_end;
                        _AppContext.PaymentRecordApp.Save(paymentRecord);
                        rsp = _AppContext.PaymentRecordApp.Find(reqQuery.OutTradeNo);

                    }
                    else
                    {
                        rsp.ReturnMsg = "网络请求过程中发生错误";
                    }
                }
                else
                {
                    rsp.ReturnMsg = "通知的消息体格式错误，请校验";
                }
            }
            catch (Exception ex)
            {
                rsp.ReturnMsg = "系统内部错误";
                log4net.LogManager.GetLogger("weixin-pay-updatestate").Error(ex);
            }
            return this.Ok(new ReturnObject("200", "success", rsp));
        }
    }
}
