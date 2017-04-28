using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Mvc;
using Antlr.Runtime.Misc;
using AspNet.Identity.SQL;
using PetaPoco;
using Vcyber.BLMS.WebApi.Models;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Entity.Common;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.Entity.Generated;
using Vcyber.BLMS.IRepository;
using Vcyber.BLMS.WebApi.Filter;
using Vcyber.BLMS.WebApi.Models.RequestData;
using Vcyber.BLMS.WebApi.Models.ResponseData;


namespace Vcyber.BLMS.WebApi.Controllers
{
    public class CustomCardController : ApiController
    {


        /// <summary>
        /// 获取卡券列表
        /// </summary>
        /// <returns></returns>      
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/CustomCard/GetCustomCardList")]
        [ResponseType(typeof(ReturnCustomCardTypeModel))]
        [IOVAuthorize]
        public IHttpActionResult GetCustomCardList()
        {
            var list = _AppContext.SCServiceCardTypeApp.GetSCServiceCardTypeList().ToList();
            return this.Ok(new ReturnObject(list));
        }



        #region 生成12为优惠券码

        /// <summary>
        /// 生成12为优惠券码
        /// </summary>
        /// <returns></returns>
        private string GetUserCustomCardCode()
        {
            var guid = Guid.NewGuid().ToString();
            var hashCode = guid.GetHashCode();
            var absHashCode = Math.Abs(guid.GetHashCode()).ToString();

            int rdmLength = 12 - absHashCode.Length;
            if (rdmLength > 4)
            {
                guid = Guid.NewGuid().ToString();
                hashCode = guid.GetHashCode();
                absHashCode = Math.Abs(guid.GetHashCode()).ToString();
            }
            if (rdmLength == 0)
            {
                return absHashCode;
            }
            else
            {
                int minRan = 1;
                int maxRan = 10;
                if (rdmLength == 4)
                {
                    minRan = 1000;
                    maxRan = 10000;
                }
                else if (rdmLength == 3)
                {
                    minRan = 100;
                    maxRan = 1000;
                }
                else if (rdmLength == 2)
                {
                    minRan = 10;
                    maxRan = 100;
                }
                Random dom = new Random(hashCode);
                int random = dom.Next(minRan, maxRan);
                return absHashCode + random.ToString();
            }
        }

        /// <summary>
        /// 生成的优惠券兑奖码入库
        /// </summary>
        /// <param name="customcard">用户优惠券兑奖码信息</param>
        /// <returns></returns>
        private bool GetHashAddCustomCard(CustomCard customcard)
        {
            bool isOk = true;
            try
            {
                var retAdd = _AppContext.CustomCardApp.AddCustomCard(customcard);
                if (!retAdd.IsSuccess)
                {
                    isOk = false;
                }
            }
            catch (Exception)
            {

                isOk = false;
            }
            return isOk;
        }

        #endregion


        /// <summary>
        /// 返回用户领取卡券兑奖码信息
        /// </summary>
        /// <param name="cardType">卡券Guid</param>
        /// <param name="userId">用户ID </param>
        /// <param name="phone">电话号码</param>
        /// <param name="source">数据来源(blms:前台网站；blms_web：手机app;blms_wechat：微信)</param>
        /// <returns>返回用户领取兑奖码信息</returns>
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/CustomCard/GetUserCustomCardNo")]
        [ResponseType(typeof(ReturnUserCustomCardNoInfo))]
        [IOVAuthorize]
        public IHttpActionResult GetUserCustomCardNo(string cardType, string userId, string phone, string source)
        {
            ReturnUserCustomCardNoInfo result = new ReturnUserCustomCardNoInfo { IsSuccess = true };
            if (string.IsNullOrEmpty(cardType))
            {
                result.Message = "卡券Guid为空";
                result.IsSuccess = false;
                return Ok(result);
            }
            if (string.IsNullOrEmpty(userId))
            {
                result.Message = "用户ID为空";
                result.IsSuccess = false;
                return Ok(result);
            }
            if (string.IsNullOrEmpty(phone))
            {
                result.Message = "用户电话号码为空";
                result.IsSuccess = false;
                return Ok(result);
            }
            var userInfo = _AppContext.DealerMembershipApp.GetMembershipByUserId(userId);
            if (userInfo == null)
            {
                result.Message = "用户ID不存在";
                result.IsSuccess = false;
                return Ok(result);
            }
            if (userInfo.UserName != phone)
            {
                result.Message = "输入的手机号与会员的手机号不一致";
                result.IsSuccess = false;
                return Ok(result);
            }

            //构建用户获取自定义优惠券信息
            var userCardCode = "";
            var resCardCode = "";
            if (string.IsNullOrEmpty(source)) { source = "blms_wechat"; }
            var customCardInfo = _AppContext.CustomCardInfoApp.GetSingleCustomCardInfoByGuid(cardType);
            if (customCardInfo == null)
            {
                result.Message = "卡券类型不存在";
                result.IsSuccess = false;
                return Ok(result);
            }
            if (DateTime.Now < customCardInfo.CardBeginDate || DateTime.Now > customCardInfo.CardEndDate)
            {
                result.Message = "领取的卡券不在有效期内，请联系管理员";
                result.IsSuccess = false;
                return Ok(result);
            }
            if (customCardInfo.CardSource == (int)EMerchantType.Bjxd)
            {
                #region  北京现代卡券
                var usedCount = _AppContext.CustomCardApp.GetCardUsedCount(customCardInfo.CardType);
                if (customCardInfo.Quantity - usedCount > 0)
                {
                    userCardCode = GetUserCustomCardCode();
                    resCardCode = userCardCode;
                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = "券码库存不足了";
                    return Ok(result);
                }
                #endregion
            }
            else
            {
                #region  合作商户 卡券
                var merchant = _AppContext.CustomCardMerchantConsumeCodeApp.GetSingleCardMerchantConsumeCode(cardType);
                if (merchant != null)
                {
                    userCardCode = string.Format("{0}[{1}]", cardType, merchant.CardCode);
                    resCardCode = merchant.CardCode;
                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = "商户券码库存不足了";
                    return Ok(result);
                }
                #endregion
            }
            var customcard = new CustomCard()
            {
                CardType = customCardInfo.CardType,
                CardCode = userCardCode,
                CardId = customCardInfo.Id,
                CreateTime = DateTime.Now,
                IsSave = true,
                IsCancel = false,
                UserId = userId,
                IsReissue = false,
                Tel = phone,
                IsSend = true,
                Source = source,
                OpenId = "",
            };

            //根据Guid,随机数，生成优惠券兑奖码；
            var hashCustomCard = GetHashAddCustomCard(customcard);
            if (!hashCustomCard)
            {
                //用户第一次建生成的优惠券兑奖码入库失败后，补入一次；如果失败直接返回获取兑奖码获取失败；
                if (customCardInfo.CardSource == (int)EMerchantType.Bjxd)
                {
                    customcard.CardCode = GetUserCustomCardCode();
                    resCardCode = customcard.CardCode;
                }
                else
                {
                    var merchant = _AppContext.CustomCardMerchantConsumeCodeApp.GetSingleCardMerchantConsumeCode(cardType);
                    if (merchant != null)
                    {
                        customcard.CardCode = string.Format("{0}[{1}]", cardType, merchant.CardCode);
                        resCardCode = merchant.CardCode;
                    }
                }

                hashCustomCard = GetHashAddCustomCard(customcard);
                if (hashCustomCard)
                {
                    //更新卡券库存；
                    _AppContext.CustomCardInfoApp.UpdateCustomCardQuantityByType(customcard.CardType);

                    //给用户发送卡券领取短信；
                    _AppContext.CustomCardApp.SendCustomCardSms(customCardInfo,
                        new CustomCardSms() { CardCode = resCardCode }, phone);
                    result.IsSuccess = true;
                    result.Code = resCardCode;
                    result.Message = "领取兑奖码成功";
                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = "领取兑奖码失败";
                }
            }
            else
            {
                _AppContext.CustomCardInfoApp.UpdateCustomCardQuantityByType(customcard.CardType);
                result.IsSuccess = true;
                result.Code = resCardCode;
                result.Message = "领取兑奖码成功";
            }
            return Ok(result);
        }



        /// <summary>
        /// 返回一张卡券信息
        /// </summary>
        /// <param name="cardType">卡券GUID</param>
        /// <returns>返回一张卡券信息</returns>
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/CustomCard/GetSingleCustomCardInfo")]
        [ResponseType(typeof(ReturnCustomCardInfo))]
        //[IOVAuthorize]
        public IHttpActionResult GetSingleCustomCardInfo(string cardType)
        {
            ReturnResult result = new ReturnResult();
            if (string.IsNullOrEmpty(cardType))
            {
                result.Message = "卡券Guid为空";
                result.IsSuccess = false;
                return Ok(result);
            }
            var info = _AppContext.CustomCardInfoApp.GetSingleCustomCardInfo(cardType);
            result.Data = info;
            result.IsSuccess = true;
            return this.Ok(new ReturnObject(result));
        }

        /// <summary>
        /// 返回用户卡券列表状态（未使用、已使用、已过期）
        /// </summary>
        /// <param name="merchantType">商户类型 1：北京现代，2：合作商户</param>
        /// <param name="userId">用户ID</param>
        /// <returns>返回用户卡券状态（未使用、已使用、已过期）</returns>
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/CustomCard/GetUserCustomCardList")]
        [ResponseType(typeof(ReturnUserCustomCardModel))]
        [IOVAuthorize]
        public IHttpActionResult GetUserCustomCardList(string merchantType, string userId)
        {
            ReturnResult result = new ReturnResult();
            if (string.IsNullOrEmpty(merchantType))
            {
                result.Message = "商户类型为空";
                result.IsSuccess = false;
                return Ok(result);
            }
            if (string.IsNullOrEmpty(userId))
            {
                result.Message = "用户ID为空";
                result.IsSuccess = false;
                return Ok(result);
            }


            UserCustomCardModel serCustomCardModel = new UserCustomCardModel();
            //未使用
            serCustomCardModel.ReceivedCustomCardList = new List<ReturnUserCustomCardModel>();
            serCustomCardModel.ReceivedCustomCardList = _AppContext.CustomCardApp.GetUserCustomCardList(userId, Convert.ToInt32(merchantType), 1).ToList();

            //已使用
            serCustomCardModel.UsedCustomCardList = new List<ReturnUserCustomCardModel>();
            serCustomCardModel.UsedCustomCardList = _AppContext.CustomCardApp.GetUserCustomCardList(userId, Convert.ToInt32(merchantType), 2).ToList();

            //已过期
            serCustomCardModel.ExpiredCustomCardList = new List<ReturnUserCustomCardModel>();
            serCustomCardModel.ExpiredCustomCardList = _AppContext.CustomCardApp.GetUserCustomCardList(userId, Convert.ToInt32(merchantType), 3).ToList();
            result.IsSuccess = true;
            result.Data = serCustomCardModel;

            return this.Ok(new ReturnObject(result));
        }

        /// <summary>
        /// 返回一张用户卡券信息
        /// </summary>
        /// <param name="id">用户卡券核销编号</param>
        /// <param name="userId">用户ID</param>
        /// <returns>返回一张用户卡券信息</returns>
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/CustomCard/GetSingleUserCustomCardInfo")]
        [ResponseType(typeof(ReturnUserCustomCardInfo))]
        //[IOVAuthorize]
        public IHttpActionResult GetSingleUserCustomCardInfo(string id, string userId)
        {
            ReturnResult result = new ReturnResult();
            if (string.IsNullOrEmpty(id))
            {
                result.Message = "用户卡券核销编号";
                result.IsSuccess = false;
                return Ok(result);
            }
            if (string.IsNullOrEmpty(userId))
            {
                result.Message = "用户ID为空";
                result.IsSuccess = false;
                return Ok(result);
            }
            var info = _AppContext.CustomCardInfoApp.GetSingleUserCustomCardInfoByIdAndUserId(id, userId);
            result.Data = info;
            result.IsSuccess = true;
            return this.Ok(new ReturnObject(result));
        }

        /// <summary>
        /// 返回一张兑奖信息，是否核销，是否领取，是否发送，劵码有效期；
        /// </summary>
        /// <param name="cardType">卡券Guid</param>
        /// <param name="cardCode">兑奖码</param>
        /// <returns>返回一张兑奖信息，是否核销，是否领取，是否发送，劵码有效期；</returns>
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/CustomCard/GetCustomCardCodeInfo")]
        [ResponseType(typeof(ReturnCustomCardCodeInfo))]
        //[IOVAuthorize]
        public IHttpActionResult GetCustomCardCodeInfo(string cardType, string cardCode)
        {
            ReturnResult result = new ReturnResult();
            if (string.IsNullOrEmpty(cardType))
            {
                result.Message = "卡券Guid为空";
                result.IsSuccess = false;
                return Ok(result);
            }
            if (string.IsNullOrEmpty(cardCode))
            {
                result.Message = "兑奖码为空";
                result.IsSuccess = false;
                return Ok(result);
            }
            var cardinfo = _AppContext.CustomCardInfoApp.GetSingleCustomCardInfo(cardType);
            if (cardinfo == null)
            {
                result.Message = "卡券类型不存在";
                result.IsSuccess = false;
                return Ok(result);
            }
            string code = cardCode;
            if (cardinfo.CardSource == (int)EMerchantType.Partner)
            {                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                       
                code = string.Format("{0}[{1}]", cardType, cardCode);
            }
            var codeInfo = _AppContext.CustomCardApp.GetCustomCardCodeInfo(code);
            result.Data = codeInfo;
            result.IsSuccess = true;
            return this.Ok(new ReturnObject(result));

        }


        #region 夏季保养

        /// <summary>
        /// 获取夏季包养卡券列表
        /// </summary>
        /// <returns></returns>      
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/CustomCard/GetSummerCardList")]
        [ResponseType(typeof(ReturnCustomCardTypeModel))]
        [IOVAuthorize]
        public IHttpActionResult GetSummerCardList()
        {
            var list = _AppContext.SCServiceCardTypeApp.GetSummerCardList().ToList();
            return this.Ok(new ReturnObject(list));
        }
        /// <summary>
        /// 返回用户领取卡券兑奖码信息
        /// </summary>
        /// <param name="cardType">卡券Guid</param>
        /// <param name="userId">用户ID </param>
        /// <param name="phone">电话号码</param>
        /// <param name="source">数据来源(blms:前台网站；blms_web：手机app;blms_wechat：微信)</param>
        /// <returns>返回用户领取兑奖码信息</returns>
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/CustomCard/GetSummerUserCustomCardNo")]
        [ResponseType(typeof(ReturnUserCustomCardNoInfo))]
        [IOVAuthorize]
        public IHttpActionResult GetSummerUserCustomCardNo(string cardType, string userId, string phone, string source = "blms_wechat")
        {
            ReturnUserCustomCardNoInfo result = new ReturnUserCustomCardNoInfo { IsSuccess = true };
            if (string.IsNullOrEmpty(cardType))
            {
                result.Message = "卡券Guid为空";
                result.IsSuccess = false;
                return Ok(result);
            }
            if (string.IsNullOrEmpty(userId))
            {
                result.Message = "用户ID为空";
                result.IsSuccess = false;
                return Ok(result);
            }
            if (string.IsNullOrEmpty(phone))
            {
                result.Message = "用户电话号码为空";
                result.IsSuccess = false;
                return Ok(result);
            }
            var userInfo = _AppContext.DealerMembershipApp.GetMembershipByUserId(userId);
            if (userInfo == null)
            {
                result.Message = "用户ID不存在";
                result.IsSuccess = false;
                return Ok(result);
            }
            if (userInfo.UserName != phone)
            {
                result.Message = "输入的手机号与会员的手机号不一致";
                result.IsSuccess = false;
                return Ok(result);
            }

            //用户是否已经领取夏季保养卡券，如果已经领取直接返回以前领取的卡券；
            var userCard = _AppContext.CustomCardApp.GetUserSummerCustomCardListByUserId(userId).ToList();
            if (userCard != null && userCard.Count > 0)
            {
                result.IsSuccess = true;
                result.Code = userCard.FirstOrDefault().CardCode;
                result.Message = "领取兑奖码成功";
                return Ok(result);
            }

            //构建用户获取自定义优惠券信息
            var userCardCode = "";
            var resCardCode = "";
            var customCardInfo = _AppContext.CustomCardInfoApp.GetSingleCustomCardInfoByGuid(cardType);
            if (customCardInfo == null)
            {
                result.Message = "卡券类型不存在";
                result.IsSuccess = false;
                return Ok(result);
            }
            if (DateTime.Now < customCardInfo.CardBeginDate || DateTime.Now > customCardInfo.CardEndDate)
            {
                result.Message = "领取的卡券不在有效期内，请联系管理员";
                result.IsSuccess = false;
                return Ok(result);
            }
            if (customCardInfo.CardSource == (int)EMerchantType.Bjxd)
            {
                #region  北京现代卡券
                var usedCount = _AppContext.CustomCardApp.GetCardUsedCount(customCardInfo.CardType);
                if (customCardInfo.Quantity - usedCount > 0)
                {
                    userCardCode = GetUserCustomCardCode();
                    resCardCode = userCardCode;
                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = "券码库存不足了";
                    return Ok(result);
                }
                #endregion
            }
            else
            {
                #region  合作商户 卡券
                var merchant = _AppContext.CustomCardMerchantConsumeCodeApp.GetSingleCardMerchantConsumeCode(cardType);
                if (merchant != null)
                {
                    userCardCode = string.Format("{0}[{1}]", cardType, merchant.CardCode);
                    resCardCode = merchant.CardCode;
                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = "商户券码库存不足了";
                    return Ok(result);
                }
                #endregion
            }
            var customcard = new CustomCard()
            {
                CardType = customCardInfo.CardType,
                CardCode = userCardCode,
                CardId = customCardInfo.Id,
                CreateTime = DateTime.Now,
                IsSave = true,
                IsCancel = false,
                UserId = userId,
                IsReissue = false,
                Tel = phone,
                IsSend = true,
                Source = source,
                OpenId = "",
            };

            //根据Guid,随机数，生成优惠券兑奖码；
            var hashCustomCard = GetHashAddCustomCard(customcard);
            if (!hashCustomCard)
            {
                //用户第一次建生成的优惠券兑奖码入库失败后，补入一次；如果失败直接返回获取兑奖码获取失败；
                if (customCardInfo.CardSource == (int)EMerchantType.Bjxd)
                {
                    customcard.CardCode = GetUserCustomCardCode();
                    resCardCode = customcard.CardCode;
                }
                else
                {
                    var merchant = _AppContext.CustomCardMerchantConsumeCodeApp.GetSingleCardMerchantConsumeCode(cardType);
                    if (merchant != null)
                    {
                        customcard.CardCode = string.Format("{0}[{1}]", cardType, merchant.CardCode);
                        resCardCode = merchant.CardCode;
                    }
                }

                hashCustomCard = GetHashAddCustomCard(customcard);
                if (hashCustomCard)
                {
                    //更新卡券库存；
                    _AppContext.CustomCardInfoApp.UpdateCustomCardQuantityByType(customcard.CardType);

                    //给用户发送卡券领取短信；
                    _AppContext.CustomCardApp.SendCustomCardSms(customCardInfo,
                        new CustomCardSms() { CardCode = resCardCode }, phone);

                    result.IsSuccess = true;
                    result.Code = resCardCode;
                    result.Message = "领取兑奖码成功";
                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = "领取兑奖码失败";
                }
            }
            else
            {
                //更新卡券库存；
                _AppContext.CustomCardInfoApp.UpdateCustomCardQuantityByType(customcard.CardType);

                //给用户发送卡券领取短信；
                _AppContext.CustomCardApp.SendCustomCardSms(customCardInfo,
                    new CustomCardSms() { CardCode = resCardCode }, phone);

                result.IsSuccess = true;
                result.Code = resCardCode;
                result.Message = "领取兑奖码成功";
            }
            return Ok(result);
        }


        #endregion

        #region  DMS-BM 查询卡券是否可用

        /// <summary>
        /// 查询卡券是否可用
        /// </summary>
        /// <param name="model">查询卡券是否可用传入参数实体</param>
        /// <returns>查询卡券是否可用</returns>
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/CustomCard/GetCustomCardCodeInfoByDMSInfo")]
        [ResponseType(typeof(ResCustomCardInfo))]
        //[IOVAuthorize]
        public IHttpActionResult GetCustomCardCodeInfoByDMSInfo(RequestCustomCardInfo model)
        {
            ReturnResult result = new ReturnResult() { IsSuccess = true, Errors = "", Message = "", Data = new { } };
            try
            {
                #region 校验信息
                if (string.IsNullOrEmpty(model.CardNo))
                {
                    result.Message = "卡券兑奖码不能为空";
                    result.IsSuccess = false;
                    return Ok(result);
                }
                #region update by 20161221
                //if (string.IsNullOrEmpty(model.CardType))
                //{
                //    result.Message = "卡券类型不能为空";
                //    result.IsSuccess = false;
                //    return Ok(result);
                //}
                //if (string.IsNullOrEmpty(model.DealerId))
                //{
                //    result.Message = "经销商ID不能为空";
                //    result.IsSuccess = false;
                //    return Ok(result);
                //}
                //if (string.IsNullOrEmpty(model.IdentityNumber))
                //{
                //    result.Message = "证件号码不能为空";
                //    result.IsSuccess = false;
                //    return Ok(result);
                //}
                #endregion
                //if (string.IsNullOrEmpty(model.Vin))
                //{
                //    result.Message = "车架号不能为空";
                //    result.IsSuccess = false;
                //    return Ok(result);
                //}
                //如果身份证号码不为空，获取会员信息；
                if (!string.IsNullOrEmpty(model.IdentityNumber))
                {
                    Task<FrontIdentityUser> cusObj = null;
                    cusObj = new FrontUserStore<FrontIdentityUser>().FindByIdentityNumber(model.IdentityNumber);
                    if (cusObj == null || cusObj.Result == null || string.IsNullOrEmpty(cusObj.Result.Id))
                    {
                        result.IsSuccess = false;
                        result.Message = "未找到该身份证下的会员信息 ";
                        return Ok(result);
                    }
                }
                //如果经销商ID不为空，获取经销商信息
                if (!string.IsNullOrEmpty(model.DealerId))
                {
                    var dealer = _AppContext.DealerApp.GetDealerByDealerId(model.DealerId);
                    if (dealer == null)
                    {
                        result.IsSuccess = false;
                        result.Message = "未找到经销商信息 ";
                        return Ok(result);
                    }
                }
                //获取卡券信息
               // var customCardInfo = _AppContext.CustomCardInfoApp.GetSingleCustomCardInfoByGuid(model.CardType);
                var customCardInfo = _AppContext.CustomCardInfoApp.GetSingleCustomCardInfoByCNo(model.CardNo);
                if (customCardInfo == null)
                {
                    result.IsSuccess = false;
                    result.Message = "未找到该卡券信息 ";
                    return Ok(result);
                }

                #endregion

                #region 处理卡券信息
                var resCustomCardInfo = new ResCustomCardInfo();
                string code = model.CardNo;
                if (customCardInfo.CardSource == (int)EMerchantType.Partner)
                {
                    code = string.Format("{0}[{1}]", customCardInfo.CardType, model.CardNo);
                }
                //兑奖码是否存在
                var codeInfo = _AppContext.CustomCardApp.GetCustomCardCodeInfo(code);
                if (codeInfo == null)
                {
                    result.IsSuccess = false;
                    result.Message = "未找到该卡券的兑换码信息 ";
                    return Ok(result);
                }
                //兑奖码是否已经核销
                if (codeInfo.IsCancel)
                {
                    resCustomCardInfo.Msg = "兑奖码已核销";
                    resCustomCardInfo.IfResult = "F";
                }
                //兑奖码是否在有效期内
                else if (codeInfo.CardBeginDate <= DateTime.Now && codeInfo.CardEndDate >= DateTime.Now)
                {
                    resCustomCardInfo.Msg = "兑奖码可以使用";
                    resCustomCardInfo.IfResult = "S";
                }
                else
                {
                    resCustomCardInfo.Msg = "兑奖码不在卡券有效期内";
                    resCustomCardInfo.IfResult = "F";
                }
                result.Data = resCustomCardInfo;
                result.Message = "SUCCESS";
                return Ok(result);
                #endregion
            }
            catch (Exception)
            {
                result.IsSuccess = false;
                result.Message = "ERROR";
                return Ok(result);
            }
        }

        /// <summary>
        /// 卡券使用
        /// </summary>
        /// <param name="model">用户卡券使用传参实体</param>
        /// <returns>返回卡券使用信息</returns>
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/CustomCard/UserAwardCustomCardInfo")]
        [ResponseType(typeof(ResUserAwardCustomCardInfo))]
        //[IOVAuthorize]
        public IHttpActionResult UserAwardCustomCardInfo(RequestUserAwardCustomCardInfo model)
        {
            log4net.LogManager.GetLogger("UserAwardCustomCardInfo").Info("-----处理程序开始执行");
            ReturnResult result = new ReturnResult() { IsSuccess = true, Errors = "", Message = "", Data = new { } };
            try
            {
                #region 校验信息
                if (string.IsNullOrEmpty(model.CardNo))
                {
                    result.Message = "卡券兑奖码不能为空";
                    result.IsSuccess = false;
                    return Ok(result);
                }
                #region update by 20161221
                //if (string.IsNullOrEmpty(model.CardType))
                //{
                //    result.Message = "卡券类型不能为空";
                //    result.IsSuccess = false;
                //    return Ok(result);
                //}
                if (string.IsNullOrEmpty(model.DealerId))
                {
                    result.Message = "经销商ID不能为空";
                    result.IsSuccess = false;
                    return Ok(result);
                }
                //if (string.IsNullOrEmpty(model.IdentityNumber))
                //{
                //    result.Message = "证件号码不能为空";
                //    result.IsSuccess = false;
                //    return Ok(result);
                //}
                #endregion
                if (string.IsNullOrEmpty(model.Vin))
                {
                    result.Message = "车架号不能为空";
                    result.IsSuccess = false;
                    return Ok(result);
                }

                if (string.IsNullOrEmpty(model.DMSOrderNo))
                {
                    result.Message = "工单号不能为空";
                    result.IsSuccess = false;
                    return Ok(result);
                }
                //if (string.IsNullOrEmpty(model.CarCategory))
                //{
                //    result.Message = "车型不能为空";
                //    result.IsSuccess = false;
                //    return Ok(result);
                //}
                //if (string.IsNullOrEmpty(model.CustName))
                //{
                //    result.Message = "客户名称不能为空";
                //    result.IsSuccess = false;
                //    return Ok(result);
                //}
                //if (string.IsNullOrEmpty(model.MileAge))
                //{
                //    result.Message = "行驶不能为空";
                //    result.IsSuccess = false;
                //    return Ok(result);
                //}
                //如果身份证号不为空，获取会员信息；
                if (!string.IsNullOrEmpty(model.IdentityNumber))
                {
                    var cusObj = new FrontUserStore<FrontIdentityUser>().FindByIdentityNumber(model.IdentityNumber);

                    if (cusObj == null || cusObj.Result == null || string.IsNullOrEmpty(cusObj.Result.Id))
                    {
                        result.IsSuccess = false;
                        result.Message = "未找到该身份证下的会员信息 ";
                        return Ok(result);
                    }
                }
                //如果经销商ID不为空，获取经销商信息
                
                    var dealer = _AppContext.DealerApp.GetDealerByDealerId(model.DealerId);
                    if (dealer == null)
                    {
                        result.IsSuccess = false;
                        result.Message = "未找到经销商信息 ";
                        return Ok(result);
                    }
                
                #region add by wangchunrong
                //根据CardNo查询CardType
                //是合作商户还是北现
                //if (model..CardSource == (int)EMerchantType.Partner)
                //{
                //    code = string.Format("{0}[{1}]",model.CardType, model.CardNo);
                //}
                //根据CardNO去查找Cardtype，然后根据Cardtype查找卡券信息
                //var codeInfo = _AppContext.CustomCardApp.GetCustomCardCodeInfo(model.CardNo);wangchunrong add
                //var customCardInfo = _AppContext.CustomCardInfoApp.GetSingleCustomCardInfoByGuid(codeInfo.CardType); wangchunrong add
                #endregion

                //update by wangchunrong 20161221
                //获取卡券信息
                //var customCardInfo = _AppContext.CustomCardInfoApp.GetSingleCustomCardInfoByGuid(model.CardType);
                var customCardInfo = _AppContext.CustomCardInfoApp.GetSingleCustomCardInfoByCNo(model.CardNo);
               
                if (customCardInfo == null)
                {
                    result.IsSuccess = false;
                    result.Message = "未找到该卡券信息 ";
                    return Ok(result);
                }

                //是否已经核销了
                //var vinData = _AppContext.ServiceCardUsedRecordApp.SelectRecordByVin(model.Vin, customCardInfo.ActivityType);// todo:核销
                var vinData = _AppContext.ServiceCardUsedRecordApp.SelectRecordByVin(model.Vin, customCardInfo.CardType);
                if (vinData != null && vinData.Count() > 0)
                {
                    result.IsSuccess = false;
                    result.Message = "该VIN码在此次活动中已经核销过，不能重复使用";
                    return Ok(result);
                }

                //客户信息
                IFCustomer cust = null;

                //车辆信息
                Car car = _AppContext.CarServiceUserApp.GetCarInfoByVIN(model.Vin, out cust);

                //VIN验证失败
                if (car == null)
                {
                    result.IsSuccess = false;
                    result.Message = "你输入的VIN码验证失败，请确认是否输入正确";
                    return Ok(result);
                }
                #region 在A店买的车，领取的卡券也可以在B点去核销的，所以这块不需要验证 update by 20161221
                //Vin中 经销商信息与客户输入的经销商ID不相符；
                //if (car.DealerId != dealer.DealerId)
                //{
                //    result.IsSuccess = false;
                //    result.Message = "你输入的VIN信息与经销商信息不相符";
                //    return Ok(result);
                //}
                #endregion
                #endregion
                //客户姓名
                string custName = string.Empty;
                string userId = string.Empty;

                if (cust != null)
                {
                    custName = cust.CustName;

                    //用户信息
                    var frontUserStore = new FrontUserStore<FrontIdentityUser>();
                    var membership = frontUserStore.FindByIdentityNumber(cust.IdentityNumber);

                    if (membership.Result != null)
                    {
                        userId = membership.Result.Id;
                    }
                }


                //用户卡券记录ID
                var usedRecord = _AppContext.ServiceCardUsedRecordApp.GetServiceCardInfo(customCardInfo.CardType, model.CardNo, customCardInfo.ActivityType);
                if (!usedRecord.IsSuccess)
                {
                    result.IsSuccess = false;
                    result.Message = usedRecord.Message;
                    return Ok(result);
                }
                if (string.IsNullOrEmpty(model.MileAge))
                {
                    model.MileAge = "0";
                }
                //构建核销记录实体信息
                SC_ServiceCardUsedRecord record = new SC_ServiceCardUsedRecord();
                record.VIN = model.Vin;
                record.ActivityTag = customCardInfo.ActivityType;
                //record.CarCategory = model.CarCategory;
                record.CarCategory = car.CarCategory;
                record.CardInfo = customCardInfo.CardName;
                record.CardNo = model.CardNo;
                record.CardType = customCardInfo.CardType;
                record.CreateTime = DateTime.Now;
                record.DealerId = model.DealerId;
                record.ConsumeDate = DateTime.Now;
                record.Mileage = Convert.ToInt32(model.MileAge);
                //record.UserId = cusObj.Result.Id;
                record.UserId = userId;
                record.DMSOrderNo = model.DMSOrderNo;//根据DMS需求，添加工单号字段
                log4net.LogManager.GetLogger("UserAwardCustomCardInfo").Info("-----查找卡券信息");
                //用户领取卡券信息
                AfterSaleServiceWXModel assModel = (AfterSaleServiceWXModel)usedRecord.Data;
                if (assModel != null)
                {
                    record.Id = assModel.data.id;
                    record.PhoneNumber = assModel.data.tel;
                    record.OpenId = assModel.data.openId;
                }

                //客户姓名
                record.CustName = model.CustName;
                if (cust != null)
                {
                    record.CustName = cust.CustName;
                }

                //车型
                record.CarCategory = model.CarCategory;
                if (car != null)
                {
                    record.CarCategory = car.CarCategory;
                }

                //构建核销结果信息
                var resUserAwardCustomCardInfo = new ResUserAwardCustomCardInfo();

                //开始核销
                var consumResult = _AppContext.ActivitiesApp.ServiceCardConsum(record.Id);
                log4net.LogManager.GetLogger("UserAwardCustomCardInfo").Info("-----开始核销");
                if (consumResult.IsSuccess)
                {
                    resUserAwardCustomCardInfo.Status = "1";
                    resUserAwardCustomCardInfo.ResultMsg = "核销卡券成功";
                    //保存核销记录

                    _AppContext.ServiceCardUsedRecordApp.ConfirmUseCard(record);
                    log4net.LogManager.GetLogger("UserAwardCustomCardInfo").Info("-----保存核销记录");
                }
                else
                {
                    resUserAwardCustomCardInfo.Status = "0";
                    resUserAwardCustomCardInfo.ResultMsg = "核销卡券失败";
                }
                result.Data = resUserAwardCustomCardInfo;
                result.IsSuccess = true;
                result.Message = "SUCCESS";
                return Ok(result);
            }
            catch (Exception ex)
            {
                log4net.LogManager.GetLogger("nov2016-proc").Error(ex);
                result.IsSuccess = false;
                result.Message = "ERROR";
                return Ok(result);
               
            }
        }
        #endregion

        #region 卡券核销索赔接口
        /// <summary>
        /// 卡券核销索赔接口
        /// </summary>
        /// <param name="reqCarClaimInformation">请求参数</param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/CustomCard/ClaimInformation")]
        [ResponseType(typeof(ResCarClaimInformation))]
        //[IOVAuthorize]
        // public IHttpActionResult ClaimInformation(string code, string activitycode, string createtime)
        public IHttpActionResult ClaimInformation(RequestCarClaimInformation reqCarClaimInformation)
        {
            ReturnResult result = new ReturnResult() { IsSuccess = true, Errors = "", Message = "", Data = new { } };
            try
            {
                //参数验证
                //if (string.IsNullOrEmpty(code) && string.IsNullOrEmpty(activitycode))
                //{
                //    result.IsSuccess = false;    
                //    result.Message = "活动代码和活动编号不能同时为空";
                //    return Ok(result);
                //}
                if (string.IsNullOrEmpty(reqCarClaimInformation.starttime) || string.IsNullOrEmpty(reqCarClaimInformation.endtime))
                {
                    result.IsSuccess = false;
                    result.Message = "时间参数不可以为空！！！";
                    return Ok(result);
                }
                IEnumerable<ResCarClaimInformation> query = _AppContext.CarClaimInformationApp.GetCarClaimInformation(reqCarClaimInformation.starttime, reqCarClaimInformation.endtime);

                result.Data = query;
                result.IsSuccess = true;
                result.Message = "SUCCESS";
                return Ok(result);
            }
            catch (Exception)
            {
                result.IsSuccess = false;
                result.Message = "ERROR";
                return Ok(result);
            }
        }
        #endregion

        #region  DMS-BM 查询卡券基本信息

        /// <summary>
        /// 查询卡券基本信息
        /// </summary>
        /// <param name="model">查询卡券是否可用传入参数实体</param>
        /// <returns>查询卡券是否可用</returns>
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/CustomCard/GetCustomCardCodeInfoByDMS")]
        [ResponseType(typeof(ResCustomCardInfoByDMS))]
        public IHttpActionResult GetCustomCardCodeInfoByDMS(RequestCustomCardInfo model)
        {
            ReturnResult result = new ReturnResult() { IsSuccess = true, Errors = "", Message = "", Data = new { } };
            try
            {
                #region 校验信息
                if (string.IsNullOrEmpty(model.CardNo))
                {
                    result.Message = "卡券兑奖码不能为空";
                    result.IsSuccess = false;
                    return Ok(result);
                }
                #region update by 20161221
                //if (string.IsNullOrEmpty(model.CardType))
                //{
                //    result.Message = "卡券类型不能为空";
                //    result.IsSuccess = false;
                //    return Ok(result);
                //}
                //if (string.IsNullOrEmpty(model.DealerId))
                //{
                //    result.Message = "经销商ID不能为空";
                //    result.IsSuccess = false;
                //    return Ok(result);
                //}
                //if (string.IsNullOrEmpty(model.IdentityNumber))
                //{
                //    result.Message = "证件号码不能为空";
                //    result.IsSuccess = false;
                //    return Ok(result);
                //}
                #endregion
                //if (string.IsNullOrEmpty(model.Vin))
                //{
                //    result.Message = "车架号不能为空";
                //    result.IsSuccess = false;
                //    return Ok(result);
                //}
                //如果身份证号不为空，获取会员信息；
                if (!string.IsNullOrEmpty(model.IdentityNumber))
                {
                    Task<FrontIdentityUser> cusObj = null;
                    cusObj = new FrontUserStore<FrontIdentityUser>().FindByIdentityNumber(model.IdentityNumber);
                    if (cusObj == null || cusObj.Result == null || string.IsNullOrEmpty(cusObj.Result.Id))
                    {
                        result.IsSuccess = false;
                        result.Message = "未找到该身份证下的会员信息 ";
                        return Ok(result);
                    }
                }
                //如果经销商ID部位空，获取经销商信息
                if (!string.IsNullOrEmpty(model.DealerId))
                {
                    var dealer = _AppContext.DealerApp.GetDealerByDealerId(model.DealerId);
                    if (dealer == null)
                    {
                        result.IsSuccess = false;
                        result.Message = "未找到经销商信息 ";
                        return Ok(result);
                    }
                }
                //获取卡券信息
                //var customCardInfo = _AppContext.CustomCardInfoApp.GetSingleCustomCardInfoByGuid(model.CardType);
                //add by wangchunrong 20161221
                var customCardInfo = _AppContext.CustomCardInfoApp.GetSingleCustomCardInfoByCNo(model.CardNo);

                if (customCardInfo == null)
                {
                    result.IsSuccess = false;
                    result.Message = "未找到该卡券信息 ";
                    return Ok(result);
                }

                #endregion

                #region 处理卡券信息
                var resCustomCardInfo = new ResCustomCardInfoByDMS();
                string code = model.CardNo;
                if (customCardInfo.CardSource == (int)EMerchantType.Partner)
                {
                    code = string.Format("{0}[{1}]", customCardInfo.CardType, model.CardNo);
                }
                //兑奖码是否存在
                var codeInfo = _AppContext.CustomCardApp.GetCustomCardCodeInfoByDMS(code);
                if (codeInfo == null)
                {
                    result.IsSuccess = false;
                    result.Message = "未找到该卡券的兑换码信息 ";
                    return Ok(result);
                }
                //兑奖码是否已经核销
                if (codeInfo.IsCancel)
                {
                    resCustomCardInfo.Msg = "兑奖码已核销";
                    resCustomCardInfo.IfResult = "F";
                    resCustomCardInfo.ReduceCost = codeInfo.ReduceCost;
                    resCustomCardInfo.Tel = codeInfo.Tel;
                    resCustomCardInfo.CardRemark = codeInfo.CardRemark;
                    resCustomCardInfo.CardBeginDate = codeInfo.CardBeginDate;
                    resCustomCardInfo.CardEndDate = codeInfo.CardEndDate;
                    resCustomCardInfo.CardCategory = codeInfo.CardCategory;//根据需求添加此字段

                }
                //兑奖码是否在有效期内
                else if (Convert.ToDateTime(codeInfo.CardBeginDate) <= DateTime.Now && Convert.ToDateTime(codeInfo.CardEndDate) >= DateTime.Now)
                {
                    resCustomCardInfo.Msg = "兑奖码可以使用";
                    resCustomCardInfo.IfResult = "S";
                    resCustomCardInfo.ReduceCost = codeInfo.ReduceCost;
                    resCustomCardInfo.Tel = codeInfo.Tel;
                    resCustomCardInfo.CardRemark = codeInfo.CardRemark;
                    resCustomCardInfo.CardBeginDate = codeInfo.CardBeginDate;
                    resCustomCardInfo.CardEndDate = codeInfo.CardEndDate;
                    resCustomCardInfo.CardCategory = codeInfo.CardCategory;//根据需求添加此字段
                }
                else
                {
                    resCustomCardInfo.Msg = "兑奖码不在卡券有效期内";
                    resCustomCardInfo.IfResult = "F";
                    resCustomCardInfo.ReduceCost = codeInfo.ReduceCost;
                    resCustomCardInfo.Tel = codeInfo.Tel;
                    resCustomCardInfo.CardRemark = codeInfo.CardRemark;
                    resCustomCardInfo.CardBeginDate = codeInfo.CardBeginDate;
                    resCustomCardInfo.CardEndDate = codeInfo.CardEndDate;
                    resCustomCardInfo.CardCategory = codeInfo.CardCategory;//根据需求添加此字段
                }
                result.Data = resCustomCardInfo;
                result.Message = "SUCCESS";
                return Ok(result);
                #endregion
            }
            catch (Exception)
            {
                result.IsSuccess = false;
                result.Message = "ERROR";
                return Ok(result);
            }
        }

        
        #endregion
    }
}