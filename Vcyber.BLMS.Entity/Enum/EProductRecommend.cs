using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Common;

namespace Vcyber.BLMS.Entity.Enum
{
    /// <summary>
    /// 商品推荐
    /// </summary>
    public enum EProductRecommend
    {
        /// <summary>
        /// 普通
        /// </summary>
        [EnumDescribe("普通")]
        BT = 0,

        /// <summary>
        /// 新品
        /// </summary>
        [EnumDescribe("新品")]
        XP = 1,

        /// <summary>
        /// 热销
        /// </summary>
        [EnumDescribe("热销")]
        RX = 2
    }
}
