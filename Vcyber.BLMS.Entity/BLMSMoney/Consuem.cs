using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity.Enum;

namespace Vcyber.BLMS.Entity
{
    /// <summary>
    /// DMS取消工单消费
    /// </summary>
    public class Consuem
    {
        #region ==== 构造函数 ====

        public Consuem() { }

        #endregion

        #region ==== 公共属性 ====

        /// <summary>
        /// id编号
        /// </summary>
       public string ID { get; set; }
       
        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 经销商ID
        /// </summary>
        public string DealerId { get; set; }

        /// <summary>
        /// 经销商名称
        /// </summary>
        public string DealerName { get; set; }
        /// <summary>
        /// 消费类型（0：事故车维修（普通），1：首次保养，2：购车，3：定期保养，5：配件，6：精品8：钣喷）
        /// </summary>
        public string ConsumeType { get; set;}

        /// <summary>
        /// 消费使用积分数量（取消工单的时候应该加上）
        /// </summary>
        public int  ConsumePoints { get; set; }

        /// <summary>
        /// 消费是返还的积分（取消工单的时候要减去）
        /// </summary>
        public int RewardPoints { get; set; }

        /// <summary>
        /// 身份证号
        /// </summary>
        public string IdentityNumber { get; set; }

        /// <summary>
        /// 车架号
        /// </summary>
        public string VIN { get; set; }

        /// <summary>
        /// 实际支付金额
        /// </summary>
        public string TotalCost { get; set; }

        /// <summary>
        /// 消费工单号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 应付金额（总费用）
        /// </summary>
        public string PurchaseCost { get; set; }

        /// <summary>
        /// 使用积分抵扣的金额
        /// </summary>
        public string PointCost { get; set; }

        /// <summary>
        /// DMSOrderNo消费工单号
        /// </summary>
        public string DMSOrderNo { get; set; }
        #endregion
    }

}
