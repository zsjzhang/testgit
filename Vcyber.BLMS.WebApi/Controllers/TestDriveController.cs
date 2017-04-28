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
    using Vcyber.BLMS.Entity.Enum;
    using Vcyber.BLMS.Entity.Generated;
    using Vcyber.BLMS.Entity.Common;
    using Vcyber.BLMS.WebApi.Filter;
    using Vcyber.BLMS.WebApi.Common;

    /// <summary>
    /// 预约试驾
    /// </summary>
    public class TestDriveController : ApiController
    {
        /// <summary>
        /// 新增预约试驾订单
        /// </summary>
        /// <param name="entity">订单内容</param>
        /// <returns>订单Id</returns>
        [Route("api/TestDrive")]
        [HttpPost]
        [ResponseType(typeof(int))]
        [AllowAnonymous]
        public IHttpActionResult Add(TestDriveEntity entity)
        {
            if (string.IsNullOrEmpty(entity.UserId))
            {
                entity.UserId = "blms";
            }
            if (string.IsNullOrEmpty(entity.DataSource))
            {
                entity.DataSource = "blms";
            }
            var account = new FrontUserStore<FrontIdentityUser>().FindByIdAsync(entity.UserId).Result;

            //获取请求渠道
            IEnumerable<string> appkeys = null;
            string appkey = string.Empty;

            if (this.Request.Headers.TryGetValues("appkey", out appkeys))
            {
                appkey = appkeys.First();
            }

            entity.DataSource = appkey == string.Empty ? entity.DataSource : appkey;
            int retval = _AppContext.TestDriveApp.Add(entity);
            if (retval > 0)
            {
                if (account != null)
                {
                    //int outValue;
                    //_AppContext.BreadApp.BlueBeanBread(
                    //    EBRuleType.预约试驾,
                    //    entity.UserId,
                    //    (MemshipLevel)account.MLevel,
                    //    out outValue);
                    //_AppContext.BreadApp.EmpiricBread(EEmpiricRule.预约试驾, entity.UserId, out outValue);
                }
                //预约成功后发送短信
                _AppContext.SMSApp.SendSMS(ESmsType.预约_预约成功, entity.Phone, new string[] { "试驾", entity.DealerName });
                return this.Ok(new ReturnObject((object)retval));
            }
            return this.Ok(new ReturnObject(201));
        }


       


        /// <summary>
        /// 根据用户Id查询预约试驾订单
        /// </summary>
        /// <param name="id">User Id</param>
        /// <param name="page">当前页数</param>
        /// <param name="itemsPerPage">每页数据条数</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/TestDrive/User")]
        [ResponseType(typeof(Page<CSTestDrive>))]
        //[IOVAuthorize]
        public IHttpActionResult GetByUserId(long page = 1, long itemsPerPage = 1)
        {

            Page<CSTestDrive> list = _AppContext.TestDriveApp.QueryOrdersByUserId(User.Identity.Name, page, itemsPerPage);
            return this.Ok(new ReturnObject(list));
        }

        /// <summary>
        /// 根据预约试驾订单ID查询详情
        /// </summary>
        /// <param name="id">预约试驾订单ID</param>
        /// <returns></returns>
        [Route("api/TestDrive/GetByUserId{id}")]
        [HttpGet]
        [ResponseType(typeof(CSTestDrive))]
        public IHttpActionResult GetByUserId(int id)
        {
            var entity = _AppContext.TestDriveApp.GetEntityById(id);
            return this.Ok(new ReturnObject(entity));
        }


       
    }
}
