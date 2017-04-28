using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    public class CustomerServiceRecord
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 客服编号
        /// </summary>
        public string Worker { get; set; }
        /// <summary>
        /// 接收消息数量
        /// </summary>
        public int ReceiveCount { get; set; }
        /// <summary>
        /// 回复消息数量
        /// </summary>
        public int ReplyCount { get; set; }
        /// <summary>
        /// 接收消息的人数
        /// </summary>
        public int ReceivePersons { get; set; }
        /// <summary>
        /// 回复消息的人数
        /// </summary>
        public int ReplyPersons { get; set; }
        /// <summary>
        /// 响应时长
        /// </summary>
        public int Mins { get; set; }
        /// <summary>
        /// 首次响应时长
        /// </summary>
        public int FirstMins { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 查询时间段
        /// </summary>
        public string BetweenTime { get; set; }
    }
}
