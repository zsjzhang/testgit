using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Vcyber.BLMS.ManageWeb.Models
{



    public class UserModel
    {

        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 管理员名称
        /// </summary>
        [Required(ErrorMessage = "请输入用户名！")]
        public string UserName { get; set; }

        /// <summary>
        /// 管理员密码
        /// </summary>
        [Required(ErrorMessage = "密码不能为空")]
        [DataType(DataType.Password)]
        [StringLength(20, ErrorMessage = "{0}最小长度为{2}", MinimumLength = 6)]
        public string Password { get; set; }

       
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "两次密码输入不一致！")]
        public string RepeatPassword
        {
            get;

            set;

        }

        /// <summary>
        /// 是否删除
        /// </summary>
        public int IsDel { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [Required(ErrorMessage = "请输入邮箱地址！")]
        [RegularExpression(@"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "请输入正确的E-Mail格式")]
        [StringLength(200)]
        public string Email { get; set; }

        private string _phone;

        /// <summary>
        /// 手机
        /// </summary>
        [Display(Name = "联系电话")]
        [StringLength(20)]
        [RegularExpression(
            @"((\d{11})|^((\d{7,8})|(\d{4}|\d{3})-(\d{7,8})|(\d{4}|\d{3})-(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1})|(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1}))$)",
            ErrorMessage = "请输入正确的电话号码")]

        public string Phone
        {
            get {
                if (_phone == null)
                {
                    return string.Empty;
                }
                return _phone;
            }
            set {  _phone = value; }
        }

        private string _department;

        /// <summary>
        /// 部门名称
        /// </summary>
        
        public string Department
        {
            get {
                if (_department == null)
                {
                    return string.Empty;
                }
                return _department;
            }
            set
            {
                _department = value;
            }
        }

        /// <summary>
        /// 管理员状态
        /// </summary>
        public int Status { get; set; }
        public string StatusName { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateTime { get; set; }

        private string _lastLoginTime;

        public string LastLoginTime
        {
            get {
                if (_lastLoginTime == null)
                {
                    return string.Empty;
                }
                return _lastLoginTime;
            }
            set
            {
                _lastLoginTime = value;
            }
        }

        /// <summary>
        /// 所属角色名称
        /// </summary>
        public string RoleName { get; set; }

        public string DealerId { get; set; }

        public string DealerName { get; set; }

        //[Range(0, 10000)]
        //public int RoleId { get; set; }
    }


    public class ResetPWViewModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "确认密码")]
        [Compare("Password", ErrorMessage = "密码和确认密码不匹配。")]
        public string ConfirmPassword { get; set; }
    }
}