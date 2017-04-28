using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Common;

namespace Vcyber.BLMS.Entity
{
    public enum ESendType
    {
        [EnumDescribe("网站")]
        WebSite = 1,

        [EnumDescribe("APP")]
        App = 2,

        [EnumDescribe("系统")]
        System = 3,

        [EnumDescribe("积分兑换")]
        Trade = 4
    }
}
