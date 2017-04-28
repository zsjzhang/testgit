using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.ManageWeb.Models;

namespace Vcyber.BLMS.ManageWeb.Controllers
{
    [MvcAuthorize]
    public class ImageCarouselController : Controller
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

        // GET: ImageCarousel
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetImageCarouselList(int? type, int? start,int? count)
        {
            int total = 0;
            var data = _AppContext.ImageCarouselApp.GetImageCarouselList(null,type,start??0,count??0,out total);
            var imgCarouselModelList = new List<ImageCarouselModel>();
            if (data != null || data.Count() > 0)
            {
                foreach (var item in data)
                {
                    var model = new ImageCarouselModel();
                    model.Id = item.Id;
                    model.ImageUrl = item.ImageUrl;
                    model.TrueImageUrl = item.TrueImageUrl;
                    model.LinkUrl = item.LinkUrl;
                    model.Priority = item.Priority;
                    model.Title = item.Title;
                    model.UpdateTime = item.UpdateTime.ToShortDateString();
                    model.IsApproved = item.IsApproved;
                    model.Type = item.Type;
                    imgCarouselModelList.Add(model);
                }
            }
            var result = new { data = imgCarouselModelList, pos = start ?? 0, total_count = total };
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public ActionResult ImageCarouselDetail(int id)
        {
            var data = _AppContext.ImageCarouselApp.GetImageCarouselById(id);
            if (data == null)
            {
                return Json(new { success = false, msg = "当前轮播图不存在！" });
            }

            var model = new ImageCarouselModel();
            model.Title = data.Title;
            model.Id = data.Id;
            model.LinkUrl = data.LinkUrl;
            model.ImageUrl = data.ImageUrl;
            model.NewPage = data.NewPage;
            model.Type = data.Type;
            model.Priority = data.Priority;
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(int id) 
        {
            
            var data = _AppContext.ImageCarouselApp.GetImageCarouselById(id);
            if(data == null)
            {
             return Json(new {success = false, msg = "当前轮播图不存在！"});
            }

            var success = _AppContext.ImageCarouselApp.Delete(id, HttpContext.User.Identity.Name);
            if(success)
            {
                var operationData = new OperatorLog();
                operationData.Type = (int) ELogType.ImageCarousel;
                operationData.SourceId = id.ToString();
                operationData.OperateTime = DateTime.Now;
                operationData.OperaterName = HttpContext.User.Identity.Name;
                operationData.OperaterId = HttpContext.User.Identity.GetUserId();
                operationData.OperateItem = "删除字段";
                operationData.OriginalValue = "未删除";
                operationData.CurrentValue = "以删除";
                operationData.Remark = "删除轮播图";
                _AppContext.LogOperatorApp.Add(operationData,ELogType.ImageCarousel);
                return Json(new { success = true, msg = "删除成功！" });
            }
            else
                return Json(new { success = false, msg = "删除失败！" });
        }

        [HttpPost]
        public ActionResult EditImageCarousel(ImageCarouselModel model)
        {
              if (!ModelState.IsValid)
            {
                return Json(new { success = false, msg = "参数错误" });
            }

            //check whether this item is existed
             var data = _AppContext.ImageCarouselApp.GetImageCarouselById(model.Id);
            if(data == null)
            {
             return Json(new {success = false, msg = "当前轮播图不存在！"});
            }
            // check whether one property should be unique

            data.ImageUrl = model.ImageUrl;
            data.LinkUrl = model.LinkUrl;
            data.Priority = model.Priority;
            data.NewPage = 0;
            data.Title = model.Title;
            data.Type = model.Type;
            //data.IsApproved = 0;
            data.UpdateBy = HttpContext.User.Identity.Name;

            var success = _AppContext.ImageCarouselApp.Update(data);
            if (success)
            {
                if (data.IsApproved != (int)EApproveStatus.NoBegin)
                {
                    //创建审批记录（创建新闻）
                    var approve = new ApproveRecord();
                    approve.ItemId = data.Id;
                    approve.ItemType = ((int)EApproveType.ImageCarousel).ToString();
                    approve.ApprovalMemo = "更新轮播图";
                    approve.OperatorId = HttpContext.User.Identity.GetUserId();
                    approve.OperatorName = HttpContext.User.Identity.Name;
                    _AppContext.ApproveRecordApp.UpdateStatus(approve);
                }

                var operationData = new OperatorLog();
                operationData.Type = (int)ELogType.ImageCarousel;
                operationData.SourceId = model.Id.ToString();
                operationData.OperateTime = DateTime.Now;
                operationData.OperaterName = HttpContext.User.Identity.Name;
                operationData.OperaterId = HttpContext.User.Identity.GetUserId();
                operationData.OperateItem = "";
                operationData.OriginalValue = "";
                operationData.CurrentValue = "";
                operationData.Remark = "编辑轮播图";
                _AppContext.LogOperatorApp.Add(operationData, ELogType.ImageCarousel);

                return Json(new { success = true, msg = "更新成功！" });
            }
            else
                return Json(new { success = false, msg = "更新失败！" });
        }
        public JsonResult GetDealerId()
        {
            var user = UserManager.FindById(HttpContext.User.Identity.GetUserId());

            return Json(user.DealerId, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        //public ActionResult UpdatePriority(int changeValue,int Id)
        //{
        //    //check whether this item is existed
        //    var data = _AppContext.ImageCarouselApp.GetImageCarouselById(Id);
        //    if (data == null)
        //    {
        //        return Json(new { success = false, msg = "当前轮播图不存在！" });
        //    }

        //    var success = _AppContext.ImageCarouselApp.UpdatePriority(changeValue,data);
        //    if (success)
        //    {
        //        var operationData = new OperatorLog();
        //        operationData.Type = (int)ELogType.ImageCarousel;
        //        operationData.SourceId = Id.ToString();
        //        operationData.OperateTime = DateTime.Now;
        //        operationData.OperaterName = HttpContext.User.Identity.Name;
        //        operationData.OperaterId = HttpContext.User.Identity.GetUserId();
        //        operationData.OperateItem = "权重";
        //        operationData.OriginalValue = data.Priority.ToString();
        //        operationData.CurrentValue = changeValue.ToString();
        //        operationData.Remark = "更改轮播图权重";
        //        _AppContext.LogOperatorApp.Add(operationData, ELogType.ImageCarousel);

        //        return Json(new { success = true, msg = "更新成功！" });
        //    }
        //    else
        //        return Json(new { success = false, msg = "更新失败！" });
        //}

        [HttpPost]
        public JsonResult UpdatePriority(int changeValue, int Id)
        {
            if (changeValue<=0)
            {
                return Json(new { success = false, msg = "权值必须大于零" });
            } 

            var obj = _AppContext.ImageCarouselApp.GetImageCarouselById(Id);

            if (obj == null)
            {
                return Json(new { success = false, msg = "当前轮播图不存在！" });
            }
            if (changeValue == obj.Priority)
            {
                return Json(new { success = false, msg = "值没有改变" });
            }

            var data = _AppContext.ImageCarouselApp.UpdatePriority(obj.Id, changeValue, HttpContext.User.Identity.Name);

            if (data)
            {
                //添加操作记录（修改新闻权值）
                var operationData = new OperatorLog();
                operationData.Type = (int)ELogType.News;
                operationData.SourceId = Id.ToString();
                operationData.OperateTime = DateTime.Now;
                operationData.OperaterName = HttpContext.User.Identity.Name;
                operationData.OperaterId = HttpContext.User.Identity.GetUserId();
                operationData.OperateItem = "权值";
                operationData.OriginalValue = obj.Priority.ToString();
                operationData.CurrentValue = changeValue.ToString();
                operationData.Remark = "修改权值";
                _AppContext.LogOperatorApp.Add(operationData, ELogType.News);

                return Json(new { success = true, msg = "成功！" });
            }
            else
                return Json(new { success = false, msg = "失败！" });
        }


        [HttpPost]
        public ActionResult CreateImageCarousel(ImageCarouselModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, msg = "参数错误" });
            }
            if (model.Type<0)
            {
                return Json(new { success = false, msg = "参数错误" });
            }
            var data = new ImageCarousel();
            data.ImageUrl = model.ImageUrl;
            data.LinkUrl = model.LinkUrl;
            data.Priority = model.Priority;
            data.NewPage = 0;
            data.Title = model.Title;
            data.Type = model.Type;
            data.CreateBy = HttpContext.User.Identity.Name;
            data.UpdateBy = HttpContext.User.Identity.Name;
            data.Priority = 0;
            //data.Priority = _AppContext.ImageCarouselApp.GetMaxPriority() + 1;
            var success = _AppContext.ImageCarouselApp.Create(data);
            if (success>0)
            {
                //创建审批记录（创建轮播图）
                var approve = new ApproveRecord();
                approve.ItemId = success;
                approve.ItemType = ((int)EApproveType.ImageCarousel).ToString();
                approve.ApprovalMemo = "创建轮播图";
                approve.OperatorId = HttpContext.User.Identity.GetUserId();
                approve.OperatorName = HttpContext.User.Identity.Name;
                _AppContext.ApproveRecordApp.UpdateStatus(approve);

                //添加纪录日志

                var operationData = new OperatorLog();
                operationData.Type = (int)ELogType.ImageCarousel;
                operationData.SourceId = success.ToString();
                operationData.OperateTime = DateTime.Now;
                operationData.OperaterName = HttpContext.User.Identity.Name;
                operationData.OperaterId = HttpContext.User.Identity.GetUserId();
                operationData.OperateItem = "";
                operationData.OriginalValue = "";
                operationData.CurrentValue = "";
                operationData.Remark = "创建轮播图";
                _AppContext.LogOperatorApp.Add(operationData, ELogType.ImageCarousel);


                return Json(new { success = true, msg = "创建成功！" });
            }
            else
                return Json(new { success = false, msg = "创建失败！" });

        }

        public JsonResult GetApproveList(int id)
        {
            var data = _AppContext.ApproveRecordApp.GetApproveRecordList(id, ((int)EApproveType.ImageCarousel).ToString());
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

        //[HttpPost]
        //public JsonResult Approval(int id, int status, string memo)
        //{
        //    if (status > 2 || status < 0 || memo.Length <= 0 || memo.Length > 100)
        //    {
        //        return Json(new { success = false, msg = "参数不对" });
        //    }

        //    var data = _AppContext.ImageCarouselApp.ApprovedImageCarousel(id, HttpContext.User.Identity.GetUserId(), HttpContext.User.Identity.Name, status, memo);

        //    if (data)
        //    {
        //        var operationData = new OperatorLog();
        //        operationData.Type = (int)ELogType.ImageCarousel;
        //        operationData.SourceId = id.ToString();
        //        operationData.OperateTime = DateTime.Now;
        //        operationData.OperaterName = HttpContext.User.Identity.Name;
        //        operationData.OperaterId = HttpContext.User.Identity.GetUserId();
        //        operationData.OperateItem = "";
        //        operationData.OriginalValue = "";
        //        operationData.CurrentValue = "";
        //        operationData.Remark = "更新轮播图审批状态";
        //        _AppContext.LogOperatorApp.Add(operationData, ELogType.ImageCarousel);

        //        return Json(new { success = true, msg = "成功！" });
        //    }
        //    else
        //        return Json(new { success = false, msg = "失败！" });
        //}

        public ActionResult OperationLog()
        {
            return View();
        }

        public JsonResult GetOperationLog(int Id, int? start, int? count)
        {
            int total = 0;

            var logModelList = new List<OperationLogModel>();
            var logList = _AppContext.LogOperatorApp.Select(Id.ToString(), ELogType.ImageCarousel, start ?? 0, count ?? 0, out total);
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

    }
}