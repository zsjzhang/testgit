using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vcyber.BLMS.WebApi.Models.RequestData
{
    /// <summary>
    /// 会员新增消费记录参数实体；
    /// </summary>
    public class RequestCancelConsume
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId { get; set; }


        /// <summary>
        /// 经销商Id
        /// </summary>
        public string DealerId { get; set; }


        /// <summary>
        /// 消费类型（0：事故车维修（普通），1：首次保养，2：购车，3：定期保养，5：配件，6：精品，8：钣喷）
        /// </summary>
        public string ConsumeType { get; set; }

      

        /// <summary>
        /// 总费用
        /// </summary>
        public string TotalCost { get; set; }

        /// <summary>
        /// 消耗积分
        /// </summary>
        public string ConsumePoints { get; set; }

        /// <summary>
        /// 返还积分
        /// </summary>
        public string RewardPoints { get; set; }
        /// <summary>
        /// 消费时间
        /// </summary>
        public string ConsumeDate { get; set; }

        /// <summary>
        /// 用户手机号
        /// </summary>
        public string PhoneNumber { get; set; }


        /// <summary>
        ///  录入人ID
        /// </summary>
        public string Operator { get; set; }

        /// <summary>
        /// 录入日期
        /// </summary>
        public string OperatorDate { get; set; }


        /// <summary>
        /// 身份证号
        /// </summary>
        public string IdentityNumber { get; set; }


        /// <summary>
        /// 车架号
        /// </summary>
        public string VIN { get; set; }


    }
}