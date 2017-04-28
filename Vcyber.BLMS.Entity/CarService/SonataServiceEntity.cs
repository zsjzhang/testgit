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

    public class SonataServiceEntity
    {

        /// <summary>
        /// 订单类型
        /// </summary>
        [Display(Name = "订单类型")]
        [Required]
        public EBMServiceType OrderType { get; set; }

        /// <summary>
        /// 用户编号
        /// </summary>
        [Display(Name = "用户编号")]
        [Required]
        public string UserId { get; set; }

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
        [Required]
        public string DealerName { get; set; }

        /// <summary>
        /// 经销商所在城市
        /// </summary>
        [Display(Name = "经销商所在城市")]
       
        public string DealerCity { get; set; }

        /// <summary>
        /// 经销商所在省份
        /// </summary>
        [Display(Name = "经销商所在省份")]
        
        public string DealerProvince { get; set; }

        /// <summary>
        /// 购车日期
        /// </summary>
        [Display(Name = "购车日期")]
        [JsonConverter(typeof(CustomDateConverter))]
        public DateTime? PurchaseDate { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [Display(Name = "用户姓名")]
        [Required]
        public string UserName { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        [Display(Name = "用户性别(0: 女, 1:男)")]
        [Required]
        public int UserSex { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        [Display(Name = "电话")]
        [Required]
        public string Phone { get; set; }

        /// <summary>
        /// 电子邮箱
        /// </summary>
        [Display(Name = "电子邮箱")]
        public string Email { get; set; }

        /// <summary>
        /// 注释
        /// </summary>
        [Display(Name = "注释")]
        public string Comment { get; set; }

        /// <summary>
        /// 服务日期
        /// </summary>
        [Display(Name = "服务日期")]
        [Required]
        [JsonConverter(typeof(CustomDateConverter))]
        public DateTime ScheduleDate { get; set; }

        /// <summary>
        /// 取车地址
        /// </summary>
        [Display(Name = "取车地址")]

        public string TakeAddress { get; set; }

        /// <summary>
        /// 取车地址经度
        /// </summary>
        [Display(Name = "取车地址经度")]
        public double? TakeLong { get; set; }

        /// <summary>
        /// 取车地址纬度
        /// </summary>
        [Display(Name = "取车地址纬度")]
        public double? TakeLat { get; set; }

        /// <summary>
        /// 送车地址
        /// </summary>
        [Display(Name = "送车地址")]
        public string ReturnAddress { get; set; }

        /// <summary>
        /// 送车地址经度
        /// </summary>
        [Display(Name = "送车地址经度")]
        public double? ReturnLong { get; set; }

        /// <summary>
        /// 送车地址纬度
        /// </summary>
        [Display(Name = "送车地址纬度")]
        public double? ReturnLat { get; set; }

        /// <summary>
        /// 送车时间
        /// </summary>
        [Display(Name = "送车时间")]
        [JsonConverter(typeof(CustomDateConverter))]
        public DateTime? ReturnDate { get; set; }


        /// <summary>
        /// 顾问Id
        /// </summary>
        public int? ConsultantId { get; set; }

        /// <summary>
        /// 顾问名称
        /// </summary>
        public string ConsultantName { get; set; }

        /// <summary>
        /// 车型
        /// </summary>
        public string CarSeries { get; set; }

        /// <summary>
        /// 车牌号
        /// </summary>
        public string LicensePlate { get; set; }

        /// <summary>
        /// 车架号
        /// </summary>
        public string VIN { get; set; }

        /// <summary>
        /// 行驶里程
        /// </summary>
        public int? MileAge { get; set; }

        /// <summary>
        /// 购车日期
        /// </summary>
        [Display(Name = "购车年份")]
        public string PurchaseYear { get; set; }

        /// <summary>
        /// 数据来源(blms_pc_web:前台网站；blms_web：手机app;blms_wechat：微信)
        /// </summary>
        [Display(Name = "数据来源")]
        public string DataSource { get; set; }

        /// <summary>
        /// 维保类型（服务项目，0：维修，1：保养，2:维保）
        /// </summary>
        public EServiceType ServiceType { get; set; }

        /// <summary>
        /// 记录外部系统ID
        /// </summary>
        public string ForeignId { get; set; }

        /// <summary>
        /// Weixin OpenId
        /// </summary>
        public string OpenId { get; set; }
    }
}
