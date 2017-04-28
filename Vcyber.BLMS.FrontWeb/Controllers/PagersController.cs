using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Vcyber.BLMS.FrontWeb.Controllers
{
    public class PagersController : Controller
    {
        //
        // GET: /Pagers/
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 用户个人中心的分页
        /// </summary>
        /// <returns></returns>
        public ActionResult Pager1(int? pageIndex = 1, int? pageSize = 10, int? totalCount = 1)
        {
            double _totalCount = totalCount.Value * 1.0;
            ViewBag.totalCount = totalCount;
            ViewBag.pageIndex = pageIndex ?? 1;
            ViewBag.pageSize = pageSize;
            ViewBag.pageCount = Math.Ceiling(_totalCount / pageSize.Value);
            return View();
        }

        /// <summary>
        /// 用于商城的分页
        /// </summary>
        /// <returns></returns>
        public ActionResult MallPager(int? pageIndex = 1, int? pageSize = 10, int? totalCount = 0)
        {            
            double _totalCount = totalCount.Value * 1.0;
            ViewBag.totalCount = totalCount;
            ViewBag.pageIndex = pageIndex ?? 1;
            ViewBag.pageSize = pageSize ?? 10;
            ViewBag.pageCount = (int)Math.Ceiling(_totalCount / pageSize.Value);
            return View();
        }

        /// <summary>
        /// 用于商城的分页
        /// </summary>
        /// <returns></returns>
        public ActionResult MallPagerShop(string Category, int? pageIndex = 1, int? pageSize = 10, int? totalCount = 0)
        {
            ViewBag.levelOneCategory = Category;
            double _totalCount = totalCount.Value * 1.0;
            ViewBag.totalCount = totalCount;
            ViewBag.pageIndex = pageIndex ?? 1;
            ViewBag.pageSize = pageSize ?? 10;
            ViewBag.pageCount = (int)Math.Ceiling(_totalCount / pageSize.Value);
            return PartialView();
        }

        public ActionResult CommonPager(int? pageIndex = 1, int? pageSize = 10, int? totalCount = 0)
        {
            double _totalCount = totalCount.Value * 1.0;
            ViewBag.totalCount = totalCount;
            ViewBag.pageIndex = pageIndex ?? 1;
            ViewBag.pageSize = pageSize ?? 10;
            ViewBag.pageCount = (int)Math.Ceiling(_totalCount / pageSize.Value);
            return View();
        }

        public ActionResult WalletRecordsPager(int? pageIndex = 1, int? pageSize = 10, int? totalCount = 0)
        {
            double _totalCount = totalCount.Value * 1.0;
            ViewBag.totalCount = totalCount;
            ViewBag.pageIndex = pageIndex ?? 1;
            ViewBag.pageSize = pageSize ?? 10;
            ViewBag.pageCount = (int)Math.Ceiling(_totalCount / pageSize.Value);
            return View();
        }

        /// <summary>
        /// 论坛分页
        /// </summary>
        /// <returns></returns>
        public ActionResult BBSPager(int id, string dataViewPath, string conatinerid, int? pageIndex = 1, int? pageSize = 10, int? totalCount = 0)
        {
            double _totalCount = totalCount.Value * 1.0;
            ViewBag.totalCount = totalCount;
            ViewBag.pageIndex = pageIndex ?? 1;
            ViewBag.pageSize = pageSize ?? 10;
            ViewBag.pageCount = (int)Math.Ceiling(_totalCount / pageSize.Value);
            ViewBag.showId = id;
            ViewBag.dataViewPath = dataViewPath;
            ViewBag.containerId = conatinerid;
            return View();
        }
    }
}