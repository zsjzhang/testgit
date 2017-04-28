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

    public sealed class BaseCarEntity
    {

        /// <summary>
        /// 主键
        /// </summary>
        [Display(Name = "主键")]
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// Car Id
        /// </summary>
        [Display(Name = "Car Id")]
        [Required]
        public int SeriesId { get; set; }

        /// <summary>
        /// 车型
        /// </summary>
        [Display(Name = "车型")]
        [Required]
        public string SeriesName { get; set; }

        /// <summary>
        /// 0:试驾， 1：订车， 2：预约（备用）
        /// </summary>
        [Display(Name = "0:试驾， 1：订车， 2：预约（备用）")]
        public ECarSeriesType? CarType { get; set; }

        /// <summary>
        /// 操作人Id
        /// </summary>
        [Display(Name = "操作人Id")]
        public string UpdateId { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        [Display(Name = "更新时间")]
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        [Display(Name = "是否删除")]
        public bool? IsDeleted { get; set; }
    }
}
