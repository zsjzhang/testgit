using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vcyber.BLMS.FrontWeb.Models.WeChat
{
    /// <summary>
    /// 错误返回
    /// </summary>
    public class ErrorMsg
    {
        /// <summary>
        /// 错误码
        /// </summary>
        public String errcode { get; set; }

        /// <summary>
        /// 错误消息
        /// </summary>
        public String errmsg { get; set; }
    }
}