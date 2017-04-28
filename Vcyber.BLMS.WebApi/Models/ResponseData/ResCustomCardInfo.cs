using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vcyber.BLMS.WebApi.Controllers;

namespace Vcyber.BLMS.WebApi.Models.ResponseData
{
    /// <summary>
    /// 返回查询卡券是否可用
    /// </summary>
    public class ResCustomCardInfo
    {
        /// <summary>
        /// 不能使用的原因
        /// </summary>
        public string Msg { set; get; }
        /// <summary>
        /// "S" : SUCCESS / "F" : FAIL 
        /// </summary>
        public string IfResult { set; get; }
    }

    /// <summary>
    /// 返回查询卡券基本信息
    /// </summary>
    public class ResCustomCardInfoByDMS
    {
        /// <summary>
        /// 不能使用的原因
        /// </summary>
        public string Msg { set; get; }
        /// <summary>
        /// "S" : SUCCESS / "F" : FAIL 
        /// </summary>
        public string IfResult { set; get; }

        /// <summary>
        /// 卡券抵扣金额
        /// </summary>
        public string ReduceCost { set; get; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Tel { set; get; }

        /// <summary>
        /// 卡券有效开始日期
        /// </summary>
        public string CardBeginDate { set; get; }

        /// <summary>
        /// 卡券失效日期
        /// </summary>
        public string CardEndDate { set; get; }

        /// <summary>
        /// 卡券优惠说明
        /// </summary>
        public string CardRemark { set; get; }

        /// <summary>
        /// 卡券类型（1：工时费；2：配件费；3：维修费）
        /// </summary>
        public string CardCategory { set; get; }

    }
}