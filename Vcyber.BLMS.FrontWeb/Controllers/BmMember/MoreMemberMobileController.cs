using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.FrontWeb.Controllers.BmMember
{
    public class MoreMemberMobileController : Controller
    {
        //
        // GET: /MoreMemberMobile/
        public ActionResult Index(string source)
        {
            ViewBag.source = source;
            return View();
        }

        public ActionResult Game(string source)
        {
            ViewBag.source = source;
            ViewData["provinceList"] = Vcyber.BLMS.Common.City.CityService.Instance.GetProvince();
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

        /// <summary>
        /// 游戏规则
        /// </summary>
        /// <returns></returns>
        public ActionResult Rule()
        {
            return View();
        }

        public ActionResult Prize()
        {
            List<WinningModel> wmList = _AppContext.WinningInfoApp.GetWinningModelsByActivityId(2);
            ViewBag.wmList = wmList;
            return View();
        }
        /// <summary>
        /// 游戏介绍
        /// </summary>
        /// <returns></returns>
        public ActionResult Introduction()
        {
            return View();
        }
	}
}