using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.SelectCondition;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;

namespace Vcyber.BLMS.FrontWeb.Controllers
{
    /// <summary>
    /// 会员
    /// </summary>
    public class MemberController : Controller
    {
        #region ==== 私有字段 ====

        private ApplicationUserManager _userManager;

        private ApplicationSignInManager _signInManager;

        #endregion

        #region ==== 公共属性 ====

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
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set { _signInManager = value; }
        }


        #endregion

        private int PAGESIZE = 6;
        /// <summary>
        /// 会员计划
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 会员杂志（与新闻中的杂志列表可能重复，二选一即可）
        /// </summary>
        /// <returns></returns>
        public ActionResult Magazine()
        {
            ViewData["MagazineList"] = null;
            return View();
        }

        public ActionResult birthday(int? pageNum)
        {
            //获取生日特权商品类型ID
            var categoryId = _AppContext.CategoryApp.selectIdByName("生日特权");
           ProductSearchCondition condition = new ProductSearchCondition();
           condition.CategoryID = categoryId;
           int _totalCount = 0;
           int pageIndex = pageNum ?? 1;
           var _productList = _AppContext.ProductApp.GetProductByBirthday(condition, new PageData() { Index = pageIndex, Size = PAGESIZE }, out _totalCount);
           var pdetail = new List<ProductDetail>();
           if (_productList.ToList().Count > 0)
           {
               foreach (var item in _productList)
               {
                    pdetail.Add(_AppContext.ProductApp.GetDetail(item.ID));                   
               }
           }
           ViewBag.totalCount = _totalCount;
           ViewBag.pageCount = int.Parse(Math.Ceiling((_totalCount * 1.0) / PAGESIZE).ToString());
           ViewBag.pageIndex = pageIndex;
           ViewBag.pageSize = PAGESIZE;
           ViewBag.categoryId = categoryId;
           ViewBag.pdetailList = pdetail;
           return View(_productList);
        }

        public ActionResult ajaxBirthday(int? pageNum,int categoryId)
        {
            if (this.User.Identity.IsAuthenticated)
            {
                var LoginUser = UserManager.FindById(this.User.Identity.GetUserId());
                ViewBag.birthday = LoginUser.Birthday;
                ViewBag.mlevel = LoginUser.MLevel;
            }
            else
            {
                ViewBag.birthday = string.Empty;
                ViewBag.mlevel = 0;
            }
            ProductSearchCondition condition = new ProductSearchCondition();
            condition.CategoryID = categoryId;
            int _totalCount = 0;
            int pageIndex = pageNum ?? 1;
            var _productList = _AppContext.ProductApp.GetProductByBirthday(condition, new PageData() { Index = pageIndex, Size = PAGESIZE }, out _totalCount);
            var pdetail = new List<ProductDetail>();
            if (_productList.ToList().Count > 0)
            {
                foreach (var item in _productList)
                {
                    pdetail.Add(_AppContext.ProductApp.GetDetail(item.ID));
                }
            }
            ViewBag.pdetailList = pdetail;
            return View(_productList);
        }

        /// <summary>
        /// 生日产品点击立即领取权限逻辑判断
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public  ActionResult checkpost()
        {
            if (!this.User.Identity.IsAuthenticated)
            {
                return Json(new { code = 301, msg = "请登录后领取" });
            }
            //获取生日特权商品类型ID
            var categoryId = _AppContext.CategoryApp.selectIdByName("生日特权");
            var LoginUser = UserManager.FindById(this.User.Identity.GetUserId());
            if (LoginUser == null)
            {
                return Json(new { code = 301, msg = "登录已失效,请重新登录" });
            }
            if (LoginUser.MLevel != 12)
            {
                return Json(new { code = 301, msg = "该权益只有金卡会员可以享受哦" });
            }
            var month = DateTime.Parse(LoginUser.Birthday).Month;
            var nowMonth = DateTime.Now.Month;
            if (month!=nowMonth)
            {
                return Json(new { code = 301, msg = "您好,本活动只针对生日当月的用户！" });
            }
            if (_AppContext.ProductApp.checkProduct(categoryId, this.User.Identity.GetUserId()) > 0)
            {
                 return Json(new { code = 301, msg = "您好,您已领取了生日礼物，不要太贪心哦！" });
            }
            //用户验证成功，前台JS添加到购物车进行购买
            return Json(new { code = 302, msg = "success" });
        }
    }
}