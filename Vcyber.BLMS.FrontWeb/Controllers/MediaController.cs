using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.SelectCondition;

namespace Vcyber.BLMS.FrontWeb.Controllers
{
    public class MediaController : Controller
    {
        int pagesize = 10;

        //
        // GET: /Media/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List(int pageIndex = 1)
        {
            int start = (pageIndex-1) * pagesize;
            int total = 0;
            int fileType = 2;
            IEnumerable<ShareResources> _result = _AppContext.ShareResourcesApp.GetShareRes(new object(), fileType, string.Empty, start, pagesize, out total);

            int _pagecount = int.Parse(Math.Ceiling(Convert.ToDecimal(total) / pagesize).ToString());
            ViewData["pagecount"] = _pagecount;
            ViewData["pageindex"] = pageIndex;
            return View(_result);
        }

        public ActionResult Player(int id)
        {
            ShareResources _mediaEntity = _AppContext.ShareResourcesApp.GetShareResById(id);
            return View(_mediaEntity);
        }

        public ActionResult RelatedList(string category)
        {
            int total = 0;
            int fileType = 2;
            int start = 0;
            IEnumerable<ShareResources> _result = _AppContext.ShareResourcesApp.GetShareRes(new object(), fileType, category, start, pagesize, out total);
            return View(_result);
        }

        public ActionResult HotMedia()
        {
            int total = 0;
            int fileType = 2;
            int start = 0;
            int count = 1;
            IEnumerable<ShareResources> _result = _AppContext.ShareResourcesApp.GetShareRes(new object(), fileType, string.Empty, start, count, out total);
            //获取生日特权热销产品
            var categoryId = _AppContext.CategoryApp.selectIdByName("生日特权");
            ProductSearchCondition condition = new ProductSearchCondition();
            condition.CategoryID = categoryId;
            int PAGESIZE = 100;
            int _totalCount = 0;
            int pageIndex = 1;
            var _productList = _AppContext.ProductApp.GetProductByBirthday(condition, new PageData() { Index = pageIndex, Size = PAGESIZE }, out _totalCount);
            //返回查询出来的热销产品，如果没有查到热销产品就返回查询到产品的第一个
            var product = _productList.Where(t => t.IsRecommend == 2).FirstOrDefault() == null ? _productList.FirstOrDefault() : _productList.Where(t => t.IsRecommend == 2).FirstOrDefault();
            if (product != null) 
                product.Detail = _AppContext.ProductApp.GetDetail(product.ID);
            ViewBag.hotProduct = product;
            return View(_result);
        }

        public ActionResult PagerList(int pageIndex = 1)
        {
            int start = (pageIndex - 1) * pagesize;
            int total = 0;
            int fileType = 2;
            IEnumerable<ShareResources> _result = _AppContext.ShareResourcesApp.GetShareRes(new object(), fileType, string.Empty, start, pagesize, out total);
            return View(_result);
        }
    }
}