
namespace Vcyber.BLMS.ManageWeb.Controllers
{
    using System;
    using System.Web;
    using System.Web.Routing;
    using System.Web.UI.WebControls;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.Owin;

    using PetaPoco;
    using System.Web.Mvc;
    using System.Web.UI;
    using Vcyber.BLMS.Application;
    using Vcyber.BLMS.Entity.CarService;
    using Vcyber.BLMS.Entity.Enum;
    using Vcyber.BLMS.Entity.Generated;
    using Vcyber.BLMS.ManageWeb.Helper;

    [MvcAuthorize]
    public class SonataServiceController : Controller
    {
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
        //public ActionResult QueryOrders(QueryParamEntity entity, long page = 1, long itemsPerPage = 20)
        //{
        //    Page<CSSonataService> list = _AppContext.SonataServiceApp.QueryOrders(entity, page, itemsPerPage);
        //    ViewData.Add("data", list);
        //    return this.View(entity);
        //}

        //public ActionResult Update(int id, QueryParamEntity entity, long page, long itemsPerPage)
        //{
        //    _AppContext.SonataServiceApp.UpdateState(id, EOrderState.Processed, HttpContext.User.Identity.GetUserId(), HttpContext.User.Identity.GetUserName());
        //    return RedirectToAction("QueryOrders", new { entity, page, itemsPerPage });
        //}

        public ActionResult QueryOrders(string phone, string orderNo, string orderType, string licensePlate, string state, long page = 1, long itemsPerPage = 50)
        {
            if (string.IsNullOrEmpty(state)) state = EOrderState.All.ToString();
            EOrderState oState;
            if (!Enum.TryParse(state, out oState)) oState = EOrderState.All;

            if (string.IsNullOrEmpty(orderType)) state = EOrderType.Care.ToString();
            EOrderType oType;
            if (!Enum.TryParse(orderType, out oType)) oType = EOrderType.Care;

            QueryParamEntity entity = new QueryParamEntity() { Phone = phone, OrderType = oType, OrderNo = orderNo, LicensePlate = licensePlate, State = oState };

            entity.DealerId = UserManager.FindById(User.Identity.GetUserId()).DealerId;
            
            Page<CSSonataService> list = _AppContext.SonataServiceApp.QueryOrders(entity, page, itemsPerPage);
            ViewData.Add("data", list);
            return this.View(entity);

        }

        //public ActionResult Update(int id, string phone, string orderNo, string orderType, string licensePlate, string state, long page = 1, long itemsPerPage = 10)
        //{
        //    if (string.IsNullOrEmpty(state)) state = EOrderState.All.ToString();
        //    //EOrderState oState;
        //    //if (!Enum.TryParse(state, out oState)) oState = EOrderState.All;

        //    if (string.IsNullOrEmpty(orderType)) state = EOrderType._100dayCare.ToString();
        //    //EOrderType oType;
        //    //if (!Enum.TryParse(orderType, out oType)) oType = EOrderType._100dayCare;

        //    _AppContext.SonataServiceApp.UpdateState(id, EOrderState.Processed, HttpContext.User.Identity.GetUserId(), HttpContext.User.Identity.GetUserName());
        //    return RedirectToAction("QueryOrders", new { phone, orderNo, orderType, licensePlate, state, page, itemsPerPage });
        //}
        public ActionResult Update(int id, string reservationType, string updatename, string state, long page = 1, long itemsPerPage = 10)
        {
            EOrderState oState;
            EOrderType oType;
            Enum.TryParse(reservationType, out oType);
            if (!Enum.TryParse(state, out oState)) oState = EOrderState.All;
            _AppContext.SonataServiceApp.UpdateState(id, EOrderState.Processed, HttpContext.User.Identity.GetUserId(), HttpContext.User.Identity.GetUserName());
            return RedirectToAction("List", "Reservation", new { ReservationType = oType, UpdateName = updatename, State = (int)oState, page, itemsPerPage });
        }

        /// <summary>
        /// 新增订车订单
        /// </summary>
        /// <param name="entity">订单内容</param>
        /// <returns>订单Id</returns>
        [HttpPost]
        public ActionResult Add(SonataServiceEntity entity)
        {
            if (!ModelState.IsValid)
            {
                return this.View("Add", entity);
            }
            int retval = _AppContext.SonataServiceApp.Add(entity, HttpContext.User.Identity.GetUserId(), HttpContext.User.Identity.GetUserName());
            this.ViewData.Add("message", retval > 0 ? "提交成功" : "提交失败");
            return RedirectToAction("Add");

        }
        [HttpGet]
        public ActionResult Add()
        {
            return this.View();
        }

        public ActionResult GetProvinceList()
        {
            return Json(_AppContext.DealerApp.GetProvinceList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCityList(string province)
        {
            return Json(_AppContext.DealerApp.GetCityListByProvince(province), JsonRequestBehavior.AllowGet);
        }

        private ActionResult GetDealerList(string province, string city)
        {
            return Json(_AppContext.DealerApp.GetDealerList(province, city), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetDealerList(string province1, string city1, double long1, double lat1, string province2, string city2, double long2, double lat2,int distance)
        {
            return Json(
                _AppContext.DealerApp.GetDealerListByDistance(province1, city1, long1, lat1, province2, city2, long2, lat2, distance),
                JsonRequestBehavior.AllowGet);
        }


    }
}
