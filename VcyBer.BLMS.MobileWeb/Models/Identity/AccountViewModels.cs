using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Vcyber.BLMS.Entity.Enum;

namespace VcyBer.BLMS.MobileWeb.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "电子邮件")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "代码")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "记住此浏览器?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "电子邮件")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        //[Required]
        //[Display(Name = "电子邮件")]
        //[EmailAddress]
        //public string Email { get; set; }
        [Required]
        [Display(Name = "账户名")]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [Display(Name = "记住我?")]
        public bool RememberMe { get; set; }

        [Display(Name = "验证码")]
        public string Captcha { get; set; }

        [Display(Name = "短信码")]
        public string SMSCaptcha { get; set; }

        [Display(Name = "手机帐号")]
        public string PhoneNumber { get; set; }

        public string Url { get; set; }
    }

    public class RegisterViewModel
    {
        [EmailAddress]
        [Display(Name = "电子邮件")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 8)]
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

        [Required]
        [Display(Name = "验证码")]
        public string Captcha { get; set; }

        [Required]
        public string NickName { get; set; }

        [Required]
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

        public string Source { get; set; }

        public string PaperWork { get; set; }

        public string ImgCaptcha { get; set; }

        public string Mid { get; set; }

    }

    public class ResetPasswordViewModel
    {

        [EmailAddress]
        [Display(Name = "邮箱")]
        public string Email { get; set; }

        [Required]
        //[EmailAddress]
        [Display(Name = "旧密码")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "确认密码")]
        [Compare("Password", ErrorMessage = "密码和确认密码不匹配。")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ResetPasswordByAdminViewModel
    {
        [Display(Name = "账户Id")]
        public string Id { get; set; }

        [Display(Name = "账户")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "确认密码")]
        [Compare("Password", ErrorMessage = "密码和确认密码不匹配。")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "电子邮件")]
        public string Email { get; set; }
    }
}
