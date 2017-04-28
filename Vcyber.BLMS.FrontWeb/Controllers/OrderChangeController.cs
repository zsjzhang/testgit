using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Entity;
using Microsoft.AspNet.Identity;
using Vcyber.BLMS.FrontWeb.Models;
using Microsoft.AspNet.Identity.Owin;
using Vcyber.BLMS.Common.City;
using System.Configuration;
using AspNet.Identity.SQL;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.Entity.Common;
using Vcyber.BLMS.Entity.Enum;

namespace Vcyber.BLMS.FrontWeb.Controllers
{
    public class OrderChangeController : Controller
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
        private int activityId = int.Parse(ConfigurationManager.AppSettings["ActivityId"]);
        public OrderChangeController() { }

        public ActionResult ExChange()
        {
            return View();
        }

        #region 置换专区
        /// <summary>
        /// 置换政策
        /// </summary>
        /// <returns></returns>
        public ActionResult ExChangePolicy()
        {
            int activityid = activityId;
            IEnumerable<XDLotteryRecord> lstLotteryRecord = _AppContext.XDLotteryRecordApp.GetXDLotteryRecordList(activityid, 0, 0);
            var result = lstLotteryRecord.Where(o => o.LotteryName != "谢谢惠顾");

            return View(result);
        }
        /// <summary>
        /// 置换活动
        /// </summary>
        /// <returns></returns>
        public ActionResult ExChangeActivity(int id=0,int searchActivityId=0,string phone="")
        {
            IEnumerable<XDActivity> activityList = _AppContext.XDActivityApp.GetXDActivityList(2);//获取预约置换列表

            XDActivity searchActivity = _AppContext.XDActivityApp.GetXDActivityList(2, searchActivityId).FirstOrDefault();
            var topActivity = searchActivity == null ? activityList.FirstOrDefault() : searchActivity;//获取最新的一条置换活动

            IEnumerable<XDLotteryRecord> lstLotteryRecord = _AppContext.XDLotteryRecordApp.GetXDLotteryRecordList(id == 0 ? topActivity.ActivityId : id, 0, 0);//获取中奖纪录

            var result = phone == "" ? lstLotteryRecord.Where(o => o.LotteryName != "谢谢惠顾") : lstLotteryRecord.Where(o => o.LotteryName != "谢谢惠顾").Where(u => u.UserMobile == phone);

            ViewBag.topActivity = topActivity;
            ViewBag.activityList = activityList;

            return View(result);
        }
        /// <summary>
        /// 某个置换活动的详情
        /// </summary>
        /// <returns></returns>
        public ActionResult ExChangeActivityDetail(int activityId,string phone="")
        {
            IEnumerable<XDActivity> activityList = _AppContext.XDActivityApp.GetXDActivityList(2);

            XDActivity activityDetail = _AppContext.XDActivityApp.GetXDActivityById(activityId);

            IEnumerable<XDLotteryRecord> lstLotteryRecord = _AppContext.XDLotteryRecordApp.GetXDLotteryRecordList(activityId == 0 ? activityDetail.ActivityId : activityId, 0, 0);//获取中奖纪录
            var result = phone == "" ? lstLotteryRecord.Where(o => o.LotteryName != "谢谢惠顾") : lstLotteryRecord.Where(o => o.LotteryName != "谢谢惠顾").Where(u => u.UserMobile == phone);

            ViewBag.ActivityDetail = activityDetail;
            ViewBag.activityList = activityList;

            return View(result);
        }
            


        #endregion

        public ActionResult Inviter()
        {
            return View();
        }
        public ActionResult ExChangeLottery()
        {
            int activityId = 1;
            IEnumerable<XDLotteryRecord> lstLotteryRecord = _AppContext.XDLotteryRecordApp.GetXDLotteryRecordList(activityId, 1, 0);
            return View(lstLotteryRecord);
        }
        public ActionResult InviterLottery()
        {
            int activityId = 1;
            IEnumerable<XDLotteryRecord> lstLotteryRecord = _AppContext.XDLotteryRecordApp.GetXDLotteryRecordList(activityId, 2, 0);
            return View(lstLotteryRecord);
        }
        /// <summary>
        /// 添加预约置换
        /// </summary>
        /// <param name="xDOrderChange"></param>
        /// <returns></returns>
        public JsonResult AddOrderChange(XDOrderChange xDOrderChange)
        {
            try
            {
                xDOrderChange.ActivityId = activityId;
                xDOrderChange.CreaterId = "0";
                xDOrderChange.CreaterName = xDOrderChange.Name;
                xDOrderChange.CreaterTime = DateTime.Now;
                xDOrderChange.UpdaterId = "0";
                xDOrderChange.UpdaterName = xDOrderChange.Name;
                xDOrderChange.UpdaterTime = DateTime.Now;
                xDOrderChange.OrderChangeTime = DateTime.Now;
                xDOrderChange.IsValid = 1;
                bool isExistLottery = _AppContext.XDLotteryRecordApp.IsExistLotteryRecordByUserMobile(xDOrderChange.Mobile, activityId, 0);
                if (_AppContext.XDOrderChangeApp.IsCanOrderChange(xDOrderChange.CarSeriers,xDOrderChange.Mobile,xDOrderChange.ShopCode))
                {
                    return Json(new { code = "0", msg = "您已经预约过，请耐心等待，经销商会主动与您取得联系！" });
                }
                bool result = _AppContext.XDOrderChangeApp.AddOrderChange(xDOrderChange);
                if (!result)
                {
                    return Json(new { code = "0", msg = "预约置换失败！" });
                }
                else
                 {
                    if (isExistLottery)
                    {
                        return Json(new { code = "201", msg = "已抽奖！" });
                    }
                    else
                    {
                        return Json(new { code = "200", msg = "恭喜您，您的预约申请已成功提交，北京现代特约店稍后将与您取得联系，请您保持手机畅通。" });
                    }
                }
            }
            catch (Exception)
            {
                return Json(new { code = "0", msg = "预约置换异常！" });
            }
        }
        public JsonResult GetOrderChange(int activityId, string mobile)
        {
            try
            {
                XDOrderChange xDOrderChange = _AppContext.XDOrderChangeApp.GetOrderChangeByMobile(activityId, mobile);
                return Json(new { code = "1", msg = "获取预约置换！", data = xDOrderChange });
            }
            catch (Exception)
            {
                return Json(new { code = "0", msg = "获取预约置换异常！" });
            }
        }
        public JsonResult AddXDInviter([ModelBinder(typeof(GenericModelBinder<XDInviters>))]XDInviters inviters)
        {
            try
            {
                string userId = string.Empty;
                if (this.User.Identity.IsAuthenticated)
                {
                    userId = this.User.Identity.GetUserId();
                }
                if (string.IsNullOrEmpty(userId))
                {
                    return Json(new { code = "001", msg = "该用户没有登录,不可以推荐！" });
                }
                if (inviters != null && inviters.xDInviters.Count > 0)
                {
                    foreach (var xDInviter in inviters.xDInviters)
                    {
                        if (_AppContext.XDInviterApp.IsInvited(xDInviter.InviteredMobile, activityId))
                        {
                            return Json(new { code = "001", msg = "该手机号：" + xDInviter.InviteredMobile + "已经被邀请过！" });
                        }
                    }
                    foreach (var xDInviter in inviters.xDInviters)
                    {
                        ApplicationUser user = UserManager.FindById(userId);
                        xDInviter.InviterUserId = userId;
                        xDInviter.ActivityId = activityId;
                        xDInviter.InviterName = string.IsNullOrEmpty(user.RealName) ? user.NickName : user.UserName;
                        xDInviter.InviterMobile = user.PhoneNumber;
                        xDInviter.InviterTime = DateTime.Now;
                        xDInviter.IsValid = 1;
                        xDInviter.InviterType = 1;
                        xDInviter.CreaterId = userId;
                        xDInviter.CreaterName = string.IsNullOrEmpty(user.RealName) ? user.NickName : user.UserName;
                        xDInviter.CreaterTime = DateTime.Now;
                        xDInviter.UpdaterId = userId;
                        xDInviter.UpdaterName = string.IsNullOrEmpty(user.RealName) ? user.NickName : user.UserName;
                        xDInviter.UpdaterTime = DateTime.Now;
                        _AppContext.XDInviterApp.AddXDInviter(xDInviter);
                    }
                }
                bool isExistLottery = _AppContext.XDLotteryRecordApp.IsExistLotteryRecordByUserId(userId, activityId, 0);
                if (isExistLottery)
                {
                    return Json(new { code = "201", msg = "已抽奖！" });
                }
                else
                {
                    return Json(new { code = "200", msg = "推荐成功！" });
                }
            }
            catch (Exception)
            {
                return Json(new { code = "0", msg = "推荐异常！" });
            }
        }
        public JsonResult AddXDSendInfo(XDSendInfo xDSendInfo)
        {
            try
            {
                string userId = string.Empty;
                if (this.User.Identity.IsAuthenticated)
                {
                    userId = this.User.Identity.GetUserId();
                }
                if (string.IsNullOrEmpty(userId))
                {
                    return Json(new { code = "001", msg = "该用户没有登录！" });
                }
                xDSendInfo.UserId = userId;
                xDSendInfo.ActivityId = activityId;
                xDSendInfo.CreaterId = "0";
                xDSendInfo.CreaterName = xDSendInfo.UserName;
                xDSendInfo.CreaterTime = DateTime.Now;
                xDSendInfo.UpdaterId = "0";
                xDSendInfo.UpdaterName = xDSendInfo.UserName;
                xDSendInfo.UpdaterTime = DateTime.Now;
                xDSendInfo.IsValid = 1;
                if (_AppContext.XDSendInfoApp.AddXDSendInfo(xDSendInfo))
                {
                    return Json(new { code = "200", msg = "添加邮寄地址成功！" });
                }
                else
                {
                    return Json(new { code = "0", msg = "添加邮寄地址失败！" });
                }
            }
            catch (Exception)
            {
                return Json(new { code = "0", msg = "添加邮寄地址异常！" });
            }

        }
        public JsonResult IsCanInviter()
        {
            string userId = string.Empty;
            if (this.User.Identity.IsAuthenticated)
            {
                userId = this.User.Identity.GetUserId();
            }
            if (string.IsNullOrEmpty(userId))
            {
                return Json(new { code = "001", msg = "该用户没有登录,不可以抽奖！" });
            }
            var account = new FrontUserStore<FrontIdentityUser>().FindByIdAsync(userId);
            if (account == null || account.Result == null || account.Result.SystemMType != 2)
            {
                return Json(new { code = "002", msg = "用户还没有绑定身份证！" });
            }
            return Json(new { code = "200", msg = "该用户没有登录,不可以推荐！" });
        }
        public JsonResult BindIdentityNumber(string identityNumber)
        {
            try
            {
                string userId = string.Empty;
                if (this.User.Identity.IsAuthenticated)
                {
                    userId = this.User.Identity.GetUserId();
                }
                if (string.IsNullOrEmpty(userId))
                {
                    return Json(new { code = "001", msg = "该用户没有登录,不可以抽奖！" });
                }

                var account = new FrontUserStore<FrontIdentityUser>().FindByIdAsync(userId);
                if (account == null || account.Result == null)
                {
                    return Json(new { code = "002", msg = "用户不存在！" });
                }
                //var customerEntity = _AppContext.UserInfoApp.GetInfoWhenBuyCar(identityNumber);
                //if (customerEntity != null)
                //{
                //    return Json(new { code = "003", msg = "该身份证号已绑定！" });
                //}

                var store = new FrontUserStore<FrontIdentityUser>();
                var user = store.FindByIdAsync(userId).Result;
                if (store.IsIdentityNumberRepeate(identityNumber))
                {
                    var carList1 = _AppContext.CarServiceUserApp.SelectCarListByIdentity(identityNumber);
                    if (carList1 == null || carList1.Count() <= 0)
                    {
                        return Json(new
                        {
                            code = 301,
                            msg = "此身份证号已被其他车主绑定,如有疑问请拨打:400-800-1100"
                        });
                    }
                }
                    

                //设置车主的类型
                //gradeUser.MType = mtype;
                user.IdentityNumber = identityNumber;
                //获取用户车辆信息
                var carList = _AppContext.CarServiceUserApp.SelectCarListByIdentity(identityNumber);
                if (carList == null || carList.Count() <= 0)
                {
                    // UserManager.Update(gradeUser);
                    //提示未匹配到您的车辆，请拨打客服电话
                    return Json(new
                    {
                        code = 301,
                        msg = CommonConst.NOCARTIP
                    });
                }

                user.MLevel = Convert.ToInt32(_AppContext.DealerMembershipApp.GetFirstRegistMLevelByIdNumber(identityNumber));
                user.PaperWork = "1";
                user.SystemMType = (int)MembershipType.WhitCar;

                #region  pagework
                store.AddPaperworkToMembership_Schedule(user);
                #endregion


                //判断交费级别
                var returnintegral = (int)_AppContext.CarServiceUserApp.GetReIntegralTypeByIdentity(user.IdentityNumber);
                store.UpdateAsync(user);
                //if (returnintegral != -1)
                //{
                //    return Json(new
                //    {
                //        code = 302,
                //        msg = "缴费返积分",
                //        reintegralType = returnintegral
                //    });

                //}
                return Json(new { code = "200", msg = "绑定成功！" });
            }
            catch (Exception)
            {
                return Json(new { code = "004", msg = "绑定身份证出现异常！" });
            }
        }
        /// <summary>
        /// 是否参与过预约置换抽奖
        /// </summary>
        /// <param name="xDOrderChange"></param>
        /// <returns></returns>
        private ReturnResult GetOrderChangeLottery(string mobile, int activityId, int lotteryType, string lotterySource)
        {
            try
            {
                if (!_AppContext.XDOrderChangeApp.IsCanOrderChange(activityId, mobile))
                {
                    return new ReturnResult { IsSuccess = false, Message = "该手机号未参与过预约置换,不可以抽奖！" };
                }
                else if (_AppContext.XDLotteryRecordApp.IsExistLotteryRecordByUserMobile(mobile, activityId, lotteryType))
                {
                    return new ReturnResult { IsSuccess = false, Message = "该手机号已参与预约置换抽奖！" };
                }
                XDActivity activity = _AppContext.XDActivityApp.GetXDActivityByActivityId(activityId);
                if (activity == null)
                {
                    return new ReturnResult { IsSuccess = false, Message = "该活动不存在！" };
                }
                XDLottery lottery = _AppContext.XDLotteryApp.GetNextLottery(activityId, lotteryType);
                if (lottery == null)
                {
                    return new ReturnResult { IsSuccess = false, Message = "奖品已抽完！" };
                }
                else
                {
                    XDLotteryRecord xDLotteryRecord = new XDLotteryRecord();
                    xDLotteryRecord.LotteryId = lottery.LotteryId;
                    xDLotteryRecord.ActivityId = activityId;
                    xDLotteryRecord.ActivityName = activity.ActivityName;
                    xDLotteryRecord.LotteryName = lottery.LotteryName;
                    xDLotteryRecord.UserId = mobile;
                    xDLotteryRecord.UserName = mobile;
                    xDLotteryRecord.LotteryRecordTime = DateTime.Now;
                    xDLotteryRecord.UserIp = "";
                    xDLotteryRecord.IsValid = 1;
                    xDLotteryRecord.LotteryType = lotteryType;
                    xDLotteryRecord.UserMobile = mobile;
                    xDLotteryRecord.LotteryRecordSource = lotterySource;
                    xDLotteryRecord.CreaterId = mobile;
                    xDLotteryRecord.CreaterName = mobile;
                    xDLotteryRecord.CreaterTime = DateTime.Now;
                    xDLotteryRecord.UpdaterId = mobile;
                    xDLotteryRecord.UpdaterName = mobile;
                    xDLotteryRecord.UpdaterTime = DateTime.Now;
                    //添加中奖记录
                    int LotteryRecordId = _AppContext.XDLotteryRecordApp.AddXDLotteryRecord(xDLotteryRecord);
                    lottery.LotteryRecordId = LotteryRecordId;
                    if (lottery.LotteryId > 0)
                    {
                        //减少活动的奖品剩余数量
                        _AppContext.XDActivityApp.UpdateActivityLotteryBalanceCount(activityId);
                        //减少奖品的剩余数量
                        _AppContext.XDLotteryApp.UpdateLotteryBalanceCount(lottery.LotteryId, activityId);
                        XDOrderChange xDOrderChange = _AppContext.XDOrderChangeApp.GetOrderChangeByMobile(activityId, mobile);

                        XDSendInfo xDSendInfo = new XDSendInfo();
                        xDSendInfo.UserId = "";
                        xDSendInfo.ActivityId = activityId;
                        xDSendInfo.CreaterId = "0";
                        xDSendInfo.CreaterName = xDOrderChange.Name;
                        xDSendInfo.CreaterTime = DateTime.Now;
                        xDSendInfo.UpdaterId = "0";
                        xDSendInfo.UpdaterName = xDOrderChange.Name;
                        xDSendInfo.UpdaterTime = DateTime.Now;
                        xDSendInfo.IsValid = 1;
                        xDSendInfo.LotteryId = lottery.LotteryId;
                        xDSendInfo.UserName = xDOrderChange.Name;
                        xDSendInfo.UserMobile = xDOrderChange.Mobile;
                        xDSendInfo.SendProvince = xDOrderChange.SendProvince;
                        xDSendInfo.SendCity = xDOrderChange.SendCity;
                        xDSendInfo.SendDistrinct = xDOrderChange.SendDistrinct;
                        xDSendInfo.SendAddress = xDOrderChange.SendAddress;
                        xDSendInfo.SendSource = lotterySource;
                        xDSendInfo.LotteryRecordId = LotteryRecordId;
                        _AppContext.XDSendInfoApp.AddXDSendInfo(xDSendInfo);
                    }
                    return new ReturnResult { IsSuccess = true, Message = "可以参与推荐抽奖！", Data = lottery };
                }

            }
            catch (Exception)
            {
                return new ReturnResult { IsSuccess = false, Message = "是否参与过预约置换抽奖校验异常！" };
            }
        }

        private ReturnResult GetInviterLottery(int activityId, int lotteryType, string lotterySource)
        {
            try
            {
                string userId = string.Empty;
                if (this.User.Identity.IsAuthenticated)
                {
                    userId = this.User.Identity.GetUserId();
                }
                if (string.IsNullOrEmpty(userId))
                {
                    return new ReturnResult { IsSuccess = false, Message = "该用户没有登录,不可以抽奖！" };
                }
                var account = new FrontUserStore<FrontIdentityUser>().FindByIdAsync(userId);
                if (account == null || account.Result == null)
                {
                    return new ReturnResult { IsSuccess = false, Message = "会员账号不存在！" };
                }

                if (account.Result.SystemMType != 2)//是否绑定身份证
                {
                    return new ReturnResult { IsSuccess = false, Message = "用户还没有绑定身份证！" };
                }
                if (_AppContext.XDInviterApp.IsExistXDInviter(userId, activityId) <= 0)
                {
                    return new ReturnResult { IsSuccess = false, Message = "该用户没有推荐,不可以抽奖！" };
                }
                else if (_AppContext.XDLotteryRecordApp.IsExistLotteryRecordByUserId(userId, activityId, lotteryType))
                {
                    return new ReturnResult { IsSuccess = false, Message = "该用户已参与推荐抽奖！" };
                }
                XDActivity activity = _AppContext.XDActivityApp.GetXDActivityByActivityId(activityId);
                if (activity == null)
                {
                    return new ReturnResult { IsSuccess = false, Message = "该活动不存在！" };
                }
                XDLottery lottery = _AppContext.XDLotteryApp.GetNextLottery(activityId, lotteryType);
                if (lottery == null)
                {
                    return new ReturnResult { IsSuccess = false, Message = "奖品已抽完！" };
                }
                else
                {
                    var user = account.Result;
                    XDLotteryRecord xDLotteryRecord = new XDLotteryRecord();
                    xDLotteryRecord.LotteryId = lottery.LotteryId;
                    xDLotteryRecord.ActivityId = activityId;
                    xDLotteryRecord.ActivityName = activity.ActivityName;
                    xDLotteryRecord.LotteryName = lottery.LotteryName;
                    xDLotteryRecord.UserId = userId;
                    xDLotteryRecord.UserName = string.IsNullOrEmpty(user.UserName) ? user.NickName : user.UserName;
                    xDLotteryRecord.LotteryRecordTime = DateTime.Now;
                    xDLotteryRecord.UserIp = "";
                    xDLotteryRecord.IsValid = 1;
                    xDLotteryRecord.LotteryType = lotteryType;
                    xDLotteryRecord.UserMobile = user.PhoneNumber;
                    xDLotteryRecord.LotteryRecordSource = lotterySource;
                    xDLotteryRecord.CreaterId = userId;
                    xDLotteryRecord.CreaterName = string.IsNullOrEmpty(user.UserName) ? user.NickName : user.UserName;
                    xDLotteryRecord.CreaterTime = DateTime.Now;
                    xDLotteryRecord.UpdaterId = userId;
                    xDLotteryRecord.UpdaterName = string.IsNullOrEmpty(user.UserName) ? user.NickName : user.UserName;
                    xDLotteryRecord.UpdaterTime = DateTime.Now;
                    //添加中奖记录
                    int LotteryRecordId = _AppContext.XDLotteryRecordApp.AddXDLotteryRecord(xDLotteryRecord);
                    lottery.LotteryRecordId = LotteryRecordId;
                    if (lottery.LotteryId > 0)
                    {
                        //减少活动的奖品剩余数量
                        _AppContext.XDActivityApp.UpdateActivityLotteryBalanceCount(activityId);
                        //减少奖品的剩余数量
                        _AppContext.XDLotteryApp.UpdateLotteryBalanceCount(lottery.LotteryId, activityId);
                    }
                    return new ReturnResult { IsSuccess = true, Message = "可以参与推荐抽奖！", Data = lottery };
                }
            }
            catch (Exception)
            {
                return new ReturnResult { IsSuccess = false, Message = "是否参与过预约置换抽奖校验异常！" };
            }
        }

        public JsonResult GetRandomLottery(string mobileOrUserId, int lotteryType, string source, string inviteOrChange)
        {
            try
            {
                ReturnResult result = null;
                if (inviteOrChange == "change")
                {
                    result = GetOrderChangeLottery(mobileOrUserId, activityId, lotteryType, source);
                }
                else
                {
                    result = GetInviterLottery(activityId, lotteryType, source);
                }

                return Json(new { code = result.IsSuccess ? "200" : "0", msg = result.Message, data = result.Data });
            }
            catch (Exception)
            {
                return Json(new { code = "0", msg = "是否参与过预约置换抽奖校验异常！" });
            }
        }

        /// <summary>
        /// 获取用户ip
        /// </summary>
        /// <returns>获取用户ip</returns>
        private string GetUserIP()
        {
            string strIP = string.Empty;
            HttpRequest request = System.Web.HttpContext.Current.Request;
            if (request.ServerVariables["HTTP_VIA"] != null && request.ServerVariables["HTTP_VIA"] != "")
                strIP = request.ServerVariables["HTTP_X_FORWARDED_FOR"] + "";
            else
                strIP = request.ServerVariables["REMOTE_ADDR"] + "";
            string[] IPs = strIP.Split(new char[] { ',' });
            for (int i = 0; i < IPs.Length; i++)
            {
                if (IPs[i] != null && IPs[i].Trim() != "")
                {
                    strIP = IPs[i].Trim();
                    break;
                }
            }
            if (strIP == null || strIP.Trim() == "")
            {
                strIP = "0.0.0.0";
            }
            return strIP;
        }


        public ActionResult Index(string type, string s = "pc")
        {
            ViewBag.Source = s;
            ViewBag.SourceType = type;
            return View();
        }
        public ActionResult BangDing()
        {
            var cookieValue = CookieHelper.GetCookieValue("CustomCookie");
            if (!string.IsNullOrWhiteSpace(cookieValue) && cookieValue.ToLower().Contains("webinspect"))
            {
                return Redirect("/Contents/error.htm");
            }
            string source = Request["source"] == null ? "" : Request["source"].ToString();
            string flag = Request["flag"] == null ? "" : Request["flag"].ToString();
            ViewBag.Source = source;
            ViewBag.Flag = flag;
            return View();
        }
        public ActionResult JiangPing()
        {
            string source = Request["source"] == null ? "" : Request["source"].ToString();
            ViewBag.Source = source;
            return View();
        }
        public ActionResult JieShao()
        {
            string source = Request["source"] == null ? "" : Request["source"].ToString();
            ViewBag.Source = source;
            return View();
        }
        public ActionResult LibaoLD()
        {
            string source = Request["source"] == null ? "" : Request["source"].ToString();
            ViewBag.Source = source;
            return View();
        }

        public ActionResult LibaoSD()
        {
            string source = Request["source"] == null ? "" : Request["source"].ToString();
            ViewBag.Source = source;
            return View();
        }

        public ActionResult LuckyIndex()
        {
            var cookieValue = CookieHelper.GetCookieValue("CustomCookie");
            if (!string.IsNullOrWhiteSpace(cookieValue) && cookieValue.ToLower().Contains("webinspect"))
            {
                return Redirect("/Contents/error.htm");
            }
            string flag = Request["flag"] == null ? "1" : Request["flag"].ToString();
            string source = Request["source"] == null ? "" : Request["source"].ToString();
            string pagesource = Request["pagesource"] == null ? "" : Request["pagesource"].ToString();
            string mobile = Request["mobile"] == null ? "" : Request["mobile"].ToString();
            string userId = string.Empty;
            if (this.User.Identity.IsAuthenticated)
            {
                userId = this.User.Identity.GetUserId();
            }
            ViewBag.UserId = userId;
            ViewBag.Source = source;
            ViewBag.Flag = flag;
            ViewBag.PageSource = pagesource;
            ViewBag.Mobile = mobile;
            return View();
        }

        public ActionResult ShiJia()
        {
            int flag = Request["flag"] == null ? 1 : int.Parse(Request["flag"]);
            string source = Request["source"] == null ? "" : Request["source"].ToString();
            ViewBag.Source = source;
            ViewBag.Flag = flag;
            ViewData["provinceList"] = CityService.Instance.GetProvince();
            return View();
        }

        public ActionResult TuiJian()
        {
            int flag = Request["flag"] == null ? 1 : int.Parse(Request["flag"]);
            string source = Request["source"] == null ? "" : Request["source"].ToString();
            ViewBag.Source = source;
            ViewBag.Flag = flag;
            return View();
        }
        public ActionResult ZhongJiang()
        {
            int activityid = activityId;
            IEnumerable<XDLotteryRecord> lstLotteryRecord = _AppContext.XDLotteryRecordApp.GetXDLotteryRecordList(activityid, 0, 0);
            var result = lstLotteryRecord.Where(o => o.LotteryName != "谢谢惠顾");
            return View(result);
        }
    }
}