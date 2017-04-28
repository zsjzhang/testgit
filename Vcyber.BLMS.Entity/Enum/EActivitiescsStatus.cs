using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Common;

namespace Vcyber.BLMS.Entity.Enum
{
    public enum EActivitiescsStatus
    {
        [EnumDescribe("未开始")]
        NoBegin = 1,
        [EnumDescribe("进行中")]
        InProcess = 2,
        [EnumDescribe("已结束")]
        Finished = 3
    }
    //0 进行中   1已结束   2 未开始
    public enum EPermuteActivitiescsStatus
    {
        [EnumDescribe("进行中")]
        InProcess = 0,
        [EnumDescribe("已结束")]
        Finished = 1,
        [EnumDescribe("未开始")]
        NoBegin = 2,
        [EnumDescribe("未知")]
        UnKnow 

    }
}
