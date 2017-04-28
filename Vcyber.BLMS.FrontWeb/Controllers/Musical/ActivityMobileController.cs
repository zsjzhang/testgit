using AspNet.Identity.SQL;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Domain.CarService;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.FrontWeb.Models;
using Microsoft.AspNet.Identity;

namespace Vcyber.BLMS.FrontWeb.Controllers.Musical
{
    public class ActivityMobileController : Controller
    {
        private ApplicationSignInManager _signInManager;
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set { _signInManager = value; }
        }

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
        public ActivityMobileController()
        {
        }

        public ActivityMobileController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }
        public static string name = "";
        //
        // GET: /GrabTicket/
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 微信二维码关注页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Follow()
        {
            return View();
        }

        public ActionResult GetLogin()
        {
            if (!this.User.Identity.IsAuthenticated)
            {
                return Json(new { msg = "N" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { msg = "true" }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Mobile(string source)
        {
            ViewBag.source = source;
            ViewData["provinceList"] = Vcyber.BLMS.Common.City.CityService.Instance.GetProvince();
            ViewData["powerWinnerList"] = _AppContext.WinningInfoApp.GetWinningsByWhere(1, " Prizesid=10 and UpdateTime >'2015-11-10'");
            ViewData["piaoWinerList"] = _AppContext.WinningInfoApp.GetWinningsByWhere(1, " Prizesid=9 and UpdateTime >'2015-11-10'");
            return View();
        }

        [HttpPost]
        public ActionResult JoinActivity(JoinActivity entity)
        {
            entity.ActivityId = 1;
            entity.UserId = string.Empty;
            entity.CreateDate = DateTime.Now;
            if (User.Identity.IsAuthenticated)
            {
                entity.UserId = User.Identity.Name;
                //获取vin码
                entity.Results1 = GetVinByUserId(entity.UserId);
            }
            int _result = _AppContext.JoinActivityApp.AddJoinActivityNew(entity);
            if (_result <= 0)
            {
                Json(new { msg = "N", id = _result }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { msg = "true", id = _result }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 用户选择面具
        /// </summary>
        /// <param name="id"></param>
        /// <param name="maskName"></param>
        /// <returns></returns>
        public ActionResult AddMaskToActivity(int id, string maskName)
        {
            JoinActivity _entity = _AppContext.JoinActivityApp.GetJoinActivityById(id);
            _entity.Results2 = maskName;
            if (_AppContext.JoinActivityApp.UpdateJoinActivity(_entity))
            {
                return Json(new { msg = "N" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { msg = "true" }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取用户vin码（如果存在索九或者全新途胜，则使用该车的vin码，否则就车主的任意车的vin码）
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public string GetVinByUserId(string userid)
        {
            string _vin = string.Empty;
            string _userid = string.Empty;
            ApplicationUser _curUser = UserManager.FindByName(userid);
            if (_curUser == null || string.IsNullOrEmpty(_curUser.Id))
            {
                return _vin;
            }
            IEnumerable<Car> _carList = _AppContext.CarServiceUserApp.SelectCarListByUserId(_curUser.Id);
            if (_carList == null || _carList.Count() <= 0)
            {
                return _vin;
            }
            if (_carList != null && _carList.Any())
            {
                foreach (var item in _carList)
                {
                    //如果是索九或者全新途胜则取vin码，否则取最后一个车的vin码
                    if (item.CarCategory == "索纳塔9" || item.CarCategory == "全新途胜" || item.CarCategory == "第九代索纳塔")
                    {
                        _vin = item.VIN;
                        break;
                    }
                    _vin = item.VIN;
                }
            }
            return _vin;
        }
        /// <summary>
        /// 分享数据统计
        /// </summary>
        /// <returns></returns>
        public ActionResult ShareStat(ShareRecord entity)
        {
            if (User.Identity.IsAuthenticated)
            {
                entity.UserId = User.Identity.Name;
            }
            _AppContext.ShareRecordApp.AddShareRecord(entity);
            return Json(new { msg = "true" }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult FindWinnerInfoByMobile(string mobile)
        {
            WinningInfo _winnerInfoEntity = _AppContext.WinningInfoApp.GetWinningByTelAndActicityId(1, mobile);
            return Json(new { code = 200, result = _winnerInfoEntity }, JsonRequestBehavior.AllowGet);
        }
    }
}