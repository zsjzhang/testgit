using System.Threading.Tasks;
using System.Web.Helpers;
using AspNet.Identity.SQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.CarService;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.IRepository;
using Vcyber.BLMS.ManageWeb.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using WebGrease.Css.Extensions;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.ManageWeb.EF;
using System.Text.RegularExpressions;
using System.IO;
using Vcyber.BLMS.Entity.Generated;

namespace Vcyber.BLMS.ManageWeb
{
    //[MvcAuthorize]
    public class DealerMembershipController : Controller
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

        /// <summary>
        /// 车型下拉框
        /// </summary>
        /// <returns></returns>
        public ActionResult CarTypeView()
        {
            IEnumerable<CSBaseCar> _result = _AppContext.BaseCarApp.QueryCars(ECarSeriesType.OrderCar);
            return View(_result);
        }
        public ActionResult Index()
        {
            var dealerId = UserManager.FindById(this.User.Identity.GetUserId()).DealerId;

            if (!string.IsNullOrEmpty(dealerId))
            {
                ViewBag.DealerId = dealerId;
                ViewBag.IsEnable = true;
            }
            else
            {
                ViewBag.DealerId = "";
                ViewBag.IsEnable = false;
            }
            ViewData["cartype"] = _AppContext.BaseCarApp.GetNamelist();
            return View();
        }

        public ActionResult NoJoin()
        {
            var dealerId = UserManager.FindById(this.User.Identity.GetUserId()).DealerId;

            if (!string.IsNullOrEmpty(dealerId))
            {
                ViewBag.DealerId = dealerId;
                ViewBag.IsEnable = true;
            }
            else
            {
                ViewBag.DealerId = "";
                ViewBag.IsEnable = false;
            }
            ViewData["cartype"] = _AppContext.BaseCarApp.GetNamelist();
            return View();      

        }

        //店内入会索九会员
        public ActionResult MemberList(string phoneNumber, string identityNumber, string vin, string startTime, string endTime, string userType, int? pageindex, int? pagesize, string DealerId, string CarCategory, string PaperWork)
        {
            int total = 0;

            string dealerId = UserManager.FindById(this.User.Identity.GetUserId()).DealerId;

            if (string.IsNullOrEmpty(dealerId))
            {
                dealerId = DealerId;
            }

            var cardList = _AppContext.DealerMembershipApp.SelectMemberList(phoneNumber, identityNumber, vin, dealerId, startTime, endTime, userType,CarCategory,PaperWork, pageindex ?? 0, 10, out total);

            var result = new { data = cardList, pos = pageindex ?? 0, total_count = total };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //店内出库未入会
        public ActionResult MemberListNoJoin(string custName, string phoneNumber, string identityNumber, string vin, string startTime, string endTime, int? pageindex, int? pagesize)
        {
            int total = 0;

            string dealerId = UserManager.FindById(this.User.Identity.GetUserId()).DealerId;

            var cardList = _AppContext.DealerMembershipApp.SelectMemberListNoJoin(custName, phoneNumber, identityNumber, vin, dealerId, startTime, endTime, pageindex ?? 0, 10, out total);

            var result = new { data = cardList, pos = pageindex ?? 0, total_count = total };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //店内所有出库
        public ActionResult MemberListAll(string status, string custName, string phoneNumber, string identityNumber, string vin, string startTime, string endTime, string selectCanJoin, string userType, string dealerID,string PaperWork, int? pageindex, int? pagesize, int? IsPay, decimal? Amount)
        {
            int total = 0;
            string dealerId = string.Empty;
            if (string.IsNullOrEmpty(dealerID))
            {
                dealerId = UserManager.FindById(this.User.Identity.GetUserId()).DealerId;

            }
            else
            {
                dealerId = dealerID;
            }

            IsPay = IsPay == null ? -1 : IsPay.Value;
            Amount = Amount == null ? -1 : Amount.Value;
            var cardList = _AppContext.DealerMembershipApp.SelectMemberListAll(status, custName, phoneNumber, identityNumber, vin, startTime, endTime, dealerId, selectCanJoin, userType, PaperWork, pageindex ?? 0, 10, out total, IsPay.Value, Amount.Value);
            foreach (var item in cardList)
            {
                item.Amount = _AppContext.CarServiceUserApp.GetIntegrationByBuyCarPayMoneyJoinMember1(item.IdentityNumber);
            }
            var result = new { data = cardList, pos = pageindex ?? 0, total_count = total };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 出库未入会导出
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <param name="identityNumber"></param>
        /// <param name="vin"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="userType"></param>
        /// <param name="CarCategory"></param>
        /// <returns></returns>
        public FileResult ExportMemberListAll(string status, string custName, string phoneNumber, string identityNumber, string vin, string startTime, string endTime, string selectCanJoin, string userType, string dealerID, string PaperWork,  int? IsPay, decimal? Amount)
        {
            int total = 0;
            string dealerId = string.Empty;
            if (string.IsNullOrEmpty(dealerID))
            {
                dealerId = UserManager.FindById(this.User.Identity.GetUserId()).DealerId;

            }
            else
            {
                dealerId = dealerID;
            }
            IsPay = IsPay == null ? -1 : IsPay.Value;
            Amount = Amount == null ? -1 : Amount.Value;

            //创建Excel文件的对象
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();

            //添加一个sheet
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");

          

            //获取list数据
            var list = _AppContext.DealerMembershipApp.SelectMemberListAll(status, custName, phoneNumber, identityNumber, vin, startTime, endTime, dealerId, selectCanJoin, userType, PaperWork, 1, 500000, out total, IsPay.Value, Amount.Value).ToList();

            //给sheet1添加第一行的头部标题
            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
            row1.CreateCell(0).SetCellValue("入会状态");
            row1.CreateCell(1).SetCellValue("是否同意会");
            row1.CreateCell(2).SetCellValue("会员卡号");
            row1.CreateCell(3).SetCellValue("手机号");
            row1.CreateCell(4).SetCellValue("店代码");
            row1.CreateCell(5).SetCellValue("姓名");
            row1.CreateCell(6).SetCellValue("VIN");
            row1.CreateCell(7).SetCellValue("证件号");
            row1.CreateCell(8).SetCellValue("性别");
            row1.CreateCell(9).SetCellValue("购车时间");
            row1.CreateCell(10).SetCellValue("城市");
            row1.CreateCell(11).SetCellValue("地址");
            row1.CreateCell(12).SetCellValue("应付金额");
            row1.CreateCell(13).SetCellValue("缴费状态");

            //将数据逐步写入sheet1各个行
            for (int i = 0; i < list.Count(); i++)
            {
         
                NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);
                rowtemp.CreateCell(0).SetCellValue(list[i].MembershipId == null ? "否" : "是");
                rowtemp.CreateCell(1).SetCellValue(list[i].IsCanJoin);
                rowtemp.CreateCell(2).SetCellValue(list[i].No);
                rowtemp.CreateCell(3).SetCellValue(list[i].CustMobile);
                rowtemp.CreateCell(4).SetCellValue(list[i].DealerId);
                rowtemp.CreateCell(5).SetCellValue(list[i].CustName);
                rowtemp.CreateCell(6).SetCellValue(list[i].VIN);
                rowtemp.CreateCell(7).SetCellValue(list[i].IdentityNumber);
                rowtemp.CreateCell(8).SetCellValue(list[i].Gender);
                rowtemp.CreateCell(9).SetCellValue(list[i].BuyTime);
                rowtemp.CreateCell(10).SetCellValue(list[i].City);
                rowtemp.CreateCell(11).SetCellValue(list[i].Address);
                rowtemp.CreateCell(12).SetCellValue(list[i].Amount.ToString());
                rowtemp.CreateCell(13).SetCellValue(list[i].IsPayState);
            }

            // 写入到客户端 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", "入会用户列表.xls");
        }

        //导出所有出库用户
        

        #region  页面导出
        //店内会员导出
        public FileResult ExportMemberList(string phoneNumber, string identityNumber, string vin, string startTime, string endTime, string userType, int? pageindex, int? pagesize, string DealerId, string CarCategory, string PaperWork)
        {
            int cou = 0;

            //创建Excel文件的对象
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();

            //添加一个sheet
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");

            string dealerId = string.Empty;
            if (string.IsNullOrEmpty(DealerId))
            {
                dealerId = UserManager.FindById(this.User.Identity.GetUserId()).DealerId;

            }
            else
            {
                dealerId = DealerId;
            }
            int total = 0;
            //获取list数据
            var list = _AppContext.DealerMembershipApp.SelectMemberList(phoneNumber, identityNumber, vin, dealerId, startTime, endTime, userType, CarCategory, PaperWork,1, 50000, out total).ToList();//_AppContext.DealerMembershipApp.SelectMemberList("", "", "", dealerId, startTime, endTime, "", "", "", 1, 50000, out cou).ToList();

            //给sheet1添加第一行的头部标题
            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
            row1.CreateCell(0).SetCellValue("店代码");
            row1.CreateCell(1).SetCellValue("手机号");
            row1.CreateCell(2).SetCellValue("姓名");
            row1.CreateCell(3).SetCellValue("用户名");
            row1.CreateCell(4).SetCellValue("用户等级");
       
            row1.CreateCell(5).SetCellValue("身份证号");
            row1.CreateCell(6).SetCellValue("会员卡号");
            row1.CreateCell(7).SetCellValue("入会时间");
            row1.CreateCell(8).SetCellValue("创建人");
            row1.CreateCell(9).SetCellValue("付款码");

            row1.CreateCell(10).SetCellValue("性别");
            row1.CreateCell(11).SetCellValue("年龄");
            row1.CreateCell(12).SetCellValue("地区");
            row1.CreateCell(13).SetCellValue("城市");
           
            row1.CreateCell(14).SetCellValue("车型");
            row1.CreateCell(15).SetCellValue("使用积分数量");
            row1.CreateCell(16).SetCellValue("剩余积分数量");
            row1.CreateCell(17).SetCellValue("积分总数量");

            //将数据逐步写入sheet1各个行
            for (int i = 0; i < list.Count(); i++)
            {
                NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);
                rowtemp.CreateCell(0).SetCellValue(list[i].DealerId);
                rowtemp.CreateCell(1).SetCellValue(list[i].UserName);
                rowtemp.CreateCell(2).SetCellValue(list[i].CustName);
                rowtemp.CreateCell(3).SetCellValue(list[i].NickName);
                rowtemp.CreateCell(4).SetCellValue(list[i].MLevel);
              
                rowtemp.CreateCell(5).SetCellValue(list[i].IdentityNumber);
                rowtemp.CreateCell(6).SetCellValue(list[i].No);
                rowtemp.CreateCell(7).SetCellValue(list[i].CreateTime);
                rowtemp.CreateCell(8).SetCellValue(list[i].CreatedPerson);
                rowtemp.CreateCell(9).SetCellValue(list[i].PayNumber);

                rowtemp.CreateCell(10).SetCellValue(list[i].Gender);
                rowtemp.CreateCell(11).SetCellValue(list[i].Age);
                rowtemp.CreateCell(12).SetCellValue(list[i].Area);
                rowtemp.CreateCell(13).SetCellValue(list[i].City);
               
                rowtemp.CreateCell(14).SetCellValue(list[i].CarCategory);
                rowtemp.CreateCell(15).SetCellValue(list[i].Shiyongintegral);
                rowtemp.CreateCell(16).SetCellValue(list[i].Shenyuintegral);
                rowtemp.CreateCell(17).SetCellValue(list[i].TotalIntegral);
            }

            // 写入到客户端 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", "入会用户列表.xls");
        }

    
        #endregion
    }
}