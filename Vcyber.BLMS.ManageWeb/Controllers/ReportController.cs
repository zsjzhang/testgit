using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using Vcyber.BLMS.Application;
using System.IO;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.Entity.SelectCondition;
using Vcyber.BLMS.Entity.Generated;
using Vcyber.BLMS.ManageWeb.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

using System.Net;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using System.Text;

namespace Vcyber.BLMS.ManageWeb.Controllers
{
    //[MvcAuthorize]
    public class ReportController : Controller
    {
        readonly int EXCEL03_MaxRow = 65535;

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


        //[MvcAuthorize]
        //
        // GET: /Report/
        public ActionResult Index()
        {
            var cookieValue = CookieHelper.GetCookieValue("CustomCookie");
            if (!string.IsNullOrWhiteSpace(cookieValue) && cookieValue.ToLower().Contains("webinspect"))
            {
                return Redirect("/Content/error.htm");
            }

            //var table = _AppContext.ReportApp.GetReport(startTime ?? DateTime.Now.AddDays(-1), endTime ?? DateTime.Now, type?? 1);
            return View();
        }

        public ActionResult PartialPage(DateTime? startTime, DateTime? endTime, string tableName)
        {
            var table = _AppContext.ReportApp.GetReport(startTime ?? DateTime.Now.AddDays(-1), endTime ?? DateTime.Now, tableName);

            return PartialView(table);
        }

        #region  常用下拉选项数据获取
        /// <summary>
        /// 获取所有车型号
        /// </summary>
        /// <returns></returns>
        public string GetCarCategoryList()
        {
            string tableName = "GetCarCategoryList";
            DataTable table = _AppContext.ReportApp.GetReport(tableName);

            return DataTable2Json(table);
        }
        /// <summary>
        /// 获取所有客户类型：公司客户，个人客户
        /// </summary>
        /// <returns></returns>
        public string GetAccntTypeList()
        {
            string tableName = "GetAccntTypeList";
            DataTable table = _AppContext.ReportApp.GetReport(tableName);
            return DataTable2Json(table);
        }
        /// <summary>
        /// 获取 ReMeal的套餐 名称
        /// </summary>
        /// <returns></returns>
        public string GetReMealCardTypeList()
        {
            string tableName = "GetReMealCardTypeList";
            DataTable table = _AppContext.ReportApp.GetReport(tableName);
            return DataTable2Json(table);
        }


        #region  按车型、区域统计会员入会数量
        /// <summary>
        /// 获取所有活动信息
        /// </summary>
        /// <returns></returns>
        public string GetActivityInfoList()
        {
            string tableName = "GetActivityInfoList";
            DataTable table = _AppContext.ReportApp.GetReport(tableName);
            return DataTable2Json(table);
        }

        #endregion

        #region  按车型、区域统计会员入会数量--------[表格]
        /// <summary>
        /// 报表管理--按车型、区域统计会员入会数量
        /// </summary>
        /// <returns></returns>
        public ActionResult CommonIndex()
        {
            var cookieValue = CookieHelper.GetCookieValue("CustomCookie");
            if (!string.IsNullOrWhiteSpace(cookieValue) && cookieValue.ToLower().Contains("webinspect"))
            {
                return Redirect("/Content/error.htm");
            }

            return View();
        }
        /// <summary>
        /// 报表管理--按车型、区域统计会员入会数量【导出】
        /// </summary>
        /// <param name="CreateTimeStart">入会起始时间</param>
        /// <param name="CreateTimeEnd">入会截至时间</param>
        /// <param name="AuthenticationTimeStart">会员认证起始时间</param>
        /// <param name="AuthenticationTimeEnd">会员认证截至时间</param>
        /// <param name="BuyTimeStart">购车起始时间</param>
        /// <param name="BuyTimeEnd">购车截至时间</param>
        /// <param name="CarCategory">车型</param>
        /// <returns></returns>
        public ActionResult CommonIndexExport(DateTime? CreateTimeStart, DateTime? CreateTimeEnd, DateTime? AuthenticationTimeStart, DateTime? AuthenticationTimeEnd, DateTime? BuyTimeStart, DateTime? BuyTimeEnd, string CarCategory, string AccntType)
        {
            string tableName = "GetCommonGroupByRegion";
            if (string.IsNullOrEmpty(CarCategory))
            {
                CarCategory = null;
            }
            if (string.IsNullOrEmpty(AccntType))
            {
                AccntType = null;
            }
            IWorkbook book = new HSSFWorkbook();

            //获取list数据
            DataTable dt = _AppContext.ReportApp.GetReport(CreateTimeStart, CreateTimeEnd, AuthenticationTimeStart, AuthenticationTimeEnd, BuyTimeStart, BuyTimeEnd, CarCategory, AccntType, tableName);

            if (dt.Rows.Count < EXCEL03_MaxRow)
                DataWrite2Sheet(dt, 0, dt.Rows.Count - 1, book, "Sheet1");
            else
            {
                int page = (int)Math.Ceiling((double)dt.Rows.Count / EXCEL03_MaxRow);
                for (int i = 0; i < page; i++)
                {
                    int start = i * EXCEL03_MaxRow;
                    int end = (i * EXCEL03_MaxRow) + EXCEL03_MaxRow - 1;
                    DataWrite2Sheet(dt, start, end, book, "Sheet" + i.ToString());
                }
                int lastPageItemCount = dt.Rows.Count % EXCEL03_MaxRow;
                DataWrite2Sheet(dt, dt.Rows.Count - lastPageItemCount, dt.Rows.Count - 1, book, "Sheet" + page.ToString());
            }

            // 写入到客户端 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", tableName + ".xls");
        }




        public static DataTable CommonPartialPageTable;
        /// <summary>
        /// 报表管理--按车型、区域统计会员入会数量【数据展示页面】
        /// </summary>
        /// <param name="CreateTimeStart"></param>
        /// <param name="CreateTimeEnd"></param>
        /// <param name="AuthenticationTimeStart"></param>
        /// <param name="AuthenticationTimeEnd"></param>
        /// <param name="BuyTimeStart"></param>
        /// <param name="BuyTimeEnd"></param>
        /// <param name="CarCategory"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult CommonPartialPage(DateTime? CreateTimeStart, DateTime? CreateTimeEnd, DateTime? AuthenticationTimeStart, DateTime? AuthenticationTimeEnd, DateTime? BuyTimeStart, DateTime? BuyTimeEnd, string CarCategory, string AccntType, int pageIndex, int pageSize)
        {
            string tableName = "GetCommonGroupByRegion";
            if (string.IsNullOrEmpty(CarCategory))
            {
                CarCategory = null;
            }
            if (string.IsNullOrEmpty(AccntType))
            {
                AccntType = null;
            }
            var table = _AppContext.ReportApp.GetReport(CreateTimeStart, CreateTimeEnd, AuthenticationTimeStart, AuthenticationTimeEnd, BuyTimeStart, BuyTimeEnd, CarCategory, AccntType, tableName);
            pageIndex = pageIndex <= 0 ? 1 : pageIndex;
            pageSize = pageSize < 1 ? 10 : pageSize;
            int pageCount=0;
            if(table!=null)
            {
            pageCount =Convert.ToInt32( Math.Ceiling((double)table.Rows.Count / pageSize));
            }
            ViewBag.pageIndex = pageIndex;
            ViewBag.pageSize = pageSize;
            ViewBag.count = count;
            ViewBag.pageCount = pageCount;

            ViewBag.PrePage = pageIndex > 1 ? (pageIndex - 1) : 1;
            ViewBag.NextPage = pageIndex < pageCount ? (pageIndex + 1) : pageCount;

            ViewBag.EndPage = pageCount <= 0 ? 1 : pageCount;

            return PartialView(CommonPartialPageTable);
        }
        #endregion

        #region  差用数据格式转换以及筛选
        /// <summary>  
        /// dataTable转换成Json格式  
        /// </summary>  
        /// <param name="dt"></param>  
        /// <returns></returns>  
        public static string DataTable2Json(DataTable dt)
        {
            StringBuilder jsonBuilder = new StringBuilder();
            //jsonBuilder.Append("{\"");
            //jsonBuilder.Append(dt.TableName);
            //jsonBuilder.Append("\":[");

            jsonBuilder.Append("[");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                jsonBuilder.Append("{");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    jsonBuilder.Append("\"");
                    jsonBuilder.Append(dt.Columns[j].ColumnName);
                    jsonBuilder.Append("\":\"");
                    jsonBuilder.Append(dt.Rows[i][j].ToString());
                    jsonBuilder.Append("\",");
                }
                jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
                jsonBuilder.Append("},");
            }
            jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
            jsonBuilder.Append("]");
            //jsonBuilder.Append("}");
            return jsonBuilder.ToString();
        }
        /// <summary>
        /// 获取指定页数的数据
        /// </summary>
        /// <param name="page">数据索引页</param>
        /// <param name="pagesize">页大小</param>
        /// <param name="oDT">数据源</param>
        /// <returns></returns>
        public static DataTable GetDtPage(int page, int pagesize, DataTable oDT)
        {

            int startNumber = (page - 1) * pagesize;
            int endNumber = page * pagesize;
            if (oDT.Rows.Count < page * pagesize)
                endNumber = oDT.Rows.Count;

            DataTable NewTable = oDT.Clone();
            DataRow[] rows = oDT.Select("1=1");
            for (int i = startNumber; i < endNumber; i++)
            {
                NewTable.ImportRow((DataRow)rows[i]);
            }
            return NewTable;
        }
        #endregion

        #region  统计出库量--------[表格]
        /// <summary>
        /// 统计出库量
        /// </summary>
        /// <returns></returns>
        public ActionResult GetCarNumberByCarCategoryIndex()
        {
            var cookieValue = CookieHelper.GetCookieValue("CustomCookie");
            if (!string.IsNullOrWhiteSpace(cookieValue) && cookieValue.ToLower().Contains("webinspect"))
            {
                return Redirect("/Content/error.htm");
            }
            return View();
        }
        /// <summary>
        /// 分页展示 统计出库数量数据
        /// </summary>
        /// <param name="BuyTimeStart"></param>
        /// <param name="BuyTimeEnd"></param>
        /// <param name="CarCategory"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult GetCarNumberByCarCategoryPage(DateTime? BuyTimeStart, DateTime? BuyTimeEnd, string CarCategory, int pageIndex, int pageSize)
        {
            string tableName = "GetCarNumberByCarCategory";
            if (string.IsNullOrEmpty(CarCategory))
            {
                CarCategory = null;
            }
            var table = _AppContext.ReportApp.GetReport(BuyTimeStart, BuyTimeEnd, CarCategory, tableName);
            pageIndex = pageIndex <= 0 ? 1 : pageIndex;
            pageSize = pageSize < 1 ? 10 : pageSize;
            int pageCount = 0;
            DataTable GetCarNumberByCarCategoryPageTable = new DataTable();
            int count = 0;
            if (table != null)
            {
                pageCount = Convert.ToInt32(Math.Ceiling((double)table.Rows.Count / pageSize));

                count = table.Rows.Count;
                GetCarNumberByCarCategoryPageTable = GetDtPage(pageIndex, pageSize, table);
            }
            ViewBag.pageIndex = pageIndex;
            ViewBag.pageSize = pageSize;
            ViewBag.count = count;
            ViewBag.pageCount = pageCount;

            ViewBag.PrePage = pageIndex > 1 ? (pageIndex - 1) : 1;
            ViewBag.NextPage = pageIndex < pageCount ? (pageIndex + 1) : pageCount;
            ViewBag.EndPage = pageCount <= 0 ? 1 : pageCount;

            return PartialView(GetCarNumberByCarCategoryPageTable);
        }
        /// <summary>
        /// 统计出库数量数据【导出】
        /// </summary>
        /// <param name="BuyTimeStart"></param>
        /// <param name="BuyTimeEnd"></param>
        /// <param name="CarCategory"></param>
        /// <returns></returns>
        public ActionResult GetCarNumberByCarCategoryIndexExport(DateTime? BuyTimeStart, DateTime? BuyTimeEnd, string CarCategory)
        {
            string tableName = "GetCarNumberByCarCategory";
            if (string.IsNullOrEmpty(CarCategory))
            {
                CarCategory = null;
            }
            IWorkbook book = new HSSFWorkbook();

            //获取list数据
            DataTable dt = _AppContext.ReportApp.GetReport(BuyTimeStart, BuyTimeEnd, CarCategory, tableName);

            if (dt.Rows.Count < EXCEL03_MaxRow)
                DataWrite2Sheet(dt, 0, dt.Rows.Count - 1, book, "Sheet1");
            else
            {
                int page = (int)Math.Ceiling((double)dt.Rows.Count / EXCEL03_MaxRow);
                for (int i = 0; i < page; i++)
                {
                    int start = i * EXCEL03_MaxRow;
                    int end = (i * EXCEL03_MaxRow) + EXCEL03_MaxRow - 1;
                    DataWrite2Sheet(dt, start, end, book, "Sheet" + i.ToString());
                }
                int lastPageItemCount = dt.Rows.Count % EXCEL03_MaxRow;
                DataWrite2Sheet(dt, dt.Rows.Count - lastPageItemCount, dt.Rows.Count - 1, book, "Sheet" + page.ToString());
            }

            // 写入到客户端 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", tableName + ".xls");

        }
        #endregion

        #region 按照车型获取完成认证的会员数量-----[表格]
        /// <summary>
        /// 按照车型获取完成认证的会员数量
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAuthenticationSourceByCarCategory()
        {
            var cookieValue = CookieHelper.GetCookieValue("CustomCookie");
            if (!string.IsNullOrWhiteSpace(cookieValue) && cookieValue.ToLower().Contains("webinspect"))
            {
                return Redirect("/Content/error.htm");
            }
            return View();
        }

        /// <summary>
        /// 按照车型获取完成认证的会员数量------------展示
        /// </summary>
        /// <param name="AuthenticationTimeStart"></param>
        /// <param name="AuthenticationTimeEnd"></param>
        /// <param name="CarCategory"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult GetAuthenticationSourceByCarCategoryPage(DateTime? AuthenticationTimeStart, DateTime? AuthenticationTimeEnd, DateTime? BuyTimeStart, DateTime? BuyTimeEnd, string CarCategory, int pageIndex, int pageSize)
        {
            string tableName = "GetAuthenticationSourceByCarCategory";
            if (string.IsNullOrEmpty(CarCategory))
            {
                CarCategory = null;
            }
            var table = _AppContext.ReportApp.GetAuthenticationSourceByCarCategory(AuthenticationTimeStart, AuthenticationTimeEnd, BuyTimeStart, BuyTimeEnd, CarCategory, tableName);
            pageIndex = pageIndex <= 0 ? 1 : pageIndex;
            pageSize = pageSize < 1 ? 10 : pageSize;
            int pageCount = 0;
            int count = 0;
            if (table != null)
            {
                pageCount = Convert.ToInt32(Math.Ceiling((double)table.Rows.Count / pageSize));
                count = table.Rows.Count;
            }
            ViewBag.pageIndex = pageIndex;
            ViewBag.pageSize = pageSize;
            ViewBag.count = count;
            ViewBag.pageCount = pageCount;

            ViewBag.PrePage = pageIndex > 1 ? (pageIndex - 1) : 1;
            ViewBag.NextPage = pageIndex < pageCount ? (pageIndex + 1) : pageCount;

            ViewBag.EndPage = pageCount <= 0 ? 1 : pageCount;

            var GetCarNumberByCarCategoryPageTable = GetDtPage(pageIndex, pageSize, table);
            return PartialView(GetCarNumberByCarCategoryPageTable);
        }
        /// <summary>
        /// 按照车型获取完成认证的会员数量------[导出]
        /// </summary>
        /// <param name="AuthenticationTimeStart"></param>
        /// <param name="AuthenticationTimeEnd"></param>
        /// <param name="CarCategory"></param>
        /// <returns></returns>
        public ActionResult GetAuthenticationSourceByCarCategoryExport(DateTime? AuthenticationTimeStart, DateTime? AuthenticationTimeEnd, DateTime? BuyTimeStart, DateTime? BuyTimeEnd, string CarCategory)
        {
            string tableName = "GetAuthenticationSourceByCarCategory";
            if (string.IsNullOrEmpty(CarCategory))
            {
                CarCategory = null;
            }
            IWorkbook book = new HSSFWorkbook();

            //获取list数据
            DataTable dt = _AppContext.ReportApp.GetAuthenticationSourceByCarCategory(AuthenticationTimeStart, AuthenticationTimeEnd, BuyTimeStart, BuyTimeEnd, CarCategory, tableName);

            if (dt.Rows.Count < EXCEL03_MaxRow)
                DataWrite2Sheet(dt, 0, dt.Rows.Count - 1, book, "Sheet1");
            else
            {
                int page = Convert.ToInt32(Math.Ceiling((double)dt.Rows.Count / EXCEL03_MaxRow));
                for (int i = 0; i < page; i++)
                {
                    int start = i * EXCEL03_MaxRow;
                    int end = (i * EXCEL03_MaxRow) + EXCEL03_MaxRow - 1;
                    DataWrite2Sheet(dt, start, end, book, "Sheet" + i.ToString());
                }
                int lastPageItemCount = dt.Rows.Count % EXCEL03_MaxRow;
                DataWrite2Sheet(dt, dt.Rows.Count - lastPageItemCount, dt.Rows.Count - 1, book, "Sheet" + page.ToString());
            }

            // 写入到客户端 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", "车型渠道认证统计" + ".xls");

        }
        #endregion

        #region 车型积分下发统计-------[表格]
        /// <summary>
        /// 车型积分下发统计
        /// </summary>
        /// <returns></returns>
        public ActionResult GetUserCarIntegralRecordValueSum()
        {
            var cookieValue = CookieHelper.GetCookieValue("CustomCookie");
            if (!string.IsNullOrWhiteSpace(cookieValue) && cookieValue.ToLower().Contains("webinspect"))
            {
                return Redirect("/Content/error.htm");
            }
            return View();
        }
        /// <summary>
        /// 车型积分下发统计-------展示
        /// </summary>
        /// <param name="BuyTimeStart"></param>
        /// <param name="BuyTimeEnd"></param>
        /// <param name="CarCategory"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult GetUserCarIntegralRecordValueSumPage(DateTime? BuyTimeStart, DateTime? BuyTimeEnd, string CarCategory, int pageIndex, int pageSize)
        {
            string tableName = "GetUserCarIntegralRecordValueSum";
            if (string.IsNullOrEmpty(CarCategory))
            {
                CarCategory = null;
            }
            var table = _AppContext.ReportApp.GetUserCarIntegralRecordValueSum(BuyTimeStart, BuyTimeEnd, CarCategory, tableName);
            pageIndex = pageIndex <= 0 ? 1 : pageIndex;
            pageSize = pageSize < 1 ? 10 : pageSize;
            int pageCount = 0;
            int count = 0;
            if (table != null)
            {
                pageCount = Convert.ToInt32(Math.Ceiling((double)table.Rows.Count / pageSize));
                count = table.Rows.Count;
            }
            ViewBag.pageIndex = pageIndex;
            ViewBag.pageSize = pageSize;
            ViewBag.count = count;
            ViewBag.pageCount = pageCount;

            ViewBag.PrePage = pageIndex > 1 ? (pageIndex - 1) : 1;
            ViewBag.NextPage = pageIndex < pageCount ? (pageIndex + 1) : pageCount;

            ViewBag.EndPage = pageCount <= 0 ? 1 : pageCount;

            var GetCarNumberByCarCategoryPageTable = GetDtPage(pageIndex, pageSize, table);
            return PartialView(GetCarNumberByCarCategoryPageTable);
        }

        /// <summary>
        /// 车型积分下发统计---------[导出]
        /// </summary>
        /// <param name="BuyTimeStart"></param>
        /// <param name="BuyTimeEnd"></param>
        /// <param name="CarCategory"></param>
        /// <returns></returns>
        public ActionResult GetUserCarIntegralRecordValueSumExport(DateTime? BuyTimeStart, DateTime? BuyTimeEnd, string CarCategory)
        {
            string tableName = "GetUserCarIntegralRecordValueSum";
            if (string.IsNullOrEmpty(CarCategory))
            {
                CarCategory = null;
            }
            IWorkbook book = new HSSFWorkbook();

            //获取list数据
            DataTable dt = _AppContext.ReportApp.GetUserCarIntegralRecordValueSum(BuyTimeStart, BuyTimeEnd, CarCategory, tableName);

            if (dt.Rows.Count < EXCEL03_MaxRow)
                DataWrite2Sheet(dt, 0, dt.Rows.Count - 1, book, "Sheet1");
            else
            {
                int page = Convert.ToInt32(Math.Ceiling((double)dt.Rows.Count / EXCEL03_MaxRow));
                for (int i = 0; i < page; i++)
                {
                    int start = i * EXCEL03_MaxRow;
                    int end = (i * EXCEL03_MaxRow) + EXCEL03_MaxRow - 1;
                    DataWrite2Sheet(dt, start, end, book, "Sheet" + i.ToString());
                }
                int lastPageItemCount = dt.Rows.Count % EXCEL03_MaxRow;
                DataWrite2Sheet(dt, dt.Rows.Count - lastPageItemCount, dt.Rows.Count - 1, book, "Sheet" + page.ToString());
            }

            // 写入到客户端 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", "车型积分下发统计" + ".xls");

        }
        #endregion

        #region 按年月日统计积分增长量和消耗量
        /// <summary>
        /// 按年月日统计积分增长量和消耗量
        /// </summary>
        /// <returns></returns>
        public ActionResult GetUserIntegralRecordByTime()
        {
            var cookieValue = CookieHelper.GetCookieValue("CustomCookie");
            if (!string.IsNullOrWhiteSpace(cookieValue) && cookieValue.ToLower().Contains("webinspect"))
            {
                return Redirect("/Content/error.htm");
            }
            return View();
        }
        /// <summary>
        /// 按年月日统计积分增长量和消耗量
        /// </summary>
        /// <param name="CreateTimeStart">开始时间</param>
        /// <param name="CreateTimeEnd">结束时间</param>
        /// <param name="TimeType">查询维度，年、月、日</param>
        /// <returns></returns>
        public string GetUserIntegralRecordByTimePage(DateTime? CreateTimeStart, DateTime? CreateTimeEnd, string TimeType)
        {
            string tableName = "GetUserIntegralRecordByTime";
            string str = "";
            if (string.IsNullOrEmpty(TimeType))
            {
                TimeType = "m";
            }
            var table = _AppContext.ReportApp.GetCommonReport(CreateTimeStart, CreateTimeEnd, TimeType, tableName);
            if (table.Rows.Count > 0)
            {
                str = DataTable2Json(table);
            }
            return str;
        }
        #endregion

        #region  按年月日统计会员增长量
        /// <summary>
        /// 按年月日统计会员增长量
        /// </summary>
        /// <returns></returns>
        public ActionResult GetMembershipCountByTime()
        {
            var cookieValue = CookieHelper.GetCookieValue("CustomCookie");
            if (!string.IsNullOrWhiteSpace(cookieValue) && cookieValue.ToLower().Contains("webinspect"))
            {
                return Redirect("/Content/error.htm");
            }
            return View();
        }
        /// <summary>
        /// 按年月日统计会员增长量
        /// </summary>
        /// <param name="CreateTimeStart">会员入会开始时间</param>
        /// <param name="CreateTimeEnd">会员入会结束时间</param>
        /// <param name="TimeType">查询维度，年、月、日</param>
        /// <returns></returns>
        public string GetMembershipCountByTimePage(DateTime? CreateTimeStart, DateTime? CreateTimeEnd, string TimeType)
        {
            string tableName = "GetMembershipCountByTime";
            string str = "";
            if (string.IsNullOrEmpty(TimeType))
            {
                TimeType = "m";
            }
            var table = _AppContext.ReportApp.GetCommonReport(CreateTimeStart, CreateTimeEnd, TimeType, tableName);
            if (table.Rows.Count > 0)
            {
                str = DataTable2Json(table);
            }
            return str;
        }
        #endregion

        #region  按年月日统计返厂次数和积分消费及返还积分
        /// <summary>
        /// 按年月日统计返厂次数和积分消费及返还积分
        /// </summary>
        /// <returns></returns>
        public ActionResult GetCS_ConsumeCountByTime()
        {
            var cookieValue = CookieHelper.GetCookieValue("CustomCookie");
            if (!string.IsNullOrWhiteSpace(cookieValue) && cookieValue.ToLower().Contains("webinspect"))
            {
                return Redirect("/Content/error.htm");
            }
            return View();
        }
        /// <summary>
        /// 按年月日统计返厂次数和积分消费及返还积分
        /// </summary>
        /// <param name="CreateTimeStart">返厂开始时间</param>
        /// <param name="CreateTimeEnd">返厂结束时间</param>
        /// <param name="TimeType">查询维度，年、月、日</param>
        /// <param name="CarCategory">车型</param>
        /// <returns></returns>
        public string GetCS_ConsumeCountByTimePage(DateTime? CreateTimeStart, DateTime? CreateTimeEnd, string TimeType, string CarCategory)
        {
            string tableName = "GetCS_ConsumeCountByTime";
            string str = "";
            if (string.IsNullOrEmpty(TimeType))
            {
                TimeType = "m";
            }
            if (string.IsNullOrEmpty(CarCategory))
            {
                CarCategory = null;
            }
            var table = _AppContext.ReportApp.GetCommonReport(CreateTimeStart, CreateTimeEnd, TimeType, CarCategory, tableName);
            if (table.Rows.Count > 0)
            {
                str = DataTable2Json(table);
            }
            return str;
        }
        #endregion

        #region 按年月日统计[活动]的入会人数--------[表格]
        /// <summary>
        /// 按年月日统计[活动]的入会人数
        /// </summary>
        /// <returns></returns>
        public ActionResult GetMembershipCountByTimeAndActivity()
        {
            var cookieValue = CookieHelper.GetCookieValue("CustomCookie");
            if (!string.IsNullOrWhiteSpace(cookieValue) && cookieValue.ToLower().Contains("webinspect"))
            {
                return Redirect("/Content/error.htm");
            }
            return View();
        }
        /// <summary>
        /// 按年月日统计[活动]的入会人数[展示]
        /// </summary>
        /// <param name="CreateTimeStart">参加活动的开始时间</param>
        /// <param name="CreateTimeEnd">参加活动的结束时间</param>
        /// <param name="TimeType">查询维度，年月日</param>
        /// <param name="ActivityId">活动ID</param>
        /// <param name="tableName">存储过程名称</param>
        /// <returns></returns>
        public ActionResult GetMembershipCountByTimeAndActivityPage(DateTime? CreateTimeStart, DateTime? CreateTimeEnd, string TimeType, string ActivityId, int pageIndex, int pageSize)
        {
            string tableName = "GetMembershipCountByTimeAndActivity";
            if (string.IsNullOrEmpty(TimeType))
            {
                TimeType = "m";
            }
            if (string.IsNullOrEmpty(ActivityId))
            {
                ActivityId = null;
            }
            var table = _AppContext.ReportApp.GetMembershipCountByTimeAndActivity(CreateTimeStart, CreateTimeEnd, TimeType, ActivityId, tableName);
            pageIndex = pageIndex <= 0 ? 1 : pageIndex;
            pageSize = pageSize < 1 ? 10 : pageSize;
            int pageCount = 0;
            DataTable GetCarNumberByCarCategoryPageTable = new DataTable();
            int count = 0;
            if (table != null)
            {
                pageCount = Convert.ToInt32(Math.Ceiling((double)table.Rows.Count / pageSize));
                count = table.Rows.Count;
                GetCarNumberByCarCategoryPageTable = GetDtPage(pageIndex, pageSize, table);
            }
            ViewBag.pageIndex = pageIndex;
            ViewBag.pageSize = pageSize;
            ViewBag.count = count;
            ViewBag.pageCount = pageCount;

            ViewBag.PrePage = pageIndex > 1 ? (pageIndex - 1) : 1;
            ViewBag.NextPage = pageIndex < pageCount ? (pageIndex + 1) : pageCount;

            ViewBag.EndPage = pageCount <= 0 ? 1 : pageCount;
            return PartialView(GetCarNumberByCarCategoryPageTable);
        }
        /// <summary>
        /// 按年月日统计[活动]的入会人数[导出]
        /// </summary>
        /// <param name="CreateTimeStart">参加活动的开始时间</param>
        /// <param name="CreateTimeEnd">参加活动的结束时间</param>
        /// <param name="TimeType">查询维度，年月日</param>
        /// <param name="ActivityId">活动ID</param>
        /// <param name="tableName">存储过程名称</param>
        /// <returns></returns>
        public ActionResult GetMembershipCountByTimeAndActivityExport(DateTime? CreateTimeStart, DateTime? CreateTimeEnd, string TimeType, string ActivityId)
        {
            string tableName = "GetMembershipCountByTimeAndActivity";
            if (string.IsNullOrEmpty(TimeType))
            {
                TimeType = "m";
            }
            if (string.IsNullOrEmpty(ActivityId))
            {
                ActivityId = null;
            }
            IWorkbook book = new HSSFWorkbook();

            //获取list数据
            DataTable dt = _AppContext.ReportApp.GetMembershipCountByTimeAndActivity(CreateTimeStart, CreateTimeEnd, TimeType, ActivityId, tableName);

            if (dt.Rows.Count < EXCEL03_MaxRow)
                DataWrite2Sheet(dt, 0, dt.Rows.Count - 1, book, "Sheet1");
            else
            {
                int page = (int)Math.Ceiling((double)dt.Rows.Count / EXCEL03_MaxRow);
                for (int i = 0; i < page; i++)
                {
                    int start = i * EXCEL03_MaxRow;
                    int end = (i * EXCEL03_MaxRow) + EXCEL03_MaxRow - 1;
                    DataWrite2Sheet(dt, start, end, book, "Sheet" + i.ToString());
                }
                int lastPageItemCount = dt.Rows.Count % EXCEL03_MaxRow;
                DataWrite2Sheet(dt, dt.Rows.Count - lastPageItemCount, dt.Rows.Count - 1, book, "Sheet" + page.ToString());
            }

            // 写入到客户端 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", tableName + ".xls");

        }
        #endregion

        #region  根据活动查询发放积分--------[表格]
        /// <summary>
        /// 根据活动查询发放积分
        /// </summary>
        /// <returns></returns>
        public ActionResult GetUserIntegralRecordByTimeAndActivity()
        {
            var cookieValue = CookieHelper.GetCookieValue("CustomCookie");
            if (!string.IsNullOrWhiteSpace(cookieValue) && cookieValue.ToLower().Contains("webinspect"))
            {
                return Redirect("/Content/error.htm");
            }
            return View();
        }
        /// <summary>
        /// 根据活动查询发放积分
        /// </summary>
        /// <param name="CreateTimeStart">积分发放开始时间</param>
        /// <param name="CreateTimeEnd">积分发放结束时间</param>
        /// <param name="TimeType">查询维度，年月日</param>
        /// <param name="ActivityId">根据活动找出的 积分记录表 integralSource 的值</param>
        /// <param name="pageIndex">索引页</param>
        /// <param name="pageSize">页大小</param>
        /// <returns></returns>
        public ActionResult GetUserIntegralRecordByTimeAndActivityPage(DateTime? CreateTimeStart, DateTime? CreateTimeEnd, string TimeType, string ActivityId, int pageIndex, int pageSize)
        {
            string tableName = "GetUserIntegralRecordByTimeAndActivity";
            if (string.IsNullOrEmpty(TimeType))
            {
                TimeType = "m";
            }
            if (string.IsNullOrEmpty(ActivityId))
            {
                ActivityId = "28";  //默认车主认证活动
            }
            //由于参数个数和 【按年月日统计[活动]的入会人数】GetMembershipCountByTimeAndActivity 一致所以共用一个接口
            var table = _AppContext.ReportApp.GetMembershipCountByTimeAndActivity(CreateTimeStart, CreateTimeEnd, TimeType, ActivityId, tableName);
            pageIndex = pageIndex <= 0 ? 1 : pageIndex;
            pageSize = pageSize < 1 ? 10 : pageSize;
            int pageCount = 0;
            DataTable GetCarNumberByCarCategoryPageTable = new DataTable();
            int count = 0;
            if (table != null)
            {
                pageCount = Convert.ToInt32(Math.Ceiling((double)table.Rows.Count / pageSize));
                count = table.Rows.Count;
                GetCarNumberByCarCategoryPageTable = GetDtPage(pageIndex, pageSize, table);
            }
            ViewBag.pageIndex = pageIndex;
            ViewBag.pageSize = pageSize;
            ViewBag.count = count;
            ViewBag.pageCount = pageCount;

            ViewBag.PrePage = pageIndex > 1 ? (pageIndex - 1) : 1;
            ViewBag.NextPage = pageIndex < pageCount ? (pageIndex + 1) : pageCount;

            ViewBag.EndPage = pageCount <= 0 ? 1 : pageCount;
            return PartialView(GetCarNumberByCarCategoryPageTable);
        }
        /// <summary>
        /// 根据活动查询发放积分--------【导出】
        /// </summary>
        /// <param name="CreateTimeStart">积分发放开始时间</param>
        /// <param name="CreateTimeEnd">积分发放结束时间</param>
        /// <param name="TimeType">查询维度，年月日</param>
        /// <param name="ActivityId">根据活动找出的 积分记录表 integralSource 的值</param>
        /// <returns></returns>
        public ActionResult GetUserIntegralRecordByTimeAndActivityExport(DateTime? CreateTimeStart, DateTime? CreateTimeEnd, string TimeType, string ActivityId)
        {
            string tableName = "GetUserIntegralRecordByTimeAndActivity";
            if (string.IsNullOrEmpty(TimeType))
            {
                TimeType = "m";
            }
            if (string.IsNullOrEmpty(ActivityId))
            {
                ActivityId = "28";  //默认车主认证活动
            }
            IWorkbook book = new HSSFWorkbook();

            //获取list数据
            DataTable dt = _AppContext.ReportApp.GetMembershipCountByTimeAndActivity(CreateTimeStart, CreateTimeEnd, TimeType, ActivityId, tableName);

            if (dt.Rows.Count < EXCEL03_MaxRow)
                DataWrite2Sheet(dt, 0, dt.Rows.Count - 1, book, "Sheet1");
            else
            {
                int page = (int)Math.Ceiling((double)dt.Rows.Count / EXCEL03_MaxRow);
                for (int i = 0; i < page; i++)
                {
                    int start = i * EXCEL03_MaxRow;
                    int end = (i * EXCEL03_MaxRow) + EXCEL03_MaxRow - 1;
                    DataWrite2Sheet(dt, start, end, book, "Sheet" + i.ToString());
                }
                int lastPageItemCount = dt.Rows.Count % EXCEL03_MaxRow;
                DataWrite2Sheet(dt, dt.Rows.Count - lastPageItemCount, dt.Rows.Count - 1, book, "Sheet" + page.ToString());
            }

            // 写入到客户端 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", tableName + ".xls");

        }
        #endregion

        #region  根据活动查询奖品类的发放数量--------[表格]
        /// <summary>
        /// 根据活动查询奖品类的发放数量
        /// </summary>
        /// <returns></returns>
        public ActionResult GetWinningInfoTypeCountByTimeActivity()
        {
            var cookieValue = CookieHelper.GetCookieValue("CustomCookie");
            if (!string.IsNullOrWhiteSpace(cookieValue) && cookieValue.ToLower().Contains("webinspect"))
            {
                return Redirect("/Content/error.htm");
            }
            return View();
        }
        /// <summary>
        /// 根据活动查询奖品类的发放数量--展示
        /// </summary>
        /// <param name="CreateTimeStart">中奖开始时间</param>
        /// <param name="CreateTimeEnd">中奖结束时间</param>
        /// <param name="TimeType">查询维度，年月日</param>
        /// <param name="ActivityId">活动ID</param>
        /// <param name="pageIndex">索引页</param>
        /// <param name="pageSize">页大小</param>
        /// <returns></returns>
        public ActionResult GetWinningInfoTypeCountByTimeActivityPage(DateTime? CreateTimeStart, DateTime? CreateTimeEnd, string TimeType, string ActivityId, int pageIndex, int pageSize)
        {
            string tableName = "GetWinningInfoTypeCountByTimeActivity";
            if (string.IsNullOrEmpty(TimeType))
            {
                TimeType = "m";
            }
            if (string.IsNullOrEmpty(ActivityId))
            {
                ActivityId = null;  //默认车主认证活动
            }
            //由于参数个数和 【按年月日统计[活动]的入会人数】GetMembershipCountByTimeAndActivity 一致所以共用一个接口
            var table = _AppContext.ReportApp.GetMembershipCountByTimeAndActivity(CreateTimeStart, CreateTimeEnd, TimeType, ActivityId, tableName);
            pageIndex = pageIndex <= 0 ? 1 : pageIndex;
            pageSize = pageSize < 1 ? 10 : pageSize;
            int pageCount = 0;
            DataTable GetCarNumberByCarCategoryPageTable = new DataTable();
            int count = 0;
            if (table != null)
            {
                pageCount = Convert.ToInt32(Math.Ceiling((double)table.Rows.Count / pageSize));
                count = table.Rows.Count;
                GetCarNumberByCarCategoryPageTable = GetDtPage(pageIndex, pageSize, table);
            }
            ViewBag.pageIndex = pageIndex;
            ViewBag.pageSize = pageSize;
            ViewBag.count = count;
            ViewBag.pageCount = pageCount;

            ViewBag.PrePage = pageIndex > 1 ? (pageIndex - 1) : 1;
            ViewBag.NextPage = pageIndex < pageCount ? (pageIndex + 1) : pageCount;

            ViewBag.EndPage = pageCount <= 0 ? 1 : pageCount;
            return PartialView(GetCarNumberByCarCategoryPageTable);
        }
        /// <summary>
        /// 根据活动查询奖品类的发放数量--【导出】
        /// </summary>
        /// <param name="CreateTimeStart">中奖开始时间</param>
        /// <param name="CreateTimeEnd">中奖结束时间</param>
        /// <param name="TimeType">查询维度，年月日</param>
        /// <param name="ActivityId">活动ID</param>
        /// <returns></returns>
        public ActionResult GetWinningInfoTypeCountByTimeActivityExport(DateTime? CreateTimeStart, DateTime? CreateTimeEnd, string TimeType, string ActivityId)
        {
            string tableName = "GetWinningInfoTypeCountByTimeActivity";
            if (string.IsNullOrEmpty(TimeType))
            {
                TimeType = "m";
            }
            if (string.IsNullOrEmpty(ActivityId))
            {
                ActivityId = null;  //默认车主认证活动
            }
            IWorkbook book = new HSSFWorkbook();

            //获取list数据
            DataTable dt = _AppContext.ReportApp.GetMembershipCountByTimeAndActivity(CreateTimeStart, CreateTimeEnd, TimeType, ActivityId, tableName);

            if (dt.Rows.Count < EXCEL03_MaxRow)
                DataWrite2Sheet(dt, 0, dt.Rows.Count - 1, book, "Sheet1");
            else
            {
                int page = (int)Math.Ceiling((double)dt.Rows.Count / EXCEL03_MaxRow);
                for (int i = 0; i < page; i++)
                {
                    int start = i * EXCEL03_MaxRow;
                    int end = (i * EXCEL03_MaxRow) + EXCEL03_MaxRow - 1;
                    DataWrite2Sheet(dt, start, end, book, "Sheet" + i.ToString());
                }
                int lastPageItemCount = dt.Rows.Count % EXCEL03_MaxRow;
                DataWrite2Sheet(dt, dt.Rows.Count - lastPageItemCount, dt.Rows.Count - 1, book, "Sheet" + page.ToString());
            }

            // 写入到客户端 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", tableName + ".xls");

        }
        #endregion

        #region 查询活动中奖明细--------[表格]
        /// <summary>
        /// 查询活动中奖明细
        /// </summary>
        /// <returns></returns>
        public ActionResult GetWinningInfoDetailsByActivity()
        {
            var cookieValue = CookieHelper.GetCookieValue("CustomCookie");
            if (!string.IsNullOrWhiteSpace(cookieValue) && cookieValue.ToLower().Contains("webinspect"))
            {
                return Redirect("/Content/error.htm");
            }
            return View();
        }
        /// <summary>
        /// 查询活动中奖明细--分页展示数据
        /// </summary>
        /// <param name="CreateTimeStart">中奖开始时间</param>
        /// <param name="CreateTimeEnd">中奖结束时间</param>
        /// <param name="ActivityId">活动ID</param>
        /// <param name="pageIndex">索引页</param>
        /// <param name="pageSize">页大小</param>
        /// <returns></returns>
        public ActionResult GetWinningInfoDetailsByActivityPage(DateTime? CreateTimeStart, DateTime? CreateTimeEnd,  string ActivityId, int pageIndex, int pageSize)
        {
            string tableName = "GetWinningInfoDetailsByActivity";
        
            if (string.IsNullOrEmpty(ActivityId))
            {
                ActivityId = null;  //默认车主认证活动
            }
            //由于参数个数和 【按年月日统计[活动]的入会人数】GetMembershipCountByTimeAndActivity 一致所以共用一个接口
            var table = _AppContext.ReportApp.GetWinningInfoDetailsByActivity(CreateTimeStart, CreateTimeEnd, ActivityId, tableName);
            pageIndex = pageIndex <= 0 ? 1 : pageIndex;
            pageSize = pageSize < 1 ? 10 : pageSize;
            int pageCount = 0;
            DataTable GetCarNumberByCarCategoryPageTable = new DataTable();
            int count = 0;
            if (table != null)
            {
                pageCount = Convert.ToInt32(Math.Ceiling((double)table.Rows.Count / pageSize));
                count = table.Rows.Count;
                GetCarNumberByCarCategoryPageTable = GetDtPage(pageIndex, pageSize, table);
            }
            ViewBag.pageIndex = pageIndex;
            ViewBag.pageSize = pageSize;
            ViewBag.count = count;
            ViewBag.pageCount = pageCount;

            ViewBag.PrePage = pageIndex > 1 ? (pageIndex - 1) : 1;
            ViewBag.NextPage = pageIndex < pageCount ? (pageIndex + 1) : pageCount;

            ViewBag.EndPage = pageCount <= 0 ? 1 : pageCount;
            return PartialView(GetCarNumberByCarCategoryPageTable);
        }
        /// <summary>
        /// 查询活动中奖明细--【导出】
        /// </summary>
        /// <param name="CreateTimeStart">中奖开始时间</param>
        /// <param name="CreateTimeEnd">中奖结束时间</param>
        /// <param name="ActivityId">活动ID</param>
        /// <returns></returns>
        public ActionResult GetWinningInfoDetailsByActivityExport(DateTime? CreateTimeStart, DateTime? CreateTimeEnd,  string ActivityId)
        {
            string tableName = "GetWinningInfoDetailsByActivity";
           
            if (string.IsNullOrEmpty(ActivityId))
            {
                ActivityId = null;  //默认车主认证活动
            }
            IWorkbook book = new HSSFWorkbook();

            //获取list数据
            DataTable dt = _AppContext.ReportApp.GetWinningInfoDetailsByActivity(CreateTimeStart, CreateTimeEnd, ActivityId, tableName);

            if (dt.Rows.Count < EXCEL03_MaxRow)
                DataWrite2Sheet(dt, 0, dt.Rows.Count - 1, book, "Sheet1");
            else
            {
                int page = (int)Math.Ceiling((double)dt.Rows.Count / EXCEL03_MaxRow);
                for (int i = 0; i < page; i++)
                {
                    int start = i * EXCEL03_MaxRow;
                    int end = (i * EXCEL03_MaxRow) + EXCEL03_MaxRow - 1;
                    DataWrite2Sheet(dt, start, end, book, "Sheet" + i.ToString());
                }
                int lastPageItemCount = dt.Rows.Count % EXCEL03_MaxRow;
                DataWrite2Sheet(dt, dt.Rows.Count - lastPageItemCount, dt.Rows.Count - 1, book, "Sheet" + page.ToString());
            }

            // 写入到客户端 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", tableName + ".xls");

        }
        #endregion



        /// <summary>
        /// 会员创建渠道管理
        /// </summary>
        /// <returns></returns>
        public ActionResult GetCreatedPerson()
        {
            var cookieValue = CookieHelper.GetCookieValue("CustomCookie");
            if (!string.IsNullOrWhiteSpace(cookieValue) && cookieValue.ToLower().Contains("webinspect"))
            {
                return Redirect("/Content/error.htm");
            }
            return View();
        }
        /// <summary>
        /// 会员创建渠道管理-------数据展示
        /// </summary>
        /// <param name="qType">创建渠道归属状态, 0=未分配，1=已分配，2=所有</param>
        /// <param name="CreatedPerson">创建渠道</param>
        /// <param name="pageIndex">索引页</param>
        /// <param name="pageSize">页大小</param>
        /// <returns></returns>
        public ActionResult GetCreatedPersonPage(string qType, string CreatedPerson, int pageIndex, int pageSize)
        {
            string tableName = "GetCreatedPerson";
            if (string.IsNullOrEmpty(qType))
            {
                qType = "2";  //创建渠道归属状态, 0=未分配，1=已分配，2=所有
            }
            if (string.IsNullOrEmpty(CreatedPerson))
            {
                CreatedPerson = null;  //创建渠道
            }
            
            var table = _AppContext.ReportApp.GetCreatedPerson(qType, CreatedPerson, tableName);
            pageIndex = pageIndex <= 0 ? 1 : pageIndex;
            pageSize = pageSize < 1 ? 10 : pageSize;
            int pageCount = 0;
            DataTable GetCarNumberByCarCategoryPageTable = new DataTable();
            int count = 0;
            if (table != null)
            {
                pageCount = Convert.ToInt32(Math.Ceiling((double)table.Rows.Count / pageSize));
                count = table.Rows.Count;
                GetCarNumberByCarCategoryPageTable = GetDtPage(pageIndex, pageSize, table);
            }
            ViewBag.pageIndex = pageIndex;
            ViewBag.pageSize = pageSize;
            ViewBag.count = count;
            ViewBag.pageCount = pageCount;

            ViewBag.PrePage = pageIndex > 1 ? (pageIndex - 1) : 1;
            ViewBag.NextPage = pageIndex < pageCount ? (pageIndex + 1) : pageCount;

            ViewBag.EndPage = pageCount <= 0 ? 1 : pageCount;
            return PartialView(GetCarNumberByCarCategoryPageTable);
        }
        /// <summary>
        /// 会员创建渠道管理------【导出】
        /// </summary>
        /// <param name="qType">创建渠道归属状态, 0=未分配，1=已分配，2=所有</param>
        /// <param name="CreatedPerson">创建渠道</param>
        /// <returns></returns>
        public ActionResult GetCreatedPersonExport(string qType, string CreatedPerson)
        {
            string tableName = "GetCreatedPerson";

            if (string.IsNullOrEmpty(qType))
            {
                qType = "2";  //创建渠道归属状态, 0=未分配，1=已分配，2=所有
            }
            if (string.IsNullOrEmpty(CreatedPerson))
            {
                CreatedPerson = null;  //创建渠道
            }
            IWorkbook book = new HSSFWorkbook();

            //获取list数据
            DataTable dt = _AppContext.ReportApp.GetCreatedPerson(qType, CreatedPerson, tableName);

            if (dt.Rows.Count < EXCEL03_MaxRow)
                DataWrite2Sheet(dt, 0, dt.Rows.Count - 1, book, "Sheet1");
            else
            {
                int page = (int)Math.Ceiling((double)dt.Rows.Count / EXCEL03_MaxRow);
                for (int i = 0; i < page; i++)
                {
                    int start = i * EXCEL03_MaxRow;
                    int end = (i * EXCEL03_MaxRow) + EXCEL03_MaxRow - 1;
                    DataWrite2Sheet(dt, start, end, book, "Sheet" + i.ToString());
                }
                int lastPageItemCount = dt.Rows.Count % EXCEL03_MaxRow;
                DataWrite2Sheet(dt, dt.Rows.Count - lastPageItemCount, dt.Rows.Count - 1, book, "Sheet" + page.ToString());
            }

            // 写入到客户端 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", tableName + ".xls");

        }

        public int SaveCreatedType(string qType, string CreatedPerson)
        {
            string tableName = "SaveCreatedType";
            int flg = -9;
            if (string.IsNullOrEmpty(qType))
            {
                flg = -1;  
                return flg;
            }
            if (string.IsNullOrEmpty(CreatedPerson))
            {
                flg = -2;  
                return flg;
            }
            flg= _AppContext.ReportApp.SaveCreatedType(qType, CreatedPerson, tableName);

           
            return flg;

        }

        public ActionResult PartialPageForActivity(int? activityId, string tableName)
        {
            var table = _AppContext.ReportApp.GetReport(activityId ?? 0, tableName);

            return PartialView(table);
        }

        public ActionResult PartialPageForActivity1(string tableName)
        {
            var table = _AppContext.ReportApp.GetReport(tableName);

            return PartialView(table);
        }

        //public ActionResult Export(DateTime? startTime, DateTime? endTime, string tableName)
        //{
        //    int cou = 0;

        //    //创建Excel文件的对象
        //    NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();

        //    //添加一个sheet
        //    NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");

        //    //获取list数据
        //    DataTable tables = _AppContext.ReportApp.GetReport(startTime ?? DateTime.Now.AddDays(-1), endTime ?? DateTime.Now, tableName);

        //    //给sheet1添加第一行的头部标题
        //    NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);

        //    for (int i = 0; i < tables.Columns.Count; i++)
        //    {
        //        row1.CreateCell(i).SetCellValue(tables.Columns[i].ColumnName);
        //    }


        //    //将数据逐步写入sheet1各个行
        //    for (int i = 0; i < tables.Rows.Count; i++)
        //    {
        //        NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);
        //        for (int j = 0; j < tables.Rows[i].ItemArray.Length; j++)
        //        {
        //            rowtemp.CreateCell(j).SetCellValue(tables.Rows[i].ItemArray[j].ToString());
        //        }
        //    }

        //    // 写入到客户端 
        //    System.IO.MemoryStream ms = new System.IO.MemoryStream();
        //    book.Write(ms);
        //    ms.Seek(0, SeekOrigin.Begin);
        //    return File(ms, "application/vnd.ms-excel", tableName + ".xls");
        //}

        public ActionResult Export(DateTime? startTime, DateTime? endTime, string tableName)
        {
            IWorkbook book = new HSSFWorkbook();

            //获取list数据
            DataTable dt = _AppContext.ReportApp.GetReport(startTime ?? DateTime.Now.AddDays(-1), endTime ?? DateTime.Now, tableName);

            if (dt.Rows.Count < EXCEL03_MaxRow)
                DataWrite2Sheet(dt, 0, dt.Rows.Count - 1, book, "Sheet1");
            else
            {
                int page = (int)Math.Ceiling((double)dt.Rows.Count / EXCEL03_MaxRow);
                for (int i = 0; i < page; i++)
                {
                    int start = i * EXCEL03_MaxRow;
                    int end = (i * EXCEL03_MaxRow) + EXCEL03_MaxRow - 1;
                    DataWrite2Sheet(dt, start, end, book, "Sheet" + i.ToString());
                }
                int lastPageItemCount = dt.Rows.Count % EXCEL03_MaxRow;
                DataWrite2Sheet(dt, dt.Rows.Count - lastPageItemCount, dt.Rows.Count - 1, book, "Sheet" + page.ToString());
            }

            // 写入到客户端 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", tableName + ".xls");
        }

        private void DataWrite2Sheet(DataTable dt, int startRow, int endRow, IWorkbook book, string sheetName)
        {
            ISheet sheet = book.CreateSheet(sheetName);
            IRow header = sheet.CreateRow(0);
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                ICell cell = header.CreateCell(i);
                string val = dt.Columns[i].Caption ?? dt.Columns[i].ColumnName;
                cell.SetCellValue(val);
            }
            int rowIndex = 1;
            for (int i = startRow; i <= endRow; i++)
            {
                DataRow dtRow = dt.Rows[i];
                IRow excelRow = sheet.CreateRow(rowIndex++);
                for (int j = 0; j < dtRow.ItemArray.Length; j++)
                {
                    excelRow.CreateCell(j).SetCellValue(dtRow[j].ToString());
                }
            }
        }

        public ActionResult ExportByActivityId(int? activityId, string tableName)
        {
            int cou = 0;

            //创建Excel文件的对象
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();

            //添加一个sheet
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");

            //获取list数据
            DataTable tables = _AppContext.ReportApp.GetReport(activityId ?? 0, tableName);

            //给sheet1添加第一行的头部标题
            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);

            for (int i = 0; i < tables.Columns.Count; i++)
            {
                row1.CreateCell(i).SetCellValue(tables.Columns[i].ColumnName);
            }


            //将数据逐步写入sheet1各个行
            for (int i = 0; i < tables.Rows.Count; i++)
            {
                NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);
                for (int j = 0; j < tables.Rows[i].ItemArray.Length; j++)
                {
                    rowtemp.CreateCell(j).SetCellValue(tables.Rows[i].ItemArray[j].ToString());
                }
            }

            // 写入到客户端 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", tableName + ".xls");
        }

        public ActionResult ExportByActivityReport(string tableName)
        {
            int cou = 0;

            //创建Excel文件的对象
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();

            //添加一个sheet
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");

            //获取list数据
            DataTable tables = _AppContext.ReportApp.GetReport(tableName);

            //给sheet1添加第一行的头部标题
            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);

            for (int i = 0; i < tables.Columns.Count; i++)
            {
                row1.CreateCell(i).SetCellValue(tables.Columns[i].ColumnName);
            }


            //将数据逐步写入sheet1各个行
            for (int i = 0; i < tables.Rows.Count; i++)
            {
                NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);
                for (int j = 0; j < tables.Rows[i].ItemArray.Length; j++)
                {
                    rowtemp.CreateCell(j).SetCellValue(tables.Rows[i].ItemArray[j].ToString());
                }
            }

            // 写入到客户端 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", tableName + ".xls");
        }

        /// <summary>
        /// 会员信息报表
        /// </summary>
        /// <returns></returns>
        public ActionResult MemberIndex()
        {
            return View();
        }

        /// <summary>
        /// 会员信息分页
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public ActionResult MemberPartialPage(MemberReportCondition condition)
        {
            if (FilterStr.IsFlag<MemberReportCondition>(condition))
            {
                return Redirect("/Content/error.htm");
            }

            int total = 0;
            var list = _AppContext.ReportApp.FindMemberList(condition, new PageData() { Index = condition.index, Size = condition.size }, out total);

            int count = (int)Math.Ceiling((double)total / (double)condition.size);
            ViewBag.PageIndex = condition.index;
            ViewBag.PrePage = condition.index > 1 ? (condition.index - 1) : 1;
            ViewBag.NextPage = condition.index < count ? (condition.index + 1) : count;
            //ViewBag.EndPage = count > 10 ? 10 : count;
            ViewBag.EndPage = count <= 0 ? 1 : count;
            ViewBag.Total = total;

            return PartialView(list);
        }

        public ActionResult QuestionnaireMemberIndex()
        {
            bool IsCs = _AppContext.QuestionnaireApp.IsCSManager(this.HttpContext.User.Identity.GetUserId(), "CS问卷管理");
            ViewBag.IsCs = IsCs;
            return View();
        }

        public ActionResult QuestionnaireMemberPartialPage(QuestionnaireVisitorCondition condition)
        {
            int total = 0;
            if (condition.QuestionnaireId == null)
            {
                condition.QuestionnaireId = 0;
            }

            if (FilterStr.IsFlag<QuestionnaireVisitorCondition>(condition))
            {
                return Redirect("/Content/error.htm");
            }

            var list = _AppContext.ReportApp.FindQuestionnaireVisitorList(condition, new PageData() { Index = condition.index, Size = condition.size }, out total);
            int count = (int)Math.Ceiling((double)total / (double)condition.size);
            ViewBag.PageIndex = condition.index;
            ViewBag.PrePage = condition.index > 1 ? condition.index - 1 : 1;
            ViewBag.NextPage = condition.index < count ? condition.index + 1 : count;
            ViewBag.EndPage = count <= 0 ? 1 : count;
            ViewBag.Total = total;

            return PartialView(list);
        }

        public ActionResult QuestionnarieDayExport(string day, string endDay, int qId)
        {
            bool IsCs = _AppContext.QuestionnaireApp.IsCSManager(this.HttpContext.User.Identity.GetUserId(), "CS问卷管理");
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            if (IsCs)
            {
                QuestionnarieDayReportInfoCS qdrInfoCs = _AppContext.ReportApp.FindQuestionnaireDayInfoCs(day, endDay, qId);
                QuestionnaireDayCsExport(qdrInfoCs, book);
            }
            else
            {
                QuestionnarieDayReportInfo qdrInfo = _AppContext.ReportApp.FindQuestionnaireDayInfo(day, endDay, qId);

                QuestionnaireDayExport(qdrInfo, book);
            }

            // 写入到客户端 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", "在线调研日报" + ".xls");
        }

        public ActionResult AnswerExport(int questionnaireId)
        {
            bool IsCs = _AppContext.QuestionnaireApp.IsCSManager(this.HttpContext.User.Identity.GetUserId(), "CS问卷管理");
            List<AnswerReportInfo> anList = new List<AnswerReportInfo>();
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            if (IsCs)
            {
                //anList = _AppContext.ReportApp.FindAnswerListCs(questionnaireId);
                //AnswerExportCs(anList, book, 1);

                List<AnswerReport> anListNew = new List<AnswerReport>();
                anListNew = _AppContext.ReportApp.FindAnswerListCSNew(questionnaireId).ToList();

                List<Question> questions = _AppContext.QuestionApp.GetQuestionByPId(questionnaireId).ToList();

                if (anListNew.Count() <= 50000)
                {
                    AnswerExportCSNew(questions, anListNew, anListNew.Take(50000).ToList(), book, 1);
                }
                else if (anListNew.Count() > 50000 && anListNew.Count() < 10000)
                {
                    AnswerExportCSNew(questions, anListNew, anListNew.Take(50000).Skip(50000).ToList(), book, 2);
                }
                else
                {
                    AnswerExportCSNew(questions, anListNew, anListNew.Take(50000).Skip(100000).ToList(), book, 3);
                }

                //List<AnswerReport> anListNew = new List<AnswerReport>();
                //anListNew = _AppContext.ReportApp.FindAnswerListCSNew(questionnaireId).ToList();

                //List<Question> questions = _AppContext.QuestionApp.GetQuestionByPId(questionnaireId).ToList();

                //List<string> members = _AppContext.ReportApp.FindAnswerListMember(questionnaireId).ToList();
                ////1.问题列表 2.参加用户列表 3.答案列表
                //AnswerExportCSNew1(questions, members, anListNew.ToList(), book, 1);
            }
            else
            {
                anList = _AppContext.ReportApp.FindAnswerList(questionnaireId);
                AnswerExport(anList, book, 1);
            }
            //List<AnswerReportInfo> anList = _AppContext.ReportApp.FindAnswerList(questionnaireId);


            // 写入到客户端 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", "结果统计" + ".xls");
        }

        /// <summary>
        /// 导出会员信息
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public ActionResult MemberExport(MemberReportCondition condition)
        {

            if (FilterStr.IsFlag<MemberReportCondition>(condition))
            {
                return Redirect("/Content/error.htm");
            }
            int total;
            IEnumerable<MemberReportInfo> datas = _AppContext.ReportApp.FindMemberList(condition, new PageData() { Index = 1, Size = 10000 }, out total);
            //创建Excel文件的对象
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();

            if (total > 0 && datas != null)
            {
                int exportPage = total % 10000 == 0 ? total / 10000 : total / 10000 + 1;
                exportPage = exportPage > 5 ? 3 : exportPage;

                for (int j = 1; j <= exportPage; )
                {
                    this.MemberExport(datas, book, j);
                    j++;
                    datas = _AppContext.ReportApp.FindMemberList(condition, new PageData() { Index = j, Size = 10000 }, out total);
                }
            }

            // 写入到客户端 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", "会员信息总表" + ".xls");
        }

        public ActionResult QuestionnaireMemberExport(QuestionnaireVisitorCondition condition)
        {
            int total;
            IEnumerable<QuestionnaireVisitor> datas = _AppContext.ReportApp.FindQuestionnaireVisitorList(condition, new PageData() { Index = 1, Size = 10000 }, out total);
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            if (total > 0 && datas != null)
            {
                int exportPage = total % 10000 == 0 ? total / 10000 : total / 10000 + 1;
                exportPage = exportPage > 5 ? 3 : exportPage;
                for (int j = 1; j <= exportPage; )
                {
                    this.QuestionnaireMemberExport(datas, book, j, condition.MemberLevel);
                    j++;
                    datas = _AppContext.ReportApp.FindQuestionnaireVisitorList(condition, new PageData() { Index = j, Size = 10000 }, out total);
                }
            }
            //写入客户端
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", "参与调查问卷会员信息表" + ".xls");
        }

        /// <summary>
        /// 会员积分报表
        /// </summary>
        /// <returns></returns>
        public ActionResult IntegralIndex()
        {
            return View();
        }

        /// <summary>
        /// 会员积分分页
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public ActionResult IntegralPartialPage(IntegralReportCondition condition)
        {
            int total = 0;
            var list = _AppContext.ReportApp.FindIntegralList(condition, new PageData() { Index = condition.index, Size = condition.size }, out total);

            int count = (int)Math.Ceiling((double)total / (double)condition.size);
            ViewBag.PageIndex = condition.index;
            ViewBag.PrePage = condition.index > 1 ? (condition.index - 1) : 1;
            ViewBag.NextPage = condition.index < count ? (condition.index + 1) : count;
            //ViewBag.EndPage = count > 10 ? 10 : count;
            ViewBag.EndPage = count <= 0 ? 1 : count;
            ViewBag.Total = total;

            return PartialView(list);
        }

        /// <summary>
        /// 导出积分数据
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public ActionResult IntegralExport(IntegralReportCondition condition)
        {
            if (FilterStr.IsFlag<IntegralReportCondition>(condition))
            {
                return Redirect("/Content/error.htm");
            }
            int cou = 0;

            //创建Excel文件的对象
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            int total;
            //获取list数据
            IEnumerable<IntegralReportInfo> datas = _AppContext.ReportApp.FindIntegralList(condition, new PageData() { Index = 1, Size = 10000 }, out total);

            if (total > 0 && datas != null)
            {
                int exportPage = total % 10000 == 0 ? total / 10000 : total / 10000 + 1;
                exportPage = exportPage > 5 ? 3 : exportPage;

                for (int j = 1; j <= exportPage; )
                {
                    this.IntegralExport(book, datas, j);
                    j++;
                    datas = _AppContext.ReportApp.FindIntegralList(condition, new PageData() { Index = j, Size = 10000 }, out total);
                }
            }

            // 写入到客户端 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", "积分管理分析" + ".xls");
        }

        /// <summary>
        /// 会员获取积分信息
        /// </summary>
        /// <returns></returns>
        public ActionResult IntegralInputIndex()
        {
            return View();
        }

        /// <summary>
        /// 会员获取积分信息分页
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public ActionResult IntegralInputPartialPage(IntegralInputReportCondition condition)
        {
            var cookieValue = CookieHelper.GetCookieValue("CustomCookie");
            if (!string.IsNullOrWhiteSpace(cookieValue) && cookieValue.ToLower().Contains("webinspect"))
            {
                return Redirect("/Content/error.htm");
            }
            int total = 0;
            var list = _AppContext.ReportApp.FindIntegralInputList(condition, new PageData() { Index = condition.index, Size = condition.size }, out total);

            int count = (int)Math.Ceiling((double)total / (double)condition.size);
            ViewBag.PageIndex = condition.index;
            ViewBag.PrePage = condition.index > 1 ? (condition.index - 1) : 1;
            ViewBag.NextPage = condition.index < count ? (condition.index + 1) : count;
            //ViewBag.EndPage = count > 10 ? 10 : count;
            ViewBag.EndPage = count <= 0 ? 1 : count;
            ViewBag.Total = total;

            return PartialView(list);
        }

        /// <summary>
        /// 导出会员获取积分信息
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public ActionResult IntegralInputExport(IntegralInputReportCondition condition)
        {
            int cou = 0;

            //创建Excel文件的对象
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            int total;
            //获取list数据
            IEnumerable<IntegralInputReportInfo> datas = _AppContext.ReportApp.FindIntegralInputList(condition, new PageData() { Index = 1, Size = 50000 }, out total);

            if (total > 0 && datas != null)
            {
                //int exportPage = total % 10000 == 0 ? total / 10000 : total / 10000 + 1;
                //exportPage = exportPage > 5 ? 3 : exportPage;

                //for (int j = 1; j <= exportPage; )
                //{
                this.IntegralInputExport(book, datas, 1);
                //    j++;
                //    datas = _AppContext.ReportApp.FindIntegralInputList(condition, new PageData() { Index = j, Size = 10000 }, out total);
                //}
            }

            // 写入到客户端 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", "积分获取分析" + ".xls");
        }

        /// <summary>
        /// 会员消耗积分
        /// </summary>
        /// <returns></returns>
        public ActionResult IntegralOutIndex()
        {
            return View();
        }

        /// <summary>
        /// 活动报表导出界面
        /// </summary>
        /// <returns></returns>
        public ActionResult ActivityReportIndex()
        {
            return View();
        }

        /// <summary>
        /// 会员消耗积分分页
        /// 报表管理 积分兑换
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public ActionResult IntegralOutPartialPage(IntegralOutReportCondition condition)
        {
            int total = 0;
            var list = _AppContext.ReportApp.FindIntegralOutList(condition, new PageData() { Index = condition.index, Size = condition.size }, out total);

            int count = (int)Math.Ceiling((double)total / (double)condition.size);
            ViewBag.PageIndex = condition.index;
            ViewBag.PrePage = condition.index > 1 ? (condition.index - 1) : 1;
            ViewBag.NextPage = condition.index < count ? (condition.index + 1) : count;
            //ViewBag.EndPage = count > 10 ? 10 : count;
            ViewBag.EndPage = count <= 0 ? 1 : count;
            ViewBag.Total = total;

            return PartialView(list);
        }

        /// <summary>
        /// 积分兑换 导出 数据
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public ActionResult IntegralOutExport(IntegralOutReportCondition condition)
        {
            int cou = 0;

            //创建Excel文件的对象
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            int total;
            //获取list数据
            IEnumerable<IntegralOutReportInfo> datas = _AppContext.ReportApp.FindIntegralOutList(condition, new PageData() { Index = 1, Size = 10000 }, out total);

            if (total > 0 && datas != null)
            {
                //    int exportPage = total % 10000 == 0 ? total / 10000 : total / 10000 + 1;
                //    exportPage = exportPage > 5 ? 3 : exportPage;

                //    for (int j = 1; j <= exportPage; )
                //    {
                this.IntegralOutExport(book, datas, 1);
                //    j++;
                //    datas = _AppContext.ReportApp.FindIntegralOutList(condition, new PageData() { Index = j, Size = 10000 }, out total);
                //}
            }

            // 写入到客户端 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", "积分兑换分析" + ".xls");
        }


        #region 特约店积分结算报表

        public ActionResult IntegralCountIndex()
        {
            var dealerId = UserManager.FindById(this.User.Identity.GetUserId()).DealerId;

            if (!string.IsNullOrEmpty(dealerId))
            {
                ViewBag.DealerId = dealerId;
                ViewBag.IsEnable = false;
            }
            else
            {
                ViewBag.DealerId = "";
                ViewBag.IsEnable = true;
            }

            return View();
        }

        /// <summary>
        /// 管理员获取特约店积分结算列表数据
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public ActionResult IntegralCountPartialPage(IntegralCountReportCondition condition)
        {
            var dealerId = UserManager.FindById(this.User.Identity.GetUserId()).DealerId;
            int total;
            int count;
            if (!string.IsNullOrEmpty(dealerId))
            {
                condition.DealerId = dealerId;
            }
            #region 管理员看到的积分结算报表
            var list = _AppContext.ReportApp.FindIntegralCountList(condition);

            total = list.Count();

            count = (int)Math.Ceiling((double)total / (double)condition.size);
            ViewBag.PageIndex = condition.index;
            ViewBag.PrePage = condition.index > 1 ? (condition.index - 1) : 1;
            ViewBag.NextPage = condition.index < count ? (condition.index + 1) : count;
            //ViewBag.EndPage = count > 10 ? 10 : count;
            ViewBag.EndPage = count <= 0 ? 1 : count;
            ViewBag.Total = total;

            string result = "";
            if (condition.SettlementState == 1)
            {
                result = "已确认";
            }
            else if (condition.SettlementState == 2)
            {
                result = "待确认";
            }
            else if (condition.SettlementState == 3)
            {
                result = "待复核";
            }
            ViewBag.whereStatus = result;

            if (list.Count() > 0)
                return PartialView(list.Skip((condition.index - 1) * condition.size).Take(condition.size).ToList<IntegralCountReportInfo>());

            return PartialView(list);
            #endregion
        }

        public FileResult IntegralCountExport(IntegralCountReportCondition condition)
        {
            int cou = 0;

            //创建Excel文件的对象
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();

            int total;

            var dealerId = UserManager.FindById(this.User.Identity.GetUserId()).DealerId;
            if (!string.IsNullOrEmpty(dealerId))
            {
                condition.DealerId = dealerId;
            }

            //获取list数据
            var datas = _AppContext.ReportApp.FindIntegralCountList(condition).ToList();

            total = datas.Count();

            if (total > 0 && datas != null)
            {
                int exportPage = total % 10000 == 0 ? total / 10000 : total / 10000 + 1;
                exportPage = exportPage > 5 ? 3 : exportPage;

                for (int j = 1; j <= exportPage; )
                {
                    this.IntegralCountExport(book, datas, j);
                    j++;
                    datas = _AppContext.ReportApp.FindIntegralCountList(condition).ToList();
                }
            }

            // 写入到客户端 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", "特约店积分结算报表.xls");
        }

        private void IntegralCountExport(NPOI.HSSF.UserModel.HSSFWorkbook book, IEnumerable<IntegralCountReportInfo> datas, int j)
        {
            //添加一个sheet
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet" + j);
            //给sheet1添加第一行的头部标题
            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);

            row1.CreateCell(0).SetCellValue("序号");
            row1.CreateCell(1).SetCellValue("区域");
            row1.CreateCell(2).SetCellValue("办事处");
            row1.CreateCell(3).SetCellValue("店代码");
            row1.CreateCell(4).SetCellValue("积分获取总量");
            row1.CreateCell(5).SetCellValue("积分兑换总量");
            row1.CreateCell(6).SetCellValue("积分兑换金额");
            row1.CreateCell(7).SetCellValue("已结算金额");
            row1.CreateCell(8).SetCellValue("未结算金额");
            row1.CreateCell(9).SetCellValue("统计期间");

            int i = 0;

            foreach (var item in datas)
            {
                NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);
                rowtemp.CreateCell(0).SetCellValue((i + 1).ToString());
                rowtemp.CreateCell(1).SetCellValue(this.ConvertString(item.Area));
                rowtemp.CreateCell(2).SetCellValue(this.ConvertString(item.Region));
                rowtemp.CreateCell(3).SetCellValue(this.ConvertString(item.DealerId));
                rowtemp.CreateCell(4).SetCellValue(item.RewardPoints);
                rowtemp.CreateCell(5).SetCellValue(item.ConsumePoints);
                rowtemp.CreateCell(6).SetCellValue(item.PointCost);
                rowtemp.CreateCell(7).SetCellValue(item.SettlementY);
                rowtemp.CreateCell(8).SetCellValue(item.SettlementN);
                rowtemp.CreateCell(9).SetCellValue(item.DateString);

                i++;
            }
        }
        /// <summary>
        /// 管理员确认结算
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="SettlementState"></param>
        /// <returns></returns>
        public ActionResult Settlement(string dealerId, string startDate, string endDate, int SettlementState, string consumeType)
        {
            ReturnResult result = new ReturnResult { IsSuccess = true };

            if (string.IsNullOrEmpty(startDate) || string.IsNullOrEmpty(endDate))
            {
                result.IsSuccess = false;
                result.Message = "请选择结算开始和结束日期";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            ViewBag.StartTime = startDate;
            ViewBag.EndTime = endDate;
            //验证是否为重复结算
            var isSettledDealer = _AppContext.ReportApp.validateData(dealerId, startDate, endDate);

            if (isSettledDealer == true)
            {
                result.IsSuccess = false;
                result.Message = "已结算过此数据";
                return Json(result, JsonRequestBehavior.AllowGet);

            }
            _AppContext.ConsumeApp.Settlement(dealerId, startDate, endDate, SettlementState, consumeType);

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region 特约店积分结算确认报表
        /// <summary>
        /// 特约店积分结算确认报表 页面
        /// </summary>
        /// <returns></returns>
        public ActionResult ConfirmSettleIntegralIndex()
        {
            var dealerId = UserManager.FindById(this.User.Identity.GetUserId()).DealerId;

            if (!string.IsNullOrEmpty(dealerId))
            {
                ViewBag.DealerId = dealerId;
                ViewBag.IsEnable = false;
            }
            else
            {
                ViewBag.DealerId = "";
                ViewBag.IsEnable = true;
            }


            return View();
        }

        /// <summary>
        /// 特约店积分结算核对 报表数据
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public ActionResult ConfirmSettleIntegralPartialPage(IntegralCountReportCondition condition)
        {
            var dealerId = UserManager.FindById(this.User.Identity.GetUserId()).DealerId;
            int total;
            int count;
            #region 经销商去SettleDealerPoint表查自己的积分结算确认报表  2016 7 21 日增加 本周此功能不上线
            if (!string.IsNullOrEmpty(dealerId))
            {
                condition.DealerId = dealerId;

                //经销商去SettleDealerPoint查询自己的结算积分
                var listDealer = _AppContext.ReportApp.FindDealerCountList(condition);

                total = listDealer.Count();

                count = (int)Math.Ceiling((double)total / (double)condition.size);
                ViewBag.DealerId = dealerId;
                ViewBag.PageIndex = condition.index;
                ViewBag.PrePage = condition.index > 1 ? (condition.index - 1) : 1;
                ViewBag.NextPage = condition.index < count ? (condition.index + 1) : count;
                //ViewBag.EndPage = count > 10 ? 10 : count;
                ViewBag.EndPage = count <= 0 ? 1 : count;
                ViewBag.Total = total;

                if (listDealer.Count() > 0)
                    return PartialView(listDealer.Skip((condition.index - 1) * condition.size).Take(condition.size).ToList<IntegralCountReportInfo>());

                return PartialView(listDealer);
            }
            #endregion
            //管理员去SettleDealerPoint查询所有的特约店积分结算确认（列表）
            var list = _AppContext.ReportApp.FindDealerCountList(condition);

            total = list.Count();

            count = (int)Math.Ceiling((double)total / (double)condition.size);
            ViewBag.DealerId = dealerId;
            ViewBag.PageIndex = condition.index;
            ViewBag.PrePage = condition.index > 1 ? (condition.index - 1) : 1;
            ViewBag.NextPage = condition.index < count ? (condition.index + 1) : count;
            //ViewBag.EndPage = count > 10 ? 10 : count;
            ViewBag.EndPage = count <= 0 ? 1 : count;
            ViewBag.Total = total;

            if (list.Count() > 0)
                return PartialView(list.Skip((condition.index - 1) * condition.size).Take(condition.size).ToList<IntegralCountReportInfo>());

            return PartialView(list);

        }
        /// <summary>
        /// 经销商积分结算 确认 或 申请复核
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="SettlementState"></param>
        /// <param name="consumeType"></param>
        /// <returns></returns>
        public ActionResult SettlementDealerIntegral(string dealerId, string startDate, string endDate, int SettlementState, string consumeType)
        {
            dealerId = UserManager.FindById(this.User.Identity.GetUserId()).DealerId;

            ReturnResult result = new ReturnResult { IsSuccess = true };

            if (string.IsNullOrEmpty(startDate) || string.IsNullOrEmpty(endDate))
            {
                result.IsSuccess = false;
                result.Message = "请选择结算开始和结束日期";
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            _AppContext.ReportApp.DealerSettlement(dealerId, startDate, endDate, SettlementState);

            return Json(result, JsonRequestBehavior.AllowGet);

        }
        /// <summary>
        /// 特约店积分结算核对 报表导出
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="consumeType"></param>
        /// <param name="area"></param>
        /// <param name="region"></param>
        /// <param name="settlementState"></param>
        /// <returns></returns>
        public FileResult ConfirmSettleIntegralExport(IntegralCountReportCondition condition)
        {

            //创建Excel文件的对象
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();

            int total;

            var dealerId = UserManager.FindById(this.User.Identity.GetUserId()).DealerId;
            if (!string.IsNullOrEmpty(dealerId))
            {
                condition.DealerId = dealerId;
            }

            //获取list数据
            var datas = _AppContext.ReportApp.FindDealerCountList(condition).ToList();

            total = datas.Count();

            if (total > 0 && datas != null)
            {
                int exportPage = total % 10000 == 0 ? total / 10000 : total / 10000 + 1;
                exportPage = exportPage > 5 ? 3 : exportPage;

                for (int j = 1; j <= exportPage; )
                {
                    this.ConfirmSettleIntegralToExport(book, datas, j);
                    j++;
                    datas = _AppContext.ReportApp.FindDealerCountList(condition).ToList();
                }
            }

            // 写入到客户端 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", "特约店积分结算确认报表.xls");

        }

        private void ConfirmSettleIntegralToExport(NPOI.HSSF.UserModel.HSSFWorkbook book, IEnumerable<IntegralCountReportInfo> datas, int j)
        {
            var dealerId = UserManager.FindById(this.User.Identity.GetUserId()).DealerId;
            //添加一个sheet
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet" + j);
            //给sheet1添加第一行的头部标题
            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);

            row1.CreateCell(0).SetCellValue("序号");
            row1.CreateCell(1).SetCellValue("区域");
            row1.CreateCell(2).SetCellValue("办事处");
            row1.CreateCell(3).SetCellValue("店代码");
            row1.CreateCell(4).SetCellValue("积分获取总量");
            row1.CreateCell(5).SetCellValue("积分兑换总量");
            row1.CreateCell(6).SetCellValue("积分兑换金额");
            row1.CreateCell(7).SetCellValue("已结算金额");
            row1.CreateCell(8).SetCellValue("未结算金额");
            row1.CreateCell(9).SetCellValue("统计期间");

            int i = 0;

            foreach (var item in datas)
            {
                NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);
                rowtemp.CreateCell(0).SetCellValue((i + 1).ToString());
                rowtemp.CreateCell(1).SetCellValue(this.ConvertString(item.Area));
                rowtemp.CreateCell(2).SetCellValue(this.ConvertString(item.Region));
                rowtemp.CreateCell(3).SetCellValue(this.ConvertString(item.DealerId));
                rowtemp.CreateCell(4).SetCellValue(item.RewardPoints);
                rowtemp.CreateCell(5).SetCellValue(item.ConsumePoints);
                rowtemp.CreateCell(6).SetCellValue(item.PointCost);
                if (!string.IsNullOrEmpty(dealerId))
                {
                    if (item.SettlementState != Vcyber.BLMS.Entity.SettlementState.Confirm)
                    {
                        rowtemp.CreateCell(7).SetCellValue(item.SettlementN);
                        rowtemp.CreateCell(8).SetCellValue(item.SettlementY);
                    }
                    else
                    {
                        rowtemp.CreateCell(7).SetCellValue(item.SettlementY);
                        rowtemp.CreateCell(8).SetCellValue(item.SettlementN);
                    }
                }
                else
                {
                    rowtemp.CreateCell(7).SetCellValue(item.SettlementY);
                    rowtemp.CreateCell(8).SetCellValue(item.SettlementN);
                }
                rowtemp.CreateCell(9).SetCellValue(item.DateString);

                i++;
            }
        }

        #endregion


        /// <summary>
        /// 中间表查询
        /// </summary>
        /// <returns></returns>
        public ActionResult IFIndex()
        {
            return View();
        }

        public ActionResult OldIFIndex()
        {
            return View();
        }

        /// <summary>
        /// 查询车辆信息
        /// </summary>
        /// <param name="vin"></param>
        /// <returns></returns>
        public ActionResult FindCar(string vin, string whatType)
        {
            IFCustomer cusData;
            List<Car> datas = new List<Car>();
            List<IFCustomer> cusDatas = new List<IFCustomer>();
            Car data = new Car();
            if (whatType == "1")
            {
                data = _AppContext.CarServiceUserApp.OldGetCarInfoByVIN(vin, out cusData);
            }
            else
            {
                data = _AppContext.CarServiceUserApp.GetCarInfoByVIN(vin, out cusData);
            }
            datas.Add(data == null ? new Car() : data);
            cusDatas.Add(cusData == null ? new IFCustomer() : cusData);

            int count = data != null ? 1 : 0;
            ViewBag.PageIndex = count;
            ViewBag.PrePage = count;
            ViewBag.NextPage = count;
            ViewBag.EndPage = count;
            ViewBag.Total = count;
            ViewBag.CusData = cusDatas;

            return PartialView(datas);
        }

        /// <summary>
        /// 查询客户信息
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public ActionResult FindCustomer(CustomerCondition condition)
        {
            List<Car> carDatas;
            IEnumerable<IFCustomer> datas = new List<IFCustomer>();

            if (FilterStr.IsFlag<CustomerCondition>(condition))
            {
                return Redirect("/Content/error.htm");
            }

            if (condition.whatType == "1")
            {
                datas = _AppContext.CarServiceUserApp.OldFindCustomer(condition, out carDatas);
            }
            else
            {
                datas = _AppContext.CarServiceUserApp.FindCustomer(condition, out carDatas);
            }
            int count = datas.Count();
            ViewBag.PageIndex = count;
            ViewBag.PrePage = count;
            ViewBag.NextPage = count;
            ViewBag.EndPage = count;
            ViewBag.Total = count;
            ViewBag.CarData = carDatas;

            return PartialView(datas);
        }

        /// <summary>
        /// 添加客户信息
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddCustomer(IFCustomerV data)
        {
            if (data == null || string.IsNullOrEmpty(data.IdentityNumber))
            {
                return Json(new ResponseMessage(HttpStatusCode.NotFound, "请填写客户信息。"));
            }

            string message;
            bool result = data.ValidateProperties(out message);

            if (result)
            {
                result = data.AddInfo(out message);
            }

            return Json(new ResponseMessage(result ? HttpStatusCode.OK : HttpStatusCode.NotFound, result ? "添加成功。" : message));
        }

        /// <summary>
        /// 导入客户信息
        /// </summary>
        /// <returns></returns>
        public ActionResult ImportCoustomer()
        {
            HttpPostedFileBase file = this.Request.Files["file"];

            if (file != null)
            {
                string fileName = file.FileName;

                if (!Path.GetExtension(fileName).ToUpper().Equals(".XLS") && !Path.GetExtension(fileName).ToUpper().Equals(".XLSX"))
                {
                    return View(new ResponseMessage(HttpStatusCode.NotFound, "不支持此文件格式。"));
                }

                string strPath = HttpContext.Server.MapPath("../UploadImg");
                if (!Directory.Exists(strPath))
                {
                    Directory.CreateDirectory(strPath);
                }
                string filePath = Path.Combine(strPath, Guid.NewGuid().ToString() + Path.GetExtension(fileName));
                file.SaveAs(filePath);

                int rowNumber = 1;
                List<string> errors = new List<string>(5);
                DataTable dt = Common.NPOIHelper<OrderShipping>.ReadExcel(filePath, System.IO.Path.GetExtension(filePath));
                OverWriteExcel writeExcel = new OverWriteExcel();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    string message;
                    IFCustomerV data = new IFCustomerV();

                    try
                    {
                        data.CustName = dr[0].ToString();
                        data.CustMobile = dr[1].ToString();
                        data.IdentityNumber = dr[2].ToString();
                        data.Gender = dr[3].ToString();
                        data.Email = dr[4].ToString();
                        data.City = dr[5].ToString();
                        data.Address = dr[6].ToString();
                        data.VIN = dr[7].ToString();

                        bool result = data.ValidateProperties(out message);

                        if (result)
                        {
                            result = data.AddInfo(out message);
                        }

                        writeExcel.Writer(data, result ? "成功" : "失败", message);
                    }
                    catch (Exception ex)
                    {
                        writeExcel.Writer(data, "失败", ex.Message);
                    }

                    rowNumber++;
                }

                // 写入到客户端 
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                writeExcel.Book.Write(ms);
                ms.Seek(0, SeekOrigin.Begin);
                return File(ms, "application/vnd.ms-excel", fileName);

                //return View(new ResponseMessage(HttpStatusCode.OK, "导入成功。"));
            }

            return View(new ResponseMessage(HttpStatusCode.NotFound, "请上传文件。"));
        }

        /// <summary>
        /// 下载客户信息模板
        /// </summary>
        /// <returns></returns>
        public ActionResult DownloadCustomerTemplet()
        {
            string rootPath = this.Server.MapPath("~");
            string filePath = Path.Combine(rootPath, "App_Data\\客户信息模板.xls");


            return File(filePath, "application/vnd.ms-excel");
        }
        #region 中间表编辑（IF_Customer）
        public ViewResult CustomerEdit(CustomerCondition condition)
        {
            return View();
        }
        //查询客户信息
        public ActionResult QueryCustomer(CustomerCondition condition)
        {
            IEnumerable<IFCustomer> datas = new List<IFCustomer>();
            datas = _AppContext.CarServiceUserApp.FindCustomer(condition);
            int count = datas.Count();
            ViewBag.PageIndex = count;
            ViewBag.PrePage = count;
            ViewBag.NextPage = count;
            ViewBag.EndPage = count;
            ViewBag.Total = count;

            return PartialView(datas);
        }
        //删除客户信息，删除信息存入excel保存
        public JsonResult DelCustomerByCustId(string custId)
        {
            bool flag = false;

            if (!string.IsNullOrEmpty(custId))
            {
                CustomerCondition condition = new CustomerCondition
                {
                    CustId = custId
                };
                DataTable exceldata = null;
                string fileName = "/AreaData/中间表删除记录.xls";
                using (ExcelHelper excelHelper = new ExcelHelper(Server.MapPath(fileName)))
                {
                    exceldata = excelHelper.ExcelToDataTable("删除数据", true);
                }

                IEnumerable<IFCustomer> datas = _AppContext.CarServiceUserApp.FindCustomer(condition);
                if (datas != null)
                {
                    foreach (var item in datas)
                    {
                        DataRow row = exceldata.NewRow();
                        row["CustId"] = item.CustId;
                        row["CustName"] = item.CustName;
                        row["CustMobile"] = item.CustMobile;
                        row["IdentityNumber"] = item.IdentityNumber;
                        row["Gender"] = item.Gender;
                        row["Email"] = item.Email;
                        row["Address"] = item.Address;
                        row["City"] = item.City;
                        row["Member"] = item.Member;
                        row["Agree"] = item.Agree;
                        row["AccntType"] = item.AccntType;
                        row["AddTime"] = item.AddTime;
                        row["UpdateTime"] = item.UpdateTime;
                        row["CustLevel"] = item.CustLevel;
                        exceldata.Rows.Add(row);
                    }
                }
                using (ExcelHelper newExcel = new ExcelHelper(Server.MapPath(fileName)))
                {
                    int num = newExcel.DataTableToExcel(exceldata, "删除数据", true);
                }

                flag = _AppContext.CarServiceUserApp.DelCustomer(condition);
            }
            return Json(flag);
        }
        #endregion
        #region ==== 私有方法 ====

        private string ConvertString(string value)
        {
            return string.IsNullOrEmpty(value) ? "" : value;
        }

        /// <summary>
        /// 导出会员获得积分信息
        /// </summary>
        /// <param name="book"></param>
        /// <param name="datas"></param>
        /// <param name="j"></param>
        private void IntegralOutExport(NPOI.HSSF.UserModel.HSSFWorkbook book, IEnumerable<IntegralOutReportInfo> datas, int j)
        {
            //添加一个sheet
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet" + j);
            //给sheet1添加第一行的头部标题
            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);

            row1.CreateCell(0).SetCellValue("序号");
            row1.CreateCell(1).SetCellValue("办事处");
            row1.CreateCell(2).SetCellValue("区域");
            row1.CreateCell(3).SetCellValue("店代码");
            row1.CreateCell(4).SetCellValue("会员卡号");
            row1.CreateCell(5).SetCellValue("会员名称");
            row1.CreateCell(6).SetCellValue("手机号");
            row1.CreateCell(7).SetCellValue("积分兑换方式");
            row1.CreateCell(8).SetCellValue("积分值");
            row1.CreateCell(9).SetCellValue("日期");

            int i = 0;

            foreach (var item in datas)
            {
                NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);
                rowtemp.CreateCell(0).SetCellValue(i.ToString());
                rowtemp.CreateCell(1).SetCellValue(this.ConvertString(item.Region));
                rowtemp.CreateCell(2).SetCellValue(this.ConvertString(item.Area));
                rowtemp.CreateCell(3).SetCellValue(this.ConvertString(item.DealerId));
                rowtemp.CreateCell(4).SetCellValue(this.ConvertString(item.No));
                rowtemp.CreateCell(5).SetCellValue(this.ConvertString(item.RealName));
                rowtemp.CreateCell(6).SetCellValue(this.ConvertString(item.PhoneNumber));
                rowtemp.CreateCell(7).SetCellValue(((EOrderMode)item.OrderMode).GetDiscribe());
                rowtemp.CreateCell(8).SetCellValue(item.IntegralValue);
                rowtemp.CreateCell(9).SetCellValue(item.CreateTime.ToString());

                i++;
            }
        }

        /// <summary>
        /// 导出会员获得积分信息
        /// </summary>
        /// <param name="book"></param>
        /// <param name="datas"></param>
        /// <param name="j"></param>
        private void IntegralInputExport(NPOI.HSSF.UserModel.HSSFWorkbook book, IEnumerable<IntegralInputReportInfo> datas, int j)
        {
            //添加一个sheet
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet" + j);
            //给sheet1添加第一行的头部标题
            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);

            row1.CreateCell(0).SetCellValue("序号");
            row1.CreateCell(1).SetCellValue("办事处");
            row1.CreateCell(2).SetCellValue("区域");
            row1.CreateCell(3).SetCellValue("店代码");
            row1.CreateCell(4).SetCellValue("会员卡号");
            row1.CreateCell(5).SetCellValue("会员名称");
            row1.CreateCell(6).SetCellValue("手机号");
            row1.CreateCell(7).SetCellValue("积分获取方式");
            row1.CreateCell(8).SetCellValue("积分值");
            row1.CreateCell(9).SetCellValue("日期");

            int i = 0;

            foreach (var item in datas)
            {
                int integralMode = 0;
                int.TryParse(item.integralSource, out integralMode);

                NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);
                rowtemp.CreateCell(0).SetCellValue(i.ToString());
                rowtemp.CreateCell(1).SetCellValue(this.ConvertString(item.Region));
                rowtemp.CreateCell(2).SetCellValue(this.ConvertString(item.Area));
                rowtemp.CreateCell(3).SetCellValue(this.ConvertString(item.DealerId));
                rowtemp.CreateCell(4).SetCellValue(this.ConvertString(item.No));
                rowtemp.CreateCell(5).SetCellValue(this.ConvertString(item.RealName));
                rowtemp.CreateCell(6).SetCellValue(this.ConvertString(item.PhoneNumber));
                rowtemp.CreateCell(7).SetCellValue(((EIRuleType)integralMode).GetDiscribe());
                rowtemp.CreateCell(8).SetCellValue(item.IntegralValue);
                rowtemp.CreateCell(9).SetCellValue(item.CreateTime.ToString());

                i++;
            }
        }

        /// <summary>
        /// 导出积分信息
        /// </summary>
        /// <param name="book"></param>
        /// <param name="datas"></param>
        /// <param name="j"></param>
        private void IntegralExport(NPOI.HSSF.UserModel.HSSFWorkbook book, IEnumerable<IntegralReportInfo> datas, int j)
        {
            //添加一个sheet
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet" + j);
            //给sheet1添加第一行的头部标题
            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);

            row1.CreateCell(0).SetCellValue("序号");
            row1.CreateCell(1).SetCellValue("区域");
            row1.CreateCell(2).SetCellValue("店代码");
            row1.CreateCell(3).SetCellValue("会员卡号");
            row1.CreateCell(4).SetCellValue("会员名称");
            row1.CreateCell(5).SetCellValue("手机号");
            row1.CreateCell(6).SetCellValue("首次购车获取");
            row1.CreateCell(7).SetCellValue("曾换购获取");
            row1.CreateCell(8).SetCellValue("维保获取");
            row1.CreateCell(9).SetCellValue("积分获取合计");
            row1.CreateCell(10).SetCellValue("维保兑换");
            row1.CreateCell(11).SetCellValue("机场服务兑换");
            row1.CreateCell(12).SetCellValue("礼品兑换");
            row1.CreateCell(13).SetCellValue("积分兑换合计");
            row1.CreateCell(14).SetCellValue("累计已失效积分");

            int i = 0;

            foreach (var item in datas)
            {
                NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);
                rowtemp.CreateCell(0).SetCellValue(i.ToString());
                rowtemp.CreateCell(1).SetCellValue(this.ConvertString(item.Area));
                rowtemp.CreateCell(2).SetCellValue(this.ConvertString(item.DealerId));
                rowtemp.CreateCell(3).SetCellValue(this.ConvertString(item.No));
                rowtemp.CreateCell(4).SetCellValue(this.ConvertString(item.RealName));
                rowtemp.CreateCell(5).SetCellValue(this.ConvertString(item.PhoneNumber));
                rowtemp.CreateCell(6).SetCellValue(item.HGCTotal);
                rowtemp.CreateCell(7).SetCellValue(item.HZHTotal);
                rowtemp.CreateCell(8).SetCellValue(item.HWBTotal);
                rowtemp.CreateCell(9).SetCellValue(item.HGCTotal + item.HZHTotal + item.HWBTotal);
                rowtemp.CreateCell(10).SetCellValue(item.XBYTotal + item.XWXTotal);
                rowtemp.CreateCell(12).SetCellValue(item.XJCTotal);
                rowtemp.CreateCell(12).SetCellValue(item.XLPTotal);
                rowtemp.CreateCell(13).SetCellValue(item.XBYTotal + item.XWXTotal + item.XJCTotal + item.XLPTotal);
                rowtemp.CreateCell(14).SetCellValue(item.SXTotal);

                i++;
            }
        }

        /// <summary>
        /// 导出会员信息
        /// </summary>
        /// <param name="datas">导出数据</param>
        /// <param name="book"></param>
        /// <param name="j">Excel页编号</param>
        private void MemberExport(IEnumerable<MemberReportInfo> datas, NPOI.HSSF.UserModel.HSSFWorkbook book, int j)
        {

            //添加一个sheet
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet" + j);

            //给sheet1添加第一行的头部标题
            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);

            row1.CreateCell(0).SetCellValue("序号");
            row1.CreateCell(1).SetCellValue("姓名");
            row1.CreateCell(2).SetCellValue("性别");
            row1.CreateCell(3).SetCellValue("用户名");
            row1.CreateCell(4).SetCellValue("年龄");
            row1.CreateCell(5).SetCellValue("会员等级");
            row1.CreateCell(6).SetCellValue("身份证号");
            row1.CreateCell(7).SetCellValue("手机号");
            row1.CreateCell(8).SetCellValue("车型");
            row1.CreateCell(9).SetCellValue("车架号");
            row1.CreateCell(10).SetCellValue("购车时间");
            row1.CreateCell(11).SetCellValue("购车店代码");
            row1.CreateCell(12).SetCellValue("购车店名称");
            row1.CreateCell(13).SetCellValue("序购车区域号");
            row1.CreateCell(14).SetCellValue("注册时间");
            row1.CreateCell(15).SetCellValue("成为会员时间");
            row1.CreateCell(16).SetCellValue("银卡申请状态");
            row1.CreateCell(17).SetCellValue("申请渠道");
            row1.CreateCell(18).SetCellValue("支付方式");
            row1.CreateCell(19).SetCellValue("缴费经销商");
            row1.CreateCell(20).SetCellValue("市");
            row1.CreateCell(21).SetCellValue("地址");
            row1.CreateCell(22).SetCellValue("邮箱");
            row1.CreateCell(23).SetCellValue("出生日期");
            row1.CreateCell(24).SetCellValue("兴趣爱好");

            int i = 0;

            foreach (var item in datas)
            {
                NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);
                rowtemp.CreateCell(0).SetCellValue(i.ToString());
                rowtemp.CreateCell(1).SetCellValue(this.ConvertString(item.RealName));
                rowtemp.CreateCell(2).SetCellValue(item.Gender == 1 ? "男" : "女");
                rowtemp.CreateCell(3).SetCellValue(this.ConvertString(item.NickName));
                rowtemp.CreateCell(4).SetCellValue(item.Age.ToString());
                rowtemp.CreateCell(5).SetCellValue(item.MLevelName.ToString());
                rowtemp.CreateCell(6).SetCellValue(this.ConvertString(item.IdentityNumber));
                rowtemp.CreateCell(7).SetCellValue(this.ConvertString(item.PhoneNumber));
                rowtemp.CreateCell(8).SetCellValue(this.ConvertString(item.CarCategory));
                rowtemp.CreateCell(9).SetCellValue(this.ConvertString(item.VIN));
                rowtemp.CreateCell(10).SetCellValue(item.BuyTime.ToString());
                rowtemp.CreateCell(11).SetCellValue(this.ConvertString(item.DealerId));
                rowtemp.CreateCell(12).SetCellValue(this.ConvertString(item.DealerName));
                rowtemp.CreateCell(13).SetCellValue(this.ConvertString(item.BuyingArea));
                rowtemp.CreateCell(14).SetCellValue(item.RegisterTime.ToString());
                rowtemp.CreateCell(15).SetCellValue(item.MemberTime.ToString());
                rowtemp.CreateCell(16).SetCellValue(item.YKStatusName.ToString());
                rowtemp.CreateCell(17).SetCellValue(item.SDataSourceName.ToString());
                rowtemp.CreateCell(18).SetCellValue(string.IsNullOrEmpty(item.PayNumber) ? "4S店支付" : "天猫支付".ToString());
                rowtemp.CreateCell(19).SetCellValue(this.ConvertString(item.PayDealerName));
                rowtemp.CreateCell(20).SetCellValue(this.ConvertString(item.City));
                rowtemp.CreateCell(21).SetCellValue(this.ConvertString(item.Address));
                rowtemp.CreateCell(22).SetCellValue(this.ConvertString(item.Email));
                rowtemp.CreateCell(23).SetCellValue(this.ConvertString(item.Birthday));
                rowtemp.CreateCell(24).SetCellValue(this.ConvertString(item.Interest));

                i++;
            }
        }

        private void QuestionnaireMemberExport(IEnumerable<QuestionnaireVisitor> datas, NPOI.HSSF.UserModel.HSSFWorkbook book, int j, int? MemberLevel)
        {
            //添加一个sheet
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet" + j);
            bool IsCs = _AppContext.QuestionnaireApp.IsCSManager(this.HttpContext.User.Identity.GetUserId(), "CS问卷管理"); // User.IsInRole("CS问卷管理");

            //给sheet1添加第一行的头部标题
            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);

            row1.CreateCell(0).SetCellValue("序号");
            row1.CreateCell(1).SetCellValue("姓名");
            row1.CreateCell(2).SetCellValue("性别");
            row1.CreateCell(3).SetCellValue("电话");
            row1.CreateCell(4).SetCellValue("省");
            row1.CreateCell(5).SetCellValue("市");
            row1.CreateCell(6).SetCellValue("县区");
            row1.CreateCell(7).SetCellValue("邮寄地址");
            row1.CreateCell(8).SetCellValue("年龄");
            row1.CreateCell(9).SetCellValue("学历");
            row1.CreateCell(10).SetCellValue("车型");
            row1.CreateCell(11).SetCellValue("问卷答案提交时间");
            row1.CreateCell(12).SetCellValue("车架号");
            row1.CreateCell(13).SetCellValue("身份证号");
            row1.CreateCell(14).SetCellValue("会员等级");
            row1.CreateCell(15).SetCellValue("来源");
            if (IsCs)
            {
                row1.CreateCell(16).SetCellValue("注册来源");
            }
            int i = 0;



            foreach (var item in datas)
            {
                NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);
                rowtemp.CreateCell(0).SetCellValue(i.ToString());
                rowtemp.CreateCell(1).SetCellValue(this.ConvertString(item.VName));
                rowtemp.CreateCell(2).SetCellValue(item.Sex == 1 ? "男" : "女");
                rowtemp.CreateCell(3).SetCellValue(item.PhoneNumber);
                rowtemp.CreateCell(4).SetCellValue(item.Provency);
                rowtemp.CreateCell(5).SetCellValue(item.City);
                rowtemp.CreateCell(6).SetCellValue(item.Area);
                rowtemp.CreateCell(7).SetCellValue(item.MailAddress);
                rowtemp.CreateCell(8).SetCellValue(item.Age);
                rowtemp.CreateCell(9).SetCellValue(item.Education);
                rowtemp.CreateCell(10).SetCellValue(item.CarModels);
                rowtemp.CreateCell(11).SetCellValue(item.CreateTime.ToString());
                //if (MemberLevel != null)
                //{
                rowtemp.CreateCell(12).SetCellValue(item.VIN);
                rowtemp.CreateCell(13).SetCellValue(item.IdentityNumber);
                rowtemp.CreateCell(14).SetCellValue(item.MLevel);
                //}
                rowtemp.CreateCell(15).SetCellValue(item.VSource);
                if (IsCs)
                {
                    rowtemp.CreateCell(16).SetCellValue(item.CreatedPerson == "cs_questionnaire" ? "是" : "否");
                }
                i++;
            }
        }

        private void QuestionnaireDayExport(QuestionnarieDayReportInfo data, HSSFWorkbook book)
        {
            ISheet sheet1 = book.CreateSheet("Sheet1");

            //给sheet1添加第一行的头部标题
            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
            row1.CreateCell(0).SetCellValue("索九会员");
            row1.CreateCell(1).SetCellValue("普通车主会员");
            row1.CreateCell(2).SetCellValue("非车主会员");
            row1.CreateCell(3).SetCellValue("游客");

            IRow rowtemp = sheet1.CreateRow(1);
            rowtemp.CreateCell(0).SetCellValue(data.SjCount.ToString());
            rowtemp.CreateCell(1).SetCellValue(data.PtCount.ToString());
            rowtemp.CreateCell(2).SetCellValue(data.FcCount.ToString());
            rowtemp.CreateCell(3).SetCellValue(data.VisitorCount.ToString());

        }

        private void QuestionnaireDayCsExport(QuestionnarieDayReportInfoCS data, HSSFWorkbook book)
        {
            ISheet sheet1 = book.CreateSheet("Sheet1");
            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
            row1.CreateCell(0).SetCellValue("索九会员");
            row1.CreateCell(1).SetCellValue("普通车主会员");
            row1.CreateCell(2).SetCellValue("新增索九会员");
            row1.CreateCell(3).SetCellValue("新增普通车主会员");

            IRow rowtemp = sheet1.CreateRow(1);
            rowtemp.CreateCell(0).SetCellValue(data.SjCount.ToString());
            rowtemp.CreateCell(1).SetCellValue(data.PtCount.ToString());
            rowtemp.CreateCell(2).SetCellValue(data.NewSjCount.ToString());
            rowtemp.CreateCell(3).SetCellValue(data.NewPtCount.ToString());
        }

        private void AnswerExport(List<AnswerReportInfo> datas, NPOI.HSSF.UserModel.HSSFWorkbook book, int j)
        {
            //添加一个sheet
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet" + j);
            //给sheet1添加第一行的头部标题
            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);

            row1.CreateCell(0).SetCellValue("序号");
            row1.CreateCell(1).SetCellValue("一级题目");
            row1.CreateCell(2).SetCellValue("二级题目");
            row1.CreateCell(3).SetCellValue("答案");
            row1.CreateCell(4).SetCellValue("姓名");
            row1.CreateCell(5).SetCellValue("电话");
            row1.CreateCell(6).SetCellValue("年龄");
            row1.CreateCell(7).SetCellValue("学历");
            row1.CreateCell(8).SetCellValue("车型");
            row1.CreateCell(9).SetCellValue("会员等级");
            row1.CreateCell(10).SetCellValue("身份证号");
            row1.CreateCell(11).SetCellValue("车架号");
            int i = 0;

            foreach (var item in datas)
            {
                NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);
                rowtemp.CreateCell(0).SetCellValue(item.Sort.ToString());
                rowtemp.CreateCell(1).SetCellValue(item.QContent);
                rowtemp.CreateCell(2).SetCellValue(item.CQContent);
                rowtemp.CreateCell(3).SetCellValue(item.OName.Replace("&#44;", ","));
                rowtemp.CreateCell(4).SetCellValue(item.Name);
                rowtemp.CreateCell(5).SetCellValue(item.PhoneNumber);
                rowtemp.CreateCell(6).SetCellValue(item.Age);
                rowtemp.CreateCell(7).SetCellValue(item.Education);
                rowtemp.CreateCell(8).SetCellValue(item.CarModels);
                rowtemp.CreateCell(9).SetCellValue(item.MLevel);
                rowtemp.CreateCell(10).SetCellValue(item.IdentityNumber);
                rowtemp.CreateCell(11).SetCellValue(item.VIN);
                i++;
            }
        }

        private void AnswerExportCs(List<AnswerReportInfo> datas, NPOI.HSSF.UserModel.HSSFWorkbook book, int j)
        {
            //添加一个sheet
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet" + j);
            //给sheet1添加第一行的头部标题
            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);

            row1.CreateCell(0).SetCellValue("序号");
            row1.CreateCell(1).SetCellValue("一级题目");
            row1.CreateCell(2).SetCellValue("二级题目");
            row1.CreateCell(3).SetCellValue("答案");
            row1.CreateCell(4).SetCellValue("姓名");
            row1.CreateCell(5).SetCellValue("电话");
            row1.CreateCell(6).SetCellValue("会员等级");
            row1.CreateCell(7).SetCellValue("身份证号");
            row1.CreateCell(8).SetCellValue("车架号");
            int i = 0;

            foreach (var item in datas)
            {
                NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);
                rowtemp.CreateCell(0).SetCellValue(item.Sort.ToString());
                rowtemp.CreateCell(1).SetCellValue(item.QContent);
                rowtemp.CreateCell(2).SetCellValue(item.CQContent);
                rowtemp.CreateCell(3).SetCellValue(item.OName.Replace("&#44;", ","));
                rowtemp.CreateCell(4).SetCellValue(item.Name);
                rowtemp.CreateCell(5).SetCellValue(item.PhoneNumber);
                rowtemp.CreateCell(6).SetCellValue(item.MLevel);
                rowtemp.CreateCell(7).SetCellValue(item.IdentityNumber);
                rowtemp.CreateCell(8).SetCellValue(item.VIN);
                i++;
            }
        }

        private void AnswerExportCSNew(List<Question> questions, List<AnswerReport> result, List<AnswerReport> datas, HSSFWorkbook book, int p)
        {
            //添加一个sheet
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet" + p);
            //给sheet1添加第一行的头部标题
            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);

            row1.CreateCell(0).SetCellValue("题目");
            row1.CreateCell(1).SetCellValue("答案");
            row1.CreateCell(2).SetCellValue("会员手机号");
            row1.CreateCell(3).SetCellValue("会员姓名");
            row1.CreateCell(4).SetCellValue("会员等级");
            row1.CreateCell(5).SetCellValue("身份证号");
            row1.CreateCell(6).SetCellValue("车架号");
            int i = 0;
            int index = 0;
            int id = 0;

            //获取所有答案的总参与人数
            int joinTotal = result.Where(x => x.Id == (questions.First(z => z.IsRequired).Id)).Count();

            foreach (var item in datas)
            {
                if (id != item.Id && id != 0 && index == joinTotal)
                {
                    index = 0;
                }

                if (id != item.Id && id != 0 && index != 0 && index < joinTotal)
                {
                    for (int x = 0; x < joinTotal - index; x++)
                    {
                        NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);
                        rowtemp.CreateCell(0).SetCellValue(sheet1.GetRow(i).GetCell(0).StringCellValue);
                        rowtemp.CreateCell(1).SetCellValue("");
                        rowtemp.CreateCell(2).SetCellValue(sheet1.GetRow(i).GetCell(2).StringCellValue);
                        rowtemp.CreateCell(3).SetCellValue(sheet1.GetRow(i).GetCell(3).StringCellValue);
                        rowtemp.CreateCell(4).SetCellValue(sheet1.GetRow(i).GetCell(4).StringCellValue);
                        rowtemp.CreateCell(5).SetCellValue(sheet1.GetRow(i).GetCell(5).StringCellValue);
                        rowtemp.CreateCell(6).SetCellValue(sheet1.GetRow(i).GetCell(6).StringCellValue);

                        i++;
                        index++;
                    }
                }
                else
                {
                    NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);
                    rowtemp.CreateCell(0).SetCellValue(item.QContent);
                    rowtemp.CreateCell(1).SetCellValue(item.Answer);
                    rowtemp.CreateCell(2).SetCellValue(item.UserName);
                    rowtemp.CreateCell(3).SetCellValue(item.RealName);
                    rowtemp.CreateCell(4).SetCellValue(item.MLevel);
                    rowtemp.CreateCell(5).SetCellValue(item.IdentityNumber);
                    rowtemp.CreateCell(6).SetCellValue(item.VIN);

                    i++;
                    id = item.Id;
                    index++;
                }
            }
        }

        private void AnswerExportCSNew1(List<Question> questions, List<string> members, List<AnswerReport> datas, HSSFWorkbook book, int p)
        {
            //添加一个sheet
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet" + p);
            //给sheet1添加第一行的头部标题
            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);

            //设置标题头部
            for (int i = 0; i < questions.Count; i++)
            {
                row1.CreateCell(i).SetCellValue(questions[i].QContent.ToString());
            }

            int index = 0;

            //写入具体内容
            foreach (var item in members)
            {
                IEnumerable<AnswerReport> answers = datas.Where(x => x.MemberId == item);

                NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(index + 1);

                //int j = 0;
                //foreach (var ans in answers)
                //{
                //    rowtemp.CreateCell(j).SetCellValue(ans.Answer);
                //    j++;
                //}

                for (int i = 0; i < questions.Count; i++)
                {
                    IEnumerable<AnswerReport> lis = answers.Where(x => x.Id == questions[i].Id);

                    AnswerReport report = lis.Count() > 0 ? lis.First() : null;

                    string answer = report != null ? report.Answer : "";
                    rowtemp.CreateCell(i).SetCellValue(answer);
                }

                index++;
            }
        }
        #endregion
        #endregion
    }
}