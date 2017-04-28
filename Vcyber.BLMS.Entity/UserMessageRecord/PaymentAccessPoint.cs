using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vcyber.BLMS.Entity
{
    //缴费获取积分接口实体
    public class PaymentAccessPoint
    {

        /// <summary>
        /// 经销商ID
        /// </summary>
        public string DealerId { get; set; }

        /// <summary>
        ///手机号
        /// </summary>
        public string Tel { get; set;}

        /// <summary>
        /// 身份证号
        /// </summary>
        public string IdentityNumber { get; set; }

        /// <summary>
        ///应付金额
        /// </summary>
        public string Amount { get; set; }

        /// <summary>
        /// 车主姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 是否付费 
        /// 是否已支付会员入会费 0:未支付 1:已支付 2:用户已提交支付
        /// </summary>
        public string Ispay { get; set; }

        /// <summary>
        /// 付款码
        /// </summary>
        public string PayNumber { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        public string createtime { get; set; }

        /// <summary>
        /// 状态
        /// 1：待审批，3：审批通过，4：未通过
        /// </summary>
        public string Status { get; set; }
    }
}