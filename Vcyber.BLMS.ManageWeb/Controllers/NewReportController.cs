using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.Entity.Report;

namespace Vcyber.BLMS.ManageWeb.Controllers
{
    public class NewReportController : Controller
    {
        readonly int EXCEL03_MaxRow = 65535;
        // GET: NewReport
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult WeekReport(string start, string end)
        {
            DataTable dt = _AppContext.ReportApp.GetWeekReport(start, end);
            ViewBag.zc = dt.Rows[0][0];
            ViewBag.pk = dt.Rows[0][1];
            ViewBag.yk = dt.Rows[0][2];
            ViewBag.jk = dt.Rows[0][3] == DBNull.Value ? "0" : dt.Rows[0][3];
            ViewBag.qd = dt.Rows[0][4];
            return View();
        }

        [HttpGet]
        public ActionResult WeekReport()
        {
            return View();
        }

        [HttpGet]
        public ActionResult CardReport()
        {

            return View();
        }

        [HttpPost]
        public ActionResult GetUserintegralReport(string start, string end)
        {

            ViewBag.start = start;
            ViewBag.end = end;
            DataTable dt = _AppContext.ReportApp.GetUserintegralReport(start, end);

            ViewBag.NewBuy = dt.Rows[0][0] == DBNull.Value ? "0" : dt.Rows[0][0];
            ViewBag.AddBuy = dt.Rows[0][1] == DBNull.Value ? "0" : dt.Rows[0][1];
            ViewBag.weibao = dt.Rows[0][2] == DBNull.Value ? "0" : dt.Rows[0][2];
            ViewBag.amdin = dt.Rows[0][3] == DBNull.Value ? "0" : dt.Rows[0][3];
            ViewBag.renzheng = dt.Rows[0][4] == DBNull.Value ? "0" : dt.Rows[0][4];
            ViewBag.yuena = dt.Rows[0][5] == DBNull.Value ? "0" : dt.Rows[0][5];
            return View();
        }
        [HttpGet]
        public ActionResult GetUserintegralReport()
        {
            return View();
        }

        [HttpPost]
        public ActionResult PointCost(string start, string end)
        {
            ViewBag.start = start;
            ViewBag.end = end;
            DataTable dt = _AppContext.ReportApp.GetPointCostReport(start, end);
            ViewBag.lpdh = dt.Rows[0][0];
            ViewBag.wbxf = dt.Rows[0][1];
            ViewBag.jcxf = dt.Rows[0][2];
            return View();
        }
        [HttpGet]
        public ActionResult PointCost()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetMemberResourceReport(string start, string end)
        {

            ViewBag.start = start;
            ViewBag.end = end;
            DataTable dt = _AppContext.ReportApp.GetMemberResourceReport(start, end);
            ViewBag.Fwx = dt.Rows[0][0];
            ViewBag.Fapp = dt.Rows[0][1];
            ViewBag.Fweb = dt.Rows[0][2];
            ViewBag.LikeD = dt.Rows[0][3];
            ViewBag.chezhuwx = dt.Rows[0][4];
            ViewBag.chezhuapp = dt.Rows[0][5];
            ViewBag.chezhuweb = dt.Rows[0][6];
            return View();
        }
        [HttpGet]
        public ActionResult GetMemberResourceReport()
        {
            return View();
        }

        //获取经销商购车且入会的报表数据
        [HttpGet]
        public ActionResult GetMemberDearBuyCarReport()
        {
            return View();
        }

        public ActionResult DearBuyCarReport(string startTime, string endTime, string tableName)
        {
            IWorkbook book = new HSSFWorkbook();
            //DateTime startTime="2016-01-01";
            //    DateTime endTime="2016-08-01";
            //  string tableName="GetMemeberDearCarReport";
            //  IWorkbook book = new HSSFWorkbook();

            //获取list数据
            DataTable dt = _AppContext.ReportApp.GetMemberDearBuyCarReport(startTime, endTime, tableName);

            if (dt.Rows.Count < EXCEL03_MaxRow)
                DataWrite2Sheet(dt, 0, dt.Rows.Count - 1, book, "Sheet1");
            else
            {
                int page = dt.Rows.Count / EXCEL03_MaxRow;
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
            if (tableName == "[dbo].[BLMS_MemberCarCategorydate]")
            {
                return File(ms, "application/vnd.ms-excel", "会员车辆数据分析" + ".xls");
            }
            return File(ms, "application/vnd.ms-excel", tableName + ".xls");
        }
        public ActionResult PartialPage(string startTime, string endTime, string tableName)
        {
            var table = _AppContext.ReportApp.GetMemberDearBuyCarReport(startTime, endTime, tableName);


            return PartialView(table);
        }

        public ActionResult PartialCard(string name, string type)
        {
            DataTable dt = _AppContext.ReportApp.CardReport(name, type);
            string table = "";
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    table += string.Format("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><tr>", dr[0], dr[1], dr[2], dr[3]);
                }
            }
            ViewBag.table = table;
            return PartialView();
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


        //购车入会率（时间，办事处）-------------------------------------------
        [HttpGet]
        public ActionResult GetMemberJoinDate()
        {
            return View();
        }

        //会员信息总表
        [HttpGet]
        public ActionResult GetSumMemberReport()
        {
            return View();
        }

        public ActionResult GetMemberJoinDate1(DateTime? startTime, DateTime? endTime, DateTime? buyStartTime, DateTime? buyEndTime, string region, string tableName)
        {
            IWorkbook book = new HSSFWorkbook();
            //DateTime startTime="2016-01-01";
            //    DateTime endTime="2016-08-01";
            //  string tableName="GetMemeberDearCarReport";
            //  IWorkbook book = new HSSFWorkbook();

            //获取list数据
            DataTable dt = _AppContext.ReportApp.GetMemberJoinDate(startTime ?? DateTime.Now.AddDays(-1), endTime ?? DateTime.Now, buyStartTime ?? DateTime.Now, buyEndTime ?? DateTime.Now, region, tableName);

            if (dt.Rows.Count < EXCEL03_MaxRow)
                DataWrite2Sheet(dt, 0, dt.Rows.Count - 1, book, "Sheet1");
            else
            {
                int page = dt.Rows.Count / EXCEL03_MaxRow;
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
            if (tableName == "[dbo].[BLMS_MemberJoindateEnd1]")
            {
                return File(ms, "application/vnd.ms-excel", "购车入会率" + ".xls");
            }
            else if (tableName == "[dbo].[BLMS_MemberJoindateEnd2]")
            {
                return File(ms, "application/vnd.ms-excel", "新车主入会率" + ".xls");
            }
            else
            {
                return File(ms, "application/vnd.ms-excel", "累积入会率" + ".xls");
            }

        }
        public ActionResult PartialPageNew(DateTime? startTime, DateTime? endTime, DateTime buyStartTime, DateTime buyEndTime, string region, string tableName)
        {
            var table = _AppContext.ReportApp.GetMemberJoinDate(startTime ?? DateTime.Now.AddDays(-1), endTime ?? DateTime.Now, buyStartTime, buyEndTime, region, tableName);

            return PartialView(table);
        }

        //购车入会率（时间，办事处）-------------------------------------------
        [HttpGet]
        public ActionResult GetDealeridMemberJoinDate()
        {
            return View();
        }
        public ActionResult DealeridPartialPageNew(DateTime? startTime, DateTime? endTime, DateTime buyStartTime, DateTime buyEndTime, string region, string tableName)
        {
            var table = _AppContext.ReportApp.GetMemberJoinDate(startTime ?? DateTime.Now.AddDays(-1), endTime ?? DateTime.Now, buyStartTime, buyEndTime, region, tableName);

            return PartialView(table);
        }
        //按照店代码导出
        public ActionResult DealeridGetMemberJoinDate(DateTime? startTime, DateTime? endTime, DateTime? buyStartTime, DateTime? buyEndTime, string region, string tableName)
        {
            IWorkbook book = new HSSFWorkbook();

            //获取list数据
            DataTable dt = _AppContext.ReportApp.GetMemberJoinDate(startTime ?? DateTime.Now.AddDays(-1), endTime ?? DateTime.Now, buyStartTime ?? DateTime.Now, buyEndTime ?? DateTime.Now, region, tableName);

            if (dt.Rows.Count < EXCEL03_MaxRow)
                DataWrite2Sheet(dt, 0, dt.Rows.Count - 1, book, "Sheet1");
            else
            {
                int page = dt.Rows.Count / EXCEL03_MaxRow;
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
            if (tableName == "[dbo].[BLMS_DealerId_MemberJoindate]")
            {
                return File(ms, "application/vnd.ms-excel", "店代码入会率" + ".xls");
            }
            else
            {
                return File(ms, "application/vnd.ms-excel", "店代码入会率" + ".xls");
            }
        }

        //积分使用率统计
        [HttpGet]
        public ActionResult Userintegral_ReportDate()
        {
            return View();
        }
        public ActionResult UserintegralPartialPage(string startTime, string endTime, string tableName)
        {
            var table = _AppContext.ReportApp.GetMemberDearBuyCarReport(startTime, endTime, tableName);
            ViewBag.pcount = table.Rows.Count;// Math.Ceiling((double)table.Rows.Count / 10);
            ViewBag.pagesize = 10;
            ViewBag.page = 0;
            //string  tablestr= DataTableToStringFen_New(table, "classS",table.Rows.Count,"");

            return PartialView(DtSelectTop(10, table));
        }

        /// <summary>
        /// DataTable转换成前台html表格
        /// </summary>
        /// <param name="dt">数据源</param>
        /// <param name="classS">样式</param>
        /// <param name="Count">总数量</param>
        /// <param name="opre">是否有操作列</param>
        /// <returns></returns>
        public static string DataTableToStringFen_New(DataTable dt, string classS, int Count, string opre)
        {
            int j = 1;//计数
            int ii = 0;//循环每行的每列数据
            int jj = 0;//改变循环列的初始值gh
            string str = string.Empty;//返回的字符串
            int color = 0;//改变行的颜色
            str += "<span>共'" + Count + "'条数据</span>";
            str += "<table  border='" + 1 + "'  cellpadding='" + 0 + "' cellspacing='" + 0 + "' class='" + classS + "'><thead><tr >";
            //循环列标题
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                str += "<th>" + dt.Columns[i] + " </th>";
            }
            if (opre != null && opre != "")
            {
                str += "<th>" + "操作" + " </th>";
            }
            str += "</tr></thead><tbody id='group_one'>";
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow dt1 = null;
                DataTable dtb = null;
                dtb = dt.Copy();
                dt1 = dtb.Rows[0];
                dtb.Rows.Remove(dt1);

                //循环每行的数据
                DataTable newDT = DtSelectTop(10, dt);  //取前10条 复制给一个新的 DataTable
                foreach (DataRow dr in newDT.Rows)
                {
                    if (color % 2 == 0)
                    {
                        str += "<tr align='center' class='GridViewRowStyle'>";
                    }
                    else
                    {
                        str += "<tr align='center' class='GridViewRowStyle odd'>";
                    }
                    //判断是否有序号列
                    for (int iii = 0; iii < dt.Columns.Count; iii++)
                    {
                        if (dt.Columns[iii].ToString().Contains("序号"))
                        {
                            str += "<td>" + j + "</td>";
                            jj = 1;
                        }
                    }
                    for (ii = jj; ii < dt.Columns.Count; ii++)
                    {
                        str += " <td>" + dr[ii] + "</td>";


                    }
                    if (opre != null && opre != "")
                    {
                        str += " <td>" + "<a href='#' class='BJImg' onclick='BJ(" + dr[0] + ")'>" + opre + "</a>" + "</td>";  //<a>查看</a>/
                    }
                    str += " </tr>";
                    j++;
                    color = color + 1;
                }
                str += "</tbody></table>";

            }
            else
            {
                str += "<tr><td>暂无数据显示</td></tr>";
                str += "</tbody></table>";
            }

            return str;
        }
        /// <summary>
        /// 取前N行
        /// </summary>
        /// <param name="TopItem"></param>
        /// <param name="oDT"></param>
        /// <returns></returns>
        public static DataTable DtSelectTop(int TopItem, DataTable oDT)
        {
            if (oDT.Rows.Count < TopItem) return oDT;

            DataTable NewTable = oDT.Clone();
            DataRow[] rows = oDT.Select("1=1");
            for (int i = 0; i < TopItem; i++)
            {
                NewTable.ImportRow((DataRow)rows[i]);
            }
            return NewTable;
        }

        [HttpGet]
        public ActionResult ConsumeUserintegral_REPORT()
        {
            ViewBag.pcount =0;
            ViewBag.pagesize = 10;
            ViewBag.page=0;
            return View();
        }

        //积分使用率统计
        [HttpGet]
        public ActionResult AcitivityCard_NoCancel()
        {
            var list = _AppContext.ReportApp.GetScServiceActivitName();
            return View();
        }
        public JsonResult GetScServiceActivitName()
        {
            var list = _AppContext.ReportApp.GetScServiceActivitName().ToList();
            var activityTypelist = list.Select(a => a.ActivityType);
            return Json(activityTypelist, JsonRequestBehavior.AllowGet);
        }
        //已领取未核销卡券列表
        public ActionResult AcitivityCard_NoCancelPage(string startTime, string endTime, string AcitivityName)
        {
            var tableName = "[dbo].[BLMS_AcitivityCard_NoCancel]";
            var table = _AppContext.ReportApp.AcitivityCard_NoCancel(startTime, endTime, tableName, AcitivityName);
            return PartialView(table);
        }
        public ActionResult AcitivityCard_NoCancelRoport(string startTime, string endTime, string tableName, string AcitivityName)
        {
            IWorkbook book = new HSSFWorkbook();

            //获取list数据
            tableName = "[dbo].[BLMS_AcitivityCard_NoCancel]";
            DataTable dt = _AppContext.ReportApp.AcitivityCard_NoCancel(startTime, endTime, tableName, AcitivityName);

            if (dt.Rows.Count < EXCEL03_MaxRow)
                DataWrite2Sheet(dt, 0, dt.Rows.Count - 1, book, "Sheet1");
            else
            {
                int page = dt.Rows.Count / EXCEL03_MaxRow;
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
            return File(ms, "application/vnd.ms-excel", "已领取未核销卡券" + ".xls");
        }

        public ActionResult SqlQuery()
        {
            return View();
        }
        public ActionResult SqlQueryPartial(string sqlStr)
        {
            var table = new DataTable();
            if (HasSqlKey(sqlStr))
            {
                return Json(new { IsSuccess = false, Message = "功能只限sql查询！" }, JsonRequestBehavior.AllowGet);
            }
            try
            {
                DBHelper dbHelper = new DBHelper();
                var data = dbHelper.ExecDataSet(sqlStr, new SqlParameter[1] { new SqlParameter("@param", "") });
                table = data.Tables[0];
            }
            catch (Exception ex)
            {
                return Json(new { IsSuccess = false, Message = "查询失败，请检查sql语句！" }, JsonRequestBehavior.AllowGet);
            }
            return PartialView(table);
        }

        private bool HasSqlKey(string InText)
        {
            Regex reg = new Regex(@"\W+update\s+|\W+delete\s+|\W+truncate\s+|\W+exec\s+|\W+insert\s+|\W+drop\s+|^[^select]");
            if (!string.IsNullOrEmpty(InText)&&reg.IsMatch(InText.ToLower()))
                return true;
            return false;
        }
        #region Excel导出
        public void ExportData(string sql)
        {
            string result = string.Empty;
            try
            {
                if (!string.IsNullOrWhiteSpace(sql))
                {
                    DBHelper dbHelper = new DBHelper();
                    var db = dbHelper.ExecDataSet(sql, new SqlParameter[1] { new SqlParameter("@param", "") }).Tables[0];
                    //DataTable db = Session["sqlStr"] as DataTable;
                    if (db.Rows.Count > 0)
                    {
                        NPOIHelper<string>.ExportDataTableToExcel(db, "导出数据.xls");
                    }
                    else
                    {
                        Response.Write("<script>alert('查询数据列表为空，无数据导出！');window.history.go(-1);</script>");
                    }
                }
                else
                {
                    Response.Write("<script>alert('请先查询数据再导出');window.history.go(-1);</script>");
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('系统异常，请联系管理员');window.history.go(-1);</script>");
            }
        }
        #endregion

        /// <summary>
        /// DealerStory
        /// </summary>
        /// <returns></returns>
        public ActionResult DealerStory()
        {
            return View();
        }
        public ActionResult DealerStoryPartial(string phoneNumber, string StartTime, string EndTime)
        {
            DataSet ds = new DBHelper().ExecDataSetProc("BLMS_DealerStory", new SqlParameter[] { new SqlParameter("@PhoneNumber", phoneNumber), new SqlParameter("@BeginDay", StartTime), new SqlParameter("@EndDay", EndTime) });
            if (ds.Tables.Count > 0)
            {
                return PartialView(GetListDealerStoryPartial(ds.Tables[0]));
            }
            return PartialView(new List<DealerStoryDto>());
        }

        public void DealerStoryToExcel(string ids)
        {
            //string phoneNumber = "18210219279";
            //string StartTime = ""; 
            //string EndTime = "";
            //var list = GetListDealerStoryPartial(phoneNumber, StartTime, EndTime);
            var list = new List<DealerStoryDto>();
            //查询数据
            DataSet ds = new DBHelper().ExecDataSetProc("BLMS_DealerStoryIds", new SqlParameter[] { new SqlParameter("@Ids", ids) });
            if (ds.Tables.Count > 0)
            {
                list = GetListDealerStoryPartial(ds.Tables[0]);
            }
            //导出到excel
            if (list.Count > 0)
            {
                //创建一个工作簿
                HSSFWorkbook workbook = new HSSFWorkbook();
                //创建一个sheet
                ISheet sheet1 = workbook.CreateSheet("sheet1");
                // 设置列宽,excel列宽每个像素是1/256
                sheet1.SetColumnWidth(0, 18 * 256);
                sheet1.SetColumnWidth(1, 18 * 256);
                IRow rowHeader = sheet1.CreateRow(0);//创建表头行
                rowHeader.CreateCell(0, CellType.String).SetCellValue("手机号");
                rowHeader.CreateCell(1, CellType.String).SetCellValue("标题");
                rowHeader.CreateCell(2, CellType.String).SetCellValue("内容");
                rowHeader.CreateCell(3, CellType.String).SetCellValue("图片1");
                rowHeader.CreateCell(4, CellType.String).SetCellValue("图片2");
                rowHeader.CreateCell(5, CellType.String).SetCellValue("图片3");
                rowHeader.CreateCell(6, CellType.String).SetCellValue("图片4");
                rowHeader.CreateCell(7, CellType.String).SetCellValue("图片5");
                rowHeader.CreateCell(8, CellType.String).SetCellValue("图片6");
                rowHeader.CreateCell(9, CellType.String).SetCellValue("图片7");
                rowHeader.CreateCell(10, CellType.String).SetCellValue("图片8");
                rowHeader.CreateCell(11, CellType.String).SetCellValue("图片9");

                int rowNum = 1;
                foreach (var item in list)
                {
                    IRow row = sheet1.CreateRow(rowNum);
                    //设置行高 ,excel行高度每个像素点是1/20
                    //if (item.ImgeUrls.Count > 0)
                    //{
                    //    row.Height = 80 * 20;
                    //}
                    //sheet1.SetColumnWidth(3, 280 * 20);
                    //sheet1.SetColumnWidth(4, 280 * 20);
                    //sheet1.SetColumnWidth(5, 280 * 20);
                    //sheet1.SetColumnWidth(6, 280 * 20);
                    //sheet1.SetColumnWidth(7, 280 * 20);
                    //sheet1.SetColumnWidth(8, 280 * 20);
                    //sheet1.SetColumnWidth(9, 280 * 20);
                    //sheet1.SetColumnWidth(10, 280 * 20);
                    //sheet1.SetColumnWidth(11, 280 * 20);
                    //填入手机号、标题、内容
                    row.CreateCell(0, CellType.String).SetCellValue(item.PhoneNumber);
                    row.CreateCell(1, CellType.String).SetCellValue(item.Title);
                    row.CreateCell(2, CellType.String).SetCellValue(item.Contents);
                    //将图片文件读入一个字符串
                    int picIndex = 3;
                    //HSSFPatriarch patriarch = (HSSFPatriarch)sheet1.CreateDrawingPatriarch();
                    foreach (var img in item.ImgeUrls)
                    {
                        //try
                        //{
                        //    WebClient w = new WebClient();
                        //    byte[] bytes = w.DownloadData(img);
                        //    //byte[] bytes = System.IO.File.ReadAllBytes(img);
                        //    int pictureIdx = workbook.AddPicture(bytes, PictureType.JPEG);
                        //    // 插图片的位置  HSSFClientAnchor（dx1,dy1,dx2,dy2,col1,row1,col2,row2) 后面再作解释
                        //    HSSFClientAnchor anchor = new HSSFClientAnchor(0, 0, 0, 0, picIndex + 3, rowNum, picIndex + 4, rowNum + 1);
                        //    //把图片插到相应的位置
                        //    HSSFPicture pict = (HSSFPicture)patriarch.CreatePicture(anchor, pictureIdx);
                        //}
                        //catch (Exception e)
                        //{
                        //    continue;
                        //}
                        row.CreateCell(picIndex, CellType.String).SetCellValue(img);
                        picIndex++;
                    }

                    rowNum++;
                }


                MemoryStream ms = new MemoryStream();
                workbook.Write(ms);
                ms.Flush();
                ms.Position = 0;
                sheet1 = null;
                rowHeader = null;
                workbook = null;

                System.Web.HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=车主故事.xls");
                System.Web.HttpContext.Current.Response.BinaryWrite(ms.ToArray());
                System.Web.HttpContext.Current.ApplicationInstance.CompleteRequest();
                ms.Close();
                ms = null;
            }
            else
            {
                Response.Write("<script>alert('请选择要导出的数据！');window.history.go(-1);</script>");
            }
        }
        private List<DealerStoryDto> GetListDealerStoryPartial(DataTable dt)
        {
            List<DealerStoryDto> dtoList = new List<DealerStoryDto>();

            foreach (DataRow item in dt.Rows)
            {
                List<string> imageUrlList = new List<string>();
                var itemImageUrlList = item["Image"].ToString().Split(new char[] { ',' }).ToList();

                foreach (var url in itemImageUrlList)
                {
                    if (!string.IsNullOrWhiteSpace(url))
                        imageUrlList.Add(ConfigurationManager.AppSettings["DealerStoryUrl"].ToString() + url);
                }
                dtoList.Add(new DealerStoryDto()
                {
                    Id = item["ID"].ToString(),
                    Title = item["Title"].ToString(),
                    Contents = item["Contents"].ToString(),
                    PhoneNumber = item["PhoneNumber"].ToString(),
                    ImgeUrls = imageUrlList
                });
            }

            return dtoList;
        }
    }

}