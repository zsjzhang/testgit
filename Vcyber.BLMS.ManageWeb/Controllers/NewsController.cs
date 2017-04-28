using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.ManageWeb.Models;

namespace Vcyber.BLMS.ManageWeb.Controllers
{
    [MvcAuthorize]
    public class NewsController : Controller
    {
        private ApplicationUserManager _userManager;
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

        // GET: News
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CreateNews()
        {
            return View();
        }


        [HttpPost]
        public JsonResult CreateNews(NewsModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, msg = "参数有误" });
            }
            var news = new News();
            news.Title = model.Title;
            news.MajorImageUrl = model.MajorImageUrl;
            news.IsHot = 0;
            news.IsDisplay = model.IsDisplay;
            news.Content = model.Content;
            news.Summary = model.Summary;
            news.CreateBy = HttpContext.User.Identity.Name;
            news.UpdateBy = HttpContext.User.Identity.Name;
            news.Priority = (string.IsNullOrWhiteSpace(model.Priority+"") ? 0 : model.Priority);
            var user = UserManager.FindById(HttpContext.User.Identity.GetUserId());
            news.Dealer = user.DealerId;
            var result = _AppContext.NewsApp.AddNews(news);
            if (result > 0)
            {
                //创建审批记录（创建新闻）
                var approve = new ApproveRecord();
                approve.ItemId = result;
                approve.ItemType = ((int)EApproveType.News).ToString();
                approve.ApprovalMemo = "创建新闻";
                approve.OperatorId = HttpContext.User.Identity.GetUserId();
                approve.OperatorName = HttpContext.User.Identity.Name;
                _AppContext.ApproveRecordApp.UpdateStatus(approve);

                //创建操作记录（创建新闻）
                var operationData = new OperatorLog();
                operationData.Type = (int)ELogType.News;
                operationData.SourceId = result.ToString();
                operationData.OperateTime = DateTime.Now;
                operationData.OperaterName = HttpContext.User.Identity.Name;
                operationData.OperaterId = HttpContext.User.Identity.GetUserId();
                operationData.OperateItem = "";
                operationData.OriginalValue = "";
                operationData.CurrentValue = "";
                operationData.Remark = "创建新闻";
                _AppContext.LogOperatorApp.Add(operationData, ELogType.News);

                //返回结果
                return Json(new { success = true, msg = "创建成功" });
            }
            else
            { return Json(new { success = true, msg = "创建失败" }); }

        }

        public JsonResult LoadNews(int id)
        {
            var data = _AppContext.NewsApp.GetNewsById(id);
            var model = new NewsModel();
            if (data != null)
            {
                model.Content = data.Content;
                model.IsDisplay = data.IsDisplay;
                model.MajorImageUrl = data.MajorImageUrl;
                model.IsHot = data.IsHot;
                model.Priority = data.Priority;
                model.Title = data.Title;
                model.Summary = data.Summary;
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditNews()
        {
            return View();
        }

        [HttpPost]
        public JsonResult EditNews(NewsModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, msg = "参数有误" });
            }

            var data = _AppContext.NewsApp.GetNewsById(model.Id);
            if (data == null)
            {
                return Json(new { success = false, msg = "当前新闻不存在！" });
            }

            // var news = new News();
            //news.Id = model.Id;
            data.Title = model.Title;
            data.MajorImageUrl = model.MajorImageUrl;
            
            //更新时不再能更新显示标志
            data.Content = model.Content;
            data.Summary = model.Summary;

            var result = _AppContext.NewsApp.UpdateNews(data);
            if (result)
            {
                if (data.IsApproved != (int)EApproveStatus.NoBegin)
                {
                    //添加审批记录（编辑新闻）
                    var approve = new ApproveRecord();
                    approve.ItemId = model.Id;
                    approve.ItemType = ((int)EApproveType.News).ToString();
                    approve.OperatorId = HttpContext.User.Identity.GetUserId();
                    approve.OperatorName = HttpContext.User.Identity.Name;
                    approve.ApprovalMemo = "编辑新闻";
                    approve.IsApproval = (int)EApproveStatus.NoBegin;
                    _AppContext.ApproveRecordApp.UpdateStatus(approve);
                }

                //添加操作记录（编辑新闻）
                var operationData = new OperatorLog();
                operationData.Type = (int)ELogType.News;
                operationData.SourceId = model.Id.ToString();
                operationData.OperateTime = DateTime.Now;
                operationData.OperaterName = HttpContext.User.Identity.Name;
                operationData.OperaterId = HttpContext.User.Identity.GetUserId();
                operationData.OperateItem = "";
                operationData.OriginalValue = "";
                operationData.CurrentValue = "";
                operationData.Remark = "编辑新闻";
                _AppContext.LogOperatorApp.Add(operationData, ELogType.News);

                return Json(new { success = true, msg = "编辑成功" });
            }
            else
            { return Json(new { success = true, msg = "编辑失败" }); }

        }
        [HttpPost]
        public JsonResult Delete(int id)
        {
            var data = _AppContext.NewsApp.GetNewsById(id);
            if (data == null)
            {
                return Json(new { success = false, msg = "当前新闻不存在！" });
            }

            var success = _AppContext.NewsApp.DeleteNews(id, HttpContext.User.Identity.Name);
            if (success)
            {
                //添加操作记录（编辑新闻）
                var operationData = new OperatorLog();
                operationData.Type = (int)ELogType.News;
                operationData.SourceId = id.ToString();
                operationData.OperateTime = DateTime.Now;
                operationData.OperaterName = HttpContext.User.Identity.Name;
                operationData.OperaterId = HttpContext.User.Identity.GetUserId();
                operationData.OperateItem = "";
                operationData.OriginalValue = "";
                operationData.CurrentValue = "";
                operationData.Remark = "删除新闻";
                _AppContext.LogOperatorApp.Add(operationData, ELogType.News);

                return Json(new { success = true, msg = "删除成功！" });
            }
            else
                return Json(new { success = false, msg = "删除失败！" });
        }

        public JsonResult GetDealerId()
        {
            var user = UserManager.FindById(HttpContext.User.Identity.GetUserId());

            return Json(user.DealerId, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetNewsList(string title, string dealer, int? start, int? count,string gstart,string gend)
        {
            int total = 0;
            var newsModelList = new List<NewsModel>();

            var user = UserManager.FindById(HttpContext.User.Identity.GetUserId());

            if (!string.IsNullOrEmpty(user.DealerId))
            {
                dealer = user.DealerId;
            }

            if (dealer == "-1")
            {
                dealer = null;
            }
            var newsList = _AppContext.NewsApp.Select1(title, null, null, null, start ?? 0, count ?? 0, out total, gstart, gend);
            if (newsList != null)
            {
                foreach (var item in newsList)
                {
                    var news = new NewsModel();
                    news.Id = item.Id;
                    news.Title = item.Title;
                    news.IsDisplay = item.IsDisplay;
                    news.IsHot = item.IsHot;
                    news.CreateTime = item.CreateTime.ToShortDateString();
                    news.CreateBy = item.CreateBy;
                    news.IsApproved = item.IsApproved;
                    news.Priority = item.Priority;
                    news.MajorImageUrl = item.MajorImageUrl;
                    news.ApprovedTime = item.ApprovedTime.ToString("yyyy-MM-dd");
                    var dealerObj = _AppContext.DealerApp.GetDealerByDealerId(item.Dealer);
                    if (dealer != null)
                    {
                        news.DealerName = dealerObj.Name;
                        
                    }
                   
                    newsModelList.Add(news);
                }
            }

            var result = new { data = newsModelList, pos = start ?? 0, total_count = total };
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetApproveList(int id)
        {
            var data = _AppContext.ApproveRecordApp.GetApproveRecordList(id, ((int)EApproveType.News).ToString());
            var modelList = new List<ApproveRecordModel>();
            if (data != null)
            {
                foreach (var item in data)
                {
                    var record = new ApproveRecordModel();
                    record.ApprovalMemo = item.ApprovalMemo;
                    record.IsApproval = item.IsApproval;
                    record.UpdateTime = item.UpdateTime.ToShortDateString();
                    record.OperatorName = item.OperatorName;
                    record.OperatorId = item.OperatorId;

                    modelList.Add(record);
                }
            }
            return Json(modelList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult OperationLog()
        {
            return View();
        }

        public JsonResult GetOperationLog(int Id, int? start, int? count)
        {
            int total = 0;

            var logModelList = new List<OperationLogModel>();
            var logList = _AppContext.LogOperatorApp.Select(Id.ToString(), ELogType.News, start ?? 0, count ?? 0, out total);
            if (logList != null)
            {
                foreach (var item in logList)
                {
                    var log = new OperationLogModel();
                    log.OperateItem = item.OperateItem;
                    log.SourceId = item.SourceId;
                    log.OperateTime = item.OperateTime.ToShortDateString();
                    log.OperaterName = item.OperaterName;
                    log.Type = item.Type;
                    log.Remark = item.Remark;
                    log.OriginalValue = item.OriginalValue;
                    log.CurrentValue = item.CurrentValue;
                    logModelList.Add(log);
                }
            }

            var result = new { data = logModelList, pos = start ?? 0, total_count = total };
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult UpdateIsDisplay(int id, int status)
        {
            var obj = _AppContext.NewsApp.GetNewsById(id);
            if (obj == null)
            {
                return Json(new { success = false, msg = "当前新闻不存在！" });
            }
            if (status == obj.IsDisplay)
            {
                return Json(new { success = false, msg = "值没有改变" });
            }

            var data = _AppContext.NewsApp.UpdateIsDisplay(obj.Id, status, HttpContext.User.Identity.Name);

            if (data)
            {
                //添加操作记录（修改新闻显示状态）
                var operationData = new OperatorLog();
                operationData.Type = (int)ELogType.News;
                operationData.SourceId = id.ToString();
                operationData.OperateTime = DateTime.Now;
                operationData.OperaterName = HttpContext.User.Identity.Name;
                operationData.OperaterId = HttpContext.User.Identity.GetUserId();
                operationData.OperateItem = "显示状态";
                operationData.OriginalValue = obj.IsDisplay.ToString();
                operationData.CurrentValue = status.ToString();
                operationData.Remark = "修改显示状态";
                _AppContext.LogOperatorApp.Add(operationData, ELogType.News);

                return Json(new { success = true, msg = "成功！" });
            }
            else
                return Json(new { success = false, msg = "失败！" });
        }

        [HttpPost]
        public JsonResult UpdateAllDisplay(int id, int priority, int display, int isHot)
        {
            var obj = _AppContext.NewsApp.GetNewsById(id);
            if (obj == null)
            {
                return Json(new { success = false, msg = "当前新闻不存在！" });
            }

            var user = UserManager.FindById(HttpContext.User.Identity.GetUserId());

            if (!string.IsNullOrEmpty(user.DealerId))
            {
                if (obj.Priority != priority || obj.IsHot != isHot)
                {
                    return Json(new { success = false, msg = "当前账号不能更新权重和热点！" });

                }

            }
            var data = _AppContext.NewsApp.UpdateAllDisplay(obj.Id, priority,display,isHot, HttpContext.User.Identity.Name);

            if (data)
            {
                //添加操作记录（修改新闻权值）
                var operationData = new OperatorLog();
                operationData.Type = (int)ELogType.News;
                operationData.SourceId = id.ToString();
                operationData.OperateTime = DateTime.Now;
                operationData.OperaterName = HttpContext.User.Identity.Name;
                operationData.OperaterId = HttpContext.User.Identity.GetUserId();
                operationData.OperateItem = "";
                operationData.OriginalValue = "";
                operationData.CurrentValue = "";
                operationData.Remark = "修改";
                _AppContext.LogOperatorApp.Add(operationData, ELogType.News);

                return Json(new { success = true, msg = "成功！" });
            }
            else
                return Json(new { success = false, msg = "失败！" });
        }


    }
}