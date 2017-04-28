using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Common;

namespace Vcyber.BLMS.Entity.Enum
{
    public enum EUserType
    {
        [EnumDescribe("LF")]
        LF = 1,

        [EnumDescribe("Tlc")]
        Tlc = 2,

        [EnumDescribe("TOP")]
        TOP = 3
    }
}
