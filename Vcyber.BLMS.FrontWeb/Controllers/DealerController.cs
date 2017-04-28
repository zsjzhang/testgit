using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Vcyber.BLMS.FrontWeb.Controllers
{
    using AspNet.Identity.SQL;
    using Common;
    using Microsoft.AspNet.Identity;

    using Vcyber.BLMS.Application;
    using Vcyber.BLMS.Entity.Enum;
    using Vcyber.BLMS.Entity.Generated;

    public class DealerController : Controller
    {
        //
        // GET: /Dealer/
        public ActionResult Index()
        {
            var cookieValue = CookieHelper.GetCookieValue("CustomCookie");
            if (!string.IsNullOrWhiteSpace(cookieValue) && cookieValue.ToLower().Contains("webinspect"))
            {
                return Redirect("/Contents/error.htm");
            }

            return View();
        }

        /// <summary>
        /// 供应商下拉框
        /// </summary>
        /// <returns></returns>
        public ActionResult ProvinceCity()
        {
            IEnumerable<string> _provinces = _AppContext.DealerApp.GetProvinceList();
            return View(_provinces);
        }

        /// <summary>
        /// 根据省获取省下的市
        /// </summary>
        /// <param name="provinceId"></param>
        /// <returns></returns>
        public JsonResult Citys(string provinceValue)
        {
            IList<string> _result = new List<string>();
            IEnumerable<string> _citys = _AppContext.DealerApp.GetCityListByProvince(provinceValue);
            if (_citys != null && _citys.Any())
            {
                _result = _citys.ToList();
            }
            return Json(_result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 根据城市获取供应商
        /// </summary>
        /// <param name="cityValue"></param>
        /// <returns></returns>
        public JsonResult Dealers(string cityValue, string provinceValue, int IsWeibao, int Istestserver, int IsDingChe)
        {
            IList<CSCarDealerShip> _result = new List<CSCarDealerShip>();
           // IEnumerable<CSCarDealerShip> _dealers = _AppContext.DealerApp.GetDealerList(provinceValue, cityValue);
            IEnumerable<CSCarDealerShip> _dealers = _AppContext.DealerApp.GetDealerShipList(provinceValue, cityValue, IsWeibao, Istestserver, IsDingChe);
            if (_dealers != null && _dealers.Any())
            {
                _result = _dealers.ToList();
            }


            if (this.User.Identity.GetUserId() != null)
            {
                int outValue;
                var account =
                    new FrontUserStore<FrontIdentityUser>().FindByIdAsync(this.User.Identity.GetUserId()).Result;
                if(account!=null)
                //_AppContext.BreadApp.BlueBeanBread(
                //    EBRuleType.经销商查询,
                //    account.Id,
                //    (MemshipLevel)account.MLevel,
                //    out outValue);
                _AppContext.BreadApp.EmpiricBread(EEmpiricRule.经销商查询, account.Id, out outValue);
            }

            return Json(_result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 根据城市获取供应商
        /// </summary>
        /// <param name="dealerId"></param>
        /// <returns></returns>
        public JsonResult GetDealer(string dealerId)
        {
            CSCarDealerShip _dealer = _AppContext.DealerApp.GetDealerByDealerId(dealerId); 

            return Json(_dealer, JsonRequestBehavior.AllowGet);
        }
	}
}