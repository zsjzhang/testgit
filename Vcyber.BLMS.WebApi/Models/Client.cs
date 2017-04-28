using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Vcyber.BLMS.WebApi.Models
{
    /// <summary>
    /// 客户端
    /// </summary>
    [DataContract]
    public class Client
    {
        /// <summary>
        /// 客户端ID
        /// </summary>
        [DataMember]
        public string Id { get; set; }

        /// <summary>
        /// 客户端密码
        /// </summary>
        [DataMember]
        public string Secret { get; set; }

        /// <summary>
        /// 返回URL
        /// </summary>
        [DataMember]
        public string RedirectUrl { get; set; }

        /// <summary>
        /// 是否为可信客户端
        /// </summary>
        [DataMember]
        public bool IsTrusted { get; set; }

        /// <summary>
        /// 是否需要验证码验证
        /// </summary>
        [DataMember]
        public bool RequiredVerificationCode { get; set; }

        /// <summary>
        /// 客户端所具有的Scope
        /// </summary>
        [DataMember]
        public List<string> ClientScope { get; set; }
    }

    [DataContract]
    public class ClientConfig
    {
        [DataMember]
        public List<Client> Clients{ get; set; }
    }
}