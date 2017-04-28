using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    /// <summary>
    /// 会员登陆记录
    /// </summary>
    public class LoginMemberRecord
    {
        #region ==== 构造函数 ====

        public LoginMemberRecord() { }

        #endregion

        #region ==== 公共属性 ====

        /// <summary>
        /// 
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 会员Id
        /// </summary>
        public string MemberId { get; set; }

        /// <summary>
        /// 会员昵称
        /// </summary>
        public string MemberName { get; set; }

        /// <summary>
        /// 数据渠道(blms:前台网站；blms_web：手机app;blms_wechat：微信)
        /// </summary>
        public string DataSource { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Remark { get; set; }

        #endregion
    }
}
