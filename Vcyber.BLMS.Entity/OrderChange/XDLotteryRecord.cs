using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    public class XDLotteryRecord
    {
        /// <summary>
        /// 奖品记录ID
        /// </summary>
        public int LotteryRecordId { set; get; }
        /// <summary>
        /// 奖品id
        /// </summary>
        public int LotteryId { set; get; }
        /// <summary>
        /// 活动id
        /// </summary>
        public int ActivityId { set; get; }
        /// <summary>
        /// 活动名
        /// </summary>
        public string ActivityName { set; get; }
        /// <summary>
        /// 奖品名
        /// </summary>
        public string LotteryName { set; get; }
        /// <summary>
        /// 奖品类型
        /// </summary>
        public int LotteryType { set; get; }
        /// <summary>
        /// 中奖来源
        /// </summary>
        public string LotteryRecordSource { set; get; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId { set; get; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { set; get; }
        /// <summary>
        /// 用户手机号
        /// </summary>
        public string UserMobile { set; get; }
        /// <summary>
        /// 抽奖时间
        /// </summary>
        public DateTime LotteryRecordTime { set; get; }
        /// <summary>
        /// 用户ip
        /// </summary>
        public string UserIp { set; get; }
        /// <summary>
        /// 是否有效（0：无效，1：有效）
        /// </summary>
        public int IsValid { set; get; }
        /// <summary>
        /// 创建人id
        /// </summary>
        public string CreaterId { set; get; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string CreaterName { set; get; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreaterTime { set; get; }
        /// <summary>
        /// 修改人id
        /// </summary>
        public string UpdaterId { set; get; }
        /// <summary>
        /// 修改人
        /// </summary>
        public string UpdaterName { set; get; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime UpdaterTime { set; get; }
    }
}
