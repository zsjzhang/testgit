using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Common;

namespace Vcyber.BLMS.Entity.Enum
{
    public enum EUserState
    {
        [EnumDescribe("未激活")]
        InActive = 0,

        [EnumDescribe("正常")]
        Active = 1,

        [EnumDescribe("已删除")]
        Delete = 2

    }
}
