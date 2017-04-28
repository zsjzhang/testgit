using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Common;

namespace Vcyber.BLMS.Entity.Enum
{
    /// <summary>
    /// 订单形式
    /// </summary>
    public enum EOrderMode
    {
        /// <summary>
        /// 邮寄实物
        /// </summary>
        [EnumDescribe("礼品")]
        YJSW = 1,

        /// <summary>
        /// 维修
        /// </summary>
        [EnumDescribe("维修")]
        Repair = 2,

        /// <summary>
        /// 保养
        /// </summary>
        [EnumDescribe("保养")]
        Maintain = 3,

        /// <summary>
        /// 购车
        /// </summary>
        [EnumDescribe("购车")]
        Purchase = 4,

        /// <summary>
        /// 机场服务
        /// </summary>
        [EnumDescribe("机场服务")]
        JCService = 5
    }
}
