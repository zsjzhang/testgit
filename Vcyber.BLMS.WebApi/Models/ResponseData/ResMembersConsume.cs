using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vcyber.BLMS.WebApi.Models.ResponseData
{

    /// <summary>
    /// 返回会员积分消费信息
    /// </summary>
    public class ResMembersConsume
    {

        /// <summary>
        /// 剩余积分
        /// </summary>
        public string Point
        {
            get;
            set;
        }
        /// <summary>
        /// 积分抵扣（积累）固有号码
        /// </summary>
        public string PointSeq
        {
            get;
            set;
        }

        ///// <summary>
        ///// 消费工单号
        ///// </summary>
        //public string OrderNo
        //{
        //    get;
        //    set;
          
        //}
    }
}