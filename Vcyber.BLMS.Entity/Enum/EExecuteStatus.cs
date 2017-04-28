using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Common;

namespace Vcyber.BLMS.Entity.Enum
{
    /// <summary>
    /// 执行状态
    /// </summary>
    public enum EExecuteStatus
    {
        /// <summary>
        /// 成功
        /// </summary>
        [EnumDescribe("成功")]
        Success = 1,

        /// <summary>
        /// 失败
        /// </summary>
        [EnumDescribe("失败")]
        Fail = 2
    }
}
