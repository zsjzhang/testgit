using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Vcyber.BLMS.Application;
using Vcyber.Pay.FrontWeb.Models;

namespace Vcyber.BLMS.FrontWeb.Controllers
{
    public class HeaderController : Controller
    {
        //
        // GET: /Header/
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// header
        /// </summary>
        /// <param name="pageName"></param>
        /// <returns></returns>
        public ActionResult HomeHeader(string pageName, bool showMenu = true)
        {
            ViewBag.showMenu = showMenu;
            ViewBag.curPageName = pageName;
            ViewBag.IsHasUnReadMsg = false;
            ViewBag.showMenu = showMenu;
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.IsHasUnReadMsg = _AppContext.UserMessageRecordApp.GetUnReadMessage(User.Identity.GetUserId()) > 0;
            }
            return View();
        }
    }
}