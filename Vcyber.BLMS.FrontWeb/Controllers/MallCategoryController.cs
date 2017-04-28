using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.Entity.SelectCondition;

namespace Vcyber.BLMS.FrontWeb.Controllers
{
    public class MallCategoryController : Controller
    {
        private const int PAGESIZE = 9;
        //
        // GET: /MallCategory/
        public ActionResult Index(string levelOneCategory, string Name, int? pageNum)
        {
            string _levelOneCategory = Request.QueryString["levelOneCategory"];
            string _levelTwoCategory = Request.QueryString["levelTwoCategory"];
            string _exchangeType = Request.QueryString["exchangeType"];
            string _orderType = Request.QueryString["orderType"];
            int _category = 0;
            int.TryParse(levelOneCategory, out _category);
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

            IEnumerable<Product> _productList = _AppContext.ProductApp.GetProduct(_searchCondition, new PageData() { Index = 1, Size = PAGESIZE }, out _totalCount);



            ViewBag.levelOneCategory = _category;
            ViewBag.totalCount = _totalCount;
            ViewBag.pageCount = int.Parse(Math.Ceiling((_totalCount * 1.0) / PAGESIZE).ToString());
            ViewBag.pageIndex = pageNum ?? 1;
            ViewBag.pageSize = PAGESIZE;
            ViewBag.TitleName = Name;
            return View(_productList);
        }


        /// <summary>
        /// 商城页面一级分类
        /// </summary>
        /// <returns></returns>
        public ActionResult MallLevelOne()
        {
            IEnumerable<Category> _result = _AppContext.CategoryApp.GetList();
            return View(_result);
        }

        /// <summary>
        /// 商城二级分类
        /// </summary>
        /// <returns></returns>
        public ActionResult MallLevelTwo(int oneLevel)
        {
            IEnumerable<Category> _levelTwo = _AppContext.CategoryApp.GetList(oneLevel);
            ViewBag.levelOneCategory = oneLevel;
            return View(_levelTwo);
        }
    }
}