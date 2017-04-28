using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.ManageWeb.Controllers
{
    public class FreeRoadRescueController : Controller
    {
        //
        // GET: /FreeRoadRescue/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Detail()
        {
            return View();
        }

        public JsonResult List(string start, string end, string phoneNumber, string state, int pageindex, int pagesize)
        {
            int total = 0;

            var list = _AppContext.FreeRoadRescueApp.SelectFreeRoadRescueList(start, end, phoneNumber, state);

            total = list.Count();

            var cardList = list.Skip(pagesize * (pageindex - 1)).Take(pagesize);

            var result = new { data = cardList, pos = pageindex, total_count = total };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public FileResult Export(string start, string end, string phoneNumber, string state)
        {
            int cou = 0;

            //创建Excel文件的对象
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();

            //添加一个sheet
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");

            List<CS_FreeRoadRescue> cards = new List<CS_FreeRoadRescue>();

            //获取list数据
            cards = _AppContext.FreeRoadRescueApp.SelectFreeRoadRescueList(start, end, phoneNumber, state).ToList();

            //给sheet1添加第一行的头部标题
            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
            row1.CreateCell(0).SetCellValue("编号");
            row1.CreateCell(1).SetCellValue("手机号");
            row1.CreateCell(2).SetCellValue("地址");
            row1.CreateCell(3).SetCellValue("位置坐标");
            row1.CreateCell(4).SetCellValue("已处理");
            row1.CreateCell(5).SetCellValue("创建时间");
            row1.CreateCell(6).SetCellValue("处理时间");

            //将数据逐步写入sheet1各个行
            for (int i = 0; i < cards.Count(); i++)
            {
                NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);
                rowtemp.CreateCell(0).SetCellValue(cards[i].Id.ToString());
                rowtemp.CreateCell(1).SetCellValue(cards[i].PhoneNumber);
                rowtemp.CreateCell(2).SetCellValue(cards[i].Address);
                rowtemp.CreateCell(3).SetCellValue(cards[i].Position);
                rowtemp.CreateCell(4).SetCellValue(cards[i].IsFinish);
                rowtemp.CreateCell(5).SetCellValue(cards[i].CreateTime.ToString());
                rowtemp.CreateCell(6).SetCellValue(cards[i].UpdateTime.ToString());
            }

            // 写入到客户端 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", "紧急道路救援.xls");
        }

        [HttpPost]
        public JsonResult UpdateState(int id)
        {
            var result = new ReturnResult() { IsSuccess = true };

            result.IsSuccess = _AppContext.FreeRoadRescueApp.FinishFreeRoadRescue(id);

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}