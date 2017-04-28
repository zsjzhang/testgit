using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity.Enum;

namespace Vcyber.BLMS.Entity
{
    /// <summary>
    /// 用户登录信息
    /// </summary>
    public class UserLoginInfo
    {
        #region ==== 构造函数 ====

        public UserLoginInfo() { }

        #endregion

        #region ==== 公共属性 ====

        /// <summary>
        /// 车音通Id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 用户Guid(任何商城的Id)
        /// </summary>
        public string UserGuid { get; set; }

        /// <summary>
        /// 商城编号
        /// </summary>
        public string MallCode { get; set; }

        public string UserName { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        #endregion
    }
}
