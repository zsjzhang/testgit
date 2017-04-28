using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vcyber.BLMS.Common;

namespace Vcyber.BLMS.ManageWeb.Controllers
{
    using AspNet.Identity.SQL;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.Owin;

    using Vcyber.BLMS.Application;
    using Vcyber.BLMS.Entity;
    using Vcyber.BLMS.Entity.AirportService;
    using Vcyber.BLMS.ManageWeb.Models;

    [MvcAuthorize]
    public class AirportServiceController : Controller
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

 

        public JsonResult GetFreeCount(string phone)
        {
            FrontUserStore<FrontIdentityUser> frontUserStore = new FrontUserStore<FrontIdentityUser>();
            FrontIdentityUser user = frontUserStore.FindByNameAsync(phone).Result;
            IEnumerable<SNCard> _result = _AppContext.AirportServiceApp.AirportServiceList(phone);//获取使用记录，供列表展示使用

            int hasSendCount = 0;//记录已经下发的免费预约次数(不包括积分兑换和系统下发)
            int remainFreeCount = 0;//剩余免费预约次数
            if(user!=null)
            {
                //当前是索9会员
                if(!string.IsNullOrEmpty(user.Id))
                    hasSendCount = _AppContext.AirportServiceApp.GetSNCardNumber(user.Id);//查询已经下发的免费预约记录次数
                if (user.MLevel==11)//银卡会员
                {
                    remainFreeCount = 2 - hasSendCount;
                }
                else if (user.MLevel == 12)//金卡会员
                {
                    remainFreeCount = 3 - hasSendCount;
                }
              
            }
            dynamic o = new
            {
                UserId = user.Id,
                Count = remainFreeCount,
                Name = user.RealName,
                Gender = ((Vcyber.BLMS.Entity.Enum.EBUserGrade)user.MLevel).GetDiscribe(),
                Data=_result
            };

            

            return Json(o, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Add(AireportInputEntity entity)
        {
            ViewData["provinces"] = _AppContext.AirportServiceApp.SelectAirportProvince();
            if (entity.FreeCount > entity.ScheduleCount) return this.View();
            //todo: check verify code
            ReturnResult result = _AppContext.AirportServiceApp.GetServiceCard(entity.UserId, entity.Phone, entity.ScheduleCount, 0, int.Parse(entity.Airport), "Y", "blms");

            if (result.IsSuccess)
            {
                ViewBag.IsSuccess = true;
                return View();
            }
            else
            {
                ViewBag.IsSuccess = false;
                ViewBag.Message = result.Message;
                return View();
            }
            
        }

        [HttpGet]
        public ActionResult Add()
        {
            ViewData["provinces"] = _AppContext.AirportServiceApp.SelectAirportProvince();
            return this.View();
        }


        /// <summary>
        /// 根据省获取机场
        /// </summary>
        /// <param name="province"></param>
        /// <returns></returns>
        public ActionResult GetAirportsByProvince(string province)
        {
            IEnumerable<Airport> _result = _AppContext.AirportServiceApp.SelectAirportList(province, string.Empty);
            return View(_result);
        }

        ///// <summary>
        ///// 机场服务预约
        ///// </summary>
        ///// <returns></returns>
        //public JsonResult LiveReserve(string userId, string phoneNumber, int freeCount, int scoreCount, int airportId)
        //{
        //    if (!this.User.Identity.IsAuthenticated)
        //    {
        //        return Json(new ReturnResult() { IsSuccess = false, Message = "用户账号已过期" });
        //    }

        //    ReturnResult _entity = _AppContext.AirportServiceApp.GetServiceCard(userId, phoneNumber, freeCount, scoreCount, airportId);

        //    return Json(_entity);

        //    //IEnumerable<SNCard> cards = (IEnumerable<SNCard>)_entity.Data;

        //}

          public JsonResult SendVerifyCode(string phone)
        {
            ReturnResult result = _AppContext.UserSecurityApp.SendVerifyCodeWithMessage(phone, 5, "欢迎使用北京现代机场免费候机服务，温馨提示：以下5位验证码仅用于会员身份验证，并非机场服务码。请将5位验证码告知坐席人员，通过身份验证后，您将短信获取9位空港易行服务码，持空港易行服务码到达指定机场便可享受候机服务。");
            //ReturnResult result = new ReturnResult { IsSuccess = true };
            if (result.IsSuccess)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }

        public JsonResult VerifyCode(string phone, string code)
        {
            ReturnResult result = _AppContext.UserSecurityApp.ValidateMobileVerifyCode(phone, code);
            //ReturnResult result = new ReturnResult { IsSuccess = true };
            return Json(result.IsSuccess, JsonRequestBehavior.AllowGet);
        }
    }
}