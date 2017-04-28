using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.Application
{
    public interface IXDLotteryRecordApp
    {
        int AddXDLotteryRecord(XDLotteryRecord xDLotteryRecord);
        IEnumerable<XDLotteryRecord> GetXDLotteryRecordList(int activityId, int lotteryType, int lotteryRecordSource);

        bool IsExistLotteryRecordByUserId(string userId, int activityId, int lotteryType);

        bool IsExistLotteryRecordByUserMobile(string mobile, int activityId, int lotteryType);

    }
}
