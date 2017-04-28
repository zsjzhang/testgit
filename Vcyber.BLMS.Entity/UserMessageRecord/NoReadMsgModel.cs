using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    public class NoReadMsgModel
    {
        /// <summary>
        /// 未读系统消息
        /// </summary>
        public int SysMsgCount { get; set; }

        /// <summary>
        /// 未读积分变动消息
        /// </summary>
        public int IntegralchangeMsgCount { get; set; }

        /// <summary>
        /// 未读卡券消息
        /// </summary>
        public int CardcouponMsgCount { get; set; }

        /// <summary>
        /// 未读活动和服务消息
        /// </summary>
        public int ActAndServerCount { get; set; }
    }
}
