using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using log4net;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.IRepository;

namespace Vcyber.BLMS.Domain
{
    public class BluebeanWinRecordApp : IBluebeanWinRecordApp
    {
        private static ILog logger = LogManager.GetLogger(typeof(BluebeanWinRecordApp));
        private static readonly int BluePrizeRate = Convert.ToInt32(ConfigurationManager.AppSettings["BluePrizeRate"]);
        private static readonly int BluePrizeNum = Convert.ToInt32(ConfigurationManager.AppSettings["BluePrizeNum"]);
        public string QueryWinRecord(string phone)
        {
            return AppServiceLocator.Instance.GetInstance<IBluebeanWinRecordStorager>().QueryWinRecord(phone);
        }

        public IEnumerable<BluebeanWinRecord> QueryWinRecords(int quantity = 50)
        {
            var list = AppServiceLocator.Instance.GetInstance<IBluebeanWinRecordStorager>().QueryWinRecords(quantity);
            var newList = new List<BluebeanWinRecord>();
            foreach (var item in list)
            {
                BluebeanWinRecord winRecord = new BluebeanWinRecord();
                try
                {
                    if (!string.IsNullOrEmpty(item.Phone))
                    {
                        var len = item.Phone.Length;
                        var begin = item.Phone.Substring(0, 3);
                        var end = item.Phone.Substring(len - 4);
                        winRecord.Phone = string.Format("{0}****{1}", begin, end);
                    }
                    else
                    {
                        winRecord.Phone = string.Format("13{1}****{0}",
                            new Random(Guid.NewGuid().GetHashCode()).Next(1000, 9999),
                            new Random(Guid.NewGuid().GetHashCode()).Next(3, 10));
                    }
                }
                catch (Exception)
                {
                    winRecord.Phone = string.Format("13{1}****{0}",
                           new Random(Guid.NewGuid().GetHashCode()).Next(1000, 9999),
                           new Random(Guid.NewGuid().GetHashCode()).Next(3, 10));
                }
                winRecord.Prize = item.Prize;
                newList.Add(winRecord);
            }
            return newList;
        }

        public bool UpdateAddress(BluebeanWinRecord bluebeanWinRecord)
        {
            return AppServiceLocator.Instance.GetInstance<IBluebeanWinRecordStorager>().UpdateAddress(bluebeanWinRecord);
        }

        public BluebeanWinResult DrawLuck(string userId)
        {
            BluebeanWinResult result = new BluebeanWinResult();
            try
            {
                var dt = Convert.ToDateTime(ConfigurationManager.AppSettings["BlueActivityTimeout"]);
                if (dt < DateTime.Now)
                {
                    result.Success = false;
                    result.Msg = "活动已经下线,无法抽奖";
                    return result;
                }
                var blueBean = _AppContext.UserBlueBeanApp.GetTotalBlueBean(userId);
                if (blueBean < 100)
                {
                    result.Success = false;
                    result.Msg = "您好，您的蓝豆不足，无法兑换抽奖机会哦。";
                    return result;
                }
                var blueBeanList = _AppContext.UserBlueBeanApp.GetList(userId);
                List<UserblueBean> cleanList = new List<UserblueBean>();
                var left = 0;
                var currentClear = 0;
                foreach (var item in blueBeanList)
                {
                    currentClear = item.value - item.usevalue;
                    if (currentClear >= 100 - left)
                    {
                        UserblueBean bean = new UserblueBean();
                        bean.Id = item.Id;
                        bean.usevalue = item.usevalue + 100 - left;
                        cleanList.Add(bean);
                        break;
                    }
                    else
                    {
                        UserblueBean bean = new UserblueBean();
                        bean.Id = item.Id;
                        left += item.value - item.usevalue;
                        bean.usevalue = item.value;
                        cleanList.Add(bean);
                    }
                }
                _DbSession.UserBlueBeanStorager.CleanBlueBean(cleanList);
                Random rd = new Random(Guid.NewGuid().GetHashCode());
                var n = rd.Next();
                var badLuck = n % 2 == 1 ? 0 : 2;
                var count = AppServiceLocator.Instance.GetInstance<IBluebeanWinRecordStorager>().GetCurrentWinPrizeCount();
                if (count >= BluePrizeNum)
                {
                    InserRecord(new BluebeanActiveRecord() { UserId = userId, IsSelected = false, PrizeName = "" });
                    result.Success = true;
                    result.Number = badLuck;
                    return result;
                }
                var isSelected = AppServiceLocator.Instance.GetInstance<IBluebeanWinRecordStorager>().IsSelected(userId);
                if (isSelected)
                {
                    InserRecord(new BluebeanActiveRecord() { UserId = userId, IsSelected = false, PrizeName = "" });
                    result.Success = true;
                    result.Number = badLuck;
                    return result;
                }
                var prizeList = AppServiceLocator.Instance.GetInstance<IBluebeanWinRecordStorager>().GetPrizeByType("Bluebean").ToList();
                if (prizeList != null)
                {
                    if (prizeList.All(p => p.LeftNum == 0))
                    {
                        InserRecord(new BluebeanActiveRecord() { UserId = userId, IsSelected = false, PrizeName = "" });
                        result.Success = true;
                        result.Number = badLuck;
                        return result;
                    }
                }

                Random random = new Random(Guid.NewGuid().GetHashCode());
                var num = random.Next(1, 101);
                var isGoogLuck = num <= BluePrizeRate;
                if (isGoogLuck)
                {

                    if (prizeList.All(p => p.LeftNum > 0))
                    {
                        result.Number = prizeList[n % 2].Id;
                    }
                    else
                    {
                        result.Number = prizeList[0].Id;
                    }
                    string name = prizeList.Single(p => p.Id == result.Number).Name;
                    InserRecord(new BluebeanActiveRecord() { UserId = userId, IsSelected = true, PrizeName = name });
                    result.WinId = AppServiceLocator.Instance.GetInstance<IBluebeanWinRecordStorager>()
                         .InsertWinRecords(result.Number, new BluebeanWinRecord()
                         {
                             UserId = userId,
                             Prize = name
                         });
                    result.Success = true;
                }
                else
                {
                    InserRecord(new BluebeanActiveRecord() { UserId = userId, IsSelected = false, PrizeName = "" });
                    result.Success = true;
                    result.Number = badLuck;
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception);
                result.Success = true;
                result.Number = 0;
            }

            return result;
        }

        private void InserRecord(BluebeanActiveRecord bluebeanActiveRecord)
        {
            AppServiceLocator.Instance.GetInstance<IBluebeanWinRecordStorager>().Insert(bluebeanActiveRecord);
        }



        public BluebeanWinRecord QueryWinRecordByUserId(string userId)
        {
            return AppServiceLocator.Instance.GetInstance<IBluebeanWinRecordStorager>().QueryWinRecordByUserId(userId);
        }
    }
}