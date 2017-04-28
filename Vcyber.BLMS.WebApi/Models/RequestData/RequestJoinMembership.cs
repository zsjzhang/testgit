using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vcyber.BLMS.WebApi.Models.RequestData
{
    /// <summary>
    /// 经销商一键入会参数实体信息
    /// </summary>
    public class RequestJoinMembership : RequestMembership
    {
        /// <summary>
        /// 车架号码
        /// </summary>
        public string VIN { get; set; }
    }
}