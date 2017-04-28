using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.Domain.Common;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;

namespace Vcyber.BLMS.Domain
{
    public class SmsApp : ISmsApp
    {

        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="type">短信类型</param>
        /// <param name="phoneNumber">手机号</param>
        /// <param name="data">短信数据</param>
        /// <returns></returns>
        public ReturnResult SendSMS(ESmsType type, string phoneNumber, string[] datas, bool isValidSendCount = true)
        {
            string message = SmsSendBase.ConvertContent(type, datas);            
            return this.SendSMS(phoneNumber, message, isValidSendCount);
        }




        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="type">短信类型</param>
        /// <param name="data">短信数据</param>
        /// <returns></returns>
        public ReturnResult SendSMS(ESmsType type, SmsData data, bool isValidSendCount = true)
        {
            string message = this.SendFactory(type).ConvertContent(type, data);
            return this.SendSMS(data.PhoneNumber, message, isValidSendCount);
        }

        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="phoneNumber">手机号</param>
        /// <param name="message">短信内容</param>
        /// <returns>执行结果</returns>
        public ReturnResult SendSMS(string phoneNumber, string message, bool isValidSendCount = true)
        {
            //初始化返回结果
            var returnResult = new ReturnResult { IsSuccess = true };

            if (isValidSendCount)
            {
                int count = _AppContext.ValidateCodeApp.GetCount(phoneNumber);

                if (count >= 10)
                {
                    returnResult.IsSuccess = false;
                    returnResult.Message = "超出今日验证码发送次数";
                    return returnResult;
                }

                int count1 = _AppContext.ValidateCodeApp.InMinuteValidateCount(phoneNumber);

                if (count1 >= 2)
                {
                    returnResult.IsSuccess = false;
                    returnResult.Message = "1分钟内只能发送一次验证码，请稍后再试";
                    return returnResult;
                }

                int count2 = _AppContext.ValidateCodeApp.InHourValidateCount(phoneNumber);

                if (count2 >= 5)
                {
                    returnResult.IsSuccess = false;
                    returnResult.Message = "1小时内只能发送三次验证码，请稍后再试";
                    return returnResult;
                }
            }

            //发送验证码
            var result = SMSHelperUtils.SendSMS(new SmsSendMessage
            {
                mobile = phoneNumber,
                msg = message
            });
            //配置参数不完整
            if (string.IsNullOrEmpty(result))
            {
                returnResult.IsSuccess = false;
                returnResult.Message = "短信发送失败，请确认配置参数";
                return returnResult;
            }

            //数据不完整
            if (string.IsNullOrEmpty(phoneNumber) || string.IsNullOrEmpty(message))
            {
                returnResult.IsSuccess = false;
                returnResult.Message = "短信发送失败，请确认手机号和短信内容";
                return returnResult;
            }

            //发送失败的情况
            if (!result.Contains("ok"))
            {
                returnResult.IsSuccess = false;
                returnResult.Message = result;
                return returnResult;
            }

            return returnResult;
        }

        #region ==== 私有方法 ====

        private SmsSendBase SendFactory(ESmsType type)
        {
            switch (type)
            {
                case ESmsType.注册成功:
                    break;
                case ESmsType.提交注册_发送验证码:
                    break;
                case ESmsType.提交注册_未匹配到索九车:
                    break;
                //case ESmsType.提交注册_匹配到索九车:
                //    break;
                case ESmsType.找回密码:
                    break;
                case ESmsType.重置密码:
                    break;
                case ESmsType.新购LF车主:
                    break;
                case ESmsType.换购增购LF车主:
                    break;
                case ESmsType.积分支付: return new IntegralPaySms();
                    break;
                case ESmsType.蓝豆支付:
                    break;
                case ESmsType.支付完成_bluemembers银卡会员:
                    break;
                case ESmsType.后台订单发货:
                    break;
                case ESmsType.消耗积分发送验证码:
                    break;
                case ESmsType.消费管理_积分消耗成功:
                    break;
                case ESmsType.消费管理_审核通过:
                    break;
                case ESmsType.预约_预约成功:
                    break;
                case ESmsType.上门关怀_索九会员:
                    break;
                case ESmsType.上门关怀_上门关怀服务预约成功后提示:
                    break;
                case ESmsType.机场服务_预约成功下发短信:
                    break;
                case ESmsType.机场服务_免费兑换次数用完告知短信:
                    break;
                case ESmsType.机场服务_成功使用服务码告知短信:
                    break;
                case ESmsType.机场服务_编码使用记录告知短信:
                    break;
                case ESmsType.机场服务_编码过期告知短信:
                    break;
                case ESmsType.电子卡券:
                    break;
                default:
                    break;
            }

            return null;
        }

        #endregion
    }
}
