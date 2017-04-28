using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.IRepository;

namespace Vcyber.BLMS.Domain
{
    public class XDLotteryApp : IXDLotteryApp
    {
        public IEnumerable<XDLottery> GetLotteryListByActivityId(int activityId, int lotteryType)
        {
            return _DbSession.XDLotteryStorager.GetLotteryListByActivityId(activityId, lotteryType);
        }

        public XDLottery GetNextLottery(int activityId, int lotteryType)
        {
            XDLottery thankLottery = new XDLottery();
            thankLottery.ActivityId = activityId;
            thankLottery.LotteryId = 0;
            thankLottery.LotteryName = ConfigurationManager.AppSettings["ThanksLotteryName"];
            thankLottery.LotteryType = lotteryType;
            thankLottery.LotteryPosition = ConfigurationManager.AppSettings["ThanksLotteryPosition"];
            XDActivity activity = _DbSession.XDActivityStorager.GetXDActivityByActivityId(activityId);
            List<XDLottery> lstLottery = _DbSession.XDLotteryStorager.GetLotteryListByActivityId(activityId, lotteryType).ToList();
            XDLottery lottery = GetRandomLottery(lstLottery);
            if (lottery==null)
            {
                return thankLottery;
            }
            int now = ConvertDateTimeInt(DateTime.Now);
            int startTime = ConvertDateTimeInt(activity.ActivityStartTime);
            int endTime = ConvertDateTimeInt(activity.ActivityEndTime);
            int lotteryTotalCount = activity.LotteryTotalCount;
            int lotteryBalanceCount = activity.LotteryBalanceCount;

            int detaTime = (endTime - startTime) / lotteryTotalCount;
            int seed = startTime + (lotteryTotalCount - lotteryBalanceCount) * detaTime + lotteryTotalCount;
            Random random = new Random(seed);

            DateTime preReleaseTime = GetTime((startTime + (lotteryTotalCount - lotteryBalanceCount) * detaTime).ToString());

            int releaseTime = startTime + (lotteryTotalCount - lotteryBalanceCount) * detaTime + Math.Abs(random.Next()) % detaTime;

            DateTime releaseDate = GetTime(releaseTime.ToString());
            if (now < releaseTime)
            {
                return thankLottery;
            }

            return lottery;
        }
        /// <summary>
        /// 时间戳转为C#格式时间
        /// </summary>
        /// <param name=”timeStamp”></param>
        /// <returns></returns>
        private DateTime GetTime(string timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime); return dtStart.Add(toNow);
        }
        private int ConvertDateTimeInt(DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (int)(time - startTime).TotalSeconds;
        }

        private XDLottery GetRandomLottery(List<XDLottery> lstLottery)
        {
            if (lstLottery == null || lstLottery.Count <= 0)
            {
                return null;
            }
            int weight = 0;
            foreach (var item in lstLottery)
            {
                weight += item.LotteryRate;
            }

            Random random = new Random((DateTime.Now.Hour+DateTime.Now.Minute+DateTime.Now.Second+DateTime.Now.Millisecond));
            int num = random.Next(weight);

            foreach (var item in lstLottery)
            {
                num -= item.LotteryRate;
                if (true)
                {
                    if (num < 0)
                    {
                        return item;
                    }
                }
            }
            return null;
        }

        public void UpdateLotteryBalanceCount(int lotteryId, int activityId)
        {
            _DbSession.XDLotteryStorager.UpdateLotteryBalanceCount(lotteryId, activityId);
        }
    }
}
