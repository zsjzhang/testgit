using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Common;

namespace Vcyber.BLMS.Entity
{
    public enum MessageType
    {
        /// <summary>
        /// 全部消息
        /// </summary>
        [EnumDescribe("全部消息")]
        All = 0,

        /// <summary>
        /// 系统消息
        /// </summary>
        [EnumDescribe("系统消息")]
        System = 1,

        /// <summary>
        ///维修保养提醒
        /// </summary>
        [EnumDescribe("保养提醒")]
        Maintain = 2,

        /// <summary>
        /// 积分变动提醒
        /// </summary>
        [EnumDescribe("积分变动提醒")]
        IntegralConsum = 3,

        /// <summary>
        /// 卡券消息
        /// </summary>
        [EnumDescribe("卡券消息")]
        CardMessage = 4,

        /// <summary>
        /// 服务活动
        /// </summary>
        [EnumDescribe("服务和活动")]
        ServiceAcitive = 5

    }
}
