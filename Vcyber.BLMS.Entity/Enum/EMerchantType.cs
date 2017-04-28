using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Common;

namespace Vcyber.BLMS.Entity.Enum
{
    /// <summary>
    ///  卡券来源，归属
    /// </summary>
    public enum EMerchantType
    {
        [EnumDescribe("北京现代")]
        Bjxd = 1,
        [EnumDescribe("合作商户")]
        Partner = 2
    }

    /// <summary>
    /// 卡券有效期，1：固定时间；2：领取后生效
    /// </summary>
    public enum ECardValidityType
    {
        [EnumDescribe("固定时间")]
        Fixed = 1,
        [EnumDescribe("领取后生效")]
        After = 2
    }

    /// <summary>
    /// 优惠劵，活动类型
    /// </summary>
    public enum ECustomerCardActType
    {
        [EnumDescribe("普通活动")]
        Common = 1,
        [EnumDescribe("夏季保养活动")]
        Summer = 2,
        [EnumDescribe("5种养护产品")]
        Maintain = 3,
    }
}