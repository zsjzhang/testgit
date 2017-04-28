using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vcyber.BLMS.WebApi.Models
{
    public class WXUnbindModelV
    {
        /// <summary>
        /// 微信open id
        /// </summary>
        public string OpenId { get; set; }
        /// <summary>
        /// 用户Id
        /// </summary>
        public string UserId { get; set; }

        //安全验证变量
        /// <summary>
        /// WXAppId
        /// </summary>
        public string WXAppId { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        public string WXTimestamp { get; set; }

        /// <summary>
        /// 利用加密函数生成的ticket,
        /// </summary>
        public string WXTicket { get; set; }
       
    }
}