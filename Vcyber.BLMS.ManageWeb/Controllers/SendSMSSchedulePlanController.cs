using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Application;

namespace Vcyber.BLMS.ManageWeb.Controllers
{
    public class SendSMSSchedulePlanController : Controller
    {
        //
        // GET: /SendSMSSchedulePlan/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Send()
        {
            return View();
        }

        public JsonResult List(string start, string end, string serviceTitle, string state, string timeType, string carType, int pageindex, int pagesize)
        {
            int total = 0;

            var list = _AppContext.SendSMSSchedulePlanApp.SelectSendSMSSchedulePlanList(start, end, serviceTitle, timeType, state, carType);

            total = list.Count();

            var cardList = list.Skip(pagesize * (pageindex - 1)).Take(pagesize);

            var result = new { data = cardList, pos = pageindex, total_count = total };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Edit(int id, string isOpen)
        {
            var result = new ReturnResult { IsSuccess = false };

            result.IsSuccess = _AppContext.SendSMSSchedulePlanApp.UpdateSendSMSSchedulePlan(id, isOpen);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Update(SendSMSSchedulePlan entity)
        {
            var result = new ReturnResult { IsSuccess = false };

            result.IsSuccess = _AppContext.SendSMSSchedulePlanApp.UpdateSendSMSSchedulePlan(entity);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Add(SendSMSSchedulePlan entity)
        {
            var result = new ReturnResult { IsSuccess = false };

            result.IsSuccess = _AppContext.SendSMSSchedulePlanApp.AddSendSMSSchedulePlan(entity);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SendList(string start, string end, string userName, string state, string title, string carType, int pageindex, int pagesize)
        {
            int total = 0;

            var list = _AppContext.SendSMSSchedulePlanApp.SelectSendSMSSchedulePlanResultList(start, end, userName, state, title, carType);

            total = list.Count();

            var cardList = list.Skip(pagesize * (pageindex - 1)).Take(pagesize);

            var result = new { data = cardList, pos = pageindex, total_count = total };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}