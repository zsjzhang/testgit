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
    public class PrizesInfoApp : IPrizesInfoApp
    {
        public List<PrizesInfo> GetPrizesInfosByActivityId(int activityId)
        {
            return _DbSession.PrizesInfoStorager.GetPrizesInfosByActivity(activityId).ToList();
        }
        /// <summary>
        /// 获取景区门票中奖记录
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns></returns>
        public List<PrizesInfo> GetPrizesUsedNumByActivity(int activityId)
        {
            return _DbSession.PrizesInfoStorager.GetPrizesUsedNumByActivity(activityId).ToList();
        }

        public bool AddPrizesInfo(PrizesInfo entity)
        {
            int offsetNum = _DbSession.PrizesInfoStorager.AddPrizesInfo(entity);
            if (offsetNum > 0) return true;
            else return false;
        }

        public ActivityResult StartActivityInfo(string userId, int activityId, string source, string vin)
        {
            ActivityResult result = new ActivityResult();

            Membership user = _DbSession.PrizesInfoStorager.GetUserInfoById(userId);
            int level = 0;
            int.TryParse(user.MLevel, out level);
            if (level <= 1)
            {
                result.Info(-2, "非车主会员不能参加");
                return result;
            }


            //判断是否参加过此活动
            bool isJoin = _DbSession.JoinActivityStorager.IsUserJoinActivity(activityId, userId);
            if (isJoin)
            {
                result.Info(-1, "已经参加该活动");
                return result;
            }
            //添加参加活动记录
            JoinActivity joinEntity = new JoinActivity();
            joinEntity.Join(userId, activityId, source, vin);
            int joinId = _DbSession.JoinActivityStorager.AddJoinActivity(joinEntity);

            //获取所有的奖项
            PrizesInfo prize = new PrizesInfo();
            IEnumerable<PrizesInfo> prizesList = _DbSession.PrizesInfoStorager.GetPrizesInfoNotNull(activityId);
            prizesList = FilterPrizeInfo(user, prizesList);
            decimal[] rateArray = prizesList.Select(p => p.Rate).ToArray();
            int range = ProbabilityRange(rateArray);
            int radix = ProbabilityRadix(rateArray);
            int randNum = ProbabilityRandom(0, range);
            int rateSum = 0;
            foreach (var item in prizesList)
            {
                rateSum += (int)(item.Rate * radix);
                if (randNum <= rateSum)
                {
                    prize = item;
                    break;
                }
            }
            //修改奖品数量
            prize.WinningPrize();
            _DbSession.PrizesInfoStorager.UpdatePrizesInfo(prize);
            if (prize.PrizeFlag == 0)
            {
                //蓝豆增加
                UserblueBean ub = new UserblueBean();
                ub.Grow(userId, Entity.Enum.EBRuleType.定期活动, Convert.ToInt32(prize.Price));
                _DbSession.UserBlueBeanStorager.Add(ub);
            }

            //添加获奖记录
            WinningInfo winning = new WinningInfo();
            winning.WinningPrize(user, activityId, prize.Id);
            int winningId = _DbSession.WinningInfoStorager.AddWinningInfo(winning);
            winning = _DbSession.WinningInfoStorager.GetWinningInfo(winningId);

            result.Info(prize.PrizeFlag, "抽中奖品");
            result.Result(prize, winning);
            return result;
        }


        /// <summary>
        /// 根据新老会员、是否有车会员筛选奖品
        /// </summary>
        /// <returns></returns>
        private IEnumerable<PrizesInfo> FilterPrizeInfo(Membership user, IEnumerable<PrizesInfo> prizesList)
        {
            IEnumerable<PrizesInfo> list = prizesList;
            string date = ConfigurationManager.AppSettings["NewMemberExpiryDate"];
            DateTime expriyDate = Convert.ToDateTime(date);
            if (Convert.ToDateTime(user.CreateTime) < expriyDate)
            {
                //老会员
                prizesList = prizesList.Where(p => p.PrizeLevel < 4);
            }
            return prizesList;
        }

        /// <summary>
        /// 获取概率的范围
        /// </summary>
        /// <param name="rateArray"></param>
        /// <returns></returns>
        private int ProbabilityRange(decimal[] rateArray)
        {
            int range = 0;
            int radix = ProbabilityRadix(rateArray);
            foreach (decimal item in rateArray)
            {
                range += (int)(item * radix);
            }
            return range;
        }

        /// <summary>
        /// 获取概率的基数
        /// </summary>
        /// <param name="rateArray"></param>
        private int ProbabilityRadix(decimal[] rateArray)
        {
            decimal minRate = rateArray.Min();

            int len = 0;
            foreach (decimal item in rateArray)
            {
                string temp = item.ToString();
                if (!temp.Contains('.'))
                {
                    continue;
                }
                temp = temp.Substring(temp.IndexOf('.')).Replace(".", "");
                len = temp.Length > len ? temp.Length : len;
            }
            return (int)Math.Pow(10, len);
        }

        /// <summary>
        /// 获取随机数
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        private int ProbabilityRandom(long min, long max)
        {
            Random random = new Random(Guid.NewGuid().GetHashCode());
            byte[] minArr = BitConverter.GetBytes(min);
            int hMin = BitConverter.ToInt32(minArr, 4);
            int lMin = BitConverter.ToInt32(new byte[] { minArr[0], minArr[1], minArr[2], minArr[3] }, 0);
            byte[] maxArr = BitConverter.GetBytes(max);
            int hMax = BitConverter.ToInt32(maxArr, 4);
            int lMax = BitConverter.ToInt32(new byte[] { maxArr[0], maxArr[1], maxArr[2], maxArr[3] }, 0);
            if (random == null)
            {
                random = new Random();
            }
            int h = random.Next(hMin, hMax);
            int l = 0;
            if (h == hMin)
            {
                l = random.Next(Math.Min(lMin, lMax), Math.Max(lMin, lMax));
            }
            else
            {
                l = random.Next(0, Int32.MaxValue);
            }
            byte[] lArr = BitConverter.GetBytes(l);
            byte[] hArr = BitConverter.GetBytes(h);
            byte[] result = new byte[8];
            for (int i = 0; i < lArr.Length; i++)
            {
                result[i] = lArr[i];
                result[i + 4] = hArr[i];
            }
            return BitConverter.ToInt32(result, 0);
        }


        public bool UpdatePrizesInfo(PrizesInfo entity)
        {
            return _DbSession.PrizesInfoStorager.UpdatePrizesInfo(entity);
        }


        public PrizesInfo GetPrizeInfoMode(int prizeId)
        {
            return _DbSession.PrizesInfoStorager.GetPrizeInfoMode(prizeId);
        }

        public Membership GetMembershipMode(string phone)
        {
            return _DbSession.PrizesInfoStorager.GetMembershipMode(phone);
        }


        public List<PrizesInfo> GetPrizesInfosByActivityId(int activityId, PageData pageData, out int total)
        {
            return _DbSession.PrizesInfoStorager.GetPrizesInfosByActivity(activityId, pageData, out total).ToList();
        }


        public bool DelPrizesInfo(int id)
        {
            return _DbSession.PrizesInfoStorager.DelPrizesInfo(id) > 0;
        }


        public bool CutDownPrizeCyclesUnuseNum(int prizeId, int activityId, int num)
        {
            return _DbSession.PrizesInfoStorager.CutDownPrizeCyclesUnuseNum(prizeId, activityId, num);
        }

        public int GetCyclesUnuseNumById(int prizeId, int activityId)
        {
            return _DbSession.PrizesInfoStorager.GetCyclesUnuseNumById(prizeId, activityId);
        }
        /// <summary>
        /// 奖品减库存
        /// </summary>
        /// <param name="id">奖品ID</param>
        public void PrizeMinus(int id) 
        {
            _DbSession.PrizesInfoStorager.PrizeMinus(id);
        }

       
    }
}
