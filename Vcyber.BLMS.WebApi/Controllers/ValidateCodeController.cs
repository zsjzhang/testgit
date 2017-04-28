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
    public class ValidateCodeController : ApiController
    {

        /// <summary>
        /// 验证用户输入的手机验证是否正确
        /// </summary>
        /// <param name="phoneNumber">手机号</param>
        /// <param name="valideCode">验证码</param>
        /// <returns>执行结果</returns>
        [HttpGet]
        [ResponseType(typeof(ReturnResult))]
        [Route("api/ValidateCode/Validate")]
        public IHttpActionResult Validate(string phoneNumber, string valideCode)
        {
            var result = _AppContext.UserSecurityApp.ValidateMobileVerifyCode(phoneNumber, valideCode);

            return Ok(new ReturnObject(result));
        }
    }
}
