using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Vcyber.BLMS.FrontWeb.Models
{
    public class UserSecurityInfoViewModel
    {
        [Required]
        [RegularExpression("^d{11}$")]
        [Display(Name = "手机号")]
        public string PhoneNumber { get; set; }

        [Required]
        [RegularExpression("^w+[-+.]w+)*@w+([-.]w+)*.w+([-.]w+)*$")]
        [Display(Name = "邮箱")]
        public string Email { get; set; }

        [Required]
        [RegularExpression("^d{4}$")]
        [Display(Name = "验证码")]
        public string ValidateCode { get; set; }

        [Required]
        [RegularExpression("^d{6}$")]
        [Display(Name = "短信验证码")]
        public string PhoneValidateCode { get; set; }

        [Required]
        [RegularExpression("^(\\d{15}$|^\\d{18}$|^\\d{17}(\\d|X|x))$")]
        [Display(Name = "身份证号")]
        public string IdentityNumber { get; set; }
    }
}