using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vcyber.BLMS.Application;

namespace VcyBer.BLMS.MobileWeb.Controllers
{
    public class OnlineReadController : BaseController
    {
        // GET: OnlineRead
        public ActionResult Index()
        {
            var result = _AppContext.MagazineApp.GetMagazineAll().ToList();

            List<int> years = new List<int>();
            years = result.Select(y => y.Year).Distinct().ToList();
            ViewBag.Years = years;
            return View(result);
        }
    }
}