using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity.AirportService
{
    using System.ComponentModel.DataAnnotations;

    public class AireportInputEntity
    {
        /// <summary>
        /// 手机号
        /// </summary>
        [Display(Name = "手机号")]
        [Required]
        public string Phone { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [Display(Name = "姓名")]
        public string Name { get; set; }

        /// <summary>
        /// 身份证号
        /// </summary>
        [Display(Name = "身份证号")]
        [Required]
        public string IdentityNo { get; set; }

        /// <summary>
        /// 兑换次数
        /// </summary>
         [Display(Name = "预约次数")]
        public int ScheduleCount { get; set; }

        /// <summary>
        /// 可用免费次数
        /// </summary>
        [Display(Name = "可用免费次数")]
        public int FreeCount { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        [Display(Name = "验证码")]
        [Required]
        public string VerifyCode { get; set; }

        /// <summary>
        /// 用户
        /// </summary>
        [Display(Name = "用户Id")]
        [Required]
        public string UserId { get; set; }

        /// <summary>
        /// 机场Id
        /// </summary>
        public string AirportId { get; set; }

        public string Airport { get; set; }

        /// <summary>
        /// 会员等级
        /// </summary>
        public string Gender
        {
        get;set;
        }
    }
}
