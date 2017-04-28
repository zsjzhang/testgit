using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity.CarService
{
    using System.ComponentModel.DataAnnotations;

    using Newtonsoft.Json;

    using Vcyber.BLMS.Common;
    using Vcyber.BLMS.Entity.Enum;

    public sealed class ConsumeEntity
    {
        public ConsumeEntity()
        {
            this.EConsumeType = EConsumeType.ALL;
        }
        /// <summary>
        /// 经销商Id
        /// </summary>
        [Display(Name = "经销商Id")]
        [Required]
        public string DealerId { get; set; }

        /// <summary>
        /// 经销商名称
        /// </summary>
        [Display(Name = "经销商名称")]
        //[Required]
        public string DealerName { get; set; }

        /// <summary>
        /// 预约单号
        /// </summary>
        [Display(Name = "预约单号")]
        public string ScheduleOrderNo { get; set; }

        /// <summary>
        /// 消费类型（0：维修，1：保养，2：购车）
        /// </summary>
        [Display(Name = "消费类型")]
        public int ConsumeType { get; set; }

        [Display(Name = "消费类型")]
        public EConsumeType EConsumeType { get; set; }

        /// <summary>
        /// 配件费
        /// </summary>
        [Display(Name = "配件费")]
       [Range(0, int.MaxValue)]
        public decimal? PartCost { get; set; }

        /// <summary>
        /// 材料费
        /// </summary>
        [Display(Name = "材料费")]
       [Range(0, int.MaxValue)]
        public decimal? MaterialCost { get; set; }

        /// <summary>
        /// 工时费
        /// </summary>
        [Display(Name = "工时费")]
       [Range(0, int.MaxValue)]
        public decimal? LaborCost { get; set; }

        /// <summary>
        /// 购车费
        /// </summary>
        [Display(Name = "总费用")]
        [Range(0, int.MaxValue)]
        public decimal? PurchaseCost { get; set; }

        /// <summary>
        /// 积分抵扣
        /// </summary>
        [Display(Name = "积分抵扣(元)")]
        [Range(0, int.MaxValue)]
        public decimal? PointCost { get; set; }

        /// <summary>
        /// 总费用
        /// </summary>
        [Display(Name = "实际支付费用")]
        public decimal TotalCost { get; set; }

        /// <summary>
        /// 消耗积分
        /// </summary>
        [Display(Name = "消耗积分")]
        [Range(0, int.MaxValue)]
        public int? ConsumePoints { get; set; }

        /// <summary>
        /// 工单/发票
        /// </summary>
        [Display(Name = "工单/发票")]
        public string PaperOrder { get; set; }

        /// <summary>
        /// 工单费用
        /// </summary>
        [Display(Name="工单费用")]
        [Range(0,int.MaxValue)]
        public decimal? PaperOrderCost { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "备注")]
        public string Comment { get; set; }

        /// <summary>
        /// 消费时间
        /// </summary>
        [Display(Name = "消费时间")]
        public DateTime? ConsumeDate { get; set; }

        /// <summary>
        /// 用户手机号
        /// </summary>
        [Display(Name = "手机号")]
        [Required]
        public string Phone { get; set; }

        /// <summary>
        /// 用户编号
        /// </summary>
        [Display(Name = "用户编号")]
        [Required]
        public string UserId { get; set; }

        /// <summary>
        /// 手机验证码
        /// </summary>
        [Display(Name = "验证码")]
        public string VerifyCode { get; set; }

        /// <summary>
        /// 身份证号
        /// </summary>
        [Display(Name = "身份证号")]
        public string IdentityNumber { get; set; }

        /// <summary>
        /// 用户姓名
        /// </summary>
        [Display(Name = "用户姓名")]
        public string UserName { get; set; }

        /// <summary>
        /// 车架号
        /// </summary>
        [Display(Name = "车架号")]
        public string VIN { get; set; }
        /// <summary>
        /// 消费省份
        /// </summary>
        [Display(Name="省份")]
        public string DealerProvince{get; set; }
        /// <summary>
        /// 消费城市
        /// </summary>
        [Display(Name="城市")]
        public string DealerCity { get; set; }
        /// <summary>
        /// 获取积分
        /// </summary>
        [Display(Name="获取积分")]
        public string CreateIntegral { get; set; }
        /// <summary>
        /// 打印服务类型
        /// </summary>
        [Display(Name="服务类型")]
        public string EConsumeName { get; set; }

        /// <summary>
        /// 用户等级
        /// </summary>
        [Display(Name = "用户等级")]
        public int MLevel { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMsg { get; set; }

        /// <summary>
        /// DMS工单号
        /// </summary>
        public string DMSOrderNo { get; set; }
    }
}
