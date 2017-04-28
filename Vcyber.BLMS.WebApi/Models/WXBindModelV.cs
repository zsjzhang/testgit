using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vcyber.BLMS.WebApi.Models
{
    public class WXBindModelV
    {
        /// <summary>
        /// 微信open id
        /// </summary>
        public string OpenId { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string Tel { get; set; }
        /// <summary>
        /// 验证码
        /// </summary>
        public string Captcha { get; set; }
        /// <summary>
        /// 车辆Vin
        /// </summary>
        public string Vin { get; set; }
        /// <summary>
        /// 用户姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 微信昵称
        /// </summary>
        public string WXNickname { get; set; }

        //一下三个变量因为url的原因暂时放到entity里面
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

        /// <summary>
        /// 身份证
        /// </summary>
        public string IdentityNumber { get; set; }
    }
}