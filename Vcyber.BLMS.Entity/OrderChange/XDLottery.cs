using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    public class XDLottery
    {
        /// <summary>
        /// 奖品ID
        /// </summary>
        public int LotteryId { set; get; }
        /// <summary>
        /// 活动id
        /// </summary>
        public int ActivityId { set; get; }
        /// <summary>
        /// 奖品编码
        /// </summary>
        public string LotteryCode { set; get; }
        /// <summary>
        /// 奖品名
        /// </summary>
        public string LotteryName { set; get; }
        /// <summary>
        /// 奖品图片
        /// </summary>
        public string LotteryImage { set; get; }
        /// <summary>
        /// 奖品数量
        /// </summary>
        public int LotteryCount { set; get; }
        /// <summary>
        /// 奖品剩余数量
        /// </summary>
        public int LotteryBalanceCount { set; get; }
        /// <summary>
        /// 奖品类型
        /// </summary>
        public int LotteryType { set; get; }
        /// <summary>
        /// 中奖概率
        /// </summary>
        public int LotteryRate { set; get; }
        /// <summary>
        /// 奖品状态
        /// </summary>
        public int LotteryStatus { set; get; }
        /// <summary>
        /// 奖品投放位置
        /// </summary>
        public int LotteryPutType { set; get; }
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
        /// <summary>
        /// 奖品位置
        /// </summary>
        public string LotteryPosition { set; get; }
        /// <summary>
        /// 中奖记录ID（临时用）
        /// </summary>
        public int LotteryRecordId { set; get; }
    }
}
