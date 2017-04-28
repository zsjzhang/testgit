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
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.WebApi.Filter;

namespace Vcyber.BLMS.WebApi.Controllers
{
    public class SMSController : ApiController
    {
        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="phoneNumber">手机号</param>
        /// <param name="message">短信内容</param>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(ReturnResult))]
        [Route("api/SMS/Send")]
        public IHttpActionResult Send(string phoneNumber, string message)
        {
            var result = _AppContext.SMSApp.SendSMS(phoneNumber, message);

            return Ok(new ReturnObject(result));
        }

        /// <summary>
        /// 发送指定位数的数字验证码
        /// </summary>
        /// <param name="phoneNumber">手机号</param>
        /// <param name="num">验证码位数（默认6位）</param>
        /// <returns>发送结果</returns>
        [HttpPost]
        [ResponseType(typeof(ReturnResult))]
        [Route("api/SMS/ValidateCode/")]
        public IHttpActionResult Send(string phoneNumber, int num)
        {

            //获取请求渠道
            IEnumerable<string> appkeys = null;
            string appkey = string.Empty;
            string dataSource = string.Empty;
            if (this.Request.Headers.TryGetValues("appkey", out appkeys))
            {
                appkey = appkeys.First();
            }
            if (!string.IsNullOrEmpty(appkey))
            {
                dataSource = appkey;
            }
            else
            {
                dataSource = "blms_wechat";
            }
            var result = _AppContext.UserSecurityApp.SendMobileVerifyCode(phoneNumber, num < 4 ? 4 : num, dataSource);
            if (!result.IsSuccess)
                Vcyber.BLMS.Common.LogService.Instance.Error(result.Message);
            return Ok(new ReturnObject(result));
        }

    


    }
}
