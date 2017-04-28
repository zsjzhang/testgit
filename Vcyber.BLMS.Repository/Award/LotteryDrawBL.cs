using AspNet.Identity.SQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Repository;

namespace Vcyber.BLMS.Repository
{
    public class LotteryDrawBL
    {
        static Random rdom = new Random();

        /// <summary>
        /// 抽奖
        /// </summary>
        public LotteryDrawPool Execute(LotteryDrawPoolType type, int probability)
        {
            LotteryDrawPoolRepository repository = new LotteryDrawPoolRepository();
            List<LotteryDrawPool> pool = repository.GetLotteryDrawPool(type).ToList();
            pool.All((e) =>
            {
                if (!string.IsNullOrEmpty(e.Probability))
                {
                    if (e.Probability.Split(',').Count() > 1)
                    {
                        e.ProbabilityLeft = Convert.ToInt32(e.Probability.Split(',')[0]);
                        e.ProbabilityRight = Convert.ToInt32(e.Probability.Split(',')[1]);
                    }
                }
                return true;
            });

            if (pool != null && pool.Count > 0)
            {
                int r = rdom.Next(100);
                LogService.Instance.Info(string.Format("奖池类型：{0}, 随机数：{1}", type, r));
                if (r < probability)//中奖
                {
                    int pr = rdom.Next(pool.Max(q => q.ProbabilityRight).Value);
                    int hitIndex = 0;
                    pool.All((e) =>
                    {
                        if (e.ProbabilityLeft <= pr && e.ProbabilityRight > pr)
                        {
                            return false;
                        }
                        hitIndex++;
                        return true;
                    });
                    return pool[2];
                }
            }
            return null;
        }

        /// <summary>
        /// 更新奖品池数量
        /// </summary>
        /// <param name="aw"></param>
        /// <returns></returns>
        public int UpdateLotteryDrawPool(int PoolId, int VersionNumber)
        {
            LotteryDrawPoolRepository repository = new LotteryDrawPoolRepository();
            return repository.UpdateLottery(PoolId, VersionNumber);
        }

        /// <summary>
        /// 虚拟奖品需要立即发放
        /// </summary>
        /// <param name="aw"></param>
        /// <param name="openid"></param>
        public bool SendVisualAward(LotteryDrawPool LDP, string openid)
        {
            bool sendResult = false;
            if (LDP.Award.VisualProductType == VisualProductType.AirportTicket)
            {
                //机场券
                WxBindNewRepository webing = new WxBindNewRepository();
                var bindUser = webing.GetUser(openid);

                SNCardStorager snstorage = new SNCardStorager();
                SNCard card = snstorage.GetSNCard(1).First();
                sendResult = snstorage.SendSNCard(bindUser.UserId, card.Id, 4, 0, bindUser.PhoneNumber, "blms_wechat");
                if(!sendResult)
                {
                    LogService.Instance.Error(string.Format("机场券发送失败，用户id:{0}, 手机号:{1}, SNCardID:{2}", bindUser.UserId, bindUser.PhoneNumber, card.Id));
                }
            }
            if (LDP.Award.VisualProductType == VisualProductType.WxCard)
            {
                //调用微信发卡券
                WXCardRepository wxRepository = new WXCardRepository();
                SendCard sendCard = new SendCard
                {
                    msgtype = "wxcard",
                    touser = openid,
                    wxcard = new WXCard { card_id = LDP.CardId, card_ext = new Card_ext() }
                };
                sendResult = wxRepository.SendWXCard(sendCard);
                if (!sendResult)
                {
                    LogService.Instance.Error(string.Format("微信卡券发送失败，卡券ID:{0}, LDP Id: {1}, OpenId:{2}", LDP.CardId, LDP.Id, openid));
                }
            }
            return sendResult;
        }

        /// <summary>
        /// 奖品发放记录
        /// </summary>
        /// <param name="LDP"></param>
        /// <param name="entity"></param>
        public int AwardSendRecord(LotteryDrawPool LDP, string openid, bool? visualCardSendState)
        {
            AwardSendRecordRepository repository = new AwardSendRecordRepository();
            AwardSendRecord record = new AwardSendRecord { OpenId = openid, Award = LDP.Award, AwardId = LDP.AwardId, LotteryDrawPoolId = LDP.Id, SendState = visualCardSendState == true ? 1 : 0 };
            return repository.AddRecord(record);
        }

        public LotteryDrawPool GetVersionNumber(int id)
        {
            LotteryDrawPoolRepository repository = new LotteryDrawPoolRepository();
            return repository.GetById(id);
        }

    }
}
