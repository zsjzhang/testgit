using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vcyber.BLMS.WebApi.Models.RequestData
{
    public class RequestMembershipApproval
    {
        /// <summary>
        /// 经销商Id
        /// </summary>
        public string DealerId { get; set; }


        /// <summary>
        /// 手机号码
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        ///   VIN 码
        /// </summary>
        public string Vin { get; set; }


        /// <summary>
        /// 身份证号
        /// </summary>
        public string IdentityNumber { get; set; }


    }
}