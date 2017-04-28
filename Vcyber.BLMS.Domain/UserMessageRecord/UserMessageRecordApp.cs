using System.Collections.Generic;
using System.Threading.Tasks;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.IRepository;


namespace Vcyber.BLMS.Domain
{
    public class UserMessageRecordApp : IUserMessageRecordApp
    {
        public int Insert(UserMessageRecord userMessageRecord)
        {
            return _DbSession.UserMessageRecordStorager.Insert(userMessageRecord);
        }

        public int UpdateState(int id)
        {
            return _DbSession.UserMessageRecordStorager.UpdateState(id);
        }

        public int GetUnReadMessage(string userId)
        {
            return _DbSession.UserMessageRecordStorager.GetUnReadMessage(userId);
        }

        public IEnumerable<UnReadMsgStatistics> StatisticsUnReadMsgType(string userId)
        {
            return _DbSession.UserMessageRecordStorager.StatisticsUnReadMsgType(userId);
        }


        public IEnumerable<UserMessageRecord> GetUserMessageRecords(int msgType, string userId, int pageIndex,
            int pageSize, out int totalCount)
        {
            return _DbSession.UserMessageRecordStorager.GetUserMessageRecords(msgType, userId, pageIndex, pageSize,
                out totalCount);
        }

        public void UpdateState(string userId, int msgType)
        {
            _DbSession.UserMessageRecordStorager.UpdateState(userId, msgType);
        }

        public void InsertLoginChangePasswordMessage(string userId)
        {
            Task.Factory.StartNew(() =>
                {
                    if (!_DbSession.MembershipLoginNotifyStorager.IsExists(userId, LoginNotifyType.Password))
                    {
                        _DbSession.MembershipLoginNotifyStorager.Insert(new MembershipLoginNotify
                        {
                            UserId = userId,
                            NotifyType = LoginNotifyType.Password,
                            DataSource = 1
                        });
                        _DbSession.UserMessageRecordStorager.Insert(new UserMessageRecord
                        {
                            UserId = userId,
                            MsgContent = "尊敬的会员：您好，密码系统升级，为确保您的账户信息安全，要求密码不少于8位。如需修改，请前往“账户管理”－“修改密码”功能，进行修改。",
                            MsgType = MessageType.System
                        });
                    }
                });

        }

        /// <summary>
        /// 更具userid和msgtype获取消息列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="msgtype"></param>
        /// <returns></returns>
        public IEnumerable<UserMessageRecord> GetMsgList(string userId, int msgtype)
        {
            return _DbSession.UserMessageRecordStorager.GetMsgList(userId, msgtype);
        }

        /// <summary>
        /// 获取个人中心未读消息数目
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        //public NoReadMsgModel NoReadList(string userId)
        //{
        //    return _DbSession.UserMessageRecordStorager.NoReadList(userId);
        //}

        public IEnumerable<UserMessageRecord> GetNoReadMsg(int msgType, string userId)
        {
            return _DbSession.UserMessageRecordStorager.GetNoReadMsg(msgType, userId);
        }

        public IEnumerable<PaymentAccessPoint> GetPaymentAccessPoint(string dealerid, string starttime, string endtime)
        {
            return _DbSession.UserMessageRecordStorager.GetPaymentAccessPoint(dealerid, starttime, endtime);
        }
    }
}