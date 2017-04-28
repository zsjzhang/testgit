using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    /// <summary>
    /// 服务卡
    /// </summary>
    public class ServiceCard
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        public int CardId { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>
        public string BatchNo { get; set; }

        /// <summary>
        /// 卡劵号
        /// </summary>
        public string CardNo { get; set; }

        /// <summary>
        /// 卡劵状态(1：未发放2：已发放3：已核销4：已结算)
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 发放用户
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 发放时间
        /// </summary>
        public DateTime? SendTime { get; set; }

        /// <summary>
        /// 使用时间
        /// </summary>
        public DateTime? UseTime { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        #region ==== 附件属性 ====


        /// <summary>
        /// 批次名称
        /// </summary>
        public string BatchName { get; set; }

        /// <summary>
        /// 服务卡面值
        /// </summary>
        public decimal BatchPrice { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime ValidateTime { get; set; }

        /// <summary>
        /// 卡卷是否过期
        /// </summary>
        public string IsOverdue { get { return this.ValidateTime > DateTime.Now ? "未过期" : "已过期"; } }

        /// <summary>
        /// 卡卷类型Code
        /// </summary>
        public string TypeCode { get; set; }

        /// <summary>
        /// 卡卷类名称
        /// </summary>
        public string TypeName { get; set; }

        #endregion

    }
}
