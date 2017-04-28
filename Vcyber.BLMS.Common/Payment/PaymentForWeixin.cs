
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vcyber.BLMS.Common.Payment.Model;
using Vcyber.BLMS.Common.Payment.Model.ReponseCreate;
using Vcyber.BLMS.Common.Payment.Model.ResponseNotify;
using Vcyber.BLMS.Common.Payment.Model.Request;
using Vcyber.BLMS.Common.Payment.Model.Response;
using Vcyber.BLMS.Common.Payment.Model.ResponseQuery;
using Vcyber.BLMS.Common.Payment.Model.ResponseRefund;
using Vcyber.BLMS.Common.Web;

namespace Vcyber.BLMS.Common.Payment
{
    public class PaymentForWeixin : IPayment
    {
        public ResponseCreate Create(RequestCreate obj)
        {
            var rsp = new ResponseCreate();
            var url = "https://api.mch.weixin.qq.com/pay/unifiedorder";                                    
            var key = string.Empty;
            var redisExtend = new RedisExtend();
            using (redisExtend)
            {
                redisExtend.Connect();
                key = redisExtend.Hget("weixin:pay:keys", obj.MchId);
            }

            var currDate = DateTime.Now;
            obj.AppId = "wx8ba75eb0ebbb764b";
            obj.DeviceInfo = "WEB";            
            obj.NonceStr = Guid.NewGuid().ToString().Replace("-", "");
            obj.SignType = "MD5";
            obj.FeeType = "CNY";
            obj.TimeStart = currDate.ToString("yyyyMMddhhmmss");
            obj.TimeExpire = currDate.AddDays(1).ToString("yyyyMMddhhmmss");            
            SortedDictionary<string, object> paramsDic = new SortedDictionary<string, object>();
            paramsDic.Add("appid", obj.AppId);
            paramsDic.Add("mch_id", obj.MchId);
            paramsDic.Add("device_info", obj.DeviceInfo);
            paramsDic.Add("nonce_str", obj.NonceStr);            
            paramsDic.Add("sign_type", obj.SignType);
            paramsDic.Add("body", obj.Body);
            paramsDic.Add("detail", obj.Detail);
            paramsDic.Add("attach", obj.Attach);
            paramsDic.Add("out_trade_no", obj.OutTradeNo);
            paramsDic.Add("fee_type", obj.FeeType);
            paramsDic.Add("total_fee", obj.TotalFee);
            paramsDic.Add("spbill_create_ip", obj.SpbillCreateIp);
            paramsDic.Add("time_start", obj.TimeStart);
            paramsDic.Add("time_expire", obj.TimeExpire);
            paramsDic.Add("notify_url", obj.NotifyUrl);
            paramsDic.Add("trade_type", obj.TradeType);
            paramsDic.Add("product_id", obj.ProductId);
            paramsDic.Add("openid", obj.OpenId);

            var strParams = string.Empty;
            foreach (var k in paramsDic.Keys)
            {
                strParams += strParams.Trim().Length > 0 ? "&" : "";
                strParams = strParams + k + "=" + paramsDic[k].ToString();
            }
            string strSignTemp = strParams + "&key=" + key;
            obj.Sign = CommonUtilitys.EncodeMD5(strSignTemp, "UTF-8").ToUpper();
            string data = WebUtils.ObjToXml<RequestCreate>(obj);
            var result = HttpUtil.Post(url, data, "text/xml");
            log4net.LogManager.GetLogger("weixin-pay-request").Info(data);
            log4net.LogManager.GetLogger("weixin-pay-response").Info(result);
            if (!string.IsNullOrEmpty(result))
            {
                rsp = (ResponseCreate)WebUtils.XmlDeserialize<Vcyber.BLMS.Common.Payment.Model.ReponseCreate.xml>(result);
            }
            rsp.key = key;//返回key,h5调起需要用到
            return rsp;
        }
        public ResponseRefund Refund(RequestRefund obj)
        {
            var rsp = new ResponseRefund();
            var url = "https://api.mch.weixin.qq.com/secapi/pay/refund";
            var certPaht = System.Configuration.ConfigurationManager.AppSettings["CertPath"]+"\\"+obj.MchId+".p12";
            var certPwd = obj.MchId;
            var key = string.Empty;
            var redisExtend = new RedisExtend();
            using (redisExtend)
            {
                redisExtend.Connect();
                key = redisExtend.Hget("weixin:pay:keys", obj.MchId);                
            }
            var currDate = DateTime.Now;
            obj.AppId = "wx8ba75eb0ebbb764b";
            obj.DeviceInfo = "WEB";
            obj.NonceStr = Guid.NewGuid().ToString().Replace("-", "");
            obj.SignType = "MD5";
            obj.RefundFeeType = "CNY";
            obj.OpUserId = obj.MchId;
            SortedDictionary<string, object> paramsDic = new SortedDictionary<string, object>();
            paramsDic.Add("appid", obj.AppId);
            paramsDic.Add("mch_id", obj.MchId);
            paramsDic.Add("device_info", obj.DeviceInfo);
            paramsDic.Add("nonce_str", obj.NonceStr);
            paramsDic.Add("sign_type", obj.SignType);
            paramsDic.Add("out_trade_no", obj.OutTradeNo);
            paramsDic.Add("out_refund_no", obj.OutRefundNo);
            paramsDic.Add("total_fee", obj.TotalFee);
            paramsDic.Add("refund_fee", obj.RefundFee);
            paramsDic.Add("refund_fee_type", obj.RefundFeeType);
            paramsDic.Add("op_user_id", obj.OpUserId);
            paramsDic.Add("refund_account", obj.RefundAccount);

            var strParams = string.Empty;
            foreach (var k in paramsDic.Keys)
            {
                strParams += strParams.Trim().Length > 0 ? "&" : "";
                strParams = strParams + k + "=" + paramsDic[k].ToString();
            }
            string strSignTemp = strParams + "&key=" + key;
            obj.Sign = CommonUtilitys.EncodeMD5(strSignTemp, "UTF-8").ToUpper();
            string data = WebUtils.ObjToXml<RequestRefund>(obj);
            var result = HttpUtil.Post(url, data, "text/xml", certPaht, certPwd);
            log4net.LogManager.GetLogger("weixin-pay-refund").Error(result);
            if (!string.IsNullOrEmpty(result))
            {
                if (result.IndexOf("xml") > -1)
                {
                    rsp = (ResponseRefund)WebUtils.XmlDeserialize<Vcyber.BLMS.Common.Payment.Model.ResponseRefund.xml>(result);
                }
                else
                {
                    rsp.return_code = "FAIL";
                    rsp.return_msg = result;
                }
            }
            return rsp;
        }
        /// <summary>
        /// 企业付款抢红包
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public ResponseGiveFund GiveFund(RequestGiveFund obj)
        {
            var rsp = new ResponseGiveFund();
            var url = "https://api.mch.weixin.qq.com/mmpaymkttransfers/promotion/transfers";
            var certPaht = System.Configuration.ConfigurationManager.AppSettings["CertPath"];
            var certPwd = "1233041902";
            var key = "1Z9H8A1N0G3Y0U2NFEIAPISAVEZHIFUM";
            obj.mch_appid = "wx8ba75eb0ebbb764b";
            obj.mchid = "1233041902";            
            obj.partner_trade_no = obj.nonce_str;
            obj.check_name = "NO_CHECK";
            SortedDictionary<string, object> paramsDic = new SortedDictionary<string, object>();
            paramsDic.Add("amount", obj.amount);
            paramsDic.Add("check_name", obj.check_name);
            paramsDic.Add("desc", obj.desc);
            paramsDic.Add("mch_appid", obj.mch_appid);
            paramsDic.Add("mchid", obj.mchid);
            paramsDic.Add("nonce_str", obj.nonce_str);
            paramsDic.Add("openid", obj.openid);
            paramsDic.Add("partner_trade_no", obj.partner_trade_no);
            paramsDic.Add("spbill_create_ip", obj.spbill_create_ip);

            var strParams = string.Empty;
            foreach (var k in paramsDic.Keys)
            {
                strParams += strParams.Trim().Length > 0 ? "&" : "";
                strParams = strParams + k + "=" + paramsDic[k].ToString();
            }
            string strSignTemp = strParams + "&key=" + key;
            obj.sign = CommonUtilitys.EncodeMD5(strSignTemp, "UTF-8").ToUpper();
            Vcyber.BLMS.Common.Payment.Model.Request.xml xmlObj = new Vcyber.BLMS.Common.Payment.Model.Request.xml() 
            { 
                amount = obj.amount,
                desc = obj.desc,
                mch_appid = obj.mch_appid,
                mchid = obj.mchid,
                nonce_str = obj.nonce_str,
                openid = obj.openid,
                partner_trade_no = obj.partner_trade_no,
                sign = obj.sign,
                spbill_create_ip = obj.spbill_create_ip,
                check_name = obj.check_name
            };
            string data = WebUtils.ObjToXml<Vcyber.BLMS.Common.Payment.Model.Request.xml>(xmlObj);           
            var result = HttpUtil.Post(url, data, "text/xml", certPaht, certPwd);
            if (!string.IsNullOrEmpty(result))
            {
                rsp = (ResponseGiveFund)WebUtils.XmlDeserialize<Vcyber.BLMS.Common.Payment.Model.Response.xml>(result);                
            }            
            return rsp;
        }

        /// <summary>
        /// 通知
        /// </summary>
        /// <param name="content">通知内容</param>
        /// <returns>通知实体</returns>
        public ResponseNotify Notify(string content)
        {
            var rsp = new ResponseNotify();
            log4net.LogManager.GetLogger("weixin-pay-notify").Error(content);
            if (!string.IsNullOrEmpty(content))
            {
                rsp = (ResponseNotify)WebUtils.XmlDeserialize<Vcyber.BLMS.Common.Payment.Model.ResponseNotify.xml>(content);
            }
            return rsp;
        }
        /// <summary>
        /// 查询订单
        /// </summary>
        /// <param name="obj">查询条件</param>
        /// <returns>订单信息</returns>
        public ResponseQuery Query(RequestQuery obj)
        {
            var rsp = new ResponseQuery();
            var url = "https://api.mch.weixin.qq.com/pay/orderquery";
            var key = string.Empty;
            var redisExtend = new RedisExtend();
            using (redisExtend)
            {
                redisExtend.Connect();
                key = redisExtend.Hget("weixin:pay:keys", obj.MchId);
            }            
            obj.AppId = "wx8ba75eb0ebbb764b";            
            obj.NonceStr = Guid.NewGuid().ToString().Replace("-", "");
            obj.SignType = "MD5";            
            SortedDictionary<string, object> paramsDic = new SortedDictionary<string, object>();
            paramsDic.Add("appid", obj.AppId);
            paramsDic.Add("mch_id", obj.MchId);
            paramsDic.Add("out_trade_no", obj.OutTradeNo);
            paramsDic.Add("nonce_str", obj.NonceStr);
            paramsDic.Add("sign_type", obj.SignType);
            //生成sign
            var strParams = string.Empty;
            foreach (var k in paramsDic.Keys)
            {
                strParams += strParams.Trim().Length > 0 ? "&" : "";
                strParams = strParams + k + "=" + paramsDic[k].ToString();
            }
            string strSignTemp = strParams + "&key=" + key;
            obj.Sign = CommonUtilitys.EncodeMD5(strSignTemp, "UTF-8").ToUpper();
            string data = WebUtils.ObjToXml<RequestQuery>(obj);
            var result = HttpUtil.Post(url, data, "text/xml");
            log4net.LogManager.GetLogger("weixin-pay-query").Error(result);
            if (!string.IsNullOrEmpty(result))
            {
                rsp = (ResponseQuery)WebUtils.XmlDeserialize<Vcyber.BLMS.Common.Payment.Model.ResponseQuery.xml>(result);
            }            
            return rsp;
        }
    }
}