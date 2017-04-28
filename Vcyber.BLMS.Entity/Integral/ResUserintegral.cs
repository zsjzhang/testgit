using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vcyber.BLMS.Entity
{

    /// <summary>
    /// 蓝缤会员积分明细实体
    /// </summary>
    public class ResUserintegral
    {
        /// <summary>
        /// 经销商Id
        /// </summary>
        public string DealerId { get; set; }

        /// <summary>
        /// 0新增，1消费
        /// </summary>
        public string Type { set; get; }

        /// <summary>
        /// 积分变动时间
        /// </summary>
        public string UpdateDate { set; get; }


        /// <summary>
        /// 积分变动数值
        /// </summary>
        public string Point { set; get; }


        /// <summary>
        /// 积分变动原因
        /// </summary>
        public string Remark { set; get; }


        /// <summary>
        ///  操作人
        /// </summary>
        public string Operator { get; set; }


    }
}