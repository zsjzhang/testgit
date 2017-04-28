using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.ManageWeb.Models;
using System.Net;
using System.IO;

namespace Vcyber.BLMS.ManageWeb.Controllers
{
    /// <summary>
    /// 服务卡
    /// </summary>
    public class ServiceCarController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 查询批次信息
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public ActionResult FindBatch(ServiceCarBatchCondition condition)
        {
            int total = 0;
            PageData page = new PageData() { Index = condition.Index, Size = condition.Size };
            var list = _AppContext.ServiceCardBatchApp.findCondition(condition, page, out total);
            var statusStatistics = _AppContext.ServiceCardBatchApp.serviceCardStatistics(condition);

            int count = (int)Math.Ceiling((double)total / (double)page.Size);
            ViewBag.Total = total;
            ViewBag.PageIndex = condition.Index;
            ViewBag.PrePage = condition.Index > 1 ? (condition.Index - 1) : 1;
            ViewBag.NextPage = condition.Index < count ? (condition.Index + 1) : count;
            ViewBag.EndPage = count;
            ViewBag.StatusStatistics = statusStatistics;

            return PartialView(list);
        }

        /// <summary>
        /// 批量生成卡卷
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddBatch(ServiceCardBatchV data)
        {
            string message;
            bool result = data.ValidateData(out message);

            if (result)
            {
                ServiceCardBatch batchData = CommonUtilitys.CopyData<ServiceCardBatchV, ServiceCardBatch>(data);
                ReturnResult rs = _AppContext.ServiceCardBatchApp.CreateServiceCardBatch(batchData);
                result = rs.IsSuccess;
                message = rs.Message;
            }

            return Json(new ResponseMessage(result ? HttpStatusCode.OK : HttpStatusCode.InternalServerError, message));
        }

        /// <summary>
        /// 服务卡
        /// </summary>
        /// <returns></returns>
        public ActionResult ServiceCardIndex()
        {
            return View();
        }

        /// <summary>
        /// 服务卡查询
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        public ActionResult FindServiceCard(ServiceCardCondition condition)
        {
            int total = 0;
            PageData page = new PageData() { Index = condition.Index, Size = condition.Size };
            var list = _AppContext.ServiceCardApp.FindList(condition, page, out total);

            int count = (int)Math.Ceiling((double)total / (double)page.Size);
            ViewBag.Total = total;
            ViewBag.PageIndex = condition.Index;
            ViewBag.PrePage = condition.Index > 1 ? (condition.Index - 1) : 1;
            ViewBag.NextPage = condition.Index < count ? (condition.Index + 1) : count;
            ViewBag.EndPage = count;

            return PartialView(list);
        }

        /// <summary>
        /// 导出服务卡信息
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public ActionResult ExportServiceCard(ServiceCardCondition condition)
        {
            ServiceCardExport export = new ServiceCardExport();
            var datas = _AppContext.ServiceCardApp.FindList(condition, 10000);

            if (datas!=null&&datas.Count()>0)
            {
                foreach (var data in datas)
                {
                    export.Writer(data);
                }
            }

            // 写入到客户端 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            export.Book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", "服务卡" + ".xls");
        }
    }
}