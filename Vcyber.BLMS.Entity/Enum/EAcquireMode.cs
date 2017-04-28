using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity.Enum
{
    /// <summary>
    /// 蓝豆获取方式
    /// </summary>
    public enum EAcquireMode
    {
        /// <summary>
        /// 一次性
        /// </summary>
        YCX = 1,

        /// <summary>
        /// 每日
        /// </summary>
        MR = 2,

        /// <summary>
        /// 每月
        /// </summary>
        MY = 3,

        /// <summary>
        /// 每次
        /// </summary>
        MC = 4,

        /// <summary>
        /// 双月
        /// </summary>
        SY=5
    }
}
