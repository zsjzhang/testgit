using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Common;

namespace Vcyber.BLMS.Entity.Enum
{
    /// <summary>
    /// 服务卡状态
    /// </summary>
    public enum EServiceCardStatus
    {
        [EnumDescribe("未发放")]
        WF = 1,

        [EnumDescribe("已发放")]
        YF = 2,

        [EnumDescribe("已核销")]
        YHX = 3,

        [EnumDescribe("已结算")]
        YJS = 4
    }
}
