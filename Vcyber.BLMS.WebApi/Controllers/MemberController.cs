using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Entity.Common;
using Vcyber.BLMS.Entity.Member;
using Vcyber.BLMS.Entity;
namespace Vcyber.BLMS.WebApi.Controllers
{
    public class MemberController : ApiController
    {
        /// <summary>
        /// 添加领取记录
        /// </summary>
        [HttpPost]
        [Route("api/member/addreceiverecord")]
        [ResponseType(typeof(String))]
        public IHttpActionResult AddReceiveRecord(ReceiveRecord data)
        {
            var rsp = new ReturnObject("400", "领取记录创建失败", "");
            var flag = _AppContext.ReceiveRecordApp.Add(data);
            if (flag)
            {
                rsp.Code = "200";
                rsp.Message = "success";
            }                        
            return this.Ok(rsp);
        }
        /// <summary>
        /// 是否存在
        /// </summary>
        [HttpGet]
        [Route("api/member/receiverecordisexist")]
        [ResponseType(typeof(String))]
        public IHttpActionResult ReceiveRecordIsExist(string userId, string brandName)
        {
            var rsp = new ReturnObject("404", "领取记录不存在", "");
            var flag = _AppContext.ReceiveRecordApp.IsExist(userId, brandName);
            if (flag)
            {
                rsp.Code = "200";
                rsp.Message = "success";
            }
            return this.Ok(rsp);
        }
        /// <summary>
        /// 添加用户排名
        /// </summary>
        [HttpPost]
        [Route("api/member/addoweaggregations")]
        [ResponseType(typeof(String))]
        public IHttpActionResult AddOweAggregations(OweAggregations data)
        {
            var rsp = new ReturnObject("200", "success", "");
            _AppContext.OweAggregationsApp.Add(data);
            return this.Ok(rsp);
        }
    }
}
