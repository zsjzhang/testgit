using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;

namespace Vcyber.BLMS.FrontWeb.Controllers
{
    public class BannerController : Controller
    {
        //
        // GET: /Banner/
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 首页banner视图
        /// </summary>
        /// <returns></returns>
        public ActionResult HomeBanner()
        {
            int _totalCount = 0;
            IEnumerable<ImageCarousel> _result = _AppContext.ImageCarouselApp.GetImageCarouselList((int)EApproveStatus.Approved, (int)EImageCarouselType.Home, 0, 5, out _totalCount);
            return View(_result);
        }

        /// <summary>
        /// 商城banner
        /// </summary>
        /// <returns></returns>
        public ActionResult MallBanner()
        {
            int _totalCount = 0;
            IEnumerable<ImageCarousel> _result = _AppContext.ImageCarouselApp.GetImageCarouselList((int)EApproveStatus.Approved, (int)EImageCarouselType.Product, 0, 3, out _totalCount);


            return View(_result);
        }

        /// <summary>
        /// 活动中心banner视图
        /// </summary>
        /// <returns></returns>
        public ActionResult
            ActivesBanner()
        {
            int _totalCount = 0;
            IEnumerable<ImageCarousel> _result = _AppContext.ImageCarouselApp.GetImageCarouselList((int)EApproveStatus.Approved, (int)EImageCarouselType.Activities, 0, 3, out _totalCount);
            return View(_result);
        }

        /// <summary>
        /// 新闻banner视图
        /// </summary>
        /// <returns></returns>
        public ActionResult NewsBanner()
        {
            int _totalCount = 0;
            IEnumerable<ImageCarousel> _result = _AppContext.ImageCarouselApp.GetImageCarouselList((int)EApproveStatus.Approved, (int)EImageCarouselType.News, 0, 3, out _totalCount);
            return View(_result);
        }

        /// <summary>
        /// 新闻6+1服务banner视图
        /// </summary>
        /// <returns></returns>
        public ActionResult Home7Banner()
        {
            int _totalCount = 0;
            IEnumerable<ImageCarousel> _result = _AppContext.ImageCarouselApp.GetImageCarouselList((int)EApproveStatus.Approved, (int)EImageCarouselType.Home7, 0, 7, out _totalCount);
            return View(_result);
        }

    }
}