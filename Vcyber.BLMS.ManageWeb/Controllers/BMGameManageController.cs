using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Application;
using System.IO;
using System.Data;
using Vcyber.BLMS.ManageWeb.Models;
using System.Net;

namespace Vcyber.BLMS.ManageWeb.Controllers
{
    public class BMGameManageController : Controller
    {
        //
        // GET: /BMGameManage/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ActivityInfo()
        {
            return View();
        }

        public ActionResult JoinActivity()
        {
            return View();
        }

        public ActionResult WinningInfo()
        {
            return View();
        }

        public ActionResult PrizeInfo()
        {
            return View();
        }

        public ActionResult ActivityIdOptions()
        {
            List<int> list = _AppContext.ActivityInfoApp.GetDistinctActivityId();
            return View(list);
        }

        public ActionResult ShareRecord()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ImportWinningInfo()
        {
            HttpPostedFileBase file = Request.Files["file"];
            string msg = "";
            if (file != null)
            {
                string fileName = file.FileName;

                if (!Path.GetExtension(fileName).ToUpper().Equals(".XLS") && !Path.GetExtension(fileName).ToUpper().Equals(".XLSX"))
                {
                    return Content("<script>alert('文件格式错误或不存在');window.open('" + Url.Content("~/#/BMGameManage/WinningInfo") + "','_self')</script>");
                }

                string filePath = Path.Combine(HttpContext.Server.MapPath("../UploadImg"), Guid.NewGuid().ToString() + Path.GetExtension(fileName));
                file.SaveAs(filePath);

                List<string> errors = new List<string>(5);
                DataTable dt = Common.NPOIHelper<OrderShipping>.ReadExcel(filePath, System.IO.Path.GetExtension(filePath));
                foreach (DataRow dr in dt.Rows)
                {
                    try
                    {

                        int activityId = int.Parse(dr["活动编号"].ToString());
                        string uId = dr["用户编号"].ToString();
                        string name = dr["姓名"].ToString();

                        string tel = dr["电话号码"].ToString();
                        string province = dr["省"].ToString();
                        string city = dr["市"].ToString();
                        string area = dr["县区"].ToString();
                        string address = dr["详细地址"].ToString();
                        int prizesId = int.Parse(dr["奖品编号"].ToString());

                        WinningInfo wiEntity = new WinningInfo();
                        wiEntity.ActivityId = activityId;
                        wiEntity.UserId = uId;
                        wiEntity.UserName = name;
                        wiEntity.UserTel = tel;
                        wiEntity.Province = province;
                        wiEntity.City = city;
                        wiEntity.Area = area;
                        wiEntity.Address = address;
                        wiEntity.PrizesId = prizesId;
                        wiEntity.UpdateTime = DateTime.Now;
                        bool ret = _AppContext.WinningInfoApp.AddWinningInfo(wiEntity);
                        if (ret)
                        {
                            msg = "名单导入成功";
                        }
                        else
                        {
                            msg = "名单导入失败";
                        }
                    }
                    catch (Exception ex)
                    {
                        msg = "异常:" + ex.Message.ToString();
                    }
                }
            }
            return Content("<script>alert('" + msg + "');window.open('" + Url.Content("~/#/BMGameManage/WinningInfo") + "','_self')</script>");
        }

        [HttpPost]
        public JsonResult SelectJoinActivityList(int? id, int pagesize, int pageindex)
        {
            int totalCount = 0;
            PageData page = new PageData() { Index = pageindex, Size = pagesize };
            var _aiList = _AppContext.JoinActivityApp.GetJoinActivitiesAll(id ?? 0, page, out totalCount);
            var result = new { joinactivity = _aiList, totalnum = totalCount };
            return Json(new { result, JsonRequestBehavior.AllowGet });
        }

        [HttpPost]
        public JsonResult SelectShareRecordList(int? id, int pagesize, int pageindex)
        {
            int totalCount = 0;
            PageData page = new PageData() { Index = pageindex, Size = pagesize };
            var _aiList = _AppContext.ShareRecordApp.GetShareRecordsByActivity(id ?? 0, page, out totalCount);
            var result = new { shareRecords = _aiList, totalnum = totalCount };
            return Json(new { result, JsonRequestBehavior.AllowGet });
        }

        #region activityinfo
        /// <summary>
        /// 结束活动  操作
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EndActivityInfo(int id)
        {
            var _ret = _AppContext.ActivityInfoApp.EndActivityInfo(id);
            return Json(new { issuccess = _ret, JsonRequestBehavior.AllowGet });
        }

        [HttpPost]
        public JsonResult SelectActivityInfoListById(int id)
        {
            var _aiList = _AppContext.ActivityInfoApp.GetActivityInfoAll();
            var _retList = _aiList.Where(f => f.Id == id);
            var result = new { activityInfos = _retList.ToArray() };
            return Json(new { result, JsonRequestBehavior.AllowGet });
        }

        [HttpPost]
        public JsonResult SelectActivityInfoList()
        {
            var _aiList = _AppContext.ActivityInfoApp.GetActivityInfoAll();
            var result = new { activityInfos = _aiList.ToArray() };
            return Json(new { result, JsonRequestBehavior.AllowGet });
        }

        [HttpPost]
        public JsonResult AddActivityInfo(ActivityInfo entity)
        {
            if (entity == null)
            {
                return Json(new { issuccess = false, message = "活动数据不存在" });
            }
            entity.UpdateDate = DateTime.Now;
            var _ret = _AppContext.ActivityInfoApp.AddActivityInfo(entity);
            return Json(new { issuccess = _ret, JsonRequestBehavior.AllowGet });
        }

        [HttpPost]
        public JsonResult EditActivityInfo(ActivityInfo entity)
        {
            if (entity == null)
            {
                return Json(new { issuccess = false, message = "活动数据不存在" });
            }
            entity.UpdateDate = DateTime.Now;
            var _ret = _AppContext.ActivityInfoApp.UpdateActivityInfo(entity);
            return Json(new { issuccess = _ret, JsonRequestBehavior.AllowGet });
        }

        #endregion

        #region prizesinfo

        [HttpPost]
        public JsonResult SelectPrizeInfos(int id, int pagesize, int pageindex)
        {
            PageData data = new PageData() { Size = pagesize, Index = pageindex };
            int totalCount = 0;
            var _piList = _AppContext.PrizesInfoApp.GetPrizesInfosByActivityId(id, data, out totalCount);
            var result = new { prizeInfos = _piList.ToArray(), totalNum = totalCount };
            return Json(new { result, JsonRequestBehavior.AllowGet });
        }

        [HttpPost]
        public JsonResult GetPrizeInfoById(int prizesId)
        {
            var prizeinfo = _AppContext.PrizesInfoApp.GetPrizeInfoMode(prizesId);
            //string imgPath = System.Web.Configuration.WebConfigurationManager.AppSettings["ImgPath"];
            //prizeinfo.Img = string.IsNullOrEmpty(prizeinfo.Img) ? "" : imgPath + prizeinfo.Img;
            return Json(new { prizeinfo, JsonRequestBehavior.AllowGet });
        }

        [HttpPost]
        public JsonResult addPrizesInfo(PrizesInfo pinfo)
        {
            if (pinfo == null)
            {
                return Json(new { issuccess = false, message = "实体模型不存在", JsonRequestBehavior.AllowGet });
            }
            pinfo.CyclesUnuseNum = pinfo.CyclesNum;
            pinfo.UpdateTime = DateTime.Now;
            var ret = _AppContext.PrizesInfoApp.AddPrizesInfo(pinfo);
            return Json(new { issuccess = ret, message = "", JsonRequestBehavior.AllowGet });
        }

        [HttpPost]
        public JsonResult editPrizesInfo(PrizesInfo pinfo)
        {
            if (pinfo == null)
            {
                return Json(new { issuccess = false, message = "实体模型不存在", JsonRequestBehavior.AllowGet });
            }
            var ret = _AppContext.PrizesInfoApp.UpdatePrizesInfo(pinfo);
            return Json(new { issuccess = ret, message = "", JsonRequestBehavior.AllowGet });
        }

        [HttpPost]
        public JsonResult deletePrizesInfo(int id)
        {
            var _ret = _AppContext.PrizesInfoApp.DelPrizesInfo(id);
            return Json(new { issuccess = _ret, JsonRequestBehavior.AllowGet });
        }

        #endregion

        #region WinningInfo

        [HttpPost]
        public JsonResult SelectWinnintInfos(int? id, int pageindex, int pagesize)
        {
            int totalNum = 0;
            PageData page = new PageData() { Size = pagesize, Index = pageindex };
            var _wiList = _AppContext.WinningInfoApp.GetWinningInfoByActivityId(id ?? 0, page, out totalNum);
            var result = new { winningInfos = _wiList.ToArray(), totalNum = totalNum };
            return Json(new { result, JsonRequestBehavior.AllowGet });
        }

        public FileResult WinningInfoExport(int? activityId)
        {
            //创建Excel文件的对象
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();

            //添加一个sheet
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("获奖名单");

            List<WinningInfo> winninginfos = new List<WinningInfo>();
            List<PrizesInfo> prizesInfos = new List<PrizesInfo>();

            //获取list数据
            winninginfos = _AppContext.WinningInfoApp.GetWinningInfosByActivityId(activityId ?? 0).ToList();
            prizesInfos = _AppContext.PrizesInfoApp.GetPrizesInfosByActivityId(activityId ?? 0).ToList();

            //给sheet1添加第一行的头部标题
            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
            row1.CreateCell(0).SetCellValue("编号");
            row1.CreateCell(1).SetCellValue("活动编号");

            row1.CreateCell(2).SetCellValue("用户编号");
            row1.CreateCell(3).SetCellValue("用户名");
            row1.CreateCell(4).SetCellValue("电话号码");
            row1.CreateCell(5).SetCellValue("省");

            row1.CreateCell(6).SetCellValue("市");
            row1.CreateCell(7).SetCellValue("县区");
            row1.CreateCell(8).SetCellValue("详细地址");
            row1.CreateCell(9).SetCellValue("奖品编号");
            row1.CreateCell(10).SetCellValue("奖品名称");
            row1.CreateCell(11).SetCellValue("价值(￥)");

            //将数据逐步写入sheet1各个行
            for (int i = 0; i < winninginfos.Count(); i++)
            {
                PrizesInfo pi = prizesInfos.SingleOrDefault(f => f.Id == winninginfos[i].PrizesId);

                NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);
                rowtemp.CreateCell(0).SetCellValue(winninginfos[i].Id.ToString());
                rowtemp.CreateCell(1).SetCellValue(winninginfos[i].ActivityId.ToString());

                rowtemp.CreateCell(2).SetCellValue(winninginfos[i].UserId);
                rowtemp.CreateCell(3).SetCellValue(winninginfos[i].UserName);
                rowtemp.CreateCell(4).SetCellValue(winninginfos[i].UserTel);
                rowtemp.CreateCell(5).SetCellValue(winninginfos[i].Province);

                rowtemp.CreateCell(6).SetCellValue(winninginfos[i].City);
                rowtemp.CreateCell(7).SetCellValue(winninginfos[i].Area);
                rowtemp.CreateCell(8).SetCellValue(winninginfos[i].Address);
                rowtemp.CreateCell(9).SetCellValue(winninginfos[i].PrizesId);
                rowtemp.CreateCell(10).SetCellValue(pi.Title);
                rowtemp.CreateCell(11).SetCellValue(pi.Price.ToString());
            }

            // 写入到客户端 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", "WinningInfos.xls");
        }

        public FileResult Export(int? activityId)
        {
            //创建Excel文件的对象
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();

            //添加一个sheet
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("参与人员导出");

            List<JoinActivity> joininfos = new List<JoinActivity>();

            //获取list数据
            joininfos = _AppContext.JoinActivityApp.GetJoinActivitiesByAId(activityId ?? 0).ToList();

            //给sheet1添加第一行的头部标题
            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
            row1.CreateCell(0).SetCellValue("编号");
            row1.CreateCell(1).SetCellValue("活动编号");

            row1.CreateCell(2).SetCellValue("用户编号");
            row1.CreateCell(3).SetCellValue("姓名");
            row1.CreateCell(4).SetCellValue("电话号码");
            row1.CreateCell(5).SetCellValue("省");

            row1.CreateCell(6).SetCellValue("市");
            row1.CreateCell(7).SetCellValue("县区");
            row1.CreateCell(8).SetCellValue("详细地址");
            row1.CreateCell(9).SetCellValue("Email");
            row1.CreateCell(10).SetCellValue("设备类型");
            row1.CreateCell(11).SetCellValue("答案1");
            row1.CreateCell(12).SetCellValue("答案2");
            row1.CreateCell(13).SetCellValue("答案3");
            row1.CreateCell(14).SetCellValue("创建时间");

            //将数据逐步写入sheet1各个行
            for (int i = 0; i < joininfos.Count(); i++)
            {
                NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);
                rowtemp.CreateCell(0).SetCellValue(joininfos[i].Id.ToString());
                rowtemp.CreateCell(1).SetCellValue(joininfos[i].ActivityId.ToString());

                rowtemp.CreateCell(2).SetCellValue(joininfos[i].UserId);
                rowtemp.CreateCell(3).SetCellValue(joininfos[i].Name);
                rowtemp.CreateCell(4).SetCellValue(joininfos[i].Tel);
                rowtemp.CreateCell(5).SetCellValue(joininfos[i].Province);

                rowtemp.CreateCell(6).SetCellValue(joininfos[i].City);
                rowtemp.CreateCell(7).SetCellValue(joininfos[i].Area);
                rowtemp.CreateCell(8).SetCellValue(joininfos[i].Address);
                rowtemp.CreateCell(9).SetCellValue(joininfos[i].Email);
                rowtemp.CreateCell(10).SetCellValue(joininfos[i].DeviceType);
                rowtemp.CreateCell(11).SetCellValue(joininfos[i].Results1);
                rowtemp.CreateCell(12).SetCellValue(joininfos[i].Results2);
                rowtemp.CreateCell(13).SetCellValue(joininfos[i].Results3);
                rowtemp.CreateCell(14).SetCellValue(joininfos[i].CreateDate.ToString());
            }

            // 写入到客户端 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", "JoinActivity.xls");
        }

        #endregion

        //[HttpPost]
        //public JsonResult ImportWinningInfo(string path)
        //{
        //    //HttpPostedFileBase file = Request.Files["file"];
        //    //var activityId = Request.Form["qId"];

        //    //return Json(new { issuccess = true, JsonRequestBehavior.AllowGet });

        //    var result = new ReturnResult() { IsSuccess = true };

        //    result = _AppContext.WinningInfoApp.ImportWinningInfoData(path);

        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}

        public FileResult ShareRecordExport(int? activityId)
        {
            //创建Excel文件的对象
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();

            //添加一个sheet
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("分享列表");

            List<ShareRecord> srs = new List<ShareRecord>();

            //获取list数据
            srs = _AppContext.ShareRecordApp.GetShareRecordsByActivity(activityId ?? 0).ToList();

            //给sheet1添加第一行的头部标题
            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
            row1.CreateCell(0).SetCellValue("编号");
            row1.CreateCell(1).SetCellValue("活动编号");
            row1.CreateCell(2).SetCellValue("用户编号");
            row1.CreateCell(3).SetCellValue("分享来源");
            row1.CreateCell(4).SetCellValue("分享到");
            row1.CreateCell(5).SetCellValue("分享时间");


            //将数据逐步写入sheet1各个行
            for (int i = 0; i < srs.Count(); i++)
            {
                NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);
                rowtemp.CreateCell(0).SetCellValue(srs[i].Id.ToString());
                rowtemp.CreateCell(1).SetCellValue(srs[i].ActivityId.ToString());

                rowtemp.CreateCell(2).SetCellValue(srs[i].UserId);
                rowtemp.CreateCell(3).SetCellValue(srs[i].Source);
                rowtemp.CreateCell(4).SetCellValue(srs[i].ShareType);
                rowtemp.CreateCell(5).SetCellValue(srs[i].CreateTime.ToString("yyyy-MM-dd hh:mm:ss"));
            }

            // 写入到客户端 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", "ShareRecord.xls");
        }
    }
}