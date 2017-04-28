using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Mvc;
using PetaPoco;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Entity.Common;
using Vcyber.BLMS.Entity.Enum;

namespace Vcyber.BLMS.WebApi.Controllers
{
    using Vcyber.BLMS.Entity.CarService;
    using Vcyber.BLMS.Entity.Generated;
    using Vcyber.BLMS.WebApi.Filter;

    /// <summary>
    ///所有预约服务
    /// </summary>
    [IOVAuthorize]
    public class ScheduleServiceController : ApiController
    {
        /// <summary>
        /// 查询指定用户的预约服务（不包括候机服务）
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <param name="page">当前页数（默认第1页）</param>
        /// <param name="itemsPerPage">每页项数（默认10000）</param>
        /// <returns>预约服务列表，其中PurchaseTimeFrame是计划购车时间</returns>
        [System.Web.Http.HttpGet]
        [ResponseType(typeof(Page<CSSonataServiceV>))]
        public IHttpActionResult QueryData(string userId, long page = 1, long itemsPerPage = 10000)
        {
            return this.Ok(new ReturnObject(_AppContext.ScheduleServiceApp.QueryUserOrdersByType(userId, page, itemsPerPage)));
        }
    }
}

