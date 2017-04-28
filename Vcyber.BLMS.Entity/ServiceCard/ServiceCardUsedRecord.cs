using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    /// <summary>
    /// 服务卡使用记录
    /// </summary>
    public class ServiceCardUsedRecord
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        public int RecordId { get; set; }

        /// <summary>
        /// 卡劵号
        /// </summary>
        public string CardNo { get; set; }

        /// <summary>
        /// 使用时间
        /// </summary>
        public DateTime UseTime { get; set; }

        /// <summary>
        /// 使用4S店   
        /// </summary>
        public string DealerId { get; set; }

        /// <summary>
        /// 消费记录ID
        /// </summary>
        public int ConsumeId { get; set; }

        /// <summary>
        /// 记录创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 消费类型（T01:4s店消费、T02：活动消费）
        /// </summary>
        public string ConsumeType { get; set; }

    }
}
