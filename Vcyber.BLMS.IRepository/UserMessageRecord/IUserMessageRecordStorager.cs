using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.IRepository
{
   public interface IUserMessageRecordStorager
   {

       int Insert(UserMessageRecord userMessageRecord);
       int UpdateState(int id);

       /// <summary>
       /// 返回用户的未读消息数量
       /// </summary>
       /// <param name="userId"></param>
       /// <returns></returns>
       int GetUnReadMessage(string userId);

       IEnumerable<UnReadMsgStatistics> StatisticsUnReadMsgType(string userId);

       IEnumerable<UserMessageRecord> GetUserMessageRecords(int msgType, string userId, int pageIndex, int pageSize,out int totalCount);
       
       void UpdateState(string userId, int msgType);

        IEnumerable<PaymentAccessPoint> GetPaymentAccessPoint(string dealerid, string starttime, string endtime);

        /// <summary>
        /// 更具userid和msgtype获取消息列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="msgtype"></param>
        /// <returns></returns>
        IEnumerable<UserMessageRecord> GetMsgList(string userId, int msgtype);

        /// <summary>
        /// 获取个人中心未读消息数目
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        //NoReadMsgModel NoReadList(string userId);

        IEnumerable<UserMessageRecord> GetNoReadMsg(int msgType, string userId);
    }
}
