using System;

namespace Vcyber.BLMS.Entity
{
    /// <summary>
    ///     登录提醒
    /// </summary>
    public class MembershipLoginNotify
    {
        public int Id { get; set; }

        /// <summary>
        ///     用户Id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        ///     更新时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        ///     创建时间
        /// </summary>
        public DateTime UpdateTime { get; set; }

        /// <summary>
        ///     1=pc 2=weixin
        /// </summary>
        public int DataSource { get; set; }

        /// <summary>
        ///     1=修改密码
        /// </summary>
        public LoginNotifyType NotifyType { get; set; }
    }
}