
namespace Vcyber.BLMS.ManageWeb.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Web.Routing;
    using System.Web.UI.WebControls;

    using Microsoft.AspNet.Identity;

    using PetaPoco;
    using System.Web.Mvc;
    using System.Web.UI;
    using Vcyber.BLMS.Application;
    using Vcyber.BLMS.Entity.CarService;
    using Vcyber.BLMS.Entity.Enum;
    using Vcyber.BLMS.Entity.Generated;

    [MvcAuthorize]
    public class CarTypeController : Controller
    {
        public ActionResult List(ECarSeriesType type)
        {
            if (type == null) type = ECarSeriesType.TestDrive;
            IEnumerable<CSBaseCar> list = _AppContext.BaseCarApp.QueryCars(type);
            ViewData.Add("data", list);
            return this.View(list);
        }

        public ActionResult Update(int id, QueryParamEntity entity, long page, long itemsPerPage)
        {
            _AppContext.SonataServiceApp.UpdateState(id, EOrderState.Processed, HttpContext.User.Identity.GetUserId(), HttpContext.User.Identity.GetUserName());
            return RedirectToAction("QueryOrders", new { entity, page, itemsPerPage });
        }

        /// <summary>
        /// 新增订车订单
        /// </summary>
        /// <param name="entity">订单内容</param>
        /// <returns>订单Id</returns>
        [HttpPost]
        public ActionResult Add(CSBaseCar entity)
        {
            if (!ModelState.IsValid)
            {
                return this.View("Add", entity);
            }
            int retval = _AppContext.BaseCarApp.Add(entity);
            this.ViewData.Add("message", retval > 0 ? "提交成功" : "提交失败");
            return RedirectToAction("Add");

        }
        [HttpGet]
        public ActionResult Add()
        {
            return this.View();
        }

        /// <summary>
        /// 车型下拉框
        /// </summary>
        /// <returns></returns>
        public ActionResult CarTypeView()
        {
            IEnumerable<CSBaseCar> _result = _AppContext.BaseCarApp.QueryCars(ECarSeriesType.OrderCar);
            return View(_result);
        }


    }
}
