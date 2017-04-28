using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Vcyber.BLMS.Common;

namespace Vcyber.BLMS.FrontWeb.Models
{
    public class FreeMotModel
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
        /// 行驶里程
        /// </summary>
        [Required]
        public int MileAge { get; set; }

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

        // <summary>
        // 经销商名称
        // </summary>
        //[Display(Name = "经销商名称")]
        //[Required]
        //public string DealerName { get; set; }

        /// <summary>
        /// 经销商所在城市
        /// </summary>
        [Display(Name = "经销商所在城市")]
        [Required]
        public string DealerCity { get; set; }

        /// <summary>
        /// 经销商所在省份
        /// </summary>
        [Display(Name = "经销商所在省份")]
        [Required]
        public string DealerProvince { get; set; }

        /// <summary>
        /// 购车年份
        /// </summary>
        [Display(Name = "购车年份")]
      
        public string PurchaseYear { get; set; }

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
        /// 服务日期
        /// </summary>
        [Display(Name = "服务日期")]
        [Required]
        [JsonConverter(typeof(CustomDateConverter))]
        public DateTime ScheduleDate { get; set; }

        [Display(Name = "来源")]
        public string DataSource { get; set; }
    }
}