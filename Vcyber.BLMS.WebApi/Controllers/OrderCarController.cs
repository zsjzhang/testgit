using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Vcyber.BLMS.WebApi.Controllers
{
    using System.Web;
    using System.Web.Http.Description;
    using System.Web.UI;

    using AspNet.Identity.SQL;

    using Microsoft.AspNet.Identity;

    using PetaPoco;

    using Vcyber.BLMS.Application;
    using Vcyber.BLMS.Entity.CarService;
    using Vcyber.BLMS.Entity;
    using Vcyber.BLMS.Entity.Generated;
    using Vcyber.BLMS.Entity.Common;
    using Vcyber.BLMS.WebApi.Filter;

    /// <summary>
    /// 订车订单
    /// </summary>
    //[IOVAuthorize]
    public class OrderCarController : ApiController
    {
        /// <summary>
        /// 新增订车订单
        /// </summary>
        /// <param name="entity">订单内容</param>
        /// <returns>订单Id</returns>
        [Route("api/OrderCar/Add")]
        [HttpPost]
        [ResponseType(typeof(int))]
        [AllowAnonymous]
        public IHttpActionResult Add(OrderCarEntity entity)
        {
            //var account = new FrontUserStore<FrontIdentityUser>().FindByIdAsync(entity.UserId);

            if (string.IsNullOrEmpty(entity.UserId))
            {
                entity.UserId = "blms";
            }
            if (string.IsNullOrEmpty(entity.DataSource))
            {
                entity.DataSource = "blms";
            }
            //获取请求渠道
            IEnumerable<string> appkeys = null;
            string appkey = string.Empty;

            if (this.Request.Headers.TryGetValues("appkey", out appkeys))
            {
                appkey = appkeys.First();
            }
            entity.DataSource = appkey == string.Empty ? entity.DataSource : appkey;
            //entity.DataSource = appkey == string.Empty ? "blms" : appkey;

            int retval = _AppContext.OrderCarApp.Add(entity);
            if (retval > 0) return this.Ok(new ReturnObject((object)retval));
            return this.Ok(new ReturnObject(201));
        }

       /// <summary>
        /// 根据用户Id查询订车订单
       /// </summary>
       /// <param name="page">当前页数</param>
       /// <param name="itemsPerPage">每页数据条数</param>
       /// <returns></returns>
        [HttpGet]
        [Route("api/OrderCar/User")]
        [ResponseType(typeof(Page))]
       // [IOVAuthorize]
        public IHttpActionResult GetByUserId(long page=1, long itemsPerPage=20)
       {
           string userId = HttpContext.Current.User.Identity.Name;
           Page<CSOrderCar> list = _AppContext.OrderCarApp.QueryOrdersByUserId(userId, page, itemsPerPage);
            return this.Ok(new ReturnObject(list));
        }

        /// <summary>
        /// 根据订单Id获取订单详情
        /// </summary>
        /// <param name="id">订单Id</param>
        /// <returns>订单详情</returns>
        [HttpGet]
        [Route("GetById")]
       // [IOVAuthorize]
        [ResponseType(typeof(CSOrderCar))]
        public IHttpActionResult GetById(int id)
        {
            return this.Ok(new ReturnObject(_AppContext.OrderCarApp.GetEntityById(id)));
        }

        /// <summary>
        /// 根据用户Id获取预约试驾订单
        /// </summary>
        /// <param name="id">用户Id</param>
        /// <returns>预约试驾订单</returns>
        [HttpGet]
        [Route("GetTestListByUserId")]
        // [IOVAuthorize]
        [ResponseType(typeof(CSTestDrive))]
        public IHttpActionResult GetTestListByUserId(int UserId)
        {
            return this.Ok(new ReturnObject(_AppContext.OrderCarApp.GetEntityById(UserId)));
        }




    }
}
