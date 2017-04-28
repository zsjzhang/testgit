using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.FrontWeb.Controllers
{
    public class UserGuideController : Controller
    {
        // GET: UserGuide
        public ActionResult Index()
        {
            int total;
            var _result = _AppContext.UserGuideApp.GetUserGuideList(1, 0, 10, out total);
            return View(_result);
        }
    }
}