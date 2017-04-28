using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity.CarService
{
    using System.ComponentModel.DataAnnotations;

    using Vcyber.BLMS.Entity.Enum;

    /// <summary>
    /// 普通查询参数
    /// </summary>
    public class QueryParamEntity
    {
        /// <summary>
        /// 用户编号
        /// </summary>
        [Display(Name = "用户编号")]
        public string UserId { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        [Display(Name = "手机号")]
        public string Phone { get; set; }

        /// <summary>
        /// 创建日期(从)
        /// </summary>
        [Display(Name = "创建日期(从)")]
        public DateTime? CreateFromDate { get; set; }

        /// <summary>
        /// 创建日期(到)
        /// </summary>
        [Display(Name = "创建日期(到)")]
        public DateTime? CreateToDate { get; set; }

        /// <summary>
        /// 预约日期(从)
        /// </summary>
        [Display(Name = "预约日期(从)")]
        public DateTime? ScheduleFromDate { get; set; }

        /// <summary>
        /// 预约日期(到)
        /// </summary>
        [Display(Name = "预约日期(到)")]
        public DateTime? ScheduleToDate { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        [Display(Name = "订单号")]
        public string OrderNo { get; set; }

        /// <summary>
        /// 车型
        /// </summary>
        [Display(Name = "车型")]
        public string CarSeries { get; set; }

        /// <summary>
        /// 车牌号
        /// </summary>
        [Display(Name = "车牌号")]
        public string LicensePlate { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [Display(Name = "状态")]
        public EOrderState State { get; set; }

        /// <summary>
        /// 受理人
        /// </summary>
        [Display(Name = "受理人")]
        public string UpdateName { get; set; }

        ///// <summary>
        ///// 订单类型
        ///// </summary>
        //[Display(Name = "订单类型")]
        //public EOrderType Type { get; set; }

        /// <summary>
        /// 经销商Id
        /// </summary>
        public string DealerId { get; set; }

        /// <summary>
        /// VIN码
        /// </summary>
        [Display(Name = "VIN码")]
        public string VIN { get; set; }

        /// <summary>
        /// 订单类型
        /// </summary>
        [Display(Name = "订单类型")]
        public EOrderType? OrderType { get; set; }

        /// <summary>
        /// 是否已经导出
        /// </summary>
        [Display(Name = "是否导出")]
        public EExportSatus? IsExported { get; set; }
    }
}
