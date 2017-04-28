using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Vcyber.BLMS.Common;

namespace Vcyber.BLMS.FrontWeb.Models
{
    public class Home2HomeModel
    {
        /// <summary>
        /// 车型
        /// </summary>
        [Required]
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
        /// 经销商Id
        /// </summary>
        [Display(Name = "经销商Id")]
        //[Required]
        public string DealerId { get; set; }

        /// <summary>
        /// 取车地址
        /// </summary>
        [Display(Name = "取车地址")]
        [Required]
        public string TakeAddress { get; set; }

        /// <summary>
        /// 取车地址经度
        /// </summary>
        [Display(Name = "取车地址经度")]
        [Required]
        public double TakeLong { get; set; }

        /// <summary>
        /// 取车地址纬度
        /// </summary>
        [Display(Name = "取车地址纬度")]
        public double? TakeLat { get; set; }

        /// <summary>
        /// 送车地址
        /// </summary>
        [Display(Name = "送车地址")]
        [Required]
        public string ReturnAddress { get; set; }

        /// <summary>
        /// 送车地址经度
        /// </summary>
        [Display(Name = "送车地址经度")]
        [Required]
        public double ReturnLong { get; set; }

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
        public DateTime ReturnDate { get; set; }


        /// <summary>
        /// 注释
        /// </summary>
        [Display(Name = "注释")]
        public string Comment { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        [Display(Name = "电话")]
        [Required]
        public string Phone { get; set; }

        /// <summary>
        /// 取车日期
        /// </summary>
        [Display(Name = "取车日期")]
        [Required]
        [JsonConverter(typeof(CustomDateConverter))]
        public DateTime ScheduleDate { get; set; }

        [Display(Name = "来源")]
        public string DataSource { get; set; }

    }
}