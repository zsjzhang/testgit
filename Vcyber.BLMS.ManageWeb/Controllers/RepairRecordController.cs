using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AspNet.Identity.SQL;
using Microsoft.AspNet.Identity.Owin;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.ManageWeb.Models;




//using Vcyber.BLMS.ManageWeb.EF;
using Microsoft.AspNet.Identity;


namespace Vcyber.BLMS.ManageWeb.Controllers
{
    public class RepairRecordController : Controller
    {
        //
        // GET: /RepairRecord/
        public ActionResult Index()
        {
            return View();
        }


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

        public JsonResult GetRepairRecordList(string phoneNumber, string vin, DateTime? repairStartTime, DateTime? repairEndTime, int? serviceType, int? skip, int? count)
        {
           

            var listModel = new List<RepairRecordModel>();

            var totalCount = 0;
            var identityNumber = "";
            if (!string.IsNullOrEmpty(phoneNumber))
            {
                var user = new FrontUserStore<FrontIdentityUser>().FindByNameAsync(phoneNumber);
                if (user.Result != null && string.IsNullOrEmpty(user.Result.IdentityNumber))
                {
                    identityNumber = user.Result.IdentityNumber;
                }

            }

            var serviceTypeValue = "";
            if (serviceType == null || serviceType == -1)
            {
                serviceTypeValue = string.Empty;
            }
            else
            {
                serviceTypeValue = ((EDMSServiceType)serviceType).DisplayName();
            }

            var userId = this.User.Identity.GetUserId();
            var users = UserManager.FindById(userId);
            var list = _AppContext.RepairRecordApp.GetRepirRecordList(identityNumber, vin, repairStartTime, repairEndTime, serviceTypeValue, skip,count, users.DealerId,out totalCount);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        var model = new RepairRecordModel();
                        model.Id = item.Id;
                        model.IdentityNumber = item.IdentityNumber;
                        model.ServiceType = item.ServiceType;
                        model.VINCode = item.VINCode;
                        model.DealerName = item.DealerName;
                        if (!string.IsNullOrEmpty(item.IdentityNumber))
                        {
                            var user = new FrontUserStore<FrontIdentityUser>().FindByIdentityNumber(item.IdentityNumber);
                            if (user.Result != null)
                            {
                                model.PhoneNumber = user.Result.PhoneNumber;
                            }

                        }

                        model.RepairTime = item.RepairTime.ToShortDateString();
                        model.FinishTime = item.FinishTime.ToShortDateString();
                        model.Status = item.Status;
                        model.IdentityNumber = item.IdentityNumber;
                        model.RepairReportId = item.RepairReportId;

                        listModel.Add(model);
                    }


                }
            var result  = new { data = listModel, total_count = totalCount };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}