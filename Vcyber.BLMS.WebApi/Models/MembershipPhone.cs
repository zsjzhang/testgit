using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vcyber.BLMS.WebApi.Models
{
    /// <summary>
    /// 会员手机号
    /// </summary>
    public class MembershipPhone
    {
        #region ==== 构造函数 ====

        public MembershipPhone() { }

        #endregion

        #region ==== 公共属性 ====

        /// <summary>
        /// 会员Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 会员手机号
        /// </summary>
        public string Phone { get; set; }

        #endregion
    }
}