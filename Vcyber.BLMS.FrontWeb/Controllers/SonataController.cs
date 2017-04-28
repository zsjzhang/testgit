using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Entity.CarService;
using Vcyber.BLMS.Entity.Enum;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using PetaPoco;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.FrontWeb.Models;
using System.Text;
using Vcyber.BLMS.Entity.Generated;

namespace Vcyber.BLMS.FrontWeb.Controllers
{
    /// <summary>
    /// 索九专区
    /// </summary>
    public class SonataController : Controller
    {
        #region ==== 私有字段 ====

        private ApplicationUserManager _userManager;

        #endregion
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
        /// <summary>
        /// 索九专区首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult FreeMotIndex(string source)
        {
            ViewBag.source = source;
            return View();
        }

        /// <summary>
        /// 3年9次免费年检
        /// </summary>
        /// <returns></returns>
        [MvcAuthorize]
        public ActionResult FreeMot(string source)
        {
            ViewBag.source = source;

            var user = UserManager.FindByNameAsync(this.User.Identity.Name).Result;
            if (user.UserType != EnumExtension.GetDiscribe<EUserType>(EUserType.LF) && user.UserType != EnumExtension.GetDiscribe<EUserType>(EUserType.TOP))
            {
                return Content("<script>popWindownBlue( '尊敬的车主您好，此服务为第九代索纳塔车主专项服务，请核实您的车型信息。');window.open('" + Url.Content("~/Sonata/Index") + "', '_self')</script>");
            }

            return View();
        }

        /// <summary>
        /// 3年9次免费结果页面显示
        /// </summary>
        /// <returns></returns>
        public ActionResult Result()
        {
            return View();
        }

        /// <summary>
        /// 五种车辆产品服务
        /// </summary>
        /// <returns></returns>
        public ActionResult FiveCarService()
        {
            var carList = _AppContext.MaintainServiceAPP.GetCustomCardInfoByActType("3").ToList();//3为5种车辆养护产品类型ID
            ViewBag.carlist = carList;
            return View();
        }

        /// <summary>
        /// 验证当前用户是否可以领取
        /// </summary>
        /// <param name="cardType">卡券类型</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult checkCard(string cardType)
        {
            if (!this.User.Identity.IsAuthenticated)
            {
                return Json(new { code = 301, msg = "尊敬的会员您好，此服务仅限金卡和银卡会员可领取哦，请您升级后领取！" });
            }
            var user = UserManager.FindById(this.User.Identity.GetUserId());
            if (user == null)
            {
                return Json(new { code = 301, msg = "尊敬的会员您好，此服务仅限金卡和银卡会员可领取哦，请您升级后领取！" });
            }
            if (user.MLevel != 11 && user.MLevel != 12)
            {
                return Json(new { code = 301, msg = "尊敬的会员您好，此服务仅限金卡和银卡会员可领取哦，请您升级后领取！" });
            }
            //到这一步说明已经是金卡或者银卡会员，再次验证在2016.4.1之后有没有购车
            var carList = _AppContext.CarServiceUserApp.SelectCarListByIdentity(user.IdentityNumber);
            if (carList == null || carList.Count() < 0)
           {
               return Json(new { code = 301, msg = "您好，5种养护产品只针对2016年4月1日之后购车并注册成为bluemembers银卡/金卡会员的用户，您可关注其它会员服务。" });
           }
            //筛选购车时间在 2016.04.01之后的数据
           carList = carList.Where(t => t.BuyTime >= DateTime.Parse("2016-04-01 00:00:00")).ToList();
           if (carList.Count() <= 0)
           {
               return Json(new { code = 301, msg = "您好，5种养护产品只针对2016年4月1日之后购车并注册成为bluemembers银卡/金卡会员的用户，您可关注其它会员服务。" });
           }
            //查询卡券领取记录 3是5种车辆养护产品的类型ID
           var customCardList = _AppContext.CustomCardApp.getUserCustomCardByUID(this.User.Identity.GetUserId(), "3");
           //获取会员有效期
           var MLevelBeginDate = user.MLevelBeginDate;
           var MLevelInvalidDate = user.MLevelInvalidDate;
           //判断当前时间是否在会员有效期内
           if (MLevelBeginDate > DateTime.Now || MLevelInvalidDate < DateTime.Now)
           {
               return Json(new { code = 301, msg = "尊敬的会员您好，此服务仅限金卡和银卡会员可领取哦，请您升级后领取！" });
           }
            //如果没有领取过，则验证成功，可以领取
           if (customCardList.Count <= 0)
           {
               return Json(new { code = 302, msg = "success" });
           }
           #region 按照自然年业务流程  已注释
            //筛选只保留今年领取记录
           customCardList=customCardList.Where(t=>t.CreateTime.Year ==DateTime.Now.Year).ToList();
           //如果今年没有领取过，则验证成功，可以领取
            if (customCardList.Count <= 0)
           {
               return Json(new { code = 302, msg = "success" });
           }
            //根据会员级别来判断可以领取的次数
           switch (user.MLevel)
           {
               case 11:
                    if (customCardList.Count >= 1)
                   {
                       return Json(new { code = 301, msg = "尊敬的会员您好，银卡会员只有1次/年领取机会，您已领取过了，不要贪心哦。您可以第2年再来领取！" });
                   }
                   break;
               case 12:
                    if (customCardList.Count >= 2)
                   {
                       return Json(new { code = 301, msg = "尊敬的会员您好，金卡会员只有2次/年领取机会，您已领取过了，不要贪心哦。您可以第2年再来领取！" });
                   }
                   //如果只领取了1次，需要在判断这次领取的是不是同一个产品
                   foreach (var item in customCardList)
                   {
                       if (item.CardType == cardType)
                       {
                           return Json(new { code = 301, msg = "尊敬的会员您好，同一种车辆养护产品1年只能领取1次哦。您已领取过了，请您选择其他养护产品吧！" });
                       }
                   }
                   break;
           }

           #endregion

           return Json(new { code = 302, msg = "success" });
        }

        /// <summary>
        /// 5种车辆养护产品领取成功后发送短信
        /// </summary>
        /// <param name="expiryTime">卡券有效期</param>
        /// <param name="cardName">卡券名字</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult sendMsg(string cardName,string expiryTime)
        {
            var user = UserManager.FindById(this.User.Identity.GetUserId());
            _AppContext.SMSApp.SendSMS(ESmsType.车辆养护产品5种, user.PhoneNumber, new string[] { cardName, expiryTime }, false);
            return Json(new { code = 301, msg = "success" });
        }
        /// <summary>
        /// 五种车辆产品服务
        /// </summary>
        /// <returns></returns>
        public ActionResult CodeService()
        {
            return View();
        }
        /// <summary>
        /// 3年9次免费失败页面显示
        /// </summary>
        /// <returns></returns>
        public ActionResult Fail(EOrderType type)
        {
            ViewBag.Message = type.GetDiscribe();
            return View();
        }
        /// <summary>
        /// 3年9次免费年检表单
        /// </summary>
        /// <returns></returns>
        [MvcAuthorize]
        [S9MvcAuthorize]
        public ActionResult FreeMotForm(string source)
        {
            ViewBag.source = source;

            return View();
        }

        /// <summary>
        /// 3年9次免费年检服务预约
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public JsonResult FreeMotFormSave(FreeMotModel entity)
        {
            if (string.IsNullOrEmpty(this.User.Identity.Name))
            {
                return Json(new { success = false, message = "当前没有登录，请登录。" });
            }

            var user = UserManager.FindByNameAsync(this.User.Identity.Name).Result;
            if (user == null)
            {
                return Json(new { success = false, message = "手机号对应的账号不存在" });
            }

            if (string.IsNullOrEmpty(user.IdentityNumber))
            {
                return Json(new { success = false, message = "手机号对应的账号身份证不存在，不能预约" });
            }

            if (string.IsNullOrEmpty(user.No))
            {
                return Json(new { success = false, message = "抱歉，您不是银卡会员，不能参与预约" });
            }

            if (user.UserType != EnumExtension.GetDiscribe<EUserType>(EUserType.LF) && user.UserType != EnumExtension.GetDiscribe<EUserType>(EUserType.TOP))
            {
                return Json(new { success = false, message = "抱歉，您不是索九车主，不能参与预约" });
            }

            if (!ModelState.IsValid)
            {
                //参数错误
                return Json(new { success = false, message = "参数错误" });
            }

            DateTime dBuyTime = _AppContext.SonataServiceApp.GetSonataBuyTime(user.IdentityNumber);
            if (dBuyTime != null)
            {
                int iYear = dBuyTime.AddYears(3).Year;
                int noeYear = DateTime.Now.Year;
                if (noeYear > iYear)
                {
                    return Json(new { success = false, message = "您的购车时间为：" + dBuyTime.ToString("yyyy-MM-dd") + "，3年9次免费，有效时间已过（自购车之日起3年内，每年为您的爱车免费提供3次专项检测服务）" });
                }
            }



            //通过身份证查找剩余服务次数
            var count = _AppContext.CarServiceUserApp.GetFreeServiceCount(user.IdentityNumber, EDMSServiceType.FreeCheck);
            if (count <= 0)
            {
                //return Json(new { success = false, message = "3年9次免费次数已经用完，无法再次预约" });
                return Json(new { success = false, message = "今年3次免费已用完，无法再次预约" });

            }
            //获取DealerName
            var dealer = _AppContext.DealerApp.GetDealerByDealerId(entity.DealerId);

            //why copy a new model?
            var sonataService = new SonataServiceEntity();
            sonataService.Phone = entity.Phone;
            sonataService.UserName = entity.UserName;
            sonataService.UserSex = entity.UserSex;
            sonataService.MileAge = entity.MileAge;
            sonataService.ScheduleDate = entity.ScheduleDate;
            sonataService.LicensePlate = entity.LicensePlate;
            sonataService.VIN = entity.VIN;
            sonataService.DealerId = entity.DealerId;
            sonataService.DealerCity = entity.DealerCity;
            sonataService.DealerProvince = entity.DealerProvince;
            sonataService.PurchaseDate = new DateTime(int.Parse(entity.PurchaseYear), 1, 1); //entity.PurchaseDate;
            sonataService.Comment = entity.Comment;
            sonataService.UserId = user.Id;
            sonataService.DealerName = dealer.Name;
            sonataService.OrderType = EBMServiceType.FreeCheck;
            sonataService.CarSeries = entity.CarSeries;
            sonataService.PurchaseYear = entity.PurchaseYear;

            sonataService.DataSource = entity.DataSource;

            int retval = _AppContext.SonataServiceApp.Add(sonataService, this.User.Identity.GetUserId(), User.Identity.GetUserName());

            if (retval > 0)
            {
                return Json(new { success = true, message = "预约成功" });
            }

            return Json(new { success = false, message = "预约失败" });
        }

        /// <summary>
        /// HomeToHome服务
        /// </summary>
        /// <returns></returns>
        public ActionResult ServiceToHome(string source)
        {
            if (this.User.Identity.IsAuthenticated)
            {
                //ViewBag.UserLevel = UserManager.FindById(this.User.Identity.GetUserId()).MLevel;
                var user = UserManager.FindById(this.User.Identity.GetUserId());
                ViewBag.UserLevel = user.MLevel;
                ViewBag.IsInValid = user.MLevelInvalidDate < DateTime.Now;
                var freeServiceRecords = _AppContext.CarServiceUserApp.GetFreeServiceRecord(user.IdentityNumber, EDMSServiceType.Home2Home);
                ViewBag.Count = freeServiceRecords.Where(w => w.MLevelBeginDate <= w.CreateTime && w.MLevelInvalidDate >= w.CreateTime).ToList().Count > 0 ? 0 : 1;
            }
            ViewBag.source = source;
            return View();
        }

        /// <summary>
        /// HomeToHome服务
        /// </summary>
        /// <returns></returns>
        //[S9MvcAuthorize]
        [MvcAuthorize]
        public ActionResult ServiceToHomeForm(string source)
        {
            ViewBag.source = source;
            return View();
        }

        /// <summary>
        /// HomeToHome服务预约
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public JsonResult ServiceToHomeFormSave(Home2HomeModel entity)
        {
            if (string.IsNullOrEmpty(this.User.Identity.Name))
            {
                return Json(new { success = false, message = "当前没有登录，请登录。" });
            }

            var user = UserManager.FindByNameAsync(this.User.Identity.Name).Result;
            if (user == null)
            {
                return Json(new { success = false, message = "手机号对应的账号不存在" });
            }

            if (string.IsNullOrEmpty(user.IdentityNumber))
            {
                return Json(new { success = false, message = "手机号对应的账号身份证不存在，不能预约" });
            }

            if (!ModelState.IsValid)
            {
                //参数错误
                return Json(new { success = false, message = "参数错误" });
            }
            var time = DateTime.Parse(DateTime.Now.AddHours(48).ToShortDateString());
            if (entity.ScheduleDate < time)
            {
                return Json(new { success = false, message = "很抱歉，网站暂时不支持48小时内的预约服务，请您拨打400-800-1100电话预约。" });
            }
            if (entity.ScheduleDate > entity.ReturnDate)
            {
                return Json(new { success = false, message = "很抱歉，送车时间不能小于取车时间。" });

            }

            //DateTime dBuyTime = _AppContext.SonataServiceApp.GetSonataBuyTime(user.IdentityNumber);

            //if (dBuyTime != null)
            //{
            //    DateTime dtNew = dBuyTime.AddYears(1);
            //    int aCompare = DateTime.Now.CompareTo(dtNew);
            //    if (aCompare > 0)
            //    {
            //        return Json(new { success = false, message = "很抱歉，已超过免费取送车服务有效时间（购车之日起1年内赠送1次免费取送车服务）" });
            //    }
            //}

            //var count = _AppContext.CarServiceUserApp.GetFreeServiceCount(user.Id, EDMSServiceType.Home2Home);
            //if (count <= 0)
            //{
            //    return Json(new { success = false, message = "很抱歉，免费取送车服务次数已用完，无法预约" });
            //}
            //业务修改
            if (user.MLevel != 12)
            {
                return Json(new { success = false, message = "很抱歉，您不是金卡会员！" });
            }
            if (user.MLevelInvalidDate < DateTime.Now)
            {
                return Json(new { success = false, message = "很抱歉，已超过免费取送车服务有效时间（购车之日起1年内赠送1次免费取送车服务）" });
            }
            var freeServiceRecords = _AppContext.CarServiceUserApp.GetFreeServiceRecord(user.IdentityNumber, EDMSServiceType.Home2Home);
            var count = freeServiceRecords.Where(w => w.MLevelBeginDate <= w.CreateTime && w.MLevelInvalidDate >= w.CreateTime).ToList().Count;            
            if (count > 0)
            {
                return Json(new { success = false, message = "很抱歉，免费取送车服务次数已用完，无法预约" });
            }


            var dealer = _AppContext.DealerApp.GetDealerByDealerId(entity.DealerId);

            var sonataService = new SonataServiceEntity();
            sonataService.Phone = entity.Phone;
            sonataService.UserName = entity.UserName;
            sonataService.UserSex = entity.UserSex;
            sonataService.ScheduleDate = entity.ScheduleDate;
            sonataService.LicensePlate = entity.LicensePlate;
            sonataService.VIN = entity.VIN;
            sonataService.DealerId = entity.DealerId;
            sonataService.ReturnDate = entity.ReturnDate;
            sonataService.Comment = entity.Comment;
            sonataService.UserId = user.Id;
            sonataService.DealerName = dealer.Name;
            sonataService.DealerCity = dealer.City;
            sonataService.DealerProvince = dealer.Province;
            sonataService.OrderType = EBMServiceType.Home2Home;
            sonataService.TakeAddress = entity.TakeAddress;
            sonataService.ReturnAddress = entity.ReturnAddress;
            sonataService.TakeLong = entity.TakeLong;
            sonataService.TakeLat = entity.TakeLat;

            sonataService.ReturnLong = entity.ReturnLong;
            sonataService.ReturnLat = entity.ReturnLat;

            sonataService.CarSeries = entity.CarSeries;

            sonataService.DataSource = entity.DataSource;

            int retval = _AppContext.SonataServiceApp.Add(sonataService, this.User.Identity.GetUserId(), User.Identity.GetUserName());

            if (retval > 0)
            {
                return Json(new { success = true, message = "您的预约已成功！特约店工作人员会及时与您联系，请保持手机畅通。祝您用车愉快！客服电话：4008001100" });
            }

            return Json(new { success = false, message = "预约失败" });
        }

        /// <summary>
        /// 100日上门关怀服务
        /// </summary>
        /// <returns></returns>
        public ActionResult GoHomeIndex(string source)
        {
            ViewBag.source = source;
            return View();
        }

        public ActionResult FreeRoadRescue()
        {
            return View();
        }

        /// <summary>
        /// 5年10万公里
        /// </summary>
        /// <returns></returns>
        public ActionResult FiveY10KM()
        {
            return View();
        }

        /// <summary>
        /// 100日上门关怀服务
        /// </summary>
        /// <returns></returns>
        [MvcAuthorize]
        public ActionResult GoHomeForm(string source)
        {
            ViewBag.source = source;
            return View();
        }
        /// <summary>
        /// 100日上门关怀服务预约
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public JsonResult GoHomeFormSave(SonataServiceEntity data)
        {
            if (string.IsNullOrEmpty(this.User.Identity.Name))
            {
                return Json(new { success = false, message = "当前没有登录，请登录。" });
            }

            var user = UserManager.FindByNameAsync(this.User.Identity.Name).Result;
            if (user == null)
            {
                return Json(new { success = false, message = "手机号对应的账号不存在" });
            }

            if (string.IsNullOrEmpty(user.IdentityNumber))
            {
                return Json(new { success = false, message = "手机号对应的账号身份证不存在，不能预约" });
            }

            if (string.IsNullOrEmpty(user.No))
            {
                return Json(new { success = false, message = "抱歉，您不是银卡会员，不能参与预约" });
            }

            if (data.ScheduleDate <= DateTime.Now.AddHours(48))
            {
                return Json(new { success = false, message = "很抱歉，网站暂时不支持48小时内的预约服务，请您拨打400-800-1100电话预约。" });
            }

            DateTime dBuyTime = _AppContext.SonataServiceApp.GetSonataBuyTime(user.IdentityNumber);
            if (dBuyTime != null)
            {
                DateTime begin = dBuyTime.AddMonths(3);
                DateTime end = dBuyTime.AddMonths(4);
                int a = DateTime.Now.CompareTo(begin);
                int b = DateTime.Now.CompareTo(end);
                if (!(a == 1 && b == -1))
                {
                    return Json(new { success = false, message = "很抱歉,不在有效服务时间范围（服务有效期为“购车后3-4月”）。" });
                }
            }
            var count = _AppContext.CarServiceUserApp.GetFreeServiceCount(user.IdentityNumber, EDMSServiceType.Care);
            if (count < 0)
            {
                return Json(new { success = false, message = "很抱歉，100日上门关怀服务次数已用完，无法预约" });
            }

            var dealer = _AppContext.DealerApp.GetDealerByDealerId(data.DealerId);

            data.OrderType = EBMServiceType.Care;

            var sonataService = new SonataServiceEntity();
            sonataService.Phone = data.Phone;
            sonataService.UserName = data.UserName;
            sonataService.UserSex = data.UserSex;
            sonataService.ScheduleDate = data.ScheduleDate;
            sonataService.LicensePlate = data.LicensePlate;
            sonataService.VIN = data.VIN;
            sonataService.DealerId = data.DealerId;

            sonataService.Comment = data.Comment;
            sonataService.UserId = user.Id;
            sonataService.DealerName = dealer.Name;
            sonataService.DealerCity = dealer.City;
            sonataService.DealerProvince = dealer.Province;
            sonataService.OrderType = data.OrderType;
            sonataService.TakeAddress = data.TakeAddress;

            sonataService.TakeLong = data.TakeLong;
            sonataService.TakeLat = data.TakeLat;

            sonataService.ReturnLong = 0;
            sonataService.ReturnLat = 0;

            sonataService.CarSeries = data.CarSeries;

            sonataService.DataSource = data.DataSource;

            int retval = _AppContext.SonataServiceApp.Add(sonataService, this.User.Identity.GetUserId(), User.Identity.GetUserName());

            if (retval > 0)
            {
                return Json(new { success = true, message = "预约成功" });
            }

            return Json(new { success = false, message = "预约失败" });
        }

        /// <summary>
        /// 24小时道路救援服务
        /// </summary>
        /// <returns></returns>
        public ActionResult RescueServiceForm()
        {
            return View();
        }

        /// <summary>
        /// 一对一服务
        /// </summary>
        /// <returns></returns>
        public ActionResult OTwoOIndex(string source)
        {
            ViewBag.source = source;
            return View();
        }

        /// <summary>
        /// 一对一服务
        /// </summary>
        /// <returns></returns>
        [MvcAuthorize]
        public ActionResult OTwoOService(string source)
        {
            ViewBag.source = source;
            return View();
        }

        /// <summary>
        /// 保存一对一服务
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult OTwoOServiceSave(SonataServiceEntity data)
        {
            if (data == null)
            {
                return new JsonResult() { Data = new { Status = "00", message = "数据异常。" } };
            }

            string status = "00";

            if (string.IsNullOrEmpty(this.User.Identity.Name))
            {
                return Json(new { Status = "00", message = "当前没有登录，请登录。" });
            }

            var user = UserManager.FindByNameAsync(this.User.Identity.Name).Result;
            if (user == null)
            {
                return Json(new { Status = "00", message = "手机号对应的账号不存在" });
            }

            if (string.IsNullOrEmpty(user.IdentityNumber))
            {
                return Json(new { Status = "00", message = "手机号对应的账号身份证不存在，不能预约" });
            }

            if (string.IsNullOrEmpty(user.No))
            {
                return Json(new { Status = "00", message = "抱歉，您不是银卡会员，不能参与预约" });
            }

            DateTime tempTime = DateTime.Now.AddHours(-DateTime.Now.Hour).AddMinutes(-DateTime.Now.Minute).AddSeconds(-DateTime.Now.Second);

            if (data.ScheduleDate < tempTime.AddHours(48) || data.ScheduleDate > tempTime.AddHours(168))
            {
                return Json(new { Status = "00", message = "尊敬的车主您好,预计时间必须在后天至7日内。" });
            }

            data.OrderType = EBMServiceType.SpecialMaintain;
            string userId = HttpContext.User.Identity.GetUserId();
            //string userName = HttpContext.User.Identity.GetUserName();
            data.UserId = userId == null ? "" : userId;
            //data.DataSource = "blms";
            // User name is from front page not database
            //data.UserName = userName == null ? "" : userName;
            data.DealerName = _AppContext.DealerApp.GetDealerByDealerId(data.DealerId).Name;
            data.ConsultantId = data.ConsultantId == null ? 0 : data.ConsultantId;
            data.ConsultantName = string.IsNullOrEmpty(data.ConsultantName) ? "" : data.ConsultantName;

            int result = _AppContext.SonataServiceApp.Add(data);
            status = result > 0 ? "11" : "00";
            return new JsonResult() { Data = new { Status = status, message = "数据异常。" } };
        }

        /// <summary>
        /// 获取经销商下的服务顾问
        /// </summary>
        /// <param name="dealerId">经销Id</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ServicePerson(string dealerId)
        {
            var datas = _AppContext.CSConsultantApp.GetList(dealerId);
            return new JsonResult() { Data = new { Status = datas != null ? "11" : "00", Datas = datas } };
        }

        public JsonResult RescueServiceFormSave()
        {
            return Json(new { });
        }

        /// <summary>
        /// 免费候机服务
        /// </summary>
        /// <returns></returns>
        public ActionResult FreeFeePlaneService()
        {
            return View();
        }


        /// <summary>
        /// 免费候机服务ajax请求
        /// </summary>
        /// <returns></returns>
        public JsonResult FreeFeePlaneServiceSave()
        {
            return Json(new { });
        }

        /// <summary>
        /// 长途旅行页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Travel(string source)
        {
            ViewBag.source = source;
            return View();
        }

        /// <summary>
        /// 长途旅行预约表单页面
        /// </summary>
        /// <returns></returns>
        [MvcAuthorizeAttribute]
        public ActionResult TravelForm(string source)
        {
            ViewBag.source = source;

            var user = UserManager.FindByNameAsync(this.User.Identity.Name).Result;
            if (user.UserType != EnumExtension.GetDiscribe<EUserType>(EUserType.Tlc) && user.UserType != EnumExtension.GetDiscribe<EUserType>(EUserType.TOP))
            {
                return Content("<script>popWindownBlue( '尊敬的车主您好，此服务为全新途胜车主专项服务，请核实您的车型信息。');window.open('" + Url.Content("~/Sonata/Index") + "', '_self')</script>");
            }

            return View();
        }

        public JsonResult DoTravel(FreeMotModel entity)
        {

            if (string.IsNullOrEmpty(this.User.Identity.Name))
            {
                return Json(new { success = false, message = "当前没有登录，请登录。" });
            }

            var user = UserManager.FindByNameAsync(this.User.Identity.Name).Result;
            if (user == null)
            {
                return Json(new { success = false, message = "手机号对应的账号不存在" });
            }

            if (string.IsNullOrEmpty(user.IdentityNumber))
            {
                return Json(new { success = false, message = "手机号对应的账号身份证不存在，不能预约" });
            }

            if (string.IsNullOrEmpty(user.No))
            {
                return Json(new { success = false, message = "抱歉，您不是银卡会员，不能参与预约" });
            }

            if (user.UserType != EnumExtension.GetDiscribe<EUserType>(EUserType.Tlc) && user.UserType != EnumExtension.GetDiscribe<EUserType>(EUserType.TOP))
            {
                return Json(new { success = false, message = "抱歉，您不是全新途胜车主，不能参与预约" });
            }

            if (!ModelState.IsValid)
            {
                //参数错误
                return Json(new { success = false, message = "参数错误" });
            }

            DateTime dBuyTime = _AppContext.SonataServiceApp.GetSonataBuyTime(user.IdentityNumber);
            if (dBuyTime != null)
            {
                int iYear = dBuyTime.AddYears(3).Year;
                int noeYear = DateTime.Now.Year;
                if (noeYear > iYear)
                {
                    return Json(new { success = false, message = "您的购车时间为：" + dBuyTime.ToString("yyyy-MM-dd") + "，长途旅行关怀（全新途胜专享），有效时间已过（自购车之日起3年内，每年为您的爱车免费提供1次专项服务）" });
                }
            }



            //通过身份证查找剩余服务次数
            var count = _AppContext.CarServiceUserApp.GetFreeServiceCount(user.IdentityNumber, EDMSServiceType.LongDistanceTravel);
            if (count <= 0)
            {
                //return Json(new { success = false, message = "3年9次免费次数已经用完，无法再次预约" });
                return Json(new { success = false, message = "今年1次免费已用完，无法再次预约" });

            }
            //获取DealerName
            var dealer = _AppContext.DealerApp.GetDealerByDealerId(entity.DealerId);

            //why copy a new model?
            var sonataService = new SonataServiceEntity();
            sonataService.Phone = entity.Phone;
            sonataService.UserName = entity.UserName;
            sonataService.UserSex = entity.UserSex;
            sonataService.MileAge = entity.MileAge;
            sonataService.ScheduleDate = entity.ScheduleDate;
            sonataService.LicensePlate = entity.LicensePlate;
            sonataService.VIN = entity.VIN;
            sonataService.DealerId = entity.DealerId;
            sonataService.DealerCity = entity.DealerCity;
            sonataService.DealerProvince = entity.DealerProvince;
            sonataService.PurchaseDate = new DateTime(int.Parse(entity.PurchaseYear), 1, 1); //entity.PurchaseDate;
            sonataService.Comment = entity.Comment;
            sonataService.UserId = user.Id;
            sonataService.DealerName = dealer.Name;
            sonataService.OrderType = EBMServiceType.LongDistanceTravel;
            sonataService.CarSeries = entity.CarSeries;
            sonataService.PurchaseYear = entity.PurchaseYear;

            sonataService.DataSource = entity.DataSource;

            int retval = _AppContext.SonataServiceApp.Add(sonataService, this.User.Identity.GetUserId(), User.Identity.GetUserName());

            if (retval > 0)
            {
                return Json(new { success = true, message = "预约成功" });
            }

            return Json(new { success = false, message = "预约失败" });
        }

        /// <summary>
        /// 索9会员激活
        /// </summary>
        /// <returns></returns>
        public ActionResult SonataActive()
        {
            if (!this.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("LogonPage", "Account", new { returnUrl = "/Sonata/SonataActive" });
            }
            return View();
        }

        /// <summary>
        /// 激活第二步
        /// </summary>
        /// <returns></returns>
        public ActionResult SonataActiveStepTwo()
        {
            if (!this.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("LogonPage", "Account", new { returnUrl = "/Sonata/SonataActive" });
            }
            return View();
        }

        #region ==== 经销商省市级联 ====

        /// <summary>
        /// 供应商下拉框
        /// </summary>
        /// <returns></returns>
        public ActionResult ProvinceCity()
        {
            IEnumerable<string> _provinces = _AppContext.DealerApp.GetProvinceList();
            return View(_provinces);
        }


        /// <summary>
        /// 供应商下拉框(2)
        /// </summary>
        /// <returns></returns>
        public ActionResult ProvinceAndCity()
        {
            IEnumerable<string> _provinces = _AppContext.DealerApp.GetProvinceList();
            return View(_provinces);
        }

        public JsonResult Province()
        {
            IEnumerable<string> _provinces = _AppContext.DealerApp.GetProvinceList();
            return Json(_provinces, JsonRequestBehavior.AllowGet);
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
            IList<CSCarDealerShip> _result = new List<CSCarDealerShip>();
            IEnumerable<CSCarDealerShip> _dealers = _AppContext.DealerApp.GetDealerList(provinceValue, cityValue);
            if (_dealers != null && _dealers.Any())
            {
                _result = _dealers.ToList();
                return new JsonResult() { Data = new { Status = "11", Datas = _result }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            return new JsonResult() { Data = new { Status = "00", Datas = _result }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        #endregion
    }
}