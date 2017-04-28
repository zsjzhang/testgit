using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vcyber.BLMS.Entity
{
    /// <summary>
    /// DMS积分清单
    /// </summary>
    public class IntegraListV
    {

        /// <summary>
        /// 用户ID
        /// </summary>
        public string BlueMembership_Id { get; set;}

        ///// <summary>
        ///// 用户名称
        ///// </summary>
        //public string UserName { get; set; }

        ///// <summary>
        ///// 工单号 
        ///// </summary>
        //public string OrderNo { get; set; }

        /// <summary>
        /// DMSo工单号
        /// </summary>
        public string DMSOrderNo { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get; set; }

        ///// <summary>
        ///// 经销商ID
        ///// </summary>
        // public string DealerId { get; set; }

        ///// <summary>
        ///// 经销商名称
        ///// </summary>
        //public string DealerName { get; set; }

        ///// <summary>
        ///// 预约单号
        ///// </summary>
        //public string ScheduleOrderNo { get; set; }

        /// <summary>
        /// 消费类型 3:定期保养，0:事故车维修，8:钣喷,1:首次保养,2:购车
        /// </summary>
        public string ConsumeType { get; set; }

        ///// <summary>
        ///// 配件费
        ///// </summary>
        //public decimal PartCost { get; set; }

        ///// <summary>
        ///// 材料费
        ///// </summary>
        //public decimal MaterialCost { get; set; }

        ///// <summary>
        ///// 工时费
        ///// </summary>
        //public decimal LaborCost { get; set; }

        ///// <summary>
        ///// 购车费
        ///// </summary>
        //public decimal PurchaseCost { get; set; }

        /// <summary>
        /// 积分抵扣
        /// </summary>
        public decimal PointCost { get; set; }

        /// <summary>
        /// 总费用
        /// </summary>
        public decimal TotalCost { get; set; }

        /// <summary>
        /// 消耗积分
        /// </summary>
        public int ConsumePoints { get; set; }

        /// <summary>
        /// 产生积分
        /// </summary>
        public int RewardPoints { get; set; }

        ///// <summary>
        ///// 工单/发票
        ///// </summary>
        //public string PaperOrder { get; set; }

        ///// <summary>
        ///// 审核状态(0:未审核，1：通过，2：未通过)
        ///// </summary>
        //public int ApproveStatus { get; set; }

        ///// <summary>
        ///// 积分状态（0：未发放，1：已发放）
        ///// </summary>
        //public int PointStatus { get; set; }

        /// <summary>
        /// 消费时间
        /// </summary>
        public DateTime ConsumeDate { get; set; }

        ///// <summary>
        ///// 创建时间
        ///// </summary>
        //public DateTime CreateTime { get; set; }

        ///// <summary>
        ///// 创建人Id
        ///// </summary>
        //public string CreateId { get; set; }

        ///// <summary>
        ///// 创建人姓名
        ///// </summary>
        //public string CreateName { get; set; }

        ///// <summary>
        ///// 更新时间
        ///// </summary>
        //public DateTime UpdateTime { get; set; }

        ///// <summary>
        ///// 操作人Id
        ///// </summary>
        //public string UpdateId { get; set; }

        ///// <summary>
        ///// 操作人姓名
        ///// </summary>
        //public string UpdateName { get; set; }

        ///// <summary>
        ///// 备注
        ///// </summary>
        //public string Comment { get; set; }

        /// <summary>
        /// 身份证号
        /// </summary>
        public string IdentityNumber { get; set; }

        /// <summary>
        /// 车架号
        /// </summary>
        public string VIN { get; set; }

    }
}