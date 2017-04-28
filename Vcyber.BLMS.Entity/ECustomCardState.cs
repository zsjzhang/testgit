using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Common;

namespace Vcyber.BLMS.Entity
{
    public enum ECustomCardReceiveState
    {
        [EnumDescribe("已领取")]
        Yes = 1,
        [EnumDescribe("未领取")]
        No = 2

    }
}
