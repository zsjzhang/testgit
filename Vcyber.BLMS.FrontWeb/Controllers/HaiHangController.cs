using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vcyber.BLMS.Application;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.FrontWeb.Controllers
{
    public class HaiHangController : Controller
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

        //
        // GET: /HaiHang/
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Send()
        {
            ReturnResult result = new ReturnResult { IsSuccess = false };

            if (this.User.Identity.IsAuthenticated)
            {
                var user = UserManager.FindById(this.User.Identity.GetUserId());

                if (user.IdentityNumber.Length < 15)
                {
                    result.IsSuccess = false;
                    result.Message = "集团用户暂时无法完成注册";
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                var customer = _AppContext.CarServiceUserApp.GetCustomer(user.IdentityNumber);

                //处理姓名
                var family_name = "";
                var first_name = "";

                if (customer.CustName.Length < 1)
                {
                    family_name = "蓝";
                    first_name = "缤";
                }
                else if (customer.CustName.Length == 1)
                {
                    family_name = customer.CustName.Substring(0, 1);
                    first_name = "某";
                }
                else if (customer.CustName.Length == 2)
                {
                    family_name = customer.CustName.Substring(0, 1);
                    first_name = customer.CustName.Substring(1, 1);
                }
                else
                {
                    family_name = customer.CustName.Substring(0, 1);
                    first_name = customer.CustName.Substring(1, 2);
                }


                var client = new HaiHangServiceReference.MemberServiceClient();

                var request = new HaiHangServiceReference.RecruitmentMemberRequest
                {
                    securityKey = "BJ123",
                    civilization_title_code = customer.Gender == "男" ? "Mr" : "Mrs",
                    family_name = family_name,
                    first_name = first_name,
                    mobile_phone = user.UserName,
                    password = "999999",
                    email_option = 0,
                    email_optionSpecified = true,
                    segmentSpecified = true,
                    language = "CN",
                    segment = 4
                };

                //发送请求，获取结果
                var response = new HaiHangServiceReference.RecruitmentMemberResponse();

                //模拟接口
                response.exitCode = 0;
                response.account_number = "960289881942";

                //response = client.submitRecruitmentMember(request);

                if (response.exitCode == 0)
                {
                    result.IsSuccess = true;
                    result.Data = response.account_number;

                    _AppContext.BrandServiceApp.AddMembershipBrand(new MembershipBrand
                    {
                        UserId = user.Id,
                        BrandName = "HaiHang",
                        IsMember = "Y",
                        JoinTime = DateTime.Now
                    });
                }
                else if (response.exitCode == -1 || response.exitCode == -9)
                {
                    result.IsSuccess = false;
                    result.Message = "接口发生内部错误";
                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = response.errorInfo;
                }

                //TO-DO 保存记录
                IF_RequestLog logEntity = new IF_RequestLog()
                {
                    UserId=user.Id,
                    RequestData = _AppContext.RequestLogApp.ConverterToJson(request),
                    ResponseData = _AppContext.RequestLogApp.ConverterToJson(response)
                };
                _AppContext.RequestLogApp.Add(logEntity);

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
    }
}