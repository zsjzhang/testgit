using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vcyber.BLMS.WebApi.Models.ResponseData
{
    public class ResUserOrderProducts
    {
        /// <summary>
        /// 订单ID
        /// </summary>
        public string OrderId { set; get; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName { set; get; }

        /// <summary>
        /// 商品积分
        /// </summary>
        public string Integral { get; set; }


        /// <summary>
        /// 数量
        /// </summary>
        public string ProductQty { get; set; }


        /// <summary>
        /// 下单时间
        /// </summary>
        public string OrderDate { set; get; }



        /// <summary>
        /// 商品图片
        /// </summary>
        public string ProductImg { get; set; }


        /// <summary>
        /// 订单状态 交易中：1；交易完成：2； 部分退款：3； 全部退款：4； 交易取消：5；  已发货：6；待发货：17
        /// </summary>
        public string OrderState { get; set; }


    }
}