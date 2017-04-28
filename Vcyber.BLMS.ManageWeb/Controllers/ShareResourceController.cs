using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.ManageWeb.Controllers
{
    public class ShareResourceController : Controller
    {
        // GET: ShareResource
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetAllList(int start, int count, int fileType, string category)
        {
            int total = 0;

            var data = _AppContext.ShareResourcesApp.GetShareRes(new object(), fileType, category, start, count, out total);

            var result = new { data = data, pos = start, total_count = total };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetOne(int id)
        {
            var data = _AppContext.ShareResourcesApp.GetShareResById(id);

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddResourceFile(ShareResources model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, msg = "参数有误" });
            }
            var shareRes = new ShareResources();
            shareRes.Title = model.Title;
            shareRes.SubTitle = model.SubTitle;
            shareRes.CreateTime = DateTime.Now;
            shareRes.IsDisplay = model.IsDisplay;
            shareRes.Summary = model.Summary;
            shareRes.LinkUrl = model.LinkUrl;
            shareRes.FileType = model.FileType;
            shareRes.Category = model.Category;
            shareRes.ListImageUrl = model.ListImageUrl;
            shareRes.PlayImageUrl = model.PlayImageUrl;
            var result = _AppContext.ShareResourcesApp.AddShareRes(shareRes);
            if (result > 0)
            {
                //返回结果
                return Json(new { success = true, msg = "创建成功" });
            }
            else
            { return Json(new { success = false, msg = "创建失败" }); }
        }

        [HttpPost]
        public JsonResult UpdateResourceFile(ShareResources model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, msg = "参数有误" });
            }
            var shareRes = new ShareResources();
            shareRes.Id = model.Id;
            shareRes.Title = model.Title;
            shareRes.SubTitle = model.SubTitle;
            shareRes.CreateTime = DateTime.Now;
            shareRes.IsDisplay = model.IsDisplay;
            shareRes.Summary = model.Summary;
            shareRes.LinkUrl = model.LinkUrl;
            shareRes.FileType = model.FileType;
            shareRes.Category = model.Category;
            shareRes.ListImageUrl = model.ListImageUrl;
            shareRes.PlayImageUrl = model.PlayImageUrl;
            var result = _AppContext.ShareResourcesApp.UpdateShareRes(shareRes);
            if (result > 0)
            {
                //返回结果
                return Json(new { success = true, msg = "创建成功" });
            }
            else
            { return Json(new { success = false, msg = "创建失败" }); }
        }

        [HttpPost]
        public JsonResult DelResourceFile(int id)
        {
            var result = _AppContext.ShareResourcesApp.DelShareResByID(id);
            if (result > 0)
            {
                //返回结果
                return Json(new { success = true, msg = "删除成功" });
            }
            else
            { return Json(new { success = false, msg = "删除失败" }); }
        }

        public ActionResult ResDownload()
        {
            return View();
        }

    }
}