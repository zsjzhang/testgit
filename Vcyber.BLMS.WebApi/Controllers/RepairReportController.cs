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

namespace Vcyber.BLMS.WebApi.Controllers
{
    using PetaPoco;

    using Vcyber.BLMS.Entity.Enum;
    using Vcyber.BLMS.Entity.Generated;

    /// <summary>
    /// 服务记录
    /// </summary>
      [IOVAuthorize]
    public class RepairReportController : ApiController
    {
        /// <summary>
        /// 查询用户的服务记录（包括上门关怀, Home2Home, 3年9次免检）
        /// </summary>
        /// <param name="identityNumber">身份证号</param>
        /// <param name="type">服务类型</param>
        /// <param name="page">当前页数（默认第1页）</param>
        /// <param name="itemsPerPage">每页项数（默认10000）</param>
        /// <returns>服务信息列表</returns>
        [HttpGet]
        [ResponseType(typeof(Page<IFRepairReport>))]
        [Route("api/RepairReport/")]
        public IHttpActionResult QueryRepairRecords(string identityNumber, EDMSServiceType4Q type, long page = 1, long itemsPerPage = 10000)
        {
            Page<IFRepairReport> result = _AppContext.RepairRecordApp.QueryServiceOrders(identityNumber, type, page, itemsPerPage);
            return Ok(new ReturnObject(result));
        }

    }
}
