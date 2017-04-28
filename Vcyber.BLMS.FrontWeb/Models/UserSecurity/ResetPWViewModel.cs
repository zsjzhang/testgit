using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Vcyber.BLMS.FrontWeb.Models
{
    public class ResetPWViewModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string LoginPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "确认密码")]
        [Compare("LoginPassword", ErrorMessage = "密码和确认密码不匹配。")]
        public string ConfirmLoginPassword { get; set; }

        [Required]
        [Display(Name = "用户ID")]
        public string UserGuid { get; set; }
    }
}