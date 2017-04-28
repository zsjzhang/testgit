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
using Vcyber.BLMS.Common;
using Vcyber.BLMS.Common.Web;

namespace Vcyber.BLMS.ManageWeb.Controllers
{
    [MvcAuthorize]
    public class MagazineController : Controller
    {
        // GET: Magazine
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetMagazinelList(int? year, int? month, string name, int? start,int? count)
        {
            int total = 0;

            var data = _AppContext.MagazineApp.GetMagazineList(null,year ?? 0, month ?? 0, name, start ?? 0, count ?? 0, out total);

           
            var imgCarouselModelList = new List<MagazineModel>();
            if (data != null || data.Count() > 0)
            {
                foreach (var item in data)
                {
                    var model = new MagazineModel();
                    model.Id = item.Id;
                    model.ImageUrl = item.ImageUrl;
                    model.ImageUrl = item.TrueImageUrl;
                    model.LinkUrl = item.LinkUrl;
                    //model.Summary = item.Summary;
                    model.Summary =WebUtils.ReplaceHtmlTag(item.TrueSummary);
                    model.Title = item.Title;
                    model.Year = item.Year;
                    model.Month = item.Month;
                    model.UpdateTime = item.UpdateTime.ToShortDateString();
                    model.IsApproved = item.IsApproved;
                    model.IsDisplay = item.IsDisplay;
                    imgCarouselModelList.Add(model);
                }
            }
            var result = new { data = imgCarouselModelList, pos = start ?? 0, total_count = total };
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// 获取纸质杂志的申请记录
        /// </summary>
        /// <param name="datetime">申请时间</param>
        /// <param name="title">杂志标题</param>
        /// <returns></returns>
        public JsonResult GetMagazinelApplyList(string datetime, string title, string phoneNum, int? start, int? count)
        {
            try
            {
                int total = 0;
                int year = 0;
                int month = 0;
                int day = 0;
                if (!string.IsNullOrEmpty(datetime))
                {
                    var dateVar = Convert.ToDateTime(datetime);
                    year = dateVar.Year;
                    month = dateVar.Month;
                    day = dateVar.Day;
                }

                var data = _AppContext.MagazineApplyApp.GetMagazineApplyList(0, year, month,day, title, phoneNum, start ?? 0, count ?? 0, out total);
                var imgCarouselModelList = new List<MagazineApply>();
                if (data != null || data.Count() > 0)
                {
                    foreach (var item in data)
                    {
                        var model = new MagazineApply();
                        model.Id = item.Id;
                        model.MagazineId = item.MagazineId;
                        model.MagazineTitle = item.MagazineTitle;
                        model.CreateTime = item.CreateTime;
                        model.Phone = item.Phone;
                        model.PCC = item.PCC + item.Detail;
                        model.Detail = item.Detail;
                        model.ReceiveName = item.ReceiveName;
                        model.UpdateTime = item.UpdateTime;
                        model.Status = item.Status;
                        imgCarouselModelList.Add(model);
                    }
                }
                var result = new { data = imgCarouselModelList, pos = start ?? 0, total_count = total };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(e.Message, JsonRequestBehavior.AllowGet);
            }

        }

        /// <summary>
        /// 车型下拉框
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public JsonResult CarTypeView()
        {
            IEnumerable<Vcyber.BLMS.Entity.Generated.CSBaseCar> _result = _AppContext.BaseCarApp.QueryCars(ECarSeriesType.OrderCar);
            return Json(_result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 导出纸质杂志申请记录到Excel
        /// </summary>
        /// <param name="datetime"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public ActionResult ExportMagazineApplyData(string datetime, string title)
        {
            try
            {
                int year = 0;
                int month = 0;
                int day = 0;
                if (!string.IsNullOrEmpty(datetime))
                {
                    var dateVar = Convert.ToDateTime(datetime);
                    year = dateVar.Year;
                    month = dateVar.Month;
                    day = dateVar.Day;
                }

                var data = _AppContext.MagazineApplyApp.ExportMagazineApplyList(0, year, month, day, title);
                var magazineApplyList = new List<MagazineApply>();
                if (data != null || data.Count() > 0)
                {
                    foreach (var item in data)
                    {
                        var model = new MagazineApply();
                        model.Id = item.Id;
                        model.MagazineId = item.MagazineId;
                        model.MagazineTitle = item.MagazineTitle;
                        model.CreateTime = item.CreateTime;
                        model.Phone = item.Phone;
                        model.PCC = item.PCC + item.Detail;
                        model.Detail = item.Detail;
                        model.ReceiveName = item.ReceiveName;
                        model.UpdateTime = item.UpdateTime;
                        model.Status = item.Status;
                        model.ZipCode = item.ZipCode;
                        magazineApplyList.Add(model);
                    }
                }

                List<string> propertyName = new List<string> { "Detail", "ReceiveName", "Phone", "邮箱", "CreateTime" };
                List<string> columName = new List<string> {  "车型", "姓名", "手机号", "邮箱", "申请时间" };

                string fileName = string.Format("纸质杂志申请{0}", DateTime.Now.ToString("yyyyMMdd")) + ".xls";

                //创建Excel文件的对象
                NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();

                //添加一个sheet
                NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("纸质杂志申请列表");
                sheet1.SetColumnWidth(1, 150 * 36);
                sheet1.SetColumnWidth(3, 150 * 36);
                sheet1.SetColumnWidth(4, 150 * 36);
                sheet1.SetColumnWidth(5, 300 * 36);
                if (magazineApplyList != null)
                {

                    //给sheet1添加第一行的头部标题
                    NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
                    for (int i = 0; i < columName.Count; i++)
                    {
                        row1.CreateCell(i).SetCellValue(columName[i]);
                    }


                    //将数据逐步写入sheet1各个行
                    for (int i = 0; i < magazineApplyList.Count(); i++)
                    {
                        NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);
                        rowtemp.CreateCell(0, NPOI.SS.UserModel.CellType.String).SetCellValue(magazineApplyList[i].Detail.ToString());
                        rowtemp.CreateCell(1).SetCellValue(magazineApplyList[i].ReceiveName);
                        rowtemp.CreateCell(2).SetCellValue(magazineApplyList[i].Phone);
                        rowtemp.CreateCell(3).SetCellValue(magazineApplyList[i].ZipCode);
                        rowtemp.CreateCell(4).SetCellValue(magazineApplyList[i].CreateTime.ToShortDateString());
                    }
                }

                else
                {
                    NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
                    row1.CreateCell(0).SetCellValue("导出数据出错");

                }
                // 写入到客户端 
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                book.Write(ms);
                ms.Seek(0, System.IO.SeekOrigin.Begin);
                return File(ms, "application/ms-excel", fileName);

                //NPOIHelper<MagazineApply>.ListToExcel(magazineApplyList, fileName, propertyName, columName);
                //return null;

            }
            catch (Exception e)
            {
                return Json(e.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult MagazineDetail(int id)
        {
            var data = _AppContext.MagazineApp.GetMagazineById(id);
            if (data == null)
            {
                return Json(new { success = false, msg = "当前报刊不存在！" });
            }

            var model = new MagazineModel();
            model.Id = data.Id;
            model.ImageUrl = data.ImageUrl;
            model.LinkUrl = data.LinkUrl;
            model.Summary = data.Summary;
            model.Title = data.Title;
            model.Year = data.Year;
            model.Month = data.Month;
            model.UpdateTime = data.UpdateTime.ToShortDateString();
            model.IsDisplay = data.IsDisplay;
            model.QuestionUrl = data.QuestionUrl;
            model.ResultUrl = data.ResultUrl;
            model.ReadLink = data.ReadLink;
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {

            var data = _AppContext.MagazineApp.GetMagazineById(id);
            if (data == null)
            {
                return Json(new { success = false, msg = "当前不存在！" });
            }

            var success = _AppContext.MagazineApp.Delete(id, HttpContext.User.Identity.Name);
            if (success)
            {
                if (data.IsApproved != (int)EApproveStatus.NoBegin)
                {
                    //添加审批记录（删除报刊）
                    var approve = new ApproveRecord();
                    approve.ItemId =id;
                    approve.ItemType = ((int)EApproveType.Magazine).ToString();
                    approve.OperatorId = HttpContext.User.Identity.GetUserId();
                    approve.OperatorName = HttpContext.User.Identity.Name;
                    approve.ApprovalMemo = "删除报刊";
                    _AppContext.ApproveRecordApp.UpdateStatus(approve);
                }

                //添加操作记录（删除报刊）
                var operationData = new OperatorLog();
                operationData.Type = (int)ELogType.Magazine;
                operationData.SourceId = id.ToString();
                operationData.OperateTime = DateTime.Now;
                operationData.OperaterName = HttpContext.User.Identity.Name;
                operationData.OperaterId = HttpContext.User.Identity.GetUserId();
                operationData.OperateItem = "";
                operationData.OriginalValue = "";
                operationData.CurrentValue = "";
                operationData.Remark = "删除报刊";
                _AppContext.LogOperatorApp.Add(operationData, ELogType.Magazine);

                return Json(new { success = true, msg = "删除成功！" });
            }
            else
                return Json(new { success = false, msg = "删除失败！" });
        }
        public ActionResult EditMagazine()
        {
            return View();
        }

        [HttpPost]
        public ActionResult EditMagazine(MagazineModel model)
        {
            string message = "";
            if (!ModelState.IsValid)
            {
                foreach (var value in ModelState.Values)
                {
                    if(value.Errors!=null&&value.Errors.Count>0)
                    {
                        foreach( var error in value.Errors){
                            message +=error.ErrorMessage;
                        }
                    }
                }
                return Json(new { success = false, msg = message });
            }

            //check whether this item is existed
            var data = _AppContext.MagazineApp.GetMagazineById(model.Id);
            if (data == null)
            {
                return Json(new { success = false, msg = "当前不存在！" });
            }
            // check whether one property should be unique

            data.ImageUrl = model.ImageUrl;
            data.LinkUrl = model.LinkUrl;
            data.Summary = model.Summary;
            data.Title = model.Title;
            data.Year = model.Year;
            data.Month = model.Month;
            data.UpdateBy = HttpContext.User.Identity.Name;
            data.QuestionUrl = model.QuestionUrl;
            data.ResultUrl = model.ResultUrl;
            data.ReadLink = model.ReadLink;
            var success = _AppContext.MagazineApp.Update(data);
            if (success)
            {
                if (data.IsApproved != (int) EApproveStatus.NoBegin)
                {
                    //添加审批记录（编辑报刊）
                    var approve = new ApproveRecord();
                    approve.ItemId = model.Id;
                    approve.ItemType = ((int) EApproveType.Magazine).ToString();
                    approve.OperatorId = HttpContext.User.Identity.GetUserId();
                    approve.OperatorName = HttpContext.User.Identity.Name;
                    approve.ApprovalMemo = "编辑报刊";
                    _AppContext.ApproveRecordApp.UpdateStatus(approve);
                }
                //添加操作记录（编辑报刊）
                var operationData = new OperatorLog();
                operationData.Type = (int)ELogType.Magazine;
                operationData.SourceId = model.Id.ToString();
                operationData.OperateTime = DateTime.Now;
                operationData.OperaterName = HttpContext.User.Identity.Name;
                operationData.OperaterId = HttpContext.User.Identity.GetUserId();
                operationData.OperateItem = "";
                operationData.OriginalValue = "";
                operationData.CurrentValue = "";
                operationData.Remark = "编辑报刊";
                _AppContext.LogOperatorApp.Add(operationData, ELogType.Magazine);

                return Json(new { success = true, msg = "更新成功！" });
            }
            else
                return Json(new { success = false, msg = "更新失败！" });


        }
        public ActionResult CreateMagazine()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateMagazine(MagazineModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, msg = "参数错误" });
            }
            var data = new Magazine();
            data.ImageUrl = model.ImageUrl;
            data.LinkUrl = model.LinkUrl;
            data.Summary = model.Summary;
            data.Title = model.Title;
            data.Year = model.Year;
            data.Month = model.Month;
            data.IsDisplay = model.IsDisplay;
            data.IsApproved = 0;
            data.CreateBy = HttpContext.User.Identity.Name;
            data.UpdateBy = HttpContext.User.Identity.Name;
            data.QuestionUrl = model.QuestionUrl;
            data.ResultUrl = model.ResultUrl;
            data.ReadLink = model.ReadLink;

            var result = _AppContext.MagazineApp.Create(data);
            if (result > 0)
            {
                //创建审批记录（创建报刊）
                var approve = new ApproveRecord();
                approve.ItemId = result;
                approve.ItemType = ((int)EApproveType.Magazine).ToString();
                approve.ApprovalMemo = "创建报刊";
                approve.OperatorId = HttpContext.User.Identity.GetUserId();
                approve.OperatorName = HttpContext.User.Identity.Name;
                _AppContext.ApproveRecordApp.UpdateStatus(approve);

                //创建操作记录（创建报刊）
                var operationData = new OperatorLog();
                operationData.Type = (int)ELogType.Magazine;
                operationData.SourceId = result.ToString();
                operationData.OperateTime = DateTime.Now;
                operationData.OperaterName = HttpContext.User.Identity.Name;
                operationData.OperaterId = HttpContext.User.Identity.GetUserId();
                operationData.OperateItem = "";
                operationData.OriginalValue = "";
                operationData.CurrentValue = "";
                operationData.Remark = "创建报刊";
                _AppContext.LogOperatorApp.Add(operationData, ELogType.Magazine);


                return Json(new { success = true, msg = "创建成功！" });
            }
            else
                return Json(new { success = false, msg = "创建失败！" });
        }
        public JsonResult GetApproveList(int id)
        {
            var data = _AppContext.ApproveRecordApp.GetApproveRecordList(id, ((int)EApproveType.Magazine).ToString());
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
            var logList = _AppContext.LogOperatorApp.Select(Id.ToString(), ELogType.Magazine, start ?? 0, count ?? 0, out total);
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
            var obj = _AppContext.MagazineApp.GetMagazineById(id);
            if (obj == null)
            {
                return Json(new { success = false, msg = "当前报刊不存在！" });
            }
            if (status == obj.IsDisplay)
            {
                return Json(new { success = true, msg = "值没有改变" });
            }
            obj.IsDisplay = status;
            obj.UpdateBy = HttpContext.User.Identity.Name;

            var data = _AppContext.MagazineApp.Update(obj);

            if (data)
            {
                
                //添加操作记录（修改新闻显示状态）
                var operationData = new OperatorLog();
                operationData.Type = (int)ELogType.Magazine;
                operationData.SourceId = id.ToString();
                operationData.OperateTime = DateTime.Now;
                operationData.OperaterName = HttpContext.User.Identity.Name;
                operationData.OperaterId = HttpContext.User.Identity.GetUserId();
                operationData.OperateItem = "显示状态";
                operationData.OriginalValue = obj.IsDisplay.ToString();
                operationData.CurrentValue = status.ToString();
                operationData.Remark = "修改新闻显示状态";
                _AppContext.LogOperatorApp.Add(operationData, ELogType.News);

                return Json(new { success = true, msg = "成功！" });
            }
            else
                return Json(new { success = false, msg = "失败！" });
        }


        // GET: MagazineApplyManage
        public ActionResult MagazineApplyManage()
        {
            return View();
        }

    }
}