using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vcyber.BLMS.Application;
using System.Data;
using Vcyber.BLMS.ManageWeb.Models;
using Vcyber.BLMS.Entity;
using System.IO;

namespace Vcyber.BLMS.ManageWeb.Controllers
{
    public class EquityController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ServiceUse()
        {
            return View();
        }
        /// <summary>
        /// 获取服务权益分析
        /// </summary>
        /// <returns>（0:100日上门关怀，1:3年9次免检，2：免费取送车服务，3：一对一专属，4：维保)</returns>
        [AllowAnonymous]
        public JsonResult GetEquity(string startTime, string endTime)
        {
            var EquityModelList = new List<EquityModel>();
            DataTable dt = _AppContext.ReportApp.GetEquity(startTime, endTime);

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    var model = new EquityModel();
                    if (dr["OrderType"] != null)
                    {
                        model.OrderType = Convert.ToInt32(dr["OrderType"]);
                    }
                    if (dr["memberNo"] != null)
                    {
                        model.memberNo = Convert.ToInt32(dr["memberNo"]);
                    }
                    if (dr["member"] != null)
                    {
                        model.member = Convert.ToInt32(dr["member"]);
                    }
                    if (dr["total"] != null)
                    {
                        model.total = Convert.ToInt32(dr["total"]);
                    }
                    if (dr["rate"] != null)
                    {
                        string i = String.Format("{0:F}", Convert.ToDecimal(dr["rate"].ToString()) * 100);
                        model.rate = Convert.ToDecimal(i);
                    }
                    EquityModelList.Add(model);
                }
            }
            var result = new
            {
                data = EquityModelList,
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }




        public ActionResult EquityToExcel(string startTime, string endTime)
        {
            DataTable dt = _AppContext.ReportApp.GetEquity(startTime, endTime);
            //创建Excel文件的对象
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();

            //添加一个sheet
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");

            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);

            row1.CreateCell(0).SetCellValue("序号");
            row1.CreateCell(1).SetCellValue("服务权益");
            row1.CreateCell(2).SetCellValue("有资格会员数");
            row1.CreateCell(3).SetCellValue("享受该服务的会员数");
            row1.CreateCell(4).SetCellValue("享受该服务的次数");
            row1.CreateCell(5).SetCellValue("服务产品使用率(%)");

            if (dt.Rows.Count > 0)
            {
                var i = 1;
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["OrderType"].ToString() != "2" && dr["OrderType"].ToString() != "4")
                    {
                        NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i);
                        rowtemp.CreateCell(0).SetCellValue(i.ToString());
                        var fwxm = "";
                        switch (dr["OrderType"].ToString())
                        {
                            case "0":
                                fwxm = "100日上门关怀";
                                break;
                            case "1":
                                fwxm = "3年9次免检";
                                break;
                            case "2":
                                fwxm = "Home To Home";
                                break;
                            case "3":
                                fwxm = "一对一专属服务";
                                break;
                            case "4":
                                fwxm = "维保";
                                break;
                            case "5":
                                fwxm = "机场服务";
                                break;
                            default:
                                fwxm = "";
                                break;
                        }
                        rowtemp.CreateCell(1).SetCellValue(fwxm);
                        rowtemp.CreateCell(2).SetCellValue(this.ConvertString(dr["memberNo"].ToString()));
                        rowtemp.CreateCell(3).SetCellValue(this.ConvertString(dr["member"].ToString()));
                        rowtemp.CreateCell(4).SetCellValue(this.ConvertString(dr["total"].ToString()));
                        rowtemp.CreateCell(5).SetCellValue(this.ConvertString(String.Format("{0:F}", Convert.ToDecimal(dr["rate"].ToString()) * 100)));
                        i++;
                    }
                }
            }

            // 写入到客户端 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", "权益使用分析" + ".xls");
        }


        /// <summary>
        /// 服务使用明细
        /// </summary>
        /// <param name="Createtime"></param>
        /// <param name="RealName"></param>
        /// <param name="phoneNumber"></param>
        /// <param name="DataSource"></param>
        /// <param name="AirportName"></param>
        /// <param name="DealerId"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult GetServiceUse(string Createtime, string RealName, string phoneNumber, string DataSource, string AirportName, string DealerId, string Status, string OrderType, int size, int index)
        {
            int total = 0;
            var list = _AppContext.ReportApp.ServiceUse(Createtime, RealName, phoneNumber, DataSource, AirportName, DealerId, Status,OrderType, (index - 1) * size, size, out total);

            int count = (int)Math.Ceiling((double)total / (double)size);
            ViewBag.Total = total;
            ViewBag.PageIndex = index;
            ViewBag.PrePage = index > 1 ? (index - 1) : 1;
            ViewBag.NextPage = index < count ? (index + 1) : count;
            ViewBag.EndPage = count <= 0 ? 1 : count;
            return PartialView(list);
        }



        public ActionResult SerViceUserToExcel(string Createtime, string RealName, string phoneNumber, string DataSource, string AirportName, string DealerId,string Status, string OrderType)
        {
            int total;
            IEnumerable<ServiceModel> datas = _AppContext.ReportApp.ServiceUse(Createtime, RealName, phoneNumber, DataSource, AirportName, DealerId, Status,  OrderType, 0, 1000, out total);
            //创建Excel文件的对象
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            if (total > 0 && datas != null)
            {
                int servicePage = total % 1000 == 0 ? total / 1000 : total / 1000 + 1;
                //servicePage = servicePage > 5 ? 3 : servicePage;
                for (int j = 1; j <= servicePage; )
                {
                    datas = _AppContext.ReportApp.ServiceUse(Createtime, RealName, phoneNumber, DataSource, AirportName, DealerId, Status,  OrderType, (j - 1) * 1000, 1000, out total);
                    this.SerViceExport(datas, book, j);
                    j++;
                }
            }
            // 写入到客户端 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", "服务使用明细" + ".xls");
        }


        /// <summary>
        /// 导出服务使用明细
        /// </summary>
        /// <param name="datas">导出数据</param>
        /// <param name="book"></param>
        /// <param name="j">Excel页编号</param>
        private void SerViceExport(IEnumerable<ServiceModel> datas, NPOI.HSSF.UserModel.HSSFWorkbook book, int j)
        {
            //添加一个sheet
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet" + j);
            //给sheet1添加第一行的头部标题
            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
            row1.CreateCell(0).SetCellValue("序号");
            row1.CreateCell(1).SetCellValue("预约时间");
            row1.CreateCell(2).SetCellValue("使用人姓名");
            row1.CreateCell(3).SetCellValue("手机号");
            row1.CreateCell(4).SetCellValue("身份证号");
            row1.CreateCell(5).SetCellValue("车架号");
            row1.CreateCell(6).SetCellValue("预约项目");
            row1.CreateCell(7).SetCellValue("渠道");
            row1.CreateCell(8).SetCellValue("串码");
            row1.CreateCell(9).SetCellValue("是否免费");
            row1.CreateCell(10).SetCellValue("是否使用");
            row1.CreateCell(11).SetCellValue("使用时间");
            row1.CreateCell(12).SetCellValue("预约机场");
            row1.CreateCell(13).SetCellValue("实际使用机场");
            row1.CreateCell(14).SetCellValue("店代码");
            row1.CreateCell(15).SetCellValue("店简称");
            row1.CreateCell(16).SetCellValue("办事处");
            row1.CreateCell(17).SetCellValue("区域");
            row1.CreateCell(18).SetCellValue("服务使用状态");
            int i = 0;
            foreach (var item in datas)
            {
                NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);
                rowtemp.CreateCell(0).SetCellValue(item.rowid);
                rowtemp.CreateCell(1).SetCellValue(this.ConvertString(item.CreateTime));
                rowtemp.CreateCell(2).SetCellValue(this.ConvertString(item.RealName));
                rowtemp.CreateCell(3).SetCellValue(this.ConvertString(item.PhoneNumber));
                rowtemp.CreateCell(4).SetCellValue(this.ConvertString(item.IdentityNumber));
                rowtemp.CreateCell(5).SetCellValue(this.ConvertString(item.VIN));
                rowtemp.CreateCell(6).SetCellValue(this.ConvertString(item.OrderType));
                rowtemp.CreateCell(7).SetCellValue(this.ConvertString(item.DataSource));
                rowtemp.CreateCell(8).SetCellValue(this.ConvertString(item.SNCode));
                rowtemp.CreateCell(9).SetCellValue(this.ConvertString(item.SendType));
                rowtemp.CreateCell(10).SetCellValue(this.ConvertString(item.IsUse));
                rowtemp.CreateCell(11).SetCellValue(this.ConvertString(item.UseTime));
                rowtemp.CreateCell(12).SetCellValue(this.ConvertString(item.AirportName));
                rowtemp.CreateCell(13).SetCellValue(this.ConvertString(item.UseAdd));
                rowtemp.CreateCell(14).SetCellValue(this.ConvertString(item.DealerId));
                rowtemp.CreateCell(15).SetCellValue(this.ConvertString(item.Name));
                rowtemp.CreateCell(16).SetCellValue(this.ConvertString(item.Region));
                rowtemp.CreateCell(16).SetCellValue(this.ConvertString(item.Area));
                rowtemp.CreateCell(17).SetCellValue(this.ConvertString(item.Status));
                i++;
            }
        }

        private string ConvertString(string value)
        {
            return string.IsNullOrEmpty(value) ? "" : value;
        }

    }
}