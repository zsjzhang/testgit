
namespace Vcyber.BLMS.WebApi.Controllers
{
    using Microsoft.AspNet.Identity;

    using PetaPoco;
    using System.Web.Mvc;
    using Vcyber.BLMS.Application;
    using Vcyber.BLMS.Entity.CarService;
    using Vcyber.BLMS.Entity.Generated;

    public class ScheduleMaintController : Controller
    {
        /// <summary>
        /// 新增预约维保订单
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
    }
}
