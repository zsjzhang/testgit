using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    /// <summary>
    /// 用户登录记录
    /// </summary>
    public class UserLoginRecord
    {
        #region ==== 构造函数 ====

        public UserLoginRecord() { }

        #endregion

        #region ==== 公共属性 ====

        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 登录方式（1=车音通、2=马自达、3=车联邦）
        /// </summary>
        public int LoginMode { get; set; }

        public DateTime CreateTime { get; set; }

        #endregion
    }
}
