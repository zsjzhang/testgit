using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vcyber.BLMS.WebApi.Models.ResponseData
{
    /// <summary>
    /// 返回用户卡券使用信息
    /// </summary>
    public class ResUserAwardCustomCardInfo
    {
        /// <summary>
        /// 核销状态 1:已使用，0：未使用
        /// </summary>
        public string Status { set; get; }
        
        /// <summary>
        /// 不能使用的原因
        /// </summary>
        public string ResultMsg{ set; get; }

        
    }
}