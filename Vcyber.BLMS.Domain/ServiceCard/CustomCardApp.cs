using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.IRepository;

namespace Vcyber.BLMS.Domain
{
    public class CustomCardApp : ICustomCardApp
    {

        public ReturnResult AddRepair(Remeal model)
        {
            ReturnResult result = new ReturnResult{IsSuccess=true};

            bool flag= _DbSession.CustomCardStorager.AddRepair(model);

            if (!flag)
            {
                result.IsSuccess = false;
                result.Message = "购买套餐失败";
                return result;
            }
            return result;


        }

        public ReturnResult AddCustomCard(CustomCard model)
        {
            ReturnResult result = new ReturnResult { IsSuccess = true };
            model.CreateTime = DateTime.Now;
            var addResult = _DbSession.CustomCardStorager.AddCustomCard(model);
            if (!addResult)
            {
                result.IsSuccess = false;
                result.Message = "添加卡券失败";
                return result;
            }
            return result;
        }

        /// <summary>
        /// 获取用户自定义优惠券 状态（未使用，已使用，已过期）
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="source">卡券来源 1：北京现代 ；2：合作商户；</param>
        /// <param name="status">状态（1：未使用，2：已使用，3：已过期）</param>
        /// <returns>优惠券列表信息</returns>
        public IEnumerable<ReturnUserCustomCardModel> GetUserCustomCardList(string userId, int source, int status)
        {
            return _DbSession.CustomCardStorager.GetUserCustomCardList(userId, source, status);
        }

        /// <summary>
        /// 获取候机服务券
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="source"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public IEnumerable<SNCard> GetTerminalservicevoucherCardList(string userId, int source, int status)
        {
            return _DbSession.CustomCardStorager.GetTerminalservicevoucherCardList(userId, source, status);
        }

        /// <summary>
        /// 根据手机号码，查询用户卡券信息
        /// </summary>
        /// <param name="phone">手机号码</param>
        /// <param name="activityName">活动名称</param>
        /// <param name="cardType">卡券ID</param>
        /// <param name="pageIndex">分页号</param>
        /// <param name="pageSize">分页数</param>
        /// <param name="totalCount">返回总行数</param>
        /// <returns></returns>
        public IEnumerable<UserCustomCard> GetUserCustomCardListByPhone(string userId, string activityName, string cardType, int pageIndex, int pageSize, out int totalCount)
        {
            return _DbSession.CustomCardStorager.GetUserCustomCardListByPhone(userId, activityName, cardType, pageIndex, pageSize, out totalCount);
        }





        public CustomCard GetUserCustomCardByCardType(string cardType)
        {
            return _DbSession.CustomCardStorager.GetUserCustomCardByCardType(cardType);
        }

        /// <summary>
        /// 根据用户ID和卡券的活动名字获取用户领取的卡券列表
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="ActivityType">卡券的活动名字</param>
        /// <returns></returns>
        public List<CustomCard> getUserCustomCardByUID(string userId, string ActivityType)
        {
            return _DbSession.CustomCardStorager.getUserCustomCardByUID(userId, ActivityType);
        }
        /// <summary>
        /// 根据用户ID和卡券的活动名字获取用户领取的卡券列表
        /// </summary>
        public List<CustomCard> GetUserCustomCardByActivityType(string userId, string ActivityType)
        {
            return _DbSession.CustomCardStorager.GetUserCustomCardByActivityType(userId, ActivityType);
        }
        public int GetCardUsedCount(string cardType)
        {
            return _DbSession.CustomCardStorager.GetCardUsedCount(cardType);
        }

        public IEnumerable<CustomCardInfo> GetCarsByCardName(string cardname)
        {
            return _DbSession.CustomCardStorager.GetCarsByCardName(cardname);
        }


        public int GetUserReissueCount(string cardType, string phone)
        {
            return _DbSession.CustomCardStorager.GetUserReissueCount(cardType, phone);
        }

        public int GetCountRepar(string vin, string cardType)
        {
            return _DbSession.CustomCardStorager.GetCountRepar(vin,cardType);
        }

        public Remeal GetRemeal(string vin, string cardtype, string cardcode)
        {
            return _DbSession.CustomCardStorager.GetRemeal(vin, cardtype,cardcode);
        }

        public ReturnCustomCardCodeInfo GetCustomCardCodeInfo(string cardCode)
        {
            return _DbSession.CustomCardStorager.GetCustomCardCodeInfo(cardCode);
        }

        public ReturnCustomCardCodeInfoByDMS GetCustomCardCodeInfoByDMS(string cardCode)
        {
            return _DbSession.CustomCardStorager.GetCustomCardCodeInfoByDMS(cardCode);
        }


        public IEnumerable<ReturnCustomCardCodeInfo> GetUserSummerCustomCardListByUserId(string userId)
        {
            return _DbSession.CustomCardStorager.GetUserSummerCustomCardListByUserId(userId);
        }


        /// <summary>
        /// 给用户发送获取卡券信息的短信，目前短信格式只支持兑奖码参数传入，其他短信信息必须在卡券后台短信字段里录入；
        /// </summary>
        /// <param name="model">卡券信息</param>
        /// <param name="cardCode">兑奖码</param>
        /// <param name="phone">手机号码</param>
        /// <returns></returns>
        public ReturnResult SendCustomCardSms(CustomCardInfo model, string cardCode, string phone)
        {
            ReturnResult resultSms = new ReturnResult() { IsSuccess = true, Message = "001" };
            //发送卡券短信信息
            string message = model.SmsContent;
            if (!string.IsNullOrEmpty(model.SmsContent))
            {
                //是否推送兑换码信息
                if (model.SmsContent.Contains("{0}"))
                {
                    message = string.Format(model.SmsContent, cardCode);
                }
                return _AppContext.SMSApp.SendSMS(phone, message, false);
            }
            return resultSms;
        }

        /// <summary>
        /// 给用户发送获取卡券信息的短信，目前短信格式只支持兑奖码参数传入，其他短信信息必须在卡券后台短信字段里录入；
        /// </summary>
        /// <param name="model">卡券信息</param>
        /// <param name="sms">卡券短信参数</param>
        /// <param name="phone">手机号码</param>
        /// <returns></returns>
        public ReturnResult SendCustomCardSms(CustomCardInfo model, CustomCardSms sms, string phone)
        {
            ReturnResult resultSms = new ReturnResult() { IsSuccess = true, Message = "001" };
            //发送卡券短信信息
            string message = model.SmsContent;
            if (!string.IsNullOrEmpty(model.SmsContent))
            {
                //{0}：兑换码； {1}：有效期
                if (model.SmsContent.Contains("{1}"))
                {
                    //构建用户卡券有效期；
                    var dateInfo = "";
                    var beginDate = model.CardBeginDate;
                    var endDate = model.CardEndDate;
                    //是否是领取后xx天有效；
                    if (model.CardValidityType == (int)ECardValidityType.After)
                    {
                        beginDate = DateTime.Now;
                        endDate = beginDate.AddDays(model.CardValidity);
                    }
                    //日期格式化yyyy年MM月dd日
                    dateInfo = string.Format("{0}至{1}", beginDate.ToString("yyyy年MM月dd日"), endDate.ToString("yyyy年MM月dd日"));
                    message = string.Format(model.SmsContent, sms.CardCode, dateInfo);
                }
                else if (model.SmsContent.Contains("{0}"))
                {
                    message = string.Format(model.SmsContent, sms.CardCode);
                }
                return _AppContext.SMSApp.SendSMS(phone, message, false);
            }
            return resultSms;
        }

        /// <summary>
        /// 获取用户可用的卡券数量
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public int GetReceivedCustomCardCount(string userid)
        {
            return  _DbSession.CustomCardStorager.GetReceivedCustomCardCount(userid);
        }
    }
}
