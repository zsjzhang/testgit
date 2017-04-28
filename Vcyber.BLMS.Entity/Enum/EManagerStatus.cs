using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Common;

namespace Vcyber.BLMS.Entity.Enum
{
    /// <summary>
    /// 管理员状态
    /// </summary>
    public enum EManagerStatus
    {
        /// <summary>
        /// 启用
        /// </summary>
        [EnumDescribe("启用")]
        Usable=1,

        /// <summary>
        /// 未启用
        /// </summary>
        [EnumDescribe("未启用")]
        UnUsable=0,

        /// <summary>
        /// 锁定
        /// </summary>
        [EnumDescribe("锁定")]
        Lock=2
    }
}
