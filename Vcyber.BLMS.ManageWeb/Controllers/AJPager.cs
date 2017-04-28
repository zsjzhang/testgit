using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Vcyber.BLMS.ManageWeb.Controllers
{
    [MvcAuthorize]
    public class AJPager : Controller
    {
        public ActionResult Pager(int pageIndex, int pageCount, string formId)
        {
            ViewData["pageIndex"] = pageIndex;
            ViewData["pageCount"] = pageCount;
            ViewData["formId"] = formId;
            return View();

        }
    }
}