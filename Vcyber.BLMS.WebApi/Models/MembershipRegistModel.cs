using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Vcyber.BLMS.Entity.Enum;

namespace Vcyber.BLMS.WebApi.Models
{
    /// <summary>
    /// 会员注册
    /// </summary>
    public class MembershipRegistModel
    {
        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        [Display(Name = "手机")]
        [StringLength(20)]
        [RegularExpression(@"1[3|4|5|7|8|][0-9]{9}", ErrorMessage = "请输入正确的电话号码")]
        public string PhoneNumber { get; set; }


        /// <summary>
        /// 密码(至少6位, 包含大小写字母,数字,特殊符号)
        /// </summary>
        [Required]
        public string PassWord { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        [Required]
        public string NickName { get; set; }

        /// <summary>
        /// 身份证编号
        /// </summary>
        public string IdentityNumber { get; set; }

        /// <summary>
        /// 会员类型
        /// </summary>
        [Required]
        public MembershipType MType { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        [Required]
        public string ValideCode { get; set; }
    }
}