using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity.Enum
{
    /// <summary>
    /// 资料申请状态
    /// </summary>
    public enum EMApplyStatus
    {
        /// <summary>
        /// 待审核
        /// </summary>
        DSH=0,

        /// <summary>
        /// 审核通过
        /// </summary>
        SHTG=1,

      
       

        /// <summary>
        /// 已经邮寄
        /// </summary>
        YJYJ=3,

        /// <summary>
        /// 审核失败
        /// </summary>
        SHSB=13
    }
}
