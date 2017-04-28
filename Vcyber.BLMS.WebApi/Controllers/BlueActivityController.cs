using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.WebApi.Filter;

namespace Vcyber.BLMS.WebApi.Controllers
{
    /// <summary>
    /// 蓝豆清零活动
    /// </summary>
    [IOVAuthorize]
    public class BlueActivityController : ApiController
    {
        /// <summary>
        /// 根据手机号查询获奖情况，中奖则返回奖品名称
        /// </summary>
        /// <param name="phone">手机号</param>
        /// <returns>中奖返回奖品名称，否则返回空</returns>
        [ResponseType(typeof(string))]
        [Route("api/BlueActivity/QueryWinRecordByPhone")]
        public IHttpActionResult QueryWinRecordByPhone(string phone)
        {
            var name = _AppContext.BluebeanWinRecordApp.QueryWinRecord(phone);

            return Ok(name);
        }
        /// <summary>
        /// 查询获奖名单
        /// </summary>
        /// <param name="quantity">查询数量</param>
        /// <returns>名单记录</returns>
        [ResponseType(typeof(IEnumerable<BluebeanWinRecord>))]
        [Route("api/BlueActivity/QueryWinRecords")]
        public HttpResponseMessage QueryWinRecords(int quantity = 50)
        {
            var list = _AppContext.BluebeanWinRecordApp.QueryWinRecords(50);
            return this.Request.CreateResponse(HttpStatusCode.OK, list);
        }

        /// <summary>
        /// 查询用户的中奖记录
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <returns>中奖信息</returns>
        [ResponseType(typeof(BluebeanWinRecord))]
        [Route("api/BlueActivity/QueryWinRecordByUserId")]
        public IHttpActionResult QueryWinRecordByUserId(string userId)
        {
            var bluebeanWinRecord = _AppContext.BluebeanWinRecordApp.QueryWinRecordByUserId(userId);

            return Ok(bluebeanWinRecord);
        }

        /// <summary>
        /// 用户开始抽奖
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <returns></returns>
        [ResponseType(typeof(BluebeanWinResult))]
        [Route("api/BlueActivity/DrawLuck")]
        public IHttpActionResult DrawLuck(string userId)
        {
            var result = _AppContext.BluebeanWinRecordApp.DrawLuck(userId);

            return Ok(result);
        }
        /// <summary>
        /// 填写邮寄地址
        /// </summary>
        /// <param name="bluebeanWinRecord"></param>
        /// <returns></returns>
        [ResponseType(typeof(bool))]
        [Route("api/BlueActivity/Address")]
        public IHttpActionResult Address(BluebeanWinRecord bluebeanWinRecord)
        {
            var result = _AppContext.BluebeanWinRecordApp.UpdateAddress(bluebeanWinRecord);

            return Ok(result);
        }
    }
}