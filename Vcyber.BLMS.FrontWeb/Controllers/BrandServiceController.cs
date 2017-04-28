using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.IRepository;

using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using AspNet.Identity.SQL;

namespace Vcyber.BLMS.FrontWeb.Controllers
{
    public class BrandServiceController : Controller
    {
        //
        // GET: /BrandService/
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetBrandServiceCode(string brandName)
        {
            ReturnResult result = new ReturnResult { IsSuccess = false };

            if (!this.User.Identity.IsAuthenticated)
            {
                result.Message = "请先登录";
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            var frontUserStore = new FrontUserStore<FrontIdentityUser>();
            var membership = frontUserStore.FindByIdAsync(this.User.Identity.GetUserId());

            result = _AppContext.BrandServiceApp.GetBrandServiceCode(this.User.Identity.GetUserId(), membership.Result.UserName, brandName);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetMembershipBrand()
        {
            ReturnResult result = new ReturnResult { IsSuccess = true };

            if (!this.User.Identity.IsAuthenticated)
            {
                result.IsSuccess = false;
                result.Message = "请先登录";
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            var frontUserStore = new FrontUserStore<FrontIdentityUser>();
            var membership = frontUserStore.FindByIdAsync(this.User.Identity.GetUserId());

            result.Data = _AppContext.BrandServiceApp.SelectMembershipBrandByUserId(this.User.Identity.GetUserId()).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}