
namespace Vcyber.BLMS.ManageWeb.Controllers
{
    using System;
    using System.Web;
    using System.Web.Routing;
    using System.Web.UI.WebControls;

    using PetaPoco;
    using System.Web.Mvc;
    using System.Web.UI;
    using Vcyber.BLMS.Application;
    using Vcyber.BLMS.Entity.CarService;
    using Vcyber.BLMS.Entity.Enum;
    using Vcyber.BLMS.Entity.Generated;
    using Vcyber.BLMS.ManageWeb.Helper;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.Owin;

    [MvcAuthorize]
    public class OrderCarController : Controller
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

        /// <summary>
        /// 根据用户Id查询订车订单
        /// </summary>
        /// <param name="id">User Id</param>
        /// <param name="page">当前页数</param>
        /// <param name="itemsPerPage">每页数据条数</param>
        /// <returns></returns>
        public ActionResult QueryOrders1(QueryParamEntity entity, long page = 1, long itemsPerPage = 10)
        {
            Page<CSOrderCar> list = _AppContext.OrderCarApp.QueryOrders(entity, page, itemsPerPage);
            ViewData.Add("data", list);
            return this.View(entity);
        }

        public ActionResult QueryOrders(string phone, string orderNo, string carSeries, string licensePlate, string state, long page = 1, long itemsPerPage = 10)
        {
            if (string.IsNullOrEmpty(state)) state = EOrderState.All.ToString();
            EOrderState oState;
            if (!Enum.TryParse(state, out oState)) oState = EOrderState.All;
            QueryParamEntity entity = new QueryParamEntity(){Phone = phone, CarSeries = carSeries,OrderNo = orderNo, LicensePlate = licensePlate, State = oState};

            entity.DealerId = this.UserManager.FindById(User.Identity.GetUserId()).DealerId;

            Page<CSOrderCar> list = _AppContext.OrderCarApp.QueryOrders(entity, page, itemsPerPage);
            ViewData.Add("data", list);
            return this.View(entity);
            
        }

        //public ActionResult Update(int id, string phone, string orderNo, string carSeries, string licensePlate, string state, long page = 1, long itemsPerPage = 50)
        //{
        //    if (string.IsNullOrEmpty(state)) state = EOrderState.All.ToString();
        //    EOrderState oState;
        //    if (!Enum.TryParse(state, out oState)) oState = EOrderState.All;
        //    _AppContext.OrderCarApp.UpdateState(id, EOrderState.Processed, HttpContext.User.Identity.GetUserId(), HttpContext.User.Identity.GetUserName());
        //    return RedirectToAction("QueryOrders", new { phone, orderNo, carSeries, licensePlate, state, page, itemsPerPage });
        //}
        public ActionResult Update(int id, string reservationType, string updatename, string state, long page = 1, long itemsPerPage = 50)
        {
            EOrderState oState;
            EOrderType oType;
            Enum.TryParse(reservationType, out oType);
            if (!Enum.TryParse(state, out oState)) oState = EOrderState.All;
            _AppContext.OrderCarApp.UpdateState(id, EOrderState.Processed, HttpContext.User.Identity.GetUserId(), HttpContext.User.Identity.GetUserName());
            return RedirectToAction("List", "Reservation", new { ReservationType = oType, UpdateName = updatename, State = (int)oState, page, itemsPerPage });
        }


    }
}
