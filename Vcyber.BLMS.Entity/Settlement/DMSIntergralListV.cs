using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;

namespace Vcyber.BLMS.Entity
{
    /// <summary>
    /// 交易信息
    /// </summary>
    public class DMSIntergralListV
    {
        #region ==== 构造函数 ====

        public DMSIntergralListV() { }

        #endregion

        #region ==== 公共属性 ====

        /// <summary>
        /// 经销商Id
        /// </summary>  
        public string DealerId { get; set; }

        

        /// <summary>
        /// 结算开始时间
        /// </summary>
        public DateTime FromTime { get; set; }

        /// <summary>
        /// 结算结束时间
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 结算总积分
        /// </summary>
        public int TotalPoint {get; set;}

        /// <summary>
        /// 结算积分兑换金额
        /// </summary>
        public decimal TotalMoney { get; set; }

        /// <summary>
        /// 是否同意结算Y/N
        /// </summary>
        public string ISAgree { get; set; }


        /// <summary>
        /// 备注
        /// </summary>
        public string Comment { get; set; }
        /// <summary>
        /// 积分结算清单
        /// </summary>
        public List<IntegraListV> IntegraList { get; set; }

        #endregion

    }


}