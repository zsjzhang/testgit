using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vcyber.BLMS.Application;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Vcyber.BLMS.Entity;
using System.Web.Script.Serialization;
using Vcyber.BLMS.FrontWeb.Models;

namespace Vcyber.BLMS.FrontWeb.Controllers
{
    /// <summary>
    /// 生活服务
    /// </summary>
    public class LiveController : Controller
    {
        private int ExchangeOnScore = 1800;


        #region ==== 私有字段 ====

        private ApplicationUserManager _userManager;

        private ApplicationSignInManager _signInManager;

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
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set { _signInManager = value; }
        }


        #endregion

        #region ==== 构造函数 ====

        public LiveController()
        {
        }

        public LiveController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        #endregion


        //Sonata--索九
        /// <summary>
        /// 生活服务首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(string source)
        {
            //if (!this.User.Identity.IsAuthenticated)
            //{
            //    return RedirectToAction("LogonPage", "Account", new { returnUrl = "/Live/Index" });
            //}
            //ViewBag.userId = this.User.Identity.GetUserId();
            ViewBag.source = source;
            return View();
        }

        /// <summary>
        /// 预约成功
        /// </summary>
        /// <returns></returns>
        public ActionResult ReserveSuccess()
        {
            if (!this.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("LogonPage", "Account");
            }
            ReturnResult _result = null;
            HttpCookie _httpCookieEntity = HttpContext.Request.Cookies.Get("sncodes");
            if (_httpCookieEntity != null)
            {
                string _resultValue = HttpContext.Server.UrlDecode(_httpCookieEntity.Value);
                JavaScriptSerializer jss = new JavaScriptSerializer();
                _result = jss.Deserialize<ReturnResult>(_resultValue);
            }

            ViewBag.SNCodes = Session["SNCODES"];

            return View(_result);

        }

        /// <summary>
        /// 预约失败
        /// </summary>
        /// <returns></returns>
        public ActionResult ReserveFailed()
        {
            return View();
        }

        /// <summary>
        /// 索九车主转向候机服务-预约
        /// </summary>
        /// <returns></returns>
        public ActionResult ReserveLayer(string source)
        {
            if (!this.User.Identity.IsAuthenticated)
            {
                return null;
            }

            ApplicationUser _userEntity = UserManager.FindById(this.User.Identity.GetUserId());

            if (_userEntity.MLevel < 10)
            {
                return null;
            }
            string _userId = this.User.Identity.GetUserId();

            int _totalScore = _AppContext.UserIntegralApp.GetTotalIntegral(_userId);
            int _totalBlueBean = _AppContext.UserBlueBeanApp.GetTotalBlueBean(_userId);
            int _surplusFreeFrequency = _AppContext.AirportServiceApp.GetSNCardNumber(_userId);
             
            if (_userEntity.MLevel == 11)
            {
                //银卡会员可领取次数
                _surplusFreeFrequency = 2 - _surplusFreeFrequency;
            }
            if (_userEntity.MLevel == 12)
            {
                //金卡会员可领取次数
                _surplusFreeFrequency = 3 - _surplusFreeFrequency;
            
            }
            ViewBag.SurplusFreeFrequency = _surplusFreeFrequency;
            ViewBag.surplusTotalScore = _totalScore;
            ViewBag.exchangeOnScore = ExchangeOnScore;
            ViewBag.userId = _userId;
            ViewBag.surplusScoreFrequency = (_totalScore / ExchangeOnScore);
            ViewData["provinces"] = _AppContext.AirportServiceApp.SelectAirportProvince();

            ViewBag.source = source;
            return View();
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


        /// <summary>
        /// 获取所有的机场
        /// </summary>
        /// <param name="province"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SelectAirportList()
        {
            IEnumerable<Airport> _result = _AppContext.AirportServiceApp.SelectAirportList();
            return Json(_result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 根据机场名称查询所有机场候机室
        /// </summary>
        /// <param name="airportName">机场名称</param>
        /// <returns></returns>
        public ActionResult GetAirportRoomsByAirportName(string airportName)
        {
            IEnumerable<Airport> _result = _AppContext.AirportServiceApp.SelectAirportRoomList(airportName);
            return View(_result);
        }

        /// <summary>
        /// 根据机场名称查询所有机场候机室
        /// </summary>
        /// <param name="airportName">机场名称</param>
        /// <returns></returns>
        public JsonResult GetAirportRoomListByAirportName(string airportName)
        {
            IEnumerable<Airport> _result = _AppContext.AirportServiceApp.SelectAirportRoomList(airportName);
            return Json(_result,JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 查询所有机场候机室
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AllAirportRoomList()
        {
            IEnumerable<Airport> _result = _AppContext.AirportServiceApp.AllAirportRoomList();
            return Json(_result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 机场服务预约
        /// </summary>
        /// <returns></returns>
        public JsonResult LiveReserve(string userId, string phoneNumber, int freeCount, int scoreCount, int airportId, string source)
        {
            if (!this.User.Identity.IsAuthenticated)
            {
                return Json(new ReturnResult() { IsSuccess = false, Message = "用户账号已过期" });
            }

            ReturnResult _entity = _AppContext.AirportServiceApp.GetServiceCard(userId, phoneNumber, freeCount, scoreCount, airportId, "N", source);

            Session["SNCODES"] = (IEnumerable<SNCard>)(_entity.Data);

            return Json(_entity);

            //IEnumerable<SNCard> cards = (IEnumerable<SNCard>)_entity.Data;

        }

        /// <summary>
        /// 积分兑换服务失败
        /// </summary>
        /// <returns></returns>
        public ActionResult ExchangeFailed()
        {
            return View();
        }
    }
}