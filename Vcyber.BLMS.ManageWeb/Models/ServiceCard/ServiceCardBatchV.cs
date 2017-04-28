using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using Vcyber.BLMS.Application;

namespace Vcyber.BLMS.ManageWeb.Models
{
    /// <summary>
    /// 服务批次
    /// </summary>
    public class ServiceCardBatchV
    {
        #region ==== 构造函数 ====

        public ServiceCardBatchV() { }

        #endregion

        #region ==== 公共属性 ====

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

        #endregion

        #region ==== 公共方法 ====

        public bool ValidateData(out string message)
        {
            message = string.Empty;

            if (string.IsNullOrEmpty(this.BatchName) || string.IsNullOrEmpty(this.TypeCode))
            {
                message = "请填写完整的卡卷信息。";
                return false;
            }

            if (this.BatchTotalMoney <= 0)
            {
                message = "批次总金额不能小于等于0";
                return false;
            }

            if (this.BatchQty <= 0)
            {
                message = "批次数量不能小于等于0";
                return false;
            }

            if (this.BatchQty >5000)
            {
                message = "批次数量不能大于等于5000";
                return false;
            }


            if (this.ValidateTime <= DateTime.Now)
            {
                message = "卡卷过期时间不能等于当前时间。";
                return false;
            }

            if (_AppContext.ServiceCardBatchApp.IsExist(this.BatchName.Trim()))
            {
                message = "卡卷名称重复。";
                return false;
            }

            this.BatchPrice = (decimal)((double)this.BatchTotalMoney / this.BatchQty);

            return true;
        }

        #endregion
    }
}