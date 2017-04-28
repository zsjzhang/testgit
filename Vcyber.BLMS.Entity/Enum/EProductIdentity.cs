using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity.Enum
{
    /// <summary>
    /// 商品购买者身份
    /// </summary>
    public enum EProductIdentity
    {
        /// <summary>
        /// 无限制
        /// </summary>
        YB = 0,

        /// <summary>
        /// 索久会员
        /// </summary>
        SJ = 1,

        /// <summary>
        /// 普通会员
        /// </summary>
        PT = 2
    }
}
