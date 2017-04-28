using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Domain;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Common.City;
using Vcyber.BLMS.ManageWeb.Models;
using System.Net;
using System.IO;
using System.Data;
using Vcyber.BLMS.Common;

namespace Vcyber.BLMS.ManageWeb.Controllers
{
    /// <summary>
    /// 经销商管理
    /// </summary>
    [MvcAuthorize]
    public class DealerShipController : Controller
    {
        #region ==== 公共方法 ====

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 查询经销商信息
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
       [ValidateAntiForgeryToken]
        public ActionResult FindDealerShip(DealerShipCondition condition)
        {

            if (FilterStr.IsFlag<DealerShipCondition>(condition))
            {
                return Redirect("/Content/error.htm");   
            }

            int total = 0;
            PageData page = new PageData() { Index = condition.Index, Size = condition.Size };
            var list = _AppContext.DealerApp.findList(condition, page, out total);

            int count = (int)Math.Ceiling((double)total / (double)page.Size);
            ViewBag.Total = total;
            ViewBag.PageIndex = condition.Index;
            ViewBag.PrePage = condition.Index > 1 ? (condition.Index - 1) : 1;
            ViewBag.NextPage = condition.Index < count ? (condition.Index + 1) : count;
            ViewBag.EndPage = count;

            return PartialView(list);
        }

        /// <summary>
        /// 显示添加视图
        /// </summary>
        /// <returns></returns>
        public ActionResult ShowAddView()
        {
            return View();
        }

        /// <summary>
        /// 添加经销商
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
         [ValidateAntiForgeryToken]
        public JsonResult Save(DealerShipV data)
        {
            if (data == null)
            {
                return Json(new ResponseMessage(HttpStatusCode.NotFound, "请填写经销商信息。"));
            }

            string message;
            bool result = data.AddData(out message);

            return Json(new ResponseMessage(result ? HttpStatusCode.OK : HttpStatusCode.NotFound, result ? "添加成功。" : message));
        }

        /// <summary>
        /// 编辑经销商信息
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
         [ValidateAntiForgeryToken]
        public JsonResult Edit(DealerShipV data)
        {
            if (data == null)
            {
                return Json(new ResponseMessage(HttpStatusCode.NotFound, "请填写经销商信息。"));
            }

            string message;
            bool result = data.EditeData(out message);

            return Json(new ResponseMessage(result ? HttpStatusCode.OK : HttpStatusCode.NotFound, result ? "编辑成功。" : message));
        }

        /// <summary>
        /// 获取经销商信息
        /// </summary>
        /// <param name="dealerId">店代码</param>
        /// <returns></returns>
        public JsonResult FindOne(string dealerId)
        {
            var data = _AppContext.DealerApp.findOne(dealerId);
            return Json(new ResponseMessage(data != null ? HttpStatusCode.OK : HttpStatusCode.NotFound, data));
        }

        /// <summary>
        /// 导入经销商信息
        /// </summary>
        /// <returns></returns>
       [ValidateAntiForgeryToken]
        public ActionResult ImportDealerShip()
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
                OverWriteDSExcel writeExcel = new OverWriteDSExcel();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    string message;
                    string jwd = string.Empty;
                    DealerShipV data = new DealerShipV();

                    try
                    {
                        this.CreateDealerShipData(dr, ref jwd, data);
                        bool result = data.AddData(out message);
                        writeExcel.Writer(data, jwd, result ? "成功" : "失败", message);
                    }
                    catch (Exception ex)
                    {
                        writeExcel.Writer(data, jwd, "失败", ex.Message);
                    }

                    rowNumber++;
                }

                // 写入到客户端 
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                writeExcel.Book.Write(ms);
                ms.Seek(0, SeekOrigin.Begin);
                return File(ms, "application/vnd.ms-excel", fileName);
                //return View(new ResponseMessage(HttpStatusCode.OK, "导入成功！！！"));
            }

            return View(new ResponseMessage(HttpStatusCode.NotFound, "请上传文件。"));
        }

        public FileResult ExportDealerShip()
        {
            int cou = 0;

            //创建Excel文件的对象
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();

            //添加一个sheet
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");

            List<Vcyber.BLMS.Entity.Generated.CSCarDealerShip> cards = new List<Vcyber.BLMS.Entity.Generated.CSCarDealerShip>();

            //获取list数据
            cards = _AppContext.DealerApp.GetAll().ToList();

            //给sheet1添加第一行的头部标题
            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
            row1.CreateCell(0).SetCellValue("店代码");
            row1.CreateCell(1).SetCellValue("全称");
            row1.CreateCell(2).SetCellValue("简称");
            row1.CreateCell(3).SetCellValue("省");
            row1.CreateCell(4).SetCellValue("市");
            row1.CreateCell(5).SetCellValue("销售电话");
            row1.CreateCell(6).SetCellValue("售后电话");
            row1.CreateCell(7).SetCellValue("办事处");
            row1.CreateCell(8).SetCellValue("区域");
            row1.CreateCell(9).SetCellValue("淘宝账号");
            row1.CreateCell(10).SetCellValue("支付账户");
            row1.CreateCell(11).SetCellValue("邮箱");
            row1.CreateCell(12).SetCellValue("地址坐标");
            row1.CreateCell(13).SetCellValue("道路救援电话");

            //将数据逐步写入sheet1各个行
            for (int i = 0; i < cards.Count(); i++)
            {
                NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);
                rowtemp.CreateCell(0).SetCellValue(cards[i].DealerId.ToString());
                rowtemp.CreateCell(1).SetCellValue(cards[i].Name.ToString());

                rowtemp.CreateCell(2).SetCellValue(cards[i].Abbreviation == null ? "" : cards[i].Abbreviation);
                rowtemp.CreateCell(3).SetCellValue(cards[i].Province == null ? "" : cards[i].Province);
                rowtemp.CreateCell(4).SetCellValue(cards[i].City == null ? "" : cards[i].City);
                rowtemp.CreateCell(5).SetCellValue(cards[i].Phone == null ? "" : cards[i].AfterSalesPhone);

                rowtemp.CreateCell(6).SetCellValue(cards[i].AfterSalesPhone);
                rowtemp.CreateCell(7).SetCellValue(cards[i].Region == null ? "" : cards[i].Region);
                rowtemp.CreateCell(8).SetCellValue(cards[i].Area == null ? "" : cards[i].Area);
                rowtemp.CreateCell(9).SetCellValue(cards[i].TBAccount == null ? "" : cards[i].TBAccount.ToString());
                rowtemp.CreateCell(10).SetCellValue(cards[i].AlipayAccount == null ? "" : cards[i].AlipayAccount.ToString());
                rowtemp.CreateCell(11).SetCellValue(cards[i].Email == null ? "" : cards[i].Email.ToString());
                rowtemp.CreateCell(12).SetCellValue(cards[i].Position == null ? "" : cards[i].Position.ToString());
                rowtemp.CreateCell(13).SetCellValue(cards[i].FreeRoadRescuePhone == null ? "" : cards[i].FreeRoadRescuePhone.ToString());
            }

            // 写入到客户端 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", "DealerShipList.xls");
        }

        /// <summary>
        /// 下载客户信息模板
        /// </summary>
        /// <returns></returns>
        public ActionResult DownloadCustomerTemplet()
        {
            string rootPath = this.Server.MapPath("~");
            string filePath = Path.Combine(rootPath, "App_Data\\经销商信息.xls");


            return File(filePath, "application/vnd.ms-excel");
        }

        /// <summary>
        /// 供应商下拉框
        /// </summary>
        /// <returns></returns>
        public ActionResult ProvinceCity()
        {
            List<Provinces> proDatas = CityService.Instance.GetProvince();
            var proNames = proDatas.Select<Provinces, string>((p) => { return p.Name; });
            return View(proNames);
        }

        /// <summary>
        /// 根据省获取省下的市
        /// </summary>
        /// <param name="provinceId"></param>
        /// <returns></returns>
        public JsonResult Citys(string provinceValue)
        {
            List<City> cityDatas = CityService.Instance.GetCityName(provinceValue);
            var _result = cityDatas.Select<City, string>((c) => { return c.Name; });
            return Json(_result, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region ==== 私有方法 ====

        /// <summary>
        /// 创建经销商数据
        /// </summary>
        /// <param name="dr">Excel一行数据</param>
        /// <param name="wd">纬度</param>
        /// <param name="jd">经度</param>
        /// <param name="data"></param>
        private void CreateDealerShipData(DataRow dr, ref string jwd, DealerShipV data)
        {
            data.DealerId = dr[0].ToString();
            data.Name = dr[1].ToString();
            data.Abbreviation = dr[2].ToString();
            data.Province = dr[3].ToString();
            data.City = dr[4].ToString();
            data.Address = dr[5].ToString();
            data.Phone = dr[6].ToString();
            data.AfterSalesPhone = dr[7].ToString();
            data.Region = dr[8].ToString();
            data.Area = dr[9].ToString();
            data.TBAccount = dr[10].ToString();
            data.AlipayAccount = dr[11].ToString();
            data.Email = dr[12].ToString();
            data.FreeRoadRescuePhone = dr[14].ToString();
            
            jwd = dr[13].ToString();

            if (!string.IsNullOrEmpty(jwd))
            {
                data.Position = jwd;
            }
        }

        #endregion

    }
}