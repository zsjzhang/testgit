using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Common;

namespace Vcyber.BLMS.ManageWeb.Controllers
{
    public class WeixinController : Controller
    {
        /// <summary>
        /// 客户消息记录
        /// </summary>        
        public ActionResult CustomerServiceRecord(string begintime,string endtime)
        {
            var q = _AppContext.CustomerServiceMessageApp.GroupByWorker(begintime, endtime);
            return View(q);
        }
        /// <summary>
        /// 客户消息记录导出
        /// </summary>
        public void CustomerServiceRecordToExcel(string begintime, string endtime)
        {
            List<string> propertyName = new List<string> { "Worker", "ReceiveCount", "ReplyCount", "ReceivePersons", "ReplyPersons", /*"Mins",*/ "FirstMins", "BetweenTime" };
            List<string> columName = new List<string> { "客服", "接入消息数", "回复消息数", "接入人数", "回复人数", /*"平均响应时长(秒)",*/ "首次响应时长(秒)", "统计时间" };
            var q = _AppContext.CustomerServiceMessageApp.GroupByWorker(begintime, endtime);
            NPOIHelper<Entity.CustomerServiceRecord>.ListToExcel(q.ToList(), "微信多客服绩效分析.xls", propertyName, columName);
        }
        /// <summary>
        /// 客服消息汇总
        /// </summary>        
        public ActionResult CustomerServiceTotal(string begintime, string endtime) 
        {
            var dt = _AppContext.CustomerServiceMessageApp.Total(begintime, endtime);
            return View(dt);
        }

        /// <summary>
        /// 客服消息汇总导出
        /// </summary>
        public FileResult CustomerServiceTotalToExcel(string begintime, string endtime)
        {
            //创建Excel文件的对象
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();

            //获取数据
            var dt = _AppContext.CustomerServiceMessageApp.Total(begintime, endtime);
            this.CustomerServiceExport(book, dt);

            // 写入到客户端 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", "压力分析数据.xls");
        }

        /// <summary>
        /// 通过DT将数据写入excel中
        /// </summary>
        /// <param name="book"></param>
        /// <param name="dt"></param>
        private void CustomerServiceExport(NPOI.HSSF.UserModel.HSSFWorkbook book, DataTable dt)
        {
            //添加一个sheet
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("压力分析数据");
            //给sheet1添加第一行的头部标题
            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);

            row1.CreateCell(0).SetCellValue("序号");
            row1.CreateCell(1).SetCellValue("时间");
            row1.CreateCell(2).SetCellValue("进线数量");
            row1.CreateCell(3).SetCellValue("进线人数");
            row1.CreateCell(4).SetCellValue("回复数量");
            row1.CreateCell(5).SetCellValue("回复人数");
            //row1.CreateCell(6).SetCellValue("平均响应时长");

            int i = 0;

            for (var j = 0; j < dt.Rows.Count; j++)
            {
                NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);
                rowtemp.CreateCell(0).SetCellValue((i + 1).ToString());
                rowtemp.CreateCell(1).SetCellValue(dt.Rows[j][0].ToString());
                rowtemp.CreateCell(2).SetCellValue(dt.Rows[j][1].ToString());
                rowtemp.CreateCell(3).SetCellValue(dt.Rows[j][2].ToString());
                rowtemp.CreateCell(4).SetCellValue(dt.Rows[j][3].ToString());
                rowtemp.CreateCell(5).SetCellValue(dt.Rows[j][4].ToString());
                //rowtemp.CreateCell(6).SetCellValue(dt.Rows[j][5].ToString());

                i++;
            }
        }

        /// <summary>
        /// 微信多客服绩效统计
        /// </summary>
        /// <param name="begintime"></param>
        /// <param name="endtime"></param>
        /// <returns></returns>
        public ActionResult PerformanceStatisticsList(string begintime, string endtime)
        {
            var dt = _AppContext.CustomerServiceMessageApp.PerformanceStatisticsList(begintime, endtime);
            return View(dt);
        }

        /// <summary>
        /// 导出微信多客服绩效统计
        /// </summary>
        /// <param name="begintime"></param>
        /// <param name="endtime"></param>
        /// <returns></returns>
        public FileResult PerformanceStatisticsToExcel(string begintime, string endtime)
        {
            //创建Excel文件的对象
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();

            //获取数据
            var dt = _AppContext.CustomerServiceMessageApp.PerformanceStatisticsList(begintime, endtime);
            this.PerformanceStatisticsExport(book, dt);

            // 写入到客户端 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", "微信多客服绩效统计.xls");
        }

        /// <summary>
        /// 微信多客服绩效统计数据生成excel文档
        /// </summary>
        /// <param name="book"></param>
        /// <param name="dt"></param>
        private void PerformanceStatisticsExport(NPOI.HSSF.UserModel.HSSFWorkbook book, DataTable dt)
        {
            //添加一个sheet
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("微信多客服绩效统计");
            //给sheet1添加第一行的头部标题
            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);

            row1.CreateCell(0).SetCellValue("序号");
            row1.CreateCell(1).SetCellValue("时间");
            row1.CreateCell(2).SetCellValue("进线数量");
            row1.CreateCell(3).SetCellValue("进线人数");
            row1.CreateCell(4).SetCellValue("回复数量");
            row1.CreateCell(5).SetCellValue("回复人数");
            //row1.CreateCell(6).SetCellValue("平均响应时长");
            row1.CreateCell(6).SetCellValue("首次响应时长");

            int i = 0;

            for (var j = 0; j < dt.Rows.Count; j++)
            {
                NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);
                rowtemp.CreateCell(0).SetCellValue((i + 1).ToString());
                rowtemp.CreateCell(1).SetCellValue(dt.Rows[j][0].ToString());
                rowtemp.CreateCell(2).SetCellValue(dt.Rows[j][1].ToString());
                rowtemp.CreateCell(3).SetCellValue(dt.Rows[j][2].ToString());
                rowtemp.CreateCell(4).SetCellValue(dt.Rows[j][3].ToString());
                rowtemp.CreateCell(5).SetCellValue(dt.Rows[j][4].ToString());
                //rowtemp.CreateCell(6).SetCellValue(dt.Rows[j][5].ToString());
                rowtemp.CreateCell(6).SetCellValue(dt.Rows[j][6].ToString());

                i++;
            }
        }
    }
}
