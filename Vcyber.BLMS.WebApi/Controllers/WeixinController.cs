using AspNet.Identity.SQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Domain;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Common;
using Vcyber.BLMS.Entity.Member;


namespace Vcyber.BLMS.WebApi.Controllers
{
    /// <summary>
    /// 微信相关
    /// </summary>
    public class WeixinController : ApiController
    {
        /// <summary>
        /// 添加客服消息
        /// </summary>
        /// <param name="query">消息集</param>
        /// <returns>无</returns>
        [HttpPost]
        [Route("api/weixin/addcustomerservicemessage")]
        public IHttpActionResult AddCustomerServiceMessage(CustomerServiceMessageQuery query)
        {
            DateTime baseStartTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            foreach (var obj in query.Items)
            {
                TimeSpan nowTimestamp = new TimeSpan(obj.time * 10000000);
                obj.opertime = baseStartTime.Add(nowTimestamp);  
                obj.timestamp = obj.time;                                
                _AppContext.CustomerServiceMessageApp.Add(obj);
            }
            _AppContext.CustomerServiceMessageApp.AddRecord(query.CurrDate.ToString());
            return this.Ok(new ReturnObject("200", "success", null));
        }
    }
}