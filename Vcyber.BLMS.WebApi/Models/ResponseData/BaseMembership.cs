using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vcyber.BLMS.WebApi.Models.ResponseData
{

    /// <summary>
    ///会员基本信息
    /// </summary>
    public class BaseMembership
    {
        /// <summary>
        /// 会员编号
        /// </summary>
        public string BlueMembership_Id { get; set; }
        /// <summary>
        /// 会员卡号
        /// </summary>
        public string BlueMembership_No { get; set; }
        /// <summary>
        /// 会员等级1-注册用户 10-普卡  11-银卡 12-金卡
        /// </summary>
        public string MLevel { get; set; }



    }
}