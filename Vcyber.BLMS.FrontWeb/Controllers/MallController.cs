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
using Vcyber.BLMS.FrontWeb.Models;

namespace Vcyber.BLMS.FrontWeb.Controllers
{
    /// <summary>
    /// 商城
    /// </summary>
    public class MallController : Controller
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

        #region ==== 构造函数 ====

        public MallController()
        {
        }

        public MallController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        #endregion

        private int PAGESIZE = 1;
        /// <summary>
        /// 商城首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(int? pageIndex, int? categoryId)
        {
            //int _totalCount = 0;
            //ProductCondition _conditionEntity = new ProductCondition();
            //_conditionEntity.CategoryID = categoryId ?? 0;
            //IEnumerable<Product> _productList = _AppContext.ProductApp.GetPageProduct(_conditionEntity, new PageData() { Index = pageIndex ?? 0 }, out _totalCount);

            //ProductSearchCondition _searchCondition = new ProductSearchCondition();
            //IEnumerable<Product> _productList = _AppContext.ProductApp.GetProduct(_searchCondition, new PageData() { Index = pageIndex ?? 1,Size=PAGESIZE }, out _totalCount);
            //ViewData["ProductList"] = _productList;
            //ViewBag.totalCount = _totalCount;
            //ViewBag.pageIndex = pageIndex ?? 1;
            //ViewBag.pageSize = PAGESIZE;
            //ViewBag.pageCount = Math.Ceiling((_totalCount * 1.0) / PAGESIZE);
            return View();
        }

        /// <summary>
        /// 新品
        /// </summary>
        /// <returns></returns>
        public ActionResult NewProduct()
        {
            int _totalCount = 0;
            IEnumerable<Product> _result = _AppContext.ProductApp.GetProduct(Entity.Enum.EProductRecommend.XP, new PageData() { Index = 0, Size = 3 }, out _totalCount);
            return View(_result);
        }

        /// <summary>
        /// 热销商品
        /// </summary>
        /// <returns></returns>
        public ActionResult HotProduct()
        {
            int _totalCount = 0;
            IEnumerable<Product> _result = _AppContext.ProductApp.GetProduct(Entity.Enum.EProductRecommend.RX, new PageData() { Index = 0, Size = 3 }, out _totalCount);
            if (_result == null || _result.Count() <= 0)
            {
                _result = _AppContext.ProductApp.GetProduct(Entity.Enum.EProductRecommend.XP, new PageData() { Index = 0, Size = 3 }, out _totalCount);
            }
            return View(_result);
        }

        /// <summary>
        /// 商城兑换排行
        /// </summary>
        /// <returns></returns>
        public ActionResult ExchangeList()
        {
            int _totalCount = 0;
            IEnumerable<Product> _result = _AppContext.ProductApp.GetSalesProduct(new PageData() { Index = 0, Size = 3 }, out _totalCount);
            return View(_result);
        }

        /// <summary>
        /// 商城登录模块
        /// </summary>
        /// <returns></returns>
        public ActionResult LogonPage()
        {
            return View();
        }

        /// <summary>
        /// 商城登录成功模块
        /// </summary>
        /// <returns></returns>
        public ActionResult LogonSuccessPage()
        {
            ApplicationUser _result = null;
            int _totalScore = 0;
          //  int _totalBlueBean = 0;
            string _nickName = string.Empty;
        //    int _totalEmpiric = 0;
            string _userFaceImg = "/img/default_user.jpg";

            int _unReadMsgCount = 0;
            if (this.User.Identity.IsAuthenticated)
            {
                _result = UserManager.FindById(this.User.Identity.GetUserId());
                _totalScore = Vcyber.BLMS.Application._AppContext.UserIntegralApp.GetTotalIntegral(this.User.Identity.GetUserId());
              //  _totalBlueBean = Vcyber.BLMS.Application._AppContext.UserBlueBeanApp.GetTotalBlueBean(this.User.Identity.GetUserId());
                _nickName = _result.NickName;
             //   _totalEmpiric = _AppContext.UserEmpiricApp.TotalValue(this.User.Identity.GetUserId());
                _userFaceImg = _result.FaceImage;
                _unReadMsgCount = _AppContext.UserMessageRecordApp.GetUnReadMessage(this.User.Identity.GetUserId());
            }

            ViewBag.totalScore = _totalScore;
             // ViewBag.totalBlueBean = _totalBlueBean;
            ViewBag.nickName = _nickName;
           // ViewBag.totalEmpiric = _totalEmpiric;
            ViewBag.userFaceImg = _userFaceImg;
            ViewBag.UnReadMsgCount = _unReadMsgCount;
            return View(_result);
        }
    }
}