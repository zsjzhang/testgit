using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AspNet.Identity.SQL;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace Vcyber.BLMS.FrontWeb.Controllers
{
    /// <summary>
    /// 活动
    /// </summary>
    public class MavementController : Controller
    {
        #region ==== 私有字段 ====

        private int PAGESIZE = 10;

        private ApplicationUserManager _userManager;

        #endregion
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

        /// <summary>
        /// 活动中心首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 全部活动
        /// </summary>
        /// <returns></returns>
        public ActionResult All(int? pageIndex)
        {
            int _totalCount = 0;
            IEnumerable<Activities> _resultl = _AppContext.ActivitiesApp.Select(string.Empty, null, pageIndex ?? 0, PAGESIZE, out _totalCount);
            return View(_resultl);
        }

        /// <summary>
        /// 进行中的活动
        /// </summary>
        /// <returns></returns>
        public ActionResult Doing(int? pageIndex)
        {
            int _totalCount = 0;
            IEnumerable<Activities> _resultl = _AppContext.ActivitiesApp.Select(string.Empty, (int)EActivitiescsStatus.InProcess, pageIndex ?? 0, PAGESIZE, out _totalCount);
            return View(_resultl);
        }


        /// <summary>
        /// 进行中的活动
        /// </summary>
        /// <returns></returns>
        public ActionResult Past(int? pageIndex)
        {
            int _totalCount = 0;
            IEnumerable<Activities> _resultl = _AppContext.ActivitiesApp.Select(string.Empty, (int)EActivitiescsStatus.Finished, pageIndex ?? 0, PAGESIZE, out _totalCount);
            return View(_resultl);
        }

        /// <summary>
        /// 活动详细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Detail(int id)
        {
            //设置用户账号和活动Id
            string _userid = string.Empty;
            if (this.User.Identity.IsAuthenticated)
            {
                _userid = this.User.Identity.GetUserId();
            }
            ViewBag.userId = _userid;
            ViewBag.movementId = id;

            //根据用户账号与活动Id获取报名的详情，如果已经报名则报名按钮显示已报名
            int _totalCount = 0;
            int _movementStatus = 0;
            IEnumerable<ActivitiesSignUp> _signUpList = _AppContext.ActivitiesSignUpApp.GetSignUpListByUserId(_userid, 0, 1, out  _totalCount);
            if (_signUpList != null && _signUpList.Any())
            {
                _movementStatus = 1;
            }
            Activities _result = _AppContext.ActivitiesApp.GetActivitiesById(id);
            if (_result.EndTime != null && _result.EndTime <= DateTime.Now)
            {
                _movementStatus = 2;
            }
            //判断活动状态
            ViewBag.movementStatus = _movementStatus;
            return View(_result);
        }

        /// <summary>
        /// 活动报名
        /// </summary>
        /// <param name="activitiesEntity"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ApplyMovement(ActivitiesSignUp activitiesEntity)
        {
            string userName = string.Empty;

            if (this.User.Identity.IsAuthenticated && ModelState.IsValid)
            {
               var user = UserManager.FindByNameAsync(this.User.Identity.Name).Result;
          
                var act = _AppContext.ActivitiesApp.GetActivitiesById(activitiesEntity.ActivitiesId);
                var signUp = _AppContext.ActivitiesSignUpApp.GetItemByUserIdAndActivitiesId(user.Id, act.Id);
                bool hasSignUp = (signUp!=null);

                if (hasSignUp)
                {
                    return Json(new { code = "400", msg = "报名失败" });
                }
                //是否支持在线报名（只有在线报名才能参与报名）
                if (act.SignUp == 0)
                {
                    return Json(new { code = "400", msg = "报名失败" });
                }

                //只有车主才能参加
                if (act.IsCarOwner == 1 && user.SystemMType == (int)MembershipType.WhitoutCar )
                {
                    return Json(new { code = "400", msg = "报名失败" });
                }

                activitiesEntity.UserName = User.Identity.GetUserName();
                activitiesEntity.UserId = User.Identity.GetUserId();
                activitiesEntity.CreateTime = DateTime.Now;
                //报名成功
                var _result = _AppContext.ActivitiesSignUpApp.SignUpActivities(activitiesEntity);
                if (_result > 0)
                {
                    return Json(new { code = "200", msg = "报名成功" });
                }
            }

            return Json(new { code = "400", msg = "报名失败" });
        }
    }
}