using AspNet.Identity.SQL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Results;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Common;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.WebApi.Filter;
using Vcyber.BLMS.WebApi.Models.ResponseData;

namespace Vcyber.BLMS.WebApi.Controllers
{

    [RoutePrefix("api/MaintainService")]
    public class MaintainServiceController : ApiController
    {
        /// <summary>
        /// 获取5种养护产品信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetMaintainServiceList")]
        [ResponseType(typeof(List<ResMaintainServiceInfo>))]
        [IOVAuthorize]
        public IHttpActionResult GetMaintainServiceList()
        {
            ReturnResult result = new ReturnResult();
            try
            {
                List<ResMaintainServiceInfo> cardList = new List<ResMaintainServiceInfo>();
                //获取5种养护产品信息
                var list =
                    _AppContext.MaintainServiceAPP.GetCustomCardInfoByActType(
                        Convert.ToString((int)ECustomerCardActType.Maintain));
                if (list != null)
                {
                    cardList =
                        list.Select(o => new ResMaintainServiceInfo()
                        {
                            ActivityType = o.ActivityType,
                            CardType = o.CardType,
                            CardLogoUrl = ConfigurationManager.AppSettings["ImgPath"] + o.CardLogoUrl,
                            CardName = o.CardName
                        }).ToList();
                }
                result.Data = cardList;
                result.IsSuccess = true;
                result.Message = "SUCCESS";
                return Ok(result);
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = "ERROR";
                log4net.LogManager.GetLogger("五种养护").Info(ex);
                return Ok(result);
            }
        }

        /// <summary>
        /// 判断是否可以兑换五种养护产品
        /// </summary>        
        /// <param name="userId">用户id</param>
        /// /// <param name="cardType">卡券ID</param>
        /// <returns>是否可以兑换（0：没有登录，1：没有权限，2：可以兑换）</returns>        
        [Route("checkstatus")]
        [HttpGet]
        [ResponseType(typeof(String))]
        public IHttpActionResult CheckStatus(string userId, string cardType)
        {
            var store = new FrontUserStore<FrontIdentityUser>();
            var rsp = new ReturnObject("2", "success", "");
            if (string.IsNullOrEmpty(userId))
            {
                return this.Ok(new ReturnObject("0", "尊敬的会员您好，此服务仅限金卡和银卡会员可领取哦，请您升级后领取！", ""));
            }
            var user = store.FindByIdAsync(userId).Result;
            if (user == null)
            {
                return this.Ok(new ReturnObject("0", "尊敬的会员您好，此服务仅限金卡和银卡会员可领取哦，请您升级后领取！", ""));                
            }
            if (user.MLevel != 11 && user.MLevel != 12)
            {
                return this.Ok(new ReturnObject("1", "尊敬的会员您好，此服务仅限金卡和银卡会员可领取哦，请您升级后领取！", ""));                    
            }
            //到这一步说明已经是金卡或者银卡会员，再次验证在2016.4.1之后有没有购车
            var carList = _AppContext.CarServiceUserApp.SelectCarListByIdentity(user.IdentityNumber);
            if (carList == null || carList.Count() < 0)
            {
                return this.Ok(new ReturnObject("1", "您好，5种养护产品只针对2016年4月1日之后购车并注册成为bluemembers银卡/金卡会员的用户，您可关注其它会员服务。", ""));                    
            }
            //筛选购车时间在 2016.04.01之后的数据
            carList = carList.Where(t => t.BuyTime >= DateTime.Parse("2016-04-01 00:00:00")).ToList();
            if (carList.Count() <= 0)
            {
                return this.Ok(new ReturnObject("1", "您好，5种养护产品只针对2016年4月1日之后购车并注册成为bluemembers银卡/金卡会员的用户，您可关注其它会员服务。", ""));                  
            }
            //查询卡券领取记录 3是5种车辆养护产品的类型ID
            var customCardList = _AppContext.CustomCardApp.getUserCustomCardByUID(userId, "3");
            //获取会员有效期
            var MLevelBeginDate = user.MLevelBeginDate;
            var MLevelInvalidDate = user.MLevelInvalidDate;
            //判断当前时间是否在会员有效期内
            if (MLevelBeginDate > DateTime.Now || MLevelInvalidDate < DateTime.Now)
            {
                return this.Ok(new ReturnObject("1", "尊敬的会员您好，此服务仅限金卡和银卡会员可领取哦，请您升级后领取！", ""));                  
            }
            //如果没有领取过，则验证成功，可以领取
            if (customCardList.Count <= 0)
            {
                return this.Ok(rsp);
            }

            #region 第3种流程
            //筛选只保留会员有效期内领取记录
            customCardList = customCardList.Where(t => t.CreateTime >= MLevelBeginDate && t.CreateTime <= MLevelInvalidDate).ToList();

            //如果今年没有领取过，则验证成功，可以领取
            if (customCardList.Count <= 0)
            {
                return this.Ok(rsp);
            }
            //根据会员级别来判断可以领取的次数
            switch (user.MLevel)
            {
                case 11:
                    if (customCardList.Count >= 1)
                    {
                        return this.Ok(new ReturnObject("1", "尊敬的会员您好，银卡会员只有1次/年领取机会，您已领取过了，不要贪心哦。您可以第2年再来领取！", ""));                            
                    }
                    break;
                case 12:
                    if (customCardList.Count >= 2)
                    {
                        return this.Ok(new ReturnObject("1", "尊敬的会员您好，金卡会员只有2次/年领取机会，您已领取过了，不要贪心哦。您可以第2年再来领取！", ""));    
                    }
                    //如果只领取了1次，需要在判断这次领取的是不是同一个产品
                    foreach (var item in customCardList)
                    {
                        if (item.CardType == cardType)
                        {
                            return this.Ok(new ReturnObject("1", "尊敬的会员您好，同一种车辆养护产品1年只能领取1次哦。您已领取过了，请您选择其他养护产品吧！", ""));                                
                        }
                    }
                    break;
            }

            #endregion
            
            return this.Ok(rsp);
        }

        /// <summary>
        /// 兑换五种养护产品卡券
        /// </summary>
        /// <param name="userId">用户id</param>
        /// /// <param name="cardType">卡券ID</param>
        /// <returns>是否可以兑换（1：失败，2：成功）</returns>   
        [HttpPost]
        [Route("exchange")]
        [ResponseType(typeof(String))]
        [IOVAuthorize]
        public IHttpActionResult Exchange(string userId, string cardType)
        {
            var rsp = new ReturnObject("2", "success", ""); 
            var member = _AppContext.DealerMembershipApp.GetMembershipByUserId(userId);
            if (member != null)
            {
                var customCardInfo = _AppContext.CustomCardInfoApp.GetSingleCustomCardInfoByGuid(cardType);
                if (customCardInfo == null)
                {
                    return Ok(new ReturnObject("0", "尊敬的会员您好，此卡券不存在，请联系管理员！", ""));                    
                }
                if (DateTime.Now < customCardInfo.CardBeginDate || DateTime.Now > customCardInfo.CardEndDate)
                {
                    return Ok(new ReturnObject("0", "尊敬的会员您好，未在卡券的领取有效期内，请联系管理员！", ""));  
                }
                var rescardCode = Vcyber.BLMS.Common.RandomNumberHelper.GetUserCustomCardCode();
                var customcard = new CustomCard()
                {
                    CardType = customCardInfo.CardType,
                    CardCode = rescardCode,
                    CardId = customCardInfo.Id,
                    CreateTime = DateTime.Now,
                    IsSave = true,
                    IsCancel = false,
                    UserId = userId,
                    IsReissue = false,
                    Tel = member.UserName,
                    IsSend = true,
                    OpenId = "",
                    Source = "blms_wechat"
                };
                var result = _AppContext.CustomCardApp.AddCustomCard(customcard);
                if (result.IsSuccess)
                {
                    _AppContext.CustomCardInfoApp.UpdateCustomCardQuantityByType(customCardInfo.CardType);
                    var expiryTime = string.Empty;
                    var message = "尊敬的会员您好，您已领取{0}卡券，卡券有效期{1}，请尽快使用！详情登录bluemembers个人中心查看。";
                    if (customCardInfo.CardValidity == 0)
                    {
                        var cardBeginDate = customCardInfo.CardBeginDate.ToString("yyyy年MM月dd");
                        var cardEndDate = customCardInfo.CardEndDate.ToString("yyyy年MM月dd");
                        expiryTime = cardBeginDate + "到" + cardEndDate;                        
                    }
                    else
                    {
                        expiryTime = DateTime.Now.ToString("yyyy年MM月dd") + "到" + (DateTime.Now.AddDays(customCardInfo.CardValidity).ToString("yyyy年MM月dd"));                        
                    }
                    rsp.Message = string.Format(message, customCardInfo.CardName, expiryTime);
                    //如果卡券配置了SmsContent，则使用SmsContent
                    if (string.IsNullOrEmpty(customCardInfo.SmsContent))
                    {
                        _AppContext.SMSApp.SendSMS(ESmsType.车辆养护产品5种, member.PhoneNumber, new string[] { customCardInfo.CardName.Replace("卡券", ""), expiryTime }, false);
                    }
                    else
                    {
                        _AppContext.CustomCardApp.SendCustomCardSms(customCardInfo, new CustomCardSms() { CardCode = rescardCode }, member.UserName);
                    }                    
                }
                else
                {
                    rsp.Code = "1";
                    rsp.Message = "卡券兑换失败！";
                }
            }

            return Ok(rsp);
        }
    }
}