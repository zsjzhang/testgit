using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VcyBer.BLMS.MobileWeb.Controllers
{
    public class MoreServiceController : BaseController
    {
        // GET: ServiceMore
        public ActionResult Index()
        {
            ViewBag.CurrDate = string.Format("{0:MM/dd} {1}", DateTime.Now, Week());
            return View();
        }
        private string Week()
        {
            string[] weekdays = { "星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六" };
            string week = weekdays[Convert.ToInt32(DateTime.Now.DayOfWeek)];
            return week;
        }
        /// <summary>
        /// 附近加油站，停车场
        /// </summary>
        /// <returns></returns>
        public ActionResult GasStationCarPark()
        {
            string maptype = Request["maptype"];
            @ViewBag.MapType = maptype;
            return View();
        }
    }
}