using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Vcyber.BLMS.Common;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.ManageWeb.Models;
using AspNet.Identity.SQL;
using Microsoft.AspNet.Identity;
using Vcyber.BLMS.Entity.CarService;
using System.Configuration;
using Microsoft.Reporting.WebForms;

namespace Vcyber.BLMS.ManageWeb.Controllers
{
    [AllowAnonymous]
    public class ValidateCodeController : Controller
    {
        [HttpPost]
        public ActionResult Send(string PhoneNumber)
        {
            ReturnResult returnResult = _AppContext.UserSecurityApp.SendMobileVerifyCode(PhoneNumber, 6, "blms");

            return Json(returnResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult checkUserIsOf(MembershipModel model)
        {
            var store = new FrontUserStore<FrontIdentityUser>();
            var membershipManager = new UserManager<FrontIdentityUser>(store);

            if (store.CheckUserNameIsExist(model.PhoneNumber))
            {
                return Json(new { success = false, msg = "系统中已存在此手机号" });
            }

            if (store.IsIdentityNumberRepeate(model.IdentityNumber))
            {
                return Json(new { success = false, msg = "系统中已存在此身份证号" });
            }

            return Json(new { success = true , msg =string.Empty });
        }

        /// <summary>
        /// 打印新增消费
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public FileContentResult PrintAddConsumeRecord(ConsumeEntity entity)
        {


            var imgPath = ConfigurationManager.AppSettings["ImgPath"];    //上线用
           
            entity.PaperOrder = imgPath + entity.PaperOrder;

            LocalReport localReport = new LocalReport();
            localReport.ReportPath = Server.MapPath("~/Content/AddConsume.rdlc");
            IList<ConsumeEntity> cardList = new List<ConsumeEntity>();

            cardList.Add(entity);

            ReportDataSource reportDataSource = new ReportDataSource();
            reportDataSource.Name = "DataSet2";

            reportDataSource.Value = cardList;

            localReport.EnableHyperlinks = true;
            localReport.EnableExternalImages = true;

            localReport.DataSources.Add(reportDataSource);
            string reportType = "Image";
            string mimeType;
            string encoding;
            string fileNameExtension;
            //The DeviceInfo settings should be changed based on the reportType            
            //http://msdn2.microsoft.com/en-us/library/ms155397.aspx 

            string deviceInfo = "<DeviceInfo>" +
                "  <OutputFormat>jpeg</OutputFormat>" +
                "  <PageWidth>8.5in</PageWidth>" +
                "  <PageHeight>11in</PageHeight>" +
                "  <MarginTop>0.5in</MarginTop>" +
                "  <MarginLeft>1in</MarginLeft>" +
                "  <MarginRight>1in</MarginRight>" +
                "  <MarginBottom>0.5in</MarginBottom>" +
                "</DeviceInfo>";
            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;
            //Render the report            
            renderedBytes = localReport.Render(reportType, deviceInfo, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
            //Response.AddHeader("content-disposition", "attachment; filename=NorthWindCustomers." + fileNameExtension); 

            return File(renderedBytes, "image/jpeg");
        }






	}
}