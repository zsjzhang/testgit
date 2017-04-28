using AspNet.Identity.SQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.IRepository;

namespace Vcyber.BLMS.Domain
{
    public class ActivityInfoApp : IActivityInfoApp
    {
        public ActivityInfo GetActivityInfoByID(int id)
        {
            return _DbSession.ActivityInfoStorager.GetActivityInfoByID(id);
        }
        /// <summary>
        /// 查询单条活动记录
        /// </summary>
        /// <param name="name">活动名称</param>
        /// <returns></returns>
        public ActivityInfo GetActivityInfoByName(string name)
        {
            return _DbSession.ActivityInfoStorager.GetActivityInfoByName(name);
        }
        public List<ActivityInfo> GetActivityInfoAll()
        {
            return _DbSession.ActivityInfoStorager.GetActivityInfoAll().ToList();
        }

        public bool AddActivityInfo(ActivityInfo entity)
        {
            int offsetnum = _DbSession.ActivityInfoStorager.AddActivityInfo(entity);
            if (offsetnum > 0) return true;
            else return false;
        }


        public bool UpdateActivityInfo(ActivityInfo entity)
        {
            return _DbSession.ActivityInfoStorager.UpdateActivityInfo(entity) > 0;
        }


        public List<int> GetDistinctActivityId()
        {
            return _DbSession.ActivityInfoStorager.GetDistinctActivityId().ToList();
        }


        public bool EndActivityInfo(int id)
        {
            return _DbSession.ActivityInfoStorager.EndActivityInfo(id) > 0;
        }
        /// <summary>
        /// 抽奖（根据活动的中奖率）
        /// </summary>
        /// <param name="activityId">活动ID</param>
        /// <param name="userId">用户ID</param>        
        /// <returns>中奖的奖品</returns>
        public PrizesInfo NormalDraw(int activityId, string userId, string source = "blms_web")
        {
            var activity = _DbSession.ActivityInfoStorager.GetActivityInfoByID(activityId);
            //添加参加活动记录
            _DbSession.JoinActivityStorager.AddJoinActivity(
                new JoinActivity()
                {
                    ActivityId = activityId,
                    UserId = userId,
                    Results1 = source,
                    CreateDate = DateTime.Now
                }
            );
            var rand = new Random();
            var activityRand = rand.Next(100);
            PrizesInfo p = new PrizesInfo();
            //活动中奖率，按百分比，随机数小于中奖率的可以中奖
            if (activityRand < activity.Probability)
            {
                var prizes = _AppContext.PrizesInfoApp.GetPrizesInfosByActivityId(activityId);
                var prizesUseForDay = _DbSession.WinningInfoStorager.GetPrizeUse(activityId);
                //抽奖
                var waitDrawPrizes = new List<PrizesInfo>();
                int lastSection = 0;                
                //遍历还可以抽的奖项
                foreach (var prize in prizes)
                {
                    //如果有库存
                    if (prize.UsedNum < prize.TotalNum)
                    {
                        var usePrizeForDay = prizesUseForDay.FirstOrDefault(x => x.Id == prize.Id);
                        //如果今天没有出奖或者没有达到今天的出奖数量
                        if (usePrizeForDay == null || usePrizeForDay.CyclesUnuseNum < prize.CyclesNum || prize.CyclesNum == 0)
                        {                            
                            //当前奖品比例
                            int rate = (int)prize.Rate + lastSection;
                            //不能小于上个奖品的比例
                            prize.BeginSection = lastSection;
                            //不能大于等于当前奖品比例
                            prize.EndSection = rate;
                            //当前奖品比例赋给上次奖励比例，供下次用
                            lastSection = rate;
                            waitDrawPrizes.Add(prize);
                        }
                    }
                }
                //如果有可以抽的将，抽奖
                if (waitDrawPrizes.Count() > 0)
                {
                    var randomMaxValue = 0;//随机值
                    //如果使用概率抽奖
                    if (lastSection > 0)
                    {
                        randomMaxValue = lastSection + 1;//随机最大值
                        var prizeRand = rand.Next(1, randomMaxValue);
                        p = waitDrawPrizes.FirstOrDefault(x => (prizeRand > x.BeginSection && prizeRand <= x.EndSection));
                    }
                    else
                    {
                        //如果使用平均随机抽奖
                        randomMaxValue = waitDrawPrizes.Count;//随机最大值
                        var prizeRand = rand.Next(randomMaxValue);
                        p = waitDrawPrizes[prizeRand];
                    }
                    //添加中奖记录
                    _DbSession.WinningInfoStorager.AddWinningInfo(
                        new WinningInfo()
                        {
                            ActivityId = activityId,
                            UserId = userId,
                            UserType = 1,
                            State = 0,
                            PrizesId = p.Id,
                            UpdateTime = DateTime.Now,
                            CreateTime = DateTime.Now
                        }
                    );
                    //增加使用量
                    _DbSession.PrizesInfoStorager.PrizeMinus(p.Id);
                }
            }
            return p;
        }
        /// <summary>
        /// 抽奖（感恩季活动）
        /// </summary>
        /// <param name="activityId">活动ID</param>
        /// <param name="userId">用户ID</param>        
        /// <returns>中奖的奖品</returns>
        public PrizesInfo ThankDraw(int activityId, string userId, string source = "blms_web")
        {
            var activity = _DbSession.ActivityInfoStorager.GetActivityInfoByID(activityId);            
            //添加参加活动记录
            _DbSession.JoinActivityStorager.AddJoinActivity(new JoinActivity() { ActivityId = activityId, UserId = userId, Results1 = source, CreateDate = DateTime.Now });
            var rand = new Random();
            var activityRand = rand.Next(100);
            PrizesInfo p = new PrizesInfo();
            var winningObj = _AppContext.WinningInfoApp.GetWinningByUserIdAndActicityId(activityId, userId);
            //活动中奖率，按百分比，随机数小于中奖率的可以中奖,并且本次活动没有中过奖。            
            if (winningObj == null && activityRand < activity.Probability)
            {
                var integral = _AppContext.UserIntegralApp.GetTotalIntegral(userId);
                if (integral > 0 && integral <= 10000)
                {
                    var prizes = _AppContext.PrizesInfoApp.GetPrizesInfosByActivityId(activityId);
                    var prizesUseForDay = _DbSession.WinningInfoStorager.GetPrizeUse(activityId);
                    //抽奖
                    var waitDrawPrizes = new List<PrizesInfo>();
                    int lastSection = 0;
                    //遍历还可以抽的奖项
                    foreach (var prize in prizes)
                    {
                        //如果有库存
                        if (prize.UsedNum < prize.TotalNum)
                        {
                            var usePrizeForDay = prizesUseForDay.FirstOrDefault(x => x.Id == prize.Id);
                            //如果今天没有出奖或者没有达到今天的出奖数量
                            if (usePrizeForDay == null || usePrizeForDay.CyclesUnuseNum < prize.CyclesNum)
                            {                                
                                //当前奖品比例
                                int rate = (int)prize.Rate + lastSection;
                                //不能小于上个奖品的比例
                                prize.BeginSection = lastSection;
                                //不能大于等于当前奖品比例
                                prize.EndSection = rate;
                                //当前奖品比例赋给上次奖励比例，供下次用
                                lastSection = rate;
                                waitDrawPrizes.Add(prize);
                            }
                        }
                    }
                    //如果有可以抽的将，抽奖
                    if (waitDrawPrizes.Count() > 0)
                    {
                        var randomMaxValue = 0;//随机值
                        //如果使用概率抽奖
                        if (lastSection > 0)
                        {
                            randomMaxValue = lastSection + 1;//随机最大值
                            var prizeRand = rand.Next(1, randomMaxValue);
                            p = waitDrawPrizes.FirstOrDefault(x => (prizeRand > x.BeginSection && prizeRand <= x.EndSection));
                        }
                        else
                        {
                            //如果使用平均随机抽奖
                            randomMaxValue = waitDrawPrizes.Count;//随机最大值
                            var prizeRand = rand.Next(randomMaxValue);
                            p = waitDrawPrizes[prizeRand];
                        }
                        if (p.Id > 0)
                        {
                            var obj = new OweAggregations();
                            var _redisExtend = new RedisExtend();
                            using (_redisExtend)
                            {
                                _redisExtend.Connect();
                                obj = _redisExtend.Hget<OweAggregations>("MyDB", userId);
                                if (obj == null)
                                {
                                    obj = new OweAggregations();
                                }
                            }
                            if (obj.AccntType != "个人客户")
                            {
                                p = new PrizesInfo();
                            }
                            //如果中了一等奖，判断是否有资格领取
                            if (p.PrizeLevel == 1)
                            {
                                var account = new FrontUserStore<FrontIdentityUser>().FindByIdAsync(userId).Result;
                                if (obj == null)
                                {
                                    obj = new OweAggregations();
                                    obj.Mlevel = account.MLevel;
                                }
                                //金卡/银卡 &&　返厂次数大于３
                                if (obj.Mlevel < 11 || obj.RepairCout <= 3)
                                {
                                    p = new PrizesInfo();
                                }
                                var exDays = DateTime.Parse("2017-01-05") - DateTime.Now;
                                var num = exDays.Days / 5;
                                if (num == 3 || num == 2)
                                {
                                    if (p.UsedNum >= 1)
                                    {
                                        p = new PrizesInfo();
                                    }
                                }
                                else if (num == 1)
                                {
                                    if (p.UsedNum >= 2)
                                    {
                                        p = new PrizesInfo();
                                    }                                
                                }
                                else if (num == 0)
                                {
                                    if (p.UsedNum >= 3)
                                    {
                                        p = new PrizesInfo();
                                    }
                                }
                                else
                                {
                                    p = new PrizesInfo();
                                }
                            }
                            //如果确实是中奖了
                            if (p.Id > 0)
                            {
                                //添加中奖记录
                                p.WinningInfoId = _DbSession.WinningInfoStorager.AddWinningInfo(
                                    new WinningInfo()
                                    {
                                        ActivityId = activityId,
                                        UserId = userId,
                                        UserType = 1,
                                        State = 0,
                                        PrizesId = p.Id,
                                        UpdateTime = DateTime.Now,
                                        CreateTime = DateTime.Now
                                    }
                                );
                                //增加使用量
                                _DbSession.PrizesInfoStorager.PrizeMinus(p.Id);
                            }
                        }
                    }

                }             
            }
            return p;
        }
    }
}
