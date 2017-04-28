using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.IRepository;

namespace Vcyber.BLMS.Domain
{
    public class XDLotteryRecordApp : IXDLotteryRecordApp
    {
        public XDLotteryRecordApp() { }
        public int AddXDLotteryRecord(XDLotteryRecord xDLotteryRecord)
        {
            return _DbSession.XDLotteryRecordStorager.AddXDLotteryRecord(xDLotteryRecord);

        }

        public IEnumerable<Entity.XDLotteryRecord> GetXDLotteryRecordList(int activityId, int lotteryType, int lotteryRecordSource)
        {
            return _DbSession.XDLotteryRecordStorager.GetXDLotteryRecordList(activityId, lotteryType, lotteryRecordSource);
        }

        public bool IsExistLotteryRecordByUserId(string userId, int activityId, int lotteryType)
        {
            return _DbSession.XDLotteryRecordStorager.IsExistLotteryRecordByUserId(userId, activityId, lotteryType);
        }

        public bool IsExistLotteryRecordByUserMobile(string mobile, int activityId, int lotteryType)
        {
            return _DbSession.XDLotteryRecordStorager.IsExistLotteryRecordByUserMobile(mobile,activityId,lotteryType);
        }

    }
}
