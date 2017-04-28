using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Vcyber.BLMS.Application;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.Entity.SelectCondition;
using Vcyber.BLMS.Repository;

namespace Vcyber.BLMS.ManageWeb.Controllers
{
    /// <summary>
    /// 商品类型
    /// </summary>
    [MvcAuthorize]
    public class CategoryController : Controller
    {
        #region ==== 构造函数 ====

        public CategoryController() { }

        #endregion

        #region ==== 公共方法 ====

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {

            var cookieValue = CookieHelper.GetCookieValue("CustomCookie");
            if (!string.IsNullOrWhiteSpace(cookieValue) && cookieValue.ToLower().Contains("webinspect"))
            {
                return Redirect("/Content/error.htm");
            }

            Session["csxfguid"] = Guid.NewGuid().ToString();
            var categoryDatas = _AppContext.CategoryApp.GetList();
            this.ViewBag.Data = categoryDatas != null ? categoryDatas.ToList<Category>() : new List<Category>(1);

            var cardTypeList = _AppContext.SCServiceCardTypeApp.GetMerchantCardTypeList();
            return View(cardTypeList);
        }

        /// <summary>
        /// 添加商品类型
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Add(string name, string cardtype, string guidtext,int parentId = 0)
        {
            //if (FilterStr.IsFlagXssStrSplit(string.Format("{0}|{1}|{2}", name, cardtype, parentId)))
            //{
            //    Response.Redirect("/Content/error.htm");
            //}
            if ((Session["csxfguid"]==null) || (Session["csxfguid"] != null && Session["csxfguid"].ToString() != guidtext))
            {
                return new JsonResult() { Data = new { Code = "Fail" } };
            }
            try
            {
                int id;
                _AppContext.CategoryApp.Add(new Category() { Name = name, Createtime = DateTime.Now, ParentID = parentId, Updatetime = DateTime.Now, CardType = cardtype }, out id);
                return new JsonResult() { Data = new { Code = "Success" } };

            }
            catch (RepeatException ex)
            {
                return new JsonResult() { Data = new { Code = "Repeat" } };
            }
            catch (Exception ex)
            {
                return new JsonResult() { Data = new { Code = "Fail" } };
            }
        }

        /// <summary>
        /// 修改商品类型名称
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Update(int id, string name, string cardtype, string guidtext,int parentid = 0)
        {

            //if (FilterStr.IsFlagXssStrSplit(string.Format("{0}|{1}|{2}|{3}|{4}", id, name, cardtype, guidtext, parentid)))
            //{
            //    Response.Redirect("/Content/error.htm");
            //}
            if ((Session["csxfguid"] == null) || (Session["csxfguid"] != null && Session["csxfguid"].ToString() != guidtext))
            {
                return new JsonResult() { Data = new { Code = "Fail" } };
            }
            try
            {
                
                bool result = _AppContext.CategoryApp.Update(new Category() { ID = id, Name = name, Updatetime = DateTime.Now, ParentID = parentid, CardType = cardtype });

                return new JsonResult() { Data = new { Code = result ? "Success" : "Fail" } };
            }
            catch (RepeatException ex)
            {
                return new JsonResult() { Data = new { Code = "Repeat" } };
            }
        }

        /// <summary>
        /// 删除商品类型
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Delete(int id, string guidtext)
        {
            //if (FilterStr.IsFlagXssStrSplit(string.Format("{0}|{1}", id, guidtext)))
            //{
            //    Response.Redirect("/Content/error.htm");
            //}
            if ((Session["csxfguid"] == null) || (Session["csxfguid"] != null && Session["csxfguid"].ToString() != guidtext))
            {
                return new JsonResult() { Data = new { Code = "Fail" } };
            }
            try
            {
                bool result = _AppContext.CategoryApp.Delete(id);
                return new JsonResult() { Data = new { Code = result ? "Success" : "Fail" } };
            }
            catch (RepeatException ex)
            {
                return new JsonResult() { Data = new { Code = "Repeat" } };
            }
        }



        public JsonResult GetMerchantCardTypeList()
        {
            return Json(_AppContext.SCServiceCardTypeApp.GetMerchantCardTypeList(), JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}