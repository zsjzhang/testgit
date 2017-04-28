using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.Entity.SelectCondition;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
namespace Vcyber.BLMS.FrontWeb.Controllers
{
    /// <summary>
    /// 商品
    /// </summary>
    public class ProductController : Controller
    {
        private int PAGESIZE = 9;
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

        public ProductController()
        {
        }

        public ProductController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        #endregion

        //
        // GET: /Product/
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 商品列表
        /// </summary>
        /// <returns></returns>
        public ActionResult ProductListByMall(IEnumerable<Product> productList, int? totalCount)
        {
            IEnumerable<Product> _result = productList;
            ViewBag.totalCount = totalCount ?? 0;
            //int _totalCount = 0;
            //ProductSearchCondition _searchCondition = new ProductSearchCondition();
            //IEnumerable<Product> _result = _AppContext.ProductApp.GetPageProduct(oneLevel, new PageData() { Index = pageNum ?? 1, Size = PAGESIZE }, out _totalCount);
            return View(_result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public ActionResult ProductPagers(int? pageIndex)
        {
            int _totalCount = 0;
            ProductSearchCondition _searchCondition = new ProductSearchCondition();
            IEnumerable<Product> _productList = _AppContext.ProductApp.GetProduct(_searchCondition, new PageData() { Index = pageIndex ?? 1, Size = PAGESIZE }, out _totalCount);
            ViewData["ProductList"] = _productList;
            ViewBag.totalCount = _totalCount;
            ViewBag.pageIndex = pageIndex ?? 1;
            ViewBag.pageSize = PAGESIZE;
            ViewBag.pageCount = Math.Ceiling((_totalCount * 1.0) / PAGESIZE);
            return View(_productList);
        }

        public ActionResult AjaxProductPagers(int? pageIndex)
        {
            string _levelOneCategory = Request.QueryString["levelOneCategory"];
            string _levelTwoCategory = Request.QueryString["levelTwoCategory"];
            string _exchangeType = Request.QueryString["exchangeType"];
            string _orderType = Request.QueryString["orderType"];
            int _totalCount = 0;
            ProductSearchCondition _searchCondition = new ProductSearchCondition();
            if (!string.IsNullOrEmpty(_levelOneCategory))
            {
                _searchCondition.CategoryID = int.Parse(_levelOneCategory);
            }
            if (!string.IsNullOrEmpty(_levelTwoCategory))
            {
                _searchCondition.CategoryID = int.Parse(_levelTwoCategory);
            }
            if (!string.IsNullOrEmpty(_exchangeType))
            {
                if (_exchangeType == "1")
                {
                    _searchCondition.PayType = EPayType.Integral;
                }
                else if (_exchangeType == "2")
                {
                    _searchCondition.PayType = EPayType.BlueBean;
                }
            }
            if (!string.IsNullOrEmpty(_orderType))
            {
                if (_orderType == "1")
                {

                    _searchCondition.PXModel = EPaiXuMode.JF;
                }
                else if (_orderType == "2")
                {

                    _searchCondition.PXModel = EPaiXuMode.LD;
                }
                else if (_orderType == "3")
                {
                    _searchCondition.PXModel = EPaiXuMode.XL;
                }
            }
            IEnumerable<Product> _productList = _AppContext.ProductApp.GetProduct(_searchCondition, new PageData() { Index = pageIndex ?? 1, Size = PAGESIZE }, out _totalCount);
            ViewBag.totalCount = _totalCount;
            return View(_productList);
        }

        /// <summary>
        /// 商品详情
        /// </summary>
        /// <returns></returns>
        public ActionResult ProductDetail(string productId)
        {

            if (this.User.Identity.IsAuthenticated)
            {
                var LoginUser = UserManager.FindById(this.User.Identity.GetUserId());
                ViewBag.mlevel = LoginUser.MLevel;
            }
                int _productId = 0;
                if (!int.TryParse(productId, out _productId) || _productId <= 0)
                {
                    return RedirectToAction("Index", "Mall");
                }
                Product _result = _AppContext.ProductApp.GetProductById(_productId);//获取商品描述信息
                var categoryId = _AppContext.CategoryApp.selectIdByName("生日特权");//获取生日特权的类型ID
                var categoryCarId = _AppContext.CategoryApp.selectIdByName("汽车精品");//获取生活用品的类型ID
                var categoryLifeId = _AppContext.CategoryApp.selectIdByName("生活用品");//获取汽车精品的类型Id
                var productTypeId = _AppContext.ProductApp.GetProductCategory(_productId).CategoryID;//获取该商品的类型
                if (_result != null && !string.IsNullOrEmpty(_result.Description))
                {
                    _result.Description = System.Web.HttpUtility.UrlDecode(_result.Description);
                }
                ViewBag.productId = _productId;
                ViewBag.isBlock = (categoryId == productTypeId);//判断是否是生日特权产品
                ViewBag.isCarBlock = (categoryCarId == productTypeId);//判断是否是汽车精品类型的商品
                ViewBag.isLifeBlock = (categoryLifeId == productTypeId);//判断是否为生活用品类型的商品
                ViewBag.category = _result.Category;
                ProductDetail _productDetailItem = _AppContext.ProductApp.GetDetail(_productId);
                ViewData["productDetailItem"] = _productDetailItem;
                return View(_result);

            
          

        }

        /// <summary>
        /// 商品的分类列表
        /// </summary>
        /// <returns></returns>
        public ActionResult ProductCategoryByMall()
        {
            IEnumerable<Category> _result = _AppContext.CategoryApp.GetList();
            return View(_result);
        }

        /// <summary>
        /// 商品的兑换排行
        /// </summary>
        /// <returns></returns>
        public ActionResult ExchangeProductListByMall()
        {
            int _totalCount = 0;
            IEnumerable<Product> _result = _AppContext.ProductApp.GetSalesProduct(new PageData() { Index = 0, Size = 3 }, out _totalCount);
            return View(_result);
        }

        /// <summary>
        /// 左侧热门商品
        /// </summary>
        /// <returns></returns>
        public ActionResult HotExchangeProductList()
        {
            int _totalCount = 0;
            IEnumerable<Product> _result = _AppContext.ProductApp.GetProduct(Entity.Enum.EProductRecommend.RX, new PageData() { Index = 0, Size = 1 }, out _totalCount);
            if (_result == null || _result.Count() <= 0)
            {
                _result = _AppContext.ProductApp.GetProduct(Entity.Enum.EProductRecommend.XP, new PageData() { Index = 0, Size = 1 }, out _totalCount);
            }
            return View(_result);
        }

        /// <summary>
        /// 商城分页获取数据
        /// </summary>
        /// <returns></returns>
        public ActionResult QueryProductList(int? pageIndex, int? categoryId)
        {
            int _totalCount = 0;
            ProductCondition _conditionEentity = new ProductCondition();
            PageData _pageDateEntity = new PageData();
            _pageDateEntity.Index = pageIndex ?? 1;
            IEnumerable<Product> _result = _AppContext.ProductApp.GetPageProduct(_conditionEentity, _pageDateEntity, out _totalCount);
            return PartialView(_result);
        }


        /// <summary>
        /// 首页底部商城商品推荐
        /// </summary>
        /// <returns></returns>
        public ActionResult MallRecommend()
        {
            int _totalCount = 0;
            IEnumerable<Product> _result = _AppContext.ProductApp.GetProduct(EProductRecommend.RX, new PageData() { Index = 1, Size = 3 }, out _totalCount);
            if (_result == null || _result.Count() <= 0)
            {
                _result = _AppContext.ProductApp.GetProduct(EProductRecommend.XP, new PageData() { Index = 1, Size = 3 }, out _totalCount);
            }
            return View(_result);
        }

    }
}