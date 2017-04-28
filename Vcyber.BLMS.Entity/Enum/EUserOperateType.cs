using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    public enum EUserOperateType
    {
        /// <summary>
        /// 修改登录密码
        /// </summary>
        ChangeLoginPassword = 1,

        /// <summary>
        /// 找回登录密码
        /// </summary>
        FindLoginPassword = 2,

        /// <summary>
        /// 修改支付密码
        /// </summary>
        ChangePayPassword = 3,

        /// <summary>
        /// 找回支付密码
        /// </summary>
        FindPayPassword = 4
    }
}
