using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity.Enum
{
    /// <summary>
    /// 来源渠道
    /// </summary>
    public enum EDataSource
    {
        /// <summary>
        /// 网站
        /// </summary>
        blms = 1,

        /// <summary>
        /// 手机app
        /// </summary>
        blms_web = 2,

        /// <summary>
        /// 微信
        /// </summary>
        blms_wechat = 3,

        /// <summary>
        /// wap
        /// </summary>
        blms_wap = 4
    }
}
