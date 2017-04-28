using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AspNet.Identity.SQL;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using Vcyber.BLMS.FrontWeb.Models;

namespace Vcyber.BLMS.FrontWeb.Controllers
{
    public class PayFeeController : Controller
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

        public PayFeeController()
        {
        }

        public PayFeeController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        #endregion
        //
        // GET: /PayFee/
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 支付码激活页面
        /// </summary>
        /// <returns></returns>
        public ActionResult PayNumberActive()
        {
            return View();
        }

        /// <summary>
        /// 支付码激活页面
        /// </summary>
        /// <returns></returns>
        public ActionResult AccountPayNumberActive()
        {
            return View();
        }

        public ActionResult LoginPayNumberActive()
        {
            return View();
        }
        /// <summary>
        /// 支付会费页面
        /// </summary>
        /// <returns></returns>
        public ActionResult PayWay()
        {
            return View();
        }

        /// <summary>
        /// 支付结果页面
        /// </summary>
        /// <returns></returns>
        public ActionResult PayResult()
        {
            return View();
        }

        /// <summary>
        /// 支付成功
        /// </summary>
        /// <returns></returns>
        public ActionResult PaySuccess()
        {
            return View();
        }

        /// <summary>
        /// 支付失败
        /// </summary>
        /// <returns></returns>
        public ActionResult PayFailed()
        {
            return View();
        }

        /// <summary>
        /// 向特约店申请，成为会员
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ActivityApply()
        {
            if (!this.User.Identity.IsAuthenticated)
            {
                return Json(new { code = "401", msg = "失败" });
            }

            HttpCookie _activedealerIdcookie = Request.Cookies.Get("activememberdealerId");
            if (_activedealerIdcookie == null || string.IsNullOrEmpty(_activedealerIdcookie.Value))
            {
                return Json(new { code = "401", msg = "请选择供应商" });
            }
            string _activedealerId = HttpContext.Server.UrlDecode(_activedealerIdcookie.Value);

            string memo = string.Empty;
            var userStore = new FrontUserStore<FrontIdentityUser>();
            var user = UserManager.FindById(this.User.Identity.GetUserId());
            user.IsPay = 2;
            userStore.UpdateAsync(user);
            userStore.AddMembershipDealerRecord(user.Id, _activedealerId);
            var result = userStore.CreateMembershipRequest(user.Id, user.IdentityNumber, _activedealerId, memo,"blms");
            if (result)
            {
                return Json(new { code = "200", msg = "申请成功" });
            }
            return Json(new { code = "400", msg = "提交失败" });
        }

        /// <summary>
        /// 支付码激活
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult InputPayNumberToActiveSave(string payNumber)
        {
            if (this.User.Identity.IsAuthenticated)
            {
                //获取经销商ID
                HttpCookie _activedealerIdcookie = Request.Cookies.Get("activememberdealerId");
                if (_activedealerIdcookie == null || string.IsNullOrEmpty(_activedealerIdcookie.Value))
                {
                    return Json(new { code = "402", msg = "请选择供应商" });
                }
                string _activedealerId = HttpContext.Server.UrlDecode(_activedealerIdcookie.Value);

                //设置激活码
                ApplicationUser _payUserEntity = UserManager.FindById(this.User.Identity.GetUserId());
                _payUserEntity.PayNumber = payNumber;
                _payUserEntity.IsPay = 2;
                var _result = UserManager.Update(_payUserEntity);

                //向后台申请激活成为会员
                var userStore = new FrontUserStore<FrontIdentityUser>();
                userStore.AddMembershipDealerRecord(this.User.Identity.GetUserId(), _activedealerId);
                var result = userStore.CreateMembershipRequest(this.User.Identity.GetUserId(), _payUserEntity.IdentityNumber, _activedealerId, string.Empty,"blms");
                if (result)
                {
                    return Json(new { code = "200", msg = "保存成功" });
                }
                else
                {
                    return Json(new { code = "400", msg = "保存失败，请重新输入" });
                }
            }
            return Json(new { code = "401", msg = "账号登录异常，请重新登录" });
        }

    }
}