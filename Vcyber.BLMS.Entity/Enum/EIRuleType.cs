using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Vcyber.BLMS.Common;

namespace Vcyber.BLMS.Entity.Enum
{
    /// <summary>
    /// 积分获取类型
    /// </summary>
    public enum EIRuleType
    {
        [EnumDescribe("维保获取")]
        消费发放 = 1,
        [EnumDescribe("首次购车获取")]
        新购 = 9,
        [EnumDescribe("增换购获取")]
        增购 = 10,
        [EnumDescribe("管理员下发")]
        管理员下发 = 12,
        [EnumDescribe("保客营销活动")]
        保客营销活动 = 15,
         [EnumDescribe("经销商入会返积分")]
        经销商入会返积分=30
    }
}
