using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Text.RegularExpressions;
using Discuz.Toolkit;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Vcyber.BLMS.FrontWeb.Models;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Common;
using AspNet.Identity.SQL;
using System.Security.Cryptography;
using System.Configuration;
using Vcyber.BLMS.Common.Web;

namespace Vcyber.BLMS.FrontWeb.Controllers
{
    public class AccountController : Controller
    {

        #region ==== 私有字段 ====

        private ApplicationUserManager _userManager;

        private ApplicationSignInManager _signInManager;

        #endregion

        #region ==== 公共属性 ====

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

        #region ==== 构造函数 ====

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        #endregion

        #region ==== 公共方法 ====

        /// <summary>
        /// 登录页面
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        /// <summary>
        /// form表单post登录请求
        /// </summary>
        /// <param name="model"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    //SignInStatus result;

                    //第一：确认验证码
                    var validateCode = string.Empty;

                    if (Session["ValidateCode"] != null)
                        validateCode = Session["ValidateCode"].ToString();

                    if (!validateCode.Equals(model.Captcha))
                    {
                        return Json(new { url = "/Account/LogonPage", msg = "验证码输入有误" });
                        //return RedirectToAction("LogonPage", "Account", new { returnUrl = returnUrl });
                    }

                    #region ==== 原始逻辑 ====

                    ////第二：进行username校验
                    //ApplicationUser _logonUserEntity = UserManager.FindByName(model.UserName);
                    //if (_logonUserEntity == null || string.IsNullOrEmpty(_logonUserEntity.Id))
                    //{
                    //    return RedirectToAction("LogonPage", "Account", new { returnUrl = returnUrl });
                    //}
                    //if (_logonUserEntity.IsNeedModifyPw == 1)
                    //{
                    //    return RedirectToAction("ResetPasswd", "Account");
                    //}

                    //// 这不会计入到为执行帐户锁定而统计的登录失败次数中
                    //// 若要在多次输入错误密码的情况下触发帐户锁定，请更改为 shouldLockout: true
                    //result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, shouldLockout: false);

                    //switch (result)
                    //{
                    //    case SignInStatus.Success:
                    //        {
                    //            int _value = 0;
                    //            _AppContext.BreadApp.BlueBeanBread(EBRuleType.登陆, _logonUserEntity.Id, (MemshipLevel)_logonUserEntity.MLevel, out _value);
                    //            _AppContext.BreadApp.EmpiricBread(EEmpiricRule.登陆, _logonUserEntity.Id, out _value);
                    //            if (string.IsNullOrEmpty(returnUrl))
                    //            {
                    //                return RedirectToAction("Default", "Home");
                    //            }
                    //            return Redirect(returnUrl);
                    //        }
                    //    case SignInStatus.LockedOut:
                    //    case SignInStatus.RequiresVerification:
                    //    case SignInStatus.Failure:
                    //    default:
                    //        ModelState.AddModelError("", "无效的登录尝试。");
                    //        return RedirectToAction("LogonPage", "Account", new { returnUrl = returnUrl });
                    //}

                    #endregion

                    #region ==== 新逻辑 ====

                    ApplicationUser applicationUser;
                    var result = this.LoginFactory(model, out applicationUser) ? SignInStatus.Success : (SignInStatus)12;

                    switch (result)
                    {
                        case SignInStatus.Success:
                            {

                                if (applicationUser.IsNeedModifyPw == 1)
                                {
                                    return Json(new { url = "/Account/ResetPasswd" });
                                    //return RedirectToAction("ResetPasswd", "Account");
                                }

                                int _value = 0;
                                SignInManager.SignInAsync(applicationUser, model.RememberMe, rememberBrowser: false);
                                //_AppContext.BreadApp.BlueBeanBread(EBRuleType.登陆, applicationUser.Id, (MemshipLevel)applicationUser.MLevel, out _value);
                                //_AppContext.BreadApp.EmpiricBread(EEmpiricRule.登陆, applicationUser.Id, out _value);
                                _AppContext.LoginMemRecordApp.Add(applicationUser.Id, applicationUser.NickName, EDataSource.blms);

                                //BBS
                                BBSUtil.CheckAndCreateDefaultBBSMember(applicationUser);

                                if (string.IsNullOrEmpty(returnUrl))
                                {
                                    return Json(new { url = "/Home/Default" });
                                    //return RedirectToAction("Default", "Home");
                                }
                                return Json(new { url = returnUrl });
                                //return Redirect(returnUrl);
                            }
                        case SignInStatus.LockedOut:
                        case SignInStatus.RequiresVerification:
                        case SignInStatus.Failure:
                        default:
                            ModelState.AddModelError("", "无效的登录尝试。");
                            return Json(new { url = "/Account/LogonPage", msg = "用户名或密码错误" });
                        //return RedirectToAction("LogonPage", "Account", new { returnUrl = returnUrl });
                    }

                    #endregion
                }

                // 如果我们进行到这一步时某个地方出错，则重新显示表单
                return Json(new { url = "/Account/LogonPage", msg = "参数错误" });
                //return RedirectToAction("LogonPage", "Account", new { returnUrl = returnUrl });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return Json(new { url = "/Account/LogonPage", msg = "系统异常" });
                //return RedirectToAction("LogonPage", "Account", new { returnUrl = returnUrl });
            }

        }


        /// <summary>
        /// form表单post登录请求(wap移动端登陆界面)
        /// </summary>
        /// <param name="model"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> WapLogin(LoginViewModel model, string returnUrl)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //第一：确认验证码
                    var validateCode = string.Empty;

                    if (Session["ValidateCode"] != null)
                        validateCode = Session["ValidateCode"].ToString();

                    if (!validateCode.Equals(model.Captcha))
                    {
                        return RedirectToAction("WapLogin", "WapQuestionnaire", new { returnUrl = returnUrl });
                    }

                    #region ==== 新逻辑 ====

                    ApplicationUser applicationUser;
                    var result = this.LoginFactory(model, out applicationUser) ? SignInStatus.Success : (SignInStatus)12;

                    switch (result)
                    {
                        case SignInStatus.Success:
                            {

                                if (applicationUser.IsNeedModifyPw == 1)
                                {
                                    return RedirectToAction("ResetPasswd", "Account");
                                }

                                int _value = 0;
                                SignInManager.SignInAsync(applicationUser, model.RememberMe, rememberBrowser: false);
                                //_AppContext.BreadApp.BlueBeanBread(EBRuleType.登陆, applicationUser.Id, (MemshipLevel)applicationUser.MLevel, out _value);
                                //_AppContext.BreadApp.EmpiricBread(EEmpiricRule.登陆, applicationUser.Id, out _value);
                                _AppContext.LoginMemRecordApp.Add(applicationUser.Id, applicationUser.NickName, EDataSource.blms);

                                //BBS
                                BBSUtil.CheckAndCreateDefaultBBSMember(applicationUser); //new

                                if (string.IsNullOrEmpty(returnUrl))
                                {
                                    return RedirectToAction("Default", "Home");
                                }

                                return Redirect(returnUrl);
                            }
                        case SignInStatus.LockedOut:
                        case SignInStatus.RequiresVerification:
                        case SignInStatus.Failure:
                        default:
                            ModelState.AddModelError("", "无效的登录尝试。");
                            return RedirectToAction("WapLogin", "WapQuestionnaire", new { returnUrl = returnUrl });
                    }

                    #endregion
                }

                // 如果我们进行到这一步时某个地方出错，则重新显示表单
                return RedirectToAction("WapLogin", "WapQuestionnaire", new { returnUrl = returnUrl });
            }
            catch (Exception ex)
            {
                return RedirectToAction("WapLogin", "WapQuestionnaire", new { returnUrl = returnUrl });
            }

        }


        /// <summary>
        /// ajax提交登录请求
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> DoLogin(LoginViewModel model)
        {
            try
            {
                #region 测试临时注释
                if (!ModelState.IsValid) return Json(new { code = 400, msg = "登录失败" });


                //确认验证码
                var validateCode = string.Empty;

                if (Session["ValidateCode"] != null)
                    validateCode = Session["ValidateCode"].ToString();

                if (!validateCode.Equals(model.Captcha))
                {
                    return Json(new { code = 401, msg = "登录失败，请确认验证码" });
                }
                #endregion


                #region ==== 原始逻辑 ====

                //ApplicationUser logonUserEntity = UserManager.FindByName(model.UserName);
                //if (logonUserEntity == null || string.IsNullOrEmpty(logonUserEntity.Id))
                //{
                //    return Json(new { code = 402, msg = "登录失败，账号或密码有误" });
                //}
                //if (logonUserEntity.IsNeedModifyPw == 1)
                //{
                //    int _value = 0;
                //    _AppContext.BreadApp.BlueBeanBread(EBRuleType.登陆, logonUserEntity.Id, (MemshipLevel)logonUserEntity.MLevel, out _value);
                //    _AppContext.BreadApp.EmpiricBread(EEmpiricRule.登陆, logonUserEntity.Id, out _value);
                //    return Json(new { code = 300, msg = "请先重置密码" });
                //}

                //// 这不会计入到为执行帐户锁定而统计的登录失败次数中
                //// 若要在多次输入错误密码的情况下触发帐户锁定，请更改为 shouldLockout: true
                //var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, shouldLockout: false);
                ////var result = this.LoginFactory(model) ? SignInStatus.Success : (SignInStatus)12;

                //switch (result)
                //{
                //    case SignInStatus.Success:
                //        {
                //            int _value = 0;
                //            _AppContext.BreadApp.BlueBeanBread(EBRuleType.登陆, logonUserEntity.Id, (MemshipLevel)logonUserEntity.MLevel, out _value);
                //            _AppContext.BreadApp.EmpiricBread(EEmpiricRule.登陆, logonUserEntity.Id, out _value);
                //            //BBS
                //            new BBSMemberController().CheckAndCreateDefaultBBSMember(logonUserEntity);
                //            return Json(new { code = 200, msg = "登录成功" });
                //        }
                //    case SignInStatus.LockedOut:
                //    //  return View("Lockout");
                //    case SignInStatus.RequiresVerification:
                //    //return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                //    case SignInStatus.Failure:
                //    default:
                //        ModelState.AddModelError("", "无效的登录尝试。");
                //        return Json(new { code = 400, msg = "登录失败" });
                //}

                #endregion

                #region ==== 新逻辑 ====

                ApplicationUser applicationUser;
                var result = this.ReLoginStatus(model, out applicationUser);

                switch (result)
                {
                    case LoginUserStatus.NeedModfiyPassWord:
                        return Json(new { code = 300, msg = "请先重置密码" });
                    case LoginUserStatus.Sucessed:
                        _AppContext.LoginMemRecordApp.Add(applicationUser.Id, applicationUser.NickName, EDataSource.blms);
                        SignInManager.SignInAsync(applicationUser, model.RememberMe, rememberBrowser: false);
                        BBSUtil.CheckAndCreateDefaultBBSMember(applicationUser); //new
                        _AppContext.UserMessageRecordApp.InsertLoginChangePasswordMessage(applicationUser.Id);

                        var returnIntegralType = (int)_AppContext.CarServiceUserApp.GetReIntegralTypeByIdentity(applicationUser.IdentityNumber);
                        if (_AppContext.LoginMemRecordApp.IsReMemberShipRequest(applicationUser.Id) && returnIntegralType != -1 && applicationUser.IsPay != 2)
                        {
                            return Json(new { code = 200, msg = "登录成功", isrequest = 1, returnIntegralType = returnIntegralType, identityNumber = applicationUser.IdentityNumber, systemmtype = applicationUser.SystemMType });
                        }
                        else
                        {
                            return Json(new { code = 200, msg = "登录成功", isrequest = 0, identityNumber = applicationUser.IdentityNumber, systemmtype = applicationUser.SystemMType });
                        }
                    //return Json(new { code = 200, msg = "登录成功" }); 

                    case LoginUserStatus.Failed:
                        return Json(new { code = 400, msg = "登录失败" });
                    case LoginUserStatus.NameOrPassWordError:
                        return Json(new { code = 901, msg = "用户名或密码错误" });
                    case LoginUserStatus.NoExistsUser:
                        return Json(new { code = 902, msg = "用户名或密码错误"  });
                    default:
                        ModelState.AddModelError("", "无效的登录尝试。");
                        return Json(new { code = 400, msg = "登录异常" });
                }

                #endregion

                // 如果我们进行到这一步时某个地方出错，则重新显示表单
                //return Json(new { code = 400, msg = "登录失败" });
            }
            catch (Exception ex)
            {
                return Json(new { code = 400, msg = ex.Message });
            }


        }


        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        ////
        //// POST: /Account/Register
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Register(RegisterViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = new ApplicationUser() { UserName = model.UserName };
        //        var result = await UserManager.CreateAsync(user, model.Password);
        //        if (result.Succeeded)
        //        {
        //            await SignInAsync(user, isPersistent: false);
        //            return RedirectToAction("Index", "Home");
        //        }
        //        else
        //        {
        //            AddErrors(result);
        //        }
        //    }

        //    // 如果我们进行到这一步时某个地方出错，则重新显示表单
        //    return View(model);
        //}

        /// <summary>
        /// form表单post注册请求
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // 有关如何启用帐户确认和密码重置的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkID=320771
                    // 发送包含此链接的电子邮件
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "确认你的帐户", "请通过单击 <a href=\"" + callbackUrl + "\">這裏</a>来确认你的帐户");

                    return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }

            // 如果我们进行到这一步时某个地方出错，则重新显示表单
            return View(model);
        }

        /// <summary>
        /// ajax提交post注册请求
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> DoRegister_Old(RegisterViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    foreach (ModelState state in ModelState.Values.Where(state => state.Errors.Count > 0))
                    {
                        return Json(new { code = 400, msg = state.Errors[0].ErrorMessage });
                    }

                    return Json(new { code = 400, msg = "参数错误" });
                }

                ReturnResult _captchaResult = _AppContext.UserSecurityApp.ValidateMobileVerifyCode(model.Mobile, model.Captcha);
                if (!_captchaResult.IsSuccess)
                {
                    return Json(new { code = 401, msg = "验证码失败" });
                }

                var user = new ApplicationUser
                {
                    PhoneNumber = model.Mobile,
                    UserName = model.Mobile,
                    NickName = model.NickName,
                    IdentityNumber = model.IdentityNumber,
                    MLevel = (int)MemshipLevel.OneStar,
                    MType = (int)model.MType,
                    ActiveWay = (int)MembershipActiveWay.ClientWeb
                };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {

                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    var gradeUser = UserManager.FindByName(model.Mobile);
                    //如果选择我不是车主，注册完毕登录成功后直接返回注册成功
                    if ((int)MembershipType.WhitoutCar == (int)model.MType)
                    {
                        //int _value = 0;
                        //_AppContext.BreadApp.BlueBeanBread(EBRuleType.注册, gradeUser.Id, (MemshipLevel)gradeUser.MLevel, out _value);
                        //_AppContext.BreadApp.EmpiricBread(EEmpiricRule.注册, gradeUser.Id, out _value);

                        //发送短信
                        _AppContext.SMSApp.SendSMS(ESmsType.注册成功, model.Mobile, new string[] { "" });

                        return Json(new { code = 200, msg = "注册成功" });
                    }



                    //获取用户车辆信息
                    var carList = _AppContext.CarServiceUserApp.SelectCarListByIdentity(model.IdentityNumber);
                    if (carList == null || carList.Count() <= 0)
                    {
                        UserManager.Update(gradeUser);

                        //int _value = 0;
                        //_AppContext.BreadApp.BlueBeanBread(EBRuleType.注册, gradeUser.Id, (MemshipLevel)gradeUser.MLevel, out _value);
                        //_AppContext.BreadApp.EmpiricBread(EEmpiricRule.注册, gradeUser.Id, out _value);

                        //发送短信
                        _AppContext.SMSApp.SendSMS(ESmsType.提交注册_未匹配到索九车, model.Mobile, new string[] { "" });

                        //提示未匹配到您的车辆，请拨打客服电话
                        return Json(new { code = 301, msg = CommonConst.NOCARTIP });
                    }
                    gradeUser.MLevel = GetUserLevel(gradeUser.IdentityNumber);
                    gradeUser.SystemMType = (int)MembershipType.WhitCar;
                    //校验索九车主
                    var _isSonata = _AppContext.SonataServiceApp.IsSonataUser(gradeUser.IdentityNumber);
                    //提示缴费
                    if (_isSonata)
                    {
                        gradeUser.SystemMType = (int)MembershipType.Sonata9;
                        gradeUser.IsPay = 0;
                        gradeUser.ApprovalStatus = (int)MembershipApprovalStatus.NeedPay;
                        UserManager.Update(gradeUser);

                        //int _value = 0;
                        //_AppContext.BreadApp.BlueBeanBread(EBRuleType.注册, gradeUser.Id, (MemshipLevel)gradeUser.MLevel, out _value);
                        //_AppContext.BreadApp.EmpiricBread(EEmpiricRule.注册, gradeUser.Id, out _value);
                        //跳转到缴费页面
                        //DO Something

                        //发送短信
                        _AppContext.SMSApp.SendSMS(ESmsType.提交注册_未匹配到索九车, model.Mobile, new string[] { "" });

                        return Json(new { code = 302, msg = "缴费页面" });

                    }

                    UserManager.Update(gradeUser);

                    //var _bluevalue = 0;
                    //_AppContext.BreadApp.BlueBeanBread(EBRuleType.注册, gradeUser.Id, (MemshipLevel)gradeUser.MLevel, out _bluevalue);
                    //_AppContext.BreadApp.EmpiricBread(EEmpiricRule.注册, gradeUser.Id, out _bluevalue);

                    //发送短信
                    _AppContext.SMSApp.SendSMS(ESmsType.注册成功, model.Mobile, new string[] { "" });

                    return Json(new { code = 200, msg = "注册成功" });
                }
                AddErrors(result);

                var errorMsg = result.Errors.FirstOrDefault();
                if (errorMsg != null && errorMsg.StartsWith("Passwords")) errorMsg = "密码必须为6位以上数字和字母组合";
                if (errorMsg != null && errorMsg.StartsWith("Name")) errorMsg = "您的手机号已经注册过";

                // 如果我们进行到这一步时某个地方出错，则重新显示表单
                return Json(new { code = 400, msg = string.Format("{0}", errorMsg) });
            }
            catch (Exception ex)
            {
                return Json(new { code = 400, msg = string.Format("{0}", ex.Message) });
            }

        }

        /// <summary>
        /// ajax提交post注册请求（新业务逻辑）
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> DoRegister(RegisterViewModel model)
        {
            try
            {
                //1、验证请求
                if (!ModelState.IsValid)
                {
                    foreach (ModelState state in ModelState.Values.Where(state => state.Errors.Count > 0))
                    {
                        return Json(new { code = 400, msg = state.Errors[0].ErrorMessage });
                    }
                    return Json(new { code = 400, msg = "参数错误" });
                }

                //2、验证手机验证码
                ReturnResult _captchaResult = _AppContext.UserSecurityApp.ValidateMobileVerifyCode(model.Mobile, model.Captcha);
                if (!_captchaResult.IsSuccess)
                {
                    return Json(new { code = 401, msg = "验证码错误或已过期，请重新获取" });
                }

                //3、获取注册信息
                var user = new ApplicationUser
                {
                    PhoneNumber = model.Mobile,
                    UserName = model.Mobile,
                    NickName = model.NickName,
                    IdentityNumber = model.IdentityNumber,
                    MLevel = (int)MemshipLevel.OneStar,
                    MType = (int)model.MType,
                    SystemMType = (int)MembershipType.WhitoutCar,
                    ActiveWay = (int)MembershipActiveWay.ClientWeb,
                    CreatedPerson = string.IsNullOrEmpty(model.Source) ? "blms" : model.Source
                };
                string _activedealerId = model.ActivedealerId;

                //4、对于公司客户的特殊处理逻辑
                if (!string.IsNullOrEmpty(model.customerType) && model.customerType == "2")//判断是否为集团客户
                {
                    //1. 根据vin从IF_CAR中找数据，如果找不到，失败。
                    if (!string.IsNullOrEmpty(model.VIN))
                    {
                        Car c = _AppContext.CarServiceUserApp.GetCarInfoByVIN(model.VIN);
                        if (c == null)
                        {
                            return Json(new { code = 400, msg = "输入的VIN码未匹配到车辆信息" });
                        }
                    }
                    else
                    {
                        return Json(new { code = 400, msg = "请输入VIN码" });
                    }
                    //2. 在IF_customer表中增加一条数据（custid,CustId规则：JT-110222199912013115（集团-身份证号)）
                    var parms = new Entity.Generated.IFCustomer();
                    parms.CustId = "JT-" + model.IdentityNumber;
                    parms.CustName = "--";
                    parms.CustMobile = model.Mobile;
                    parms.IdentityNumber = model.IdentityNumber;
                    parms.Email = model.Email;
                    parms.Gender = "男";
                    parms.Address = "--";
                    parms.City = "--";

                    Vcyber.BLMS.Entity.Generated.IFCustomer ifCus = _AppContext.CarServiceUserApp.CreateOrGetIFCustomer(parms);
                    //3. 修改IF_car表中车辆信息的custid
                    bool b = _AppContext.CarServiceUserApp.UpdateCarInfoByVIN(model.VIN, ifCus.CustId);
                    if (!b)
                    {
                        return Json(new { code = 400, msg = "输入的VIN码未匹配到车辆信息" });
                    }
                }

                //5、检验身份证号和VIN码是否匹配
                Task<FrontIdentityUser> cusObj = null;
                if (!string.IsNullOrEmpty(model.IdentityNumber))
                {
                    cusObj = new FrontUserStore<FrontIdentityUser>().FindByIdentityNumber(model.IdentityNumber);
                    if (cusObj != null && cusObj.Result != null && string.IsNullOrEmpty(model.VIN))
                    {
                        return Json(new { code = 402, msg = "身份证号已注册，请输入VIN码" });
                    }
                }

                //6、注册用户,如果用户存在则更新该用户信息
                IdentityResult result = null;
                //if (string.IsNullOrEmpty(model.VIN))
                if (!(cusObj != null && cusObj.Result != null))
                    result = await UserManager.CreateAsync(user, model.Password);
                else
                {
                    //校验VIN码和身份证号是否匹配
                    var cus = _AppContext.CarServiceUserApp.SelectCarListByIdentity(model.IdentityNumber);
                    if (cus.Where(s => s.VIN == model.VIN).Count() <= 0)
                    {
                        return Json(new { code = 400, msg = "VIN码和身份证号不匹配" });
                    }

                    //String hashedNewPassword = UserManager.PasswordHasher.HashPassword(model.Password);
                    //user.PasswordHash = hashedNewPassword;
                    //user.IsNeedModifyPw = 0;
                    //user.UpdateTime = DateTime.Now.ToString();

                    if (cusObj != null && cusObj.Result != null)
                    {
                        //user.Id = cusObj.Result.Id;
                        user = UserManager.FindById(cusObj.Result.Id);
                        user.PhoneNumber = model.Mobile;
                        user.UserName = model.Mobile;
                        user.UpdateTime = DateTime.Now.ToString();

                    }

                    result = UserManager.Update(user);
                }

                //7、注册完成跳转
                if (result != null && result.Succeeded)
                {

                    // 1)更新车主级别 
                    var gradeUser = UserManager.FindByName(model.Mobile);
                    var carList = _AppContext.CarServiceUserApp.SelectCarListByIdentity(model.IdentityNumber);
                    gradeUser.MLevel = GetUserLevel(gradeUser.IdentityNumber);
                    if (carList.Count() > 0)
                    {
                        gradeUser.SystemMType = (int)MembershipType.WhitCar;
                    }

                    // 2)校验索九车主/全新途胜车主
                    var _isSonata = _AppContext.SonataServiceApp.IsSonataUser(gradeUser.IdentityNumber);
                    if (_isSonata)
                    {
                        gradeUser.SystemMType = (int)MembershipType.Sonata9;
                    }

                    UserManager.Update(gradeUser);

                    // 3)索九车主
                    if ((int)MembershipType.Sonata9 == (int)model.MType)
                    {
                        gradeUser.ApprovalStatus = (int)MembershipApprovalStatus.Activing;
                        if (model.ActivityType == "1" && string.IsNullOrEmpty(model.PayNumber))
                        {
                            return Json(new { code = 400, msg = "选择天猫支付必须输入支付码" });
                        }
                        if (!string.IsNullOrEmpty(model.PayNumber))
                            gradeUser.PayNumber = model.PayNumber;
                        gradeUser.IsPay = 2;

                        // a)更新用户状态
                        UserManager.Update(gradeUser);

                        // b)向后台申请激活成为会员
                        var userStore = new FrontUserStore<FrontIdentityUser>();
                        userStore.AddMembershipDealerRecord(gradeUser.Id, _activedealerId);
                        userStore.CreateMembershipRequest(gradeUser.Id, model.IdentityNumber, _activedealerId, string.Empty, "blms");
                    }

                    try
                    {
                        //模拟登陆  
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    }
                    catch (Exception ex)
                    {
                        Vcyber.BLMS.Common.LogService.Instance.Debug("注册成功模拟登陆异常", ex);
                    }

                    // 4)设置用户的蓝豆值和经验值
                    //int _value = 0;
                    //_AppContext.BreadApp.BlueBeanBread(EBRuleType.注册, gradeUser.Id, (MemshipLevel)gradeUser.MLevel, out _value);
                    //_AppContext.BreadApp.EmpiricBread(EEmpiricRule.注册, gradeUser.Id, out _value);

                    // 5)发送短信
                    _AppContext.SMSApp.SendSMS(ESmsType.注册成功, model.Mobile, new string[] { "" });

                    //活动网站注册--赠送蓝豆
                    //if (model.returnUrl == ConfigurationManager.AppSettings["Reg_returnUrl"])
                    //{
                    //    _AppContext.BreadApp.BlueBeanBread(EBRuleType.活动网站注册, gradeUser.Id, (MemshipLevel)gradeUser.MLevel, out _value);
                    //}


                    return Json(new { code = 200, msg = "注册成功" });
                }
                AddErrors(result);

                var errorMsg = result.Errors.FirstOrDefault();
                if (errorMsg != null && errorMsg.StartsWith("Passwords")) errorMsg = "密码必须为8位以上数字和字母组合";
                if (errorMsg != null && errorMsg.StartsWith("Name")) errorMsg = "您的手机号已经注册过";

                // 如果我们进行到这一步时某个地方出错，则重新显示表单
                return Json(new { code = 400, msg = string.Format("{0}", errorMsg) });
            }
            catch (Exception ex)
            {
                return Json(new { code = 400, msg = string.Format("{0}", ex.Message) });
            }

        }



        #region  提取方法公用
        /// <summary>
        /// 根据车型返积分  可提取到commnon直接方便调用
        /// </summary>
        /// <param name="identity"></param>
        /// <returns>-1 ：不返积分 0：ds  1：ds增换  2：非ds 3：非ds增换购 </returns>
        private CarTypeReturnIntegral GetCarTypeByIdentity(string identity)
        {

            var listcar = _AppContext.CarServiceUserApp.SelectCarListByIdentity(identity).Where<Car>(c => c.BuyTime > DateTime.Parse("2016-04-01")).ToList();


            if (listcar != null && listcar.Count() == 1)
            {


                if (IsDsCarType(listcar[0].CarCategory))
                {
                    return CarTypeReturnIntegral.DS;
                }
                else
                {
                    return CarTypeReturnIntegral.NoDS;
                }
            }
            if (listcar != null && listcar.Count > 1)
            {
                var returnvalue = CarTypeReturnIntegral.NoDSAdd;
                foreach (Car car in listcar)
                {
                    if (IsDsCarType(car.CarCategory))
                    {
                        returnvalue = CarTypeReturnIntegral.DSAdd;
                    }
                }
                return returnvalue;
            }
            return CarTypeReturnIntegral.DS;

        }
        /// <summary>
        /// 方法需要重写
        /// </summary>
        /// <param name="carCategory"></param>
        /// <returns></returns>
        private bool IsDsCarType(string carCategory)
        {
            List<string> listCarType = new List<string>();
            listCarType.Add("索纳塔九");
            listCarType.Add("全新途胜");
            listCarType.Add("ix25");
            listCarType.Add("ix35");
            listCarType.Add("全新胜达");
            listCarType.Add("第九代索纳塔");
            listCarType.Add("名图");
            listCarType.Add("名驭");
            if (listCarType.Contains(carCategory))
            {
                return true;
            }
            else
            {
                return false;
            }

            // return _AppContext.DealerMembershipApp.IsDsCarTypeByIdNumber ()

        }

        #endregion
        [HttpPost]
        [AllowAnonymous]
        public ActionResult DoCheckUserName(string UserName)
        {
            var store = new AspNet.Identity.SQL.FrontUserStore<AspNet.Identity.SQL.FrontIdentityUser>();
            bool bolResult = store.CheckUserNameIsExist(UserName);
            return Json(bolResult == true ? 1 : 0);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult DoCheckIdentityNumber(string IdentityNumber)
        {
            var store = new AspNet.Identity.SQL.FrontUserStore<AspNet.Identity.SQL.FrontIdentityUser>();
            var cusObj = store.FindByIdentityNumber(IdentityNumber);
            if (cusObj != null && cusObj.Result != null)
                return Json(1);
            else
                return Json(0);
        }

        //private int GetUserLevel(IEnumerable<Car> carList)
        //{
        //    int _level = (int)MemshipLevel.OneStar;
        //    if (carList != null && carList.Any())
        //    {
        //        _level = (int)MemshipLevel.TwoStar;
        //        //确定用户级别 (索8/全新胜达/索9)

        //        var isLevel3 = carList.Any(e => e.CarCategory == CommonConst.SONATA8 || e.CarCategory == CommonConst.SONATA9 || e.CarCategory == CommonConst.SONATA9_1 || e.CarCategory == CommonConst.SONATA9_2 || e.CarCategory == CommonConst.SHENGDA || e.CarCategory == CommonConst.TLC);
        //        if (isLevel3)
        //            _level = (int)MemshipLevel.ThreeStar;
        //    }
        //    return _level;
        //}

        /// <summary>
        /// 会员定级 给注册的用户定级
        /// 根据身份证号查询注册用户车辆信息来定级
        /// </summary>
        /// <param name="IdentityNumber">身份证号</param>
        /// <returns></returns>
        private int GetUserLevel(string IdentityNumber)
        {
            int _level = (int)MemshipLevel.OneStar;
            string strlevel = _AppContext.DealerMembershipApp.GetFirstRegistMLevelByIdNumber(IdentityNumber);
            int.TryParse(strlevel, out  _level);
            return _level;

        }
        ///// <summary>
        ///// 是否是D+S
        ///// </summary>
        ///// <param name="IdentityNumber"></param>
        ///// <returns></returns>
        //private bool  isDS(string IdentityNumber)
        //{
        //    return true;
        //}



        //
        // POST: /Account/Disassociate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Disassociate(string loginProvider, string providerKey)
        {
            ManageMessageId? message = null;
            //IdentityResult result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            IdentityResult result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), null);
            if (result.Succeeded)
            {
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("Manage", new { Message = message });
        }

        //
        // GET: /Account/Manage
        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "你的密码已更改。"
                : message == ManageMessageId.SetPasswordSuccess ? "已设置你的密码。"
                : message == ManageMessageId.RemoveLoginSuccess ? "已删除外部登录名。"
                : message == ManageMessageId.Error ? "出现错误。"
                : "";
            ViewBag.HasLocalPassword = HasPassword();
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        //
        // POST: /Account/Manage
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Manage(ManageUserViewModel model)
        //{
        //    bool hasPassword = HasPassword();
        //    ViewBag.HasLocalPassword = hasPassword;
        //    ViewBag.ReturnUrl = Url.Action("Manage");
        //    if (hasPassword)
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
        //            if (result.Succeeded)
        //            {
        //                return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
        //            }
        //            else
        //            {
        //                AddErrors(result);
        //            }
        //        }
        //    }
        //    else
        //    {
        //        // User does not have a password so remove any validation errors caused by a missing OldPassword field
        //        ModelState state = ModelState["OldPassword"];
        //        if (state != null)
        //        {
        //            state.Errors.Clear();
        //        }

        //        if (ModelState.IsValid)
        //        {
        //            IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
        //            if (result.Succeeded)
        //            {
        //                return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
        //            }
        //            else
        //            {
        //                AddErrors(result);
        //            }
        //        }
        //    }

        //    // 如果我们进行到这一步时某个地方出错，则重新显示表单
        //    return View(model);
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
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var user = await UserManager.FindAsync(loginInfo.Login);
            if (user != null)
            {
                await SignInAsync(user, isPersistent: false);
                return RedirectToLocal(returnUrl);
            }
            else
            {
                // If the user does not have an account, then prompt the user to create an account
                ViewBag.ReturnUrl = returnUrl;
                ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.DefaultUserName });
            }
        }

        //
        // POST: /Account/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new ChallengeResult(provider, Url.Action("LinkLoginCallback", "Account"), User.Identity.GetUserId());
        }

        //
        // GET: /Account/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
            }
            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            if (result.Succeeded)
            {
                return RedirectToAction("Manage");
            }
            return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
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
                return RedirectToAction("Manage");
            }

            if (ModelState.IsValid)
            {
                // 从外部登录提供程序获取有关用户的信息
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser() { UserName = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInAsync(user, isPersistent: false);
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
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            if (Request.Cookies["dnt"] != null)
            {
                LogService.Instance.Info("cookies:" + Request.Cookies["dnt"].Value);
                Request.Cookies["dnt"].Expires = DateTime.Now.AddDays(-1);
                Request.Cookies.Remove("dnt");

                Response.Cookies["dnt"].Expires = DateTime.Now.AddDays(-1);
                Response.Cookies["dnt"].Value = string.Empty;
                Response.Cookies["dnt"].Secure = Request.IsSecureConnection;
            }

            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Default", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult RemoveAccountList()
        {
            var linkedAccounts = UserManager.GetLogins(User.Identity.GetUserId());
            ViewBag.ShowRemoveButton = HasPassword() || linkedAccounts.Count > 1;
            return (ActionResult)PartialView("_RemoveAccountPartial", linkedAccounts);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && UserManager != null)
            {
                UserManager.Dispose();
                UserManager = null;
            }
            base.Dispose(disposing);
        }

        #region 帮助程序
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            var identity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            Error
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "UserHome");
            }
        }

        private class ChallengeResult : HttpUnauthorizedResult
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
                var properties = new AuthenticationProperties() { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion

        #endregion

        /// <summary>
        /// 首页登录模块
        /// </summary>
        /// <returns></returns>
        public ActionResult HomeLogon()
        {
            return View();
        }

        /// <summary>
        /// 首页登录模块成功展示
        /// </summary>
        /// <returns></returns>
        public ActionResult HomeLogonSuccess()
        {
            int _totalScore = 0;
            // int _totalBlueBean = 0;
            //int _totalEmpiric = 0;
            if (this.User.Identity.IsAuthenticated)
            {
                _totalScore = Vcyber.BLMS.Application._AppContext.UserIntegralApp.GetTotalIntegral(this.User.Identity.GetUserId());
                //_totalBlueBean = Vcyber.BLMS.Application._AppContext.UserBlueBeanApp.GetTotalBlueBean(this.User.Identity.GetUserId());
                //_totalEmpiric = Vcyber.BLMS.Application._AppContext.UserEmpiricApp.TotalValue(this.User.Identity.GetUserId());
            }
            ApplicationUser _result = UserManager.FindById(this.User.Identity.GetUserId());
            ViewBag.totalScore = _totalScore;
            //ViewBag.totalBlueBean = _totalBlueBean;
            //ViewBag.totalEmpiric = _totalEmpiric;
            //UserInfo _result = _AppContext.UserInfoApp.GetOne(this.User.Identity.GetUserId());
            int UnReadMsgCount = 0;
            UnReadMsgCount = _AppContext.UserMessageRecordApp.GetUnReadMessage(this.User.Identity.GetUserId());
            ViewBag.UnReadMsg = UnReadMsgCount;
            return View(_result);
        }
        /// <summary>
        /// 首页登录模块成功展示
        /// </summary>
        /// <returns></returns>
        public ActionResult HomeLogonSuccesstwo()
        {
            int _totalScore = 0;
            // int _totalBlueBean = 0;
            //int _totalEmpiric = 0;
            if (this.User.Identity.IsAuthenticated)
            {
                _totalScore = Vcyber.BLMS.Application._AppContext.UserIntegralApp.GetTotalIntegral(this.User.Identity.GetUserId());
                //_totalBlueBean = Vcyber.BLMS.Application._AppContext.UserBlueBeanApp.GetTotalBlueBean(this.User.Identity.GetUserId());
                //_totalEmpiric = Vcyber.BLMS.Application._AppContext.UserEmpiricApp.TotalValue(this.User.Identity.GetUserId());
            }
            ApplicationUser _result = UserManager.FindById(this.User.Identity.GetUserId());
            ViewBag.totalScore = _totalScore;
            //ViewBag.totalBlueBean = _totalBlueBean;
            //ViewBag.totalEmpiric = _totalEmpiric;
            //UserInfo _result = _AppContext.UserInfoApp.GetOne(this.User.Identity.GetUserId());
            int UnReadMsgCount = 0;
            UnReadMsgCount = _AppContext.UserMessageRecordApp.GetUnReadMessage(this.User.Identity.GetUserId());
            ViewBag.UnReadMsg = UnReadMsgCount;
            return View(_result);
        }

        /// <summary>
        /// 登录页面
        /// </summary>
        /// <returns></returns>
        public ActionResult LogonPage(string returnUrl)
        {
            var cookieValue = CookieHelper.GetCookieValue("CustomCookie");
            if (!string.IsNullOrWhiteSpace(cookieValue) && cookieValue.ToLower().Contains("webinspect"))
            {
                return Redirect("/Contents/error.htm");
            }

            ViewBag.returnUrl = returnUrl;
            return View();
        }


        /// <summary>
        /// 页面注册
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AccountRegister(string returnUrl, string source)
        {
            ViewBag.returnUrl = returnUrl;
            ViewBag.source = source;
            return View();
        }

        /// <summary>
        /// 免责声明
        /// </summary>
        /// <returns></returns>
        public ActionResult RegisterStatement()
        {
            return View();
        }

        /// <summary>
        /// 重置密码（找回密码）页面
        /// </summary>
        /// <returns></returns>
        public ActionResult ResetPasswd(string source)
        {
            ViewBag.source = source;
            return View();
        }
        [HttpPost]
        public JsonResult DoResetPasswd(string userAccount, string passwd, string newpasswd, string captcha)
        {
            try
            {
                ReturnResult _captchaResult = _AppContext.UserSecurityApp.ValidateMobileVerifyCode(userAccount, captcha);
                if (!_captchaResult.IsSuccess)
                {
                    return Json(new { code = "400", msg = "验证码失败" });
                }

                ApplicationUser _userEntity = UserManager.FindByName(userAccount);
                if (_userEntity == null)
                {
                    return Json(new { code = "400", msg = "用户账号不存在" });
                }

                string userId = _userEntity.Id;
                //IdentityResult _result = UserManager.ChangePassword(userId, passwd, newpasswd);
                String hashedNewPassword = UserManager.PasswordHasher.HashPassword(newpasswd);
                var user = UserManager.FindById(userId);
                user.PasswordHash = hashedNewPassword;
                user.IsNeedModifyPw = 0;
                user.UpdateTime = DateTime.Now.ToString();
                IdentityResult _result = UserManager.Update(user);

                if (_result.Succeeded)
                {
                    //ApplicationUser _resetUserEntity = UserManager.FindById(userId);
                    //_resetUserEntity.IsNeedModifyPw = 0;
                    //_resetUserEntity.UpdateTime = DateTime.Now.ToString();
                    //UserManager.Update(_resetUserEntity);

                    int _value = 0;
                    _AppContext.BreadApp.EmpiricBread(EEmpiricRule.修改密码, user.Id, out _value);
                    return Json(new { code = "200", msg = "重置成功", });
                }
                else
                {
                    return Json(new { code = "400", msg = "重置失败" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { code = "400", msg = ex.Message });
            }

        }

        [HttpGet]
        public JsonResult DoPraise(string type, string id)
        {
            int _type = -1;
            int _id = -1;
            int.TryParse(type, out _type);
            int.TryParse(id, out _id);
            if (_type <= 0 || _id <= 0)
            {
                return Json(new { }, JsonRequestBehavior.AllowGet);
            }
            if (!User.Identity.IsAuthenticated)
            {
                return Json(new { }, JsonRequestBehavior.AllowGet);
            }
            ApplicationUser _curUser = UserManager.FindById(this.User.Identity.GetUserId());
            if (_curUser == null || string.IsNullOrEmpty(_curUser.Id))
            {
                return Json(new { }, JsonRequestBehavior.AllowGet);
            }
            _AppContext.ProductApp.Praise((EPraiseType)_type, _curUser.Id, (MemshipLevel)(_curUser.MLevel == 0 ? 1 : _curUser.MLevel), _id);
            return Json(new { code = "200" }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult IsParaise(string type, string id)
        {
            int _level = 1;
            int count = -1;
            int _type = -1;
            int _id = -1;
            int.TryParse(type, out _type);
            int.TryParse(id, out _id);
            if (_type <= 0 || _id <= 0)
            {
                return Json(new { code = "400", curCount = count, level = _level }, JsonRequestBehavior.AllowGet);
            }
            if (!User.Identity.IsAuthenticated)
            {
                return Json(new { code = "401", curCount = count, level = _level }, JsonRequestBehavior.AllowGet);
            }
            ApplicationUser _curUser = UserManager.FindById(this.User.Identity.GetUserId());
            if (_curUser == null || string.IsNullOrEmpty(_curUser.Id))
            {
                return Json(new { code = "401", curCount = count, level = _level }, JsonRequestBehavior.AllowGet);
            }
            _level = _curUser.MLevel <= 1 ? 1 : _curUser.MLevel;
            if (!_AppContext.ProductApp.IsParaise((EPraiseType)_type, _curUser.Id, _id, out count))
            {
                return Json(new { code = "201", curCount = count, level = _level }, JsonRequestBehavior.AllowGet);
                //return Json(new { code = "200", curCount = count, level = _level }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { code = "200", curCount = count, level = _level }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult PraiseCount(string type, string id)
        {
            int _type = -1;
            int _id = -1;
            int.TryParse(type, out _type);
            int.TryParse(id, out _id);
            if (_type <= 0 || _id <= 0)
            {
                return Json(new { code = "201" }, JsonRequestBehavior.AllowGet);
            }
            if (!User.Identity.IsAuthenticated)
            {
                return Json(new { code = "201" }, JsonRequestBehavior.AllowGet);
            }
            ApplicationUser _curUser = UserManager.FindById(this.User.Identity.GetUserId());
            if (_curUser == null || string.IsNullOrEmpty(_curUser.Id))
            {
                return Json(new { code = "201" }, JsonRequestBehavior.AllowGet);
            }

            int _praiseCount = _AppContext.ProductApp.PraiseCount((EPraiseType)_type, _id);
            return Json(new { code = "200", content = _praiseCount }, JsonRequestBehavior.AllowGet);
        }

        #region ==== 私有方法 ====

        private LoginUserStatus ReLoginStatus(LoginViewModel model, out ApplicationUser applicationUser, int loginType = 1)
        {
            applicationUser = new ApplicationUser();
            var userStor = new FrontUserStore<FrontIdentityUser>();
            var userDatas = userStor.FindLogin(model.UserName);
            if (userDatas == null || userDatas.Count == 0)
            {
                return LoginUserStatus.NoExistsUser;
            }

            foreach (var userData in userDatas)
            {
                if (loginType == 1)
                {

                    if (!string.IsNullOrEmpty(userData.PasswordHash))
                    {
                        bool result = VerifyHashedPassword(userData.PasswordHash, model.Password);

                        if (result)
                        {
                            if (userData != null)
                            {
                                if (userData.IsNeedModifyPw == 1)
                                {
                                    //  applicationUser = this.ConvertApplicationUser(userDatas[0]);
                                    return LoginUserStatus.NeedModfiyPassWord;
                                }

                                applicationUser = this.ConvertApplicationUser(userData);
                                return LoginUserStatus.Sucessed;
                            }
                        }
                        else
                        {

                            return LoginUserStatus.NameOrPassWordError;
                        }
                    }
                }
                else
                {
                    if (userData != null)
                    {
                        applicationUser = this.ConvertApplicationUser(userData);
                        return LoginUserStatus.Sucessed;
                    }
                }
            }

            return LoginUserStatus.Failed;
        }

        private bool LoginFactory(LoginViewModel model, out ApplicationUser applicationUser, int loginType = 1)
        {
            applicationUser = new ApplicationUser();
            var userStor = new FrontUserStore<FrontIdentityUser>();
            var userDatas = userStor.FindLogin(model.UserName);

            //if (userDatas.Count > 0)
            //{
            //    if (userDatas[0].IsNeedModifyPw == 1)
            //    {
            //        applicationUser = this.ConvertApplicationUser(userDatas[0]);
            //        return true;
            //    }
            //}



            foreach (var userData in userDatas)
            {
                if (loginType == 1)
                {

                    if (!string.IsNullOrEmpty(userData.PasswordHash))
                    {
                        bool result = VerifyHashedPassword(userData.PasswordHash, model.Password);

                        if (result)
                        {
                            if (userData != null)
                            {
                                applicationUser = this.ConvertApplicationUser(userData);
                                return true;
                            }
                        }
                    }
                }
                else
                {
                    if (userData != null)
                    {
                        applicationUser = this.ConvertApplicationUser(userData);
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// 密码匹配
        /// </summary>
        /// <param name="hashedPassword"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static bool VerifyHashedPassword(string hashedPassword, string password)
        {
            if (hashedPassword == null)
            {
                return false;
            }
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            byte[] array = Convert.FromBase64String(hashedPassword);
            if (array.Length != 49 || array[0] != 0)
            {
                return false;
            }
            byte[] array2 = new byte[16];
            Buffer.BlockCopy(array, 1, array2, 0, 16);
            byte[] array3 = new byte[32];
            Buffer.BlockCopy(array, 17, array3, 0, 32);
            byte[] bytes;
            using (Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, array2, 1000))
            {
                bytes = rfc2898DeriveBytes.GetBytes(32);
            }
            return ByteArraysEqual(array3, bytes);
        }

        private static bool ByteArraysEqual(byte[] a, byte[] b)
        {
            if (object.ReferenceEquals(a, b))
            {
                return true;
            }
            if (a == null || b == null || a.Length != b.Length)
            {
                return false;
            }
            bool flag = true;
            for (int i = 0; i < a.Length; i++)
            {
                flag &= (a[i] == b[i]);
            }
            return flag;
        }

        private ApplicationUser ConvertApplicationUser(FrontIdentityUser userData)
        {
            return new ApplicationUser()
                            {
                                Id = userData.Id,
                                AccessFailedCount = userData.AccessFailedCount,
                                ActiveWay =
                                    userData.ActiveWay,
                                Address = userData.Address,
                                Age = userData.Age,
                                Area = userData.Area,
                                ApprovalStatus = userData.ApprovalStatus,
                                Birthday = userData.Birthday,
                                City = userData.City,
                                CreatedPerson = userData.CreatedPerson,
                                CreateTime = userData.CreateTime,
                                Email = userData.Email,
                                EmailConfirmed = userData.EmailConfirmed,
                                FaceImage = userData.FaceImage,
                                Gender = userData.Gender,
                                GenderName = userData.GenderName,
                                IdentityConfirmed = userData.IdentityConfirmed,
                                IdentityNumber = userData.IdentityNumber,
                                Interest = userData.Interest,
                                IsDel = userData.IsDel,
                                IsNeedModifyPw = userData.IsNeedModifyPw,
                                IsPay = userData.IsPay,
                                IsPayName = userData.IsPayName,
                                LockoutEnabled = userData.LockoutEnabled,
                                LockoutEndDateUtc = userData.LockoutEndDateUtc,
                                MLevel = userData.MLevel,
                                MLevelName = userData.MLevelName,
                                MType = userData.MType,
                                MTypeName = userData.MTypeName,
                                NickName = userData.NickName,
                                No = userData.No,
                                Password = userData.Password,
                                PasswordHash = userData.PasswordHash,
                                PayNumber = userData.PayNumber,
                                PhoneNumber = userData.PhoneNumber,
                                PhoneNumberConfirmed = userData.PhoneNumberConfirmed,
                                Provency = userData.Provency,
                                RealName = userData.RealName,
                                SecurityStamp = userData.SecurityStamp,
                                Status = userData.Status,
                                StatusName = userData.StatusName,
                                SystemMType = userData.SystemMType,
                                SystemMTypeName = userData.SystemMTypeName,
                                TwoFactorEnabled = userData.TwoFactorEnabled,
                                UpdateTime = userData.UpdateTime,
                                UserName = userData.UserName,
                                VIN = userData.VIN
                            };
        }

        #endregion


        #region  ===会员体系升级====

        /// <summary>
        /// 页面注册
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AccountRegisterVip(string returnUrl, string source)
        {
            ViewBag.returnUrl = returnUrl;
            ViewBag.source = source;
            return View();
        }

        /// <summary>
        ///  短信登陆
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> DoLoginByPhone(LoginViewModel model)
        {
            try
            {
                int isFirst = 0;

                if (!string.IsNullOrEmpty(model.Captcha))
                {
                    //第一：确认验证码
                    var validateCode = string.Empty;

                    if (Session["ValidateCode"] != null)
                        validateCode = Session["ValidateCode"].ToString();

                    if (!validateCode.Equals(model.Captcha))
                    {
                        return Json(new { code = 400, url = "/Account/LogonPage", msg = "图片验证码输入有误" });
                        //return RedirectToAction("LogonPage", "Account", new { returnUrl = returnUrl });
                    }
                }
                //  var errors = ModelState.Values.SelectMany(v => v.Errors);
                //if (!ModelState.IsValid) return Json(new { code = 400, msg = "登录失败" });
                ReturnResult _captchaResult = _AppContext.UserSecurityApp.ValidateMobileVerifyCode(model.UserName, model.SMSCaptcha);
                if (!_captchaResult.IsSuccess)
                {
                    return Json(new { code = 400, msg = "短信验证码错误或已过期，请重新获取" });
                }

                //_AppContext.OrderApp.GetOrderAddress();

                //手机号是注册用直接登陆，否则注册新用户登陆
                ApplicationUser applicationUser;
                if (this.LoginFactory(model, out applicationUser, 2))
                {

                    if (applicationUser != null)
                    {
                        if (applicationUser.IsNeedModifyPw == 1)
                        {
                            return Json(new { code = 300, msg = " 帐户存在风险， 请先重置密码" });
                        }
                    }
                    _AppContext.LoginMemRecordApp.Add(applicationUser.Id, applicationUser.NickName, EDataSource.blms);
                    await SignInManager.SignInAsync(applicationUser, model.RememberMe, rememberBrowser: false);
                    BBSUtil.CheckAndCreateDefaultBBSMember(applicationUser); //new

                    var returnIntegralType = (int)_AppContext.CarServiceUserApp.GetReIntegralTypeByIdentity(applicationUser.IdentityNumber);
                    if (_AppContext.LoginMemRecordApp.IsReMemberShipRequest(applicationUser.Id) && returnIntegralType != -1 && applicationUser.IsPay != 2)
                    {
                        return Json(new { code = 200, msg = "登录成功", isrequest = 1, returnIntegralType = returnIntegralType, identityNumber = applicationUser.IdentityNumber });
                    }
                    else
                    {
                        return Json(new { code = 200, msg = "登录成功", isrequest = 0 });
                    }



                }
                else //手机号未注册 ，注册并登陆
                {
                    isFirst = 1;
                    var store = new FrontUserStore<FrontIdentityUser>();
                    var membershipManager = new UserManager<FrontIdentityUser>(store);
                    if (store.CheckUserNameIsExist(model.UserName))
                        return Json(new { code = 400, success = false, msg = "系统中已存在此手机号" });

                    IdentityResult CreateResult = null;
                    var membershipIdentity = new FrontIdentityUser
                    {
                        No = _AppContext.MemberNumberApp.GetNumber("1"),//会员卡号
                        NickName = CommonUtilitys.GetNikeName(),
                        UserName = model.UserName,// model.UserName,
                        PhoneNumber = model.UserName,
                        Password = "Bm" + model.UserName.Substring(model.UserName.Length - 6, 6),
                        Status = (int)MembershipStatus.Nomal,
                        CreateTime = DateTime.Now.ToLongTimeString(),
                        CreatedPerson = WebUtils.Device(),
                        MType = (int)MembershipType.WhitoutCar,//非车主
                        MLevel = (int)MemshipLevel.OneStar,//级别
                        IsPay = (int)MembershipPayStatus.NotPay,//经销商新增的会员均为已缴纳100付费
                        ApprovalStatus = (int)MembershipApprovalStatus.Activing, //激活中
                        ActiveWay = (int)MembershipActiveWay.ClientWeb,
                        IsNeedModifyPw = (int)MembershipNeedModifyPw.No,
                        MLevelBeginDate = DateTime.Parse(DateTime.Now.ToShortDateString()),
                        MLevelInvalidDate = DateTime.Parse(DateTime.Now.ToShortDateString()).AddYears(1),
                        AuthenticationTime=DateTime.Parse("1900-01-01")

                    };
                    CreateResult = membershipManager.Create(membershipIdentity, membershipIdentity.Password);
                    // var user = store.FindByNameAsync(model.UserName).Result;
                    if (CreateResult != null && !CreateResult.Succeeded)
                    {
                        var message = "";
                        foreach (var error in CreateResult.Errors)
                        {
                            message += error;
                        }
                        ModelState.AddModelError("", message);
                        return Json(new { success = false, msg = message });
                    }

                    this.LoginFactory(model, out applicationUser, 2);
                    _AppContext.LoginMemRecordApp.Add(applicationUser.Id, applicationUser.NickName, EDataSource.blms);
                    await SignInManager.SignInAsync(applicationUser, model.RememberMe, rememberBrowser: false);
                    BBSUtil.CheckAndCreateDefaultBBSMember(applicationUser); //new

                    _AppContext.UserMessageRecordApp.InsertLoginChangePasswordMessage(applicationUser.Id);
                    return Json(new { code = 200, msg = "登录成功", first = isFirst });


                }
            }
            catch (Exception ex)
            {
                return Json(new { code = 400, msg = ex.Message });
            }
        }


        /// <summary>
        /// ajax提交post注册请求（会员体系升级）
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> DoRegister_Vip(RegisterViewModel model)
        {
            try
            {
                //1、验证请求
                if (!ModelState.IsValid)
                {
                    foreach (ModelState state in ModelState.Values.Where(state => state.Errors.Count > 0))
                    {
                        return Json(new { code = 400, msg = state.Errors[0].ErrorMessage });
                    }
                    return Json(new { code = 400, msg = "注册失败,参数错误" });
                }

                var birthday = string.Empty;
                int Gender = -1;
                int Age = -1;
                string IdentityCardType = ((int)Vcyber.BLMS.Entity.Enum.ECustomerIdentificationType.IdentityCard).ToString();
                //判断证件类型是否是身份证
                if (model.PaperWork == IdentityCardType)
                {
                    switch (model.IdentityNumber.Length)
                    {
                        case 15:
                            birthday = "19" + model.IdentityNumber.Substring(6, 2) + "-" + model.IdentityNumber.Substring(8, 2) + "-" + model.IdentityNumber.Substring(10, 2);
                            Gender = (Convert.ToInt32(model.IdentityNumber.Substring(14)) % 2 == 0 ? 2 : 1);
                            Age = DateTime.Now.Year - Convert.ToInt32("19" + model.IdentityNumber.Substring(6, 2));
                            break;
                        case 18:
                            birthday = model.IdentityNumber.Substring(6, 4) + "-" + model.IdentityNumber.Substring(10, 2) + "-" + model.IdentityNumber.Substring(12, 2);
                            Gender = (Convert.ToInt32(model.IdentityNumber.Substring(16, 1)) % 2 == 0 ? 2 : 1);
                            Age = DateTime.Now.Year - Convert.ToInt32(model.IdentityNumber.Substring(6, 4));
                            break;
                        default:
                            return Json(new { code = 402, msg = "请正确输入证件号码" });
                    }
                    DateTime time;
                    if (!DateTime.TryParse(birthday, out time))
                    {
                        return Json(new { code = 402, msg = "请正确输入证件号码" });
                    }
                    if (time < DateTime.Parse("1900-01-10"))
                    {
                        return Json(new { code = 402, msg = "请正确输入证件号码" });
                    }
                }

                //第一：确认验证码
                var validateCode = string.Empty;

                if (Session["ValidateCode"] != null)
                    validateCode = Session["ValidateCode"].ToString();

                if (!validateCode.Equals(model.ImgCaptcha))
                {
                    return Json(new { code = 402, url = "/Account/LogonPage", msg = "图片验证码输入有误" });
                    //return RedirectToAction("LogonPage", "Account", new { returnUrl = returnUrl });
                }
                ////2、验证手机验证码
                ReturnResult _captchaResult = _AppContext.UserSecurityApp.ValidateMobileVerifyCode(model.Mobile, model.Captcha);
                if (!_captchaResult.IsSuccess)
                {
                    return Json(new { code = 401, msg = "短信验证码错误或已过期，请重新获取" });
                }
                int userLevel = (int)MemshipLevel.OneStar;
                //根据身份证号，查询车辆信息，确定会员级别
                if (!string.IsNullOrEmpty(model.IdentityNumber))
                {
                    userLevel = GetUserLevel(model.IdentityNumber);
                }

                //判断用户级别
                //3、获取注册信息
                var user = new ApplicationUser
                {
                    No = _AppContext.MemberNumberApp.GetNumber("1"),//会员卡号
                    PhoneNumber = model.Mobile,
                    UserName = model.Mobile,
                    NickName = CommonUtilitys.GetNikeName(),
                    IdentityNumber = model.IdentityNumber,
                    MLevel = userLevel,
                    MType = (int)model.MType,
                    SystemMType = (int)MembershipType.WhitoutCar,
                    ActiveWay = (int)MembershipActiveWay.ClientWeb,
                    CreatedPerson = string.IsNullOrEmpty(model.Source) ? "blms_pc_web" : model.Source,
                    MLevelBeginDate = DateTime.Parse(DateTime.Now.ToShortDateString()),
                    MLevelInvalidDate = DateTime.Parse(DateTime.Now.ToShortDateString()).AddYears(1),
                    PaperWork = model.PaperWork,
                    Mid = model.IdentityNumber,
                    AuthenticationTime = Convert.ToDateTime("1900-01-01"),
                    Age = Age,
                    Gender = Gender.ToString()
                };
                string _activedealerId = model.ActivedealerId;

                //4、检验身份证号
                Task<FrontIdentityUser> cusObj = null;
                if (!string.IsNullOrEmpty(model.IdentityNumber))
                {
                    cusObj = new FrontUserStore<FrontIdentityUser>().FindByIdentityNumber(model.IdentityNumber);
                    if (cusObj != null && cusObj.Result != null)
                    {
                        return Json(new { code = 402, msg = "您输入的证件号已被注册" });
                    }
                }
                //5、注册用户,如果用户存在则更新该用户信息
                IdentityResult result = null;
                //if (string.IsNullOrEmpty(model.VIN))
                if (!string.IsNullOrEmpty(birthday))
                {
                    user.Birthday = birthday;
                }
                if (!(cusObj != null && cusObj.Result != null))
                {
                    result = await UserManager.CreateAsync(user, model.Password);
                }
                int IsHasCar = 0;
                //6、注册完成跳转
                if (result != null && result.Succeeded)
                {



                    // 1)检索是否有车
                    var gradeUser = UserManager.FindByName(model.Mobile);
                    var carList = _AppContext.CarServiceUserApp.SelectCarListByIdentity(model.IdentityNumber);
                    //gradeUser.MLevel = GetUserLevel(model.IdentityNumber);

                    // 正式环境取消注释
                    if (carList.Count() > 0)
                    {
                        gradeUser.SystemMType = (int)MembershipType.WhitCar;
                        IsHasCar = 1;
                        gradeUser.AuthenticationTime = DateTime.Now;
                        gradeUser.AuthenticationSource = "blms_pc_web";
                        gradeUser.Age = Age;
                        gradeUser.Gender = Gender.ToString();
                    }
                    gradeUser.Age = Age;
                    gradeUser.Gender = Gender.ToString();
                    //else
                    //{
                    //    return Json(new { code = 801, msg = "  未找到您的车辆，请及时拨打北京现代24小时服务热线400-800-1100咨询" });
                    //    //  return Json ( recode = 801; remsg = "";//未查到车辆信息
                    //}
                    UserManager.Update(gradeUser);

                    try
                    {
                        //模拟登陆
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    }
                    catch (Exception ex)
                    {
                        Vcyber.BLMS.Common.LogService.Instance.Debug("注册成功模拟登陆异常", ex);
                    }
                    // 5)发送短信

                    //6判断车型交费返积分
                    // var returnIntegralType= (int) GetCarTypeByIdentity(model.IdentityNumber);

                    var returnIntegralType = (int)_AppContext.CarServiceUserApp.GetReIntegralTypeByIdentity(model.IdentityNumber);

                    if (gradeUser.SystemMType == 2 && returnIntegralType != -1)
                    {
                        var amount = "";
                        if (returnIntegralType == 0)
                        {
                            amount = "400";
                        }
                        else if (returnIntegralType == 1)
                        {
                            amount = "700";
                        }
                        else if (returnIntegralType == 2)
                        {
                            amount = "200";
                        }
                        else
                        {
                            amount = "400";

                        }
                        _AppContext.SMSApp.SendSMS(ESmsType.提交注册_匹配到车辆, model.Mobile, new string[] { amount });
                    }
                    else
                    {
                        _AppContext.SMSApp.SendSMS(ESmsType.注册成功, model.Mobile, new string[] { "" });
                    }
                    //判断用户是否是企业用户
                    bool flag = _AppContext.DealerMembershipApp.IsPersonalUser(model.IdentityNumber);
                    if (!flag)
                    {
                        returnIntegralType = -1;
                    }
                    return Json(new { code = 200, msg = "注册成功", ReIntegralType = returnIntegralType, Mtype = gradeUser.MType, HasCar = IsHasCar });
                }
                AddErrors(result);
                var errorMsg = result.Errors.FirstOrDefault();
                if (errorMsg != null && errorMsg.StartsWith("Passwords")) errorMsg = "密码必须为8位以上数字和大小写字母组合";
                if (errorMsg != null && errorMsg.StartsWith("Name")) errorMsg = "您的手机号已经注册过";

                // 如果我们进行到这一步时某个地方出错，则重新显示表单
                return Json(new { code = 400, msg = string.Format("{0}", errorMsg) });
            }
            catch (Exception ex)
            {
                return Json(new { code = 400, msg = string.Format("{0}", ex.Message) });
            }

        }
        /// <summary>
        /// 向特约店提交申请
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="dealerId"></param>
        /// <param name="identtiyNumber"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CreateMembershipRequest(string dealerId, string identtiyNumber)
        {
            if (this.User.Identity.IsAuthenticated)
            {
                string userid = this.User.Identity.GetUserId();
                var userStore = new FrontUserStore<FrontIdentityUser>();
                var gradeUser = UserManager.FindById(userid);

                if (gradeUser.IsPay == 2)
                {
                    return Json(new { code = "402", msg = "您已申请缴费,请勿重复提交" });

                }
                gradeUser.IsPay = 2; //设置已提交支付

                var returnIntegralType = (int)_AppContext.CarServiceUserApp.GetReIntegralTypeByIdentity(identtiyNumber);
                if (returnIntegralType != -1)
                {

                    gradeUser.Amount = returnIntegralType > 1 ? 50 : 100;

                }

                int Gender = -1;
                int Age = -1;
                //更具身份证获取性别和年龄
                if (!string.IsNullOrEmpty(identtiyNumber))
                {
                    switch (identtiyNumber.Trim().Length)
                    {
                        case 15:
                            Gender = (Convert.ToInt32(identtiyNumber.Substring(14)) % 2 == 0 ? 2 : 1);
                            Age = DateTime.Now.Year - Convert.ToInt32("19" + identtiyNumber.Substring(6, 2));
                            break;
                        case 18:
                            Gender = (Convert.ToInt32(identtiyNumber.Substring(16, 1)) % 2 == 0 ? 2 : 1);
                            Age = DateTime.Now.Year - Convert.ToInt32(identtiyNumber.Substring(6, 4));
                            break;
                        default:
                            return Json(new { code = 402, msg = "请正确输入证件号码" });
                    }
                }
                gradeUser.Age = Age;
                gradeUser.Gender = Gender.ToString();
                var result = userStore.CreateMembershipRequest(userid, identtiyNumber, dealerId, string.Empty, "blms");
                if (result)
                {
                    UserManager.Update(gradeUser);
                    userStore.AddMembershipDealerRecord(userid, dealerId);
                    return Json(new { code = "200", msg = "您向特约店申请交费成功" });
                }
                else
                {
                    return Json(new { code = "400", msg = "您向特约店申请交费失败，请重新输入" });
                }
            }
            else
            {
                return Json(new { code = "401", msg = "账号登录异常，请重新登录" });
            }

        }

        /// <summary>
        ///  特约店天猫支付码
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="paynumber"></param>
        /// <param name="dealerId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult PayByTmallRequest(string paynumber, string dealerId)
        {
            if (this.User.Identity.IsAuthenticated)
            {
                string userid = this.User.Identity.GetUserId();
                var gradeUser = UserManager.FindById(userid);
                gradeUser.ApprovalStatus = (int)MembershipApprovalStatus.Activing;

                if (gradeUser.IsPay == 2)
                {
                    return Json(new { code = "402", msg = "您已申请缴费,请勿重复提交" });

                }
                if (string.IsNullOrEmpty(paynumber))
                {
                    return Json(new { code = 400, msg = "选择天猫支付必须输入支付码" });
                }
                if (!string.IsNullOrEmpty(paynumber))
                {
                    gradeUser.PayNumber = paynumber;
                }
                gradeUser.IsPay = 2;
                var returnIntegralType = (int)_AppContext.CarServiceUserApp.GetReIntegralTypeByIdentity(gradeUser.IdentityNumber);
                if (returnIntegralType != -1)
                {

                    gradeUser.Amount = returnIntegralType > 1 ? 50 : 100;

                }
                UserManager.Update(gradeUser);
                var userStore = new FrontUserStore<FrontIdentityUser>();
                userStore.AddMembershipDealerRecord(userid, dealerId);
                var result = userStore.CreateMembershipRequest(userid, gradeUser.IdentityNumber, dealerId, string.Empty, "blms");
                if (result)
                {
                    return Json(new { code = "200", msg = "保存成功" });
                }
                else
                {
                    return Json(new { code = "400", msg = "保存失败，请重新输入" });
                }
            }
            else
            {
                return Json(new { code = "401", msg = "尊敬的客户，您的账号登录异常，请重新登陆" });
            }

        }

        #endregion


    }

    public static class BBSUtil
    {
        public static void CheckAndCreateDefaultBBSMember(ApplicationUser applicationUser)
        {
            var api_key = ConfigurationManager.AppSettings["BBS_api_key"];
            var secret = ConfigurationManager.AppSettings["BBS_secret"];
            var url = ConfigurationManager.AppSettings["BBS_url"];
            var staticP = "3u29lmGRs";
            var nickName = !string.IsNullOrEmpty(applicationUser.NickName) && applicationUser.NickName.Length > 13
                ? applicationUser.NickName.Substring(0, 13)
                : applicationUser.NickName;
            var username = nickName + applicationUser.Id.Substring(30, 5);
            var email = username + "@bluemembers.com.cn";

            LogService.Instance.Info(string.Format("BBS开始登陆,Username=[{0}],nickname=[{1}]", username, nickName));

            var ds = new DiscuzSession(api_key, secret, url);
            int uid = 0;
            try { uid = ds.GetUserID(username); }
            catch { }

            if (uid == 0) //需要注册
            {
                try
                {
                    uid = ds.Register(username, staticP, email, false, nickName);
                }
                catch (Exception ex)
                {
                    LogService.Instance.Error(string.Format("BBS注册时发生错误,Username=[{0}],nickname=[{1}],{2}", username, nickName, ex));
                }
            }

            if (uid > 0)//登陆BBS
            {
                try
                {
                    ds.Login(uid, staticP, true, 10, "");
                    LogService.Instance.Info(string.Format("BBS登陆成功,Username=[{0}],nickname=[{1}]", username, nickName));
                }
                catch (Exception ex)
                {
                    LogService.Instance.Error(string.Format("BBS登陆时发生错误,Username=[{0}],nickname=[{1}],{2}", username, nickName, ex));
                }
            }
        }

        public static void Logout()
        {
            var api_key = ConfigurationManager.AppSettings["BBS_api_key"];
            var secret = ConfigurationManager.AppSettings["BBS_secret"];
            var url = ConfigurationManager.AppSettings["BBS_url"];


            var ds = new DiscuzSession(api_key, secret, url);
            ds.Logout("");
        }



    }
}