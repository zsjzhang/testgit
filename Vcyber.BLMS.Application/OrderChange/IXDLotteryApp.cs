using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.Application
{
    public interface IXDLotteryApp
    {
        IEnumerable<XDLottery> GetLotteryListByActivityId(int activityId, int lotteryType);

        XDLottery GetNextLottery(int activityId, int lotteryType);
        void UpdateLotteryBalanceCount(int lotteryId, int activityId);
    }
}
