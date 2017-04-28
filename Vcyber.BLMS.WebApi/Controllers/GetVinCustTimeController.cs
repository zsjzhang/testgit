using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Common;
using Vcyber.BLMS.WebApi.Filter;

using AspNet.Identity.SQL;

using Microsoft.AspNet.Identity;
namespace Vcyber.BLMS.WebApi.Controllers
{
    public class GetVinCustTimeController : ApiController
    {
        /// <summary>
        /// 返回银卡会员的姓名，VIN，购车日期
        /// </summary>
        /// <returns>测试</returns>
        [HttpGet]
        [ResponseType(typeof(List<GetVinCustTimeEF>))]
        [Route("api/GetVinCustTime/GetVinCustTimeList")]
        public IHttpActionResult GetVinCustTimeList()
        {
            var list = _AppContext.GetVinCustTime.GetVinCustTime().ToList();
            return Ok(new ReturnObject(list));
        }
    }
}