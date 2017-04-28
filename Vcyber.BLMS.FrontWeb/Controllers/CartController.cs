using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vcyber.BLMS.Application;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Vcyber.BLMS.FrontWeb.Models;

namespace Vcyber.BLMS.FrontWeb.Controllers
{
    /// <summary>
    /// 购物车
    /// </summary>
    public class CartController : Controller
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

        public CartController()
        {
        }

        public CartController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        #endregion

        //
        // GET: /Cart/
        public ActionResult Index()
        {
            int _surplusScore = 0;
            int _surplusBluedou = 0;
            bool _userStatus = false;
            if (this.User.Identity.IsAuthenticated)
            {
                _userStatus = true;
                _surplusScore = Vcyber.BLMS.Application._AppContext.UserIntegralApp.GetTotalIntegral(this.User.Identity.GetUserId());
                _surplusBluedou = Vcyber.BLMS.Application._AppContext.UserBlueBeanApp.GetTotalBlueBean(this.User.Identity.GetUserId());
            }
            ViewBag.UserStatus = _userStatus;
            ViewBag.surplusScore = _surplusScore;
            ViewBag.surplusBluedou = _surplusBluedou;
            return View();
        }

        /// <summary>
        /// 商城购物车
        /// </summary>
        /// <returns></returns>
        public ActionResult CartByMall()
        {
            return View();
        }
    }
}