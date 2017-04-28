using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    /// <summary>
    /// 服务卡批次
    /// </summary>
    public class ServiceCardBatch
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        public int BatchId { get; set; }

        /// <summary>
        /// 卡劵类型
        /// </summary>
        public string TypeCode { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>
        public string BatchNo { get; set; }

        /// <summary>
        /// 批次名称（卡劵名称）
        /// </summary>
        public string BatchName { get; set; }

        /// <summary>
        /// 批次生成数量
        /// </summary>
        public int BatchQty { get; set; }

        /// <summary>
        /// 卡劵面值
        /// </summary>
        public decimal BatchPrice { get; set; }

        /// <summary>
        /// 批次总金额
        /// </summary>
        public decimal BatchTotalMoney { get; set; }

        /// <summary>
        /// 生成时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 最后有效期
        /// </summary>
        public DateTime ValidateTime { get; set; }

        /// <summary>
        /// 所需消费金额
        /// </summary>
        public decimal? ConsumeMoney { get; set; }

        /// <summary>
        /// 可使用的4S店
        /// </summary>
        public string DearlerId { get; set; }

        #region ==== 附加属性 ====

        /// <summary>
        /// 卡卷是否过期
        /// </summary>
        public bool IsOverdue { get { return this.ValidateTime <= DateTime.Now; } }

        #endregion
    }
}
