using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vcyber.BLMS.Application;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using Vcyber.BLMS.FrontWeb.Models;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Common.City;
using Vcyber.BLMS.Entity.Enum;

namespace Vcyber.BLMS.FrontWeb.Controllers.BmMember
{
    public class MoreMemberController : Controller
    {
        // GET: MoreMember
        public ActionResult Index(string source)
        {
            ViewBag.source = source;
            List<WinningModel> wmList = _AppContext.WinningInfoApp.GetWinningModelsByActivityId(2);
            ViewData["provinceList"] = Vcyber.BLMS.Common.City.CityService.Instance.GetProvince();
            ViewBag.wmList = wmList;
            return View();
        }

        public ActionResult Wap()
        {
            return View();
        }

        //判断是否登录
        public ActionResult IsLogin()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                return Content("ok");
            }
            return Content("no");
        }
        //开始活动
        [HttpPost]
        public ActionResult StartActivity(int id = 2, string source = null)
        {
            string userId = this.User.Identity.GetUserId();
            ActivityResult result = _AppContext.PrizesInfoApp.StartActivityInfo(userId, id, source, GetVinByUserId(userId));
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //完善信息
        [HttpPost]
        public ActionResult PerfectInformation(int id, string name, string phone, string address, string province, string city, string area)
        {
            WinningInfo entity = _AppContext.WinningInfoApp.GetWinningInfo(id);
            entity.UserName = name;
            entity.Address = address;
            entity.UserTel = phone;
            entity.Province = province;
            entity.City = city;
            entity.Area = area;
            bool bol = _AppContext.WinningInfoApp.UpdateWinningInfo(entity);
            return Content(bol ? "ok" : "no");
        }

        public string GetVinByUserId(string userid)
        {
            string _vin = string.Empty;
            IEnumerable<Car> _carList = _AppContext.CarServiceUserApp.SelectCarListByUserId(userid);
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
    }
}