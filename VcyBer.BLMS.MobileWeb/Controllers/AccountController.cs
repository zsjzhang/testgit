using AspNet.Identity.SQL;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Security;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.Common.Web;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;
using VcyBer.BLMS.MobileWeb.Models;

namespace VcyBer.BLMS.MobileWeb.Controllers
{
    public class AccountController : BaseController
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
        
        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        public ActionResult Login(string url)
        {
            HttpContext.Session["VarName"] = "hi";

            //SessionStateSection sessionState = (SessionStateSection)ConfigurationManager.GetSection("system.web/sessionState");
            //string sidCookieName = sessionState.CookieName;

            //if (Request.Cookies[sidCookieName] != null)
            //{
            //    HttpCookie sidCookie = Request.Cookies[sidCookieName];
            //    sidCookie.Value = Session.SessionID;
            //    sidCookie.HttpOnly = true;
            //    sidCookie.Secure = true;
            //    sidCookie.Path = "/";

            //    Response.Cookies.Set(sidCookie);
            //}

            ViewBag.Url = url;
            return View();
        }

        /// <summary>
        /// ajax提交登录请求
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
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

                #region ==== 新逻辑 ====

                ApplicationUser applicationUser;
                var result = this.ReLoginStatus(model, out applicationUser);

                switch (result)
                {
                    case LoginUserStatus.NeedModfiyPassWord:
                        return Json(new { code = 300, msg = "请先重置密码" });
                    case LoginUserStatus.Sucessed:
                        _AppContext.LoginMemRecordApp.Add(applicationUser.Id, applicationUser.NickName, EDataSource.blms_wap);

                        //记住密码
                        //model.RememberMe = true;
                        SignInManager.SignInAsync(applicationUser, model.RememberMe, rememberBrowser: false).Wait();
                        
                        //BBSUtil.CheckAndCreateDefaultBBSMember(applicationUser); //new
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
                        return Json(new { code = 902, msg = "帐户不存在，请重新输入" });
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

        /// <summary>
        /// 登录状态
        /// </summary>
        /// <param name="model"></param>
        /// <param name="applicationUser"></param>
        /// <param name="loginType"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 字节数组
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 将登录用户信息写入上下文
        /// </summary>
        /// <param name="userData"></param>
        /// <returns></returns>
        private ApplicationUser ConvertApplicationUser(FrontIdentityUser userData)
        {
            return new ApplicationUser()
            {
                Id = userData.Id,
                AccessFailedCount = userData.AccessFailedCount,
                ActiveWay = userData.ActiveWay,
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

        /// <summary>
        /// 发送短信验证码
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="imageCode"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SendCaptchaAndCheckImageCode(string mobile, string imageCode)
        {
            //确认验证码
            var validateCode = string.Empty;

            if (Session["ValidateCode"] != null)
                validateCode = Session["ValidateCode"].ToString();

            if (!validateCode.Equals(imageCode))
            {
                return Json(new { code = 401, msg = "图形验证码输入错误" });
            }
            ReturnResult _result = _AppContext.UserSecurityApp.SendMobileVerifyCode(mobile, 4, "blms_wap");
            if (_result.IsSuccess)
            {
                return Json(new { code = 200, msg = _result.Message }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { code = 400, msg = _result.Message }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 用手机验证码登录
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
                    }
                }
                ReturnResult _captchaResult = _AppContext.UserSecurityApp.ValidateMobileVerifyCode(model.UserName, model.SMSCaptcha);
                if (!_captchaResult.IsSuccess)
                {
                    return Json(new { code = 400, msg = "短信验证码错误或已过期，请重新获取" });
                }

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
                    _AppContext.LoginMemRecordApp.Add(applicationUser.Id, applicationUser.NickName, EDataSource.blms_wap);
                    await SignInManager.SignInAsync(applicationUser, model.RememberMe, rememberBrowser: false);
                    //BBSUtil.CheckAndCreateDefaultBBSMember(applicationUser); //new

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
                        CreatedPerson = "blms_wap",
                        MType = (int)MembershipType.WhitoutCar,//非车主
                        MLevel = (int)MemshipLevel.OneStar,//级别
                        IsPay = (int)MembershipPayStatus.NotPay,//经销商新增的会员均为已缴纳100付费
                        ApprovalStatus = (int)MembershipApprovalStatus.Activing, //激活中
                        ActiveWay = (int)MembershipActiveWay.ClientWeb,
                        IsNeedModifyPw = (int)MembershipNeedModifyPw.No,
                        MLevelBeginDate = DateTime.Parse(DateTime.Now.ToShortDateString()),
                        MLevelInvalidDate = DateTime.Parse(DateTime.Now.ToShortDateString()).AddYears(1),
                        AuthenticationTime = DateTime.Parse("1900-01-01")

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
                    _AppContext.LoginMemRecordApp.Add(applicationUser.Id, applicationUser.NickName, EDataSource.blms_wap);
                    await SignInManager.SignInAsync(applicationUser, model.RememberMe, rememberBrowser: false);
                    //BBSUtil.CheckAndCreateDefaultBBSMember(applicationUser); //new

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
        /// 登陆厂操作
        /// </summary>
        /// <param name="model"></param>
        /// <param name="applicationUser"></param>
        /// <param name="loginType"></param>
        /// <returns></returns>
        private bool LoginFactory(LoginViewModel model, out ApplicationUser applicationUser, int loginType = 1)
        {
            applicationUser = new ApplicationUser();
            var userStor = new FrontUserStore<FrontIdentityUser>();
            var userDatas = userStor.FindLogin(model.UserName);

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
        /// 注册
        /// </summary>
        /// <returns></returns>
        public ActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// 正式注册
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
                string IdentityCardType = ((int)ECustomerIdentificationType.IdentityCard).ToString();
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
                    CreatedPerson = string.IsNullOrEmpty(model.Source) ? "blms_wap" : model.Source,
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

                    // 正式环境取消注释
                    if (carList.Count() > 0)
                    {
                        gradeUser.SystemMType = (int)MembershipType.WhitCar;
                        IsHasCar = 1;
                        gradeUser.AuthenticationTime = DateTime.Now;
                        gradeUser.AuthenticationSource = "blms_wap";
                        gradeUser.Age = Age;
                        gradeUser.Gender = Gender.ToString();
                    }
                    gradeUser.Age = Age;
                    gradeUser.Gender = Gender.ToString();
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
        /// 会员定级 给注册的用户定级
        /// 根据身份证号查询注册用户车辆信息来定级
        /// </summary>
        /// <param name="IdentityNumber">身份证号</param>
        /// <returns></returns>
        private int GetUserLevel(string IdentityNumber)
        {
            int _level = (int)MemshipLevel.OneStar;
            string strlevel = _AppContext.DealerMembershipApp.GetFirstRegistMLevelByIdNumber(IdentityNumber);
            int.TryParse(strlevel, out _level);
            return _level;

        }

        /// <summary>
        /// 抓取错误消息
        /// </summary>
        /// <param name="result"></param>
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        /// <summary>
        /// 忘记密码
        /// </summary>
        /// <returns></returns>
        public ActionResult ForgetPwd()
        {
            return View();
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="userAccount">手机号</param>
        /// <param name="passwd">新密码</param>
        /// <param name="newpasswd">重复密码</param>
        /// <param name="captcha">手机验证码</param>
        /// <param name="Imgcaptcha">图形验证码</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DoResetPasswd(string userAccount, string passwd, string newpasswd, string captcha, string Imgcaptcha)
        {
            try
            {
                //第一：确认验证码
                var validateCode = string.Empty;

                if (Session["ValidateCode"] != null)
                    validateCode = Session["ValidateCode"].ToString();

                if (!validateCode.Equals(Imgcaptcha))
                {
                    return Json(new { code = "400", msg = "图片验证码输入有误" });
                }
                ReturnResult _captchaResult = _AppContext.UserSecurityApp.ValidateMobileVerifyCode(userAccount, captcha);
                if (!_captchaResult.IsSuccess)
                {
                    return Json(new { code = "400", msg = "手机验证码失败" });
                }

                ApplicationUser _userEntity = UserManager.FindByName(userAccount);
                if (_userEntity == null)
                {
                    return Json(new { code = "400", msg = "用户账号不存在" });
                }

                string userId = _userEntity.Id;
                String hashedNewPassword = UserManager.PasswordHasher.HashPassword(newpasswd);
                var user = UserManager.FindById(userId);
                user.PasswordHash = hashedNewPassword;
                user.IsNeedModifyPw = 0;
                user.UpdateTime = DateTime.Now.ToString();
                IdentityResult _result = UserManager.Update(user);

                if (_result.Succeeded)
                {
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

        /// <summary>
        /// 退出系统
        /// </summary>
        /// <returns></returns>
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
            }

            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }
    }
}