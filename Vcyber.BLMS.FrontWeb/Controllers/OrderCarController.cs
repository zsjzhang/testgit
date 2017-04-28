
namespace Vcyber.BLMS.FrontWeb.Controllers
{
    using System;

    using Microsoft.AspNet.Identity;

    using PetaPoco;
    using System.Web.Mvc;
    using System.Web.UI;
    using Vcyber.BLMS.Application;
    using Vcyber.BLMS.Common;
    using Vcyber.BLMS.Domain.Common;
    using Vcyber.BLMS.Entity.CarService;
    using Vcyber.BLMS.Entity.Generated;

    public class OrderCarController : Controller
    {
        /// <summary>
        /// 新增订车订单
        /// </summary>
        /// <param name="entity">订单内容</param>
        /// <returns>订单Id</returns>
        [HttpPost]
        public ActionResult Add(OrderCarEntity entity)
        {
            if(!ModelState.IsValid)
            {
                return this.View("Add", entity);
            }
            int retval = _AppContext.OrderCarApp.Add(entity, HttpContext.User.Identity.GetUserId(), HttpContext.User.Identity.GetUserName());
            this.ViewData.Add("message", retval > 0 ? "提交成功" : "提交失败");
            return RedirectToAction("Add");
 
        }
        [HttpGet]
        public ActionResult Add()
        {
            return this.View();
        }

       /// <summary>
        /// 根据用户Id查询订车订单
       /// </summary>
       /// <param name="id">User Id</param>
       /// <param name="page">当前页数</param>
       /// <param name="itemsPerPage">每页数据条数</param>
       /// <returns></returns>
        //[HttpGet]
        //[Route("api/OrderCar/User/{id}")]
        //public ActionResult GetByUserId(string id, long page=1, long itemsPerPage=20)
        //{
        //    Page<CSOrderCar> list = _AppContext.OrderCarApp.QueryOrdersByUserId(id, page, itemsPerPage);
        //   return this.View();
        //}
    }
}
