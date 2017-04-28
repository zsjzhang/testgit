using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Common;

namespace Vcyber.BLMS.Entity.Enum
{
    public enum EDataStatus
    {
        /// <summary>
        /// 未删除
        /// </summary>
        [EnumDescribe("未删除")]
        NoDelete = 0,

        /// <summary>
        /// 已删除
        /// </summary>
        [EnumDescribe("已删除")]
        Delete = 1
    }
}
