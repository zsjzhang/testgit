using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Application;

namespace Vcyber.BLMS.FrontWeb.Controllers
{
    /// <summary>
    ///蓝缤服务
    /// </summary>
    public class BlueVIPController : Controller
    {
        //
        // GET: /BlueVIP/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MaintOnLine()
        {
            return View();
        }

        public ActionResult MaintOnLinePage(string carType, string KM)
        {
            IEnumerable<MaintCarOil> oils = null;
            IEnumerable<MaintCarPackage> packages = null;

            if (carType != null && KM != null)
            {
                oils = _AppContext.MaintCarOilApp.GetMaintCarOilList(carType);
                packages = _AppContext.MaintCarPackageApp.GetMaintCarPackageList(carType, KM);
            }

            ViewBag.Oils = oils;
            ViewBag.Packages = packages;

            return View();
        }
	}
}