using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Security;
using AspNet.Identity.SQL;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.WebApi.Filter;
using Vcyber.BLMS.WebApi.Models;
using Vcyber.BLMS.WebApi.Providers;
using Vcyber.BLMS.WebApi.Results;
using IdentityUser = Microsoft.AspNet.Identity.EntityFramework.IdentityUser;
using Vcyber.BLMS.Application;
using WebGrease.Css.Extensions;
using System.Web.Http.Description;
using Vcyber.BLMS.Entity.Generated;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.WebApi.Common;


namespace Vcyber.BLMS.WebApi.Controllers
{
    [RoutePrefix("api/BKYX")]
    public class BKYXController : ApiController
    {
        /// <summary>
        /// 给用户添加积分
        /// </summary>
        /// <param name="vin">车架号</param>
        /// <param name="total">需要增加的积分值</param>
        /// <param name="aname">活动名称</param>
        /// <returns>成功与否</returns>
        [HttpPost]
        [Route("AddIntegral")]
        [ResponseType(typeof(bool))]
        public IHttpActionResult AddIntegral(string vin, int total, string aname)
        {
            if (string.IsNullOrEmpty(vin) || total < 1)
                return Ok(new ReturnResult { IsSuccess = false, Message = "参数错误，请确认参数正确" });

            var result = _AppContext.CarServiceUserApp.GetUserLevelByVin(vin);

            if (result == null)
            {
                return Ok(new ReturnResult { IsSuccess = false, Message = "未找到用户，请确认vin" });
            }

            _AppContext.UserIntegralApp.Add(new UserIntegral
            {
                userId = result.UserId,
                integralSource = "15",
                value = total,
                datastate = 0,
                remark = aname,
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now
            });

            return Ok(new ReturnResult { IsSuccess = true });
        }

        /// <summary>
        /// 根据VIN查询用户的等级
        /// </summary>
        /// <param name="vin"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetUserLevel")]
        public IHttpActionResult GetUserLevelByVin(string vin)
        {
            if (string.IsNullOrEmpty(vin))
                return Ok(new ReturnResult { IsSuccess = false, Message = "VIN不能为空" });

            var result = _AppContext.CarServiceUserApp.GetUserLevelByVin(vin);

            if (result == null)
                return Ok(new ReturnResult { IsSuccess = false, Message = "VIN未查询到对应用户信息，请确认" });

            return Ok(new ReturnResult { IsSuccess = true, Data = new { Level = result.MLevel, IsSonata9 = !string.IsNullOrEmpty(result.No) } });
        }

        /// <summary>
        /// 保存参加活动记录
        /// </summary>
        /// <param name="uname">参与人</param>
        /// <param name="mobile">参与人手机</param>
        /// <param name="aname">活动名称</param>
        /// <returns>成功与否</returns>
        [HttpPost]
        [Route("AddJoinRecord")]
        public IHttpActionResult AddJoinRecord(string uname, string mobile, string aname)
        {
            bool result = _AppContext.CarServiceUserApp.AddActivityJoinRecord(uname, mobile, aname);

            return Ok(new ReturnResult { IsSuccess = result });
        }

        [HttpPost]
        [Route("GetBrandService")]
        public IHttpActionResult GetBrandService(string userId)
        {
            var result = _AppContext.BrandServiceApp.SelectMembershipBrandByUserId(userId);
            return Ok(new ReturnResult { IsSuccess = true, Data = result });
        }
    }
}