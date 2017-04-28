using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Common;

namespace Vcyber.BLMS.Entity
{
    public enum ESNCardStatus
    {
        /// <summary>
        /// 未发放
        /// </summary>
        [EnumDescribe("未发放")]
        Created = 1,

        /// <summary>
        /// 已发放
        /// </summary>
        [EnumDescribe("已发放")]
        Send = 2,

        /// <summary>
        /// 已消费
        /// </summary>
        [EnumDescribe("已消费")]
        Used = 3
    }
}
