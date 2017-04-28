using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.IRepository
{
    public interface IXDLotteryStorager
    {
        IEnumerable<XDLottery> GetLotteryListByActivityId(int activityId, int lotteryType);

        void UpdateLotteryBalanceCount(int lotteryId, int activityId);
    }
}
