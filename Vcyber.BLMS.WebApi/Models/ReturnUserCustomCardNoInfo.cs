using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vcyber.BLMS.WebApi.Models
{
    /// <summary>
    /// 返回用户领取兑奖码信息
    /// </summary>
    /// <typeparam >兑奖码信息</typeparam>
    public class ReturnUserCustomCardNoInfo
    {
        /// <summary>
        /// 返回成功：Success，失败:Error；
        /// </summary>
        public string Message { set; get; }
        /// <summary>
        /// 返回状态：成功：true，失败:false；
        /// </summary>
        public bool IsSuccess { set; get; }
        /// <summary>
        /// 兑奖码信息
        /// </summary>
        public string Code { set; get; }

    }
}