using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    /// <summary>
    /// 订单商品信息
    /// </summary>
    public class OrderProduct
    {
        #region ==== 构造函数 ====

        public OrderProduct()
        { }

        #endregion

        #region ==== 公共属性 ====

        public int ID { get; set; }

        /// <summary>
        /// 订单ID
        /// </summary>
        public string OrderID { get; set; }
        /// <summary>
        /// 商品ID
        /// </summary>
        public int ProductID { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Qty { get; set; }

        /// <summary>
        /// 价格
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 商品积分
        /// </summary>
        public int Integral { get; set; }

        /// <summary>
        /// 蓝豆价
        /// </summary>
        public int BlueBean { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 退货回复
        /// </summary>
        public string Reply { get; set; }

        /// <summary>
        /// 退货状态
        /// </summary>
        public int State { get; set; }

        /// <summary>
        /// 商品颜色
        /// </summary>
        public string ProductColor { get; set; }

        /// <summary>
        /// 商品属性
        /// </summary>
        public string ProductType { get; set; }


        /// <summary>
        /// 卡券兑换码
        /// </summary>
        public string CardCode { set; get; }

        #endregion

        #region ==== 附件属性 ====

        /// <summary>
        /// 商品图片
        /// </summary>
        public string Img { get; set; }

        /// <summary>
        /// 商品兑换时间
        /// </summary>
        public DateTime Createtime { get; set; }

        /// <summary>
        ///物流公司名称
        /// </summary>	
        public string ShippingName { get; set; }

        /// <summary>
        /// 快递单号
        /// </summary>	
        public string Number { get; set; }


        /// <summary>
        /// 订单状态
        /// </summary>
        public string TradeState { get; set; }
        /// <summary>

        #endregion
    }
}
