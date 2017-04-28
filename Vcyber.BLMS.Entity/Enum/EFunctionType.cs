using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Common;

namespace Vcyber.BLMS.Entity.Enum
{
    /// <summary>
    /// 功能类型
    /// </summary>
    public enum EFunctionType
    {
        /// <summary>
        /// 模块
        /// </summary>
        [EnumDescribe("模块")]
        Model = 0,

        /// <summary>
        /// 功能
        /// </summary>
        [EnumDescribe("功能")]
        Fun = 1
    }
}
