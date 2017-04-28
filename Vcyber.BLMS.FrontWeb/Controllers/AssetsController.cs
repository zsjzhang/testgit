using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vcyber.BLMS.Application;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.FrontWeb.Controllers
{
    /// <summary>
    /// 财富（积分、蓝豆、卡券）
    /// </summary>
    public class AssetsController : Controller
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

        public AssetsController()
        {
        }

        public AssetsController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (!this.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("LogonPage", "Account", new { returnUrl = "/MyCenter/Index" });
            }
            int _outScoreCount = 0;
            int _outVirtualCurrencyCount = 0;

            int _totalScore = 0;
            int _totalBlueBean = 0;
            int _totalEmpiric=0;
            if (this.User.Identity.IsAuthenticated)
            {
                _totalScore = _AppContext.UserIntegralApp.GetTotalIntegral(this.User.Identity.GetUserId());
                _totalBlueBean = _AppContext.UserBlueBeanApp.GetTotalBlueBean(this.User.Identity.GetUserId());
                _totalEmpiric = _AppContext.UserEmpiricApp.TotalValue(this.User.Identity.GetUserId());
            }
            ViewBag.totalScore = _totalScore;
            ViewBag.totalBlueBean = _totalBlueBean;
            ViewBag.totalEmpiric=_totalEmpiric;

            ViewData["myscore"] = _AppContext.UserIntegralApp.GetAll(this.User.Identity.GetUserId(), new PageData() { Index = 1, Size = 5 }, out _outScoreCount);
            ViewData["myvirtualcurrency"] = _AppContext.UserBlueBeanApp.GetAll(User.Identity.GetUserId(), new PageData() { Index = 1, Size = 5 }, out _outVirtualCurrencyCount);
            ViewData["myexperience"] = null;
            return View();
        }

        /// <summary>
        /// 我的积分列表
        /// </summary>
        /// <returns></returns>
        public ActionResult MyScore()
        {
            return View();
        }

        /// <summary>
        /// 我的蓝豆
        /// </summary>
        /// <returns></returns>
        public ActionResult MyVirtualCurrency()
        {
            return View();
        }

        /// <summary>
        /// 我的经验值
        /// </summary>
        /// <returns></returns>
        public ActionResult MyExperience()
        {
            return View();
        }

        /// <summary>
        /// 我的钱包
        /// </summary>
        /// <returns></returns>
        public ActionResult AssetsPages()
        {
            ViewData["myAssets"] = null;
            return View();
        }
    }
}