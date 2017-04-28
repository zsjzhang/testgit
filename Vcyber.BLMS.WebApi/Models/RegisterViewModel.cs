using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Vcyber.BLMS.Entity.Enum;

namespace Vcyber.BLMS.WebApi.Models
{
    public class RegisterViewModel
    {
      
        [Display(Name = "电子邮件")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "确认密码")]
        [Compare("Password", ErrorMessage = "密码和确认密码不匹配。")]
        public string ConfirmPassword { get; set; }

        [Required]
        [RegularExpression(@"1[3|4|5|7|8|][0-9]{9}", ErrorMessage = "请输入正确的电话号码")]
        [Display(Name = "手机号")]
        public string Mobile { get; set; }

        [Display(Name = "身份证号")]
        public string IdentityNumber { get; set; }

        //[Required]
        //[Display(Name = "验证码")]
        //public string Captcha { get; set; }
        public string NickName { get; set; }

        //[Required]
        public MembershipType MType { get; set; }
        public string PayNumber { get; set; }
        public string ActivedealerId { get; set; }
        public string VIN { get; set; }

        public string ActivityType { get; set; }

        public string returnUrl { get; set; }

        /// <summary>
        /// 客户类型 1=个人；2=集团
        /// </summary>
        public string customerType { get; set; }

        /// <summary>
        /// 短信验证码
        /// </summary>
        public string Captcha { get; set; }

        public string PaperWork { get; set; }

        /// <summary>
        /// 数据来源(blms:前台网站；blms_web：手机app;blms_wechat：微信)
        /// </summary>
        public string CreatedPerson { get; set; }

        /// <summary>
        /// 微信用户的来源
        /// </summary>
        public string Source { get; set; }
    }
}