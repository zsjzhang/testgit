using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.ManageWeb.Models;

namespace Vcyber.BLMS.ManageWeb.Controllers
{
    [MvcAuthorize]
    public class UserGuideController : Controller
    {
        // GET: UserGuide
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetUserGuideList(string title, int? start, int? count)
        {
            int total = 0;

            var data = _AppContext.UserGuideApp.GetUserGuideList(title, null,start ?? 0, count ?? 0, out total);


            var userGuideModelList = new List<UserGuideModel>();
            if (data != null || data.Count() > 0)
            {
                foreach (var item in data)
                {
                    var model = new UserGuideModel();
                    model.Id = item.Id;
                    model.ImageUrl = item.ImageUrl;
                    model.TrueImageUrl = item.TrueImageUrl;
                    model.LinkUrl = item.LinkUrl;
                    model.Summary = item.Summary;
                    model.Title = item.Title;
                    model.UpdateTime = item.UpdateTime.ToShortDateString();
                    model.IsApproved = item.IsApproved;
                    model.IsDisplay = item.IsDisplay;
                    model.DownloadTimes = item.DownloadTimes;
                    userGuideModelList.Add(model);
                }
            }
            var result = new { data = userGuideModelList, pos = start ?? 0, total_count = total };
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public ActionResult UserGuideDetail(int id)
        {
            var data = _AppContext.UserGuideApp.GetUserGuideById(id);
            if (data == null)
            {
                return Json(new { success = false, msg = "当前电子手册不存在！" });
            }

            var model = new UserGuideModel();
            model.Id = data.Id;
            model.ImageUrl = data.ImageUrl;
            model.LinkUrl = data.LinkUrl;
            model.Summary = data.Summary;
            model.Title = data.Title;
            model.UpdateTime = data.UpdateTime.ToShortDateString();
            model.IsDisplay = data.IsDisplay;
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {

            var data = _AppContext.UserGuideApp.GetUserGuideById(id);
            if (data == null)
            {
                return Json(new { success = false, msg = "当前不存在！" });
            }

            var success = _AppContext.UserGuideApp.Delete(id, HttpContext.User.Identity.Name);
            if (success)
            {
                if (data.IsApproved != (int)EApproveStatus.NoBegin)
                {
                    //添加审批记录（删除报刊）
                    var approve = new ApproveRecord();
                    approve.ItemId = id;
                    approve.ItemType = ((int)EApproveType.UserGuide).ToString();
                    approve.OperatorId = HttpContext.User.Identity.GetUserId();
                    approve.OperatorName = HttpContext.User.Identity.Name;
                    approve.ApprovalMemo = "删除电子手册";
                    _AppContext.ApproveRecordApp.UpdateStatus(approve);
                }

                //添加操作记录（删除报刊）
                var operationData = new OperatorLog();
                operationData.Type = (int)ELogType.UserGuide;
                operationData.SourceId = id.ToString();
                operationData.OperateTime = DateTime.Now;
                operationData.OperaterName = HttpContext.User.Identity.Name;
                operationData.OperaterId = HttpContext.User.Identity.GetUserId();
                operationData.OperateItem = "";
                operationData.OriginalValue = "";
                operationData.CurrentValue = "";
                operationData.Remark = "删除电子手册";
                _AppContext.LogOperatorApp.Add(operationData, ELogType.UserGuide);

                return Json(new { success = true, msg = "删除成功！" });
            }
            else
                return Json(new { success = false, msg = "删除失败！" });
        }
        public ActionResult EditUserGuide()
        {
            return View();
        }

        [HttpPost]
        public ActionResult EditUserGuide(UserGuideModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, msg = "参数错误" });
            }

            //check whether this item is existed
            var data = _AppContext.UserGuideApp.GetUserGuideById(model.Id);
            if (data == null)
            {
                return Json(new { success = false, msg = "当前不存在！" });
            }
            // check whether one property should be unique

            data.ImageUrl = model.ImageUrl;
            data.LinkUrl = model.LinkUrl;
            data.Summary = model.Summary;
            data.Title = model.Title;
            data.UpdateBy = HttpContext.User.Identity.Name;
            
            var success = _AppContext.UserGuideApp.Update(data);
            if (success)
            {
                if (data.IsApproved != (int)EApproveStatus.NoBegin)
                {
                    //添加审批记录（编辑电子手册）
                    var approve = new ApproveRecord();
                    approve.ItemId = model.Id;
                    approve.ItemType = ((int)EApproveType.UserGuide).ToString();
                    approve.OperatorId = HttpContext.User.Identity.GetUserId();
                    approve.OperatorName = HttpContext.User.Identity.Name;
                    approve.ApprovalMemo = "编辑电子手册";
                    _AppContext.ApproveRecordApp.UpdateStatus(approve);
                }
                //添加操作记录（编辑电子手册）
                var operationData = new OperatorLog();
                operationData.Type = (int)ELogType.UserGuide;
                operationData.SourceId = model.Id.ToString();
                operationData.OperateTime = DateTime.Now;
                operationData.OperaterName = HttpContext.User.Identity.Name;
                operationData.OperaterId = HttpContext.User.Identity.GetUserId();
                operationData.OperateItem = "";
                operationData.OriginalValue = "";
                operationData.CurrentValue = "";
                operationData.Remark = "编辑电子手册";
                _AppContext.LogOperatorApp.Add(operationData, ELogType.UserGuide);

                return Json(new { success = true, msg = "更新成功！" });
            }
            else
                return Json(new { success = false, msg = "更新失败！" });


        }
        public ActionResult CreateUserGuide()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateUserGuide(UserGuideModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, msg = "参数错误" });
            }
            var data = new UserGuide();
            data.ImageUrl = model.ImageUrl;
            data.LinkUrl = model.LinkUrl;
            data.Summary = model.Summary;
            data.Title = model.Title;
            data.IsDisplay = model.IsDisplay;
            data.IsApproved = 0;
            data.CreateBy = HttpContext.User.Identity.Name;
            data.UpdateBy = HttpContext.User.Identity.Name;

            var result = _AppContext.UserGuideApp.Create(data);
            if (result > 0)
            {
                //创建审批记录（创建报刊）
                var approve = new ApproveRecord();
                approve.ItemId = result;
                approve.ItemType = ((int)EApproveType.UserGuide).ToString();
                approve.ApprovalMemo = "创建电子手册";
                approve.OperatorId = HttpContext.User.Identity.GetUserId();
                approve.OperatorName = HttpContext.User.Identity.Name;
                _AppContext.ApproveRecordApp.UpdateStatus(approve);

                //创建操作记录（创建报刊）
                var operationData = new OperatorLog();
                operationData.Type = (int)ELogType.UserGuide;
                operationData.SourceId = result.ToString();
                operationData.OperateTime = DateTime.Now;
                operationData.OperaterName = HttpContext.User.Identity.Name;
                operationData.OperaterId = HttpContext.User.Identity.GetUserId();
                operationData.OperateItem = "";
                operationData.OriginalValue = "";
                operationData.CurrentValue = "";
                operationData.Remark = "创建电子手册";
                _AppContext.LogOperatorApp.Add(operationData, ELogType.UserGuide);


                return Json(new { success = true, msg = "创建成功！" });
            }
            else
                return Json(new { success = false, msg = "创建失败！" });
        }
        public JsonResult GetApproveList(int id)
        {
            var data = _AppContext.ApproveRecordApp.GetApproveRecordList(id, ((int)EApproveType.UserGuide).ToString());
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
            var logList = _AppContext.LogOperatorApp.Select(Id.ToString(), ELogType.UserGuide, start ?? 0, count ?? 0, out total);
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
            var obj = _AppContext.UserGuideApp.GetUserGuideById(id);
            if (obj == null)
            {
                return Json(new { success = false, msg = "当前电子手册不存在！" });
            }
            if (status == obj.IsDisplay)
            {
                return Json(new { success = true, msg = "值没有改变" });
            }

            var data = _AppContext.UserGuideApp.UpdateIsDisplay(obj.Id, status, HttpContext.User.Identity.Name);

            if (data)
            {
                //添加操作记录（修改新闻显示状态）
                var operationData = new OperatorLog();
                operationData.Type = (int)ELogType.UserGuide;
                operationData.SourceId = id.ToString();
                operationData.OperateTime = DateTime.Now;
                operationData.OperaterName = HttpContext.User.Identity.Name;
                operationData.OperaterId = HttpContext.User.Identity.GetUserId();
                operationData.OperateItem = "显示状态";
                operationData.OriginalValue = obj.IsDisplay.ToString();
                operationData.CurrentValue = status.ToString();
                operationData.Remark = "修改显示状态";
                _AppContext.LogOperatorApp.Add(operationData, ELogType.UserGuide);

                return Json(new { success = true, msg = "成功！" });
            }
            else
                return Json(new { success = false, msg = "失败！" });
        }

        //[HttpPost]
        //public JsonResult UpdateDownLoadTimes(int id)
        //{
        //    var obj = _AppContext.UserGuideApp.GetUserGuideById(id);
        //    if (obj == null)
        //    {
        //        return Json(new { success = false, msg = "当前电子手册不存在！" });
        //    }

        //    var data = _AppContext.UserGuideApp.UpdateDownloadTimes(obj.Id, 1, HttpContext.User.Identity.Name);

        //    if (data)
        //    {
        //        return Json(new { success = true, msg = "成功！" });
        //    }
        //    else
        //        return Json(new { success = false, msg = "失败！" });
        //}
	}
}