using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.Entity.SelectCondition;
using Vcyber.BLMS.Application;
using System.IO;
using System.Text;
using Vcyber.BLMS.Common;

namespace Vcyber.BLMS.ManageWeb.Controllers
{
    /// <summary>
    /// 商品管理
    /// </summary>
    [MvcAuthorize]
    public class ProductController : Controller
    {
        #region ==== 构造函数 ====

        public ProductController() { }

        #endregion

        #region ==== 公共方法 ====

        public ActionResult Index()
        {
            return View();
        }
        [ValidateAntiForgeryToken]
        public ActionResult PartialPage(int categoryId = 0, int state = -1, int recommend = 0, string name = null,int yjx=0,int index = 1, int size = 10)
        {
            ProductCondition condition = new ProductCondition()
            {
                Recommend = null,
                CategoryID = categoryId,
                Name = name,
                Yjx=yjx
            };

            if (state < 0)
            {
                condition.State = null;
            }
            else
            {
                condition.State = state == EProductState.SoldOut.ToInt32() ? EProductState.SoldOut : EProductState.Putaway;
            }

            if (recommend == 0)
            {
                condition.Recommend = null;
            }
            else
            {
                condition.Recommend = recommend == EProductRecommend.RX.ToInt32() ? EProductRecommend.RX : EProductRecommend.XP;
            }


            int total = 0;
            var list = _AppContext.ProductApp.GetPageProduct(condition, new PageData() { Index = index, Size = size }, out total);

            int count = (int)Math.Ceiling((double)total / (double)size);
            ViewBag.PageIndex = index;
            ViewBag.PrePage = index > 1 ? (index - 1) : 1;
            ViewBag.NextPage = index < count ? (index + 1) : count;
            ViewBag.EndPage = count;

            return PartialView(list);
        }

        //删除
        public ActionResult Delete(int id)
        {
            try
            {
                bool bol = _AppContext.ProductApp.DeleteProduct(id);

                if (bol)
                {
                    return Redirect("/Product/Index");
                }
            }
            catch (RepeatException ex)
            {
                return Content("<script type='text/javascript'>alert('未处理的订单中存在此商品'); location.href='/Product/Index';</script>");
            }

            return Content("<script type='text/javascript'>alert('商品删除失败'); location.href='/Product/Index';</script>");
        }

        //下架
        [HttpPost]
         [ValidateAntiForgeryToken]
        public ActionResult PutOff(int id)
        {
            Product entity = _AppContext.ProductApp.GetProduct(id);

            if (entity != null)
            {
                entity.State = EProductState.SoldOut.ToInt32();
                entity.Updatetime = DateTime.Now;
                bool bol = _AppContext.ProductApp.EditProduct(entity);

                if (bol)
                {
                    return Content("ok");
                }

                return Content("no");
            }

            return Content("no");
        }

        //上架
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PutOn(int id)
        {
            Product entity = _AppContext.ProductApp.GetProduct(id);

            if (entity != null)
            {
                entity.State = EProductState.Putaway.ToInt32();
                entity.Updatetime = DateTime.Now;
                bool bol = _AppContext.ProductApp.EditProduct(entity);

                if (bol)
                {
                    return Content("ok");
                }

                return Content("no");
            }

            return Content("no");
        }
        [ValidateAntiForgeryToken]
        public JsonResult SetDefault(int productId, int imgId)
        {
            _AppContext.ProductApp.SetDefaultImage(productId, imgId);
            return new JsonResult() { Data = new { Message = "设置成功" } };
        }

        //修改	
        public ActionResult Edit(int id = -1)
        {
            Product entity = _AppContext.ProductApp.GetProduct(id);

            if (entity != null)
            {
                entity.Detail = _AppContext.ProductApp.GetDetail(id);
                int imgCount = entity.Images.Count;

                for (int i = 0; i < 4 - imgCount; i++)
                {
                    entity.Images.Add(new ProductImage() { Image = "" });
                }
            }
            else
            {
                entity = new Product();
                entity.ParentCategory = new Category();
                entity.ChildCategory = new Category();
                entity.Category = new ProductCategory();
                entity.Detail = new ProductDetail();
                entity.Images = new List<ProductImage>();

                for (int i = 0; i < 4; i++)
                {
                    entity.Images.Add(new ProductImage() { Image = "" });
                }
            }

            return View(entity);
        }
        /// <summary>
        /// 新增商品
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(Product entity)
        {

            if (entity!=null && entity.Images!=null && entity.Images.Count>0)
            {
               
                foreach (var item in entity.Images)
                {
                    if (item.Image!=null)
                    {
                         item.Image = Vcyber.BLMS.Common.FilterStr.FormatHTML(item.Image);
                    }
                   
                }
            }
            //if (FilterStr.IsFlag<Product>(entity))
            //{
            //    return Redirect("/Content/error.htm");
            //}

            if (ModelState.IsValid)
            {
                ProductCategory productCategory = new ProductCategory();
                productCategory.CategoryID = entity.ChildCategory.ID == -1 ? entity.ParentCategory.ID : entity.ChildCategory.ID;
                entity.Category = productCategory;

                entity.Updatetime = DateTime.Now;
                entity.ShelfTime = DateTime.Now;
                var productData = _AppContext.ProductApp.GetProduct(entity.ID);

                if (productData == null)
                {
                    _AppContext.ProductApp.AddProduct(entity);
                }
                else
                {
                    bool bol = _AppContext.ProductApp.EditProduct(entity);

                    if (entity.Detail.ID == 0)
                    {
                        entity.Detail.ProductID = entity.ID;
                        _AppContext.ProductApp.AddDetail(entity.Detail);
                    }
                    else
                    {
                        entity.Detail.Description = entity.Detail.Description;
                        _AppContext.ProductApp.EditDetail(entity.Detail);
                    }
                }

                return this.Redirect("/Product/Index");// RedirectToAction("Index");
            }

            return View(entity);
        }

        /// <summary>
        /// 上传商品图片
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UploadImage()
        {
            HttpContextBase context = this.HttpContext;

            if (context.Request.Files != null && context.Request.Files.Count > 0)
            {
                HttpPostedFileBase file = context.Request.Files[0];
                string extendsName = Path.GetExtension(file.FileName);

                if (!this.imgExtends.Contains(extendsName.ToLower()) || file.ContentLength > 10485760)
                {
                    return Content("");
                }

                string newFileName = Guid.NewGuid().ToString("N") + extendsName;

                if (!Directory.Exists(HttpContext.Server.MapPath("/UploadImg")))
                {
                    Directory.CreateDirectory(HttpContext.Server.MapPath("/UploadImg"));
                }

                file.SaveAs(Path.Combine(HttpContext.Server.MapPath("/UploadImg"),newFileName));
                return Content("/UploadImg/" + newFileName);
            }

            return Content("");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChildCategory(int parentid)
        {
            var categorys = _AppContext.CategoryApp.GetList(parentid);
            StringBuilder html = new StringBuilder();
            html.AppendFormat("<option value='{0}'>{1}</option>", -1, "请选择");

            if (categorys!=null)
            {
                foreach (var item in categorys)
                {
                    html.AppendFormat("<option value='{0}'>{1}</option>",item.ID,item.Name);
                }
            }

            return Content(html.ToString());
        }

        #endregion

        #region ==== 私有方法 ====

        private string imgExtends = ".bmp.png.jpeg.jpg.gif";

        #endregion
    }
}