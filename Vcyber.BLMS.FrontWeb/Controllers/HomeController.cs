using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;

namespace Vcyber.BLMS.FrontWeb.Controllers
{
    /// <summary>
    /// 首页
    /// </summary>
    public class HomeController : Controller
    {

        private int HOTNEWSPAGESIZE = 4;


        public ActionResult Error()
        {
            return View();
        }
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Default(string s="pc")
        {
            //匹配web还是pc
            var agents = new string[] { "android", "iphone", "symbianos", "windows phone", "ipad", "ipod", "micromessenger" };
            var userAgent = Request.UserAgent.ToLower();
            if (agents.Count(x => userAgent.Contains(x)) > 0)
            {
                return Redirect(string.Format("http://m.bluemembers.com.cn"));//跳转到wap页面
            }

            ViewBag.Source = s;
            //第一：获取banner列表
            //第二：获取首页新闻推荐
            //第三：获取新闻分页列表
            return View();
        }

        /// <summary>
        /// app下载页面
        /// </summary>
        /// <returns></returns>
        public ActionResult AppDownLoad()
        {
            return View();
        }

        /// <summary>
        /// 在线客服
        /// </summary>
        /// <returns></returns>
        public ActionResult OnLineService()
        {
            var cookieValue = CookieHelper.GetCookieValue("CustomCookie");
            if (!string.IsNullOrWhiteSpace(cookieValue) && cookieValue.ToLower().Contains("webinspect"))
            {
                return Redirect("/Contents/error.htm");
            }

            return View();
        }
        /// <summary>
        /// 添加在线客服记录
        /// </summary>
        /// <returns></returns>
        public JsonResult AddOnLineService(OnlineServiceRecord service)
        {
            var result = _AppContext.OnlineServiceApp.AddOnLineService(service);
            if (result > 0)
            {
                return Json(new { IsSuccess = true });
            }
            else
            {
                return Json(new { IsSuccess = false });
            }
        }
        /// <summary>
        /// 微信二维码
        /// </summary>
        /// <returns></returns>
        public ActionResult WeiXinLayer()
        {
            return View();
        }

        /// <summary>
        /// 分享视图
        /// </summary>
        /// <returns></returns>
        public ActionResult ShareLayer()
        {
            return PartialView();
        }

        /// <summary>
        /// 预约入口区域
        /// </summary>
        /// <returns></returns>
        public ActionResult ReserveRegion()
        {
            return View();
        }
      

        /// <summary>
        /// 快捷入口区域
        /// </summary>
        /// <returns></returns>
        public ActionResult ShortcutRegion()
        {
            return View();
        }
        

        /// <summary>
        /// 热门新闻
        /// </summary>
        /// <returns></returns>
        public ActionResult HotNewsRegion(int? pageNum)
        {
            int _totalCount = 1;
            IEnumerable<News> _newsList = _AppContext.NewsApp.SelectHotNews(string.Empty, pageNum ?? 0, HOTNEWSPAGESIZE, out _totalCount);

            return View(_newsList);
        }

        /// <summary>
        /// Sonata底部热门礼品兑换
        /// </summary>
        /// <returns></returns>
        public ActionResult HotProductsRegion()
        {
            int _totalCount = 0;
            IEnumerable<Product> _result = _AppContext.ProductApp.GetProduct(EProductRecommend.RX, new PageData() { Index = 1, Size = 3 }, out _totalCount);
            return View(_result);
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult FindDealer()
        {
            ViewBag.Message = "查询经销商.";

            return View();
        }


        /// <summary>
        /// 首页索纳塔9专区轮播
        /// </summary>
        /// <returns></returns>
        public ActionResult SonataNice()
        {
            return PartialView();
        }

        public ActionResult DealerNew()
        {
            return PartialView();
        }

        public ActionResult WechatCode()
        {
            return View();
        }
    }
}