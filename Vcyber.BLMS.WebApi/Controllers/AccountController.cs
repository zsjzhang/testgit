using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Security;
using AspNet.Identity.SQL;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.IO;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.WebApi.Filter;
using Vcyber.BLMS.WebApi.Models;
using Vcyber.BLMS.WebApi.Providers;
using Vcyber.BLMS.WebApi.Results;
using IdentityUser = Microsoft.AspNet.Identity.EntityFramework.IdentityUser;
using Vcyber.BLMS.Application;
using WebGrease.Css.Extensions;
using System.Web.Http.Description;
using Vcyber.BLMS.Entity.Generated;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.WebApi.Common;
using Vcyber.BLMS.WebApi.Models.RequestData;
using Vcyber.BLMS.WebApi.Models.ResponseData;
using Vcyber.BLMS.Entity.BLMSMoney;

namespace Vcyber.BLMS.WebApi.Controllers
{
    using System.Configuration;
    using System.Data.Entity.Migrations.Infrastructure;
    using System.Runtime.InteropServices.WindowsRuntime;
    using System.Security.Policy;
    using System.Threading;
    using System.Web.UI;

    using log4net;

    using PetaPoco;

    using Vcyber.BLMS.Domain;
    using Vcyber.BLMS.Entity.Common;
    using Vcyber.BLMS.Entity.User;
    using Vcyber.BLMS.Entity.CarService;
    using Entity.BLMSMoney;
    using System.Text;

    /// <summary>
    /// 账号
    /// </summary>
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        private const string LocalLoginProvider = "Local";

        private ILog log = LogManager.GetLogger(typeof(AccountController));
        public AccountController()
        {
            //TO DO 
        }

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
                UserName = userData.UserName
            };
        }

        //private bool LoginFactory(LoginViewModel model, out ApplicationUser applicationUser)
        //{
        //    applicationUser = new ApplicationUser();
        //    var userStor = new FrontUserStore<FrontIdentityUser>();
        //    var userDatas = userStor.FindLogin(model.UserName);

        //    if (userDatas.Count > 0)
        //    {
        //        if (userDatas[0].IsNeedModifyPw == 1)
        //        {
        //            applicationUser = this.ConvertApplicationUser(userDatas[0]);
        //            return true;
        //        }
        //    }

        //    foreach (var userData in userDatas)
        //    {
        //        if (!string.IsNullOrEmpty(userData.PasswordHash))
        //        {
        //            bool result = VerifyHashedPassword(userData.PasswordHash, model.Password);

        //            if (result)
        //            {
        //                if (userData != null)
        //                {
        //                    applicationUser = this.ConvertApplicationUser(userData);
        //                    return true;
        //                }
        //            }
        //        }
        //    }

        //    return false;
        //}

        private bool LoginFactory(LoginViewModel model, out FrontIdentityUser applicationUser, int loginType = 1)
        {
            //applicationUser = new ApplicationUser();
            applicationUser = null;
            var userStor = new FrontUserStore<FrontIdentityUser>();
            var userDatas = userStor.FindLogin(model.UserName);

            if (userDatas.Count > 0)
            {
                if (userDatas[0].IsNeedModifyPw == 1)
                {
                    //applicationUser = this.ConvertApplicationUser(userDatas[0]);
                    applicationUser = userDatas[0];
                    return true;
                }
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
                                //applicationUser = this.ConvertApplicationUser(userData);
                                applicationUser = userData;
                                return true;
                            }
                        }
                    }
                }
                else
                {
                    if (userData != null)
                    {
                        applicationUser = userData;
                        return true;
                    }

                }
            }

            return false;
        }

        /// <summary>
        /// 登陆
        /// </summary>
        /// <returns></returns>

        [CrossSite]
        [HttpPost]
        [Route("DoLogin")]
        public IHttpActionResult DoLogin(LoginViewModel model)
        {
            try
            {
                FrontIdentityUser applicationUser;
                var result = this.LoginFactory(model, out applicationUser) ? SignInStatus.Success : (SignInStatus)12;

                switch (result)
                {
                    case SignInStatus.Success:
                        if (applicationUser != null)
                        {
                            if (applicationUser.IsNeedModifyPw == 1)
                            {
                                return Json(new { code = 300, msg = "请先重置密码" });
                            }
                        }
                        {
                            _AppContext.UserMessageRecordApp.InsertLoginChangePasswordMessage(applicationUser.Id);
                            return Json(new { code = 200, applicationUser.Id });
                        }
                    case SignInStatus.LockedOut:
                    //  return View("Lockout");
                    case SignInStatus.RequiresVerification:
                    //return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                    case SignInStatus.Failure:
                    default:
                        return Json(new { code = 500, msg = "登录失败" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { code = 400, msg = ex.Message });
            }
        }
        [IOVAuthorize]
        [HttpGet]
        public string Test()
        {
            return "hello world";
        }

        //public AccountController(ApplicationUserManager userManager,
        //    ISecureDataFormat<AuthenticationTicket> accessTokenFormat)
        //{
        //    UserManager = userManager;
        //    AccessTokenFormat = accessTokenFormat;
        //}

        /// <summary>
        /// 判断身份证是否已存在
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("IsExistIdentityNumber")]
        public IHttpActionResult IsExist(string identityNumber, string id)
        {
            var store = new FrontUserStore<FrontIdentityUser>();
            var membershipManager = new UserManager<FrontIdentityUser>(store);

            if (store.IsIdentityNumberRepeate(identityNumber, id))
                return this.Ok(new ReturnObject(new ReturnResult() { IsSuccess = true, Message = "系统中已存在此身份证号" }));
            else
                return this.Ok(new ReturnObject(new ReturnResult() { IsSuccess = false }));
        }
        /// <summary>
        /// 判断身份证是否已存在
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("IsExistIdentityNumber")]
        public IHttpActionResult IsExist(string identityNumber)
        {
            var store = new FrontUserStore<FrontIdentityUser>();
            var membershipManager = new UserManager<FrontIdentityUser>(store);

            if (store.IsIdentityNumberRepeate(identityNumber))
                return this.Ok(new ReturnObject(new ReturnResult() { IsSuccess = true, Message = "系统中已存在此身份证号" }));
            else
                return this.Ok(new ReturnObject(new ReturnResult() { IsSuccess = false }));
        }



        /// <summary>
        /// 判断手机号码是否存在
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("CheckPhoneNumberIsExist")]
        public IHttpActionResult IsPhoneNumberExist(string PhoneNumber)
        {
            var store = new FrontUserStore<FrontIdentityUser>();
            var membershipManager = new UserManager<FrontIdentityUser>(store);

            if (store.CheckPhoneNumberIsExist(PhoneNumber))
                return this.Ok(new ReturnObject(new ReturnResult() { IsSuccess = true, Message = "系统中已存在此手机号码" }));
            else
                return this.Ok(new ReturnObject(new ReturnResult() { IsSuccess = false }));
        }
        /// <summary>
        /// 判断邮箱是否存在
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("IsEmailExist")]
        public IHttpActionResult IsEmailExist(string email, string id)
        {
            var store = new FrontUserStore<FrontIdentityUser>();
            var membershipManager = new UserManager<FrontIdentityUser>(store);
            var userid = string.Empty;
            if (store.CheckEmailIsExist(email, id))
                return this.Ok(new ReturnObject(new ReturnResult() { IsSuccess = true, Message = "系统中已存在此邮箱" }));
            else
                return this.Ok(new ReturnObject(new ReturnResult() { IsSuccess = false }));
        }
        /// <summary>
        /// 判断邮箱是否存在
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("IsEmailExist")]
        public IHttpActionResult IsEmailExist(string email)
        {
            var store = new FrontUserStore<FrontIdentityUser>();
            var membershipManager = new UserManager<FrontIdentityUser>(store);
            var userid = string.Empty;
            if (store.CheckEmailIsExist(email))
                return this.Ok(new ReturnObject(new ReturnResult() { IsSuccess = true, Message = "系统中已存在此邮箱" }));
            else
                return this.Ok(new ReturnObject(new ReturnResult() { IsSuccess = false }));
        }
        /// <summary>
        ///   更具身份证号查询车辆信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("SelectCarListByIdentity")]
        public IHttpActionResult SelectCarListByIdentity(string Identity)
        {
            if (string.IsNullOrEmpty(Identity))
                return BadRequest("身份证号不能为空");
            var carList = _AppContext.CarServiceUserApp.SelectCarListByIdentity(Identity).ToList<Car>();
            if (carList == null)
            {
                return this.Ok(new ReturnObject("604", "对不起，没有此用户的车辆信息", null));
            }
            return Ok(new ReturnObject(carList));
        }
        /// <summary>
        /// 天猫支付
        /// </summary>
        /// <param name="paynumber"></param>
        /// <param name="dealerId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("PayByTmallRequest")]
        public IHttpActionResult PayByTmallRequest(string paynumber, string dealerId, string userId)
        {
            var store = new FrontUserStore<FrontIdentityUser>();
            var user = store.FindByIdAsync(userId).Result;
            IEnumerable<string> appkeys = null;
            string appkey = string.Empty;
            if (this.Request.Headers.TryGetValues("appkey", out appkeys))
            {
                appkey = appkeys.First();
            }
            //  var store = new FrontUserStore<FrontIdentityUser>();
            //  string userid = this.User.Identity.GetUserId();
            //var UserManager = new UserManager<FrontIdentityUser>(store);
            //var gradeUser = UserManager.FindById(userid);
            user.ApprovalStatus = (int)MembershipApprovalStatus.Activing;
            if (string.IsNullOrEmpty(paynumber))
            {
                return Json(new { code = 400, msg = "选择天猫支付必须输入支付码" });
            }
            if (!string.IsNullOrEmpty(paynumber))
            {
                user.PayNumber = paynumber;
            }
            user.IsPay = 2;
            var returnIntegralType = (int)_AppContext.CarServiceUserApp.GetReIntegralTypeByIdentity(user.IdentityNumber);
            if (returnIntegralType != -1)
            {

                user.Amount = returnIntegralType > 1 ? 50 : 100;

            }
            store.UpdateAsync(user);
            store.AddMembershipDealerRecord(userId, dealerId);
            var result = store.CreateMembershipRequest(userId, user.IdentityNumber, dealerId, string.Empty, appkey);
            if (result)
            {
                return Json(new { code = "200", msg = "保存成功" });
            }
            else
            {
                return Json(new { code = "400", msg = "保存失败，请重新输入" });
            }


        }

        /// <summary>
        /// 微信支付
        /// </summary>
        /// <param name="outTradeNo">商家单号</param>
        /// <param name="dealerId">经销商编号</param>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        [HttpGet]
        [Route("PayByWeixinRequest")]
        public IHttpActionResult PayByWeixinRequest(string outTradeNo, string dealerId, string userId)
        {
            var result = new ReturnObject("200", "保存成功", null);
            IEnumerable<string> appkeys = null;
            string appkey = string.Empty;
            if (this.Request.Headers.TryGetValues("appkey", out appkeys))
            {
                appkey = appkeys.First();
            }
            if (string.IsNullOrEmpty(outTradeNo))
            {
                result.Code = "400";
                result.Message = "您还没有完成缴费";
            }
            else
            {
                var payermentRecord = _AppContext.PaymentRecordApp.Find(outTradeNo);
                var userStore = new FrontUserStore<FrontIdentityUser>();
                var membershipObj = userStore.FindByIdAsync(userId).Result;
                if (membershipObj.IsPay == 0)
                {
                    //获取应缴费的金额
                    var rebateType = (int)_AppContext.CarServiceUserApp.GetReIntegralTypeByIdentity(membershipObj.IdentityNumber);
                    if (rebateType != -1)
                    {
                        membershipObj.Amount = rebateType > 1 ? 50 : 100;
                    }
                    else 
                    {
                        membershipObj.Amount = 0;
                    }
                    membershipObj.ApprovalStatus = (int)MembershipApprovalStatus.Activing;
                    membershipObj.IsPay = 2;
                    membershipObj.PayNumber = string.Format("微信支付商家单号：{0}", outTradeNo);
                    userStore.UpdateAsync(membershipObj);//修改用户状态为已提交支付
                    userStore.AddMembershipDealerRecord(userId, dealerId);//添加用户与经销商的消费关系
                    //创建一条申请缴费获取积分的记录
                    var isCreate = userStore.CreateMembershipRequest(userId, membershipObj.IdentityNumber, dealerId, string.Empty, appkey);
                    if (!isCreate)
                    {
                        result.Code = "400";
                        result.Message = "保存失败，请重新输入";
                    }
                }
                else
                {
                    result.Code = "400";
                    result.Message = "用户的申请信息已提交";
                }
            }            
            return this.Ok(result);
        }
        /// <summary>
        /// 车主认证 绑定车主
        /// </summary>
        /// <param name="identityNumber"></param>
        /// <param name="mtype"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("ToCheckCarownerSavenew")]
        public IHttpActionResult ToCheckCarownerSavenew(string userId, string identityNumber, string PaperWork,string source = "")
        {
            //获取请求渠道
            IEnumerable<string> appkeys = null;
            string appkey = string.Empty;            
            if (this.Request.Headers.TryGetValues("appkey", out appkeys))
            {
                appkey = appkeys.First();
            }
            var store = new FrontUserStore<FrontIdentityUser>();
            var user = store.FindByIdentityNumber(identityNumber).Result;//memberinfo查询该身份证号
            if (user == null)
            {
                user = store.FindByIdAsync(userId).Result; //根据当前用户id查询用户实体
            }

            if (store.IsIdentityNumberRepeate(identityNumber))
            {
                //判断当前用户是否是车主
                if (user.SystemMType > 1)
                {
                    //string phone = user.PhoneNumber.Substring(3, 4);
                    return Json(new
                    {
                        code = 301,
                        //msg = string.Format("您的证件号已经和{0}绑定,如有疑问请拨打:400-800-1100",user.PhoneNumber.Replace(phone,"****"))
                        msg = "很抱歉，您的身份未绑定成功，请联系bluemembers在线客服"
                    });
                }
            }
            //设置车主的类型
            //gradeUser.MType = mtype;
            user.IdentityNumber = identityNumber;
            //获取用户车辆信息
            var carList = _AppContext.CarServiceUserApp.SelectCarListByIdentity(identityNumber);
            if (carList == null || carList.Count() <= 0)
            {
                // UserManager.Update(gradeUser);
                //提示未匹配到您的车辆，请拨打客服电话
                return Json(new
                {
                    code = 301,
                    msg = CommonConst.NOCARTIP
                });
            }

            user.MLevel = Convert.ToInt32(_AppContext.DealerMembershipApp.GetFirstRegistMLevelByIdNumber(identityNumber));
            user.PaperWork = PaperWork;
            user.SystemMType = (int)MembershipType.WhitCar;
            user.AuthenticationTime = DateTime.Now;
            user.AuthenticationSource = appkey;
            #region  pagework
            store.AddPaperworkToMembership_Schedule(user);
            #endregion


            //判断交费级别
            var returnintegral = (int)_AppContext.CarServiceUserApp.GetReIntegralTypeByIdentity(user.IdentityNumber);

            if (user.IdentityNumber != null && user.PaperWork == "1")
            {

                if (user.IdentityNumber.Length == 15)
                {
                    var str = user.IdentityNumber.Substring(6, 6);
                    user.Birthday = "19" + str.Remove(2) + "-" + str.Substring(2, 2) + "-" + str.Substring(4, 2);
                }
                if (user.IdentityNumber.Length == 18)
                {
                    var str = user.IdentityNumber.Substring(6, 8);
                    user.Birthday = str.Remove(4) + "-" + str.Substring(4, 2) + "-" + str.Substring(6, 2);
                }
            }

            if (!string.IsNullOrEmpty(user.Birthday))
            {
                bool checkbirthday = true;
                try
                {
                    var birthday = Convert.ToDateTime(user.Birthday);
                }
                catch (Exception ex)
                {
                    checkbirthday = false;
                }
                if (!checkbirthday)
                {
                    return Json(new { code = 306, msg = "身份证填写错误" });
                }
            }
            //判断等级有效时间
            user.MLevelBeginDate = user.AuthenticationTime;
            if (user.MLevel == 11 || user.MLevel == 12)
            {
                user.MLevelInvalidDate = user.AuthenticationTime.AddYears(1);
            }
            store.UpdateAsync(user);
            if (returnintegral != -1)
            {
                return Json(new
                {
                    code = 302,
                    msg = "认证车主成功",
                    reintegralType = returnintegral
                });

            }


            return Json(new
            {
                code = 200,
                msg = "认证信息保存成功"
            });
        }

        /// <summary>
        /// 车主认证 绑定车主(针对新表)
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="pid">证件号码</param>
        /// <param name="paperType">证件类型</param>
        /// <param name="source">认证来源</param>
        /// <returns>操作结果</returns>
        [HttpGet]
        [Route("authcarowner")]
        public IHttpActionResult AuthCarOwner(string userId, string pid, string paperType,string source = "")
        {
            //获取请求渠道
            IEnumerable<string> appkeys = null;
            string appkey = string.Empty;
            if (this.Request.Headers.TryGetValues("appkey", out appkeys))
            {
                appkey = appkeys.First();
            }
            var authResult = new ReturnObject("200", "认证车主成功", null);
            var userStore = new FrontUserStore<FrontIdentityUser>();            
            var userObj = userStore.FindByIdentityNumber(pid).Result;//membership里查询该身份证号  
            var isPIDExist = userObj != null;
            //如果证件号存在
            if (userObj != null)
            {
                //如果证件号已存在并绑定到车主，不能绑定
                if (userObj.SystemMType > 1)
                {
                    authResult.Code = "301";
                    authResult.Message = "很抱歉，您的身份未绑定成功，请联系bluemembers在线客服";
                }
            }
            else
            {
                userObj = userStore.FindByIdAsync(userId).Result; //membership里查询该用户ID       
            }
            //获取用户车辆信息
            var carQuery = _AppContext.CarServiceUserApp.CarsByPID(pid);
            if (carQuery != null && carQuery.Count() > 0)//有车
            {
                var memberLevel = _AppContext.DealerMembershipApp.GetLevel(pid);//获取用户级别
                userObj.MLevel = Convert.ToInt32(memberLevel);
                userObj.PaperWork = paperType;
                userObj.SystemMType = (int)MembershipType.WhitCar;
                userObj.IdentityNumber = pid;
                userObj.AuthenticationTime = DateTime.Now;
                userObj.AuthenticationSource = appkey;
                //认证来源
                switch (source)
                {
                    case "activity_trip":
                        userObj.Remark = "source:activity_trip";//机场活动
                        break;   
                    case "activity_verna":
                        userObj.Remark = "source:activity_verna";//悦纳上市活动
                        break;                                   
                    default:
                        userObj.Remark = "source:none";//无
                        break;
                }                
                //提取生日信息
                if (userObj.IdentityNumber != null && userObj.PaperWork == "1")
                {
                    if (userObj.IdentityNumber.Length == 15)
                    {
                        var str = userObj.IdentityNumber.Substring(6, 6);
                        userObj.Birthday = "19" + str.Remove(2) + "-" + str.Substring(2, 2) + "-" + str.Substring(4, 2);
                    }
                    if (userObj.IdentityNumber.Length == 18)
                    {
                        var str = userObj.IdentityNumber.Substring(6, 8);
                        userObj.Birthday = str.Remove(4) + "-" + str.Substring(4, 2) + "-" + str.Substring(6, 2);
                    }
                    DateTime birthDay = DateTime.Now;
                    var isBirthday = DateTime.TryParse(userObj.Birthday, out birthDay);
                    if (!isBirthday)
                    {
                        authResult.Code = "306";
                        authResult.Message = "身份证填写错误";
                    }
                }
                //一切正常，绑定用户
                if (authResult.Code == "200")
                {
                    userObj.MLevelBeginDate = userObj.AuthenticationTime;
                    if (userObj.MLevel == 11 || userObj.MLevel == 12)
                    {
                        userObj.MLevelInvalidDate = userObj.AuthenticationTime.AddYears(1);
                    }
                    //添加用户和证件类型的对应关系
                    userStore.AddPaperworkToMembership_Schedule(userObj);
                    //修改用户信息
                    userStore.UpdateAsync(userObj);
                    //判断交费级别
                    var integralType = (int)_AppContext.CarServiceUserApp.GetReIntegralTypeByIdentity(pid);
                    if (integralType != -1)
                    {
                        authResult.Content = integralType;
                    }
                }
            }
            else//无车
            {
                authResult.Code = "301";
                authResult.Message = CommonConst.NOCARTIP;
            }       
            return this.Ok(authResult);
        }

        /// <summary>
        /// 车主认证 绑定车主
        /// </summary>
        /// <param name="identityNumber"></param>
        /// <param name="mtype"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("ToCheckCarownerSaveForLottery")]
        public IHttpActionResult ToCheckCarownerSaveForLottery(string userId, string identityNumber, string PaperWork)
        {
            var store = new FrontUserStore<FrontIdentityUser>();
            if (store.IsIdentityNumberRepeate(identityNumber))
            {
                return Json(new
                {
                    code = 301,
                    msg = "此身份证号已被其他车主绑定,如有疑问请拨打:400-800-1100"
                });
            }
            var user = store.FindByIdAsync(userId).Result;
            user.IdentityNumber = identityNumber;
            //获取用户车辆信息
            var carList = _AppContext.CarServiceUserApp.SelectCarListByIdentity(identityNumber);
            if (carList == null || carList.Count() <= 0)
            {
                // UserManager.Update(gradeUser);
                //提示未匹配到您的车辆，请拨打客服电话
                return Json(new
                {
                    code = 301,
                    msg = CommonConst.NOCARTIP
                });
            }

            user.MLevel = Convert.ToInt32(_AppContext.DealerMembershipApp.GetFirstRegistMLevelByIdNumber(identityNumber));
            user.PaperWork = PaperWork;
            user.SystemMType = (int)MembershipType.WhitCar;

            #region  pagework
            store.AddPaperworkToMembership_Schedule(user);
            #endregion


            //判断交费级别
            var returnintegral = (int)_AppContext.CarServiceUserApp.GetReIntegralTypeByIdentity(user.IdentityNumber);
            store.UpdateAsync(user);
            return Json(new
            {
                code = 200,
                msg = "绑定成功"
            });
        }

        /// <summary>
        /// 车主认证
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("ToCheckCarownerSave")]
        public IHttpActionResult ToCheckCarownerSave(string userName, string identityNumber, int mtype, string Id)
        {
            var errorcode = 0;
            try
            {
                var store = new FrontUserStore<FrontIdentityUser>();
                if (store.IsIdentityNumberRepeate(identityNumber))
                {
                    errorcode = 1;
                    // if (store.IsIdentityNumberId(identityNumber,Id))
                    //{

                    //}
                    var account = new FrontUserStore<FrontIdentityUser>().FindByIdAsync(Id);
                    var identityUser = account.Result;
                    if (identityUser.IdentityNumber == identityNumber)
                    {
                        errorcode = 2;
                        var carList = _AppContext.CarServiceUserApp.SelectCarListByIdentity(identityNumber);
                        if (carList == null || carList.Count() <= 0)
                        {
                            //UserManager.Update(gradeUser);
                            //提示未匹配到您的车辆，请拨打客服电话
                            return Json(new
                            {
                                Code = 304,
                                msg = "您已绑定此身份证号！但未匹配到您的车辆，由于车辆信息同步有些延时，您可稍后再试或直接拨打客服电话！"
                            });

                        }
                        else
                        {
                            return Json(new
                            {
                                Code = 202,
                                Content = carList
                            });
                        }
                        //return Json(new
                        //{
                        //    code = 306,
                        //    msg = "您已绑定此身份证号！"
                        //});
                    }
                    return Json(new
                    {
                        Code = 305,
                        msg = "此身份证号已被其他车主绑定,如有疑问请拨打:400-800-1100"
                    });

                }
                else
                {
                    var UserManager = new UserManager<FrontIdentityUser>(store);

                    var gradeUser = UserManager.FindById(Id);
                    //设置车主的类型
                    gradeUser.MType = mtype;
                    gradeUser.IdentityNumber = identityNumber;
                    //获取用户车辆信息
                    var carList = _AppContext.CarServiceUserApp.SelectCarListByIdentity(identityNumber);
                    if (carList == null || carList.Count() <= 0)
                    {
                        UserManager.Update(gradeUser);
                        //提示未匹配到您的车辆，请拨打客服电话
                        return Json(new
                        {
                            Code = 304,
                            msg = "您已绑定此身份证号！但未匹配到您的车辆，由于车辆信息同步有些延时，您可稍后再试或直接拨打客服电话！"
                        });

                    }
                    else
                    {
                        UserManager.Update(gradeUser);
                        return Json(new
                        {
                            Code = 202,
                            Content = carList
                        });
                    }

                    //    gradeUser.MLevel = GetUserLevel(carList);

                    //    gradeUser.SystemMType = (int)MembershipType.WhitCar;
                    //    //校验索九车主
                    //    var _isSonata = _AppContext.SonataServiceApp.IsSonataUser(gradeUser.IdentityNumber);
                    //    //提示缴费
                    //    if (_isSonata)
                    //    {
                    //        gradeUser.SystemMType = (int)MembershipType.Sonata9;
                    //        gradeUser.IsPay = 0;
                    //        gradeUser.ApprovalStatus = (int)MembershipApprovalStatus.NeedPay;
                    //        UserManager.Update(gradeUser);

                    //        //跳转到缴费页面
                    //        //DO Something
                    //        return Json(new
                    //        {
                    //            code = 302,
                    //            msg = "已提交认证请求,请耐心等候！"
                    //        });

                    //    }
                    //    UserManager.Update(gradeUser);
                    //    return Json(new
                    //    {
                    //        code = 200,
                    //        msg = "认证信息保存成功"
                    //    });
                }
            }
            catch
            {
                return Json(new
                {
                    Code = 404,
                    msg = "系统异常"
                });

            }
        }

        /// <summary>
        /// 判断昵称是否存在
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("CheckNickNameIsExist")]
        public IHttpActionResult IsNickNameExist(string NickName)
        {
            var store = new FrontUserStore<FrontIdentityUser>();
            var membershipManager = new UserManager<FrontIdentityUser>(store);

            if (store.CheckNickNameIsExist(NickName))
                return this.Ok(new ReturnObject(new ReturnResult() { IsSuccess = true, Message = "系统中已存在此用户名" }));
            else
                return this.Ok(new ReturnObject(new ReturnResult() { IsSuccess = false }));
        }

        /// <summary>
        /// 判断用户名称密码是否存在ToCheckCarownerSave
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("IsLoginExist")]
        public IHttpActionResult IsLoginExist(string NickName, string Password)
        {
            var userStore = new FrontUserStore<FrontIdentityUser>();
            var userManager = new UserManager<FrontIdentityUser>(userStore);
            var model = new FrontIdentityUser();
            //var id = new FrontUserStore<FrontIdentityUser>().FindByNickNameAsync(NickName);

            //var  password = userManager.PasswordHasher.HashPassword(model.Password);
            //if (userStore.IsLoginExist(NickName, Password).ToString()=="-1")
            //{
            //    return this.Ok(new ReturnObject(new ReturnResult() { IsSuccess = false, Message = "用户名不存在" }));
            //}
            var account = new FrontUserStore<FrontIdentityUser>().IsLoginExist(NickName, Password);


            //if (userStore.IsLoginExist(NickName, Password)){
            //var id1= model.Id;
            return this.Ok(new ReturnObject(new ReturnResult() { Message = model.Id }));
            //}else{
            //return this.Ok(new ReturnObject(new ReturnResult() { IsSuccess = false, Message = "用户名或密码错误" }));
            // }
        }


        /// <summary>
        /// 获取当前已登录的帐号信息
        /// </summary>
        /// <returns>账号信息</returns>
        [HttpGet]
        [IOVAuthorize]
        [Route("CurrentMembershipData")]
        [ResponseType(typeof(MembershipModel))]
        public IHttpActionResult CurrentMembershipData()
        {
            try
            {
                var account = new FrontUserStore<FrontIdentityUser>().FindByIdAsync(this.User.Identity.Name);
                if (account == null || account.Result == null)
                    return BadRequest("账号不存在");
                var userBlueBean = _AppContext.UserBlueBeanApp.GetTotalBlueBean(account.Result.Id);
                var userIntegral = _AppContext.UserIntegralApp.

                    GetTotalIntegral(account.Result.Id);
                var jingYanZhi = 0;

                var identityUser = account.Result;
                var userInfo = new MembershipModel
                {
                    Id = identityUser.Id,
                    IdentityNumber = identityUser.IdentityNumber,
                    LevelName = ((MemshipLevel)identityUser.MLevel).GetDiscribe(),
                    PhoneNumber = identityUser.UserName,
                    RealName = identityUser.RealName,
                    NickName = identityUser.NickName,
                    MemberCardNumber = identityUser.No,
                    FaceImage = identityUser.FaceImage,
                    StatusName = ((MembershipStatus)identityUser.Status).GetDiscribe(),
                    Integral = userIntegral,
                    BlueBean = userBlueBean,
                    UserType = ((MembershipType)identityUser.SystemMType).GetDiscribe(),
                    UserTypeInt = identityUser.SystemMType,
                    JingYanZhi = _AppContext.UserEmpiricApp.TotalValue(identityUser.Id),
                    GenderName = string.IsNullOrEmpty(identityUser.Gender) ? "" : identityUser.Gender.Equals("1") ? "男" : "女",
                    Birthday = identityUser.Birthday,
                    CreateTime = identityUser.CreateTime,
                    IsPay = identityUser.IsPay,
                    Address = identityUser.Address,
                    Interest = !string.IsNullOrEmpty(identityUser.Interest) ? identityUser.Interest.Split(',').ToList() : new List<string>(1),
                    IsNeedModifyPw = identityUser.IsNeedModifyPw,//是否需要修改密码
                    NEducational = identityUser.Educational,
                    NIndustry = identityUser.Industry,
                    NIsMarriage = identityUser.IsMarriage,
                    NJob = identityUser.Job,
                    NMainContact = identityUser.MainContact,
                    NMainTelePhone = identityUser.MainTelePhone,
                    NMarriageDay = identityUser.MarriageDay,
                    NOffice = identityUser.Office,
                    NOrganizationCode = identityUser.OrganizationCode,
                    NPaperWork = identityUser.PaperWork,
                    NRemark = identityUser.Remark,
                    NTelePhone = identityUser.TelePhone,
                    NTransactionTime = identityUser.TransactionTime,
                    Area = identityUser.Area,
                    City = identityUser.City,
                    Email = identityUser.Email,
                    Provency = identityUser.Provency
                };

                if (identityUser.IsNeedModifyPw == 1)
                {

                }
                return Ok(new ReturnObject(userInfo));
            }
            catch (Exception ex)
            {
                LogService.Instance.Error(ex.Message);
                return Ok(new ReturnObject("500"));
            }
        }

        /// <summary>
        /// 获取当前已登录的帐号信息
        /// </summary>
        /// <returns>账号信息</returns>
        [HttpGet]
        [IOVAuthorize]
        [Route("me")]
        [ResponseType(typeof(MembershipModel))]
        public IHttpActionResult Me()
        {
            try
            {
                var account = new FrontUserStore<FrontIdentityUser>().FindByIdAsync(this.User.Identity.Name);
                if (account == null || account.Result == null)
                    return BadRequest("账号不存在");
                var userBlueBean = _AppContext.UserBlueBeanApp.GetTotalBlueBean(account.Result.Id);
                var userIntegral = _AppContext.UserIntegralApp.GetTotalIntegral(account.Result.Id);
                var jingYanZhi = 0;

                var identityUser = account.Result;
                var userInfo = new MembershipModel
                {
                    Id = identityUser.Id,
                    IdentityNumber = identityUser.IdentityNumber,
                    LevelName = ((MemshipLevel)identityUser.MLevel).GetDiscribe(),
                    PhoneNumber = identityUser.UserName,
                    RealName = identityUser.RealName,
                    NickName = identityUser.NickName,
                    MemberCardNumber = identityUser.No,
                    FaceImage = identityUser.FaceImage,
                    StatusName = ((MembershipStatus)identityUser.Status).GetDiscribe(),
                    Integral = userIntegral,
                    BlueBean = userBlueBean,
                    UserType = ((MembershipType)identityUser.SystemMType).GetDiscribe(),
                    UserTypeInt = identityUser.SystemMType,
                    JingYanZhi = _AppContext.UserEmpiricApp.TotalValue(identityUser.Id),
                    GenderName = string.IsNullOrEmpty(identityUser.Gender) ? "" : identityUser.Gender.Equals("1") ? "男" : "女",
                    Birthday = identityUser.Birthday,
                    CreateTime = identityUser.CreateTime,
                    IsPay = identityUser.IsPay,
                    Address = identityUser.Address,
                    Interest = !string.IsNullOrEmpty(identityUser.Interest) ? identityUser.Interest.Split(',').ToList() : new List<string>(1),
                    IsNeedModifyPw = identityUser.IsNeedModifyPw,//是否需要修改密码
                    Vin = _AppContext.UserWxBindApp.GetOneVinByUserId(identityUser.Id)
                };


                if (identityUser.IsNeedModifyPw == 1)
                {

                }
                return Ok(new ReturnObject(userInfo));
            }
            catch (Exception ex)
            {
                LogService.Instance.Error(ex.Message);
                return Ok(new ReturnObject("500"));
            }
        }

        /// <summary>
        /// 根据会员昵称获取会员Id
        /// </summary>
        /// <param name="nickName">会员昵称</param>
        /// <returns></returns>
        [HttpGet]
        //[IOVAuthorize]
        [ResponseType(typeof(string))]
        [Route("MemberId")]
        public IHttpActionResult MemberId(string nickName)
        {
            if (string.IsNullOrEmpty(nickName))
            {
                return BadRequest("会员昵称不能为空。");
            }

            var account = new FrontUserStore<FrontIdentityUser>().FindByNickNameAsync(nickName);

            if (account == null || account.Result == null)
            {
                return BadRequest("会员昵称不存在");
            }

            return Ok(new ReturnObject(account.Result.Id));
        }

        /// <summary>
        /// 根据会员Id获取会员信息
        /// </summary>
        /// <param name="id">会员Id</param>
        /// <returns></returns>
        [HttpPost]
        //[IOVAuthorize]
        [Route("MemberInfo")]
        [ResponseType(typeof(MembershipModel))]
        public IHttpActionResult MemberInfo(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("会员id不能为空。");
            }

            var account = new FrontUserStore<FrontIdentityUser>().FindByIdAsync(id);

            if (account == null || account.Result == null)
            {
                return BadRequest("会员账号不存在");
            }

            var identityUser = account.Result;

            var userIntegral = _AppContext.UserIntegralApp.GetTotalIntegral(identityUser.Id);


            var userInfo = new MembershipModel
            {
                Id = identityUser.Id,
                Integral = userIntegral,
                LoginName = identityUser.UserName,
                IdentityNumber = identityUser.IdentityNumber,
                LevelName = ((MemshipLevel)identityUser.MLevel).GetDiscribe(),
                MLevel = identityUser.MLevel,
                SystemMType = identityUser.SystemMType,
                MType = identityUser.MType,
                PhoneNumber = identityUser.UserName,
                RealName = identityUser.RealName,
                NickName = identityUser.NickName,
                MemberCardNumber = identityUser.No,
                FaceImage = identityUser.FaceImage,
                StatusName = ((MembershipStatus)identityUser.Status).GetDiscribe(),
                UserType = ((MembershipType)identityUser.SystemMType).GetDiscribe(),
                UserTypeInt = identityUser.SystemMType,
                JingYanZhi = _AppContext.UserEmpiricApp.TotalValue(identityUser.Id),
                GenderName = string.IsNullOrEmpty(identityUser.Gender) ? "" : identityUser.Gender.Equals("1") ? "男" : "女",
                Birthday = identityUser.Birthday,
                CreateTime = identityUser.CreateTime,
                MLevelBeginDate = identityUser.MLevelBeginDate,
                MLevelInvalidDate = identityUser.MLevelInvalidDate,
                IsPay = identityUser.IsPay,
                Address = identityUser.Address,
                CarTypeReturnIntegral = _AppContext.CarServiceUserApp.GetReIntegralTypeByIdentity(identityUser.IdentityNumber),
                Interest = !string.IsNullOrEmpty(identityUser.Interest) ? identityUser.Interest.Split(',').ToList() : new List<string>(1),
                IsNeedModifyPw = identityUser.IsNeedModifyPw,//是否需要修改密码
                RankId = identityUser.RankID,
                AuthenticationSource = identityUser.AuthenticationSource
            };
            //判断是否是个人用户
            bool flag = _AppContext.DealerMembershipApp.IsPersonalUser(identityUser.IdentityNumber);
            if (!flag)
            {
                userInfo.CarTypeReturnIntegral = CarTypeReturnIntegral.NoIntegral;
            }
            return Ok(new ReturnObject(userInfo));
        }





        /// <summary>
        /// 根据会员Id获取会员信息2
        /// </summary>
        /// <param name="id">会员Id</param>
        /// <returns></returns>
        [HttpGet]
        //[IOVAuthorize]
        [Route("MemberInfo2")]
        [ResponseType(typeof(MembershipModel))]
        public IHttpActionResult MemberInfo2(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("会员id不能为空。");
            }

            var account = new FrontUserStore<FrontIdentityUser>().FindByIdAsync(id);

            if (account == null || account.Result == null)
            {
                return BadRequest("会员账号不存在");
            }

            var identityUser = account.Result;


            var userInfo = new MembershipModel
            {


                GenderName = string.IsNullOrEmpty(identityUser.Gender) ? "" : identityUser.Gender.Equals("1") ? "男" : "女",
                Area = identityUser.Area,
                Provency = identityUser.Provency,
                City = identityUser.City,
                NickName = identityUser.NickName,
                RealName = identityUser.RealName,
                Email = identityUser.Email,
                Address = identityUser.Address,
                NMainTelePhone = identityUser.TelePhone,
                NTransactionTime = identityUser.TransactionTime,
                NJob = identityUser.Job,
                NOffice = identityUser.Office,
                NIsMarriage = identityUser.IsMarriage,
                NMarriageDay = identityUser.MarriageDay,
                NEducational = identityUser.Educational,
                NRemark = identityUser.Remark,
                Id = identityUser.Id,
                IdentityNumber = identityUser.IdentityNumber,
                //LevelName = ((MemshipLevel)identityUser.MLevel).GetDiscribe(),
                PhoneNumber = identityUser.PhoneNumber,
                //RealName = identityUser.RealName,
                //NickName = identityUser.NickName,
                //MemberCardNumber = identityUser.No,
                //FaceImage = identityUser.FaceImage,
                StatusName = ((MembershipStatus)identityUser.Status).GetDiscribe(),
                UserType = ((MembershipType)identityUser.SystemMType).GetDiscribe(),
                UserTypeInt = identityUser.SystemMType,
                //JingYanZhi = _AppContext.UserEmpiricApp.TotalValue(identityUser.Id),
                //GenderName = string.IsNullOrEmpty(identityUser.Gender) ? "" : identityUser.Gender.Equals("1") ? "男" : "女",
                Birthday = identityUser.Birthday,
                CreateTime = identityUser.CreateTime,
                //IsPay = identityUser.IsPay,
                //Address = identityUser.Address,
                Interest = !string.IsNullOrEmpty(identityUser.Interest) ? identityUser.Interest.Split(',').ToList() : new List<string>(1),
                IsNeedModifyPw = identityUser.IsNeedModifyPw//是否需要修改密码
            };

            return Ok(new ReturnObject(userInfo));
        }

        /// <summary>
        /// 获取手机验证码
        /// </summary>
        /// <param name="phone">手机号</param>
        /// <returns></returns>
        [HttpPost]
        //[IOVAuthorize]
        [Route("ValidateCode")]
        public IHttpActionResult ValidateCode(string phone)
        {
            if (string.IsNullOrEmpty(phone))
            {
                return BadRequest("手机号不能为空。");
            }

            ReturnResult result = _AppContext.UserSecurityApp.SendMobileVerifyCode(phone, 4, "");
            return Ok(result.IsSuccess);
        }

        /// <summary>
        /// 改变用户登录账号
        /// </summary>
        /// <param name="phone">手机号</param>
        /// <param name="code">验证码</param>
        /// <returns></returns>
        [HttpPost]
        [IOVAuthorize]
        [Route("ChangeAccount")]
        public IHttpActionResult ChangeAccount(string phone, string code)
        {
            if (string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(code))
            {
                return BadRequest("手机号或验证码不能为空。");
            }

            var account = new FrontUserStore<FrontIdentityUser>().FindByNameAsync(phone.Trim());

            if (account != null && account.Result != null)
            {
                return BadRequest("用户账号已存在。");
            }

            ReturnResult _captchaResult = _AppContext.UserSecurityApp.ValidateMobileVerifyCode(phone, code);

            if (!_captchaResult.IsSuccess)
            {
                return BadRequest("验证码已过期。");
            }
            var membershipData = new FrontUserStore<FrontIdentityUser>().FindByIdAsync(this.User.Identity.GetUserId());

            membershipData.Result.UserName = phone;
            membershipData.Result.PhoneNumber = phone;
            new FrontUserStore<FrontIdentityUser>().UpdateAsync(membershipData.Result);
            return Ok(true);
        }

        /// <summary>
        /// 修改会员手机号
        /// </summary>
        /// <param name="paras"></param>
        /// <returns></returns>
        [HttpPost]
        //[IOVAuthorize]
        [Route("UpdatePhone")]
        public IHttpActionResult UpdatePhone(MembershipPhone paras)
        {
            if (paras == null || string.IsNullOrEmpty(paras.Phone))
            {
                return BadRequest("手机号不能为空。");
            }

            var userStore = new FrontUserStore<FrontIdentityUser>();
            var user = userStore.FindByIdAsync(paras.Id).Result;

            if (user != null)
            {
                user.PhoneNumber = paras.Phone;
                user.UserName = paras.Phone;
                userStore.UpdateAsync(user);
                return Ok(true);
            }

            return Ok(false);
        }

        /// <summary>
        /// 根据VIN查询会员等级
        /// </summary>
        /// <param name="vin">VIN</param>
        /// <param name="phone">购车手机号</param>
        /// <returns></returns>
        //[HttpGet]
        //[IOVAuthorize]
        //[ResponseType(typeof(MembershipLevelV))]
        //[Route("FindLevel")]
        //public IHttpActionResult FindLevel(string vin)
        //{
        //    if (string.IsNullOrEmpty(vin))
        //    {
        //        return BadRequest("VIN和手机号不能为空。");
        //    }

        //    var userStore = new FrontUserStore<FrontIdentityUser>();
        //    var result = userStore.FindLevel(vin);
        //    MembershipLevelV level = new MembershipLevelV();

        //    if (result!=null)
        //    {
        //        level.Level = result.MLevel;

        //        if (result.MLevel==3&&!string.IsNullOrEmpty(result.No))
        //        {
        //            level.Type = 3;
        //        }
        //        else
        //        {
        //            if ((result.MLevel == 2 || result.MLevel == 3) && string.IsNullOrEmpty(result.No))
        //            {
        //                level.Type = 2;
        //            }
        //            else
        //            {
        //                if (result.MLevel==1)
        //                {
        //                    level.Type = 1;
        //                }
        //            }
        //        }
        //    }

        //    return Ok(new ReturnObject() { Content = level });
        //}

        /// <summary>
        /// 根据VIN查询会员等级
        /// </summary>
        /// <param name="vin">VIN</param>
        /// <param name="phone">购车手机号</param>
        /// <returns></returns>
        [HttpGet]
        [Route("FindLevel")]
        public IHttpActionResult FindLevel(string vin)
        {
            if (string.IsNullOrEmpty(vin))
            {
                return BadRequest("VIN号不能为空。");
            }

            if (vin.Length < 8)
            {
                return BadRequest("VIN号不能少于8位。");
            }

            var userStore = new FrontUserStore<FrontIdentityUser>();
            var result = userStore.FindLevel(vin);

            var isMember = "N";
            var isSonata9 = "N";
            if (result == null) isMember = "N";

            foreach (var row in result)
            {
                isMember = "Y";

                var carCategory = row["CarCategory"];

                if (carCategory == "索纳塔9" || carCategory == "第九代索纳塔" || carCategory == "第九代索纳塔")
                    isSonata9 = "Y";
            }

            return Ok(new ReturnObject(new { IsMember = isMember, IsSonata9 = isSonata9 }));
        }

        /// <summary>
        /// 填写个人信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        // [IOVAuthorize]
        [Route("FillinData")]
        public IHttpActionResult FillinData(UpdateMembershipModel model)
        {

            #region ==== 新逻辑 ====

            var userStore = new FrontUserStore<FrontIdentityUser>();
            var _curUser = userStore.FindByIdAsync(model.Id).Result;
            //个人信息
            _curUser.MembershipId = model.Id;
            _curUser.RealName = model.RealName;
            _curUser.NickName = model.NickName;
            _curUser.Birthday = model.Birthday;
            _curUser.Email = model.Email;
            _curUser.Address = model.Address;
            _curUser.FaceImage = model.FaceImage;
            _curUser.Provency = model.Provency;
            _curUser.City = model.City;
            _curUser.Area = model.Area;
            _curUser.Gender = model.GenderName == "女" ? "2" : "1";
            // _curUser.IdentityNumber = model.IdentityNumber;

            //var membershipManager = new UserManager<FrontIdentityUser>(userStore);
            //if (userStore.CheckEmailIsExist(_curUser.Email, _curUser.MembershipId))
            //    return this.Ok(new ReturnObject(new ReturnResult() { IsSuccess = true, Message = "系统中已存在此邮箱" }));

            userStore.UpdateAsync(_curUser);

            //详细信息
            _curUser.PaperWork = model.NPaperWork;
            _curUser.MainContact = model.NMainContact;
            _curUser.MainTelePhone = model.NMainTelePhone;
            _curUser.TelePhone = model.NTelePhone;
            _curUser.TransactionTime = model.NTransactionTime;
            _curUser.Industry = model.NIndustry;
            _curUser.Job = model.NJob;
            _curUser.IsMarriage = model.NIsMarriage;
            _curUser.MarriageDay = model.NMarriageDay;
            _curUser.Educational = model.NEducational;
            _curUser.SendSms = 0;
            _curUser.SendLetter = 0;
            _curUser.MakePhone = 0;
            _curUser.SendEmail = 0;
            _curUser.Remark = model.NRemark;
            _curUser.Office = model.NOffice;

            var appro = new FrontUserStore<FrontIdentityUser>().Update_Or_Insert_Membership_Schedule(_curUser);

            #endregion

            var result = true;

            try
            {
                int outValue;
                //_AppContext.BreadApp.BlueBeanBread(
                //    EBRuleType.完善个人信息,
                //    model.Id,
                //    (MemshipLevel)_curUser.MLevel,
                //    out outValue);
                //_AppContext.BreadApp.EmpiricBread(EEmpiricRule.完善个人信息, model.Id, out outValue);
            }
            catch (Exception ex)
            {
                LogService.Instance.Error("增加蓝豆失败: " + model.Id);
                LogService.Instance.Error(ex.Message, ex);
            }

            return Ok(result);
        }

        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        //[IOVAuthorize]
        [Route("UpdateUserInfo")]
        public IHttpActionResult UpdateUserInfo(UpdateMembershipModel model)
        {
            var result = true;
            var userStore = new FrontUserStore<FrontIdentityUser>();
            var user = userStore.FindByIdAsync(model.Id).Result;
            user.IdentityNumber = model.IdentityNumber;
            user.NickName = model.NickName;
            user.FaceImage = model.FaceImage;
            user.Gender = model.GenderName == "女" ? "2" : "1";
            user.Birthday = model.Birthday;
            user.Address = model.Address;
            user.UpdateTime = DateTime.Now.ToString();
            user.Interest = model.Interest != null && model.Interest.Count() > 0 ? string.Join(",", model.Interest) : "";
            userStore.UpdateAsync(user);

            try
            {
                int outValue;
                //_AppContext.BreadApp.BlueBeanBread(
                //    EBRuleType.完善个人信息,
                //    model.Id,
                //    (MemshipLevel)user.MLevel,
                //    out outValue);
                //_AppContext.BreadApp.EmpiricBread(EEmpiricRule.完善个人信息, model.Id, out outValue);
            }
            catch (Exception ex)
            {
                LogService.Instance.Error("增加蓝豆失败: " + model.Id);
                LogService.Instance.Error(ex.Message, ex);
            }

            return Ok(result);
        }
        /// <summary>
        /// 更新用户信息2
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        // [IOVAuthorize]
        [Route("UpdateUserInfo2")]
        public IHttpActionResult UpdateUserInfo2(UpdateMembershipModel model)
        {
            var result = true;
            var userStore = new FrontUserStore<FrontIdentityUser>();
            var user = userStore.FindByIdAsync(model.Id).Result;
            user.UserName = model.UserName;
            user.Birthday = model.Birthday;
            user.Email = model.Email;
            user.RealName = model.RealName;
            user.Gender = model.GenderName;
            user.Provency = model.Provency;
            user.City = model.City;
            user.Area = model.Area;
            user.Address = model.Address;
            user.TelePhone = model.NTelePhone;
            user.TransactionTime = model.NTransactionTime;
            user.Job = model.NJob;
            user.Office = model.NOffice;
            user.IsMarriage = model.NIsMarriage == "已婚" ? "0" : "1";
            user.MarriageDay = model.NMarriageDay;
            user.Educational = model.NEducational;
            user.Remark = model.NRemark;

            user.UpdateTime = DateTime.Now.ToString();
            user.Interest = model.Interest != null && model.Interest.Count() > 0 ? string.Join(",", model.Interest) : "";
            userStore.UpdateAsync2(user);
            //try
            //{
            //    int outValue;
            //    _AppContext.BreadApp.BlueBeanBread(
            //        EBRuleType.完善个人信息,
            //        model.Id,
            //        (MemshipLevel)user.MLevel,
            //        out outValue);
            //    _AppContext.BreadApp.EmpiricBread(EEmpiricRule.完善个人信息, model.Id, out outValue);
            //}
            //catch (Exception ex)
            //{
            //    LogService.Instance.Error("增加蓝豆失败: " + model.Id);
            //    LogService.Instance.Error(ex.Message, ex);
            //}

            return Ok(result);
        }
        /// <summary>
        /// 设置用户信息
        /// </summary>
        /// <returns></returns>
        /// 

        //private ApplicationUserManager _userManager;

        //public ApplicationUserManager UserManager
        //{
        //    get
        //    {
        //        return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
        //    }
        //    private set
        //    {
        //        _userManager = value;
        //    }
        //}
        //public IHttpActionResult SetUserInfo(UpdateMembershipModel model)
        //{
        //    //if (!this.User.Identity.IsAuthenticated)
        //    //{
        //    //    return RedirectToAction("LogonPage", "Account", new
        //    //    {
        //    //        returnUrl = "/MyCenter/SetUserInfo"
        //    //    });
        //    //}
        //    ApplicationUser _result = null;
        //    int _totalScore = 0;
        //    int _totalBlueBean = 0;
        //    int _totalCard = 0;
        //    int _totalEmpiric = 0;
        //    if (this.User.Identity.IsAuthenticated)
        //    {
        //        _totalScore = _AppContext.UserIntegralApp.GetTotalIntegral(this.User.Identity.GetUserId());
        //        _totalBlueBean = _AppContext.UserBlueBeanApp.GetTotalBlueBean(this.User.Identity.GetUserId());
        //        _result = UserManager.FindById(this.User.Identity.GetUserId());
        //        _totalCard = _AppContext.AirportServiceApp.SelectSNCardByUser(this.User.Identity.GetUserId()).ToList().Count();
        //        _totalEmpiric = _AppContext.UserEmpiricApp.TotalValue(this.User.Identity.GetUserId());
        //    }


        //    //ViewData["provinceList"] = CityService.Instance.GetProvince();
        //    return Ok(new ReturnObject(200));
        //}


        /// <summary>
        /// 更新微信用户信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [IOVAuthorize]
        [Route("UpdateWXUserInfo")]
        public IHttpActionResult UpdateWXUserInfo(WXUpdateMembershipModel model)
        {
            //  string vin = model.Vin;


            //if (!string.IsNullOrEmpty(vin))
            //{
            //    if (vin.Length != 17) return this.Ok(new ReturnObject("602", "Vin输入错误！", null));

            //    var userByVin = _AppContext.UserInfoApp.SelectOneByVin(vin);
            //    if (userByVin != null && userByVin.Id != model.Id)
            //    {
            //        return this.Ok(new ReturnObject("400", "该VIN码属于其他会员！", null));
            //    }

            //    IFCustomer customer;
            //    _AppContext.CarServiceUserApp.GetCarInfoByVIN(vin, out customer);

            //    if (customer == null || string.IsNullOrEmpty(customer.IdentityNumber))
            //    {
            //        return this.Ok(new ReturnObject("400", "请确认VIN码正确！", null));
            //    }

            //    if (string.IsNullOrEmpty(model.IdentityNumber))
            //    {
            //        model.IdentityNumber = customer.IdentityNumber;
            //    }
            //}
            var result = true;
            var userStore = new FrontUserStore<FrontIdentityUser>();
            var user = userStore.FindByIdAsync(model.Id).Result;

            //验证用户名的唯一性
            var userForNickName = new FrontUserStore<FrontIdentityUser>().FindByNickNameAsync(model.NickName);
            if (userForNickName != null && userForNickName.Result != null)
            {
                if (model.Id != userForNickName.Result.Id)
                {
                    return Ok(new ReturnObject("401", "用户名已存在,请更换", null));
                }
            }

            if (!string.IsNullOrEmpty(model.IdentityNumber))
            {
                if (user.IdentityNumber != model.IdentityNumber)
                {
                    user.IdentityNumber = model.IdentityNumber;
                }
            }
            user.NickName = model.NickName;
            user.FaceImage = model.FaceImage;
            user.Gender = model.GenderName == "女" ? "2" : "1";
            user.Birthday = model.Birthday;
            user.Address = model.Address;
            user.UpdateTime = DateTime.Now.ToString();
            user.Interest = model.Interest != null && model.Interest.Count() > 0 ? string.Join(",", model.Interest) : "";
            //userStore.UpdateAsync(user);

            //2015.12.25 wubo Add  -----------------------------start
            //解决车主等级修改
            // 1)更新车主级别 
            if (!string.IsNullOrEmpty(user.IdentityNumber))
            {
                var carList = _AppContext.CarServiceUserApp.SelectCarListByIdentity(user.IdentityNumber);
                // user.MLevel = GetUserLevel(user.IdentityNumber);
                if (carList.Count() > 0)
                {
                    user.SystemMType = (int)MembershipType.WhitCar;
                }

                //// 2)校验索九车主/全新途胜车主
                //var _isSonata = _AppContext.SonataServiceApp.IsSonataUser(user.IdentityNumber);
                //if (_isSonata)
                //{
                //    user.SystemMType = (int)MembershipType.Sonata9;
                //}
            }
            userStore.UpdateAsync(user);
            //2015.12.25 wubo Add  -----------------------------end

            //try
            //{
            //    int outValue;
            //    _AppContext.BreadApp.BlueBeanBread(
            //        EBRuleType.完善个人信息,
            //        model.Id,
            //        (MemshipLevel)user.MLevel,
            //        out outValue);
            //    _AppContext.BreadApp.EmpiricBread(EEmpiricRule.完善个人信息, model.Id, out outValue);
            //}
            //catch (Exception ex)
            //{
            //    LogService.Instance.Error("增加蓝豆失败: " + model.Id);
            //    LogService.Instance.Error(ex.Message, ex);
            //}
            return Ok(new ReturnObject(200));
        }

        /// <summary>
        /// 根据用户id获取用户车辆信息
        /// </summary>
        /// <param name="id">用户id</param>
        [HttpGet]
        // [IOVAuthorize]
        [Route("CarInfo")]
        [ResponseType(typeof(List<Car>))]
        public IHttpActionResult CarInfo(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest("用户id不能为空");
            var carList = _AppContext.CarServiceUserApp.CarsByUserId(id).ToList<Car>();
            if (carList == null)
            {
                return this.Ok(new ReturnObject("604", "对不起，没有此用户的车辆信息", null));
            }
            return Ok(new ReturnObject(carList));
        }

        

        /// <summary>
        /// 获取用户使用积分明细（新增，消费，总，剩余）
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="starTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        [HttpPost]
        [IOVAuthorize]
        [Route("UserIntegralDetailByUserID")]
        [ResponseType(typeof(List<UserIntegralRecordDetail>))]
        //public IHttpActionResult UserIntegralDetailByUserID(string UserID,string starTime,string endTime)
        public IHttpActionResult UserIntegralDetailByUserID(string UserID)
        {
            if (string.IsNullOrEmpty(UserID))
                return BadRequest("用户id不能为空");
            var UserIntegralList = _AppContext.UserIntegralApp.UserIntegralDetailByUserID(UserID);
            return Ok(UserIntegralList);
        }


        /// <summary>
        /// 获取用户有效积分列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [IOVAuthorize]
        [Route("UserIntegral")]
        [ResponseType(typeof(List<UserIntegral>))]
        public IHttpActionResult UserIntegral(string id)
        {
            //旧表
            if (string.IsNullOrEmpty(id))
                return BadRequest("用户id不能为空");
            var UserIntegralList = _AppContext.UserIntegralApp.GetList(id).ToList<UserIntegral>();
            return Ok(UserIntegralList);
        }

        // POST api/Account/Register
        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="model">注册数据</param>
        /// <returns>车主类型(系统匹配)。1:非车主 2:车主 3:索9会员</returns>
        [AllowAnonymous]
        [Route("Register")]
        [ResponseType(typeof(int))]
        public IHttpActionResult Register(MembershipRegistModel model)
        {
            if (!ModelState.IsValid)
            {
                var msg = string.Empty;
                ModelState.Values.Where(c => c.Errors.Any()).Select(r => r.Errors).ForEach((e) =>
                {
                    if (e.FirstOrDefault() != null)
                        msg += e.FirstOrDefault().ErrorMessage + "\n";
                });
                return BadRequest(msg);
            }
            if (model.MType != MembershipType.WhitoutCar && string.IsNullOrEmpty(model.IdentityNumber)) return this.Ok("user-514");// BadRequest("身份证号不能为空");
            //手机验证码校验
            var valideCodeResult =
                _AppContext.UserSecurityApp.ValidateMobileVerifyCode(model.PhoneNumber, model.ValideCode);
            if (!valideCodeResult.IsSuccess) return this.Ok(new ReturnObject("user-515"));//return BadRequest("填写的验证码不正确，请重新填写.");
            var store = new FrontUserStore<FrontIdentityUser>();
            var membershipManager = new UserManager<FrontIdentityUser>(store);
            if (store.IsIdentityNumberRepeate(model.IdentityNumber)) return this.Ok(new ReturnObject("user-516"));//return BadRequest("系统中已存在此身份证号");
            if (store.CheckNickNameIsExist(model.NickName)) return this.Ok(new ReturnObject("user-517"));//return BadRequest("系统中已存在昵称");


            //获取请求渠道
            IEnumerable<string> appkeys = null;
            string appkey = string.Empty;

            if (this.Request.Headers.TryGetValues("appkey", out appkeys))
            {
                appkey = appkeys.First();
            }


            var membershipIdentity = new FrontIdentityUser
            {
                UserName = model.PhoneNumber,
                NickName = model.NickName,
                PhoneNumber = model.PhoneNumber,
                Email = model.Email,
                IdentityNumber = model.IdentityNumber,
                Password = model.PassWord,
                Status = (int)MembershipStatus.Nomal,
                CreateTime = DateTime.Now.ToLongTimeString(),
                CreatedPerson = appkey == string.Empty ? this.User.Identity.Name : appkey,
                ActiveWay = (int)MembershipActiveWay.ClientWeb,
                MType = (int)model.MType,
                IsPay = 0,
                MLevel = (int)MemshipLevel.OneStar,
                Mid = model.IdentityNumber
            };
            var result = membershipManager.Create(membershipIdentity, membershipIdentity.Password);
            string errors = string.Empty;
            if (!result.Succeeded) errors = result.Errors.Aggregate(errors, (current, item) => current + (item + " "));

            var account = new FrontUserStore<FrontIdentityUser>().FindByNameAsync(model.PhoneNumber).Result;

            if (result.Succeeded)
            {
                //int value;
                //_AppContext.BreadApp.BlueBeanBread(EBRuleType.注册, account.Id, (MemshipLevel)account.MLevel, out value);
                //_AppContext.BreadApp.EmpiricBread(EEmpiricRule.注册, account.Id, out value);
                return this.Ok(new ReturnObject("200", account == null ? -1 : account.SystemMType));
            }
            return Ok(new ReturnObject("201", errors));
        }

        // POST api/Account/Register
        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="model">注册数据</param>
        /// <returns>车主类型(系统匹配)。1:非车主 2:车主 3:索9会员</returns>
        [AllowAnonymous]
        [Route("DoRegister")]
        [ResponseType(typeof(int))]
        public IHttpActionResult DoRegister(RegisterViewModel model)
        {
            try
            {
                //1、验证请求
                if (!ModelState.IsValid)
                {
                    var msg = string.Empty;
                    ModelState.Values.Where(c => c.Errors.Any()).Select(r => r.Errors).ForEach((e) =>
                    {
                        if (e.FirstOrDefault() != null)
                            msg += e.FirstOrDefault().ErrorMessage + "\n";
                    });
                    return BadRequest(msg);
                }

                //2、验证手机验证码
                //ReturnResult _captchaResult = _AppContext.UserSecurityApp.ValidateMobileVerifyCode(model.Mobile, model.Captcha);
                //if (!_captchaResult.IsSuccess)
                //{
                //    return Ok(new ReturnObject("user-515"));
                //}

                // if (model.MType != MembershipType.WhitoutCar && string.IsNullOrEmpty(model.IdentityNumber)) return this.Ok(new ReturnObject("user-514"));// BadRequest("身份证号不能为空");

                var store = new FrontUserStore<FrontIdentityUser>();
                var membershipManager = new UserManager<FrontIdentityUser>(store);

                if (store.CheckUserNameIsExist(model.Mobile)) return this.Ok(new ReturnObject("user-206"));
                //if (store.IsIdentityNumberRepeate(model.IdentityNumber)) return this.Ok(new ReturnObject("user-516"));//return BadRequest("系统中已存在此身份证号");
                if (store.CheckNickNameIsExist(model.NickName)) return this.Ok(new ReturnObject("user-517"));//return BadRequest("系统中已存在昵称");

                //获取请求渠道
                IEnumerable<string> appkeys = null;
                string appkey = string.Empty;

                if (this.Request.Headers.TryGetValues("appkey", out appkeys))
                {
                    appkey = appkeys.First();
                }

                //3、获取注册信息
                var membershipIdentity = new FrontIdentityUser
                {
                    PhoneNumber = model.Mobile,
                    UserName = model.Mobile,
                    NickName = model.NickName,
                    Email = model.Email,
                    IdentityNumber = model.IdentityNumber,
                    CreatedPerson = appkey == string.Empty ? this.User.Identity.Name : appkey,
                    MLevel = (int)MemshipLevel.OneStar,
                    MType = (int)model.MType,
                    SystemMType = (int)MembershipType.WhitoutCar,
                    ActiveWay = (int)MembershipActiveWay.ClientWeb,
                    Mid = model.IdentityNumber
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
                            return Ok(new ReturnObject("500", "输入的VIN码未匹配到车辆信息", null));
                        }
                    }
                    else
                    {
                        return Ok(new ReturnObject("500", "请输入VIN码", null));
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
                        return Ok(new ReturnObject("500", "输入的VIN码未匹配到车辆信息", null));
                    }
                }

                //5、检验身份证号和VIN码是否匹配
                Task<FrontIdentityUser> cusObj = null;
                if (!string.IsNullOrEmpty(model.IdentityNumber))
                {
                    cusObj = new FrontUserStore<FrontIdentityUser>().FindByIdentityNumber(model.IdentityNumber);
                    if (cusObj != null && cusObj.Result != null && string.IsNullOrEmpty(model.VIN))
                    {
                        return Ok(new ReturnObject("500", "证件号已注册，请输入VIN码", null));
                    }
                }

                //6、注册用户,如果用户存在则更新该用户信息
                IdentityResult result = null;
                //if (string.IsNullOrEmpty(model.VIN))
                if (!(cusObj != null && cusObj.Result != null))
                {
                    try
                    {
                        result = membershipManager.Create(membershipIdentity, model.Password);
                    }
                    catch (Exception ex)
                    {
                        result = null;
                        return Ok(new ReturnObject("201", ex.Message, null));
                    }
                }
                else
                {
                    //校验VIN码和身份证号是否匹配
                    var cus = _AppContext.CarServiceUserApp.SelectCarListByIdentity(model.IdentityNumber);
                    if (cus.Where(s => s.VIN == model.VIN).Count() <= 0)
                    {
                        return Json(new {Code = "500", Message = "VIN码和证件号不匹配"});
                    }

                    if (cusObj != null && cusObj.Result != null)
                    {
                        //user.Id = cusObj.Result.Id;
                        membershipIdentity = membershipManager.FindById(cusObj.Result.Id);
                        membershipIdentity.PhoneNumber = model.Mobile;
                        membershipIdentity.UserName = model.Mobile;
                        membershipIdentity.UpdateTime = DateTime.Now.ToString();
                    }

                    result = membershipManager.Update(membershipIdentity);
                }

                //7、注册完成跳转
                if (result != null && result.Succeeded)
                {
                    // 1)更新车主级别 
                    var gradeUser = membershipManager.FindByName(model.Mobile);
                    var carList = _AppContext.CarServiceUserApp.SelectCarListByIdentity(model.IdentityNumber);
                    gradeUser.MLevel = GetUserLevel(model.IdentityNumber);
                    if (carList.Count() > 0)
                    {
                        gradeUser.SystemMType = (int)MembershipType.WhitCar;
                    }

                    // 2)校验索九车主
                    var _isSonata = _AppContext.SonataServiceApp.IsSonataUser(gradeUser.IdentityNumber);
                    if (_isSonata)
                    {
                        gradeUser.SystemMType = (int)MembershipType.Sonata9;
                    }

                    membershipManager.Update(gradeUser);

                    // 3)索九车主
                    if ((int)MembershipType.Sonata9 == (int)model.MType)
                    {
                        gradeUser.ApprovalStatus = (int)MembershipApprovalStatus.Activing;
                        if (model.ActivityType == "1" && string.IsNullOrEmpty(model.PayNumber))
                        {
                            return Ok(new ReturnObject("500", "选择天猫支付必须输入支付码", null));
                        }
                        if (!string.IsNullOrEmpty(model.PayNumber))
                            gradeUser.PayNumber = model.PayNumber;
                        gradeUser.IsPay = 2;

                        // a)更新用户状态
                        membershipManager.Update(gradeUser);

                        // b)向后台申请激活成为会员
                        var userStore = new FrontUserStore<FrontIdentityUser>();
                        userStore.AddMembershipDealerRecord(gradeUser.Id, _activedealerId);
                        userStore.CreateMembershipRequest(gradeUser.Id, model.IdentityNumber, _activedealerId, string.Empty, appkey);
                    }

                    try
                    {
                        //模拟登陆
                        //await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    }
                    catch (Exception ex)
                    {
                        Vcyber.BLMS.Common.LogService.Instance.Debug("注册成功模拟登陆异常", ex);
                    }

                    var account = new FrontUserStore<FrontIdentityUser>().FindByNameAsync(model.Mobile).Result;

                    // 4)设置用户的蓝豆值和经验值
                    //int _value = 0;
                    // _AppContext.BreadApp.BlueBeanBread(EBRuleType.注册, gradeUser.Id, (MemshipLevel)gradeUser.MLevel, out _value);
                    //  _AppContext.BreadApp.EmpiricBread(EEmpiricRule.注册, gradeUser.Id, out _value);

                    // 5)发送短信
                    _AppContext.SMSApp.SendSMS(ESmsType.注册成功, model.Mobile, new string[] { "" });

                    return this.Ok(new ReturnObject("200", account == null ? -1 : account.SystemMType));
                }
                AddErrors(result);

                var errorMsg = result.Errors.FirstOrDefault();
                if (errorMsg != null && errorMsg.StartsWith("Passwords")) errorMsg = "密码必须为6位以上数字和字母组合";
                if (errorMsg != null && errorMsg.StartsWith("Name")) errorMsg = "您的手机号已经注册过";

                // 如果我们进行到这一步时某个地方出错，则重新显示表单
                return Ok(new ReturnObject("201", errorMsg.ToString(), null));
            }
            catch (Exception ex)
            {
                log.ErrorFormat("自动注册发生错误：{0}", ex);
                return Ok(new ReturnObject("201", ex.Message, null));
            }
        }




        private int GetUserLevel(string IdentityNumber)
        {
            int _level = (int)MemshipLevel.OneStar;
            string strlevel = _AppContext.DealerMembershipApp.GetFirstRegistMLevelByIdNumber(IdentityNumber);
            int.TryParse(strlevel, out  _level);
            return _level;
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        [IOVAuthorize]
        [HttpPost]
        [Route("UserToMember")]
        [ResponseType(typeof(bool))]
        public IHttpActionResult UserToMember(UserToMemberModel model)
        {
            var store = new FrontUserStore<FrontIdentityUser>();
            var membershipManager = new UserManager<FrontIdentityUser>(store);

            //身份证重复判断
            if (store.IsIdentityNumberRepeate(model.IdentityNumber)) return this.Ok(new ReturnObject("user-516"));//return BadRequest("系统中已存在此身份证号");

            //查找用户
            var user = membershipManager.FindById(model.UserId);

            if (user == null)
            {
                return Ok(new ReturnObject("500", "输入的用户编号错误，请确认", null));
            }

            //4、对于公司客户的特殊处理逻辑
            if (model.CustomerType == 2)//判断是否为集团客户
            {
                //1. 根据vin从IF_CAR中找数据，如果找不到，失败。
                if (!string.IsNullOrEmpty(model.VIN))
                {
                    Car c = _AppContext.CarServiceUserApp.GetCarInfoByVIN(model.VIN);
                    if (c == null)
                    {
                        return Ok(new ReturnObject("500", "输入的VIN码未匹配到车辆信息", null));
                    }
                }
                else
                {
                    return Ok(new ReturnObject("500", "请输入VIN码", null));
                }

                //2. 在IF_customer表中增加一条数据（custid,CustId规则：JT-110222199912013115（集团-身份证号)）
                var parms = new Entity.Generated.IFCustomer();
                parms.CustId = "JT-" + model.IdentityNumber;
                parms.CustName = "--";
                parms.CustMobile = user.UserName;
                parms.IdentityNumber = model.IdentityNumber;
                parms.Email = string.Empty;
                parms.Gender = "男";
                parms.Address = "--";
                parms.City = "--";

                Vcyber.BLMS.Entity.Generated.IFCustomer ifCus = _AppContext.CarServiceUserApp.CreateOrGetIFCustomer(parms);
                //3. 修改IF_car表中车辆信息的custid
                bool b = _AppContext.CarServiceUserApp.UpdateCarInfoByVIN(model.VIN, ifCus.CustId);
                if (!b)
                {
                    return Ok(new ReturnObject("500", "输入的VIN码未匹配到车辆信息", null));
                }
            }

            //5、检验身份证号和VIN码是否匹配
            Task<FrontIdentityUser> cusObj = null;
            if (!string.IsNullOrEmpty(model.IdentityNumber))
            {
                cusObj = new FrontUserStore<FrontIdentityUser>().FindByIdentityNumber(model.IdentityNumber);
                if (cusObj != null && cusObj.Result != null && string.IsNullOrEmpty(model.VIN))
                {
                    return Ok(new ReturnObject("500", "证件号已注册，请输入VIN码", null));
                }
            }

            //6、注册用户,如果用户存在则更新该用户信息
            IdentityResult result = null;

            //校验VIN码和身份证号是否匹配
            //var cus = _AppContext.CarServiceUserApp.SelectCarListByIdentity(model.IdentityNumber);
            //if (cus.Where(s => s.VIN == model.VIN).Count() <= 0)
            //{
            //    return Json(new { Code = "500", Message = "VIN码和证件号不匹配" });
            //}

            user.IdentityNumber = model.IdentityNumber;
            result = membershipManager.Update(user);

            //7、注册完成跳转
            if (result != null && result.Succeeded)
            {
                // 1)更新车主级别 
                var gradeUser = membershipManager.FindByName(user.UserName);
                var carList = _AppContext.CarServiceUserApp.SelectCarListByIdentity(model.IdentityNumber);
                gradeUser.MLevel = GetUserLevel(model.IdentityNumber);
                if (carList.Count() > 0)
                {
                    gradeUser.SystemMType = (int)MembershipType.WhitCar;
                }

                // 2)校验索九车主
                var _isSonata = _AppContext.SonataServiceApp.IsSonataUser(gradeUser.IdentityNumber);
                if (_isSonata)
                {
                    gradeUser.SystemMType = (int)MembershipType.Sonata9;
                }

                membershipManager.Update(gradeUser);

                // 3)索九车主
                if ((int)MembershipType.Sonata9 == gradeUser.SystemMType)
                {
                    gradeUser.ApprovalStatus = (int)MembershipApprovalStatus.Activing;
                    if (model.ActivityType == 2 && string.IsNullOrEmpty(model.PayNumber))
                    {
                        return Ok(new ReturnObject("500", "选择天猫支付必须输入支付码", null));
                    }

                    if (!string.IsNullOrEmpty(model.PayNumber))
                        gradeUser.PayNumber = model.PayNumber;
                    gradeUser.IsPay = 2;

                    // a)更新用户状态
                    membershipManager.Update(gradeUser);

                    // b)向后台申请激活成为会员
                    var userStore = new FrontUserStore<FrontIdentityUser>();
                    userStore.AddMembershipDealerRecord(gradeUser.Id, model.DealerId);
                    userStore.CreateMembershipRequest(gradeUser.Id, model.IdentityNumber, model.DealerId, string.Empty, "blms_web");
                }
            }

            return this.Ok(new ReturnObject("200", null));
        }

        /// <summary>
        /// 使用付款码会费支付
        /// </summary>
        /// <param name="id">用户id</param>
        /// <param name="dealerId">经销商id</param>
        /// <param name="payNumber">支付码(没有填空值即可)</param>
        /// <param name="memo">备注</param>
        /// <returns></returns>
        [IOVAuthorize]
        [HttpPost]
        [Route("PayToDealer")]
        [ResponseType(typeof(bool))]
        public IHttpActionResult PayToDealer(string id, string dealerId, string payNumber, string memo)
        {
            bool resul = false;
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(dealerId))
                return BadRequest("用户Id和经销商Id不能为空");
            var store = new FrontUserStore<FrontIdentityUser>();
            var user = store.FindByIdAsync(id).Result;
            user.IsPay = 2;
            user.PayNumber = payNumber;
            store.UpdateAsync(user);
            store.AddMembershipDealerRecord(user.Id, dealerId);
            resul = store.CreateMembershipRequest(id, user.IdentityNumber, dealerId, memo, this.GetSysCode());
            return Ok(resul);
        }

        /// <summary>
        /// 选择特约店申请
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="dealerId">经销商Id</param>
        /// <returns></returns>
        [IOVAuthorize]
        [HttpPost]
        [Route("UserSelectDealerToPay")]
        [ResponseType(typeof(bool))]
        public IHttpActionResult UserSelectDealerToPay(string userId, string dealerId)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(dealerId))
                return BadRequest("用户Id和经销商Id不能为空");
            IEnumerable<string> appkeys = null;
            string appkey = string.Empty;
            if (this.Request.Headers.TryGetValues("appkey", out appkeys))
            {
                appkey = appkeys.First();
            }
            var store = new FrontUserStore<FrontIdentityUser>();
            var user = store.FindByIdAsync(userId).Result;
            var returnIntegralType = (int)_AppContext.CarServiceUserApp.GetReIntegralTypeByIdentity(user.IdentityNumber);
            user.IsPay = 2;
            if (returnIntegralType > -1)
            {
                user.Amount = returnIntegralType > 1 ? 50 : 100;
            }
            store.UpdateAsync(user);
            store.AddMembershipDealerRecord(userId, dealerId);
            store.CreateMembershipRequest(userId, user.IdentityNumber, dealerId, string.Empty, appkey);
            return Ok(true);
        }


        /// <summary>
        /// 会费支付状态 (0: 未支付 1:支付完成  2:已提交支付,审批中)
        /// </summary>
        /// <param name="id">用户id</param>
        /// <returns></returns>
        [IOVAuthorize]
        [HttpGet]
        [Route("MemberPayStatus")]
        [ResponseType(typeof(int))]
        public IHttpActionResult MemberPayStatus(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest("用户id不能为空");
            var store = new FrontUserStore<FrontIdentityUser>();
            var user = store.FindByIdAsync(id).Result;
            return Ok(user.IsPay);
        }

        /// <summary>
        /// 用户注册根据身份证号返回会员级别
        /// </summary>
        /// <param name="idNumber"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetFirstRegistMLevelByIdNumber")]
        public string GetFirstRegistMLevelByIdNumber(string idNumber)
        {
            return _AppContext.DealerMembershipApp.GetFirstRegistMLevelByIdNumber(idNumber);
        }
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        //  [IOVAuthorize]
        [HttpPost]
        [Route("ChangePassword")]
        public IHttpActionResult ChangePassword(ChangePasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userStore = new FrontUserStore<FrontIdentityUser>();
            var userManager = new UserManager<FrontIdentityUser>(userStore);
            IdentityResult result = userManager.ChangePasswordAsync(model.Id, model.OldPassword,
                model.NewPassword).Result;

            if (result.Succeeded)
            {
                int tempValue;
                _AppContext.BreadApp.EmpiricBread(EEmpiricRule.修改密码, model.Id, out tempValue);
            }

            return Ok(result.Succeeded);
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("ResetPassword")]
        public IHttpActionResult ResetPassword(ResetPasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ReturnResult _captchaResult = _AppContext.UserSecurityApp.ValidateMobileVerifyCode(model.PhoneNumber, model.ValideCode);
            if (!_captchaResult.IsSuccess)
            {
                ReturnResult result1 = new ReturnResult() { IsSuccess = false, Message = "填写的验证码不正确，请重新填写" };
                //return BadRequest("填写的验证码不正确，请重新填写");
                return this.Ok(new ReturnObject(result1));//"402", "填写的验证码不正确，请重新填写"

            }

            FrontUserStore<FrontIdentityUser> store = new FrontUserStore<FrontIdentityUser>();
            UserManager<FrontIdentityUser> userManager = new UserManager<FrontIdentityUser>(store);
            var user = userManager.FindByNameAsync(model.PhoneNumber).Result;
            if (user == null)
            {
                ReturnResult result1 = new ReturnResult() { IsSuccess = false, Message = "没有找到此用户" };
                return this.Ok(new ReturnObject(result1));
                // return BadRequest("没有找到此用户");
            }
            if (model.NewPassword != model.ConfirmPassword)
            {
                return BadRequest("两次输入的密码不一致！");
            }
            //手机验证码校验
            //var valideCodeResult =
            //    _AppContext.UserSecurityApp.ValidateMobileVerifyCode(model.PhoneNumber, model.ValideCode);
            //if (!valideCodeResult.IsSuccess)
            //    return BadRequest("填写的验证码不正确，请重新填写.");

            UserManager<FrontIdentityUser> UserManager = new UserManager<FrontIdentityUser>(store);
            String hashedNewPassword = UserManager.PasswordHasher.HashPassword(model.NewPassword);

            store.SetPasswordHashAsync(user, hashedNewPassword);

            user.IsNeedModifyPw = 0;
            var result = userManager.Update(user);

            ReturnResult result2 = new ReturnResult() { IsSuccess = true, Message = "修改成功" };
            return Ok(new ReturnObject(result2));
        }

        /// <summary>
        /// 检查用户名密码是否存在,返回用户id
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetUserByLogin")]
        public IHttpActionResult GetUserByLogin(string nickName, string Password)
        {
            if (string.IsNullOrEmpty(nickName))
            {
                return BadRequest("会员昵称不能为空。");
            }
            if (string.IsNullOrEmpty(Password))
            {
                return BadRequest("会员密码不能为空。");
            }
            FrontUserStore<FrontIdentityUser> store = new FrontUserStore<FrontIdentityUser>();
            UserManager<FrontIdentityUser> UserManager = new UserManager<FrontIdentityUser>(store);
            String hashedNewPassword = UserManager.PasswordHasher.HashPassword(Password);

            var account = new FrontUserStore<FrontIdentityUser>().GetUserByLogin(nickName, hashedNewPassword);

            if (account == null || account.Result == null)
            {
                return BadRequest("会员昵称或密码不存在");
            }
            var id = account.Result.Id;
            //return Ok(new ReturnObject(account.Result.Id));
            return this.Ok(new ReturnObject(new ReturnResult() { Message = account.Result.Id }));
        }
        ///// <summary>
        ///// 创建论坛用户
        ///// </summary>
        ///// <param name="model"></param>
        //private void CreateForumUser(MembershipRegistModel model)
        //{
        //    var forum = new ForumRegister
        //    {
        //        UserName = model.PhoneNumber,
        //        Password = model.IdentityNumber.Substring(model.IdentityNumber.Length - 6, 6),
        //        PageBoardId = 1,
        //        Email = string.IsNullOrEmpty(model.Email) ? string.Format("{0}@mail.com", model.PhoneNumber) : model.Email,
        //        Question = "default",
        //        Answer = "default"
        //    };
        //    forum.CreateForumUser();
        //}

        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }


        /// <summary>
        /// 获取用户购车时的信息
        /// </summary>
        /// <param name="identityNumber">身份证号</param>
        /// <returns>用户信息</returns>
        [Route("BuyCarInfo")]
        [ResponseType(typeof(IFCustomer))]
        //[IOVAuthorize]
        public IHttpActionResult GetUserInfoWhenBuyCar(string identityNumber)
        {
            return this.Ok(new ReturnObject(_AppContext.UserInfoApp.FindCustome(identityNumber)));
        }



        /// <summary>
        /// 根据身份证号验证车主
        /// </summary>
        /// <param name="identityNumber">身份证号</param>
        /// <returns>用户信息</returns>
        [Route("GetCustomerInfoByIdentityNumber")]
        [ResponseType(typeof(IFCustomer))]
        //[IOVAuthorize]
        public IHttpActionResult GetCustomerInfo(string identityNumber)
        {
            return this.Ok(new ReturnObject(_AppContext.UserInfoApp.GetInfoWhenBuyCar(identityNumber)));

        }

        //登录一键绑定
        public IHttpActionResult BindWXNew([FromBody]WXBindModelV model)
        {
            if (model == null) return this.Ok(new ReturnObject("601", "参数错误", null));

            var _ticket = PublicTools.SignatureBuilder.BuildSignature(
                model.WXAppId,
                ConfigurationManager.AppSettings["wxToken"],
                model.WXTimestamp);
            if (_ticket != model.WXTicket)
            {
                log.Debug(string.Format("appid:{0}, timestamp:{1},ticket:{2}, calculatedTicket: {3}", model.WXAppId, model.WXTimestamp, model.WXTicket, _ticket));
                return this.Unauthorized();
            }

            string openId = model.OpenId;
            string tel = model.Tel;
            string captcha = model.Captcha;
            string vin = model.Vin;
            string name = model.Name;

            log.InfoFormat("一键绑定开始 name={0}, vin={1},idcard", model.Name, model.Vin, model.IdentityNumber);

            //if (string.IsNullOrEmpty(vin) && string.IsNullOrEmpty(name))
            //{
            //    log.InfoFormat("一键绑定 name={0}, vin={1}", model.Name, model.Vin);
            //    return this.Ok(new ReturnObject("601", "对不起，车主绑定请提供车架号，粉丝绑定请提供姓名", null));
            //}

            //匹配手机号码
            var user1 = _AppContext.UserInfoApp.SelectOneByTel(tel);
            if (user1 == null)
            {
                //匹配不上自动注册
                AutoRegisterViewModel automModel1 = new AutoRegisterViewModel()
                {
                    Captcha = captcha,
                    Mobile = tel,
                    MType = MembershipType.WhitoutCar,
                    NickName = _AppContext.UserWxBindApp.GenerateNickname(model.WXNickname)
                };
                var bindresult = this.InternalBindWX(automModel1, openId);
                ReturnObject obj = ((System.Web.Http.Results.OkNegotiatedContentResult<ReturnObject>)bindresult).Content;
                if (obj.Code != "200") return bindresult;

                user1 = _AppContext.UserInfoApp.SelectOneByTel(tel);
            }
            else if (!string.IsNullOrEmpty(_AppContext.UserWxBindApp.GetOpenIdByMobile(tel)))
            {
                return this.Ok(new ReturnObject("604", "对不起，您的手机号已被其他微信账号绑定", new WXUserV() { Id = user1.Id, Name = user1.UserName }));
            }
            else
            {
                _AppContext.UserWxBindApp.InsertBindData(new WXBind() { OpenId = openId, UserId = user1.Id, Tel = tel, Vin = vin, Status = 0, UserName = user1.UserName });
            }

            return this.Ok(new ReturnObject(new WXUserV() { Id = user1.Id, Name = user1.UserName }));

        }


        /// <summary>
        /// 一键绑定,以有无Vin区分是车主还是粉丝，三个url参数用作安全验证
        /// </summary>
        /// <param name="model"></param>
        /// <returns>绑定成功返回绑定的userid和username, 授权失败将返回401</returns>
        // <param name="wxAppId">固定值：wxbm</param>
        // <param name="wxTimestamp">时间戳：yyyyMMddhhmmssfff</param>
        // <param name="wxTicket">hash值</param>
        [Route("BindWX")]
        [ResponseType(typeof(WXUserV))]
        [AllowAnonymous]
        [HttpPost]
        public IHttpActionResult BindWX([FromBody]WXBindModelV model)
        {
            if (model == null) return this.Ok(new ReturnObject("601", "参数错误", null));

            var _ticket = PublicTools.SignatureBuilder.BuildSignature(
                model.WXAppId,
                ConfigurationManager.AppSettings["wxToken"],
                model.WXTimestamp);
            if (_ticket != model.WXTicket)
            {
                log.Debug(string.Format("appid:{0}, timestamp:{1},ticket:{2}, calculatedTicket: {3}", model.WXAppId, model.WXTimestamp, model.WXTicket, _ticket));
                return this.Unauthorized();
            }

            /* 领动临时添加代码，应该使用旧有逻辑，所以注释掉。
            if (string.IsNullOrEmpty(model.Vin) && !string.IsNullOrEmpty(model.IdentityNumber))
            {
                var carlist = _AppContext.CarServiceUserApp.SelectCarListByIdentity(model.IdentityNumber);
                if (carlist !=null && carlist.Count()>0)
                {
                    foreach (var car in carlist)
                    {
                        model.Vin = car.VIN;
                        break;
                    }
                }

                log.InfoFormat("一键绑定，没有提供VIN， 根据身份证找vin。查找结果： 身份证={0}, vin={1}", model.IdentityNumber, model.Vin);
            }
            */

            string openId = model.OpenId;
            string tel = model.Tel;
            string captcha = model.Captcha;
            string vin = model.Vin;
            string name = model.Name;

            log.InfoFormat("一键绑定开始 name={0}, vin={1},idcard", model.Name, model.Vin, model.IdentityNumber);

            if (string.IsNullOrEmpty(vin) && string.IsNullOrEmpty(name))
            {
                log.InfoFormat("一键绑定 name={0}, vin={1}", model.Name, model.Vin);
                return this.Ok(new ReturnObject("601", "对不起，车主绑定请提供车架号，粉丝绑定请提供姓名", null));
            }


            if (!string.IsNullOrEmpty(vin))
            {
                if (vin.Length != 17) return this.Ok(new ReturnObject("602", "对不起，车架号输入错误，请重新输入", null));

                var user = _AppContext.UserInfoApp.SelectOneByVin(vin);
                if (user != null)
                {
                    var bindUser = _AppContext.UserWxBindApp.GetOpenIdByMobile(user.UserName);
                    if (!string.IsNullOrEmpty(bindUser) && bindUser != openId)
                    {
                        return this.Ok(new ReturnObject("400", "对不起，车架号重复绑定，可进行申诉或重新输入。", null));
                    }
                    _AppContext.UserWxBindApp.InsertBindData(new WXBind() { OpenId = openId, UserId = user.Id, Tel = tel, Vin = vin, UserName = user.UserName });
                    return this.Ok(new ReturnObject(new WXUserV() { Id = user.Id, Name = user.UserName }));
                }

                user = _AppContext.UserInfoApp.SelectOneByTel(model.Tel);
                IFCustomer customer;
                _AppContext.CarServiceUserApp.GetCarInfoByVIN(vin, out customer);
                if (customer != null && customer.CustId != null)
                {
                    if (user != null)
                    {
                        if (_AppContext.UserWxBindApp.UpdateIdentityNumber(user.Id, customer.IdentityNumber) == 1)
                        {
                            _AppContext.UserWxBindApp.InsertBindData(new WXBind() { OpenId = openId, UserId = user.Id, Tel = tel, Vin = vin, UserName = user.UserName });

                            return this.Ok(new ReturnObject(new WXUserV() { Id = user.Id, Name = user.UserName }));
                        }
                    }
                    else
                    {
                        AutoRegisterViewModel autoModel = new AutoRegisterViewModel()
                        {
                            Captcha = captcha,
                            Mobile = tel,
                            MType = MembershipType.WhitCar,
                            NickName = _AppContext.UserWxBindApp.GenerateNickname(model.WXNickname),
                            Identity =
                                customer.IdentityNumber
                        };
                        return this.InternalBindWX(autoModel, openId);
                    }
                }
                return this.Ok(new ReturnObject("401", "对不起，车架号输入错误，请重新输入。如确认此车架号，请进行申诉。", null));
            }
            //匹配手机号码
            var user1 = _AppContext.UserInfoApp.SelectOneByTel(tel);
            if (user1 == null)
            {
                //匹配不上自动注册
                AutoRegisterViewModel automModel1 = new AutoRegisterViewModel()
                {
                    Captcha = captcha,
                    Mobile = tel,
                    MType = MembershipType.WhitoutCar,
                    NickName = _AppContext.UserWxBindApp.GenerateNickname(model.WXNickname)
                };
                var bindresult = this.InternalBindWX(automModel1, openId);
                ReturnObject obj = ((System.Web.Http.Results.OkNegotiatedContentResult<ReturnObject>)bindresult).Content;
                if (obj.Code != "200") return bindresult;

                user1 = _AppContext.UserInfoApp.SelectOneByTel(tel);
            }
            else if (!string.IsNullOrEmpty(_AppContext.UserWxBindApp.GetOpenIdByMobile(tel)))
            {
                return this.Ok(new ReturnObject("604", "对不起，您的手机号已被其他微信账号绑定", new WXUserV() { Id = user1.Id, Name = user1.UserName }));
            }
            else
            {
                _AppContext.UserWxBindApp.InsertBindData(new WXBind() { OpenId = openId, UserId = user1.Id, Tel = tel, Vin = vin, Status = 0, UserName = user1.UserName });
            }

            return this.Ok(new ReturnObject(new WXUserV() { Id = user1.Id, Name = user1.UserName }));

        }

        /// <summary>
        /// 根据绑定的Vin获取用户等级
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>使用返回的MLevel</returns>
        [Route("UserLevel/Vin")]
        [ResponseType(typeof(EMemshipLevelWX))]
        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult GetLevelByVin(string userId)
        {
            //todo:名字应该是get level by userid

            return this.Ok(new ReturnObject("200", "", _AppContext.CarServiceUserApp.GetMemshipLevelWX(userId)));
        }

        /// <summary>
        /// 解除微信绑定
        /// </summary>
        /// <param name="model"></param>
        /// <returns>返回200解绑成功，否则失败</returns>
        [Route("UnbindWX")]
        [ResponseType(typeof(string))]
        [HttpPost]
        [AllowAnonymous]
        public IHttpActionResult UnbindWX(WXUnbindModelV model)
        {
            var _ticket = PublicTools.SignatureBuilder.BuildSignature(
               model.WXAppId,
               ConfigurationManager.AppSettings["wxToken"],
               model.WXTimestamp);
            if (_ticket != model.WXTicket)
            {
                log.Debug(string.Format("appid:{0}, timestamp:{1},ticket:{2}, calculatedTicket: {3}", model.WXAppId, model.WXTimestamp, model.WXTicket, _ticket));
                return this.Unauthorized();
            }

            var result = _AppContext.UserWxBindApp.UnbindWX(model.OpenId, model.UserId);
            if (result > 0) return this.Ok(new ReturnObject("200"));
            return this.Ok(new ReturnObject("601", "解除绑定失败或绑定不存在", null));
        }

        private IHttpActionResult InternalBindWX(AutoRegisterViewModel autoModel, string openId)
        {
            var x = RegisterAuto(autoModel);
            ReturnObject obj = ((System.Web.Http.Results.OkNegotiatedContentResult<ReturnObject>)x).Content;
            if (obj.Code == "200")
            {
                var user = _AppContext.UserInfoApp.SelectOneByTel(autoModel.Mobile);
                string userId = user.Id;

                //跳过首次登陆需要修改密码的逻辑
                ChangePasswordBindingModel model = new ChangePasswordBindingModel() { ConfirmPassword = "rEgisterbYwX-_-~", NewPassword = "rEgisterbYwX-_-~", OldPassword = "rEgisterbYwX-_-", PhoneNumber = autoModel.Mobile, Id = userId };
                var userStore = new FrontUserStore<FrontIdentityUser>();
                var userManager = new UserManager<FrontIdentityUser>(userStore);
                IdentityResult result = userManager.ChangePasswordAsync(model.Id, model.OldPassword,
                    model.NewPassword).Result;

                if (result.Succeeded)
                {
                    int tempValue;
                    _AppContext.BreadApp.EmpiricBread(EEmpiricRule.修改密码, model.Id, out tempValue);
                }
                else log.Error("自动修改用户密码失败，手机号：" + autoModel.Mobile);

                //绑定数据
                _AppContext.UserWxBindApp.InsertBindData(
                    new WXBind()
                    {
                        OpenId = openId,
                        UserId = userId,
                        Tel = autoModel.Mobile,
                        Vin = autoModel.VIN,
                        UserName = user.UserName
                    });
                return this.Ok(new ReturnObject("200", "", new WXUserV() { Id = user.Id, Name = user.UserName }));
            }
            log.Error(string.Format("注册失败，返回值：{0},错误信息：{1}, vin:{2}, tel:{3}, nickname:{4}", obj.Code, obj.Message, autoModel.VIN ?? "", autoModel.Mobile, autoModel.NickName));
            return this.Ok(new ReturnObject("604", "自动注册失败", null));
        }


        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="model">注册数据</param>
        /// <returns>车主类型(系统匹配)。1:非车主 2:车主 3:索9会员</returns>
        private IHttpActionResult RegisterAuto(AutoRegisterViewModel autoModel)
        {
            RegisterViewModel model = new RegisterViewModel();
            model.Password = "rEgisterbYwX-_-";
            model.Mobile = autoModel.Mobile;
            // model.Captcha = autoModel.Captcha;
            model.NickName = autoModel.NickName;
            model.MType = autoModel.MType;
            model.IdentityNumber = autoModel.Identity;
            model.customerType = "1";//个人

            return this.DoRegister(model);
        }

        //todo:该接口应该增加安全验证
        /// <summary>
        /// 获取用户绑定数据
        /// </summary>
        /// <param name="model">注册数据</param>
        /// <param name="status"></param>
        /// <returns>车主类型(系统匹配)。1:非车主 2:车主 3:索9会员</returns>
        [Route("WXBindData")]
        [ResponseType(typeof(Page<WXBindData>))]
        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult WXBindData(DateTime dateFrom, DateTime dateTo, string vin, string userName, string mobile, EWXBindStatus status, int page, int itemsPerPage)
        {
            var result = _AppContext.UserWxBindApp.GetWXBindData(
                dateFrom,
                dateTo,
                vin,
                userName,
                mobile,
                status,
                page,
                itemsPerPage);
            return this.Ok(new ReturnObject("200", "", result));
        }

        //todo:该接口应该增加安全验证
        /// <summary>
        /// 根据条件查询绑定状态
        /// </summary>

        /// <returns>绑定数据</returns>
        [Route("WXBindStatus")]
        [ResponseType(typeof(List<WXBind>))]
        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult GetBindStatus(string openId, string vin, string mobile)
        {
            var result = _AppContext.UserWxBindApp.GetBindStatus(openId, vin, mobile);
            return this.Ok(new ReturnObject("200", "", result));
        }




        //三八
        [CrossSite]
        [HttpGet]
        [Route("GetBindStateByOpenId")]
        public IHttpActionResult GetBindStateByOpenId(string openId)
        {
            try
            {
                //参数
                string key = "openId=" + openId;

                //地址
                string url = "https://www.bluemembers.com.cn/weixin/WxUserBmUser/GetUserInfoByOpenId?" + key;

                //调用服务
                var data = Vcyber.BLMS.Common.Web.WebUtils.JsonToObj<AfterSaleServiceWXModel>(Vcyber.BLMS.Common.Web.WebUtils.GET_WebRequestHTML("utf-8", url, null), null);


                if (data.ret == 0)
                {
                    //未绑定
                    return Json(new { code = 200, isbind = false });
                }
                else
                {
                    //获取其它信息
                    string userId = _AppContext.UserWxBindApp.GetUserIdByOpenId(openId);
                    var account = new FrontUserStore<FrontIdentityUser>().FindByIdAsync(userId);

                    //已绑定
                    return Json(new { code = 200, isbind = true, data = account.Result });
                }


            }
            catch (Exception ex)
            {
                return Json(new { code = 400, msg = ex.Message });
            }
        }

        //[CrossSite]
        //[HttpGet]
        //[Route("GetBindStateByOpenId")]
        //public IHttpActionResult GetBindStateByOpenId(string openId)
        //{
        //    try
        //    {
        //        //参数
        //        string key = "openId=" + openId;

        //        //地址
        //        string url = "https://www.bluemembers.com.cn/weixin/WxUserBmUser/GetUserInfoByOpenId?" + key;

        //        //调用服务
        //        var data = Vcyber.BLMS.Common.Web.WebUtils.JsonToObj<AfterSaleServiceWXModel>(Vcyber.BLMS.Common.Web.WebUtils.GET_WebRequestHTML("utf-8", url, null), null);


        //        if (data.ret == 0)
        //        {
        //            //未绑定
        //            return Json(new { code = 200, isbind = false });
        //        }
        //        else
        //        {
        //            //获取其它信息
        //            string userId = _AppContext.UserWxBindApp.GetUserIdByOpenId(openId);
        //            var account = new FrontUserStore<FrontIdentityUser>().FindByIdAsync(userId);

        //            //已绑定
        //            return Json(new { code = 200, isbind = true, data = account.Result });
        //        }


        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { code = 400, msg = ex.Message });
        //    }
        //}

        [CrossSite]
        [HttpGet]
        [Route("GetAndSendCard")]
        public IHttpActionResult GetAndSendCard(string openid, string tel)
        {
            try
            {
                //参数
                string key = "openid=" + openid + "&tel=" + tel;

                //地址
                string url = "https://www.bluemembers.com.cn/weixin/WXCard/GetAndSendCard?" + key;

                //调用服务
                var data = Vcyber.BLMS.Common.Web.WebUtils.JsonToObj<AfterSaleServiceWXModel>(Vcyber.BLMS.Common.Web.WebUtils.GET_WebRequestHTML("utf-8", url, null), null);


                if (data.ret == 1)
                {
                    //成功
                    return Json(new { code = 200, data = data.data });
                }
                else
                {
                    //失败
                    return Json(new { code = 400, msg = data.msg });
                }
            }
            catch (Exception ex)
            {
                return Json(new { code = 400, msg = ex.Message });
            }
        }


        //三八3.1[卡卷]
        [CrossSite]
        [HttpGet]
        [Route("GetWXCardNew")]
        public IHttpActionResult GetWXCardNew(string openid, string tel)
        {
            try
            {
                //参数
                string key = "openid=" + openid + "&tel=" + tel;

                //地址
                string url = "https://www.bluemembers.com.cn/weixin/WXCard/GetCardNew?" + key;

                //调用服务
                var data = Vcyber.BLMS.Common.Web.WebUtils.JsonToObj<AfterSaleServiceWXModel>(Vcyber.BLMS.Common.Web.WebUtils.GET_WebRequestHTML("utf-8", url, null), null);


                if (data.ret == 1)
                {
                    //成功
                    return Json(new { code = 200, data = data.data });
                }
                else
                {
                    //失败
                    return Json(new { code = 400, msg = data.msg });
                }
            }
            catch (Exception ex)
            {
                return Json(new { code = 400, msg = ex.Message });
            }
        }



        [CrossSite]
        [HttpGet]
        [Route("SendWXCardNew")]
        public IHttpActionResult SendWXCardNew(string openid, string tel)
        {
            try
            {
                //参数
                string key = "openid=" + openid + "&tel=" + tel;

                //地址
                string url = "https://www.bluemembers.com.cn/weixin/WXCard/SendCardNew?" + key;

                //调用服务
                var data = Vcyber.BLMS.Common.Web.WebUtils.JsonToObj<AfterSaleServiceWXModel>(Vcyber.BLMS.Common.Web.WebUtils.GET_WebRequestHTML("utf-8", url, null), null);


                if (data.ret == 1)
                {
                    //成功
                    return Json(new { code = 200, data = data.data });
                }
                else
                {
                    //失败
                    return Json(new { code = 400, msg = data.msg });
                }
            }
            catch (Exception ex)
            {
                return Json(new { code = 400, msg = ex.Message });
            }
        }


        [CrossSite]
        [HttpGet]
        [Route("GetWXCardNewByType")]
        public IHttpActionResult GetWXCardNewByType(string openid, string tel, string cardid)
        {
            try
            {
                //参数
                string key = "openid=" + openid + "&tel=" + tel + "&cardid=" + cardid;

                //地址
                string url = "https://www.bluemembers.com.cn/weixin/WXCard/GetCardNewByType?" + key;

                //调用服务
                var data = Vcyber.BLMS.Common.Web.WebUtils.JsonToObj<AfterSaleServiceWXModel>(Vcyber.BLMS.Common.Web.WebUtils.GET_WebRequestHTML("utf-8", url, null), null);


                if (data.ret == 1)
                {
                    //成功
                    return Json(new { code = 200 });
                }
                else
                {
                    //失败
                    return Json(new { code = 400, msg = data.msg });
                }
            }
            catch (Exception ex)
            {
                return Json(new { code = 400, msg = ex.Message });
            }
        }


        ///// <summary>
        ///// 用户申请成为会员
        ///// </summary>
        ///// <param name="userId">用户id</param>
        ///// <param name="identityNumber">身份证号</param>
        ///// <param name="dealerId">经销商id</param>
        ///// <param name="payNumber">付款码(如没有付款码,可以为空串)</param>
        ///// <param name="memo"></param>
        ///// <returns></returns>
        //[Route("SubmitMembershipRequest")]
        //[IOVAuthorize]
        //public IHttpActionResult SubmitMembershipRequest(string userId, string identityNumber, string dealerId, string payNumber, string memo)
        //{
        //    if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(identityNumber) || string.IsNullOrEmpty(dealerId))
        //        return BadRequest("请求的参数有误");
        //    FrontUserStore<FrontIdentityUser> userStore = new FrontUserStore<FrontIdentityUser>();
        //    var user = userStore.FindByIdAsync(userId).Result;
        //    user.IsPay = 2;//已提交支付申请
        //    user.PayNumber = payNumber;//付款码
        //    userStore.UpdateAsync(user);

        //    bool result = userStore.CreateMembershipRequest(userId, identityNumber, dealerId, memo);
        //    return this.Ok(new ReturnObject(result));
        //}

        /*
        // GET api/Account/UserInfo
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("UserInfo")]
        public UserInfoViewModel GetUserInfo()
        {
            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            return new UserInfoViewModel
            {
                Email = User.Identity.GetUserName(),
                HasRegistered = externalLogin == null,
                LoginProvider = externalLogin != null ? externalLogin.LoginProvider : null
            };
        }

        // POST api/Account/Logout
        [Route("Logout")]
        public IHttpActionResult Logout()
        {
            Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            return Ok();
        }

        // GET api/Account/ManageInfo?returnUrl=%2F&generateState=true
        [Route("ManageInfo")]
        public async Task<ManageInfoViewModel> GetManageInfo(string returnUrl, bool generateState = false)
        {
            IdentityUser user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

            if (user == null)
            {
                return null;
            }

            List<UserLoginInfoViewModel> logins = new List<UserLoginInfoViewModel>();

            foreach (IdentityUserLogin linkedAccount in user.Logins)
            {
                logins.Add(new UserLoginInfoViewModel
                {
                    LoginProvider = linkedAccount.LoginProvider,
                    ProviderKey = linkedAccount.ProviderKey
                });
            }

            if (user.PasswordHash != null)
            {
                logins.Add(new UserLoginInfoViewModel
                {
                    LoginProvider = LocalLoginProvider,
                    ProviderKey = user.UserName,
                });
            }

            return new ManageInfoViewModel
            {
                LocalLoginProvider = LocalLoginProvider,
                Email = user.UserName,
                Logins = logins,
                ExternalLoginProviders = GetExternalLogins(returnUrl, generateState)
            };
        }


        // POST api/Account/SetPassword
        [Route("SetPassword")]
        public async Task<IHttpActionResult> SetPassword(SetPasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // POST api/Account/AddExternalLogin
        [Route("AddExternalLogin")]
        public async Task<IHttpActionResult> AddExternalLogin(AddExternalLoginBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);

            AuthenticationTicket ticket = AccessTokenFormat.Unprotect(model.ExternalAccessToken);

            if (ticket == null || ticket.Identity == null || (ticket.Properties != null
                && ticket.Properties.ExpiresUtc.HasValue
                && ticket.Properties.ExpiresUtc.Value < DateTimeOffset.UtcNow))
            {
                return BadRequest("External login failure.");
            }

            ExternalLoginData externalData = ExternalLoginData.FromIdentity(ticket.Identity);

            if (externalData == null)
            {
                return BadRequest("The external login is already associated with an account.");
            }

            IdentityResult result = await UserManager.AddLoginAsync(User.Identity.GetUserId(),
                new UserLoginInfo(externalData.LoginProvider, externalData.ProviderKey));

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // POST api/Account/RemoveLogin
        [Route("RemoveLogin")]
        public async Task<IHttpActionResult> RemoveLogin(RemoveLoginBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result;

            if (model.LoginProvider == LocalLoginProvider)
            {
                result = await UserManager.RemovePasswordAsync(User.Identity.GetUserId());
            }
            else
            {
                result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(),
                    new UserLoginInfo(model.LoginProvider, model.ProviderKey));
            }

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // GET api/Account/ExternalLogin
        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalCookie)]
        [AllowAnonymous]
        [Route("ExternalLogin", Name = "ExternalLogin")]
        public async Task<IHttpActionResult> GetExternalLogin(string provider, string error = null)
        {
            if (error != null)
            {
                return Redirect(Url.Content("~/") + "#error=" + Uri.EscapeDataString(error));
            }

            if (!User.Identity.IsAuthenticated)
            {
                return new ChallengeResult(provider, this);
            }

            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            if (externalLogin == null)
            {
                return InternalServerError();
            }

            if (externalLogin.LoginProvider != provider)
            {
                Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                return new ChallengeResult(provider, this);
            }

            ApplicationUser user = await UserManager.FindAsync(new UserLoginInfo(externalLogin.LoginProvider,
                externalLogin.ProviderKey));

            bool hasRegistered = user != null;

            if (hasRegistered)
            {
                Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                
                 ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(UserManager,
                    OAuthDefaults.AuthenticationType);
                ClaimsIdentity cookieIdentity = await user.GenerateUserIdentityAsync(UserManager,
                    CookieAuthenticationDefaults.AuthenticationType);

                AuthenticationProperties properties = ApplicationOAuthProvider.CreateProperties(user.UserName);
                Authentication.SignIn(properties, oAuthIdentity, cookieIdentity);
            }
            else
            {
                IEnumerable<Claim> claims = externalLogin.GetClaims();
                ClaimsIdentity identity = new ClaimsIdentity(claims, OAuthDefaults.AuthenticationType);
                Authentication.SignIn(identity);
            }

            return Ok();
        }

        // GET api/Account/ExternalLogins?returnUrl=%2F&generateState=true
        [AllowAnonymous]
        [Route("ExternalLogins")]
        public IEnumerable<ExternalLoginViewModel> GetExternalLogins(string returnUrl, bool generateState = false)
        {
            IEnumerable<AuthenticationDescription> descriptions = Authentication.GetExternalAuthenticationTypes();
            List<ExternalLoginViewModel> logins = new List<ExternalLoginViewModel>();

            string state;

            if (generateState)
            {
                const int strengthInBits = 256;
                state = RandomOAuthStateGenerator.Generate(strengthInBits);
            }
            else
            {
                state = null;
            }

            foreach (AuthenticationDescription description in descriptions)
            {
                ExternalLoginViewModel login = new ExternalLoginViewModel
                {
                    Name = description.Caption,
                    Url = Url.Route("ExternalLogin", new
                    {
                        provider = description.AuthenticationType,
                        response_type = "token",
                        client_id = Startup.PublicClientId,
                        redirect_uri = new Uri(Request.RequestUri, returnUrl).AbsoluteUri,
                        state = state
                    }),
                    State = state
                };
                logins.Add(login);
            }

            return logins;
        }


        // POST api/Account/RegisterExternal
        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("RegisterExternal")]
        public async Task<IHttpActionResult> RegisterExternal(RegisterExternalBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var info = await Authentication.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return InternalServerError();
            }

            var user = new ApplicationUser() { UserName = model.Email, Email = model.Email };

            IdentityResult result = await UserManager.CreateAsync(user);
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            result = await UserManager.AddLoginAsync(user.Id, info.Login);
            if (!result.Succeeded)
            {
                return GetErrorResult(result); 
            }
            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

        #region Helpers

        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

        private class ExternalLoginData
        {
            public string LoginProvider { get; set; }
            public string ProviderKey { get; set; }
            public string UserName { get; set; }

            public IList<Claim> GetClaims()
            {
                IList<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.NameIdentifier, ProviderKey, null, LoginProvider));

                if (UserName != null)
                {
                    claims.Add(new Claim(ClaimTypes.Name, UserName, null, LoginProvider));
                }

                return claims;
            }

            public static ExternalLoginData FromIdentity(ClaimsIdentity identity)
            {
                if (identity == null)
                {
                    return null;
                }

                Claim providerKeyClaim = identity.FindFirst(ClaimTypes.NameIdentifier);

                if (providerKeyClaim == null || String.IsNullOrEmpty(providerKeyClaim.Issuer)
                    || String.IsNullOrEmpty(providerKeyClaim.Value))
                {
                    return null;
                }

                if (providerKeyClaim.Issuer == ClaimsIdentity.DefaultIssuer)
                {
                    return null;
                }

                return new ExternalLoginData
                {
                    LoginProvider = providerKeyClaim.Issuer,
                    ProviderKey = providerKeyClaim.Value,
                    UserName = identity.FindFirstValue(ClaimTypes.Name)
                };
            }
        }

        private static class RandomOAuthStateGenerator
        {
            private static RandomNumberGenerator _random = new RNGCryptoServiceProvider();

            public static string Generate(int strengthInBits)
            {
                const int bitsPerByte = 8;

                if (strengthInBits % bitsPerByte != 0)
                {
                    throw new ArgumentException("strengthInBits must be evenly divisible by 8.", "strengthInBits");
                }

                int strengthInBytes = strengthInBits / bitsPerByte;

                byte[] data = new byte[strengthInBytes];
                _random.GetBytes(data);
                return HttpServerUtility.UrlTokenEncode(data);
            }
        }

        #endregion*/

        #region  //会员体系升级
        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [CrossSite]
        [HttpPost]
        [Route("DoLoginByPhone")]
        public IHttpActionResult DoLoginByPhone(LoginViewModel model)
        {
            try
            {


                ReturnResult _captchaResult = _AppContext.UserSecurityApp.ValidateMobileVerifyCode(model.UserName, model.Captcha);
                if (!_captchaResult.IsSuccess)
                {
                    return Json(new { code = 401, msg = "验证码错误或已过期，请重新获取" });
                }
                FrontIdentityUser applicationUser;
                if (this.LoginFactory(model, out applicationUser, 2))
                {
                    //if (applicationUser != null)
                    //{
                    //    if (applicationUser.IsNeedModifyPw == 1)
                    //    {
                    //        return Json(new { code = 300, msg = "请先重置密码" });
                    //    }
                    //}
                    _AppContext.UserMessageRecordApp.InsertLoginChangePasswordMessage(applicationUser.Id);
                    return Json(new { code = 200, Id = applicationUser.Id, IsNew = false });
                }
                else
                {
                    IdentityResult CreateResult = null;
                    //1、验证请求
                    if (!ModelState.IsValid)
                    {
                        var msg = string.Empty;
                        ModelState.Values.Where(c => c.Errors.Any()).Select(r => r.Errors).ForEach((e) =>
                        {
                            if (e.FirstOrDefault() != null)
                                msg += e.FirstOrDefault().ErrorMessage + "\n";
                        });
                        return BadRequest(msg);
                    }

                    //2、验证手机验证码
                    //ReturnResult _captchaResult = _AppContext.UserSecurityApp.ValidateMobileVerifyCode(model.Mobile, model.Captcha);
                    //if (!_captchaResult.IsSuccess)
                    //{
                    //    return Ok(new ReturnObject("user-515"));
                    //}

                    // if (model.MType != MembershipType.WhitoutCar && string.IsNullOrEmpty(model.IdentityNumber)) return this.Ok(new ReturnObject("user-514"));// BadRequest("身份证号不能为空");

                    var store = new FrontUserStore<FrontIdentityUser>();
                    var membershipManager = new UserManager<FrontIdentityUser>(store);

                    if (store.CheckUserNameIsExist(model.UserName)) return this.Ok(new ReturnObject("user-206"));
                    //if (store.IsIdentityNumberRepeate(model.IdentityNumber)) return this.Ok(new ReturnObject("user-516"));//return BadRequest("系统中已存在此身份证号");
                    //if (store.CheckNickNameIsExist(model.NickName)) return this.Ok(new ReturnObject("user-517"));//return BadRequest("系统中已存在昵称");

                    //获取请求渠道
                    IEnumerable<string> appkeys = null;
                    string appkey = string.Empty;
                    int activeWay = 0;
                    if (this.Request.Headers.TryGetValues("appkey", out appkeys))
                    {
                        appkey = appkeys.First();
                    }
                    if (appkey == "blms_wechat")
                    {
                        activeWay = (int)MembershipActiveWay.Wechat;
                    }
                    else
                    {
                        activeWay = (int)MembershipActiveWay.App;
                    }

                    if (!string.IsNullOrEmpty(model.CreatedPerson))
                    {
                        appkey = model.CreatedPerson;
                    }

                    //No = _AppContext.MemberNumberApp.GetNumber("1"),//会员卡号
                    //   NickName =model .UserName,
                    //   UserName = model.UserName,
                    //   PhoneNumber = model.UserName,
                    //   Password ="Bm"+model.UserName .Substring(model.UserName.Length - 6, 6),
                    //   Status = (int)MembershipStatus.Nomal,
                    //   CreateTime = DateTime.Now.ToLongTimeString(),
                    //   CreatedPerson = this.User.Identity.Name,
                    //   MType = (int)MembershipType.WhitoutCar,//非车主
                    //   MLevel = (int)MemshipLevel.OneStar,//级别
                    //   IsPay = (int)MembershipPayStatus.NotPay,//经销商新增的会员均为已缴纳100付费
                    //   ApprovalStatus = (int)MembershipApprovalStatus.Activing, //激活中
                    //   ActiveWay = (int)MembershipActiveWay.ClientWeb , 
                    //   IsNeedModifyPw = (int)MembershipNeedModifyPw.No

                    //3、获取注册信息
                    var membershipIdentity = new FrontIdentityUser
                    {
                        No = _AppContext.MemberNumberApp.GetNumber("1"),
                        PhoneNumber = model.UserName,
                        UserName = model.UserName,
                        NickName = CommonUtilitys.GetNikeName(),
                        Password = "Bm" + model.UserName.Substring(model.UserName.Length - 6, 6),
                        Status = (int)MembershipStatus.Nomal,
                        CreateTime = DateTime.Now.ToLongTimeString(),
                        CreatedPerson = appkey == string.Empty ? this.User.Identity.Name : appkey,
                        MType = (int)MembershipType.WhitoutCar,//非车主
                        MLevel = (int)MemshipLevel.OneStar,//级别
                        IsPay = (int)MembershipPayStatus.NotPay,//经销商新增的会员均为已缴纳100付费
                        ApprovalStatus = (int)MembershipApprovalStatus.Activing, //激活中
                        ActiveWay = activeWay, //数据来源
                        IsNeedModifyPw = (int)MembershipNeedModifyPw.No,
                        MLevelInvalidDate = DateTime.MaxValue,
                        MLevelBeginDate = DateTime.Now.Date,
                        AuthenticationTime = DateTime.Parse("1900-01-01"),
                        AuthenticationSource = string.Empty
                    };

                    CreateResult = membershipManager.Create(membershipIdentity, membershipIdentity.Password);
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

                    if (this.LoginFactory(model, out applicationUser, 2))
                    {
                        if (applicationUser != null)
                        {
                            if (applicationUser.IsNeedModifyPw == 1)
                            {
                                return Json(new { code = 300, msg = "请先重置密码" });
                            }
                        }
                        _AppContext.UserMessageRecordApp.InsertLoginChangePasswordMessage(applicationUser.Id);
                        _AppContext.SMSApp.SendSMS(ESmsType.动态密码登陆注册成功, membershipIdentity.PhoneNumber, new string[] { membershipIdentity.PhoneNumber, membershipIdentity.Password });
                        return Json(new { code = 200, applicationUser.Id, IsNew = true });
                    }
                    else
                    {
                        return Json(new { code = 200, msg = "登陆失败" });

                    }

                }

            }
            catch (Exception ex)
            {
                return Json(new { code = 400, msg = ex.Message });
            }
        }


        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("DoRegister_Vip")]
        [ResponseType(typeof(int))]
        public IHttpActionResult DoRegister_Vip(RegisterViewModel model)
        {
            try
            {
                //1、验证请求
                if (!ModelState.IsValid)
                {
                    var msg = string.Empty;
                    ModelState.Values.Where(c => c.Errors.Any()).Select(r => r.Errors).ForEach((e) =>
                    {
                        if (e.FirstOrDefault() != null)
                            msg += e.FirstOrDefault().ErrorMessage + "\n";
                    });
                    return BadRequest(msg);
                }

                ReturnResult _captchaResult = _AppContext.UserSecurityApp.ValidateMobileVerifyCode(model.Mobile, model.Captcha);
                if (!_captchaResult.IsSuccess)
                {
                    return Ok(new ReturnObject("user-515"));
                }

                // if (model.MType != MembershipType.WhitoutCar && string.IsNullOrEmpty(model.IdentityNumber)) return this.Ok(new ReturnObject("user-514"));// BadRequest("身份证号不能为空");

                var store = new FrontUserStore<FrontIdentityUser>();
                var membershipManager = new UserManager<FrontIdentityUser>(store);

                if (store.CheckUserNameIsExist(model.Mobile)) return this.Ok(new ReturnObject("user-206"));
                if (!string.IsNullOrEmpty(model.IdentityNumber))
                {
                    if (store.IsIdentityNumberRepeate(model.IdentityNumber)) return this.Ok(new ReturnObject("user-516"));//return BadRequest("系统中已存在此身份证号");
                }

                // if (store.CheckNickNameIsExist(model.NickName)) return this.Ok(new ReturnObject("user-517"));//return BadRequest("系统中已存在昵称");

                //获取请求渠道
                IEnumerable<string> appkeys = null;
                string appkey = string.Empty;

                if (this.Request.Headers.TryGetValues("appkey", out appkeys))
                {
                    appkey = appkeys.First();
                }
                if (!string.IsNullOrEmpty(model.CreatedPerson))
                {
                    appkey = model.CreatedPerson;
                }
                int userLevel = (int)MemshipLevel.OneStar;

                if (!string.IsNullOrEmpty(model.IdentityNumber))
                {
                    userLevel = int.Parse(_AppContext.DealerMembershipApp.GetLevel(model.IdentityNumber));
                }

                //3、获取注册信息
                var membershipIdentity = new FrontIdentityUser
                {
                    No = _AppContext.MemberNumberApp.GetNumber("1"),
                    PhoneNumber = model.Mobile,
                    UserName = model.Mobile,
                    NickName = CommonUtilitys.GetNikeName(),
                    Email = model.Email,
                    IdentityNumber = model.IdentityNumber,
                    CreatedPerson = appkey,
                    MLevel = userLevel,
                    MType = (int)model.MType,
                    SystemMType = (int)MembershipType.WhitoutCar,
                    ActiveWay = (int)MembershipActiveWay.ClientWeb,
                    MLevelBeginDate = DateTime.Parse(DateTime.Now.ToShortDateString()),
                    MLevelInvalidDate = DateTime.Parse(DateTime.Now.ToShortDateString()).AddYears(1),
                    PaperWork = model.PaperWork,
                    AuthenticationTime = DateTime.Parse("1900-01-01")
                };

                string _activedealerId = model.ActivedealerId;
                #region
                //4、对于公司客户的特殊处理逻辑
                //if (!string.IsNullOrEmpty(model.customerType) && model.customerType == "2")//判断是否为集团客户
                //{
                //    //1. 根据vin从IF_CAR中找数据，如果找不到，失败。
                //    if (!string.IsNullOrEmpty(model.VIN))
                //    {
                //        Car c = _AppContext.CarServiceUserApp.GetCarInfoByVIN(model.VIN);
                //        if (c == null)
                //        {
                //            return Ok(new ReturnObject("500", "输入的VIN码未匹配到车辆信息", null));
                //        }
                //    }
                //    else
                //    {
                //        return Ok(new ReturnObject("500", "请输入VIN码", null));
                //    }
                //    //2. 在IF_customer表中增加一条数据（custid,CustId规则：JT-110222199912013115（集团-身份证号)）
                //    var parms = new Entity.Generated.IFCustomer();
                //    parms.CustId = "JT-" + model.IdentityNumber;
                //    parms.CustName = "--";
                //    parms.CustMobile = model.Mobile;
                //    parms.IdentityNumber = model.IdentityNumber;
                //    parms.Email = model.Email;
                //    parms.Gender = "男";
                //    parms.Address = "--";
                //    parms.City = "--";

                //    Vcyber.BLMS.Entity.Generated.IFCustomer ifCus = _AppContext.CarServiceUserApp.CreateOrGetIFCustomer(parms);
                //    //3. 修改IF_car表中车辆信息的custid
                //    bool b = _AppContext.CarServiceUserApp.UpdateCarInfoByVIN(model.VIN, ifCus.CustId);
                //    if (!b)
                //    {
                //        return Ok(new ReturnObject("500", "输入的VIN码未匹配到车辆信息", null));
                //    }
                //}
                #endregion
                //5、检验身份证号和VIN码是否匹配
                Task<FrontIdentityUser> cusObj = null;
                if (!string.IsNullOrEmpty(model.IdentityNumber))
                {
                    cusObj = new FrontUserStore<FrontIdentityUser>().FindByIdentityNumber(model.IdentityNumber);
                    if (cusObj != null && cusObj.Result != null)
                    {
                        string phone = cusObj.Result.PhoneNumber.Substring(3, 4);
                        return Ok(new ReturnObject("500", string.Format("您的证件号已经和{0}绑定,请联系bluemembers在线客服", cusObj.Result.PhoneNumber.Replace(phone, "****")), null));
                    }
                    //提取生日
                    if (model.IdentityNumber.Length == 15)
                    {
                        var str = model.IdentityNumber.Substring(6, 6);
                        membershipIdentity.Birthday = "19" + str.Remove(2) + "-" + str.Substring(2, 2) + "-" + str.Substring(4, 2);
                    }
                    if (model.IdentityNumber.Length == 18)
                    {
                        var str = model.IdentityNumber.Substring(6, 8);
                        membershipIdentity.Birthday = str.Remove(4) + "-" + str.Substring(4, 2) + "-" + str.Substring(6, 2);
                    }
                }

                //6、注册用户,如果用户存在则更新该用户信息
                IdentityResult result = null;
                if (!(cusObj != null && cusObj.Result != null))
                {
                    result = membershipManager.Create(membershipIdentity, model.Password);                    
                }
                //7、注册完成跳转
                if (result != null && result.Succeeded)
                {
                    int IsHasCar = 0;
                    // 1)更新车主级别 
                    var gradeUser = membershipManager.FindByName(model.Mobile);
                    var carList = _AppContext.CarServiceUserApp.CarsByPID(model.IdentityNumber);

                    if (carList.Count() > 0)
                    {
                        gradeUser.SystemMType = (int)MembershipType.WhitCar;
                        gradeUser.AuthenticationTime = DateTime.Now;
                        gradeUser.AuthenticationSource = appkey;
                        IsHasCar = 1;
                        //判断等级有效时间
                        gradeUser.MLevelBeginDate = gradeUser.AuthenticationTime;
                        if (gradeUser.MLevel == 11 || gradeUser.MLevel == 12)
                        {
                            gradeUser.MLevelInvalidDate = gradeUser.AuthenticationTime.AddYears(1);
                        }                   
                    }
                    membershipManager.Update(gradeUser);
                    var account = new FrontUserStore<FrontIdentityUser>().FindByNameAsync(model.Mobile).Result;

                    // 5)发送短信
                    _AppContext.SMSApp.SendSMS(ESmsType.注册成功, model.Mobile, new string[] { "" });

                    var returnIntegralType = (int)_AppContext.CarServiceUserApp.GetReIntegralTypeByIdentity(model.IdentityNumber);

                    return this.Ok(new ReturnObject("200", "注册成功", account == null ? -1 : account.SystemMType, returnIntegralType.ToString(), account.Id));
                }
                AddErrors(result);

                var errorMsg = result.Errors.FirstOrDefault();
                if (errorMsg != null && errorMsg.StartsWith("Passwords")) errorMsg = "密码必须为6位以上数字和字母组合";
                if (errorMsg != null && errorMsg.StartsWith("Name")) errorMsg = "您的手机号已经注册过";

                // 如果我们进行到这一步时某个地方出错，则重新显示表单
                return Ok(new ReturnObject("201", errorMsg.ToString(), null));
            }
            catch (Exception ex)
            {
                log.ErrorFormat("自动注册发生错误：{0}", ex);
                return Ok(new ReturnObject("201", ex.Message, null));
            }
        }
        #endregion




        #region DMS - MB 经销商协助入会；
        ///// <summary>
        ///// 经销商协助入会
        ///// </summary>
        ///// <param name="model">经销商，客户信息；</param>
        ///// <returns>会员信息；</returns>
        //[HttpPost]
        //[Route("CreateMembershipjoinMember")]
        //[ResponseType(typeof(ResMembership))]
        //[IOVAuthorize]
        //public IHttpActionResult CreateMembershipjoinMember(RequestMembership model)
        //{
        //    ReturnResult result = new ReturnResult() { IsSuccess = true, Errors = "", Message = "", Data = new { } };
        //    try
        //    {
        //        #region  校验参数信息
        //        //if (string.IsNullOrEmpty(model.Operator))
        //        //{
        //        //    result.IsSuccess = false;
        //        //    result.Message = "录入人ID不能为空";
        //        //    return Ok(result);
        //        //}
        //        if (string.IsNullOrEmpty(model.IdentityNumber))
        //        {
        //            result.IsSuccess = false;
        //            result.Message = "身份证号码不能为空";
        //            return Ok(result);
        //        }
        //        if (string.IsNullOrEmpty(model.PhoneNumber))
        //        {
        //            result.IsSuccess = false;
        //            result.Message = "手机号码不能为空";
        //            return Ok(result);
        //        }
        //        if (string.IsNullOrEmpty(model.DealerId))
        //        {
        //            result.IsSuccess = false;
        //            result.Message = "经销商编号不能为空";
        //            return Ok(result);
        //        }
        //        if (string.IsNullOrEmpty(model.PaperWork))
        //        {
        //            result.IsSuccess = false;
        //            result.Message = "证件类型不能为空";
        //            return Ok(result);
        //        }
        //        if (model.IdentityNumber.Length < 15 || model.IdentityNumber.Length > 18)
        //        {
        //            result.IsSuccess = false;
        //            result.Message = "输入的身份证号码不正确";
        //            return Ok(result);
        //        }
        //        //TO  DO   身份证应该添加校验；

        //        IdentityResult CreateResult = null;
        //        var store = new FrontUserStore<FrontIdentityUser>();
        //        var membershipManager = new UserManager<FrontIdentityUser>(store);
        //        if (store.IsIdentityNumberRepeate(model.IdentityNumber))
        //        {
        //            result.IsSuccess = false;
        //            result.Message = "系统中已存在此身份证号";
        //            return Ok(result);
        //        }
        //        if (store.CheckUserNameIsExist(model.PhoneNumber))
        //        {
        //            result.IsSuccess = false;
        //            result.Message = "系统中已存在此手机号";
        //            return Ok(result);

        //        }
        //        var customerInfo = _AppContext.DealerApp.GetCustomerInfoByIdentityNumber(model.IdentityNumber);
        //        if (customerInfo == null)
        //        {
        //            result.IsSuccess = false;
        //            result.Message = "系统中未找到购车客户信息";
        //            return Ok(result);
        //        }

        //        var dealer = _AppContext.DealerApp.GetDealerByDealerId(model.DealerId);
        //        if (dealer == null)
        //        {
        //            result.IsSuccess = false;
        //            result.Message = "未找到经销商信息 ";
        //            return Ok(result);
        //        }


        //        Decimal amount = 0;
        //        if (!string.IsNullOrEmpty(model.Amount))
        //        {
        //            amount = Convert.ToDecimal(model.Amount);
        //        }
        //        #endregion

        //        var member = store.FindByNameAsync(model.PhoneNumber).Result;
        //        var resMembership = new ResMembership();
        //        //构建会员信息入库
        //        if (member == null || string.IsNullOrEmpty(member.Id))
        //        {
        //            var mlevel = Convert.ToInt32(_AppContext.DealerMembershipApp.GetFirstRegistMLevelByIdNumber(model.IdentityNumber));
        //            var carList = _AppContext.CarServiceUserApp.SelectCarListByIdentity(model.IdentityNumber).ToList<Car>();
        //            var membershipIdentity = new FrontIdentityUser
        //            {
        //                No = _AppContext.MemberNumberApp.GetNumber("1"),
        //                NickName = CommonUtilitys.GetNikeName(),
        //                SystemMType = (carList != null && carList.Count > 0 ? (int)MembershipType.WhitCar : (int)MembershipType.WhitoutCar),
        //                RealName = customerInfo.CustName,
        //                UserName = model.PhoneNumber,
        //                PhoneNumber = model.PhoneNumber,
        //                IdentityNumber = model.IdentityNumber,
        //                Password = "Bm" + model.PhoneNumber.Substring(5),//这里由原来的身份证改为手机号后6微
        //                Status = (int)MembershipStatus.Nomal,
        //                CreateTime = DateTime.Now.ToLongTimeString(),
        //                CreatedPerson = model.DealerId,
        //                MType = (int)MembershipType.WhitCar,
        //                MLevel = mlevel,//级别
        //                IsPay = (int)MembershipPayStatus.Paid,//经销商新增的会员均为已缴纳100付费
        //                ApprovalStatus = (int)MembershipApprovalStatus.Activing, //激活中
        //                ActiveWay = (int)MembershipActiveWay.ManageWeb, //后台提交激活流程
        //                IsNeedModifyPw = (int)MembershipNeedModifyPw.No,
        //                MLevelBeginDate = DateTime.Now,
        //                MLevelInvalidDate = mlevel > 10 ? DateTime.Now.AddYears(1) : DateTime.MaxValue,
        //                Amount = amount,
        //                Mid = model.IdentityNumber
        //            };
        //            try
        //            {
        //                CreateResult = membershipManager.Create(membershipIdentity, membershipIdentity.Password);
        //            }
        //            catch (Exception ex)
        //            {
        //                result.IsSuccess = false;
        //                result.Message = "经销商协助入会出错了";
        //                return Ok(result);
        //            }
        //            //会员信息入库成功后处理相关数据操作；
        //            if (CreateResult.Succeeded)
        //            {
        //                //会员是否有同一号码,如果有则通过 membershipIdentity.Id  获取会员信息；
        //                var user = store.FindByNameAsync(model.PhoneNumber).Result;
        //                if (user == null || user.Id != membershipIdentity.Id)
        //                {
        //                    user = store.FindByIdAsync(membershipIdentity.Id).Result;
        //                }
        //                var userStore = new FrontUserTable<FrontIdentityUser>();
        //                //TODO  证件类型先默认身份证
        //                membershipIdentity.PaperWork = model.PaperWork;
        //                userStore.AddPaperworkToMembership_Schedule(membershipIdentity);
        //                //添加多少积分
        //                var intergration = _AppContext.CarServiceUserApp.GetIntegrationByBuyCarPayMoney(model.IdentityNumber);
        //                //会员积分入库；
        //                bool flag = _AppContext.DealerMembershipApp.IsPersonalUser(model.IdentityNumber);
        //                if (flag)
        //                {
        //                    UpdateUserIntegralNewMember(user, intergration); //添加积分
        //                }

        //                //记录经销商协助会员记录；
        //                store.AddMembershipDealerRecord(user.Id, model.DealerId);

        //                //构建经销商协助入会返回信息；
        //                resMembership.Amount = model.Amount;
        //                resMembership.BlueMembership_Id = user.Id;
        //                resMembership.BlueMembership_No = user.No;
        //                resMembership.MLevel = user.MLevel.ToString();
        //                resMembership.MLevelBeginDate = membershipIdentity.MLevelBeginDate.ToString("yyyyMMddHHmmss");
        //            }
        //            else
        //            {
        //                result.IsSuccess = false;
        //                result.Message = "FAIL";
        //                return Ok(result);
        //            }
        //        }
        //        result.Data = resMembership;
        //        result.Message = "SUCCESS";
        //        return Ok(result);
        //    }
        //    catch (Exception)
        //    {
        //        result.IsSuccess = false;
        //        result.Message = "FAIL";
        //        return Ok(result);
        //    }
        //}

        private void UpdateUserIntegralNewMember(FrontIdentityUser user, int Integral)
        {



            #region   会员体系升级
            _AppContext.UserIntegralApp.Add(new UserIntegral
            {
                CreateTime = DateTime.Now,
                datastate = 0, //这里逻辑默认0
                UpdateTime = DateTime.Now,
                userId = user.Id,
                value = Integral, //新购4000积分
                IntegralBeginDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd")),
                IntegralInvalidDate = Convert.ToDateTime(DateTime.Now.AddYears(2).ToString("yyyy/MM/dd")),
                integralSource = "30",
                remark = "经销商入会返积分"
            });
            _AppContext.UserIntegralApp.AddUserIntegralRecord(new UserIntegral
            {
                CreateTime = DateTime.Now,
                datastate = 0, //这里逻辑默认0
                UpdateTime = DateTime.Now,
                userId = user.Id,
                value = Integral, //新购4000积分
                IntegralBeginDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd")),
                IntegralInvalidDate = Convert.ToDateTime(DateTime.Now.AddYears(2).ToString("yyyy/MM/dd")),
                integralSource = "30",
                ProductName = "",
                remark = "经销商入会返积分"
            });
            if (Integral > 0)
            {
                //发送短信
                _AppContext.SMSApp.SendSMS(ESmsType.后台新增会员, user.UserName,
                    new string[] { (Integral / 10) + "", user.UserName, "Bm" + user.PhoneNumber.Substring(5) });
            }
            else
            {
                //发送短信
                _AppContext.SMSApp.SendSMS(ESmsType.新增会员不返积分, user.UserName,
                    new string[] { user.UserName, "Bm" + user.PhoneNumber.Substring(5) });
            }

            #endregion
        }


        /// <summary>
        /// 经销商一键入会；
        /// </summary>
        /// <param name="model">经销商，客户信息；</param>
        /// <returns>会员信息</returns>
        [HttpPost]
        [Route("CreateMembership")]
        [ResponseType(typeof(ResMembership))]
        //[IOVAuthorize]
        public IHttpActionResult CreateMembership(RequestJoinMembership model)
        {            
            ReturnResult result = new ReturnResult() { IsSuccess = true, Errors = "", Message = "", Data = new { } };
            try
            {
                #region  校验参数信息
                //if (string.IsNullOrEmpty(model.Operator))
                //{
                //    result.IsSuccess = false;
                //    result.Message = "录入人ID不能为空";
                //    return Ok(result);
                //}
                if (string.IsNullOrEmpty(model.IdentityNumber))
                {
                    result.IsSuccess = false;
                    result.Message = "身份证号码不能为空";
                    return Ok(result);
                }
                if (string.IsNullOrEmpty(model.PhoneNumber))
                {
                    result.IsSuccess = false;
                    result.Message = "手机号码不能为空";
                    return Ok(result);
                }
                if (string.IsNullOrEmpty(model.DealerId))
                {
                    result.IsSuccess = false;
                    result.Message = "经销商编号不能为空";
                    return Ok(result);
                }
                if (string.IsNullOrEmpty(model.PaperWork))
                {
                    result.IsSuccess = false;
                    result.Message = "证件类型不能为空";
                    return Ok(result);
                }
                if (model.IdentityNumber.Length < 15 || model.IdentityNumber.Length > 18)
                {
                    result.IsSuccess = false;
                    result.Message = "输入的身份证号码不正确";
                    return Ok(result);
                }

                var dealer = _AppContext.DealerApp.GetDealerByDealerId(model.DealerId);
                if (dealer == null)
                {
                    result.IsSuccess = false;
                    result.Message = "未找到经销商信息 ";
                    return Ok(result);
                }
                //TO  DO   身份证应该添加校验；

                IdentityResult CreateResult = null;
                var store = new FrontUserStore<FrontIdentityUser>();
                var membershipManager = new UserManager<FrontIdentityUser>(store);
                if (store.IsIdentityNumberRepeate(model.IdentityNumber))
                {
                    result.IsSuccess = false;
                    //result.Message = "系统中已存在此身份证号";
                    result.Message = "入会失败，该客户已是蓝缤会员，在其他经销商入过会";
                    return Ok(result);
                }
                if (store.CheckUserNameIsExist(model.PhoneNumber))
                {
                    result.IsSuccess = false;
                    //result.Message = "系统中已存在此手机号";
                    result.Message = "入会失败，该客户已是蓝缤会员，在其他经销商入过会";
                    return Ok(result);

                }
                //update by wangchunrong 20160830

                //var customerInfo = _AppContext.DealerApp.GetCustomerInfoByIdentityNumber(model.IdentityNumber);
                //if (customerInfo == null)
                //{
                //    result.IsSuccess = false;
                //    result.Message = "系统中未找到购车客户信息";
                //    return Ok(result);
                //}
                Decimal amount = 0;
                if (!string.IsNullOrEmpty(model.Amount))
                {
                    amount = Convert.ToDecimal(model.Amount);
                }
                #endregion

                var member = store.FindByNameAsync(model.PhoneNumber).Result;
                var resMembership = new ResMembership();
                if (member == null || string.IsNullOrEmpty(member.Id))
                {
                    var mlevel = Convert.ToInt32(_AppContext.DealerMembershipApp.GetFirstRegistMLevelByIdNumber(model.IdentityNumber));
                    var carList = _AppContext.CarServiceUserApp.SelectCarListByIdentity(model.IdentityNumber).ToList<Car>();
                    var membershipIdentity = new FrontIdentityUser
                    {
                        No = _AppContext.MemberNumberApp.GetNumber("1"),
                        NickName = CommonUtilitys.GetNikeName(),
                        SystemMType = (carList != null && carList.Count > 0 ? (int)MembershipType.WhitCar : (int)MembershipType.WhitoutCar),
                        // RealName = customerInfo.CustName,
                        UserName = model.PhoneNumber,
                        PhoneNumber = model.PhoneNumber,
                        IdentityNumber = model.IdentityNumber,
                        Password = "Bm" + model.PhoneNumber.Substring(5),//这里由原来的身份证改为手机号后6微
                        Status = (int)MembershipStatus.Nomal,
                        CreateTime = DateTime.Now.ToLongTimeString(),
                        CreatedPerson = model.DealerId,
                        MType = (int)MembershipType.WhitCar,
                        MLevel = mlevel,//级别
                        IsPay = Convert.ToInt32(model.Agree),//经销商新增的会员均为已缴纳100付费 //支付状态
                        ApprovalStatus = (int)MembershipApprovalStatus.Activing, //激活中
                        ActiveWay = (int)MembershipActiveWay.ManageWeb, //后台提交激活流程
                        IsNeedModifyPw = (int)MembershipNeedModifyPw.No,
                        MLevelBeginDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")),
                        MLevelInvalidDate = mlevel > 10 ? Convert.ToDateTime(DateTime.Now.AddYears(1).ToString("yyyy-MM-dd")) : DateTime.MaxValue,
                        Amount = amount,
                        Mid = model.IdentityNumber,
                        //AuthenticationTime = DateTime.Parse("1900-01-01")
                        //认证的时候，身份证号下没有车的时候，认证时间给一个默认的
                        AuthenticationTime =(carList != null && carList.Count > 0 ? DateTime.Parse(DateTime.Now.ToString()) : DateTime.Parse("1900-01-01"))
                    };
                    try
                    {
                        CreateResult = membershipManager.Create(membershipIdentity, membershipIdentity.Password);
                    }
                    catch (Exception ex)
                    {
                        result.IsSuccess = false;
                        result.Message = "经销商一键入会出错了";
                        log4net.LogManager.GetLogger("经销商一键入会出错").Error(model.IdentityNumber, ex);
                        return Ok(result);
                    }
                    if (CreateResult.Succeeded)
                    {
                        //会员是否有同一号码,如果有则通过 membershipIdentity.Id  获取会员信息；
                        var user = store.FindByNameAsync(model.PhoneNumber).Result;
                        if (user == null || user.Id != membershipIdentity.Id)
                        {
                            user = store.FindByIdAsync(membershipIdentity.Id).Result;
                        }
                        var userStore = new FrontUserTable<FrontIdentityUser>();
                        membershipIdentity.PaperWork = model.PaperWork;
                        userStore.AddPaperworkToMembership_Schedule(membershipIdentity);
                        if (model.Agree == "1")//符合缴费条件
                        {
                            var intergration = _AppContext.CarServiceUserApp.GetIntegrationByBuyCarPayMoney(model.IdentityNumber);//添加多少积分
                            bool flag = _AppContext.DealerMembershipApp.IsPersonalUser(model.IdentityNumber);
                            if (flag)
                            {
                                UpdateUserIntegralNewMember(user, intergration);//添加积分
                            }
                        }
                        else
                        {
                            _AppContext.SMSApp.SendSMS(ESmsType.新增会员不返积分, model.PhoneNumber, new string[] { user.UserName, "Bm" + user.PhoneNumber.Substring(5) });
                        }
                        //记录经销商协助会员记录；
                        store.AddMembershipDealerRecord(user.Id, model.DealerId);

                        #region
                        //-------------====================---------------------
                        //获取会员信息；
                        //Task<FrontIdentityUser> cusObj = null;
                        //cusObj = new FrontUserStore<FrontIdentityUser>().FindByIdentityNumber(identityNumber);
                        //if (cusObj == null || cusObj.Result == null || string.IsNullOrEmpty(cusObj.Result.Id))
                        //{
                        //    result.IsSuccess = false;
                        //    result.Message = "未找到该会员信息";
                        //    return Ok(result);
                        //}
                        //var userInfo = cusObj.Result;
                        ////获取用户有效积分；
                        //var _totalScore = _AppContext.UserIntegralApp.GetTotalIntegral(userInfo.Id);

                        //var resMemberUserintegral = new ResMemberUserintegralInfo()
                        //{
                        //    BlueMembership_Id = userInfo.Id,
                        //    BlueMembership_No = string.IsNullOrEmpty(userInfo.No) ? "" : userInfo.No,
                        //    BlueMembership_YN = string.IsNullOrEmpty(userInfo.No) ? "N" : "Y",
                        //    MLevel = userInfo.MLevel.ToString(),
                        //    MLevelInvalidDate = userInfo.MLevelInvalidDate.ToString("yyyyMMddHHmmss"),
                        //    Point = _totalScore.ToString()
                        //};
                        #endregion
                        //构建经销商协助入会返回信息；
                        resMembership.Amount = string.IsNullOrEmpty(model.Amount) ? "" : model.Amount;
                        resMembership.BlueMembership_Id = user.Id;
                        resMembership.BlueMembership_No = user.No;
                        resMembership.MLevel = user.MLevel.ToString();
                        resMembership.MLevelBeginDate = membershipIdentity.MLevelBeginDate.ToString("yyyyMMddHHmmss");//会员生效日期
                        resMembership.MLevelInvalidDate = membershipIdentity.MLevelInvalidDate.ToString("yyyyMMddHHmmss");//会员失效日期
                        //resMembership.AuthenticationTime = membershipIdentity.AuthenticationTime.ToString("yyyyMMddHHmmss");//车主认证等级时间
                        resMembership.AuthenticationTime = DateTime.Parse(membershipIdentity.CreateTime).ToString("yyyyMMddHHmmss");//根据DMS要求，字段不变，值取入会时间。
                    }
                }
                result.Data = resMembership;
                result.Message = "SUCCESS";
                log4net.LogManager.GetLogger("经销商一键入会成功").Error(model.IdentityNumber);
                return Ok(result);
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = "ERROR";
                log4net.LogManager.GetLogger("经销商一键入会出错").Error(model.IdentityNumber, ex);
                return Ok(result);
            }
        }




        /// <summary>
        /// 查询会员积分
        /// </summary>
        /// <param name="identityNumber">会员身份证号码</param>
        /// <returns>会员积分信息</returns>
        [HttpPost]
        [Route("GetMemberUserintegralInfo")]
        [ResponseType(typeof(ResMemberUserintegralInfo))]
        //[IOVAuthorize]
        public IHttpActionResult GetMemberUserintegralInfo(string identityNumber)
        {
            ReturnResult result = new ReturnResult() { IsSuccess = true, Errors = "", Message = "", Data = new { } };
            try
            {
                if (string.IsNullOrEmpty(identityNumber))
                {
                    result.IsSuccess = false;
                    result.Message = "身份证号码不能为空";
                    return Ok(result);
                }
                //获取会员信息；
                Task<FrontIdentityUser> cusObj = null;
                cusObj = new FrontUserStore<FrontIdentityUser>().FindByIdentityNumber(identityNumber);
                if (cusObj == null || cusObj.Result == null || string.IsNullOrEmpty(cusObj.Result.Id))
                {
                    result.IsSuccess = false;
                    result.Message = "未找到该会员信息";
                    return Ok(result);
                }
                var userInfo = cusObj.Result;
                //获取用户有效积分（余额）；
                //var _totalScore = _AppContext.UserIntegralApp.GetTotalIntegral(userInfo.Id);
               //可用的卡券的数量
                var _ReceivedCardCount = _AppContext.CustomCardApp.GetReceivedCustomCardCount(userInfo.Id);
                var resMemberUserintegral = new ResMemberUserintegralInfo()
                {
                    BlueMembership_Id = userInfo.Id,
                    BlueMembership_No = string.IsNullOrEmpty(userInfo.No) ? "" : userInfo.No,
                    BlueMembership_YN = string.IsNullOrEmpty(userInfo.No) ? "N" : "Y",
                    MLevel = userInfo.MLevel.ToString(),
                    MLevelBeginDate = userInfo.MLevelBeginDate.ToString("yyyyMMddHHmmss"),
                    MLevelInvalidDate = userInfo.MLevelInvalidDate.ToString("yyyyMMddHHmmss"),
                    //add by wangchunrong 20170303 车主认证时间
                    //AuthenticationTime = userInfo.AuthenticationTime.ToString("yyyyMMddHHmmss"),
                    AuthenticationTime =DateTime.Parse(userInfo.CreateTime).ToString("yyyyMMddHHmmss"),//根据DMS要求，字段不变，取值变成入会时间
                    //Point = _totalScore.ToString(),//剩余积分
                    //add by wangchunrong 2016-09-06
                    Address = string.IsNullOrEmpty(userInfo.Address) ? "" : userInfo.Address,
                    Birthday = string.IsNullOrEmpty(userInfo.Birthday) ? "" : userInfo.Birthday,
                    Email = string.IsNullOrEmpty(userInfo.Email) ? "" : userInfo.Email,
                    Gender = string.IsNullOrEmpty(userInfo.Gender) ? "" : userInfo.Gender,
                    IdentityNumber = userInfo.IdentityNumber,
                    PhoneNumber = userInfo.PhoneNumber,
                    RealName = string.IsNullOrEmpty(userInfo.RealName) ? "" : userInfo.RealName,
                    UserName = string.IsNullOrEmpty(userInfo.UserName) ? "" : userInfo.UserName,
                    Scale = "1", //根据需求调整为小数点格式。
                    MaxConsumInte = "800",
                    ReceivedCardCount = _ReceivedCardCount.ToString()
                    //end add
                };
                //获取用户积分（总，使用，剩余）
                IntegraInfo integrainfo = _AppContext.UserIntegralApp.GetIntegralIn(userInfo.Id).FirstOrDefault();

                if (integrainfo != null)
                {
                    resMemberUserintegral.TotalPoint = integrainfo.total.ToString();
                    resMemberUserintegral.UserValue = integrainfo.usevalue.ToString();
                    resMemberUserintegral.Point = integrainfo.Surplus.ToString(); 
                }
                else
                {
                    resMemberUserintegral.TotalPoint = "0"; 
                    resMemberUserintegral.UserValue = "0";
                    resMemberUserintegral.Point = "0";
                }

                result.Data = resMemberUserintegral;
                result.Message = "SUCCESS";
                return Ok(result);
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = "ERROR";
                return Ok(result);
            }
        }


        ///// <summary>
        ///// 会员信息查询
        ///// </summary>
        ///// <param name="identityNumber">会员身份证号码</param>
        ///// <returns>会员信息</returns>
        //[HttpPost]
        //[Route("GetMembershipByIdentityNumber")]
        //[ResponseType(typeof(ResMembershipInfo))]
        //[IOVAuthorize]
        //public IHttpActionResult GetMembershipByIdentityNumber(string identityNumber)
        //{
        //    ReturnResult result = new ReturnResult() { IsSuccess = true, Errors = "", Message = "", Data = new { } };
        //    try
        //    {
        //        if (string.IsNullOrEmpty(identityNumber))
        //        {
        //            result.IsSuccess = false;
        //            result.Message = "身份证号码不能为空";
        //            return Ok(result);
        //        }
        //        //获取会员信息；
        //        Task<FrontIdentityUser> cusObj = null;
        //        cusObj = new FrontUserStore<FrontIdentityUser>().FindByIdentityNumber(identityNumber);
        //        if (cusObj == null || cusObj.Result == null || string.IsNullOrEmpty(cusObj.Result.Id))
        //        {
        //            result.IsSuccess = false;
        //            result.Message = "未找到该会员信息";
        //            return Ok(result);
        //        }
        //        var userInfo = cusObj.Result;
        //        var resMembershipInfo = new ResMembershipInfo()
        //        {
        //            Address = string.IsNullOrEmpty(userInfo.Address) ? "" : userInfo.Address,
        //            Birthday = string.IsNullOrEmpty(userInfo.Birthday) ? "" : userInfo.Birthday,
        //            BlueMembership_Id = userInfo.Id,
        //            BlueMembership_No = userInfo.No,
        //            Email = string.IsNullOrEmpty(userInfo.Email) ? "" : userInfo.Email,
        //            Gender = string.IsNullOrEmpty(userInfo.Gender) ? "" : userInfo.Gender,
        //            IdentityNumber = userInfo.IdentityNumber,
        //            MLevel = userInfo.MLevel.ToString(),
        //            PhoneNumber = userInfo.PhoneNumber,
        //            RealName = string.IsNullOrEmpty(userInfo.RealName) ? "" : userInfo.RealName,
        //            UserName = string.IsNullOrEmpty(userInfo.UserName) ? "" : userInfo.UserName
        //        };
        //        result.Data = resMembershipInfo;
        //        result.Message = "SUCCESS";
        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        result.IsSuccess = false;
        //        result.Message = "ERROR";
        //        return Ok(result);
        //    }
        //}


        /// <summary>
        /// 经销商认证审核
        /// </summary>
        /// <param name="model">经销商，会员信息审核信息</param>
        /// <returns>会员信息审核信息</returns>
        [HttpPost]
        [Route("MembershipApproval")]
        [ResponseType(typeof(ResMembershipApproval))]
        //[IOVAuthorize]
        public IHttpActionResult MembershipApproval(RequestMembershipApproval model)
        {
            ReturnResult result = new ReturnResult() { IsSuccess = true, Errors = "", Message = "", Data = new { } };
            try
            {
                if (string.IsNullOrEmpty(model.IdentityNumber))
                {
                    result.IsSuccess = false;
                    result.Message = "证件号码不能为空";
                    return Ok(result);
                }
                if (string.IsNullOrEmpty(model.DealerId))
                {
                    result.IsSuccess = false;
                    result.Message = "经销商ID不能为空";
                    return Ok(result);
                }
                //add by wangchunrong 2016-09-08 需求确认为不需要这个字段

                //if (string.IsNullOrEmpty(model.Vin))
                //{
                //    result.IsSuccess = false;
                //    result.Message = "车架号不能为空";
                //    return Ok(result);
                //}
                if (string.IsNullOrEmpty(model.PhoneNumber))
                {
                    result.IsSuccess = false;
                    result.Message = "手机号码不能为空";
                    return Ok(result);
                }

                //获取会员信息；
                Task<FrontIdentityUser> cusObj = null;
                cusObj = new FrontUserStore<FrontIdentityUser>().FindByIdentityNumber(model.IdentityNumber);
                if (cusObj == null || cusObj.Result == null || string.IsNullOrEmpty(cusObj.Result.Id))
                {
                    result.IsSuccess = false;
                    result.Message = "未找到该身份证下的会员信息 ";
                    return Ok(result);
                }
                var dealer = _AppContext.DealerApp.GetDealerByDealerId(model.DealerId);
                if (dealer == null)
                {
                    result.IsSuccess = false;
                    result.Message = "未找到经销商信息 ";
                    return Ok(result);
                }
                if (cusObj.Result.UserName != model.PhoneNumber)
                {
                    result.IsSuccess = false;
                    result.Message = "未找到该手机号码下的会员信息 ";
                    return Ok(result);
                }

                var resMembershipApproval = new ResMembershipApproval();

                //获取经销商入会的需要审核的信息
                var approvingList = new FrontUserStore<FrontIdentityUser>().GetApprovingMembershipList(model.DealerId,
                    model.IdentityNumber);
                //是否有需要审核信息
                if (approvingList.Result != null && approvingList.Result.Count == 0)
                {
                    result.IsSuccess = false;
                    result.Message = "通过认证失败,该会员已经交费认证成功！ ";
                    return Ok(result);
                }
                else
                {
                    //开始审核
                    string message;
                    var approvalId = approvingList.Result.FirstOrDefault().ApprovalId;
                    var appro = new FrontUserStore<FrontIdentityUser>().ApprovalMembershipRequest(approvalId,
                        out message, model.PhoneNumber);
                    //审核不通过 
                    if (!appro.Result)
                    {
                        result.IsSuccess = false;
                        result.Message = message;
                        return Ok(result);
                    }
                    else
                    {
                        var intergration =
                            _AppContext.CarServiceUserApp.GetIntegrationByBuyCarPayMoney(model.IdentityNumber);
                        resMembershipApproval.Point = intergration.ToString();
                        resMembershipApproval.BlueMembership_Id = cusObj.Result.Id;
                        resMembershipApproval.BlueMembership_No = cusObj.Result.No;
                        //add by wangchunrong 2016-09-04
                        resMembershipApproval.Mlevel = cusObj.Result.MLevel.ToString();
                        resMembershipApproval.BlueMembership_YN = string.IsNullOrEmpty(cusObj.Result.No) ? "N" : "Y";
                        resMembershipApproval.MLevelBeginDate = cusObj.Result.MLevelBeginDate.ToString("yyyyMMddHHmmss");   //会员生效日期
                        resMembershipApproval.MLevelInvalidDate = cusObj.Result.MLevelInvalidDate.ToString("yyyyMMddHHmmss");  //会员失效日期
                        //resMembershipApproval.AuthenticationTime = cusObj.Result.AuthenticationTime.ToString("yyyyMMddHHmmss");//车主认证等级时间20170303add
                        resMembershipApproval.AuthenticationTime =DateTime.Parse(cusObj.Result.CreateTime).ToString("yyyyMMddHHmmss");//根据DMS要求，字段不变，取值变成入会时间
                    }
                }

                //返回审批结果信息
                result.Data = resMembershipApproval;
                result.IsSuccess = true;
                result.Message = "SUCCESS";
                log4net.LogManager.GetLogger("经销商认证审核成功").Error(model.DealerId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = "ERROR";
                log4net.LogManager.GetLogger("经销商认证审核出错").Error(model.DealerId, ex);
                return Ok(result);
            }
        }



        private DateTime GetstringToDateTime(string tempTime)
        {

            IFormatProvider format = new System.Globalization.CultureInfo("zh-CN");
            string TarStr = "yyyyMMddHHmmss";  //注意这里用到HH
            DateTime MyDate = DateTime.ParseExact(tempTime, TarStr, format);
            return MyDate;
        }

        /// <summary>
        /// 新增积分消费
        /// </summary>
        /// <param name="model">会员积分消费信息</param>
        /// <returns>返回积分信息</returns>
        [HttpPost]
        [Route("AddMembersConsume")]
        [ResponseType(typeof(ResMembersConsume))]
        //[IOVAuthorize]
        public IHttpActionResult AddMembersConsume(RequestMembersConsume model)
        {
            Stream s = System.Web.HttpContext.Current.Request.InputStream;
            byte[] b = new byte[s.Length];
            s.Read(b, 0, (int)s.Length);
            var postStr = Encoding.UTF8.GetString(b);
            BLMS.Common.LogService.Instance.Info("新增积分消费接口请求内容:" + postStr);

            ReturnResult result = new ReturnResult() { IsSuccess = true, Errors = "", Message = "", Data = new { } };
            try
            {
                ConsumeEntity entity = new ConsumeEntity();
                var resMembersConsume = new ResMembersConsume();

                #region  验证新增积分消费参数

                if (string.IsNullOrEmpty(model.ConsumeDate))
                {
                    result.IsSuccess = false;
                    result.Message = "消费时间不能空";
                    return Ok(result);
                }
                if (string.IsNullOrEmpty(model.ConsumePoints))
                {
                    result.IsSuccess = false;
                    result.Message = "消费积分不能为空";
                    return Ok(result);
                }
                if (string.IsNullOrEmpty(model.ConsumeType))
                {
                    result.IsSuccess = false;
                    result.Message = "消费类型不能为空";
                    return Ok(result);
                }
                if (string.IsNullOrEmpty(model.DealerId))
                {
                    result.IsSuccess = false;
                    result.Message = "经销商编号不能为空";
                    return Ok(result);
                }
                if (string.IsNullOrEmpty(model.IdentityNumber))
                {
                    result.IsSuccess = false;
                    result.Message = "证件号码不能为空";
                    return Ok(result);
                }
                if (string.IsNullOrEmpty(model.PhoneNumber))
                {
                    result.IsSuccess = false;
                    result.Message = "手机号码不能为空";
                    return Ok(result);
                }
                if (string.IsNullOrEmpty(model.TotalCost))
                {
                    result.IsSuccess = false;
                    result.Message = "总费用不能为空";
                    return Ok(result);
                }
                //if (string.IsNullOrEmpty(model.VIN))
                //{
                //    result.IsSuccess = false;
                //    result.Message = "车架号不能为空 ";
                //    return Ok(result);
                //}
                //获取会员信息；
                Task<FrontIdentityUser> cusObj = null;
                cusObj = new FrontUserStore<FrontIdentityUser>().FindByIdentityNumber(model.IdentityNumber);
                if (cusObj == null || cusObj.Result == null || string.IsNullOrEmpty(cusObj.Result.Id))
                {
                    result.IsSuccess = false;
                    result.Message = "未找到该身份证下的会员信息 ";
                    return Ok(result);
                }
                entity.UserId = cusObj.Result.Id;

                var dealer = _AppContext.DealerApp.GetDealerByDealerId(model.DealerId);
                if (dealer == null)
                {
                    result.IsSuccess = false;
                    result.Message = "未找到经销商信息 ";
                    return Ok(result);
                }

                IFCustomer customer = _AppContext.CarServiceUserApp.GetCustomer(model.IdentityNumber);
                if (customer == null)
                {
                    result.IsSuccess = false;
                    result.Message = "未找到该身份证下的车主资料";
                    return Ok(result);
                }
                if (!string.IsNullOrEmpty(model.VIN))
                {
                    Car car = _AppContext.CarServiceUserApp.GetCarInfoByVIN(model.VIN);
                    if (car == null)
                    {
                        result.IsSuccess = false;
                        result.Message = "未找到该身份证下的车辆信息";
                        return Ok(result);
                    }
                }
               

                #endregion

                //总费用
                //entity.PurchaseCost = Convert.ToInt32(model.TotalCost);
                entity.PurchaseCost = Convert.ToDecimal(model.TotalCost);

                //消费积分
                entity.ConsumePoints = Convert.ToInt32(model.ConsumePoints);
                

                //根据用户等级，返积分，不同等级分别返（普卡：0.1；银卡：0.15；金卡：0.2；生日月：0.5）
                //var discount = _AppContext.UserIntegralApp.GetUserIntegralDiscount(entity.UserId);

                //首次保养不需要费用，不返积分；
                if (Convert.ToInt32(model.ConsumeType) == (int)EConsumeType.SCBY)
                {
                    //实际总费用
                    entity.TotalCost = 0;
                    //抵扣积分
                    entity.ConsumePoints = 0;
                    //积分抵扣元
                    entity.PointCost = 0;
                    //总费用
                    entity.PurchaseCost = 0;
                }
                else
                {
                    //积分抵扣元
                   // var tempPointCost = (entity.ConsumePoints * 1) / 10;
                    decimal tempPointCost = (Convert.ToDecimal(entity.ConsumePoints) * 1) / 10;

                    //实际总费用
                    var tempCost = (entity.PurchaseCost * 1) - tempPointCost;

                    entity.PointCost = Convert.ToDecimal(tempPointCost);
                    entity.TotalCost = Convert.ToDecimal(tempCost);

                    #region 使用的积分抵扣的钱不可超过实际费用

                    if (tempPointCost > entity.PurchaseCost)
                    {
                        result.IsSuccess = false;
                        result.Message = "使用的积分不可超过实际费用！！！";
                        return Ok(result);
                    }
                    #endregion

                }

                entity.UserName = cusObj.Result.UserName;
                entity.ConsumeDate = GetstringToDateTime(model.ConsumeDate);
                entity.DealerId = model.DealerId;
                entity.DealerName = dealer.Name;
                entity.Phone = model.PhoneNumber;
                if (!string.IsNullOrEmpty(model.VIN))
                {
                    entity.VIN = model.VIN;
                }
                entity.ConsumeType = Convert.ToInt32(model.ConsumeType);
                entity.IdentityNumber = model.IdentityNumber;
                int oldUserIntegral = _AppContext.UserIntegralApp.GetTotalIntegral(entity.UserId);
                entity.DMSOrderNo = model.DMSOrderNo;//根据dms需求，添加DMS的工单号到BM
                #region 根据不同的消费类型，去判断可以使用的积分的上限


                var _totalScore = _AppContext.UserIntegralApp.GetTotalIntegral(cusObj.Result.Id);
                //3:定期保养，0:事故车维修，8:钣喷,1:首次保养，2：购车 9:配件 10：精品
                switch (entity.ConsumeType)
                {
                    case 0:
                        if (entity.ConsumePoints > 800)
                        {
                            result.IsSuccess = false;
                            result.Message = "消费积分超过上限，最高可消费积分800！！！ ";
                            return Ok(result);
                        }
                        else if (_totalScore < entity.ConsumePoints)
                        {
                            result.IsSuccess = false;
                            result.Message = "您的账户中的积分不足！！！ ";
                            return Ok(result);
                        }
                        break;
                    case 1:
                        if (entity.ConsumePoints > 0)//|| _totalScore < entity.ConsumePoints
                        {
                            result.IsSuccess = false;
                            result.Message = "首次养护不可以使用积分消费！！！ ";
                            return Ok(result);
                        }
                        break;
                    case 2:
                        if ( _totalScore < entity.ConsumePoints)
                        {
                            result.IsSuccess = false;
                            result.Message = "您的账户中的积分不足！！！ ";
                            return Ok(result);
                        }
                        break;
                    case 3:
                        if (entity.ConsumePoints > 800)
                        {
                            result.IsSuccess = false;
                            result.Message = "消费积分超过上限，最高可消费积分800！！！ ";
                            return Ok(result);
                        }
                        else if (_totalScore < entity.ConsumePoints)
                        {
                            result.IsSuccess = false;
                            result.Message = "您的账户中的积分不足！！！ ";
                            return Ok(result);
                        }
                        break;
                    case 8:
                        if (entity.ConsumePoints > 800)
                        {
                            result.IsSuccess = false;
                            result.Message = "消费积分超过上限，最高可消费积分800！！！ ";
                            return Ok(result);
                        }
                        else if (_totalScore < entity.ConsumePoints)
                        {
                            result.IsSuccess = false;
                            result.Message = "您的账户中的积分不足！！！ ";
                            return Ok(result);
                        }
                        break;
                    case 9:
                        if (entity.ConsumePoints > 800)
                        {
                            result.IsSuccess = false;
                            result.Message = "消费积分超过上限，最高可消费积分800！！！ ";
                            return Ok(result);
                        }
                        else if (_totalScore < entity.ConsumePoints)
                        {
                            result.IsSuccess = false;
                            result.Message = "您的账户中的积分不足！！！ ";
                            return Ok(result);
                        }
                        break;
                    case 10:
                        if (entity.ConsumePoints > 800)
                        {
                            result.IsSuccess = false;
                            result.Message = "消费积分超过上限，最高可消费积分800！！！ ";
                            return Ok(result);
                        }
                        else if (_totalScore < entity.ConsumePoints)
                        {
                            result.IsSuccess = false;
                            result.Message = "您的账户中的积分不足！！！ ";
                            return Ok(result);
                        }
                        break;
                    default:
                        break;
                }

                #endregion



                //消耗积分
                if ((entity.ConsumePoints ?? 0) > 0)
                {
                    if (
                        !_AppContext.TradePort.TradeService(
                            entity.UserId,
                            entity.ConsumePoints ?? 0,
                            (EOrderMode)entity.EConsumeType))
                    {
                        result.IsSuccess = false;
                        result.Message = "消费积分失败 ";
                        return Ok(result);
                    }
                }

                //增加消费记录
                int id = _AppContext.ConsumeApp.Add(
                    entity,
                    dealer.Id,
                    dealer.DealerId);
                //直接发放积分
                if (_AppContext.DealerMembershipApp.IsPersonalUser(entity.IdentityNumber))
                {
                    //购车不返积分
                    if (entity.ConsumeType != (int)EConsumeType.Purchase)
                    {
                        _AppContext.ConsumeApp.AddAndProcess(entity.UserId, id);
                    }
                    else
                    {
                        _AppContext.UserIntegralApp.AddUserIntegralRecord(new UserIntegral
                        {
                            userId = entity.UserId,
                            integralSource = "2",
                            value = Convert.ToInt32(model.ConsumePoints) * (-1),
                            datastate = 0,
                            ProductName = dealer.DealerId,
                            remark = "维保消费消耗积分",
                            IntegralBeginDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd")),
                            IntegralInvalidDate = Convert.ToDateTime(DateTime.Now.AddYears(2).ToString("yyyy/MM/dd")),
                            UpdateTime = DateTime.Now,
                            CreateTime = DateTime.Now
                        });
                    }
                }
                int newUserIntegral = _AppContext.UserIntegralApp.GetTotalIntegral(entity.UserId);
                //var pointSeq = "0";
                decimal pointSeq = 0;
                if ((entity.ConsumePoints ?? 0) > 0)
                {
                    //根据用户等级，返积分，不同等级分别返（普卡：0.1；银卡：0.15；金卡：0.2；生日月：0.5）
                    var discount = _AppContext.UserIntegralApp.GetUserIntegralDiscount(entity.UserId);
                    UserMessageRecord userMessageRecord = new UserMessageRecord();
                    userMessageRecord.UserId = entity.UserId;
                    userMessageRecord.MsgType = MessageType.IntegralConsum;
                    userMessageRecord.MsgContent = string.Format("您好，您于{0}使用{1}积分抵维保消费{2}元，感谢您的支持，祝您生活愉快。",
                        DateTime.Now, entity.ConsumePoints, entity.ConsumePoints * 0.1);
                    _AppContext.UserMessageRecordApp.Insert(userMessageRecord);//----------------------------------------------------------------------------------
                    if (Convert.ToInt32(model.ConsumeType) == (int)EConsumeType.Purchase)
                    {
                        pointSeq = 0;
                    }
                    else
                    {
                        pointSeq = (Math.Round(entity.TotalCost * Convert.ToDecimal(discount)));//.ToString()
                    }
                    _AppContext.SMSApp.SendSMS(ESmsType.消费管理_积分消耗成功, entity.Phone,
                        new string[]
                        {
                            DateTime.Now.Year.ToString(),
                            DateTime.Now.Month.ToString(),
                            DateTime.Now.Day.ToString(),
                            entity.DealerId + "-" + entity.DealerName,
                            Vcyber.BLMS.Common.EnumExtension.GetDiscribe<EConsumeType>(entity.EConsumeType),
                            oldUserIntegral.ToString(),
                            (entity.ConsumePoints ?? 0).ToString(),
                            ((int) (entity.ConsumePoints ?? 0)*0.1).ToString(),
                            entity.TotalCost.ToString(),
                            pointSeq.ToString(),
                            newUserIntegral.ToString()
                        });
                }

                resMembersConsume.Point = _AppContext.UserIntegralApp.GetTotalIntegral(entity.UserId).ToString();
                resMembersConsume.PointSeq = pointSeq.ToString();
                //resMembersConsume.OrderNo = _AppContext.ConsumeApp.GetOrderNoById(id);
                result.Data = resMembersConsume;
                result.IsSuccess = true;
                result.Message = "SUCCESS";
                return Ok(result);
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = "ERROR";
                return Ok(result);
            }
        }
        #endregion

        #region 缴费获取积分数据接口
        /// <summary>
        /// 缴费获取积分待审核列表
        /// </summary>
        /// <param name="reppmodel">请求参数（必填）</param>
        /// <returns></returns>
        [HttpPost]
        [ResponseType(typeof(PaymentAccessPoint))]
        [Route("PaymentAccessPoints")]
        [IOVAuthorize]
        //public IHttpActionResult PaymentAccessPoints(string dealerid, string starttime, string endtime)
        public IHttpActionResult PaymentAccessPoints(RequestPaymentAccessPoint reppmodel)
        {
            ReturnResult result = new ReturnResult() { IsSuccess = true, Errors = "", Message = "", Data = new { } };
           
                try
                {
                if (string.IsNullOrEmpty(reppmodel.dealerid))
                {
                    result.IsSuccess = false;
                    result.Message = "经销商ID不能为空！！！";
                    return Ok(result);
                }
                IEnumerable<PaymentAccessPoint> query = _AppContext.UserMessageRecordApp.GetPaymentAccessPoint(reppmodel.dealerid, reppmodel.starttime, reppmodel.endtime);
                    result.Data = query;
                    //result.Message = "缴费获取积分数据成功";
                    result.Message = "SUCCESS";
                    return Ok(result);
                }
                catch (Exception ex)
                {
                    result.IsSuccess = false;
                    //result.Message = "缴费获取积分数据出错！";
                    result.Message = "ERROR";
                    return Ok(result);
                }

            
          
        }

        #endregion

        #region 取消工单消费
        /// <summary>
        /// 取消工单消费
        /// </summary>
        /// <param name="DMSOrderNo">工单号</param>
        /// <returns></returns>
        [HttpPost]
        [Route("CancelMembersConsume")]
        //RequestCancelConsume
        public IHttpActionResult CancelMembersConsume(string DMSOrderNo)
        {
            Stream s = System.Web.HttpContext.Current.Request.InputStream;
            byte[] b = new byte[s.Length];
            s.Read(b, 0, (int)s.Length);
            var postStr = Encoding.UTF8.GetString(b);
            BLMS.Common.LogService.Instance.Info("取消工单消费接口请求内容:" + postStr);

            ReturnResult result = new ReturnResult() { IsSuccess = true, Errors = "", Message = "", Data = new { } };
            try
            {
                ConsumeEntity entity = new ConsumeEntity();
                var resMembersConsume = new ResMembersConsume();

                #region  验证取消工单消费参数

                if (string.IsNullOrEmpty(DMSOrderNo))
                {
                    result.IsSuccess = false;
                    result.Message = "消费工单号不能空";
                    return Ok(result);
                }
          
                var OrderInfo = _AppContext.ConsumeApp.GetUserConsumeByDmsNo(DMSOrderNo);
                if (OrderInfo == null)
                {
                    result.IsSuccess = false;
                    result.Message = "未找到该订单号信息 ";
                    return Ok(result);
                }

                //_AppContext.UserIntegralApp.InserDmsCancelOrder(OrderNo);
                var integralDatas = _AppContext.UserIntegralApp.GetList(OrderInfo.UserId);
                if (integralDatas != null && integralDatas.Count() > 0)
                {
                    int overIntegral = OrderInfo.RewardPoints;//消费使用积分

                    foreach (var integralItem in integralDatas)
                    {
                        int realerIntegral = integralItem.value - integralItem.usevalue;
                        int subIntegral = overIntegral > realerIntegral ? realerIntegral : overIntegral;

                        if (overIntegral < realerIntegral)
                        {
                             _AppContext.UserIntegralApp.SubIntegral(integralItem.Id, OrderInfo.UserId, Math.Abs(subIntegral));
                            break;
                        }
                        
                    }

                }
                _AppContext.UserIntegralApp.DmsCancelOrder(DMSOrderNo);


                //新增消费记录（userintegral，UserIntegralRecord）

                _AppContext.UserIntegralApp.Add(new UserIntegral
                {
                    CreateTime = DateTime.Now,
                    datastate = 0, //这里逻辑默认0
                    UpdateTime = DateTime.Now,
                    userId = OrderInfo.UserId,
                    value = Convert.ToInt32(OrderInfo.ConsumePoints),
                    IntegralBeginDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd")),
                    IntegralInvalidDate = Convert.ToDateTime(DateTime.Now.AddYears(2).ToString("yyyy/MM/dd")),
                    integralSource = "120",
                    remark = "取消消费工单返还积分"
                });

                //减去返还的积分


                //把工单消费时使用的积分数量重新加上
                _AppContext.UserIntegralApp.AddUserIntegralRecord(new UserIntegral
                {
                    userId = OrderInfo.UserId,
                    integralSource = "120",
                    value = Convert.ToInt32(OrderInfo.ConsumePoints),
                    datastate = 0,
                    ProductName = OrderInfo.DealerId,
                    remark = "取消消费工单返还积分",
                    IntegralBeginDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd")),
                    IntegralInvalidDate = Convert.ToDateTime(DateTime.Now.AddYears(2).ToString("yyyy/MM/dd")),
                    UpdateTime = DateTime.Now,
                    CreateTime = DateTime.Now
                });
                //把工单消费时返还的积分减掉
                _AppContext.UserIntegralApp.AddUserIntegralRecord(new UserIntegral
                {
                    userId = OrderInfo.UserId,
                    integralSource = "120",
                    value = Convert.ToInt32(OrderInfo.RewardPoints) * (-1),
                    datastate = 0,
                    ProductName = OrderInfo.DealerId,
                    remark = "取消消费工单返还积分",
                    IntegralBeginDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd")),
                    IntegralInvalidDate = Convert.ToDateTime(DateTime.Now.AddYears(2).ToString("yyyy/MM/dd")),
                    UpdateTime = DateTime.Now,
                    CreateTime = DateTime.Now
                });

                #endregion
                UserMessageRecord userMessageRecord = new UserMessageRecord();
                userMessageRecord.UserId = OrderInfo.UserId;
                userMessageRecord.MsgType = MessageType.IntegralConsum;
                userMessageRecord.MsgContent = string.Format("您好，您于{0}取消积分消费,本次取消返还您{1}积分，感谢您的支持，祝您生活愉快。",
                    DateTime.Now, OrderInfo.ConsumePoints - OrderInfo.RewardPoints);
                _AppContext.UserMessageRecordApp.Insert(userMessageRecord);
                return Ok(result);
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = "ERROR";
                return Ok(result);
            }
        }
        #endregion

        #region 修改工单消费

        /// <summary>
        /// 修改工单消费
        /// </summary>
        /// <param name="data">工单号</param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdateMembersConsume")]
        //RequestCancelConsume
        public IHttpActionResult UpdateMembersConsume(RequestUpdateConsuem data)
        {
            Stream s = System.Web.HttpContext.Current.Request.InputStream;
            byte[] b = new byte[s.Length];
            s.Read(b, 0, (int)s.Length);
            var postStr = Encoding.UTF8.GetString(b);
            BLMS.Common.LogService.Instance.Info("修改工单消费接口请求内容:" + postStr);

            ReturnResult result = new ReturnResult() { IsSuccess = true, Errors = "", Message = "", Data = new { } };
            try
            {
                ConsumeEntity entity = new ConsumeEntity();
                var resMembersConsume = new ResMembersConsume();

                if (string.IsNullOrEmpty(data.DMSOrderNo))
                {
                    result.IsSuccess = false;
                    result.Message = "消费工单号不能为空";
                    return Ok(result);
                }
                
                if (string.IsNullOrEmpty(Convert.ToString(data.ConsumePoints)))
                {
                    result.IsSuccess = false;
                    result.Message = "消费积分不能为空";
                    return Ok(result);
                }
             
                if (string.IsNullOrEmpty(data.TotalCost))
                {
                    result.IsSuccess = false;
                    result.Message = "总费用不能为空";
                    return Ok(result);
                }
                //根据消费工单查找工单信息
                var OrderInfo = _AppContext.ConsumeApp.GetUserConsumeByDmsNo(data.DMSOrderNo);
                if (OrderInfo == null)
                {
                    result.IsSuccess = false;
                    result.Message = "未找到该订单号信息 ";
                    return Ok(result);
                }

                #region 获取会员信息；
                //获取会员信息；
                Task<FrontIdentityUser> cusObj = null;
                cusObj = new FrontUserStore<FrontIdentityUser>().FindByIdentityNumber(OrderInfo.IdentityNumber);

                if (cusObj == null || cusObj.Result == null || string.IsNullOrEmpty(cusObj.Result.Id))
                {
                    result.IsSuccess = false;
                    result.Message = "未找到该身份证下的会员信息 ";
                    return Ok(result);
                }
                entity.UserId = cusObj.Result.Id;

                var dealer = _AppContext.DealerApp.GetDealerByDealerId(OrderInfo.DealerId);
                if (dealer == null)
                {
                    result.IsSuccess = false;
                    result.Message = "未找到经销商信息 ";
                    return Ok(result);
                }

                IFCustomer customer = _AppContext.CarServiceUserApp.GetCustomer(OrderInfo.IdentityNumber);
                if (customer == null)
                {
                    result.IsSuccess = false;
                    result.Message = "未找到该身份证下的未找到车主资料";
                    return Ok(result);
                }
                #endregion
                //总费用
                entity.PurchaseCost = Convert.ToDecimal(data.TotalCost);
                //消费积分
                entity.ConsumePoints = Convert.ToInt32(data.ConsumePoints);
                //首次保养不需要费用，不返积分；
                if (Convert.ToInt32(OrderInfo.ConsumeType) == (int)EConsumeType.SCBY)
                {
                    //实际总费用
                    entity.TotalCost = 0;
                    //抵扣积分
                    entity.ConsumePoints = 0;
                    //积分抵扣元
                    entity.PointCost = 0;
                    //总费用
                    entity.PurchaseCost = 0;
                }
                else
                {
                    //积分抵扣元
                    // var tempPointCost = (entity.ConsumePoints * 1) / 10;
                    decimal tempPointCost = (Convert.ToDecimal(entity.ConsumePoints) * 1) / 10;

                    //实际总费用
                    var tempCost = (entity.PurchaseCost * 1) - tempPointCost;

                    entity.PointCost = Convert.ToDecimal(tempPointCost);
                    entity.TotalCost = Convert.ToDecimal(tempCost);
                    
                    if (tempPointCost > entity.PurchaseCost)
                    {
                        result.IsSuccess = false;
                        result.Message = "使用的积分不可超过实际费用！！！";
                        return Ok(result);
                    }
                   

                }

                entity.ConsumeType = Convert.ToInt32(OrderInfo.ConsumeType);
                entity.IdentityNumber = OrderInfo.IdentityNumber;
                int oldUserIntegral = _AppContext.UserIntegralApp.GetTotalIntegral(entity.UserId);

                var _totalScore = _AppContext.UserIntegralApp.GetTotalIntegral(cusObj.Result.Id);
                #region 根据类型判断消费积分上限
                //3:定期保养，0:事故车维修，8:钣喷,1:首次保养，2：购车 9:配件 10：精品
                switch (entity.ConsumeType)
                {
                    case 0:
                        if (entity.ConsumePoints > 800)
                        {
                            result.IsSuccess = false;
                            result.Message = "消费积分超过上限，最高可消费积分800！！！ ";
                            return Ok(result);
                        }
                        else if (_totalScore < entity.ConsumePoints)
                        {
                            result.IsSuccess = false;
                            result.Message = "您的账户中的积分不足！！！ ";
                            return Ok(result);
                        }
                        break;
                    case 1:
                        if (entity.ConsumePoints > 0)//|| _totalScore < entity.ConsumePoints
                        {
                            result.IsSuccess = false;
                            result.Message = "首次养护不可以使用积分消费！！！ ";
                            return Ok(result);
                        }
                        break;
                    case 2:
                        if (_totalScore < entity.ConsumePoints)
                        {
                            result.IsSuccess = false;
                            result.Message = "您的账户中的积分不足！！！ ";
                            return Ok(result);
                        }
                        break;
                    case 3:
                        if (entity.ConsumePoints > 800)
                        {
                            result.IsSuccess = false;
                            result.Message = "消费积分超过上限，最高可消费积分800！！！ ";
                            return Ok(result);
                        }
                        else if (_totalScore < entity.ConsumePoints)
                        {
                            result.IsSuccess = false;
                            result.Message = "您的账户中的积分不足！！！ ";
                            return Ok(result);
                        }
                        break;
                    case 8:
                        if (entity.ConsumePoints > 800)
                        {
                            result.IsSuccess = false;
                            result.Message = "消费积分超过上限，最高可消费积分800！！！ ";
                            return Ok(result);
                        }
                        else if (_totalScore < entity.ConsumePoints)
                        {
                            result.IsSuccess = false;
                            result.Message = "您的账户中的积分不足！！！ ";
                            return Ok(result);
                        }
                        break;
                    case 9:
                        if (entity.ConsumePoints > 800)
                        {
                            result.IsSuccess = false;
                            result.Message = "消费积分超过上限，最高可消费积分800！！！ ";
                            return Ok(result);
                        }
                        else if (_totalScore < entity.ConsumePoints)
                        {
                            result.IsSuccess = false;
                            result.Message = "您的账户中的积分不足！！！ ";
                            return Ok(result);
                        }
                        break;
                    case 10:
                        if (entity.ConsumePoints > 800)
                        {
                            result.IsSuccess = false;
                            result.Message = "消费积分超过上限，最高可消费积分800！！！ ";
                            return Ok(result);
                        }
                        else if (_totalScore < entity.ConsumePoints)
                        {
                            result.IsSuccess = false;
                            result.Message = "您的账户中的积分不足！！！ ";
                            return Ok(result);
                        }
                        break;
                    default:
                        break;
                }
                #endregion
                //根据用户等级，返积分，不同等级分别返（普卡：0.1；银卡：0.15；金卡：0.2；生日月：0.5）
                var discount = _AppContext.UserIntegralApp.GetUserIntegralDiscount(entity.UserId);
                //计算应返还的积分
                decimal pointSeq = 0;
                pointSeq = (Math.Round(entity.TotalCost * Convert.ToDecimal(discount)));
                //积分抵扣元
                // var tempPointCost = (entity.ConsumePoints * 1) / 10;
                decimal tempPointCost1 = (Convert.ToDecimal(entity.ConsumePoints) * 1) / 10;

                //实际总费用
                var tempCost1 = (entity.PurchaseCost * 1) - tempPointCost1;
                //直接发放积分
                if (_AppContext.DealerMembershipApp.IsPersonalUser(entity.IdentityNumber))
                {
                    //购车不返积分
                    if (entity.ConsumeType != (int)EConsumeType.Purchase)
                    {
                        decimal UpdateConsumePoints = (OrderInfo.ConsumePoints - data.ConsumePoints);
                        decimal UpdateRewardPoints = (pointSeq - OrderInfo.RewardPoints);
                        decimal UpdateIntegral = UpdateConsumePoints + UpdateRewardPoints;
                        //新增消费记录抵消
                        //_AppContext.UserIntegralApp.DmsCancelOrder(data.OrderNo);

                        //修改消费记录（工单修改的时候使用）
                        _AppContext.UserIntegralApp.DmsUpdateOrder(new Consuem
                        {
                            UserId = OrderInfo.UserId,
                            UserName = customer.CustName,
                            DealerId = dealer.DealerId,
                            DealerName = dealer.Name,
                            ConsumePoints = Convert.ToInt32(data.ConsumePoints),
                            RewardPoints = Convert.ToInt32(pointSeq),
                            Phone = customer.CustMobile,
                            ConsumeType = OrderInfo.ConsumeType,
                            IdentityNumber = customer.IdentityNumber,
                            VIN = entity.VIN,
                            DMSOrderNo = data.DMSOrderNo,
                            TotalCost = Convert.ToString(tempCost1),
                            PurchaseCost =data.TotalCost,
                            PointCost = Convert.ToString(entity.PointCost)

                        }
                            );

                        if (UpdateIntegral > 0)
                        {
                            //新增消费记录（userintegral，UserIntegralRecord）

                            _AppContext.UserIntegralApp.Add(new UserIntegral
                            {
                                CreateTime = DateTime.Now,
                                datastate = 0, //这里逻辑默认0
                                UpdateTime = DateTime.Now,
                                userId = entity.UserId,
                                value = Convert.ToInt32(UpdateIntegral),
                                IntegralBeginDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd")),
                                IntegralInvalidDate = Convert.ToDateTime(DateTime.Now.AddYears(2).ToString("yyyy/MM/dd")),
                                integralSource = "120",
                                remark = "修改消费工单返还积分"
                            });
                        }
                        else
                        {
                            var integralDatas = _AppContext.UserIntegralApp.GetList(entity.UserId);
                            if (integralDatas != null && integralDatas.Count() > 0)
                            {
                                int overIntegral = Convert.ToInt32((-UpdateIntegral));//消费使用积分

                                foreach (var integralItem in integralDatas)
                                {
                                    int realerIntegral = integralItem.value - integralItem.usevalue;
                                    int subIntegral = overIntegral > realerIntegral ? realerIntegral : overIntegral;

                                    if (overIntegral < realerIntegral)
                                    {
                                        _AppContext.UserIntegralApp.SubIntegral(integralItem.Id, entity.UserId, Math.Abs(subIntegral));
                                        break;
                                    }

                                }

                            }
                        }
                        //把工单消费修改的积分数量加/减
                        _AppContext.UserIntegralApp.AddUserIntegralRecord(new UserIntegral
                        {
                            userId = entity.UserId,
                            integralSource = "120",
                            value = Convert.ToInt32(UpdateIntegral),
                            datastate = 0,
                            ProductName = dealer.DealerId,
                            remark = "修改消费工单积分",
                            IntegralBeginDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd")),
                            IntegralInvalidDate = Convert.ToDateTime(DateTime.Now.AddYears(2).ToString("yyyy/MM/dd")),
                            UpdateTime = DateTime.Now,
                            CreateTime = DateTime.Now
                        });
                    }
                }
                int newUserIntegral = _AppContext.UserIntegralApp.GetTotalIntegral(entity.UserId);
                //decimal pointSeq = 0;
                if ((entity.ConsumePoints ?? 0) > 0)
                {
                    //根据用户等级，返积分，不同等级分别返（普卡：0.1；银卡：0.15；金卡：0.2；生日月：0.5）
                    //var discount = _AppContext.UserIntegralApp.GetUserIntegralDiscount(entity.UserId);
                    UserMessageRecord userMessageRecord = new UserMessageRecord();
                    userMessageRecord.UserId = entity.UserId;
                    userMessageRecord.MsgType = MessageType.IntegralConsum;
                    userMessageRecord.MsgContent = string.Format("您好，您于{0}使用{1}积分抵维保消费{2}元，感谢您的支持，祝您生活愉快。",
                        DateTime.Now, entity.ConsumePoints, entity.ConsumePoints * 0.1);
                    _AppContext.UserMessageRecordApp.Insert(userMessageRecord);//----------------------------------------------------------------------------------
                    pointSeq = (Math.Round(entity.TotalCost * Convert.ToDecimal(discount)));//.ToString()
                    _AppContext.SMSApp.SendSMS(ESmsType.消费管理_积分消耗成功, entity.Phone,
                        new string[]
                        {
                            DateTime.Now.Year.ToString(),
                            DateTime.Now.Month.ToString(),
                            DateTime.Now.Day.ToString(),
                            entity.DealerId + "-" + entity.DealerName,
                            Vcyber.BLMS.Common.EnumExtension.GetDiscribe<EConsumeType>(entity.EConsumeType),
                            oldUserIntegral.ToString(),
                            (entity.ConsumePoints ?? 0).ToString(),
                            ((int) (entity.ConsumePoints ?? 0)*0.1).ToString(),
                            entity.TotalCost.ToString(),
                            pointSeq.ToString(),
                            newUserIntegral.ToString()
                        });
                }

                resMembersConsume.Point = _AppContext.UserIntegralApp.GetTotalIntegral(entity.UserId).ToString();
                resMembersConsume.PointSeq = pointSeq.ToString();
                //resMembersConsume.OrderNo = _AppContext.ConsumeApp.GetOrderNoById(Convert.ToInt32(OrderInfo.ID));
                result.Data = resMembersConsume;
                result.IsSuccess = true;
                result.Message = "SUCCESS";
                return Ok(result);
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = "ERROR";
                return Ok(result);
            }
        }
        #endregion

        #region 积分消费（使用积分）
        /// <summary>
        /// DMS新增工单消费（扣除积分操作）
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddMembersConsumeUseIntegral")]
        [ResponseType(typeof(ResMembersConsume))]
        //[IOVAuthorize]
        public IHttpActionResult AddMembersConsumeUseIntegral(RequestMembersConsume model)
        {
             Stream s = System.Web.HttpContext.Current.Request.InputStream;
            byte[] b = new byte[s.Length];
            s.Read(b, 0, (int)s.Length);
            var postStr = Encoding.UTF8.GetString(b);
            BLMS.Common.LogService.Instance.Info("新增积分消费的时候只处理减积分接口请求内容:" + postStr);

            ReturnResult result = new ReturnResult() { IsSuccess = true, Errors = "", Message = "", Data = new { } };
            try
            {
                ConsumeEntity entity = new ConsumeEntity();
                var resMembersConsume = new ResMembersConsume();

                #region  验证新增积分消费参数

                if (string.IsNullOrEmpty(model.ConsumeDate))
                {
                    result.IsSuccess = false;
                    result.Message = "消费时间不能空";
                    return Ok(result);
                }
                if (string.IsNullOrEmpty(model.ConsumePoints))
                {
                    result.IsSuccess = false;
                    result.Message = "消费积分不能为空";
                    return Ok(result);
                }
                if (string.IsNullOrEmpty(model.ConsumeType))
                {
                    result.IsSuccess = false;
                    result.Message = "消费类型不能为空";
                    return Ok(result);
                }
                if (string.IsNullOrEmpty(model.DealerId))
                {
                    result.IsSuccess = false;
                    result.Message = "经销商编号不能为空";
                    return Ok(result);
                }
                if (string.IsNullOrEmpty(model.IdentityNumber))
                {
                    result.IsSuccess = false;
                    result.Message = "证件号码不能为空";
                    return Ok(result);
                }
                if (string.IsNullOrEmpty(model.PhoneNumber))
                {
                    result.IsSuccess = false;
                    result.Message = "手机号码不能为空";
                    return Ok(result);
                }
                if (string.IsNullOrEmpty(model.TotalCost))
                {
                    result.IsSuccess = false;
                    result.Message = "总费用不能为空";
                    return Ok(result);
                }
                //if (string.IsNullOrEmpty(model.VIN))
                //{
                //    result.IsSuccess = false;
                //    result.Message = "车架号不能为空 ";
                //    return Ok(result);
                //}
                //获取会员信息；
                Task<FrontIdentityUser> cusObj = null;
                cusObj = new FrontUserStore<FrontIdentityUser>().FindByIdentityNumber(model.IdentityNumber);
                if (cusObj == null || cusObj.Result == null || string.IsNullOrEmpty(cusObj.Result.Id))
                {
                    result.IsSuccess = false;
                    result.Message = "未找到该身份证下的会员信息 ";
                    return Ok(result);
                }
                entity.UserId = cusObj.Result.Id;

                var dealer = _AppContext.DealerApp.GetDealerByDealerId(model.DealerId);
                if (dealer == null)
                {
                    result.IsSuccess = false;
                    result.Message = "未找到经销商信息 ";
                    return Ok(result);
                }

                IFCustomer customer = _AppContext.CarServiceUserApp.GetCustomer(model.IdentityNumber);
                if (customer == null)
                {
                    result.IsSuccess = false;
                    result.Message = "未找到该身份证下的车主资料";
                    return Ok(result);
                }
                if (!string.IsNullOrEmpty(model.VIN))
                {
                    Car car = _AppContext.CarServiceUserApp.GetCarInfoByVIN(model.VIN);
                    if (car == null)
                    {
                        result.IsSuccess = false;
                        result.Message = "未找到该身份证下的车辆信息";
                        return Ok(result);
                    }
                }


                #endregion

                //总费用
                //entity.PurchaseCost = Convert.ToInt32(model.TotalCost);
                entity.PurchaseCost = Convert.ToDecimal(model.TotalCost);

                //消费积分
                entity.ConsumePoints = Convert.ToInt32(model.ConsumePoints);


                //根据用户等级，返积分，不同等级分别返（普卡：0.1；银卡：0.15；金卡：0.2；生日月：0.5）
                //var discount = _AppContext.UserIntegralApp.GetUserIntegralDiscount(entity.UserId);

                //首次保养不需要费用，不返积分；
                if (Convert.ToInt32(model.ConsumeType) == (int)EConsumeType.SCBY)
                {
                    //实际总费用
                    entity.TotalCost = 0;
                    //抵扣积分
                    entity.ConsumePoints = 0;
                    //积分抵扣元
                    entity.PointCost = 0;
                    //总费用
                    entity.PurchaseCost = 0;
                }
                else
                {
                    //积分抵扣元
                    // var tempPointCost = (entity.ConsumePoints * 1) / 10;
                    decimal tempPointCost = (Convert.ToDecimal(entity.ConsumePoints) * 1) / 10;

                    //实际总费用
                    var tempCost = (entity.PurchaseCost * 1) - tempPointCost;

                    entity.PointCost = Convert.ToDecimal(tempPointCost);
                    entity.TotalCost = Convert.ToDecimal(tempCost);

                    #region 使用的积分抵扣的钱不可超过实际费用

                    if (tempPointCost > entity.PurchaseCost)
                    {
                        result.IsSuccess = false;
                        result.Message = "使用的积分不可超过实际费用！！！";
                        return Ok(result);
                    }
                    #endregion

                }

                entity.UserName = cusObj.Result.UserName;
                entity.ConsumeDate = GetstringToDateTime(model.ConsumeDate);
                entity.DealerId = model.DealerId;
                entity.DealerName = dealer.Name;
                entity.Phone = model.PhoneNumber;
                if (!string.IsNullOrEmpty(model.VIN))
                {
                    entity.VIN = model.VIN;
                }
                entity.ConsumeType = Convert.ToInt32(model.ConsumeType);
                entity.IdentityNumber = model.IdentityNumber;
                int oldUserIntegral = _AppContext.UserIntegralApp.GetTotalIntegral(entity.UserId);
                entity.DMSOrderNo = model.DMSOrderNo;//根据dms需求，添加DMS的工单号到BM
                #region 根据不同的消费类型，去判断可以使用的积分的上限


                var _totalScore = _AppContext.UserIntegralApp.GetTotalIntegral(cusObj.Result.Id);
                //3:定期保养，0:事故车维修，8:钣喷,1:首次保养，2：购车 9:配件 10：精品
                switch (entity.ConsumeType)
                {
                    case 0:
                        if (entity.ConsumePoints > 800)
                        {
                            result.IsSuccess = false;
                            result.Message = "消费积分超过上限，最高可消费积分800！！！ ";
                            return Ok(result);
                        }
                        else if (_totalScore < entity.ConsumePoints)
                        {
                            result.IsSuccess = false;
                            result.Message = "您的账户中的积分不足！！！ ";
                            return Ok(result);
                        }
                        break;
                    case 1:
                        if (Convert.ToInt32(model.ConsumePoints) > 0)//|| _totalScore < entity.ConsumePoints
                        {
                            result.IsSuccess = false;
                            result.Message = "首次养护不可以使用积分消费！！！ ";
                            return Ok(result);
                        }
                        break;
                    case 2:
                        if (_totalScore < entity.ConsumePoints)
                        {
                            result.IsSuccess = false;
                            result.Message = "您的账户中的积分不足！！！ ";
                            return Ok(result);
                        }
                        break;
                    case 3:
                        if (entity.ConsumePoints > 800)
                        {
                            result.IsSuccess = false;
                            result.Message = "消费积分超过上限，最高可消费积分800！！！ ";
                            return Ok(result);
                        }
                        else if (_totalScore < entity.ConsumePoints)
                        {
                            result.IsSuccess = false;
                            result.Message = "您的账户中的积分不足！！！ ";
                            return Ok(result);
                        }
                        break;
                    case 8:
                        if (entity.ConsumePoints > 800)
                        {
                            result.IsSuccess = false;
                            result.Message = "消费积分超过上限，最高可消费积分800！！！ ";
                            return Ok(result);
                        }
                        else if (_totalScore < entity.ConsumePoints)
                        {
                            result.IsSuccess = false;
                            result.Message = "您的账户中的积分不足！！！ ";
                            return Ok(result);
                        }
                        break;
                    case 9:
                        if (entity.ConsumePoints > 800)
                        {
                            result.IsSuccess = false;
                            result.Message = "消费积分超过上限，最高可消费积分800！！！ ";
                            return Ok(result);
                        }
                        else if (_totalScore < entity.ConsumePoints)
                        {
                            result.IsSuccess = false;
                            result.Message = "您的账户中的积分不足！！！ ";
                            return Ok(result);
                        }
                        break;
                    case 10:
                        if (entity.ConsumePoints > 800)
                        {
                            result.IsSuccess = false;
                            result.Message = "消费积分超过上限，最高可消费积分800！！！ ";
                            return Ok(result);
                        }
                        else if (_totalScore < entity.ConsumePoints)
                        {
                            result.IsSuccess = false;
                            result.Message = "您的账户中的积分不足！！！ ";
                            return Ok(result);
                        }
                        break;
                    default:
                        break;
                }

                #endregion
                
                //消耗积分
                if ((entity.ConsumePoints ?? 0) > 0)
                {
                    if (
                        !_AppContext.TradePort.TradeService(
                            entity.UserId,
                            entity.ConsumePoints ?? 0,
                            (EOrderMode)entity.EConsumeType))
                    {
                        result.IsSuccess = false;
                        result.Message = "消费积分失败 ";
                        return Ok(result);
                    }
                }

                //增加消费记录
                int id = _AppContext.ConsumeApp.Add(
                    entity,
                    dealer.Id,
                    dealer.DealerId);
                //IntegralRecord减积分
                _AppContext.UserIntegralApp.AddUserIntegralRecord(new UserIntegral
                        {
                            userId = entity.UserId,
                            integralSource = "2",
                            value = Convert.ToInt32(model.ConsumePoints) * (-1),
                            datastate = 0,
                            ProductName = dealer.DealerId,
                            remark = "维保消费消耗积分",
                            IntegralBeginDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd")),
                            IntegralInvalidDate = Convert.ToDateTime(DateTime.Now.AddYears(2).ToString("yyyy/MM/dd")),
                            UpdateTime = DateTime.Now,
                            CreateTime = DateTime.Now
                        });
                int newUserIntegral = _AppContext.UserIntegralApp.GetTotalIntegral(entity.UserId);
                decimal pointSeq = 0;
                if ((entity.ConsumePoints ?? 0) > 0)
                {
                    //根据用户等级，返积分，不同等级分别返（普卡：0.1；银卡：0.15；金卡：0.2；生日月：0.5）
                    var discount = _AppContext.UserIntegralApp.GetUserIntegralDiscount(entity.UserId);
                    UserMessageRecord userMessageRecord = new UserMessageRecord();
                    userMessageRecord.UserId = entity.UserId;
                    userMessageRecord.MsgType = MessageType.IntegralConsum;
                    //微信个人中心，消费中心
                    userMessageRecord.MsgContent = string.Format("您好，您于{0}使用{1}积分抵维保消费{2}元，感谢您的支持，祝您生活愉快。",
                        DateTime.Now, entity.ConsumePoints, entity.ConsumePoints * 0.1);
                    _AppContext.UserMessageRecordApp.Insert(userMessageRecord);//----------------------------------------------------------------------------------
                                                                               //短信通知
                    #region  返还积分的时候一起发信息
                    //pointSeq = (Math.Round(entity.TotalCost * Convert.ToDecimal(discount)));//.ToString()
                    //_AppContext.SMSApp.SendSMS(ESmsType.消费管理_积分消耗成功, entity.Phone,
                    //    new string[]
                    //    {
                    //        DateTime.Now.Year.ToString(),
                    //        DateTime.Now.Month.ToString(),
                    //        DateTime.Now.Day.ToString(),
                    //        entity.DealerId + "-" + entity.DealerName,
                    //        Vcyber.BLMS.Common.EnumExtension.GetDiscribe<EConsumeType>(entity.EConsumeType),
                    //        oldUserIntegral.ToString(),
                    //        (entity.ConsumePoints ?? 0).ToString(),
                    //        ((int) (entity.ConsumePoints ?? 0)*0.1).ToString(),
                    //        entity.TotalCost.ToString(),
                    //        newUserIntegral.ToString()
                    //    });
                    #endregion
                }

                resMembersConsume.Point = _AppContext.UserIntegralApp.GetTotalIntegral(entity.UserId).ToString();
                result.Data = resMembersConsume;
                result.IsSuccess = true;
                result.Message = "SUCCESS";
                return Ok(result);
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = "ERROR";
                return Ok(result);
            }
        }
        #endregion

        #region 新增积分消费（返还积分操作）
        /// <summary>
        /// DMS新增工单消费（返还积分操作）
        /// </summary>
        /// <param name="DMSOrderNo"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddMembersConsumeRewardIntegral")]
        [ResponseType(typeof(ResMembersConsume))]
        //[IOVAuthorize]
        public IHttpActionResult AddMembersConsumeRewardIntegral(string DMSOrderNo)
        {
            Stream s = System.Web.HttpContext.Current.Request.InputStream;
            byte[] b = new byte[s.Length];
            s.Read(b, 0, (int)s.Length);
            var postStr = Encoding.UTF8.GetString(b);
            BLMS.Common.LogService.Instance.Info("新DMS新增工单消费（返还积分操作）接口请求内容:" + postStr);
         
            ReturnResult result = new ReturnResult() { IsSuccess = true, Errors = "", Message = "", Data = new { } };
            try
            {
                if (string.IsNullOrEmpty(DMSOrderNo))
                {
                    result.IsSuccess = false;
                    result.Message = "工单号不能为空 ";
                    return Ok(result);
                }
                ConsumeEntity entity = new ConsumeEntity();
                var resMembersConsume = new ResMembersConsume();
                
                var consumeInfo = _AppContext.ConsumeApp.GetUserConsumeByDmsNo(DMSOrderNo);
                if (consumeInfo == null)
                {
                    result.IsSuccess = false;
                    result.Message = "未找到工单信息 ";
                    return Ok(result);
                }
                int UserIntegral = _AppContext.UserIntegralApp.GetTotalIntegral(consumeInfo.UserId);
                int oldUserIntegral = UserIntegral + consumeInfo.ConsumePoints;
                //经销商ID/Name
                entity.DealerId = consumeInfo.DealerId;
                var dealer = _AppContext.DealerApp.GetDealerByDealerId(consumeInfo.DealerId);
                if (dealer == null)
                {
                    result.IsSuccess = false;
                    result.Message = "未找到经销商信息 ";
                    return Ok(result);
                }
                entity.DealerName = dealer.Name;
                //直接发放积分
                if (_AppContext.DealerMembershipApp.IsPersonalUser(consumeInfo.IdentityNumber))
                {
                    //购车不返积分
                    if (Convert.ToInt32(consumeInfo.ConsumeType) != (int)EConsumeType.Purchase)
                    {
                        _AppContext.ConsumeApp.AddAndProcessReward(consumeInfo.UserId, Convert.ToInt32(consumeInfo.ID));
                    }
                }
                int newUserIntegral = _AppContext.UserIntegralApp.GetTotalIntegral(consumeInfo.UserId);
                decimal pointSeq = 0;
                if (consumeInfo.ConsumePoints > 0)
                {
                    //根据用户等级，返积分，不同等级分别返（普卡：0.1；银卡：0.15；金卡：0.2；生日月：0.5）
                    var discount = _AppContext.UserIntegralApp.GetUserIntegralDiscount(consumeInfo.UserId);
                    //计算返还积分
                    pointSeq = (Math.Round(Convert.ToDecimal(consumeInfo.TotalCost) * Convert.ToDecimal(discount)));//.ToString()
                    UserMessageRecord userMessageRecord = new UserMessageRecord();
                    userMessageRecord.UserId = consumeInfo.UserId;
                    userMessageRecord.MsgType = MessageType.IntegralConsum;
                    userMessageRecord.MsgContent = string.Format("您好，您于{0}新增维保返还{1}积分，感谢您的支持，祝您生活愉快。",
                        DateTime.Now, pointSeq);
                    _AppContext.UserMessageRecordApp.Insert(userMessageRecord);//----------------------------------------------------------------------------------
                    _AppContext.SMSApp.SendSMS(ESmsType.消费管理_积分消耗成功, consumeInfo.Phone,
                        new string[]
                        {
                            DateTime.Now.Year.ToString(),
                            DateTime.Now.Month.ToString(),
                            DateTime.Now.Day.ToString(),
                            entity.DealerId + "-" + entity.DealerName,
                            Vcyber.BLMS.Common.EnumExtension.GetDiscribe<EConsumeType>(entity.EConsumeType),
                            oldUserIntegral.ToString(),
                            consumeInfo.ConsumePoints.ToString(),
                            ((int)consumeInfo.ConsumePoints*0.1).ToString(),
                            consumeInfo.TotalCost.ToString(),
                            pointSeq.ToString(),
                            newUserIntegral.ToString()
                        });
                }

                resMembersConsume.Point = _AppContext.UserIntegralApp.GetTotalIntegral(consumeInfo.UserId).ToString();
                resMembersConsume.PointSeq = pointSeq.ToString();
                //resMembersConsume.OrderNo = _AppContext.ConsumeApp.GetOrderNoById(id);
                result.Data = resMembersConsume;
                result.IsSuccess = true;
                result.Message = "SUCCESS";
                return Ok(result);
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = "ERROR";
                return Ok(result);
            }
        }
        #endregion
    }
}
