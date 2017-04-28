using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Common;

namespace Vcyber.BLMS.Entity.Enum
{
    /// <summary>
    /// 验证微信传过来的VIN结果
    /// </summary>
    public enum EVinVerificationStatus
    {
        /// <summary>
        /// 未注册
        /// </summary>
        NotRegister=0,
        /// <summary>
        /// 三星会员
        /// </summary>
        ThreeStar=1,
        /// <summary>
        /// 待验证三星会员
        /// </summary>
        ToBe3Star=2,
        /// <summary>
        /// 普通会员
        /// </summary>
        Common=3,
        /// <summary>
        /// 无车会员
        /// </summary>
        NoCar=4
    }
}
