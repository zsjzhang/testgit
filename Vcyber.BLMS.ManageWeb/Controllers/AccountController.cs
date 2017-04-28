using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Vcyber.BLMS.ManageWeb.Models;
using System.Collections.Generic;
using AspNet.Identity.SQL;
using Vcyber.BLMS.ManageWeb;
using System.Text;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Domain;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.ManageWeb.Controllers
{
    public class AccountController : Controller
    {
        private ApplicationUserManager _userManager;

        public AccountController()
        {
                 

        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

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

        private ApplicationRoleManager _roleManager;
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]        
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        private ApplicationSignInManager _signInManager;

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set { _signInManager = value; }
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {

            //if (!ModelState.IsValid)
            //{
            //    return View(model);
            //}

            // 这不会计入到为执行帐户锁定而统计的登录失败次数中
            // 若要在多次输入错误密码的情况下触发帐户锁定，请更改为 shouldLockout: true
            if (string.IsNullOrEmpty(returnUrl))
            {
                returnUrl = "/Home/Index";
            }
            byte[] bytes = Convert.FromBase64String(model.Password);
            string pwd = Encoding.UTF8.GetString(bytes);

            model.Password = pwd;
            int code = 0;
            var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, shouldLockout: false);
            //var loginCookie= Request.Cookies["ASP.NET_SessionId"] ;
            //loginCookie.Secure = Request.IsSecureConnection;
            //Response.SetCookie(loginCookie);
            switch (result)
            {
                case SignInStatus.Success:
                    
                    //_AppContext.LoginMemRecordApp.ManagerAdd(this.HttpContext.User.Identity.GetUserId(), model.UserName);
                    /*
                     上面这个方法调用会报错，由于是异步请求登录，此时this.HttpContext.User.Identity.GetUserId()获取不到值，添加LoginMemRecordApp_Add方法解决了此问题
                     */
                    code = 0;
                    break;
                case SignInStatus.RequiresVerification:
                    code = 1;
                    returnUrl = "Account/SendCode";
                    break;
                case SignInStatus.LockedOut:
                case SignInStatus.Failure:
                    code = 2;
                    break;
                default:
                    code = 3;
                    break;
            }
            var json = new { code = code, url = returnUrl };
            return Json(json, JsonRequestBehavior.AllowGet);

            //switch (result)
            //{
            //    case SignInStatus.Success:
            //        return RedirectToLocal(returnUrl);
            //    case SignInStatus.LockedOut:
            //        return View("Lockout");
            //    case SignInStatus.RequiresVerification:
            //        return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
            //    case SignInStatus.Failure:
            //    default:
            //        //ModelState.AddModelError("", "无效的登录尝试。");
            //        return View(model);
            //}

        }
        /// <summary>
        /// 添加登录记录(登录成功后调用)
        /// </summary>
        public void LoginMemRecordApp_Add()
        {
            _AppContext.LoginMemRecordApp.ManagerAdd(this.HttpContext.User.Identity.GetUserId(), this.HttpContext.User.Identity.GetUserName());
        }
        ////
        //// GET: /Account/VerifyCode
        //[AllowAnonymous]
        //public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        //{
        //    // 要求用户已通过使用用户名/密码或外部登录名登录
        //    if (!await SignInManager.HasBeenVerifiedAsync())
        //    {
        //        return View("Error");
        //    }
        //    var user = await UserManager.FindByIdAsync(await SignInManager.GetVerifiedUserIdAsync());
        //    if (user != null)
        //    {
        //        var code = await UserManager.GenerateTwoFactorTokenAsync(user.Id, provider);
        //    }
        //    return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        //}

        ////
        //// POST: /Account/VerifyCode
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);
        //    }

        //    // 以下代码可以防范双重身份验证代码遭到暴力破解攻击。
        //    // 如果用户输入错误代码的次数达到指定的次数，则会将
        //    // 该用户帐户锁定指定的时间。
        //    // 可以在 IdentityConfig 中配置帐户锁定设置
        //    var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent:  model.RememberMe, rememberBrowser: model.RememberBrowser);
        //    switch (result)
        //    {
        //        case SignInStatus.Success:
        //            return RedirectToLocal(model.ReturnUrl);
        //        case SignInStatus.LockedOut:
        //            return View("Lockout");
        //        case SignInStatus.Failure:
        //        default:
        //            ModelState.AddModelError("", "代码无效。");
        //            return View(model);
        //    }
        //}

        ////
        //// GET: /Account/Register
        //[AllowAnonymous]
        //public ActionResult Register()
        //{
        //    return View();
        //}

        ////
        //// POST: /Account/Register
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Register(RegisterViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
        //        var result = await UserManager.CreateAsync(user, model.Password);
        //        if (result.Succeeded)
        //        {
        //            await SignInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false);

        //            // 有关如何启用帐户确认和密码重置的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkID=320771
        //            // 发送包含此链接的电子邮件
        //            // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
        //            // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
        //            // await UserManager.SendEmailAsync(user.Id, "确认你的帐户", "请通过单击 <a href=\"" + callbackUrl + "\">這裏</a>来确认你的帐户");

        //            return RedirectToAction("Index", "Home");
        //        }
        //        AddErrors(result);
        //    }

        //    // 如果我们进行到这一步时某个地方出错，则重新显示表单
        //    return View(model);
        //}

        ////
        //// GET: /Account/ConfirmEmail
        //[AllowAnonymous]
        //public async Task<ActionResult> ConfirmEmail(string userId, string code)
        //{
        //    if (userId == null || code == null)
        //    {
        //        return View("Error");
        //    }
        //    var result = await UserManager.ConfirmEmailAsync(userId, code);
        //    return View(result.Succeeded ? "ConfirmEmail" : "Error");
        //}

        ////
        //// GET: /Account/ForgotPassword
        //[AllowAnonymous]
        //public ActionResult ForgotPassword()
        //{
        //    return View();
        //}

        ////
        //// POST: /Account/ForgotPassword
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = await UserManager.FindByNameAsync(model.Email);
        //        if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
        //        {
        //            // 请不要显示该用户不存在或者未经确认
        //            return View("ForgotPasswordConfirmation");
        //        }

        //        // 有关如何启用帐户确认和密码重置的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkID=320771
        //        // 发送包含此链接的电子邮件
        //        // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
        //        // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
        //        // await UserManager.SendEmailAsync(user.Id, "重置密码", "请通过单击 <a href=\"" + callbackUrl + "\">此处</a>来重置你的密码");
        //        // return RedirectToAction("ForgotPasswordConfirmation", "Account");
        //    }

        //    // 如果我们进行到这一步时某个地方出错，则重新显示表单
        //    return View(model);
        //}

        ////
        //// GET: /Account/ForgotPasswordConfirmation
        //[AllowAnonymous]
        //public ActionResult ForgotPasswordConfirmation()
        //{
        //    return View();
        //}

        ////
        //// GET: /Account/ResetPassword
        //[AllowAnonymous]
        //public ActionResult ResetPassword(string code)
        //{
        //    return code == null ? View("Error") : View();
        //}

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [MvcAuthorize]
        public ActionResult ResetPassword(ResetPasswordByAdminViewModel model)
        {
            var response = new { IsSuccess = false, message = "" };
            if (!ModelState.IsValid)
            {
                var message = string.Empty;
                foreach (var value in ModelState.Values)
                {
                    if (value.Errors != null && value.Errors.Count > 0)
                    {
                        foreach (var error in value.Errors)
                        {

                            message += error.ErrorMessage + ".";
                        }
                    }
                }
                response = new { IsSuccess = false, message = message };
                return Json(response);
            }

            ApplicationUser user = UserManager.FindById(model.Id);
            if (user == null)
            {
                // 请不要显示该用户不存在
                response = new { IsSuccess = false, message = "系统中不存在此账户" };
            }
            String hashedNewPassword = UserManager.PasswordHasher.HashPassword(model.Password);
            UserStore<ApplicationUser> store = new UserStore<ApplicationUser>();
            store.SetPasswordHashAsync(user, hashedNewPassword);
            var result = UserManager.Update(user);

            if (result.Succeeded)
            {
                response = new { IsSuccess = true, message = "重置密码成功" };
            }
            else
                response = new { IsSuccess = false, message = "重置密码失败" };
            return Json(response);
        }

        ////
        //// GET: /Account/ResetPasswordConfirmation
        //[AllowAnonymous]
        //public ActionResult ResetPasswordConfirmation()
        //{
        //    return View();
        //}

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // 请求重定向到外部登录提供程序
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // 生成令牌并发送该令牌
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // 如果用户已具有登录名，则使用此外部登录提供程序将该用户登录
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // 如果用户没有帐户，则提示该用户创建帐户
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // 从外部登录提供程序获取有关用户的信息
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            if (Request.Cookies["LoginFlag"] != null)
            {
                //var rck = Request.Cookies.Get("LoginFlag");
                //rck.Expires = DateTime.Now.Add(new TimeSpan(0, 0, 0, 0));
                //Response.Cookies.Remove("LoginFlag");
                //Request.Cookies.Add(rck);
                Response.Cookies["LoginFlag"].Expires = DateTime.Now.AddDays(-1);
            }
            AuthenticationManager.SignOut();
            return RedirectToAction("Login", "Account", new { returnUrl = "" });
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        public ActionResult UpdatePwd()
        {
            return View();
        }

        [HttpPost]
        [MvcAuthorize]
        public ActionResult UpdatePwd(ResetPasswordViewModel pwModel)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "输入格式错误！" });
            }

            var result = UserManager.ChangePassword(User.Identity.GetUserId(), pwModel.OldPassword, pwModel.Password);

            if (result.Succeeded)
            {
                return Json(new { success = true, message = "重置密码成功！" });
            }


            var message = "";
            foreach (var error in result.Errors)
            {
                if (error.StartsWith("Passwords"))
                {
                    message += "密码必须为6位以上数字和字母组合";
                    continue;
                }

                message += error;
            }
            return Json(new { success = false, message });
        }
        #region 帮助程序
        // 用于在添加外部登录名时提供 XSRF 保护
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion


    }
}