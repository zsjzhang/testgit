using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vcyber.BLMS.Application;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Common.City;

namespace Vcyber.BLMS.FrontWeb.Controllers
{
    public class UserController : Controller
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

        public UserController()
        {
        }

        public UserController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        #endregion

        //
        // GET: /User/
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 我的收货地址
        /// </summary>
        /// <returns></returns>
        public ActionResult MyAddress(string userId)
        {
            IEnumerable<Address> _result = null;
            if (this.User.Identity.IsAuthenticated && !string.IsNullOrEmpty(userId))
            {
                _result = _AppContext.AddressApp.GetList(userId);
            }

            ViewData["provinceList"] = CityService.Instance.GetProvince();

            ViewBag.userId = userId;
            return View(_result);
        }

        /// <summary>
        /// 根据省份获取城市
        /// </summary>
        /// <param name="provinceCode"></param>
        /// <returns></returns>
        public ActionResult FindCitysByProvince(string provinceCode)
        {
            List<City> _cityList = CityService.Instance.GetCity(provinceCode);
            return View(_cityList);
        }

        /// <summary>
        /// 根据城市获取地区
        /// </summary>
        /// <param name="cityCode"></param>
        /// <returns></returns>
        public ActionResult FindAreasByCity(string cityCode)
        {
            IList<Area> _areaList = CityService.Instance.GetArea(cityCode);
            return View(_areaList);
        }


        /// <summary>
        /// 收货地址列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ActionResult AddressList(string userId)
        {
            IEnumerable<Address> _result = null;
            if (this.User.Identity.IsAuthenticated && !string.IsNullOrEmpty(userId))
            {
                _result = _AppContext.AddressApp.GetList(userId);
            }
            ViewBag.userId = userId;
            return View(_result);
        }

        /// <summary>
        /// 添加地址
        /// </summary>
        /// <returns></returns>
        public JsonResult AddAddress(Address addressEntity)
        {

            try
            {
                //修改地址信息
                if (addressEntity.ID != null && addressEntity.ID != 0)
                {
                    addressEntity.UpdateTime = DateTime.Now;
                    _AppContext.AddressApp.Update(addressEntity);
                }
                //添加地址信息
                else
                {
                    addressEntity.CreateTime = DateTime.Now;
                    addressEntity.UpdateTime = DateTime.Now;
                    _AppContext.AddressApp.Add(addressEntity);
                }
                return Json(new { code = 200, msg = "保存成功" });
            }
            catch (Exception ex)
            {
                return Json(new { code = 400, msg = "网络异常", innerTrace = ex.Message });
            }
        }

        /// <summary>
        /// 修改地址
        /// </summary>
        /// <returns></returns>
        public JsonResult UpdateAddress(Address addressEntity)
        {

            try
            {
                addressEntity.UpdateTime = DateTime.Now;
                _AppContext.AddressApp.Update(addressEntity);
                return Json(new { code = 200, msg = "保存成功" });
            }
            catch (Exception ex)
            {
                return Json(new { code = 400, msg = "网络异常", innerTrace = ex.Message });
            }
        }

        /// <summary>
        /// 删除地址
        /// </summary>
        /// <returns></returns>
        public JsonResult DelAddress(int id)
        {

            try
            {
                _AppContext.AddressApp.Delete(id);
                return Json(new { code = 200, msg = "删除成功" });
            }
            catch (Exception ex)
            {
                return Json(new { code = 400, msg = "网络异常", innerTrace = ex.Message });
            }
        }
    }
}