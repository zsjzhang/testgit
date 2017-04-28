using System.Collections.Generic;
using System.Web.Http;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.WebApi.Filter;

namespace Vcyber.BLMS.WebApi.Controllers
{
    /// <summary>
    /// 个人中心 消息提醒
    /// </summary>
    public class MsgCenterController : ApiController
    {
        /// <summary>
        ///     返回未读消息总数
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        [IOVAuthorize]
        [HttpPost]
        [Route("Api/Msg/UnReadMessage")]
        public int GetUnReadMessage(string userId)
        {
            return _AppContext.UserMessageRecordApp.GetUnReadMessage(userId);
        }
        /// <summary>
        /// 返回消息记录
        /// </summary>
        /// <param name="msgType">消息类型1=系统 2=保养 3=积分变动 4=卡券 5=服务和活动</param>
        /// <param name="userId"></param>
        /// <param name="pageIndex">1</param>
        /// <param name="pageSize">8</param>
        /// <param name="totalCount">100</param>
        /// <returns>消息提醒记录</returns>
        [IOVAuthorize]
        [HttpPost]
        [Route("Api/Msg/UserMessageRecords")]
        public IEnumerable<UserMessageRecord> GetUserMessageRecords(int msgType, string userId, int pageIndex,
            int pageSize, int totalCount)
        {
            _AppContext.UserMessageRecordApp.UpdateState(userId, msgType);
            return _AppContext.UserMessageRecordApp.GetUserMessageRecords(msgType, userId, pageIndex,
                pageSize, out totalCount);
        }

        /// <summary>
        /// 返回消息类型和未读的记录数
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        [IOVAuthorize]
        [HttpPost]
        [Route("Api/Msg/UnReadMsgType")]
        public IEnumerable<UnReadMsgStatistics> StatisticsUnReadMsgType(string userId)
        {
            return _AppContext.UserMessageRecordApp.StatisticsUnReadMsgType(userId);
        }
    }
}