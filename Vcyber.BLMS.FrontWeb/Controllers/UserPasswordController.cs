using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.FrontWeb.Models;

using AspNet.Identity.SQL;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace Vcyber.BLMS.FrontWeb.Controllers
{
    public class UserPasswordController : Controller
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
        // GET: /UserPassword/FindPassword
        public ActionResult FindLoginPassword()
        {
            return View();
        }

        public ActionResult FindPayPassword()
        {
            return View();
        }

        //重置密码
        [HttpPost]
        public ActionResult ResetLoginPassword(ResetPWViewModel pwModel)
        {
            ReturnResult result = new ReturnResult { IsSuccess = true };

            var errors = new Dictionary<string, string>();

            if (!ModelState.IsValid)
            {
                if (ModelState["LoginPassword"].Errors.Count() > 0)
                    errors["LoginPassword"] = "请按格式输入正确的密码";

                if (ModelState["ConfirmLoginPassword"].Errors.Count() > 0)
                    errors["ConfirmLoginPassword"] = "请输入正确的确认密码";

                result.IsSuccess = false;
                result.Message = "输入数据验证未通过";
            }
            else
            {
                var user = UserManager.FindById(pwModel.UserGuid);

                if (user != null)
                {
                    var res = UserManager.ChangePassword(pwModel.UserGuid, user.PasswordHash, pwModel.LoginPassword);
                    if (!res.Succeeded)
                    {
                        result.IsSuccess = false;
                        result.Message = "密码修改失败";
                    }
                }else
                {
                    result.IsSuccess = false;
                    result.Message = "未找到用户";
                }
            }

            //附加错误信息
            result.Errors = errors;

            return Json(result, JsonRequestBehavior.AllowGet);
        }
	}
}