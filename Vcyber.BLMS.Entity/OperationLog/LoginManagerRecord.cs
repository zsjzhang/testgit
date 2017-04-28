using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    /// <summary>
    /// 管理员登陆记录信息
    /// </summary>
    public class LoginManagerRecord
    {
        #region ==== 构造函数 ====

        public LoginManagerRecord() { }

        #endregion

        #region ==== 公共属性 ====

        /// <summary>
        /// 
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 管理员Id
        /// </summary>
        public string ManagerId { get; set; }

        /// <summary>
        /// 管理员昵称
        /// </summary>
        public string ManagerName { get; set; }

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
