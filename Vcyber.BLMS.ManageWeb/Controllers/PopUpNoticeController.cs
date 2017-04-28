using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Entity.NoticeInfos;

namespace Vcyber.BLMS.ManageWeb.Controllers
{
    public class PopUpNoticeController : Controller
    {
        // GET: PopUpNotice
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetAllList(int start, int count)
        {
            int total = 0;

            var data = _AppContext.NoticeInfosApp.GetNoticeInfos(new object(), start, count, out total);

            var result = new { data = data, pos = start, total_count = total };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CreateNotice(NoticeInfos model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, msg = "参数有误" });
            }
            var notice = new NoticeInfos();
            notice.Title = model.Title;
            notice.CreateTime = DateTime.Now;
            notice.IsDisplay = model.IsDisplay;
            notice.Summary = model.Summary;
            var result = _AppContext.NoticeInfosApp.AddNoticeInfo(notice);
            if (result > 0)
            {
                //返回结果
                return Json(new { success = true, msg = "创建成功" });
            }
            else
            { return Json(new { success = false, msg = "创建失败" }); }

        }

        [HttpPost]
        public JsonResult UpdateNotice(NoticeInfos model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, msg = "参数有误" });
            }
            var notice = new NoticeInfos();
            notice.Id = model.Id;
            notice.Title = model.Title;
            notice.CreateTime = DateTime.Now;
            notice.IsDisplay = model.IsDisplay;
            notice.Summary = model.Summary;
            var result = _AppContext.NoticeInfosApp.UpdateNoticeInfo(notice);
            if (result > 0)
            {
                //返回结果
                return Json(new { success = true, msg = "保存成功" });
            }
            else
            { return Json(new { success = false, msg = "保存失败" }); }

        }

        [HttpPost]
        public JsonResult DelNoticeById(int id)
        {
            var result = _AppContext.NoticeInfosApp.DelNoticeInfoByID(id);
            if (result > 0)
            {
                //返回结果
                return Json(new { success = true, msg = "删除成功" });
            }
            else
            { return Json(new { success = false, msg = "删除失败" }); }

        }

    }
}