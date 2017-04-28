using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Entity;
using VcyBer.BLMS.MobileWeb.Models;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.Common;
using System.Configuration;

namespace VcyBer.BLMS.MobileWeb.Controllers
{
    public class MyCenterController : BaseController
    {
        #region ==== 私有字段 ====

        private ApplicationUserManager _userManager;

        #endregion

        #region ==== 公共属性 ====

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        #endregion

        /// <summary>
        /// 个人中心首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            //var cookieValue = CookieHelper.GetCookieValue("CustomCookie");
            //if (!string.IsNullOrWhiteSpace(cookieValue) && cookieValue.ToLower().Contains("webinspect"))
            //{
            //    return Redirect("/Contents/error.htm");
            //}

            //判断是否登录
            if (!this.User.Identity.IsAuthenticated)
            {
                //return RedirectToAction("Login", "Account");
                return Redirect("/Account/Login?url=" + Request.RawUrl);
            }

            //获取当前登录人Id
            var userId = this.User.Identity.GetUserId();

            //根据当前登录用户id获取当前用户信息
            var model = _AppContext.DealerMembershipApp.GetMembershipMsgByUserId(userId);

            CarTypeReturnIntegral CarTypeReturnIntegral = _AppContext.CarServiceUserApp.GetReIntegralTypeByIdentity(model.IdentityNumber);
            //判断是否是个人用户
            bool flag = _AppContext.DealerMembershipApp.IsPersonalUser(model.IdentityNumber);
            if (!flag)
            {
                CarTypeReturnIntegral = CarTypeReturnIntegral.NoIntegral;
            }

            //获取当前登录人的总积分
            var count = _AppContext.UserIntegralApp.GetUserintegralByUserId(userId);

            //省会
            IEnumerable<string> _provinces = _AppContext.DealerApp.GetProvinceList();
            ViewBag.Provinces = _provinces;

            //该用户下的车信息
            var carList = _AppContext.CarServiceUserApp.CarsByUserId(userId).ToList<Car>();

            //获取未读消息数
            var noMsgNum = _AppContext.UserMessageRecordApp.GetUnReadMessage(userId);

            ViewBag._userID = userId;//用户id
            ViewBag.NonReadMsgcount = noMsgNum;//未读消息数
            ViewBag.Integral = count.value;
            ViewBag.CarTypeReturnIntegral = ((int)CarTypeReturnIntegral).ToString();
            ViewBag.listCarInfo = carList;
            return View(model);
        }
        /// <summary>
        /// 专属服务
        /// </summary>
        /// <returns></returns>
        public ActionResult ExclusiveService()
        {
            return View();
        }
        /// <summary>
        /// 卡券
        /// </summary>
        /// <returns></returns>
        public ActionResult MyCard(string type)
        {
            string userId = this.User.Identity.GetUserId();
            MyCard myCard = new Models.MyCard();
            switch (type)
            {
                case "1":
                default:
                    {
                        myCard.BmCardList = new UserCustomCardModel()
                        {
                            ReceivedCustomCardList = _AppContext.CustomCardApp.GetUserCustomCardList(userId, 1, 1).ToList(),
                            UsedCustomCardList = _AppContext.CustomCardApp.GetUserCustomCardList(userId, 1, 2).ToList(),
                            ExpiredCustomCardList = _AppContext.CustomCardApp.GetUserCustomCardList(userId, 1, 3).ToList()
                        };
                        break;
                    }
                case "2":
                    {
                        var sessionCode = Session["CardValidCode" + userId];
                        if (sessionCode == null || string.IsNullOrWhiteSpace(sessionCode.ToString()))
                        {
                            return Redirect("/myCenter/myCard?type=1");
                        }
                        myCard.PartnerCardList = new UserCustomCardModel()
                        {
                            ReceivedCustomCardList = _AppContext.CustomCardApp.GetUserCustomCardList(userId, 2, 1).ToList(),
                            UsedCustomCardList = _AppContext.CustomCardApp.GetUserCustomCardList(userId, 2, 2).ToList(),
                            ExpiredCustomCardList = _AppContext.CustomCardApp.GetUserCustomCardList(userId, 2, 3).ToList()
                        };
                        break;
                    }
                case "3":
                    {
                        //候机服务券(未使用)
                        myCard.FlightNoUseCardList = _AppContext.CustomCardApp.GetTerminalservicevoucherCardList(userId, 0, 2).ToList();
                        //候机服务券(已使用)
                        myCard.FlightCardUseList = _AppContext.CustomCardApp.GetTerminalservicevoucherCardList(userId, 0, 3).ToList();
                        //候机服务券(已过期)
                        myCard.FlightCardOverUseList = _AppContext.CustomCardApp.GetTerminalservicevoucherCardList(userId, 1, 2).ToList();
                        break;
                    }
            }
            var user = UserManager.FindById(userId);
            ViewBag.Phone = user.PhoneNumber.Substring(0, 3) + "****" + user.PhoneNumber.Substring(7, 4);
            ViewBag.Type = type;
            return View(myCard);
        }
        /// <summary>
        /// 北京现代、合作商卡券详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult MyCardDetail(string id)
        {
            string userId = this.User.Identity.GetUserId();
            var info = _AppContext.CustomCardInfoApp.GetSingleUserCustomCardInfoByIdAndUserId(id, userId);
            info.CardLogoUrl = ConfigurationManager.AppSettings["cardLogoUrlRoot"].ToString() + info.CardLogoUrl;
            return View(info);
        }
        /// <summary>
        /// 候机服务卡券详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult MyCardFightAwaitDetail(string id)
        {
            //获取单个候机服务券详情
            var entity = _AppContext.AirportServiceApp.GetCardByUserDetails(this.User.Identity.GetUserId(), id);
            return View(entity);
        }
        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <returns></returns>
        public JsonResult SendCodeMyCard()
        {
            //成功弹框，不成功提示
            string userId = this.User.Identity.GetUserId();
            var user = UserManager.FindById(userId);

            var sessionCode = Session["CardValidCode" + userId];

            if (sessionCode == null)
            {
                return Json(SendCode(userId, user.PhoneNumber));
            }
            else
            {
                var sessionCodeSplit = sessionCode.ToString().Split(',');
                if (sessionCodeSplit[1] == "1")
                {
                    return Json(new ReturnResult() { IsSuccess = true, Message = "已验证" });
                }
                else
                {
                    if (((int)(DateTime.Now - DateTime.Parse(sessionCodeSplit[2])).TotalSeconds) > 180)
                    {
                        return Json(SendCode(userId, user.PhoneNumber));
                    }
                    //未验证
                    return Json(new ReturnResult() { IsSuccess = true, Message = "" });
                }
            }
        }
        /// <summary>
        /// 验证验证码是否正确
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public JsonResult ValidSendCode(string code)
        {
            string userId = this.User.Identity.GetUserId();
            var sessionCode = Session["CardValidCode" + userId];
            if (sessionCode != null)
            {
                var sessionCodeSplit = sessionCode.ToString().Split(',');
                if (code != null && code == sessionCodeSplit[0])
                {
                    if (((int)(DateTime.Now - DateTime.Parse(sessionCodeSplit[2])).TotalSeconds) > 180)
                    {
                        return Json(new { IsSuccess = false, Message = "验证码有效期已过，请重新发送" });
                    }
                    Session["CardValidCode" + userId] = string.Format(string.Format("{0},1,{1}", code, sessionCodeSplit[2]));
                    return Json(new { IsSuccess = true, Message = "" });
                }
            }
            return Json(new { IsSuccess = false, Message = "验证码错误！" });
        }
        /// <summary>
        /// 获取二维码
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public FileResult GetQCode(string id)
        {
            return File(new QrCodeHelper().GetRCode(id), "image/jpg");
        }
        private ReturnResult SendCode(string userId, string phoneNumber)
        {
            int countDaylimit = _AppContext.ValidateCodeApp.GetCount(phoneNumber);
            if (countDaylimit >= 5)
            {
                return new ReturnResult() { IsSuccess = false, Message = "超出今日验证码请求次数，请联系客服或管理员" };
            }
            var code = new Random().Next(1000, 9999).ToString();//验证码
            Session["CardValidCode" + userId] = string.Format(string.Format("{0},0,{1}", code, DateTime.Now.ToString()));
            var message = string.Format("验证码为{0}，您正在查看蓝缤服务“我的卡券”。请您3分钟内完成验证，切勿将验证码告知他人。", code);
            var result = _AppContext.SMSApp.SendSMS(phoneNumber, message);
            if (result.IsSuccess)
            {
                if (_AppContext.UserSecurityApp.SaveMobileVerifyCode(phoneNumber, code, "blms_wap"))
                {
                    return new ReturnResult() { IsSuccess = true, Message = "" };
                }
                else
                {
                    return new ReturnResult() { IsSuccess = false, Message = "服务器异常！" };
                }
            }
            return new ReturnResult() { IsSuccess = false, Message = result.Message };
        }
    }
}