namespace Vcyber.BLMS.ManageWeb.Controllers
{
    using System;
    using System.Web;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.Owin;

    using PetaPoco;
    using System.Web.Mvc;
    using Vcyber.BLMS.Application;
    using Vcyber.BLMS.Entity.CarService;
    using Vcyber.BLMS.Entity.Enum;
    using Vcyber.BLMS.Entity.Generated;
    using Vcyber.BLMS.ManageWeb.Helper;

    [MvcAuthorize]
    public class ScheduleMaintController : Controller
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
        //    Page<CSMaintenance> list = _AppContext.ScheduleMaintApp.QueryOrders(entity, page, itemsPerPage);
        //    ViewData.Add("data", list);
        //    return this.View(entity);
        //}

        //public ActionResult Update(int id, QueryParamEntity entity, long page, long itemsPerPage)
        //{
        //    _AppContext.ScheduleMaintApp.UpdateState(id, EOrderState.Processed, HttpContext.User.Identity.GetUserId(), HttpContext.User.Identity.GetUserName());
        //    return RedirectToAction("QueryOrders", new { entity, page, itemsPerPage });
        //}

        public ActionResult QueryOrders(string phone, string orderNo, string carSeries, string licensePlate, string state, long page = 1, long itemsPerPage = 10)
        {
            if (string.IsNullOrEmpty(state)) state = EOrderState.All.ToString();
            EOrderState oState;
            if (!Enum.TryParse(state, out oState)) oState = EOrderState.All;
            QueryParamEntity entity = new QueryParamEntity() { Phone = phone, CarSeries = carSeries, OrderNo = orderNo, LicensePlate = licensePlate, State = oState };

            entity.DealerId = UserManager.FindById(User.Identity.GetUserId()).DealerId;

            Page<CSSonataService> list = _AppContext.SonataServiceApp.QueryOrders(entity, page, itemsPerPage);
            ViewData.Add("data", list);
            return this.View(entity);

        }

        //public ActionResult Update(int id, string phone, string orderNo, string carSeries, string licensePlate, string state, long page = 1, long itemsPerPage = 20)
        //{
        //    if (string.IsNullOrEmpty(state)) state = EOrderState.All.ToString();
        //    EOrderState oState;
        //    if (!Enum.TryParse(state, out oState)) oState = EOrderState.All;
        //    _AppContext.ScheduleMaintApp.UpdateState(id, EOrderState.Processed, HttpContext.User.Identity.GetUserId(), HttpContext.User.Identity.GetUserName());
        //    return RedirectToAction("QueryOrders", new { phone, orderNo, carSeries, licensePlate, state, page, itemsPerPage });
        //}
        public ActionResult Update(int id, string reservationType, string updatename, string state, long page = 1, long itemsPerPage = 20)
        {
            EOrderState oState;
            EOrderType oType;
            Enum.TryParse(reservationType, out oType);
            if (!Enum.TryParse(state, out oState)) oState = EOrderState.All;
            _AppContext.SonataServiceApp.UpdateState(id, EOrderState.Processed, HttpContext.User.Identity.GetUserId(), HttpContext.User.Identity.GetUserName());
            return RedirectToAction("List", "Reservation", new { ReservationType = oType, UpdateName = updatename, State = (int)oState, page, itemsPerPage });
        }
    }
}
