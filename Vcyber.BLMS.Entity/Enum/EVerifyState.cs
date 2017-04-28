using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity.Enum
{
    /// <summary>
    /// 用户信息审核状态
    /// </summary>
    public enum EVerifyState
    {
        /// <summary>
        /// 审核通过
        /// </summary>
        Success=1,

        /// <summary>
        /// 审核失败
        /// </summary>
        Fail=2,

        /// <summary>
        /// 审核中
        /// </summary>
        VerifyIn=3
    }
}
