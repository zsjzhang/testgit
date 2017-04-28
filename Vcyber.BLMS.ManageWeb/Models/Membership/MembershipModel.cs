using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Vcyber.BLMS.ManageWeb.Models
{
    public class MembershipModel
    {

        public bool Agree { get; set; }
        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// User's name
        /// </summary>
        [RegularExpression(@"1[3|4|5|7|8|][0-9]{9}", ErrorMessage = "请输入正确的电话号码")]
        public string UserName { get; set; }

        /// <summary>
        ///     Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        ///     True if the email is confirmed, default is false
        /// </summary>
        public bool EmailConfirmed { get; set; }

        /// <summary>
        ///     A random value that should change whenever a users credentials have changed (password changed, login removed)
        /// </summary>
        public string SecurityStamp { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        [Display(Name = "手机")]
        [StringLength(20)]
        [RegularExpression(
            @"1[3|4|5|7|8|][0-9]{9}",
            ErrorMessage = "请输入正确的手机号码")]
        public string PhoneNumber { get; set; }

        /// <summary>
        ///     True if the phone number is confirmed, default is false
        /// </summary>
        public bool PhoneNumberConfirmed { get; set; }

        /// <summary>
        ///     Is two factor enabled for the user
        /// </summary>
        public bool TwoFactorEnabled { get; set; }

        /// <summary>
        ///     DateTime in UTC when lockout ends, any time in the past is considered not locked out.
        /// </summary>
        public DateTime? LockoutEndDateUtc { get; set; }

        /// <summary>
        ///     Is lockout enabled for this user
        /// </summary>
        public bool LockoutEnabled { get; set; }

        /// <summary>
        ///     Used to record failures for the purposes of lockout
        /// </summary>
        public int IsDel { get; set; }

        public int Status { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime UpdateTime { get; set; }

        public int AccessFailedCount { get; set; }

        public int IdentityConfirmed { get; set; }

        /// <summary>
        /// 身份证编号
        /// </summary>
        public string IdentityNumber { get; set; }

        public int Age { get; set; }

        public DateTime Birthday { get; set; }

        /// <summary>
        /// 会员类型
        /// </summary>
        public int MType { get; set; }

        /// <summary>
        /// 会员类型名称
        /// </summary>
        public string MTypeName { get; set; }

        public int MLevel { get; set; }

        public string VIN { get; set; }

        public int Gender { get; set; }

        public int State { get; set; }

        public string Provency { get; set; }

        public string City { get; set; }

        public string Area { get; set; }

        public string Address { get; set; }

        public string RealName { get; set; }

        public string ValideCode { get; set; }

       
        public string NickName { get; set; }

        //标记是否是后台协助入会
        public string IsAutoJoin { get; set; }

        public string MembershipId { get; set; }
        public string ImageProofFront{get;set;}
        public string ImageProofVerso{get;set;}
        public string ImageProofByHand { get; set; }

        public int ApproveStatus { get; set; }
    }
}