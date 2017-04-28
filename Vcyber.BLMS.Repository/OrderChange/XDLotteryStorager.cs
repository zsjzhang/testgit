using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.IRepository;

namespace Vcyber.BLMS.Repository
{
    public class XDLotteryStorager : IXDLotteryStorager
    {
        public XDLotteryStorager() { }
        public IEnumerable<XDLottery> GetLotteryListByActivityId(int activityId, int lotteryType)
        {
            string sql = string.Empty;
            if (lotteryType==0)
            {
                sql = "SELECT * FROM XD_Lottery WHERE ActivityId=@ActivityId and IsValid=1 and LotteryBalanceCount>0";
            }
            else
            {
                sql = "SELECT * FROM XD_Lottery WHERE ActivityId=@ActivityId and  LotteryType=@LotteryType AND IsValid=1  and LotteryBalanceCount>0";
            }

            return DbHelp.Query<XDLottery>(sql, new { ActivityId = activityId, LotteryType = lotteryType });
        }

        public void UpdateLotteryBalanceCount(int lotteryId,int activityId)
        {
            string sql = "UPDATE XD_Lottery SET LotteryBalanceCount=LotteryBalanceCount-1 WHERE LotteryId=@LotteryId AND ActivityId=@ActivityId";
            DbHelp.Execute(sql, new { LotteryId = lotteryId, ActivityId = activityId });
        }
    }
}
