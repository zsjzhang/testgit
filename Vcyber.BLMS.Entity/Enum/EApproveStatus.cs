using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Common;

namespace Vcyber.BLMS.Entity.Enum
{
    public enum EApproveStatus
    {
        [EnumDescribe("未审核")]
        NoBegin = 0,
        [EnumDescribe("审核通过")]
        Approved = 1,
        [EnumDescribe("审核未通过")]
        NotApproved = 2
    }
}
