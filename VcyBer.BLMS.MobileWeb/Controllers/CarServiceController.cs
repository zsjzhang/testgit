using AspNet.Identity.SQL;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Common.Web;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.CarService;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.Entity.Generated;
using Vcyber.BLMS.Application.Common;

namespace VcyBer.BLMS.MobileWeb.Controllers
{
    public class CarServiceController : BaseController
    {
        // GET: CarService
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult RoadRescue()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();
                var user = new FrontUserStore<FrontIdentityUser>().FindByIdAsync(userId);
                var userType = 0;
                if (user != null )
                {
                    if (user.Result.SystemMType >1)
                    {
                        string[] strArray=new string[]{"第九代索纳塔","索九","索纳塔9","第九代索纳塔混合动力","全新途胜","全新胜达","悦纳"};
                        var cars = _AppContext.CarServiceUserApp.CarsByUserId(userId).Where(w=>w.BuyTime>=DateTime.Now.AddYears(-1)&&w.BuyTime<=DateTime.Parse("2016-12-31")&&strArray.Contains(w.CarCategory)).ToList();
                        if (cars.Count > 0)
                        {
                            userType = 3;
                        }
                        else
                        {
                            userType = 2;
                        }
                    }
                    else
                    {
                        userType = user.Result.SystemMType;
                    }
                }
                
                ViewBag.userId = userId;
                //根据userid获取userType
                ViewBag.userType = userType;
                return View();
            }
            else
            {
                return RedirectToAction("login", "account", new { url = "/carService/roadRescue" });
            }
        }
        public ActionResult FreeCar()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();
                var user = new FrontUserStore<FrontIdentityUser>().FindByIdAsync(userId).Result;
                var freeServiceRecords=_AppContext.CarServiceUserApp.GetFreeServiceRecord(user.IdentityNumber, EDMSServiceType.Home2Home);
                ViewBag.RecordCount = freeServiceRecords.Where(w => w.MLevelBeginDate <= w.CreateTime && w.MLevelInvalidDate >= w.CreateTime).ToList().Count;
                return View(user);
            }
            else
            {
                return RedirectToAction("login", "account", new { url = "/carService/FreeCar" });
            }
        }
        public ActionResult CardSaleAfter()
        {
            return View();
        }
        public ActionResult AddFreeCar()
        {
            return View();
        }
        public JsonResult SubmitFreeCar(SonataServiceEntity entity)
        {
            ReturnResult result = new ReturnResult() { IsSuccess = true, Message = "预约成功", Errors = 200 };
            if (this.User.Identity.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();
                var userName = User.Identity.GetUserName();

                entity.DataSource = WebUtils.Device();
                entity.UserId = userId;
                entity.UserName = userName;
                entity.OrderType = EBMServiceType.Home2Home;
                entity.DataSource = "blms_wap";

                //判断此车型是不预约过
                //QueryParamEntity par = new QueryParamEntity();
                //par.ScheduleFromDate = entity.ScheduleDate;
                //par.ScheduleToDate = entity.ScheduleDate;
                //par.LicensePlate = entity.LicensePlate;
                //var list = _AppContext.SonataServiceApp.QueryOrders(par, 1, 100);
                //if (list.Items.Count > 0)
                //{
                //    result.IsSuccess = false;
                //    result.Errors = 400;
                //    result.Message = "您已经预约过，请耐心等待，经销商会主动与您取得联系。";
                //    return Json(result, JsonRequestBehavior.AllowGet);
                //}
                var user = new FrontUserStore<FrontIdentityUser>().FindByIdAsync(userId).Result;
                var count=_AppContext.CarServiceUserApp.GetFreeServiceRecord(user.IdentityNumber, EDMSServiceType.Home2Home).Where(w => w.MLevelBeginDate <= w.CreateTime && w.MLevelInvalidDate >= w.CreateTime).ToList().Count;
                if (count > 0)
                {
                    result.IsSuccess = false;
                    result.Errors = 400;
                    result.Message = "您已经预约过，请耐心等待，经销商会主动与您取得联系。";
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                int _result = _AppContext.SonataServiceApp.Add(entity);
                if (_result <= 0)
                {
                    result.IsSuccess = false;
                    result.Errors = 400;
                    result.Message = "预约失败。";
                }
                else
                {
                    _AppContext.SMSApp.SendSMS(ESmsType.预约_预约成功, entity.Phone, new string[] { });
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult TwoPoints(string province, string city, double long1, double lat1, double long2, double lat2)
        {
            IEnumerable<CSCarDealerShip> list = _AppContext.DealerApp.GetDealerListByDistance(province,
              city,
              long1,
              lat1,
              long2,
              lat2,
             20000);
            return Json(new { Success=true,Data=list}, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult FreeRoadRescue(CS_FreeRoadRescue entity)
        {
            if (entity != null)
            {
                if (!_AppContext.FreeRoadRescueApp.AddFreeRoadRescue(entity))
                {
                    return Json(new { success = true, msg = "免费道路救援信息请求成功！" });
                }

            } 
            return Json(new { success = false, msg = "免费道路救援信息请求失败！" });
        }

    }
}