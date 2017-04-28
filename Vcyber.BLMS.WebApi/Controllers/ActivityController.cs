using AspNet.Identity.SQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.Common.Payment;
using Vcyber.BLMS.Common.Payment.Model;
using Vcyber.BLMS.Domain;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Common;
using Vcyber.BLMS.Entity.Member;
using Vcyber.BLMS.Entity.Weixin;
using Vcyber.BLMS.Common.Payment.Model.Request;
using Vcyber.BLMS.Common.Payment.Model.Response;
using System.Web.Http.Filters;

namespace Vcyber.BLMS.WebApi.Controllers
{
    /// <summary>
    /// 活动
    /// </summary>
    public class ActivityController : ApiController
    {
        /// <summary>
        /// 正在进行的活动
        /// </summary>
        /// <param name="activityId">活动Id，为0查询最新的活动</param>
        /// <returns>活动</returns>
        [HttpGet]
        [Route("api/activity/doing")]            
        public IHttpActionResult Doing(int activityId)
        {
            var obj = _AppContext.ActivityInfoApp.GetActivityInfoByID(activityId);            
            return this.Ok(new ReturnObject("200", "success", obj));
        }
        /// <summary>
        /// 根据活动名称查询活动
        /// </summary>
        /// <param name="name">活动名称</param>
        /// <returns>活动</returns>
        [HttpGet]
        [Route("api/activity/byname")]
        public IHttpActionResult ByName (string name)
        {
            var obj = _AppContext.ActivityInfoApp.GetActivityInfoByName(name);
            return this.Ok(new ReturnObject("200", "success", obj));
        }
        /// <summary>
        /// 用户是否参加活动
        /// </summary>
        /// <param name="activityId">活动ID</param>
        /// <param name="userId">用户ID</param>
        /// <returns>是或否</returns>
        [HttpGet]
        [Route("api/activity/isdrawbyactivityid")]
        public IHttpActionResult IsDrawByactivityId(int activityId,string userId)
        {            
            var flag = _AppContext.JoinActivityApp.IsUserJoinActivity(activityId, userId);               
            return this.Ok(new ReturnObject("200", "success", flag));
        }
        /// <summary>
        /// 根据活动和用户查询参加次数
        /// </summary>
        /// <param name="activityId">活动ID</param>
        /// <param name="userId">用户ID</param>
        /// <returns>数量</returns>
        [HttpGet]
        [Route("api/activity/getjoinactivityforcount")]
        public IHttpActionResult GetJoinActivityForCount(int activityId, string userId)
        {
            var count = _AppContext.JoinActivityApp.GetJoinActivityForCount(activityId, userId);
            return this.Ok(new ReturnObject("200", "success", count));
        }
        /// <summary>
        /// 活动下的奖品
        /// </summary>
        /// <param name="activityId">活动ID</param>
        /// <returns>奖品列表</returns>
        [HttpGet]
        [Route("api/activity/prizesbyactivityid")]
        public IHttpActionResult PrizesByActivityId(int activityId)
        {
            var query = _AppContext.PrizesInfoApp.GetPrizesInfosByActivityId(activityId);
            return this.Ok(new ReturnObject("200", "success", query));
        }
        /// <summary>
        /// 奖品明细
        /// </summary>
        /// <param name="prizeId">奖品ID</param>
        /// <returns>奖品详情</returns>
        [HttpGet]
        [Route("api/activity/prizebyid")]
        public IHttpActionResult PrizeById(int prizeId)
        {
            var obj = _AppContext.PrizesInfoApp.GetPrizeInfoMode(prizeId);
            return this.Ok(new ReturnObject("200", "success", obj));
        }
        /// <summary>
        /// 用户的中奖明细
        /// </summary>
        /// <param name="activityId">活动ID</param>
        /// <param name="userId">用户ID</param>
        /// <returns>用户的中奖明细</returns>
        [HttpGet]
        [Route("api/activity/winningsbyuserid")]
        public IHttpActionResult WinningsByUserID(int activityId,string userId)
        {
            var obj = _AppContext.WinningInfoApp.GetWinningByUserIdAndActicityId(activityId, userId);
            return this.Ok(new ReturnObject("200", "success", obj));
        }
        /// <summary>
        /// 用户的中奖明细
        /// </summary>
        /// <param name="activityId">活动ID</param>
        /// <param name="phone">电话</param>
        /// <returns>用户的中奖明细</returns>
        [HttpGet]
        [Route("api/activity/getwinningbytelandacticityid")]
        public IHttpActionResult GetWinningByTelAndActicityId(int activityId, string phone)
        {
            var obj = _AppContext.WinningInfoApp.GetWinningByTelAndActicityId(activityId, phone);
            return this.Ok(new ReturnObject("200", "success", obj));
        }
        /// <summary>
        /// 抽奖（用于机场服务活动）
        /// </summary>
        /// <param name="activityId">活动ID</param>
        /// <param name="userId">用户ID</param>
        /// <param name="province">用户选择省份</param>
        /// <returns>中奖的奖品</returns>
        [HttpGet]
        [Route("api/activity/tripdraw")]
        public IHttpActionResult TripDraw(int activityId, string userId,string province)
        {
            var activity = _AppContext.ActivityInfoApp.GetActivityInfoByID(activityId);
            //添加参加活动记录
            _AppContext.JoinActivityApp.AddJoinActivity(
                new JoinActivity() 
                { 
                    ActivityId = activityId,
                    UserId = userId,
                    Province = province,
                    CreateDate = DateTime.Now
                }
            );
            var rand = new Random();
            var activityRand = rand.Next(100);
            PrizesInfo p = new PrizesInfo();
            //活动中奖率，按百分比，随机数小于中奖率的可以中奖
            if (activityRand < activity.Probability)
            {
                //1、机场码
                //2、谢谢参与
                //3、旅行四件套
                //4、创意魔方旅行充
                //5、蓝牙自拍杆
                //6、单肩旅行包
                //7、景区门票
                //8、谢谢参与
                //9、索纳塔8钥匙扣
                var prizes = _AppContext.PrizesInfoApp.GetPrizesInfosByActivityId(activityId);
                var prizesUseForDay = _AppContext.WinningInfoApp.GetPrizeUse(activityId);
                var scenicTicket = prizes.FirstOrDefault(x => x.PrizeLevel == 7);
                //中奖
                var provinceArry = new string[] { "北京", "浙江", "河南", "广东" };
                //以上四地区优先中景区门票
                if (provinceArry.Contains(province))
                {
                    //有这个奖品并且还有库存
                    if (scenicTicket != null && scenicTicket.UsedNum < scenicTicket.TotalNum)
                    {
                        var scenicTicketUse = prizesUseForDay.FirstOrDefault(x => x.Id == scenicTicket.Id);
                        //如果今天没有出奖或者没有达到今天的出奖数量
                        if (scenicTicketUse == null || scenicTicketUse.CyclesUnuseNum < scenicTicket.CyclesNum)
                        {
                            p = scenicTicket;
                        }
                    }
                }
                //如果没有中门票，继续
                if (p.Id == 0)
                {
                    var waitDrawPrizes = new List<PrizesInfo>();
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
                                waitDrawPrizes.Add(prize);
                            }
                        }
                    }
                    //如果有可以抽的将其，抽奖
                    if (waitDrawPrizes.Count() > 0)
                    {
                        var prizeRand = rand.Next(waitDrawPrizes.Count());
                        p = waitDrawPrizes[prizeRand];
                    }
                }
                //开始创建中奖记录
                if (p.Id > 0)
                {
                    //添加中奖记录
                    _AppContext.WinningInfoApp.AddWinningInfo(
                        new WinningInfo()
                        {
                            ActivityId = activityId,
                            UserId = userId,
                            Province = province,
                            UserType = 1,
                            State = 0,
                            PrizesId = p.Id,
                            UpdateTime = DateTime.Now,
                            CreateTime = DateTime.Now
                        }
                    );
                    //增加使用量
                    _AppContext.PrizesInfoApp.PrizeMinus(p.Id);
                }
            }
            return this.Ok(new ReturnObject("200", "success", p));
        }

        /// <summary>
        /// 抽奖（根据活动的中奖率）
        /// </summary>
        /// <param name="activityId">活动ID</param>
        /// <param name="userId">用户ID</param>        
        /// <returns>中奖的奖品</returns>
        [HttpGet]
        [Route("api/activity/normaldraw")]
        public IHttpActionResult NormalDraw(int activityId, string userId)
        {
            IEnumerable<string> sourceKeys = null;
            var sourceKey = string.Empty;
            if (this.Request.Headers.TryGetValues("appkey", out sourceKeys))
            {
                sourceKey = sourceKeys.First();
            }
            var obj = _AppContext.ActivityInfoApp.NormalDraw(activityId, userId, sourceKey);
            return this.Ok(new ReturnObject("200", "success", obj));
        }
        /// <summary>
        /// 抽奖（感恩季活动）
        /// </summary>
        /// <param name="activityId">活动ID</param>
        /// <param name="userId">用户ID</param>        
        /// <returns>中奖的奖品</returns>
        [HttpGet]
        [Route("api/activity/thankdraw")]
        public IHttpActionResult ThankDraw(int activityId, string userId)
        {
            IEnumerable<string> sourceKeys = null;
            var sourceKey = string.Empty;
            if (this.Request.Headers.TryGetValues("appkey", out sourceKeys))
            {
                sourceKey = sourceKeys.First();
            }
            var obj = _AppContext.ActivityInfoApp.ThankDraw(activityId, userId, sourceKey);
            return this.Ok(new ReturnObject("200", "success", obj));
        }

        /// <summary>
        /// 修改中奖记录
        /// </summary>
        /// <param name="activityId">活动ID</param>
        /// <param name="userId">用户ID</param>
        /// <param name="prizesId">奖项ID</param>
        /// <param name="userName">姓名</param>
        /// <param name="userTel">电话</param>
        /// <param name="province">省份</param>
        /// <param name="city">城市</param>
        /// <param name="address">地址</param>
        /// <returns>成功是失败否</returns>
        [HttpGet]
        [Route("api/activity/updatewinnings")]
        public IHttpActionResult UpdateWinnings(int activityId, string userId, int prizesId, string userName, string userTel, string province, string city,string address)
        {
            if (string.IsNullOrEmpty(userTel))
            {
                var store = new FrontUserStore<FrontIdentityUser>();
                var user = store.FindByIdAsync(userId).Result;
                userName = user.RealName;
                userTel = user.UserName;
            }
            var obj = new WinningInfo() 
            { 
                ActivityId = activityId,
                UserId = userId,
                PrizesId = prizesId,
                UserName = userName,
                UserTel = userTel,
                Province = province,
                City = city,
                Address = address
            };
            var flag = _AppContext.WinningInfoApp.UpdateForUserId(obj);            
            return this.Ok(new ReturnObject("200", "success", flag));
        }
        /// <summary>
        /// 修改中奖记录
        /// </summary>
        [HttpGet]
        [Route("api/activity/updatewinningsforid")]
        public IHttpActionResult UpdateWinningsForId(int id, string userName, string phone, string address)
        {
            var flag = _AppContext.WinningInfoApp.UpdateForId(id, userName, phone, address);
            return this.Ok(new ReturnObject("200", "success", flag));
        }

        /// <summary>
        /// 获取机场券
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="phone">手机号</param>
        /// <returns>券码</returns>
        [HttpGet]
        [Route("api/activity/getaircard")]
        public IHttpActionResult GetAirCard(string userId, string phone = "")
        {
            //构建短信内容
            string smsContent = @"恭喜您在金秋乐享，蓝缤出行“游”大礼活动中获得北京现代bluemembers提供的空港易行候机服务，
您的候机服务码为为{0}，持该服务码到全国50家机场指定休息室均可使用，到达指定机场休息室出示此服务码短信或App、及微信候机服务二维码，可享1人次免费候机服务。
如需查询休息室位置请点击http://dwz.cn/46ztqQ 或关注“北京现代bluemembers”微信服务号。";
            SmsApp sms = new SmsApp();
            var result = new ReturnObject("200", "success", null);
            //如果phone为空，则为username
            if (string.IsNullOrEmpty(phone))
            {
                var store = new FrontUserStore<FrontIdentityUser>();
                var user = store.FindByIdAsync(userId).Result;
                phone = user.UserName;
            }
            //本次要获取的串码数据
            IList<string> cardCodeQuery = new List<string>();
            for (var i = 0; i < 2; i++)
            {
                var cardCode = _AppContext.AirportServiceApp.SendCardByActivity(phone, userId);
                if (!string.IsNullOrEmpty(cardCode))
                {
                    cardCodeQuery.Add(cardCode);
                    //发送串码短信               
                    var smsResult = sms.SendSMS(phone, string.Format(smsContent, cardCode),false);
                    //短信发送失败
                    if (!smsResult.IsSuccess)
                    {
                        result.Code = "400";
                        result.Message = "短信发送失败";
                    }   
                }
            }
            if (result.Code == "200")
            {
                result.Content = cardCodeQuery;

            }            
            return this.Ok(result);
        }
        /// <summary>
        /// 获取所中奖记录
        /// </summary>
        /// <param name="activityId">活动ID</param>
        /// <param name="userId">用户ID</param>
        /// <param name="prizeId">奖项ID</param>
        /// <returns>中奖记录</returns>
        [HttpGet]
        [Route("api/activity/getwinprize")]
        public IHttpActionResult GetWinPrize(int activityId, string userId, int prizeId) 
        {
            var obj = _AppContext.WinningInfoApp.GetWinPrize(activityId, userId, prizeId);
            return this.Ok(new ReturnObject("200", "success", obj));
        }
        /// <summary>
        /// 获取所中奖记录
        /// </summary>
        /// <param name="activityId">活动ID</param>
        /// <param name="userId">用户ID</param>
        /// <returns>中奖记录</returns>
        [HttpGet]
        [Route("api/activity/getwinbyuserid")]
        public IHttpActionResult GetWinByUserId(int activityId, string userId)
        {
            var query = _AppContext.WinningInfoApp.GetWinningsByWhere(activityId, string.Format("wi.UserId = '{0}' and wi.CreateTime > '2017-03-14 00:00:00'", userId));
            return this.Ok(new ReturnObject("200", "success", query));
        }

        /// <summary>
        /// 获取参加活动的数量
        /// </summary>
        /// <param name="activityType">活动名称</param>
        /// <returns>成功是失败否</returns>
        [HttpGet]
        [Route("api/activity/wincount")]
        public IHttpActionResult WinCount(string activityType)
        {
            var obj = _AppContext.ActivityInfoApp.GetActivityInfoByName(activityType);
            var count = _AppContext.WinningInfoApp.GetWinningsCount(obj.Id);
            return this.Ok(new ReturnObject("200", "success", count));
        }

        /// <summary>
        /// 添加分享记录
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="source">来源</param>
        /// <returns>成功是失败否</returns>
        [HttpGet]
        [Route("api/activity/addsharerecord")]
        public IHttpActionResult AddShareRecord(string userId, string source = "blms_wechat")
        {
            var activity = _AppContext.ActivityInfoApp.GetActivityInfoByID(0);
            var activityId = activity == null ? 0 : activity.Id;
            var shareObj = new ShareRecord()
            {
                Source = source,
                ShareType = "weixin",
                UserId = userId,
                ActivityId = activityId,
                CreateTime = DateTime.Now
            };
            var flag = _AppContext.ShareRecordApp.AddShareRecord(shareObj);
            return this.Ok(new ReturnObject("200", "success", flag));
        }
        /// <summary>
        /// 添加分享记录
        /// </summary>
        /// <param name="querys">好友列表</param>
        /// <returns>成功是失败否</returns>
        [HttpPost]
        [Route("api/activity/addrecommend")]
        public IHttpActionResult AddRecommend(List<Recommend> querys)
        {
            SmsApp sms = new SmsApp();
            string smsContent = "尊敬的{0}，您的好友{1}推荐您购买北京现代全新悦动，即刻试驾，一荐心动！{2}";
            var recommendId = 0;
            foreach (var obj in querys)
            {
                recommendId = _AppContext.RecommendApp.Add(obj);
                //发送短信                               
                sms.SendSMS(obj.PhoneNumber, string.Format(smsContent, obj.Name, obj.UserName,obj.Url), false);
            }
            return this.Ok(new ReturnObject("200", "success", recommendId));
        }
        /// <summary>
        /// 获取推荐详情
        /// </summary>
        /// <param name="id">推荐Id</param>
        /// <returns>详情</returns>
        [HttpGet]
        [Route("api/activity/getrecommend")]
        public IHttpActionResult GetRecommend(int id)
        {
            var obj = _AppContext.RecommendApp.Find(id);
            return this.Ok(new ReturnObject("200", "success", obj));
        }
        /// <summary>
        /// 获取推荐的数量
        /// </summary>
        /// <param name="activityType">用户ID</param>
        /// <returns>成功是失败否</returns>
        [HttpGet]
        [Route("api/activity/recommendcount")]
        public IHttpActionResult RecommendCount(string activityType)
        {
            var count = _AppContext.RecommendApp.Count(activityType);
            return this.Ok(new ReturnObject("200", "success", count));
        }
        /// <summary>
        /// 推荐手机号是否重复
        /// </summary>
        /// <param name="phone">手机号</param>
        /// <returns>是否</returns>
        [HttpGet]
        [Route("api/activity/isphoneexist")]        
        public bool IsPhoneExist(string phone)
        {
            return _AppContext.RecommendApp.IsPhoneExist(phone);
        }
        /// <summary>
        /// 获取卡券
        /// </summary>
        /// <param name="activityId">活动Id</param>
        /// <param name="userId">用户ID</param>
        /// <param name="prizesId">奖项Id</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/activity/getcardnumber")]
        public IHttpActionResult GetCardNumber(int activityId, string userId, int prizesId)
        {
            var obj = _AppContext.WinningInfoApp.GetWinPrize(activityId, userId, prizesId);
            var cardTypes = new List<string>();
            IEnumerable<string> sourceKeys = null;
            string sourceKey = string.Empty;
            if (this.Request.Headers.TryGetValues("appkey", out sourceKeys))
            {
                sourceKey = sourceKeys.First();
            }
            if (obj != null)
            {
                var prizeObj = _AppContext.PrizesInfoApp.GetPrizeInfoMode(obj.PrizesId);
                switch (prizeObj.PrizeLevel)
                { 
                    case 1:
                        cardTypes.Add("车内杀菌消毒券");                        
                        break;
                    case 2:
                        cardTypes.Add("2瓶玻璃水兑换券");                        
                        break;
                    case 3:
                        cardTypes.Add("50元保养代金券");                        
                        break;
                    case 4:
                        cardTypes.Add("途家150元代金券");
                        break;
                }                
                foreach (var cardType in cardTypes)
                {
                    _AppContext.ProductApp.SendCard(userId, "2017春季服务活动", cardType, 1, sourceKey);                
                }                
            }
            return this.Ok(new ReturnObject("200", "success", cardTypes));
        }
        /// <summary>
        /// 根据卡券名称下发卡券
        /// </summary>
        [HttpGet]
        [Route("api/activity/sendcardbyname")]
        public IHttpActionResult SendCardByName(string userId, string activityType,string cardName)
        {           
            IEnumerable<string> sourceKeys = null;
            string sourceKey = string.Empty;
            if (this.Request.Headers.TryGetValues("appkey", out sourceKeys))
            {
                sourceKey = sourceKeys.First();
            }
            var result = "302";
            var customCardQuery = _AppContext.CustomCardApp.GetUserCustomCardByActivityType(userId, activityType);
            if (customCardQuery.Count == 0)
            {
                _AppContext.ProductApp.SendCard(userId, activityType, cardName, 1, sourceKey);
                result = "200";
            }
            return this.Ok(new ReturnObject("200", "success", result));
        }
        /// <summary>
        /// 补发卡券,只针对悦纳活动和双十一活动补发，后期可删除
        /// </summary>
        [HttpGet]
        [Route("api/activity/reissuecard")]
        public IHttpActionResult ReissueCard(string userId)
        {
            if (DateTime.Now >= DateTime.Parse("2016-11-10 00:00:00"))
            {
                var redPackObj = _AppContext.RedPackApp.BySceneId("161111");//161111为本次抢红包的场景值
                var redPackRedcordForCard = _AppContext.RedPackApp.RedPacCardByUserId(redPackObj.Id, userId);
                if (redPackRedcordForCard != null)
                {
                    var customCardQuery = _AppContext.CustomCardApp.GetUserCustomCardByActivityType(redPackRedcordForCard.UserId, "双11活动");
                    if (customCardQuery.Count == 0)
                    {
                        _AppContext.ProductApp.SendCard(redPackRedcordForCard.UserId, "双11活动", redPackRedcordForCard.CardName, 1, "mt_send");
                    }
                }                
            }
            return this.Ok(new ReturnObject("200", "success", 0));
        }
        /// <summary>
        /// 红包处理流程
        /// </summary>
        [HttpGet]
        [Route("api/activity/processredpack")]
        public IHttpActionResult ProcessRedPack()
        {
            var message = "处理程序正在运行";
            var isStartProcess = "nov:isstartprocess";
            try
            {                                
                log4net.LogManager.GetLogger("nov2016-proc").Info("-----处理程序开始执行");
                var packListKey = string.Format("nov:packqueue");
                var redisExtendProcess = new RedisExtend();
                using (redisExtendProcess)
                {
                    redisExtendProcess.Connect();
                    var isStartProcessValue = redisExtendProcess.GetValueString(isStartProcess);
                    IEnumerable<string> sourceKeys = null;
                    string sourceKey = string.Empty;
                    if (this.Request.Headers.TryGetValues("appkey", out sourceKeys))
                    {
                        sourceKey = sourceKeys.First();
                    }
                    if (isStartProcessValue == null || isStartProcessValue == "0")
                    {
                        log4net.LogManager.GetLogger("nov2016-proc").Info("-----异步没有运行，新起一个异步");
                        var redPackObj = _AppContext.RedPackApp.BySceneId("161111");//161111为本次抢红包的场景值
                        //触发处理程序
                        Task t = new Task(() =>
                        {
                            try
                            {
                                var redisExtendTask = new RedisExtend();
                                using (redisExtendTask)
                                {
                                    redisExtendTask.Connect();
                                    log4net.LogManager.GetLogger("nov2016-proc").Info("-----异步任务开始执行");
                                    redisExtendTask.Set<string>(isStartProcess, "1");
                                    while (true)
                                    {
                                        var strPack = redisExtendTask.BrPop(packListKey);
                                        if (string.IsNullOrEmpty(strPack))
                                        {
                                            log4net.LogManager.GetLogger("nov2016-proc").Info("-----队列里没有对象了。");
                                            redisExtendTask.Set<string>(isStartProcess, "0");
                                            break;
                                        }
                                        log4net.LogManager.GetLogger("nov2016-proc").Info(string.Format("-----队列里对象为：{0}。", strPack));
                                        var packArray = strPack.Split('|');//用户ID|OpenId|金额|奖品等级|IP
                                        //发红包
                                        RequestGiveFund reqRedPack = new RequestGiveFund();
                                        reqRedPack.openid = packArray[1];
                                        reqRedPack.amount = int.Parse(packArray[2]);
                                        reqRedPack.spbill_create_ip = packArray[4];
                                        reqRedPack.desc = "北京现代bluemembers双11活动";
                                        var userId = packArray[0];
                                        var prizeLevel = int.Parse(packArray[3]);
                                        IPayment pay = PaymentFactory.GetInstance(PaymentType.weixin);
                                        var rspRedPack = pay.GiveFund(reqRedPack);
                                        log4net.LogManager.GetLogger("nov2016-proc").Info(string.Format("-----红包发送完毕，消息：{0}。", rspRedPack.result_code));
                                        //查询该用户之前领取的卡券
                                        var redPackRedcordForCard = _AppContext.RedPackApp.RedPacCardByUserId(redPackObj.Id, userId);
                                        var cardName = string.Empty;
                                        //发卡券
                                        if (redPackRedcordForCard == null)
                                        {
                                            switch (prizeLevel)
                                            {
                                                case 1:
                                                    cardName = "2瓶玻璃水兑换券";
                                                    break;
                                                case 2:
                                                    cardName = "维新组合兑换券";
                                                    break;
                                                case 3:
                                                    cardName = "55元维保抵用券";
                                                    break;
                                            }
                                            _AppContext.ProductApp.SendCard(userId, "双11活动", cardName, 1, sourceKey);
                                        }
                                        else
                                        {
                                            log4net.LogManager.GetLogger("nov2016-proc").Info("-----已经领取过卡券了-----");
                                        }
                                        //添加红包领取记录
                                        var redPackRecord = new RedPackRecord();
                                        redPackRecord.RedPackId = redPackObj.Id;
                                        redPackRecord.CardName = cardName;
                                        redPackRecord.UserId = packArray[0];
                                        redPackRecord.OpenId = reqRedPack.openid;
                                        redPackRecord.Amount = reqRedPack.amount;
                                        redPackRecord.TradeNo = rspRedPack.partner_trade_no;
                                        redPackRecord.PaymentNo = rspRedPack.payment_no;
                                        redPackRecord.ResultCode = rspRedPack.result_code;
                                        redPackRecord.ErrorCode = rspRedPack.err_code;
                                        redPackRecord.Source = sourceKey;
                                        redPackRecord.PaymentTime = string.IsNullOrEmpty(rspRedPack.payment_time) ? DateTime.Now : DateTime.Parse(rspRedPack.payment_time);
                                        redPackRecord.CreateTime = DateTime.Now;
                                        _AppContext.RedPackApp.AddRecord(redPackRecord);
                                        log4net.LogManager.GetLogger("nov2016-proc").Info("-----红包记录添加到数据库");
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                log4net.LogManager.GetLogger("nov2016-proc").Error(ex);
                                var redisExtendCatch = new RedisExtend();
                                using (redisExtendCatch)
                                {
                                    redisExtendCatch.Connect();
                                    redisExtendCatch.Set<string>(isStartProcess, "0");
                                }                                
                            }
                        });
                        t.Start();
                        message = "处理程序开始运行";
                    }
                    else
                    {
                        log4net.LogManager.GetLogger("nov2016-proc").Info("-----异步正在运行，本次请求作废");
                    }
                }
            }
            catch (Exception ex)
            {
                message = "处理程序还没开始就报错了。";     
                log4net.LogManager.GetLogger("nov2016-proc").Error(ex);
                var redisExtendProcessCatch = new RedisExtend();
                using (redisExtendProcessCatch)
                {
                    redisExtendProcessCatch.Connect();
                    redisExtendProcessCatch.Set<string>(isStartProcess, "0");                    
                }                                       
            }
            return this.Ok(new ReturnObject("200", "success", message));
        }
        /// <summary>
        /// 添加领奖信息
        /// </summary>
        /// <param name="activityId">活动ID</param>
        /// <param name="userId">用户ID</param>
        /// <param name="prizesId">奖品ID</param>
        /// <param name="userName">用户姓名</param>
        /// <param name="userTel">用户手机号</param>
        /// <param name="province">收货省</param>
        /// <param name="city">收货市</param>
        /// <param name="address">收货地址</param>
        /// <param name="name">收货地区</param>
        /// <returns>成功或失败</returns>
        [HttpGet]
        [Route("api/activity/addWinnings")]
        public IHttpActionResult AddWinnings(int activityId, string userId, int prizesId, string userName, string userTel, string province, string city, string address,string name)
        {
            var query = _AppContext.PrizesInfoApp.GetPrizesUsedNumByActivity(activityId);
            var obj = query.FirstOrDefault(x => x.Id == prizesId);
            var flag = "false";
            if (obj != null)
            {
                if ((obj.TotalNum - obj.UsedNum) > 0)
                {
                    //添加中奖记录
                    _AppContext.WinningInfoApp.AddWinningInfo(
                        new WinningInfo()
                        {
                            ActivityId = activityId,
                            UserId = userId,
                            Province = province,
                            UserType = 1,
                            State = 1,
                            PrizesId = prizesId,
                            UserName = userName,
                            UserTel = userTel,
                            City = city,
                            Address = address,
                            Area = name,
                            UpdateTime = DateTime.Now,
                            CreateTime = DateTime.Now
                        }
                    );
                    flag = "true";
                }
            }      
           return this.Ok(new ReturnObject("200", flag));
            
        }
        /// <summary>
        /// 获取景区门票中奖记录
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/activity/GetPrizesUsedNumByActivity")]
        public IHttpActionResult GetPrizesUsedNumByActivity(int activityId)
        {
            var query = _AppContext.PrizesInfoApp.GetPrizesUsedNumByActivity(activityId);
            return this.Ok(new ReturnObject("200", "success", query));
        }

        [HttpGet]
        [Route("api/activity/updatePrizes")]
        public IHttpActionResult updatePrizes(int id)
        {

            //添加中奖记录
            //var query = _AppContext.PrizesInfoApp.GetPrizesInfosByActivityId(activityId);
            _AppContext.PrizesInfoApp.PrizeMinus(id);
            //_AppContext.PrizesInfoApp.updatePrizes(
            //    new PrizesInfo()
            //    {
            //        ActivityId = activityId,
            //        Id = id,
            //        UpdateTime = DateTime.Now,
            //        CreateTime = DateTime.Now
            //    }
            //);
            return this.Ok(new ReturnObject("200", "true"));

        }
    }
}
