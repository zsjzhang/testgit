using AspNet.Identity.SQL;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.Entity.Enum;
using VcyBer.BLMS.MobileWeb.Models;

namespace VcyBer.BLMS.MobileWeb.Controllers
{
    public class UserController : BaseController
    {
        #region ==== 私有字段 ====

        private ApplicationUserManager _userManager;

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

        #endregion

        /// <summary>
        /// 编辑用户信息页面
        /// </summary>
        /// <returns></returns>
        public ActionResult EditUserInfo()
        {
            var cookieValue = CookieHelper.GetCookieValue("CustomCookie");
            if (!string.IsNullOrWhiteSpace(cookieValue) && cookieValue.ToLower().Contains("webinspect"))
            {
                return Redirect("/Contents/error.htm");
            }

            //判断是否登录
            if (!this.User.Identity.IsAuthenticated)
            {
                return Redirect("/Account/Login?url=" + Request.RawUrl);
            }

            //获取当前登录人Id
            var userId = this.User.Identity.GetUserId();

            //根据当前登录用户id获取当前用户信息
            var model = _AppContext.DealerMembershipApp.GetMembershipMsgByUserId(userId);

            return View(model);
        }

        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="userEntity"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SetBaseInfoSave(ApplicationUser userEntity)
        {
            if (!this.User.Identity.IsAuthenticated)
            {
                return Json(new
                {
                    code = "401",
                    msg = "账号登陆异常"
                });
            }

            //验证用户名的唯一性
            var user = new FrontUserStore<FrontIdentityUser>().FindByNickNameAsync(userEntity.NickName);
            if (user != null && user.Result != null)
            {
                if (this.User.Identity.GetUserId() != user.Result.Id)
                {
                    return Json(new { code = "401", msg = "用户名已存在,请更换" });
                }
            }
            ApplicationUser _curUser = UserManager.FindById(this.User.Identity.GetUserId());
            if (userEntity.IdentityNumber != null)
            {
                _curUser.IdentityNumber = userEntity.IdentityNumber;
            }
            //个人信息
            _curUser.NickName = userEntity.NickName;
            _curUser.Gender = userEntity.Gender;
            _curUser.Birthday = userEntity.Birthday;
            _curUser.Address = userEntity.Address;
          
            //_curUser.RealName = userEntity.RealName;
            //_curUser.NickName = userEntity.NickName;
            //_curUser.Birthday = userEntity.Birthday;
            //_curUser.Email = userEntity.Email;
            //_curUser.Address = userEntity.Address;
            //_curUser.FaceImage = userEntity.FaceImage;
            //_curUser.Provency = userEntity.Provency;
            //_curUser.City = userEntity.City;
            //_curUser.Area = userEntity.Area;
            //_curUser.Gender = userEntity.Gender;
            //_curUser.IdentityNumber = userEntity.IdentityNumber;

            UserManager.Update(_curUser);

            //详细信息
            _curUser.PaperWork = userEntity.PaperWork;
            _curUser.MainContact = userEntity.MainContact;
            _curUser.MainTelePhone = userEntity.MainTelePhone;
            _curUser.TelePhone = userEntity.TelePhone;
            _curUser.TransactionTime = userEntity.TransactionTime;
            _curUser.Industry = userEntity.Industry;
            _curUser.Job = userEntity.Job;
            _curUser.IsMarriage = userEntity.IsMarriage;
            _curUser.MarriageDay = userEntity.MarriageDay;
            _curUser.Educational = userEntity.Educational;
            _curUser.SendSms = userEntity.SendSms;
            _curUser.SendLetter = userEntity.SendLetter;
            _curUser.MakePhone = userEntity.MakePhone;
            _curUser.SendEmail = userEntity.SendEmail;
            _curUser.Remark = userEntity.Remark;

            //var appro = new FrontUserStore<FrontIdentityUser>().Update_Or_Insert_Membership_Schedule(_curUser);

            return Json(new
            {
                code = "200",
                msg = "保存成功"
            });
        }

        /// <summary>
        /// 向特约店申请
        /// </summary>
        /// <param name="dealerId"></param>
        /// <returns></returns>
        [HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public ActionResult UserSelectDealerToPay(string dealerId)
        {
            //判断是否登录
            if (!this.User.Identity.IsAuthenticated)
            {
                return Redirect("/Account/Login?url=" + Request.RawUrl);
            }

            //获取当前登录人Id
            var userId = this.User.Identity.GetUserId();

            if (string.IsNullOrEmpty(dealerId))
                return Json(new { code = "401", msg = "经销商不能为空" });
            var store = new FrontUserStore<FrontIdentityUser>();
            var user = store.FindByIdAsync(userId).Result;
            var returnIntegralType = (int)_AppContext.CarServiceUserApp.GetReIntegralTypeByIdentity(user.IdentityNumber);
            user.IsPay = 2;
            if (returnIntegralType > -1)
            {
                user.Amount = returnIntegralType > 1 ? 50 : 100;
            }
            store.UpdateAsync(user);
            if (store.AddMembershipDealerRecord(userId, dealerId) && store.CreateMembershipRequest(userId, user.IdentityNumber, dealerId, string.Empty, "blms"))
                return Json(new { code = "200", msg = "申请成功" });
            else
                return Json(new { code = "401", msg = "申请失败" });
        }

        /// <summary>
        /// 天猫付款
        /// </summary>
        /// <param name="paynumber"></param>
        /// <param name="dealerId"></param>
        /// <returns></returns>
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public ActionResult PayByTmallRequest(string paynumber, string dealerId)
        {
            //判断是否登录
            if (!this.User.Identity.IsAuthenticated)
            {
                return Redirect("/Account/Login?url=" + Request.RawUrl);
            }

            //获取当前登录人Id
            var userId = this.User.Identity.GetUserId();

            var store = new FrontUserStore<FrontIdentityUser>();
            var user = store.FindByIdAsync(userId).Result;

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
            var result = store.CreateMembershipRequest(userId, user.IdentityNumber, dealerId, string.Empty, "blms_wap");
            if (result)
            {
                return Json(new { code = "200", msg = "申请成功,如长时间未通过,请咨询申请经销商" });
            }
            else
            {
                return Json(new { code = "400", msg = "申请失败，请重新输入" });
            }
        }

        /// <summary>
        /// 绑定车辆
        /// </summary>
        /// <param name="identityNumber"></param>
        /// <param name="PaperWork"></param>
        /// <returns></returns>
        public ActionResult ToCheckCarownerSave(string identityNumber, string PaperWork, string source = "")
        {
            //判断是否登录
            if (!this.User.Identity.IsAuthenticated)
            {
                return Redirect("/Account/Login?url=" + Request.RawUrl);
            }

            //获取当前登录人Id
            var userId = this.User.Identity.GetUserId();

            string appkey = string.Empty;
            var userStore = new FrontUserStore<FrontIdentityUser>();
            var userObj = userStore.FindByIdentityNumber(identityNumber).Result;//membership里查询该身份证号  
            var isPIDExist = userObj != null;
            //如果证件号存在
            if (userObj != null)
            {
                //如果证件号已存在并绑定到车主，不能绑定
                if (userObj.SystemMType > 1)
                {
                    return Json(new { code = 400, msg = "很抱歉，您的身份未绑定成功，请联系bluemembers在线客服" });
                }
            }
            else
            {
                userObj = userStore.FindByIdAsync(userId).Result; //membership里查询该用户ID       
            }
            //获取用户车辆信息
            var carQuery = _AppContext.CarServiceUserApp.CarsByPID(identityNumber);
            if (carQuery != null && carQuery.Count() > 0)//有车
            {
                var memberLevel = _AppContext.DealerMembershipApp.GetLevel(identityNumber);//获取用户级别
                userObj.MLevel = Convert.ToInt32(memberLevel);
                userObj.PaperWork = PaperWork;
                userObj.SystemMType = (int)MembershipType.WhitCar;
                userObj.IdentityNumber = identityNumber;
                userObj.AuthenticationTime = DateTime.Now;
                userObj.AuthenticationSource = "blms_wap";
                //认证来源
                //switch (source)
                //{
                //    case "activity_trip":
                //        userObj.Remark = "source:activity_trip";//机场活动
                //        break;
                //    case "activity_verna":
                //        userObj.Remark = "source:activity_verna";//悦纳上市活动
                //        break;
                //    default:
                //        userObj.Remark = "source:none";//无
                //        break;
                //}
                //提取生日信息、性别、年龄
                int Gender = -1;
                int Age = -1;
                if (userObj.IdentityNumber != null && userObj.PaperWork == "1")
                {
                    if (userObj.IdentityNumber.Length == 15)
                    {
                        var str = userObj.IdentityNumber.Substring(6, 6);
                        userObj.Birthday = "19" + str.Remove(2) + "-" + str.Substring(2, 2) + "-" + str.Substring(4, 2);
                        Gender = (Convert.ToInt32(userObj.IdentityNumber.Substring(14)) % 2 == 0 ? 2 : 1);
                        Age = DateTime.Now.Year - Convert.ToInt32("19" + userObj.IdentityNumber.Substring(6, 2));
                    }
                    if (userObj.IdentityNumber.Length == 18)
                    {
                        var str = userObj.IdentityNumber.Substring(6, 8);
                        userObj.Birthday = str.Remove(4) + "-" + str.Substring(4, 2) + "-" + str.Substring(6, 2);
                        Gender = (Convert.ToInt32(userObj.IdentityNumber.Substring(16, 1)) % 2 == 0 ? 2 : 1);
                        Age = DateTime.Now.Year - Convert.ToInt32(userObj.IdentityNumber.Substring(6, 4));
                    }
                    DateTime birthDay = DateTime.Now;
                    var isBirthday = DateTime.TryParse(userObj.Birthday, out birthDay);
                    if (!isBirthday)
                    {
                        return Json(new { code = 400, msg = "身份证填写错误" });
                    }
                    userObj.Gender = Gender.ToString();
                    userObj.Age = Age;
                }
                //一切正常，绑定用户
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
                //var integralType = (int)_AppContext.CarServiceUserApp.GetReIntegralTypeByIdentity(identityNumber);
                //if (integralType != -1)
                //{
                //    //authResult.Content = integralType;
                //    return Json(new { code = 200, msg = "绑定成功" });
                //}
                return Json(new { code = 200, msg = "绑定成功" });
            }
            else//无车
            {
                return Json(new { code = 400, msg = "很抱歉，您的身份未绑定成功，请联系bluemembers在线客服" });
            }
        }
    }
}