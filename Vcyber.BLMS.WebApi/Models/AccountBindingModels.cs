using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Vcyber.BLMS.WebApi.Models
{
    // Models used as parameters to AccountController actions.

    public class AddExternalLoginBindingModel
    {
        [Required]
        [Display(Name = "External access token")]
        public string ExternalAccessToken { get; set; }
    }

    public class ChangePasswordBindingModel
    {
        /// <summary>
        /// 用户id
        /// </summary>
        [Required]
        public string Id { get; set; }
        /// <summary>
        /// 老密码
        /// </summary>
        [Required]
        public string OldPassword { get; set; }

        /// <summary>
        /// 新密码
        /// </summary>
        [Required]
        [StringLength(100, ErrorMessage = "至少6个字符长度", MinimumLength = 6)]
        public string NewPassword { get; set; }

        /// <summary>
        /// 确认密码
        /// </summary>
        [Compare("NewPassword", ErrorMessage = "输入的密码不一致")]
        [Required]
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        [Required]
        public string PhoneNumber { get; set; }
    }

    public class ResetPasswordBindingModel
    {
        /// <summary>
        /// 手机号
        /// </summary>
        [Required]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 新密码
        /// </summary>
        [Required]
        [StringLength(100, ErrorMessage = "至少6个字符长度", MinimumLength = 6)]
        public string NewPassword { get; set; }

        /// <summary>
        /// 确认密码
        /// </summary>
        [Compare("NewPassword", ErrorMessage = "输入的密码不一致")]
        [Required]
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        [Required]
        public string ValideCode { get; set; }
    }

    public class RegisterBindingModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class RegisterExternalBindingModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class RemoveLoginBindingModel
    {
        [Required]
        [Display(Name = "Login provider")]
        public string LoginProvider { get; set; }

        [Required]
        [Display(Name = "Provider key")]
        public string ProviderKey { get; set; }
    }

    public class SetPasswordBindingModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
