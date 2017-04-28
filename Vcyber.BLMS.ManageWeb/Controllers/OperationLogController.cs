using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Entity.SelectCondition;
using Vcyber.BLMS.ManageWeb.Models;

namespace Vcyber.BLMS.ManageWeb.Controllers
{
    [MvcAuthorize]
    public class OperationLogController : Controller
    {
        //
        // GET: /OperationLog/
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetOperationLogJsonResult(LogSearchParamater paras, int? start, int? count)
        {
            int total = 0;

            var logModelList = new List<OperationLogModel>();
            //paras.PageIndex = start ?? 0;
            //paras.PageSize = count ?? 0;
            paras.PageIndex = start ?? 0;
            paras.PageSize = count ?? 0;
            var logList = _AppContext.LogOperatorApp.GetLogInfo(paras, out total);
            if (logList != null)
            {
                foreach (var item in logList)
                {
                    var log = new OperationLogModel();
                    log.OperateItem = item.OperateItem;
                    
                    log.Id = item.Id;
                    log.OperateTime = item.OperateTime.ToShortDateString();
                    log.OperaterName = item.OperaterName;
                    logModelList.Add(log);
                }
            }

            var result = new { data = logModelList, pos = start ?? 0, total_count = total };
            return Json(result, JsonRequestBehavior.AllowGet);

        }
	}
}