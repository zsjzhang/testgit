using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.IRepository;

namespace Vcyber.BLMS.Domain
{
    public class SC_ServiceCardUsedRecordApp : ISC_ServiceCardUsedRecordApp
    {
        public ReturnResult ConfirmUseCard(SC_ServiceCardUsedRecord record)
        {
            ReturnResult result = new ReturnResult { IsSuccess = true };

            var scr = _DbSession.SC_ServiceCardUsedRecordStorager.GetSCServiceCardUsedRecord(record.CardType, record.CardNo);

            if (scr != null)
            {
                result.IsSuccess = false;
                result.Message = "该兑换劵已经被使用,请确认";
                return result;
            }
            else
            {
                result.IsSuccess = _DbSession.SC_ServiceCardUsedRecordStorager.AddISCServiceCardUsedRecord(record);
                return result;
            }
        }

        public IEnumerable<SC_ServiceCardUsedRecord> SelectRecordList(SC_ServiceCardUsedRecordSearchParam param)
        {
            return _DbSession.SC_ServiceCardUsedRecordStorager.SelectRecordList(param);
        }

        public IEnumerable<Remeal> SelectRepairList(SC_ServiceCardUsedRecordSearchParam param)
        {
            return _DbSession.SC_ServiceCardUsedRecordStorager.SelectRepairList(param);
        }

        //YC活动通过手机号获取用户信息
        public RecommendCustomer GetRecommendNameByPhone(string phone)
        {
            var model = _DbSession.CustomCardInfoStorager.GetRecommendNameByPhone(phone);
            return model;
        }

        public ReturnResult GetServiceCardInfo(string cardId, string code, string activity)
        {
            AfterSaleServiceWXModel data = null;
            ReturnResult result = new ReturnResult { IsSuccess = true };

            if (activity == "Y" || activity == "spring" || activity == "lingdong")
            {
                //密钥
                string PrivateKey = "BlueMemeberWechatApi_WXCard";

                //加密
                string sign = "";
                using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
                {
                    sign =
                        BitConverter.ToString(
                            md5.ComputeHash(UTF8Encoding.Default.GetBytes(String.Format("{0}+{1}", code, PrivateKey))))
                            .Replace("-", "");
                }

                //参数
                string key = "cardId=" + cardId + "&code=" + code + "&activity=" + activity + "&checkcode=" + sign;

                //地址
                string url = "https://www.bluemembers.com.cn/weixin/WXCard/GetTelByCode?" + key;

                //调用服务
                data =
                    Vcyber.BLMS.Common.Web.WebUtils.JsonToObj<AfterSaleServiceWXModel>(
                        Vcyber.BLMS.Common.Web.WebUtils.GET_WebRequestHTML("utf-8", url, null), null);
            }
            else
            {
                var cardModel = _DbSession.CustomCardInfoStorager.GetSingleCustomCardInfo(cardId);
                if (cardModel != null)
                {
                    if(cardModel.CardSource==2)
                    {
                        code = string.Format("{0}[{1}]", cardId, code);
                    }
                }

                data = _DbSession.ActivitiesStorager.CheckCusomCard(cardId, code);

            }

            if (data == null)
            {
                result.IsSuccess = false;
                result.Message = "无效的卡劵号码，请确认";
            }
            else if (data.ret != 1)
            {
                result.IsSuccess = false;
                result.Message = data.msg;
            }
            else
            {
                result.Data = data;
            }

            return result;
        }

        public ReturnResult ServiceCardConsum(string cardId, string code, string activity)
        {
            ReturnResult result = new ReturnResult { IsSuccess = true };

            //密钥
            string PrivateKey = "BlueMemeberWechatApi_WXCard";

            //加密
            string sign = "";
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                sign = BitConverter.ToString(md5.ComputeHash(UTF8Encoding.Default.GetBytes(String.Format("{0}+{1}", code, PrivateKey)))).Replace("-", "");
            }

            //参数
            string key = "cardId=" + cardId + "&code=" + code + "&checkcode=" + sign + "&activity=" + activity;

            //地址
            string url = "https://www.bluemembers.com.cn/weixin/WXCard/ConsumCard?" + key;

            //调用服务
            var data = Vcyber.BLMS.Common.Web.WebUtils.JsonToObj<AfterSaleServiceWXModel>(Vcyber.BLMS.Common.Web.WebUtils.GET_WebRequestHTML("utf-8", url, null), null);

            if (data.ret != 1)
            {
                result.IsSuccess = false;
                result.Message = data.msg;
            }
            else
            {
                result.Data = data;
            }

            return result;
        }

        public ReturnResult UpdateRecord(int id, string custName, int Mileage)
        {
            ReturnResult result = new ReturnResult { IsSuccess = true };

            result.IsSuccess = _DbSession.SC_ServiceCardUsedRecordStorager.Update(id, custName, Mileage);

            return result;
        }

        public IEnumerable<Remeal> SelectRemealByVin(string Vin, string CardType)
        {
            return _DbSession.SC_ServiceCardUsedRecordStorager.SelectRemealByVin(Vin, CardType);
        }

        public IEnumerable<SC_ServiceCardUsedRecord> SelectRecordByVin(string vin, string activityTag)
        {
            return _DbSession.SC_ServiceCardUsedRecordStorager.SelectRecordByVin(vin, activityTag);
        }

        public IEnumerable<SC_ServiceCardUsedRecord> SelectRecordByVinAndCardType(string vin, string CardType)
        {
            return _DbSession.SC_ServiceCardUsedRecordStorager.SelectRecordByVinAndCardType(vin, CardType);
        }

      
        public CustomCardInfo GetCustomTypeByVin(string Vin, string ActivityType)
        {
            return _DbSession.CustomCardInfoStorager.GetCustomTypeByVin(Vin, ActivityType);
        }
    }
}
