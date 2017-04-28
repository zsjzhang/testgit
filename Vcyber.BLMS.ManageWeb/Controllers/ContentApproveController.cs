using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.ManageWeb.Models;

namespace Vcyber.BLMS.ManageWeb.Controllers
{
    [MvcAuthorize]
    public class ContentApproveController : Controller
    {
        //
        // GET: /ContentApproved/
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult ApprovedType()
        {
            var optionsList = new List<OptionType>();
            foreach (var item in Enum.GetValues(typeof(EApproveType)))
            {
                var option = new OptionType();
                option.id = ((int)item).ToString();
                option.value = item.GetDiscribe();
                optionsList.Add(option);
            }

            return Json(optionsList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ApprovedStatus()
        {
            var optionsList = new List<OptionType>();
            // optionsList.Add(new OptionType("-1", "全部"));
            foreach (var item in Enum.GetValues(typeof(EApproveStatus)))
            {
                var option = new OptionType();
                option.id = ((int)item).ToString();
                option.value = item.GetDiscribe();
                optionsList.Add(option);
            }

            return Json(optionsList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetList(int? start, int? count, int? type,int? status)
        {
            var modelList = new List<ContentApproveModel>();
            int total = 0;
            if (type==null)
            {
                type = 1;
            }
            if (type == (int)EApproveType.News)
            {
                var list = _AppContext.NewsApp.GetNews(status ?? 0, start ?? 0, count ?? 0, out total);

                if (list != null)
                {
                    foreach (var obj in list)
                    {
                        var model = new ContentApproveModel();
                        model.SouceId = obj.Id;
                        model.Title = obj.Title;
                        model.Status = obj.IsApproved;
                        model.Type = (int)EApproveType.News;
                        model.UpdateBy = obj.UpdateBy;
                        model.UpdateTime = obj.UpdateTime.ToShortDateString();
                        model.ApproveBy = obj.ApprovedBy;
                        model.ApproveTime = obj.ApprovedTime.ToShortDateString();
                        modelList.Add(model);
                    }
                }
            }

            if (type == (int)EApproveType.Magazine)
            {
                var list = _AppContext.MagazineApp.GetMagazine(status ?? 0, start ?? 0, count ?? 0, out total);

                if (list != null)
                {
                    foreach (var obj in list)
                    {
                        var model = new ContentApproveModel();
                        model.SouceId = obj.Id;
                        model.Title = obj.Title;
                        model.Status = obj.IsApproved;
                        model.Type = (int)EApproveType.Magazine;
                        model.UpdateBy = obj.UpdateBy;
                        model.UpdateTime = obj.UpdateTime.ToShortDateString();
                        model.ApproveBy = obj.ApprovedBy;
                        model.ApproveTime = obj.ApprovedTime.ToShortDateString();
                        modelList.Add(model);
                    }
                }
            }
            if (type == (int)EApproveType.UserGuide)
            {
                var list = _AppContext.UserGuideApp.GetUserGuide(status ?? 0, start ?? 0, count ?? 0, out total);

                if (list != null)
                {
                    foreach (var obj in list)
                    {
                        var model = new ContentApproveModel();
                        model.SouceId = obj.Id;
                        model.Title = obj.Title;
                        model.Status = obj.IsApproved;
                        model.Type = (int)EApproveType.UserGuide;
                        model.UpdateBy = obj.UpdateBy;
                        model.UpdateTime = obj.UpdateTime.ToShortDateString();
                        model.ApproveBy = obj.ApprovedBy;
                        model.ApproveTime = obj.ApprovedTime.ToShortDateString();
                        modelList.Add(model);
                    }
                }
            }
            if (type == (int)EApproveType.ImageCarousel)
            {
                var list = _AppContext.ImageCarouselApp.GetImageCarousel(status ?? 0, start ?? 0, count ?? 0, out total);

                if (list != null)
                {
                    foreach (var obj in list)
                    {
                        var model = new ContentApproveModel();
                        model.SouceId = obj.Id;
                        model.Title = obj.Title;
                        model.Status = obj.IsApproved;
                        model.Type = (int)EApproveType.ImageCarousel;
                        model.UpdateBy = obj.UpdateBy;
                        model.UpdateTime = obj.UpdateTime.ToShortDateString();
                        model.ApproveBy = obj.ApprovedBy;
                        if (obj.ApprovedTime>DateTime.MinValue)
                        {
                            model.ApproveTime = obj.ApprovedTime.ToShortDateString();
                       
                        }
                        modelList.Add(model);
                    }
                }

            }
            if (type == (int)EApproveType.Activities)
            {
                var list = _AppContext.ActivitiesApp.GetActivities(status ?? 0, start ?? 0, count ?? 0, out total);

                if (list != null)
                {
                    foreach (var obj in list)
                    {
                        var model = new ContentApproveModel();
                        model.SouceId = obj.Id;
                        model.Title = obj.Title;
                        model.Status = obj.IsApproved;
                        model.Type = (int)EApproveType.Activities;
                        model.UpdateBy = obj.UpdateBy;
                        model.UpdateTime = obj.UpdateTime.ToShortDateString();
                        model.ApproveBy = obj.ApprovedBy;
                        model.ApproveTime = obj.ApprovedTime.ToShortDateString();
                        modelList.Add(model);
                    }
                }

            }
            var result = new { data = modelList, pos = start ?? 0, total_count = total };
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult Approval(int id,int type, int status, string memo)
        {
            if (status > 2 || status < 0 || memo.Length <= 0 || memo.Length > 100)
            {
                return Json(new { success = false, msg = "参数不对" });
            }
            var data = false;
            if (type == (int)EApproveType.News)
            {
                var obj = _AppContext.NewsApp.GetNewsById(id);
                if (obj.IsApproved == status)
                {
                    return Json(new { success = false, msg = "审批状态没有修改" });

                }
                
                data = _AppContext.NewsApp.ApprovedNews(id, HttpContext.User.Identity.GetUserId(), HttpContext.User.Identity.Name, status, memo);
            }
            if (type == (int)EApproveType.Activities)
            {
                var obj = _AppContext.ActivitiesApp.GetActivitiesById(id);
                if (obj.IsApproved == status)
                {
                    return Json(new { success = false, msg = "审批状态没有修改" });

                }

                data = _AppContext.ActivitiesApp.ApprovedActivities(id, HttpContext.User.Identity.GetUserId(), HttpContext.User.Identity.Name, status, memo);
            }
            if (type == (int)EApproveType.Magazine)
            {
                var obj = _AppContext.MagazineApp.GetMagazineById(id);
                if (obj.IsApproved == status)
                {
                    return Json(new { success = false, msg = "审批状态没有修改" });

                }

                data = _AppContext.MagazineApp.ApprovedMagazine(id, HttpContext.User.Identity.GetUserId(), HttpContext.User.Identity.Name, status, memo);
            }
            if (type == (int)EApproveType.UserGuide)
            {
                var obj = _AppContext.UserGuideApp.GetUserGuideById(id);
                if (obj.IsApproved == status)
                {
                    return Json(new { success = false, msg = "审批状态没有修改" });

                }

                data = _AppContext.UserGuideApp.ApprovedUserGuide(id, HttpContext.User.Identity.GetUserId(), HttpContext.User.Identity.Name, status, memo);
            }
            if (type == (int)EApproveType.ImageCarousel)
            {
                var obj = _AppContext.ImageCarouselApp.GetImageCarouselById(id);
                if (obj.IsApproved == status)
                {
                    return Json(new { success = false, msg = "审批状态没有修改" });

                }
                data = _AppContext.ImageCarouselApp.ApprovedImageCarousel(id, HttpContext.User.Identity.GetUserId(), HttpContext.User.Identity.Name, status, memo);
            }

            if (data)
            {
                ////创建审批记录（创建轮播图）
                //var approve = new ApproveRecord();
                //approve.ItemId = id;
                //approve.ItemType = (type).ToString();
                //approve.ApprovalMemo = "修改审批";
                //approve.OperatorId = HttpContext.User.Identity.GetUserId();
                //approve.OperatorName = HttpContext.User.Identity.Name;
                //_AppContext.ApproveRecordApp.UpdateStatus(approve);

                //添加操作记录（修改新闻审批状态）
                var operationData = new OperatorLog();
                operationData.Type = type;
                operationData.SourceId = id.ToString();
                operationData.OperateTime = DateTime.Now;
                operationData.OperaterName = HttpContext.User.Identity.Name;
                operationData.OperaterId = HttpContext.User.Identity.GetUserId();
                operationData.OperateItem = "审批状态";
                operationData.OriginalValue = "";
                operationData.CurrentValue = status.ToString();
                operationData.Remark = "修改审批状态";
                _AppContext.LogOperatorApp.Add(operationData, (ELogType)type);

                return Json(new { success = true, msg = "成功！" });
            }
            else
                return Json(new { success = false, msg = "失败！" });
        }
    }
}