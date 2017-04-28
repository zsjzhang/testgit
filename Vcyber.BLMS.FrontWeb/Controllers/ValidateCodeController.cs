using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Vcyber.BLMS.Common;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Application;

namespace Vcyber.BLMS.FrontWeb.Controllers
{
    public class ValidateCodeController : Controller
    {
        //
        // GET: /ValidateCode/
        public ActionResult Index()
        {
            //生成验证码
            ValidateCodeHelper validateCode = new ValidateCodeHelper();
            string code = validateCode.CreateValidateCode(4);
            //CacheHelp cachehelp = new CacheHelp();
            //var cacheobject = cachehelp.GetCache("ValidateCode");
            //if (cacheobject != null)
            //{
            //    cachehelp.RemoveCache("ValidateCode");
            //}
            //cachehelp.SetCache("ValidateCode", code);
            Session["ValidateCode"] = code;
            byte[] bytes = validateCode.CreateValidateGraphic(code);
            return File(bytes, @"image/jpeg");
        }

        [HttpPost]
        public ActionResult Send(string PhoneNumber)
        {
            ReturnResult returnResult = _AppContext.UserSecurityApp.SendMobileVerifyCode(PhoneNumber, 6, "blms");

            return Json(returnResult, JsonRequestBehavior.AllowGet);
        }
	}
}