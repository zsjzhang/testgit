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
using log4net;
using AspNet.Identity.SQL;

using Microsoft.AspNet.Identity;

namespace Vcyber.BLMS.WebApi.Controllers
{
    public class AirportServiceController : ApiController
    {
        private ILog log = LogManager.GetLogger(typeof(AirportServiceController));
        /// <summary>
        /// 下发服务码
        /// </summary>
        /// <param name="phoneNumber">注册用户手机号</param>
        /// <returns>服务码</returns>
        [HttpGet]
        [Route("api/AirportService/SendCard")]
        public IHttpActionResult SendCard(string phoneNumber)
        {
            var store = new FrontUserStore<FrontIdentityUser>();

            if (store.CheckUserNameIsExist(phoneNumber))
            {
                string userId = store.FindByNameAsync(phoneNumber).Result.Id;

                string sncode = _AppContext.AirportServiceApp.SendCardByActivity(phoneNumber, userId);

                return Ok(new ReturnObject(new { result = "Y", code = sncode }));
            }
            else
            {
                return Ok(new ReturnObject(new { result = "N", code = "" }));
            }
        }

        /// <summary>
        /// 活动下发服务码
        /// </summary>
        /// <param name="phoneNumber">接收手机号</param>
        /// <returns>处理结果</returns>
        [HttpGet]
        [Route("api/AirportService/SendCardByActivity")]
        public IHttpActionResult SendCardByActivity(string phoneNumber)
        {
            ReturnResult result = new ReturnResult { IsSuccess = true };
            if (string.IsNullOrEmpty(phoneNumber))
            {
                result.IsSuccess = false;
                result.Message = "接收机场服务码的手机不能为空";
                return Ok(new ReturnObject(result));
            }

            result = _AppContext.AirportServiceApp.GetFreeServiceCard("81A187AF-F961-4616-BBF9-DE80B35CFE3F", phoneNumber, 1, 4, 999999);

            if (!result.IsSuccess)
            {
                result.IsSuccess = false;
            }
            return Ok(new ReturnObject(result));
        }

        /// <summary>
        /// 机场服务预约
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <param name="phoneNumber">接收服务码手机号</param>
        /// <param name="num1">免费的次数</param>
        /// <param name="num2">兑换的次数</param>
        /// <returns>执行结果和服务码数据</returns>
        [HttpGet]
        [ResponseType(typeof(IEnumerable<SNCard>))]
        [Route("api/AirportService/Appointment")]
        [IOVAuthorize]
        public IHttpActionResult Appointment(string userId, string phoneNumber, int num1, int num2, int airPortId)
        {
            //获取请求渠道
            IEnumerable<string> appkeys = null;
            string appkey = string.Empty;

            if (this.Request.Headers.TryGetValues("appkey", out appkeys))
            {
                appkey = appkeys.First();
            }
            log.Debug(string.Format(" userid:{0}, num1:{1}, num2: {2}, airportid:{3}", userId, num1, num2, airPortId));
            var result = _AppContext.AirportServiceApp.GetServiceCard(userId, phoneNumber, num1, num2, airPortId, "", appkey);
            log.Debug(string.Format("result:{0}, message:{1} , issuccess:{2}", result.Data, result.Message, result.IsSuccess));
            return Ok(new ReturnObject(result));
        }

        /// <summary>
        /// 获取用户可预约机场服务的免费次数
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <returns>可预约机场服务的免费次数</returns>
        [HttpGet]
        [ResponseType(typeof(int))]
        [Route("api/AirportService/GetSNCardNumber")]
        [IOVAuthorize]
        public IHttpActionResult GetSNCardNumber(string userId)
        {
            int number = _AppContext.AirportServiceApp.GetSNCardNumber(userId);
            var account = new FrontUserStore<FrontIdentityUser>().FindByIdAsync(userId).Result;
            if (account.MLevel == 11)
            {
                number = 2 - number;
            }
            if (account.MLevel == 12)
            {
                number = 3 - number;

            }
            return Ok(new ReturnObject("200", number));
        }

        /// <summary>
        /// 获取用户所有的机场服务券
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <returns>服务券列表</returns>
        [HttpGet]
        [ResponseType(typeof(List<SNCard>))]
        [Route("api/AirportService/Cards")]
        [IOVAuthorize]
        public IHttpActionResult Cards(string userId)
        {
            var list = _AppContext.AirportServiceApp.SelectSNCardByUser(userId).ToList();

            return Ok(new ReturnObject(list));
        }

        /// <summary>
        /// 获取用户所有的机场服务券
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <param name="status">状态(1:未发放，2:已发放，3:已使用)</param>
        /// <returns>服务券列表</returns>
        [HttpGet]
        [ResponseType(typeof(List<SNCard>))]
        [Route("api/AirportService/SearchCards")]
        //[IOVAuthorize]
        public IHttpActionResult SearchCards(string userId, int status)
        {
            var list = _AppContext.AirportServiceApp.SelectSNCardByUser(userId).Where(x => x.Status == status).ToList();

            return Ok(new ReturnObject(list));
        }

        /// <summary>
        /// 查询服务券的详细信息
        /// </summary>
        /// <param name="code">服务券的编号</param>
        /// <returns>服务券详细信息</returns>
        [HttpGet]
        [ResponseType(typeof(SNCard))]
        [Route("api/AirportService/Card")]
        [IOVAuthorize]
        public IHttpActionResult Card(string code)
        {
            var card = _AppContext.AirportServiceApp.SelectCard(code);

            return Ok(new ReturnObject(card));
        }

        /// <summary>
        /// 查询服务券的使用信息
        /// </summary>
        /// <param name="code">服务券的编号</param>
        /// <returns>服务券的使用信息</returns>
        [HttpGet]
        [ResponseType(typeof(SNUsedRecord))]
        [Route("api/AirportService/CardUsedRecord")]
        [IOVAuthorize]
        public IHttpActionResult CardUsedRecord(string code)
        {
            var record = _AppContext.AirportServiceApp.SelectCardUsedRecord(code);

            return Ok(new ReturnObject(record));
        }

        /// <summary>
        /// 查询机场对应的所有省份
        /// </summary>
        /// <returns>省份列表</returns>
        [HttpGet]
        [ResponseType(typeof(List<string>))]
        [Route("api/AirportService/ProvinceList")]
        public IHttpActionResult ProvinceList()
        {
            var list = _AppContext.AirportServiceApp.SelectAirportProvince().ToList();

            return Ok(new ReturnObject(list));
        }

        /// <summary>
        /// 查询所有机场列表
        /// </summary>
        /// <returns>机场列表</returns>
        [HttpGet]
        [ResponseType(typeof(List<Airport>))]
        [Route("api/AirportService/AirportList")]
        public IHttpActionResult AirportList()
        {
            var list = _AppContext.AirportServiceApp.SelectAirportList().ToList();

            return Ok(new ReturnObject(list));
        }

        /// <summary>
        /// 根据省、市查询机场列表
        /// </summary>
        /// <param name="province">省</param>
        /// <param name="city">市</param>
        /// <returns>机场列表</returns>
        [HttpGet]
        [ResponseType(typeof(List<Airport>))]
        [Route("api/AirportService/SearchAirport")]
        public IHttpActionResult SearchAirport(string province, string city)
        {
            var list = _AppContext.AirportServiceApp.SelectAirportList(province, city).ToList();

            return Ok(new ReturnObject(list));
        }

        /// <summary>
        /// 根据机场名称查询机场候机室列表
        /// </summary>
        /// <param name="airportName">机场名称</param>
        /// <returns>机场候机室列表</returns>
        [HttpGet]
        [ResponseType(typeof(List<Airport>))]
        [Route("api/AirportService/SearchAirportRoom")]
        public IHttpActionResult SearchAirportRoom(string airportName)
        {
            var list = _AppContext.AirportServiceApp.SelectAirportRoomList(airportName).ToList();

            return Ok(new ReturnObject(list));
        }
    }
}
