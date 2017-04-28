using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vcyber.BLMS.WebApi.Models.ResponseData
{
    public class ResOrderProductDetail
    {
        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderId { get; set; }

        /// <summary>
        /// 下单时间
        /// </summary>
        public string OrderTime { get; set; }

        /// <summary>
        /// 订单状态 交易中：1；交易完成：2； 部分退款：3； 全部退款：4； 交易取消：5；  已发货：6；待发货：17
        /// </summary>
        public string TradeState { get; set; }

        /// <summary>
        /// 支付状态
        /// </summary>
        public string PayState { get; set; }

        /// <summary>
        /// 应付总价
        /// </summary>
        public string OrderTotal { get; set; }

        /// <summary>
        /// 订单总积分
        /// </summary>
        public string Integral { get; set; }

        /// <summary>
        /// 收货人姓名
        /// </summary>
        public string ReceiveName { get; set; }


        /// <summary>
        /// 电话
        /// </summary>
        public string ReceivePhone { get; set; }

        /// <summary>
        /// 收货地址
        /// </summary>
        public string OrderAddress { get; set; }


        /// <summary>
        ///物流公司
        /// </summary>	
        public string ShippingName { get; set; }



        /// <summary>
        /// 快递单号
        /// </summary>	
        public string ShippingNumber { get; set; }

        /// <summary>
        /// 收货时间
        /// </summary>
        public DateTime ReceiveTime { get; set; }

        /// <summary>
        /// 支付时间
        /// </summary>
        public DateTime PayTime { get; set; }

        public List<Products> Product { get; set; }

    }


    public class Products
    {
        /// <summary>
        /// 商品总价
        /// </summary>
        public string ProductTotal { get; set; }

        /// <summary>
        /// 商品图片
        /// </summary>
        public string ProductImg { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName { get; set; }


        /// <summary>
        /// 兑换数量
        /// </summary>
        public string ProductQty { get; set; }
    }
}