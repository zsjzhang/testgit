using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Configuration;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.ManageWeb.Models;

namespace Vcyber.BLMS.ManageWeb.Controllers
{
    [MvcAuthorize]
    public class ActivitiesController : Controller
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

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CreateActivities()
        {
            return View();
        }
        /// <summary>
        /// 创建活动
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CreateActivities(ActivitiesModel model)
        {
            if (!ModelState.IsValid || DateTime.Parse(model.BeginTime) > DateTime.Parse(model.EndTime))
            {
                return Json(new { success = false, msg = "参数错误" });
            }

            var activities = new Activities();
            activities.Title = model.Title;
            activities.MajorImageUrl = model.MajorImageUrl;
            activities.SignUp = model.SignUp;
            activities.BeginTime = DateTime.Parse(model.BeginTime);
            activities.EndTime = DateTime.Parse(model.EndTime);
            activities.CreateBy = HttpContext.User.Identity.Name;
            activities.UpdateBy = HttpContext.User.Identity.Name;
            activities.Content = model.Content;
            activities.IsUrl = model.IsUrl;
            activities.Url = model.Url;
            activities.IsDisplay = model.IsDisplay;
            activities.IsHot = 0;
            activities.Summary = model.Summary;
            activities.IsCarOwner = model.IsCarOwner;
            activities.Dealer = DealerId();
            activities.Priority = model.Priority;
            var result = _AppContext.ActivitiesApp.AddActivities(activities);
            if (result > 0)
            {
                //创建审批记录
                var approve = new ApproveRecord();
                approve.ItemId = result;
                approve.ItemType = ((int)EApproveType.Activities).ToString();
                approve.OperatorId = HttpContext.User.Identity.GetUserId();
                approve.OperatorName = HttpContext.User.Identity.Name;
                approve.ApprovalMemo = "创建活动";
                approve.IsApproval = (int)EApproveStatus.NoBegin;
                _AppContext.ApproveRecordApp.UpdateStatus(approve);

                //创建操作记录
                var operationData = new OperatorLog();
                operationData.Type = (int)ELogType.Activities;
                operationData.SourceId = result.ToString();
                operationData.OperateTime = DateTime.Now;
                operationData.OperaterName = HttpContext.User.Identity.Name;
                operationData.OperaterId = HttpContext.User.Identity.GetUserId();
                operationData.OperateItem = "";
                operationData.OriginalValue = "";
                operationData.CurrentValue = "";
                operationData.Remark = "创建活动";
                _AppContext.LogOperatorApp.Add(operationData, ELogType.Activities);

                return Json(new { success = true, msg = "创建成功" });
            }
            else
            { return Json(new { success = true, msg = "创建失败" }); }

        }
        /// <summary>
        /// 编辑活动
        /// </summary>
        /// <returns></returns>
        public ActionResult EditActivities()
        {
            return View();
        }

        public JsonResult LoadActivities(int id)
        {
            var data = _AppContext.ActivitiesApp.GetActivitiesById(id);
            var model = new ActivitiesModel();
            if (data != null)
            {
                model.Content = data.Content;
                model.SignUp = data.SignUp;
                model.MajorImageUrl = data.MajorImageUrl;
                model.BeginTime = data.BeginTime.ToShortDateString();
                model.EndTime = data.EndTime.ToShortDateString();
                model.Title = data.Title;
                model.IsHot = data.IsHot;
                model.IsUrl = data.IsUrl;
                model.Summary = data.Summary;
                model.Url = data.Url;
                model.IsCarOwner = data.IsCarOwner;
                model.Priority = data.Priority;
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EditActivities(ActivitiesModel model)
        {
            if (!ModelState.IsValid || DateTime.Parse(model.BeginTime) > DateTime.Parse(model.EndTime))
            {
                return Json(new { success = false, msg = "参数错误" });
            }

            var obj = _AppContext.ActivitiesApp.GetActivitiesById(model.Id);
            if (obj == null)
            {
                return Json(new { success = false, msg = "当前活动不存在" });
            }
            //if (model.Status == 1 || model.Status == 2)
            //{
            //    return Json(new { success = false, msg = "当前活动正在进行或者已经结束，不能编辑" });
            //}

            //  var activities = new Activities();
            //activities.Id = model.Id;
            obj.Title = model.Title;
            obj.MajorImageUrl = model.MajorImageUrl;
            obj.SignUp = model.SignUp;
            obj.BeginTime = DateTime.Parse(model.BeginTime);
            obj.EndTime = DateTime.Parse(model.EndTime);
            obj.UpdateBy = HttpContext.User.Identity.Name;
            obj.Content = model.Content;
            obj.IsUrl = model.IsUrl;
            obj.Summary = model.Summary;
            obj.Url = model.Url;
            obj.IsCarOwner = model.IsCarOwner;
            obj.Priority = model.Priority;
            var result = _AppContext.ActivitiesApp.UpdateActivities(obj);
            if (result)
            {
                if (obj.IsApproved != (int)EApproveStatus.NoBegin)
                {
                    //添加审批记录
                    var approve = new ApproveRecord();
                    approve.ItemId = model.Id;
                    approve.ItemType = ((int)EApproveType.Activities).ToString();
                    approve.OperatorId = HttpContext.User.Identity.GetUserId();
                    approve.OperatorName = HttpContext.User.Identity.Name;
                    approve.ApprovalMemo = "更新活动";
                    approve.IsApproval = (int)EApproveStatus.NoBegin;
                    _AppContext.ApproveRecordApp.UpdateStatus(approve);
                }
                //添加操作记录
                var operationData = new OperatorLog();
                operationData.Type = (int)ELogType.Activities;
                operationData.SourceId = model.Id.ToString();
                operationData.OperateTime = DateTime.Now;
                operationData.OperaterName = HttpContext.User.Identity.Name;
                operationData.OperaterId = HttpContext.User.Identity.GetUserId();
                operationData.OperateItem = "";
                operationData.OriginalValue = "";
                operationData.CurrentValue = "";
                operationData.Remark = "更新活动";
                _AppContext.LogOperatorApp.Add(operationData, ELogType.Activities);

                return Json(new { success = true, msg = "编辑成功" });
            }
            else
            { return Json(new { success = true, msg = "编辑失败" }); }

        }

        public ActionResult GetSignUpActivities()
        {
            return View();
        }

        public JsonResult LoadSignUpList(int id, string userName, int? start, int? count)
        {
            var obj = _AppContext.ActivitiesApp.GetActivitiesById(id);
            if (obj == null)
            {
                return Json(new { success = false, msg = "当前活动不存在" });
            }
            if (obj.SignUp == 0)
            {
                return Json(new { success = false, msg = "当前活动不支持报名" });
            }

            int total = 0;
            var modelList = new List<ActivitiesSignUpModel>();
            var result = _AppContext.ActivitiesSignUpApp.GetSignUpList(id, userName, start ?? 0, count ?? 0, out total);
            if (result != null)
            {
                foreach (var item in result)
                {
                    var model = new ActivitiesSignUpModel();
                    model.Id = item.Id;
                    model.UserName = item.UserName;
                    model.UserId = item.UserId;
                    model.CreateTime = item.CreateTime.ToShortDateString();
                    model.ActivitiesId = item.ActivitiesId;

                    modelList.Add(model);
                }
            }

            var returnobj = new { data = modelList, pos = start ?? 0, total_count = total };
            return Json(returnobj, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {

            var data = _AppContext.ActivitiesApp.GetActivitiesById(id);
            if (data == null)
            {
                return Json(new { success = false, msg = "当前活动不存在！" });
            }
            if (data.BeginTime <= DateTime.Now && data.EndTime >= DateTime.Now)
            {
                return Json(new { success = false, msg = "当前活动正在进行，不能删除" });
            }

            var success = _AppContext.ActivitiesApp.DeleteActivities(id, HttpContext.User.Identity.Name);
            if (success)
            {
                //添加操作记录
                var operationData = new OperatorLog();
                operationData.Type = (int)ELogType.Activities;
                operationData.SourceId = id.ToString();
                operationData.OperateTime = DateTime.Now;
                operationData.OperaterName = HttpContext.User.Identity.Name;
                operationData.OperaterId = HttpContext.User.Identity.GetUserId();
                operationData.OperateItem = "";
                operationData.OriginalValue = "";
                operationData.CurrentValue = "";
                operationData.Remark = "删除活动";
                _AppContext.LogOperatorApp.Add(operationData, ELogType.Activities);

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
        /// <summary>
        /// 获取活动列表 
        /// </summary>
        /// <param name="status"></param>
        /// <param name="dealer"></param>
        /// <param name="start"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public JsonResult GetActivitiesList(int? status, string dealer, int? start, int? count)
        {
            int total = 0;
            var activitiesModelList = new List<ActivitiesModel>();

            var user = UserManager.FindById(HttpContext.User.Identity.GetUserId());

            if (!string.IsNullOrEmpty(user.DealerId))
            {
                dealer = user.DealerId;
            }
            //if (dealer=="-1")
            //{
                dealer = null;
           // }
            var activitiesList = _AppContext.ActivitiesApp.SelectStatus(dealer, status, null, null, null, start ?? 0, count ?? 0, out total);
            if (activitiesList != null)
            {
                foreach (var item in activitiesList)
                {
                    var activities = new ActivitiesModel();
                    activities.Id = item.Id;
                    activities.Title = item.Title;
                    activities.SignUp = item.SignUp;
                    activities.BeginTime = item.BeginTime.ToShortDateString();
                    activities.EndTime = item.EndTime.ToShortDateString();
                    activities.MajorImageUrl = item.MajorImageUrl;

                    activities.TrueImageUrl = item.TrueMajorImageUrl;
                    activities.IsApproved = item.IsApproved;
                    activities.IsUrl = item.IsUrl;
                    activities.IsHot = item.IsHot;
                    activities.IsDisplay = item.IsDisplay;
                    activities.Priority = item.Priority;
                    var dealerObj = _AppContext.DealerApp.GetDealerByDealerId(item.Dealer);
                    if (dealerObj != null)
                    {
                        activities.DealerName = dealerObj.Name;
                    }
                    activitiesModelList.Add(activities);
                }
            }

            var result = new { data = activitiesModelList, pos = start ?? 0, total_count = total };
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetApproveList(int id)
        {
            var data = _AppContext.ApproveRecordApp.GetApproveRecordList(id, ((int)EApproveType.Activities).ToString());
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

        [HttpPost]
        public JsonResult UpdateIsDisplay(int id, int status)
        {
            var obj = _AppContext.ActivitiesApp.GetActivitiesById(id);
            if (obj == null)
            {
                return Json(new { success = false, msg = "当前活动不存在！" });
            }
            if (status == obj.IsDisplay)
            {
                return Json(new { success = false, msg = "值没有改变" });
            }
            obj.IsDisplay = status;
            obj.UpdateBy = HttpContext.User.Identity.Name;

            var data = _AppContext.ActivitiesApp.UpdateIsDisplay(obj.Id, status, HttpContext.User.Identity.Name);

            if (data)
            {
                //添加操作记录（修改显示状态）
                var operationData = new OperatorLog();
                operationData.Type = (int)ELogType.Activities;
                operationData.SourceId = id.ToString();
                operationData.OperateTime = DateTime.Now;
                operationData.OperaterName = HttpContext.User.Identity.Name;
                operationData.OperaterId = HttpContext.User.Identity.GetUserId();
                operationData.OperateItem = "显示状态";
                operationData.OriginalValue = obj.IsDisplay.ToString();
                operationData.CurrentValue = status.ToString();
                operationData.Remark = "修改显示状态";
                _AppContext.LogOperatorApp.Add(operationData, ELogType.Activities);

                return Json(new { success = true, msg = "成功！" });
            }
            else
                return Json(new { success = false, msg = "失败！" });
        }

        [HttpPost]
        public JsonResult UpdateAllDisplay(int id, int priority, int dispaly, int isHot)
        {
          
            var obj = _AppContext.ActivitiesApp.GetActivitiesById(id);
            if (obj == null)
            {
                return Json(new { success = false, msg = "当前活动不存在！" });
            }

            var user = UserManager.FindById(HttpContext.User.Identity.GetUserId());

            if (!string.IsNullOrEmpty(user.DealerId))
            {
                if (obj.Priority != priority || obj.IsHot != isHot)
                {
                    return Json(new { success = false, msg = "当前账号不能更新权重和热点！" });

                }

            }

            var data = _AppContext.ActivitiesApp.UpdateAllDisplay(obj.Id, priority, dispaly, isHot, HttpContext.User.Identity.Name);

            if (data)
            {
                //添加操作记录（修改活动权值）
                var operationData = new OperatorLog();
                operationData.Type = (int)ELogType.Activities;
                operationData.SourceId = id.ToString();
                operationData.OperateTime = DateTime.Now;
                operationData.OperaterName = HttpContext.User.Identity.Name;
                operationData.OperaterId = HttpContext.User.Identity.GetUserId();
                operationData.OperateItem = "";
                operationData.OriginalValue = "";
                operationData.CurrentValue ="";
                operationData.Remark = "修改";
                _AppContext.LogOperatorApp.Add(operationData, ELogType.Activities);

                return Json(new { success = true, msg = "成功！" });
            }
            else
                return Json(new { success = false, msg = "失败！" });
        }

        public ActionResult OperationLog()
        {
            return View();
        }

        public JsonResult GetOperationLog(int Id, int? start, int? count)
        {
            int total = 0;

            var logModelList = new List<OperationLogModel>();
            var logList = _AppContext.LogOperatorApp.Select(Id.ToString(), ELogType.Activities, start ?? 0, count ?? 0, out total);
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

        /// <summary>
        /// 活动预览
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Detail(int id)
        {
            int _movementStatus = 0;
           
            Activities _result = _AppContext.ActivitiesApp.GetActivitiesById(id);
            if (_result.EndTime != null && _result.EndTime <= DateTime.Now)
            {
                _movementStatus = 2;
            }
            //判断活动状态
            ViewBag.movementStatus = _movementStatus;
            return View(_result);
        }

        //私有方法
        private string DealerId()
        {
            var user = UserManager.FindById(HttpContext.User.Identity.GetUserId());

            return user.DealerId;
        }

        #region==置换活动管理 ======================================================
        /// <summary>
        /// 置换活动首页 PermuteIndex  Activities
        /// </summary>
        /// <returns></returns>
        public ActionResult PermuteIndex()
        {
            return View();
        }
        /// <summary>
        /// 获取置换活动数据列表
        /// </summary>
        /// <param name="status"></param>
        /// <param name="dealer"></param>
        /// <param name="start"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public JsonResult GetPermuteActivitiesList(int? status, string dealer, int? start, int? count)
        {
            int total = 0;
            var permuteActivityModelList = new List<XDActivityModel>();

            var user = UserManager.FindById(HttpContext.User.Identity.GetUserId());
            if (!string.IsNullOrEmpty(user.DealerId))
            {
                dealer = user.DealerId;
            }
            dealer = null;

            var permuteActivityList = _AppContext.XDActivityApp.SelectStatus(dealer, status, null, null, null, start ?? 0, count ?? 0, out total);

            if(permuteActivityList!=null)
            {
                foreach (var item in permuteActivityList)
                {
                    var permuteActivities=new XDActivityModel();
                    permuteActivities.ActivityId=item.ActivityId;
                    permuteActivities.XDActivityName=item.ActivityName;
                    permuteActivities.ActivitySubImage=item.ActivitySubImage;
                    permuteActivities.ActivityStatus=item.ActivityStatus;
                    
                    permuteActivities.IsValid=item.IsValid;
                    permuteActivities.ActivityStartTime=item.ActivityStartTime.ToString();

                    permuteActivities.ActivityEndTime=item.ActivityEndTime.ToString();
                    permuteActivities.ActivityUrl=item.ActivityUrl;

                    permuteActivityModelList.Add(permuteActivities);
                }
            }
            var result = new { data = permuteActivityModelList, pos = start ?? 0, total_count = total };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #region======= 新增  修改  删除 设置是否显示
        //创建新活动
        public ActionResult CreatePermuteActivity()
        {
            return View();
        }
        [HttpPost]
        public JsonResult CreatePermuteActivity(XDActivityModel activityModel)
        {
            if (!ModelState.IsValid || (Convert.ToDateTime(activityModel.ActivityStartTime) > Convert.ToDateTime(activityModel.ActivityEndTime)))
            {
                return Json(new { success = false, msg = "参数错误" });
            }
            var activity = new XDActivity();
            activity.ActivityName = activityModel.XDActivityName;
            activity.ActivityTitle = activityModel.ActivityTitle;
            activity.ActivitySubImage = activityModel.ActivitySubImage;
            activity.ActivityImage = activityModel.ActivityImage;
            activity.ActivityContent = activityModel.ActivityContent;
            activity.IsValid = activityModel.IsValid;
            activity.ActivityStartTime = DateTime.Parse(activityModel.ActivityStartTime);
            activity.ActivityEndTime = DateTime.Parse(activityModel.ActivityEndTime);
            activity.CreaterName = HttpContext.User.Identity.Name;
            activity.UpdaterName = HttpContext.User.Identity.Name;

            //写入数据
            var result = _AppContext.XDActivityApp.AddXDActivity(activity);
            if(result>0)
            {
                return Json(new { success = true, msg = "创建成功" });
            }
            else
            { return Json(new { success = false, msg = "创建失败" }); }
        }
        //编辑活动
        public ActionResult EditPermuteActivity()
        {
            return View();
        }
        //通过Id拿到数据
        public JsonResult LoadPermuteActivityById(int id)
        {
            var dataActivity = _AppContext.XDActivityApp.GetXDActivityById(id);
            var permuteModel = new XDActivityModel();
            if (dataActivity != null)
            {
                permuteModel.XDActivityName = dataActivity.ActivityName;
                permuteModel.ActivitySubImage = dataActivity.ActivitySubImage;

                permuteModel.ActivityTitle = dataActivity.ActivityTitle;

                permuteModel.IsValid = dataActivity.IsValid;
                permuteModel.ActivityStartTime = dataActivity.ActivityStartTime.ToString();

                permuteModel.ActivityEndTime = dataActivity.ActivityEndTime.ToString();
                permuteModel.ActivityContent = dataActivity.ActivityContent;
            }
            return Json(permuteModel, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 编辑活动
        /// </summary>
        /// <param name="activityModel"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EditPermuteActivity(XDActivityModel activityModel)
        {
             if (!ModelState.IsValid || DateTime.Parse(activityModel.ActivityStartTime) > DateTime.Parse(activityModel.ActivityEndTime))
            {
                return Json(new { success = false, msg = "参数错误" });
            }

             var obj = _AppContext.XDActivityApp.GetXDActivityById(activityModel.ActivityId);
            if (obj == null)
            {
                return Json(new { success = false, msg = "当前活动不存在" });
            }
            //拿到数据
            obj.ActivityName=activityModel.XDActivityName;
            obj.ActivityTitle = activityModel.ActivityTitle;
            obj.ActivitySubImage = activityModel.ActivitySubImage;
            obj.ActivityImage = activityModel.ActivityImage;
            obj.ActivityContent = activityModel.ActivityContent;
            obj.IsValid = activityModel.IsValid;
            obj.ActivityStartTime =DateTime.Parse(activityModel.ActivityStartTime);
            obj.ActivityEndTime = DateTime.Parse(activityModel.ActivityEndTime);
            obj.UpdaterName = HttpContext.User.Identity.Name;

            //更新
            var result = _AppContext.XDActivityApp.UpdateActivities(obj);

            if (result)
            {
                return Json(new { success = true, msg = "编辑成功" });
            }
             else
            { 
                return Json(new { success = false, msg = "编辑失败" }); 
            }
        }
        /// <summary>
        /// 删除活动
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult deletePermute(int id)
        {
            var data = _AppContext.XDActivityApp.GetXDActivityById(id);
            if (data == null)
            {
                return Json(new { success = false, msg = "当前活动不存在！" });
            }
            if (data.ActivityStartTime <= DateTime.Now && data.ActivityEndTime >= DateTime.Now)
            {
                return Json(new { success = false, msg = "当前活动正在进行，不能删除" });
            }

            var success = _AppContext.XDActivityApp.DeleteActivities(id, HttpContext.User.Identity.Name);
            if (success)
            {
                return Json(new { success = true, msg = "删除成功！" });
            }
            else
            {
                return Json(new { success = false, msg = "删除失败！" });

            }

        }



        #endregion

        #endregion
    }

}