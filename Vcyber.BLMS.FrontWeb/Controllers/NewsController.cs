using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace Vcyber.BLMS.FrontWeb.Controllers
{
    /// <summary>
    /// 新闻
    /// </summary>
    public class NewsController : Controller
    {
        private int PAGESIZE = 1;
        private int HOTNEWSPAGESIZE = 6;
        private int MAGAZINESIZE = 1;

        private int PAGESIZE_LIST = 6;
        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }


        //
        // GET: /News/
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 首页热门新闻
        /// </summary>
        /// <returns></returns>
        public ActionResult HotNews(int? pageNum)
        {
            int _totalCount = 1;
            IList<News> _HotNewsList = null;
            IEnumerable<News> _newsList = _AppContext.NewsApp.SelectHotNews(string.Empty, pageNum ?? 0, HOTNEWSPAGESIZE, out _totalCount);
            if (_newsList != null)
            {
                _HotNewsList = _newsList.ToList();
            }
            return View(_HotNewsList);
        }

        /// <summary>
        /// 新闻分页列表
        /// </summary>
        /// <returns></returns>
        public ActionResult NewsPages(int? pageNum)
        {
            int _totalCount = 1;
            IList<News> _result = null;
            IEnumerable<News> _newsList = _AppContext.NewsApp.Select("", 1, (int)EApproveStatus.Approved, string.Empty, pageNum ?? 0, PAGESIZE_LIST, out _totalCount);
            if (_newsList != null && _newsList.Any())
            {
                _result = _newsList.ToList();
            }
            ViewBag.totalCount = _totalCount;
            ViewBag.pageIndex = pageNum ?? 1;
            ViewBag.pageSize = PAGESIZE_LIST;
            return View(_result);
        }

        public ActionResult NewsListForPager(int? pageNum)
        {
            int pageIndex = 0;
            if (pageNum == null || pageNum <= 0)
            {
                pageNum = 1;
            }

            pageIndex = pageNum ?? 1;
            int _totalCount = 1;
            IList<News> _result = null;
            IEnumerable<News> _newsList = _AppContext.NewsApp.Select("", 1, (int)EApproveStatus.Approved, string.Empty, (pageIndex - 1) * PAGESIZE_LIST, PAGESIZE_LIST, out _totalCount);
            if (_newsList != null && _newsList.Any())
            {
                _result = _newsList.ToList();
            }
            return View(_result);
        }

        /// <summary>
        /// 新闻详情
        /// </summary>
        /// <param name="id">新闻Id</param>
        /// <returns></returns>
        public ActionResult NewsDetail(int id)
        {
            News _result = _AppContext.NewsApp.GetNewsById(id);
            return View(_result);
        }

        /// <summary>
        /// 热门杂志
        /// </summary>
        /// <returns></returns>
        public ActionResult HotMagazine(int? pageNumber)
        {
            int total = 0;
            IEnumerable<Magazine> _result = _AppContext.MagazineApp.GetMagazineList((int)EApproveStatus.Approved, null, null, string.Empty, pageNumber ?? 0, MAGAZINESIZE, out total);
            ViewBag.totalCount = total;
            return View(_result);
        }

        /// <summary>
        /// 杂志列表
        /// </summary>
        /// <returns></returns>
        public ActionResult MagazinePages(int? pageNumber)
        {
            Dictionary<int, List<Magazine>> dic = new Dictionary<int, List<Magazine>>();
            //var _result = _AppContext.MagazineApp.GetMagazineAll().GroupBy(e => e.Year).ToDictionary<List<Magazine>, int>((e) => 
            //{
            //    int year=0;
            //    e.All(i => 
            //    {
            //        year = i.Year;
            //        return true;
            //    });
            //    return year;
            //});

            var _result = _AppContext.MagazineApp.GetMagazineAll();
            foreach (var it in _result)
            {
                int index = 0;
                if (!dic.Keys.Contains(it.Year))
                {
                    dic.Add(it.Year, new List<Magazine> { it });
                }
                else
                {
                    List<Magazine> mags = dic[it.Year];
                    mags.Add(it);
                }
                index++;
            }


            return View(dic);
        }
        public ActionResult MagazinePage()
        {
            var result = _AppContext.MagazineApp.GetMagazineAll().ToList();
            
            List<int> years=new List<int>();
            years = result.Select(y => y.Year).Distinct().ToList();
            ViewBag.Years = years;

            return View(result);
        }

        [HttpGet]
        public JsonResult AddBlueMember()
        {
            //if (this.User.Identity.IsAuthenticated)
            //{
            //    var _userid = this.User.Identity.GetUserId();
            //    var frontUser = UserManager.FindById(_userid);
            //    int blueBeanValue;
            //    _AppContext.BreadApp.BlueBeanBread(EBRuleType.下载车主杂志, _userid, (MemshipLevel)frontUser.MLevel, out blueBeanValue);
            //    _AppContext.BreadApp.EmpiricBread(EEmpiricRule.下载车主杂志, _userid, out blueBeanValue);
            //}
            return Json(new { tag = "success" });
        }

        public ActionResult HotManual()
        {
            int total = 0;
            IEnumerable<UserGuide> _newsList = _AppContext.UserGuideApp.GetUserGuideList(null, 0, 5, out total);
            return View(_newsList);
        }


        /// <summary>
        /// 首页纸质杂志申请
        /// </summary>
        /// <returns></returns>
        public ActionResult MagazineApply()
        {
            ViewData["provinceList"] = Vcyber.BLMS.Common.City.CityService.Instance.GetProvince();
            ViewData["magazineList"] = _AppContext.MagazineApp.GetMagazineAll().ToList();
            return View();
        }

        /// <summary>
        /// 首页纸质杂志申请
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult MagazineApply(MagazineApply magazineApply)
        {
            if (string.IsNullOrEmpty(magazineApply.ReceiveName))
            {
                return Json(new { code = 400, msg = "请填写您的姓名" }, JsonRequestBehavior.AllowGet);
            }

            if (string.IsNullOrEmpty(magazineApply.Phone))
            {
                return Json(new { code = 400, msg = "请正确填写您的手机号" }, JsonRequestBehavior.AllowGet);
            }

            if (string.IsNullOrEmpty(magazineApply.ZipCode))
            {
                return Json(new { code = 400, msg = "请填写您的电子邮箱" }, JsonRequestBehavior.AllowGet);
            }
            try
            {
                if (this.User.Identity.IsAuthenticated)
                {
                    var _userid = this.User.Identity.GetUserId();
                    //var frontUser = UserManager.FindById(_userid);
                    //int blueBeanValue;
                    //_AppContext.BreadApp.BlueBeanBread(EBRuleType.订阅车主杂志, _userid, (MemshipLevel)frontUser.MLevel, out blueBeanValue);
                    //_AppContext.BreadApp.EmpiricBread(EEmpiricRule.订阅车主杂志, _userid, out blueBeanValue);

                    magazineApply.UserId = _userid;
                }

                string id = string.Empty;
                magazineApply.CreateTime = DateTime.Now;
                magazineApply.UpdateTime = DateTime.Now;//Convert.ToDateTime(System.Data.SqlTypes.SqlDateTime.MinValue.ToString());
                magazineApply.MagazineTitle = string.Empty;
                //magazineApply.Status = 0;
                _AppContext.MagazineApplyApp.Add(magazineApply, out id);
            }
            catch (Exception)
            {
                //日志操作
                //.....

                return Json(new { code = 400, msg = "提交申请异常，请您稍后重试。" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { code = 200 }, JsonRequestBehavior.AllowGet);
        }


    }
}