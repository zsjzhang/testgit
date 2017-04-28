using System;
using System.Configuration;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Common.City;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.FrontWeb.Models;

namespace Vcyber.BLMS.FrontWeb.Controllers
{
    [MvcAuthorize]
    public class BlueActivityController : Controller
    {
        // GET: BlueActivity
        public ActionResult Index()
        {
            ViewBag.IsTimeout = false;
            var dt = Convert.ToDateTime(ConfigurationManager.AppSettings["BlueActivityTimeout"]);
            if (dt < DateTime.Now)
            {
                ViewBag.IsTimeout = true;
            }
            var userId = this.User.Identity.GetUserId();
            ViewBag.TotalBlueBean = _AppContext.UserBlueBeanApp.GetTotalBlueBean(userId);
            return View();
        }

        public ActionResult Query()
        {
            var userId = this.User.Identity.GetUserId();
            ViewBag.TotalBlueBean = _AppContext.UserBlueBeanApp.GetTotalBlueBean(userId);
            var list = _AppContext.BluebeanWinRecordApp.QueryWinRecords(50);
            return View(list);
        }

        public ActionResult Address(int s = 0, int t = 0)
        {
            var userId = this.User.Identity.GetUserId();
            ViewData["provinceList"] = CityService.Instance.GetProvince();
            var winRecord = _AppContext.BluebeanWinRecordApp.QueryWinRecordByUserId(userId);
            ViewBag.TotalBlueBean = _AppContext.UserBlueBeanApp.GetTotalBlueBean(userId);
            ViewBag.IsShowUsb = winRecord.Prize == "usb充电器";
            return View(winRecord);
        }
        public JsonResult QueryByPhone(string phone)
        {
            var result = new DataResult();
            string name = _AppContext.BluebeanWinRecordApp.QueryWinRecord(phone);
            result.Success = !string.IsNullOrEmpty(name);
            if (result.Success)
            {
                result.Status = name == "usb充电器" ? 1 : 2;
            }
            return Json(result);
        }

        [HttpPost]
        public JsonResult Address(BluebeanWinRecord bluebeanWinRecord)
        {
            var result = new DataResult();
            result.Success = _AppContext.BluebeanWinRecordApp.UpdateAddress(bluebeanWinRecord);
            return Json(result);
        }
        public JsonResult Drawluck()
        {
            var result = _AppContext.BluebeanWinRecordApp.DrawLuck(this.User.Identity.GetUserId());
            return Json(result);
        }
    }
}