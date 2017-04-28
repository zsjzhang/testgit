using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.FrontWeb.Models;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Threading.Tasks;

namespace Vcyber.BLMS.FrontWeb.Controllers
{
    /// <summary>
    /// 用户注册
    /// </summary>
    public class UserRegisterController : Controller
    {
        #region ==== 构造函数 ====

        public UserRegisterController() { }

        #endregion

        #region ==== 属性 ====

        private ApplicationUserManager _userManager;


        private ApplicationSignInManager _signInManager;

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
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set { _signInManager = value; }
        }


        #endregion

        #region ==== 公共方法 ====

        // GET: UserRegister
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 注册用户信息
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Register(RegisterInfo data)
        {
            string message=string.Empty;

            if (data==null||!data.ValidatePara(out message))
            {
                return DataResult.CreateData(EExecuteStatus.Fail,message);
            }

            var user = new ApplicationUser { UserName = data.userName, PhoneNumber = data.userName, IdentityNumber = data.identityNumber, Mid = data.identityNumber, RealName = data.realName, PasswordHash = data.password, PhoneNumberConfirmed = true, Email = "vcyber@vcyber.com" };
            var result = await UserManager.CreateAsync(user, data.password);
            
            if (result.Succeeded)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                return DataResult.CreateData(EExecuteStatus.Success, "注册成功。");
            }

            return DataResult.CreateData(EExecuteStatus.Fail, "注册异常。");
        }

        /// <summary>
        /// 获取短信验证码
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SMSCode(string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber))
            {
                return DataResult.CreateData(EExecuteStatus.Fail, "请输入手机号。");
            }

            int validateCount = _AppContext.ValidateCodeApp.GetCount(phoneNumber);

            if (validateCount > 8)
            {
                return DataResult.CreateData(EExecuteStatus.Fail, "当天获取短信验证码次数超出。");
            }
            else
            {
                _AppContext.ValidateCodeApp.Add(phoneNumber);
                return DataResult.CreateData(EExecuteStatus.Success, "验证码已经发送");
            }
        }

        /// <summary>
        /// 验证短信验证码
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <param name="smsCode"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ValidateCode(string phoneNumber, string smsCode)
        {
            if (string.IsNullOrEmpty(phoneNumber) || string.IsNullOrEmpty(smsCode))
            {
                return DataResult.CreateData(EExecuteStatus.Fail, "请输入手机号和短信验证码。");
            }

            bool result = _AppContext.ValidateCodeApp.IsSuccess(phoneNumber, smsCode, 5);
            string message = result ? "验证通过。" : "验证码已经失效。";
            return DataResult.CreateData(result?EExecuteStatus.Success:EExecuteStatus.Fail,message);
        }

        /// <summary>
        /// 验证用户名称是否重复
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult IsUserName(string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber))
            {
                return DataResult.CreateData(EExecuteStatus.Fail,"手机号不能为空。");
            }

           bool result= _AppContext.UserInfoApp.IsName(phoneNumber);
           string message = result ? "手机号已经注册。" : "手机号可以使用。";
           return DataResult.CreateData(result?EExecuteStatus.Fail:EExecuteStatus.Success,message);
        }

        /// <summary>
        /// 验证身份证编号是否重复
        /// </summary>
        /// <param name="identityNumber"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult IsIdentityNumber(string identityNumber)
        {
            if (string.IsNullOrEmpty(identityNumber))
            {
                return DataResult.CreateData(EExecuteStatus.Fail,"身份证编号不能为空。");
            }

            bool result = _AppContext.UserInfoApp.IsIdentityNumber(identityNumber);
            string message = result ? "身份证编号已经注册" : "身份编号可以使用";
            return DataResult.CreateData(result ? EExecuteStatus.Fail : EExecuteStatus.Success, message);
        }

        #endregion
    }
}