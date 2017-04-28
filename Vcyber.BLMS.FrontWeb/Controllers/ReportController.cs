using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vcyber.BLMS.Application;

using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using System.Data;
using System.IO;

namespace Vcyber.BLMS.FrontWeb.Controllers
{
    public class ReportController : Controller
    {
        //
        // GET: /Report/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PartialPageForIndex(DateTime? startTime, DateTime? endTime, string tableName)
        {
            var table = _AppContext.ReportApp.GetReport(startTime ?? DateTime.Now.AddDays(-1), endTime ?? DateTime.Now, tableName);

            return PartialView(table);
        }

        public ActionResult Export(DateTime? startTime, DateTime? endTime, string tableName)
        {
            int cou = 0;

            //创建Excel文件的对象
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();

            //添加一个sheet
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");

            //获取list数据
            DataTable tables = _AppContext.ReportApp.GetReport(startTime ?? DateTime.Now.AddDays(-1), endTime ?? DateTime.Now, tableName);

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
	}
}