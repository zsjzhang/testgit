using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.FrontWeb.Models;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.Owin;
using AspNet.Identity.SQL;
using System.Security.Cryptography;
using Vcyber.BLMS.Entity.Enum;
using Microsoft.AspNet.Identity;

namespace Vcyber.BLMS.FrontWeb.Controllers.Musical
{
    public class ActivityController : Controller
    {
        //剧院魅影活动针对JoinActivity的冗余字段使用说明
        //result1 mapto  vin
        //result2 mapto  maskname
        //result3 mapto  source
        private ApplicationSignInManager _signInManager;
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set { _signInManager = value; }
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
        public ActivityController()
        {
        }

        public ActivityController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }
        //
        // GET: /Activity/
        public ActionResult Index(string source)
        {
            ViewBag.source = source;
            ViewData["provinceList"] = Vcyber.BLMS.Common.City.CityService.Instance.GetProvince();
            ViewData["powerWinnerList"] = _AppContext.WinningInfoApp.GetWinningsByWhere(1, " Prizesid=10 and UpdateTime >'2015-11-10'");
            ViewData["piaoWinerList"] = _AppContext.WinningInfoApp.GetWinningsByWhere(1, " Prizesid=9 and UpdateTime >'2015-11-10'");
            return View();
        }

        /// <summary>
        /// 检查用户是否登录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult IsLogin()
        {
            if (!this.User.Identity.IsAuthenticated)
            {
                //
                return Json(new { msg = "N" }, JsonRequestBehavior.AllowGet);
                //return RedirectToAction("LogonPage", "Account", new
                //{
                //    returnUrl = "/MyCenter/Index"
                //});
            }
            return Json(new { msg = "Y" }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 添加活动人员
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult JoinActivity(JoinActivity entity)
        {
            entity.ActivityId = 1;
            entity.UserId = string.Empty;
            entity.CreateDate = DateTime.Now;
            if (User.Identity.IsAuthenticated)
            {
                entity.UserId = User.Identity.Name;
                //获取vin码
                entity.Results1 = GetVinByUserId(entity.UserId);
            }
            int _result = _AppContext.JoinActivityApp.AddJoinActivityNew(entity);
            if (_result <= 0)
            {
                Json(new { msg = "N", id = _result }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { msg = "true", id = _result }, JsonRequestBehavior.AllowGet);

        }


        /// <summary>
        /// 用户选择面具
        /// </summary>
        /// <param name="id"></param>
        /// <param name="maskName"></param>
        /// <returns></returns>
        public ActionResult AddMaskToActivity(int id, string maskName)
        {
            JoinActivity _entity = _AppContext.JoinActivityApp.GetJoinActivityById(id);
            _entity.Results2 = maskName;
            if (_AppContext.JoinActivityApp.UpdateJoinActivity(_entity))
            {
                return Json(new { msg = "N" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { msg = "true" }, JsonRequestBehavior.AllowGet);
        }

        public string GetVinByUserId(string userid)
        {
            string _vin = string.Empty;
            string _userid = string.Empty;
            ApplicationUser _curUser = UserManager.FindByName(userid);
            if (_curUser == null || string.IsNullOrEmpty(_curUser.Id))
            {
                return _vin;
            }
            IEnumerable<Car> _carList = _AppContext.CarServiceUserApp.SelectCarListByUserId(_curUser.Id);
            if (_carList == null || _carList.Count() <= 0)
            {
                return _vin;
            }
            if (_carList != null && _carList.Any())
            {
                foreach (var item in _carList)
                {
                    //如果是索九或者全新途胜则取vin码，否则取最后一个车的vin码
                    if (item.CarCategory == "索纳塔9" || item.CarCategory == "全新途胜" || item.CarCategory == "第九代索纳塔")
                    {
                        _vin = item.VIN;
                        break;
                    }
                    _vin = item.VIN;
                }
            }
            return _vin;
        }


        /// <summary>
        /// form表单post登录请求
        /// </summary>
        /// <param name="model"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
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
        #region ==== 私有方法 ====

        private bool LoginFactory(LoginViewModel model, out ApplicationUser applicationUser)
        {
            applicationUser = new ApplicationUser();
            var userStor = new FrontUserStore<FrontIdentityUser>();
            var userDatas = userStor.FindLogin(model.UserName);

            if (userDatas.Count > 0)
            {
                if (userDatas[0].IsNeedModifyPw == 1)
                {
                    applicationUser = this.ConvertApplicationUser(userDatas[0]);
                    return true;
                }
            }

            foreach (var userData in userDatas)
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


    }
}