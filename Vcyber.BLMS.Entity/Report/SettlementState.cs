using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Common;

namespace Vcyber.BLMS.Entity
{
    public enum SettlementState
    {
        [EnumDescribe("")]
        None = 0,
        [EnumDescribe("已确认")]
        Confirm = 1,
        [EnumDescribe("待确认")]
        NotConfirm = 2,
        [EnumDescribe("待复核")]
        Review = 3
    }
}
