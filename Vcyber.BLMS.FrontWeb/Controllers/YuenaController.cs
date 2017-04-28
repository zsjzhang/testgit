using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.CarService;
using Vcyber.BLMS.Entity.Enum;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using AspNet.Identity.SQL;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.Common.City;
using System.Net.Http;
using Vcyber.BLMS.Common.Web;


namespace Vcyber.BLMS.FrontWeb.Controllers
{
    public class YuenaController : Controller
    {

        private ApplicationUserManager _userManager;
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
        public ActionResult IndexVerna(string from = "")
        {
            var agents = new string[] { "android", "iphone", "symbianos", "windows phone", "ipad", "ipod" };
            var userAgent = Request.UserAgent.ToLower();
            if (agents.Count(x => userAgent.Contains(x)) > 0)
            {
                return Redirect(string.Format("https://wx.bluemembers.com.cn/verna-yuena/base?from={0}", from));
            }

            //参加活动的人数
            //var count = _AppContext.RecommendApp.Count(activityType);
            //领券人数
            var obj = _AppContext.ActivityInfoApp.GetActivityInfoByName("刮刮卡-悦纳上市活动");
            //ViewBag.Count = _AppContext.WinningInfoApp.GetWinningsByWhere(obj.Id, " State = 1").Count();
            ViewBag.Count = _AppContext.WinningInfoApp.GetWinningsCount(obj.Id);
            return View(ViewBag.Count);
        }

        // GET: Yuena
        /// <summary> 
        /// 01  悦纳活动首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(string from = "")
        {
            if(!string.IsNullOrEmpty(from))
            {
                Session["baseFrom"]="yuena_"+from;
            }
            var agents = new string[] { "android", "iphone", "symbianos", "windows phone", "ipad", "ipod" };
            var userAgent = Request.UserAgent.ToLower();
            if (agents.Count(x => userAgent.Contains(x)) > 0)
            {
                return Redirect(string.Format("https://wx.bluemembers.com.cn/verna-yuena/base?from={0}", from));
            }
            if (DateTime.Now > DateTime.Parse("2016-10-18 00:00:00"))
            {
                return RedirectToAction("IndexVerna", "Yuena", new { from = from });
            }

            //参加活动的人数
            //var count = _AppContext.RecommendApp.Count(activityType);
            //领券人数
            var obj = _AppContext.ActivityInfoApp.GetActivityInfoByName("刮刮卡-悦纳上市活动");
            //ViewBag.Count = _AppContext.WinningInfoApp.GetWinningsByWhere(obj.Id, " State = 1").Count();
            ViewBag.Count = _AppContext.WinningInfoApp.GetWinningsCount(obj.Id);
            return View(ViewBag.Count);
        }

        public ActionResult SellPoint()
        {

            return View();
        }

        /// <summary>
        /// 活动介绍
        /// </summary>
        /// <returns></returns>
        public ActionResult Introduce()
        {
            return View();
        }
        /// <summary>
        /// 中奖名单
        /// </summary>
        /// <returns></returns>
        public ActionResult WinnersList(string phoneNumber)
        {
            ReturnResult result = new ReturnResult() { IsSuccess = true, Message = "成功获取中奖记录", Errors = 200 };
            //var activityIdsfist = _AppContext.ActivityInfoApp.GetActivityInfoByName("刮刮卡-悦纳上市活动").Id.ToString();
            var activityIdssecond = _AppContext.ActivityInfoApp.GetActivityInfoByName("摇一摇-悦纳上市活动").Id.ToString();
            string[] activityIds = { activityIdssecond };

            var winningList = _AppContext.WinningInfoApp.GetWinningsList(activityIds, "").ToList();
            //if (winningList == null)
            //{
            //    result.IsSuccess = false;
            //    result.Message = "未查询到中奖记录";
            //    result.Errors = 400;
            //    return Json(result, JsonRequestBehavior.AllowGet);
            //}
            //return Json(result, JsonRequestBehavior.AllowGet);
            return View(winningList);
        }
        /// <summary>
        /// 根据手机号码查询中奖信息
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        public JsonResult WinnersInfo(string phoneNumber)
        {
            ReturnResult result = new ReturnResult() { IsSuccess = true, Message = "成功获取中奖记录", Errors = 200 };
            //var activityIdsfist = _AppContext.ActivityInfoApp.GetActivityInfoByName("刮刮卡-悦纳上市活动").Id.ToString();
            var activityIdssecond = _AppContext.ActivityInfoApp.GetActivityInfoByName("摇一摇-悦纳上市活动").Id.ToString();
            string[] activityIds = { activityIdssecond };

            result.Data = _AppContext.WinningInfoApp.GetWinningsList(activityIds, phoneNumber);
            if (result.Data == null)
            {
                result.IsSuccess = false;
                result.Message = "未查询到中奖记录";
                result.Errors = 400;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 推荐好友
        /// </summary>
        /// <returns></returns>
        public ActionResult RecommendFriend()
        {
            if (!this.User.Identity.IsAuthenticated)
            {

                return RedirectToAction("LogOn", "Yuena", new
                {
                    //登录后
                    returnUrl = "/Yuena/RecommendFriend"
                });

            }
            return View();
        }
        /// <summary>
        /// 保存推荐信息
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="friends"></param>
        /// <returns></returns>
        public JsonResult SaveRecommendFriend(string userName, string friends)
        {
            ReturnResult result = new ReturnResult() { IsSuccess = true, Message = "推荐成功", Errors = 200 };

            string userId = string.Empty;
            if (this.User.Identity.IsAuthenticated)
            {
                userId = User.Identity.GetUserId();
            }
            var friendsList = friends.TrimEnd(',').Split(',');

            var query = new List<Recommend>();
            foreach (var item in friendsList)
            {
                var hashFriend = item.Split('-');

                var flag = _AppContext.RecommendApp.IsPhoneExist(hashFriend[1]);
                if (flag)
                {
                    result.Errors = "400"; result.Message = string.Format("手机号{0}已经被推荐，快去推荐其他小伙伴吧！", hashFriend[1]);
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                var obj = new Recommend()
                {
                    ActivityType = "悦纳上市活动",
                    Created = DateTime.Now,
                    UserName = userName,
                    UserId = userId,
                    Name = hashFriend[0],
                    PhoneNumber = hashFriend[1],
                    DataSource = WebUtils.Device(),
                };
                query.Add(obj);
            }

            //_AppContext.RecommendApp.Count("悦纳上市活动");
            //string smsContent = @;
            //var longUrl = string.Format("https://wx.bluemembers.com.cn/verna-yuena/testdrive/{0}?name={1}&from=smspc",userId,userName);
            var longUrl = string.Format("https://wx.bluemembers.com.cn/verna-yuena/base?page=/yuena/testdrive/{0}?name={1}&from=smspc", userId, Server.UrlEncode(userName));
            var shortUrl = ShortUrlHelper.Create(longUrl);
            foreach (var obj in query)
            {
                _AppContext.RecommendApp.Add(obj);

                //发送短信               
                //sms.SendSMS(obj.PhoneNumber, smsContent, false);
                //_AppContext.SMSApp.SendSMS(ESmsType.预约_预约成功, maintenanceEntity.Phone, new string[] { });
                _AppContext.SMSApp.SendSMS(obj.PhoneNumber, string.Format("尊敬的{0}，您的好友{1}推荐您参与【悦己 纳新】活动，即日起试驾“悦纳” 维新养护产品随时领，购车再享积分+定制礼盒！悦纳—智造新活力的精致座驾，还等什么？马上试驾！{2}", obj.Name, obj.UserName, shortUrl));
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// 查看优惠券
        /// </summary>
        /// <returns></returns>
        public ActionResult LookForCoupon()
        {
            return View();
        }
        /// <summary>
        /// 预约维保
        /// 获取预约车型
        /// </summary>
        /// <returns>车型列表</returns>
        public ActionResult ReservationMaintenance()
        {
            if (!this.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("LogOn", "Yuena", new
                {
                    //登录后
                    returnUrl = "/Yuena/ReservationMaintenance"
                });
            }

            IEnumerable<Vcyber.BLMS.Entity.Generated.CSBaseCar> _result = _AppContext.BaseCarApp.QueryCars(ECarSeriesType.Maintenance);
            return View(_result);

        }
        /// <summary>
        /// 预约维保方法
        /// 保存预约信息
        /// </summary>
        /// <param name="maintenanceEntity"></param>
        /// <returns></returns>
        public JsonResult ToReservationMaintenance(SonataServiceEntity maintenanceEntity)
        {
            ReturnResult result = new ReturnResult() { IsSuccess = true, Message = "预约成功", Errors = 200 };
            string userId = string.Empty;
            string userName = string.Empty;
            if (this.User.Identity.IsAuthenticated)
            {
                userId = User.Identity.GetUserId();
                userName = User.Identity.GetUserName();
            }

            maintenanceEntity.DataSource = WebUtils.Device();
            maintenanceEntity.UserId = userId;
            maintenanceEntity.UserName = userName;
            maintenanceEntity.OrderType = EBMServiceType.CommonMaintain;
            //判断此车型是不预约过
            QueryParamEntity par = new QueryParamEntity();
            par.ScheduleFromDate = maintenanceEntity.ScheduleDate;
            par.ScheduleToDate = maintenanceEntity.ScheduleDate;
            par.LicensePlate = maintenanceEntity.LicensePlate;
            var list = _AppContext.SonataServiceApp.QueryOrders(par, 1, 100);
            if (list.Items.Count > 0)
            {
                result.IsSuccess = false;
                result.Errors = 400;
                result.Message = "您已经预约过，请耐心等待，经销商会主动与您取得联系。";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            int _result = _AppContext.SonataServiceApp.Add(maintenanceEntity);
            if (_result <= 0)
            {
                result.IsSuccess = false;
                result.Errors = 400;
                result.Message = "预约失败。";
            }
            else
            {
                _AppContext.SMSApp.SendSMS(ESmsType.预约_预约成功, maintenanceEntity.Phone, new string[] { });
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 预约试驾
        /// </summary>
        /// <returns></returns>
        public ActionResult ReserveDriver()
        {

            return View();
        }
        /// <summary>
        /// 预约试驾方法
        /// </summary>
        /// <param name="reserveDriverEndity"></param>
        /// <returns></returns>
        public JsonResult ReserveDriver(TestDriveEntity reserveDriverEndity)
        {
            ReturnResult result = new ReturnResult() { IsSuccess = true, Message = "成功预约。", Errors = 200 };

            if (reserveDriverEndity == null)
            {
                result.Errors = 400;
                result.Message = "请输入试驾信息";
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            if (string.IsNullOrEmpty(reserveDriverEndity.Phone))
            {
                result.Errors = 400;
                result.Message = "请输入手机号码";
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            if (string.IsNullOrEmpty(reserveDriverEndity.UserName))
            {
                result.Errors = 400;
                result.Message = "请输入姓名";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            if (reserveDriverEndity.ScheduleDate == null || reserveDriverEndity.ScheduleDate.Value < DateTime.Parse(DateTime.Now.ToShortDateString()))
            {
                result.Errors = 400;
                result.Message = "请正确填写试驾日期";
                return Json(result, JsonRequestBehavior.AllowGet);
                //return Json(new { code = 400, msg = "请正确填写试驾日期" }, JsonRequestBehavior.AllowGet);
            }

            string userId = string.Empty;
            if (this.User.Identity.IsAuthenticated)
            {
                userId = User.Identity.GetUserId();
            }
            reserveDriverEndity.UserId = userId;
            reserveDriverEndity.DataSource = "Yuena-PC";
            int _result = _AppContext.TestDriveApp.Add(reserveDriverEndity);
            if (_result <= 0)
            {
                result.IsSuccess = false;
                result.Errors = 400;
                result.Message = "预约失败，请重新预约。";
            }
            else
            {
                _AppContext.SMSApp.SendSMS(ESmsType.预约_预约成功, reserveDriverEndity.Phone, new string[] { });
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 02 用户登录页面
        /// 点击首页立即领取后转到此视图
        /// </summary>
        /// <returns></returns>
        public ActionResult LogOn()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("LoginAfter", "Yuena");
            }
            return View();
        }

        /// <summary>
        /// 刮刮卡获得奖券
        /// </summary>
        /// <returns></returns>
        public ActionResult GetCoupons()
        {
            if (!this.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("LogOn", "Yuena", new
                {
                    //登录后
                    returnUrl = "/Yuena/GetCoupons"
                });
            }
            ViewBag.Count = _AppContext.RecommendApp.Count("悦纳上市活动");

            return View();
        }


        /// <summary>
        /// 检查是否已刮过卡券
        /// </summary>
        /// <returns></returns>
        public JsonResult IsHaveCoupon()
        {
            ReturnResult result = new ReturnResult() { IsSuccess = true, Message = "是车主拿到中奖信息", Errors = 200 };
            var userId = User.Identity.GetUserId();
            //进入页面后判断是否认是车主：是车主，直接去抽奖,发奖；不是车主，去认证并抽奖
            var obj = _AppContext.ActivityInfoApp.GetActivityInfoByName("刮刮卡-悦纳上市活动");
            var winningInfo = _AppContext.WinningInfoApp.GetWinningByUserIdAndActicityId(obj.Id, userId);
            //WinningInfo
            if (winningInfo != null)
            {
                result.Data = winningInfo;
            }
            else
            {
                result.IsSuccess = false;
                result.Message = "未领取过卡券";
                result.Errors = 400;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 进入领取卡券页面，直接给用户发奖，给一条获奖记录
        /// 进入页面即发奖，但置为未领取状态
        /// 一重礼 保养卡券免费领
        /// </summary>
        /// <returns></returns>
        public JsonResult GetCoupon()
        {
            ReturnResult result = new ReturnResult() { IsSuccess = true, Message = "成功获取卡券", Errors = 200 };
            string userId = string.Empty;
            string userName = string.Empty;
            if (this.User.Identity.IsAuthenticated)
            {
                userId = User.Identity.GetUserId();
                userName = User.Identity.GetUserName();

            }
            PrizesInfo prize = new PrizesInfo();
            var obj = _AppContext.ActivityInfoApp.GetActivityInfoByName("刮刮卡-悦纳上市活动");
            //进入页面发奖，但置为未领取状态
            var winningInfo = _AppContext.WinningInfoApp.GetWinningByUserIdAndActicityId(obj.Id, userId);
            //已有获奖记录
            if (winningInfo != null)
            {
                //有发奖记录 已领取
                if (winningInfo.State == 1)
                {
                    result.Errors = 400;
                    result.Data = winningInfo;
                }
                //有发奖记录 未领取
                if (winningInfo.State == 0)
                {
                    prize = _AppContext.PrizesInfoApp.GetPrizeInfoMode(winningInfo.PrizesId);
                    result.Data = prize;
                }
            }
            if (winningInfo == null)
            {
                prize = _AppContext.ActivityInfoApp.NormalDraw(obj.Id, userId);
                result.Data = prize;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 点击刮卡，更改获奖状态（领取状态），同时给用户名下发送卡券
        /// </summary>
        /// <returns></returns>
        public JsonResult GetTowConpon(string activityId, string prizesId)
        {
            ReturnResult result = new ReturnResult() { IsSuccess = true, Message = "成功获取卡券", Errors = 200 };
            string userId = string.Empty;
            string userName = string.Empty;
            if (this.User.Identity.IsAuthenticated)
            {
                userId = User.Identity.GetUserId();
                userName = User.Identity.GetUserName();

            }
            //判断是否已领奖  领奖则停止返回
            int activityID = int.Parse(activityId);
            int prizeID = int.Parse(prizesId);
            var winningInfo = _AppContext.WinningInfoApp.GetWinningByUserIdAndActicityId(activityID, userId);
            if (winningInfo.State == 1)
            {
                result.IsSuccess = false;
                result.Errors = 400;
                result.Message = "";
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            //更改中奖状态
            var obj = new WinningInfo()
            {
                ActivityId = activityID,
                UserId = userId,
                PrizesId = prizeID,
                UserName = userName,
                UserTel = userName,
            };
            var flag = _AppContext.WinningInfoApp.UpdateForUserId(obj);

            //发卡券给用户名下添加卡券
            var sendCard = _AppContext.WinningInfoApp.GetWinPrize(activityID, userId, prizeID);

            var cardTypes = new List<string>();
            if (sendCard != null)
            {
                //取奖品
                var prizeObj = _AppContext.PrizesInfoApp.GetPrizeInfoMode(obj.PrizesId);
                switch (prizeObj.PrizeLevel)
                {
                    case 1:
                        cardTypes.Add("悦纳送您的50元保养券");
                        cardTypes.Add("试乘试驾礼品券");
                        break;
                    case 2:
                        cardTypes.Add("机油滤清器兑换券");
                        cardTypes.Add("试乘试驾礼品券");
                        break;
                    case 3:
                        cardTypes.Add("2瓶玻璃水兑换券");
                        cardTypes.Add("试乘试驾礼品券");
                        break;
                }
                foreach (var cardType in cardTypes)
                {
                    _AppContext.ProductApp.SendCard(userId, "悦己纳新-售后服务活动", cardType, 1, "blms_yuena_web");
                }
            }
            result.Data = cardTypes;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 登陆后页面
        /// </summary>
        /// <returns></returns>
        public ActionResult LoginAfter()
        {
            if (!this.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("LogOn", "Yuena", new
                {
                    //登录后
                    returnUrl = "/Yuena/LoginAfter"
                });
            }

            //判断是否已刮卡获得过礼品券
            string userName = this.User.Identity.GetUserName();
            var userId = User.Identity.GetUserId();
            var obj = _AppContext.ActivityInfoApp.GetActivityInfoByName("刮刮卡-悦纳上市活动");
            var winningInfo = _AppContext.WinningInfoApp.GetWinningByUserIdAndActicityId(obj.Id, userId);
            
            
            if (winningInfo == null || winningInfo.State == 0)
            {
                //未刮卡跳到领奖
                return RedirectToAction("GetCoupons", "Yuena");
            }
            else if(winningInfo != null)
            {
                var prizeInfo = _AppContext.PrizesInfoApp.GetPrizeInfoMode(winningInfo.PrizesId);
                ViewBag.PrizeLevel = prizeInfo.PrizeLevel;
            }
            
            return View();
        }
        /// <summary>
        /// 车主认证
        /// </summary>
        /// <returns></returns>
        public ActionResult CarOwerAuthentication()
        {
            if (!this.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("LogOn", "Yuena", new
                {
                    //登录后
                    returnUrl = "/Yuena/CarOwerAuthentication"
                });
            }

            return View();
        }
        /// <summary>
        /// 检查是否抽过奖 自拍杆 旅行四件套 
        /// </summary>
        /// <returns></returns>
        public JsonResult CheckHavePrize()
        {
            ReturnResult result = new ReturnResult() { IsSuccess = true, Message = "是车主拿到中奖信息", Errors = 200 };
            var userId = User.Identity.GetUserId();
            PrizesInfo prize = new PrizesInfo();
            //进入页面后判断是否认是车主：是车主，直接去抽奖,发奖；不是车主，去认证并抽奖
            var account = new FrontUserStore<FrontIdentityUser>().FindByIdAsync(userId);
            if (account.Result.SystemMType > 1)
            {
                var obj = _AppContext.ActivityInfoApp.GetActivityInfoByName("摇一摇-悦纳上市活动");
                //prize= _AppContext.ActivityInfoApp.NormalDraw(obj.Id, userId);
                //result.Data = prize;
                //判断是否已给用户发过奖
                var winningInfo = _AppContext.WinningInfoApp.GetWinningByUserIdAndActicityId(obj.Id, userId);
                //有发奖记录 并且已经领过奖
                if (winningInfo != null)
                {
                    //有发奖记录 已领取
                    if (winningInfo.State == 1)
                    {
                        result.IsSuccess = false;
                        result.Message = "是车主且已领过奖";
                        result.Data = winningInfo;
                        result.Errors = 401;
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                    //有发奖记录 未领取
                    if (winningInfo.State == 0)
                    {
                        prize = _AppContext.PrizesInfoApp.GetPrizeInfoMode(winningInfo.PrizesId);
                        result.IsSuccess = false;
                        result.Message = "是车主但未领过奖";
                        result.Errors = 402;
                        result.Data = prize;
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                //是车主 但没有发奖记录
                if (winningInfo == null)
                {

                    prize = _AppContext.ActivityInfoApp.NormalDraw(obj.Id, userId);
                    result.Errors = 200;
                    result.Data = prize;
                    result.Message = "是车主但没有发奖记录";
                }
            }
            else
            {
                result.Data = "/Yuena/CarOwerAuthentication";
                result.IsSuccess = false;
                result.Message = "不是车主";
                result.Errors = 400;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 认证车主 抽奖
        /// 点击认车主的同时给用户发奖
        /// </summary>
        /// <param name="identityNumber"></param>
        /// <param name="mtype"></param>
        /// <returns></returns>
        public JsonResult ToCheckCarownerSave(string identityNumber, int mtype)
        {
            ReturnResult result = new ReturnResult() { IsSuccess = true, Message = "车主认证成功。", Errors = 200 };
            var store = new FrontUserStore<FrontIdentityUser>();
            var prize = new PrizesInfo();
            //判断身份证是否合法
            string birthday = string.Empty;
            if (mtype == (int)Vcyber.BLMS.Entity.Enum.ECustomerIdentificationType.IdentityCard)
            {
                switch (identityNumber.Length)
                {
                    case 15:
                        birthday = "19" + identityNumber.Substring(6, 2) + "-" + identityNumber.Substring(8, 2) + "-" + identityNumber.Substring(10, 2);
                        break;
                    case 18:
                        birthday = identityNumber.Substring(6, 4) + "-" + identityNumber.Substring(10, 2) + "-" + identityNumber.Substring(12, 2);
                        break;
                    default:
                        //return Json(new { code = 304, msg = "请正确输入证件号码" });
                        result.IsSuccess = false;
                        result.Message = "请正确输入证件号码";
                        result.Errors = 304;
                        return Json(result, JsonRequestBehavior.AllowGet);
                }
                DateTime time;
                if (!DateTime.TryParse(birthday, out time))
                {
                    result.IsSuccess = false;
                    result.Message = "请正确输入证件号码";
                    result.Errors = 304;
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            string userName = this.User.Identity.GetUserName();
            var gradeUser = UserManager.FindByName(userName);
            gradeUser.PaperWork = mtype.ToString();
            gradeUser.IdentityNumber = identityNumber;
            //获取用户车辆信息
            var carList = _AppContext.CarServiceUserApp.SelectCarListByIdentity(identityNumber);
            //var returnintegral = (int)_AppContext.CarServiceUserApp.GetReIntegralTypeByIdentity(gradeUser.IdentityNumber);
            if (store.IsIdentityNumberRepeate(identityNumber))
            {
                //根据身份证获取用户对象
                var userModel = store.FindByIdentityNumber(identityNumber).Result;
                if (userModel == null || userModel.SystemMType == 2)
                {
                    // string phone = userModel.PhoneNumber.Substring(3, 4);  如果要需求此行注释注意非空判断
                    //return Json(new { code = 301, msg = CommonConst.NOCARTIP });
                    result.IsSuccess = false;
                    result.Message = CommonConst.NOCARTIP;
                    result.Errors = 301;
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            if (carList == null || carList.Count() <= 0)
            {
                //提示未匹配到您的车辆，请拨打客服电话
                //return Json(new { code = 301, msg = CommonConst.NOCARTIP });
                result.IsSuccess = false;
                result.Message = CommonConst.NOCARTIP;
                result.Errors = 301;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else//车主认证成功 抽奖
            {
                //查到车时增加车主认证时间
                gradeUser.AuthenticationTime = DateTime.Now;
                gradeUser.AuthenticationSource = "blms_pc_web_yuena";
                //抽奖( 拿蓝牙自拍杆，旅行四件套)：
                //（1）根据活动活动名称拿到活动ID（2）根据活动ID，用户ID返回获得的奖品
                var obj = _AppContext.ActivityInfoApp.GetActivityInfoByName("摇一摇-悦纳上市活动");
                prize = _AppContext.ActivityInfoApp.NormalDraw(obj.Id, gradeUser.Id);
            }
            gradeUser.MLevel = Convert.ToInt32(_AppContext.DealerMembershipApp.GetFirstRegistMLevelByIdNumber(identityNumber));
            gradeUser.SystemMType = (int)MembershipType.WhitCar;
            if (!string.IsNullOrEmpty(birthday))
            {
                gradeUser.Birthday = birthday;
            }
            //判断交费级别
            UserManager.Update(gradeUser);
            var userStore = new FrontUserTable<FrontIdentityUser>();
            userStore.AddPaperworkToMembership_Schedule(gradeUser);
            //if (returnintegral != -1)
            //{
            //    //return Json(new { code = 302, msg = "缴费返积分", reintegralType = returnintegral });
            //    result.IsSuccess = true;
            //    result.Message = "缴费返积分";
            //    result.Errors = 302;
            //    result.Data = returnintegral;
            //    return Json(result, JsonRequestBehavior.AllowGet);
            //}
            //return Json(new { code = 200, msg = "车主认证成功" });
            result.IsSuccess = true;
            result.Message = "车主认证成功";
            result.Errors = 200;
            result.Data = prize;

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 弹出窗中点击立即提交 更改领奖信息
        /// 领取奖品 更改其领奖状态，收货地址等
        /// 获取自拍杆 旅行四件套
        /// </summary>
        /// <param name="activityId"></param>
        /// <param name="prizesId"></param>
        /// <param name="userTel"></param>
        /// <param name="province"></param>
        /// <param name="city"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        public JsonResult GetPrize(int activityId, int prizesId, string userTel, string userName, string province, string city, string address)
        {

            ReturnResult result = new ReturnResult() { IsSuccess = true, Message = "成功获领奖", Errors = 200 };
            string userId = string.Empty;
            //string userName = string.Empty;
            if (this.User.Identity.IsAuthenticated)
            {
                userId = User.Identity.GetUserId();
                //userName = User.Identity.GetUserName();

            }
            //更改中奖状态
            var obj = new WinningInfo()
            {
                ActivityId = activityId,
                UserId = userId,
                PrizesId = prizesId,
                UserName = userName,
                UserTel = userTel,
                Province = province,
                City = city,
                Address = address
            };
            var flag = _AppContext.WinningInfoApp.UpdateForUserId(obj);
            if (!flag)
            {
                result.IsSuccess = false;
                result.Message = "领取奖品失败";
                result.Errors = 400;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 卡券使用方式 介绍
        /// </summary>
        /// <returns></returns>
        public ActionResult CouponsUserMethod()
        {
            return View();
        }
        /// <summary>
        /// 推荐奖励 说明
        /// </summary>
        /// <returns></returns>
        public ActionResult RecommendRewards()
        {
            return View();
        }
        #region 预约维保获得省份城市经销商列表
        /// <summary>
        /// 获取经销商省份
        /// </summary>
        /// <param name="IsWeibao"></param>
        /// <param name="Istestserver"></param>
        /// <param name="IsDingChe"></param>
        /// <returns></returns>
        public ActionResult DealerProvince(int? IsWeibao = 0, int? Istestserver = 0, int? IsDingChe = 0)
        {
            ViewBag.IsWeibao = IsWeibao;
            ViewBag.Istestserver = Istestserver;
            ViewBag.IsDingChe = IsDingChe;
            IEnumerable<string> _provinces = _AppContext.DealerApp.GetProvinceList();
            return View(_provinces);
        }
        /// <summary>
        /// 根据省获取省下的市
        /// </summary>
        /// <param name="provinceId"></param>
        /// <returns></returns>
        public JsonResult Citys(string provinceValue)
        {
            IList<string> _result = new List<string>();
            IEnumerable<string> _citys = _AppContext.DealerApp.GetCityListByProvince(provinceValue);
            if (_citys != null && _citys.Any())
            {
                _result = _citys.ToList();
            }
            return Json(_result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 根据城市获取供应商
        /// </summary>
        /// <param name="cityValue"></param>
        /// <returns></returns>
        public JsonResult Dealers(string cityValue, string provinceValue)
        {
            IList<Vcyber.BLMS.Entity.Generated.CSCarDealerShip> _result = new List<Vcyber.BLMS.Entity.Generated.CSCarDealerShip>();
            IEnumerable<Vcyber.BLMS.Entity.Generated.CSCarDealerShip> _dealers = _AppContext.DealerApp.GetDealerList(provinceValue, cityValue);
            if (_dealers != null && _dealers.Any())
            {
                _result = _dealers.ToList();
            }
            return Json(_result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 自拍杆等奖品的收货地址省市列表
        /// <summary>
        /// 收货地址
        /// </summary>
        /// <returns></returns>
        public ActionResult GetPrizeAddress()
        {
            IEnumerable<Address> _result = null;
            //if (this.User.Identity.IsAuthenticated && !string.IsNullOrEmpty(userId))
            //{
            //    _result = _AppContext.AddressApp.GetList(userId);
            //}

            ViewData["provinceList"] = Vcyber.BLMS.Common.City.CityService.Instance.GetProvince();

            //ViewBag.userId = userId;
            return View(_result);
        }

        /// <summary>
        /// 根据省份获取城市
        /// </summary>
        /// <param name="provinceCode"></param>
        /// <returns></returns>
        public JsonResult FindCitysByProvince(string provinceCode)
        {
            List<City> _cityList = Vcyber.BLMS.Common.City.CityService.Instance.GetCity(provinceCode);
            return Json(_cityList, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 根据城市获取地区
        /// </summary>
        /// <param name="cityCode"></param>
        /// <returns></returns>
        public JsonResult FindAreasByCity(string cityCode)
        {
            IList<Area> _areaList = Vcyber.BLMS.Common.City.CityService.Instance.GetArea(cityCode);
            return Json(_areaList, JsonRequestBehavior.AllowGet);
        }
        #endregion


        /// <summary>
        /// 判断手机号是否重复
        /// </summary>
        /// <param name="phone">手机号</param>       
        public ActionResult IsPhoneExist(string phone)
        {
            var flag = _AppContext.RecommendApp.IsPhoneExist(phone);
            return Json(flag, JsonRequestBehavior.AllowGet);
        }

    }




    #region 生成短连接
    public class ShortUrlHelper
    {
        public static string Create(string url)
        {
            HttpClient client = new HttpClient();
            var content = new FormUrlEncodedContent(new Dictionary<string, string>()
                {
                    {"source", "1681459862"},
                    {"url_long", url},
                });
            HttpResponseMessage response = client.PostAsync("http://api.t.sina.com.cn/short_url/shorten.json", content).Result;
            string result = response.Content.ReadAsStringAsync().Result;
            var query = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ShortUrlResult>>(result);
            var shortUrl = string.Empty;
            if (query.Count > 0)
            {
                shortUrl = query.First().url_short;
            }
            return shortUrl;
        }
    }

    public class ShortUrlResult
    {
        public string url_short { get; set; }
        public string url_long { get; set; }
        public string type { get; set; }
    }


    #endregion
}
