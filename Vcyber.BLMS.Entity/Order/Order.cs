using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    /// <summary>
    /// 订单信息
    /// </summary>
    public class Order
    {
        #region ==== 构造函数 ====

        public Order()
        { }

        #endregion

        #region ==== 公共属性 ====

        public int ID { get; set; }

        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderId { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// 订单类型
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 商品形式
        /// </summary>
        public int Mode { get; set; }

        /// <summary>
        /// 订单总价
        /// </summary>
        public decimal Total { get; set; }

        /// <summary>
        /// 订单总积分
        /// </summary>
        public int Integral { get; set; }

        /// <summary>
        /// 蓝豆价
        /// </summary>
        public int BlueBean { get; set; }

        /// <summary>
        /// '订单支付状态（1：支付中；2：支付完成）'
        /// </summary>
        public int PayState { get; set; }

        /// <summary>
        /// 交易状态(1:交易中；2:交易完成；3：部分退款；4：全部退款；5：交易取消)
        /// </summary>
        public int TradeState { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Createtime { get; set; }

        /// <summary>
        /// 数据修改时间
        /// </summary>
        public DateTime Updatetime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Datastate { get; set; }


        /// <summary>
        /// 数据渠道(blms:前台网站；blms_web：手机app;blms_wechat：微信)
        /// </summary>
        public string DataSource { get; set; }

        #endregion

        #region ==== 附件信息 ====

        /// <summary>
        /// 订单商品信息集合
        /// </summary>
        public List<Product> Products { get; set; }

        public List<OrderProduct> OrderProduct { get; set; }

        /// <summary>
        /// 订单地址
        /// </summary>
        public OrderAddress OrderAddress { get; set; }

        /// <summary>
        /// 订单轨迹
        /// </summary>
        public List<OrderTrack> OrderTrack { get; set; }

        /// <summary>
        /// 订单物流信息
        /// </summary>
        public OrderShipping Shipping { get; set; }

        /// <summary>
        /// 购买者名称
        /// </summary>
        public string UserName { get; set; }

        #endregion

        public int ShippingType { get; set; }
        public DateTime DeliveryTime { get; set; }
        public string ReceiveName { get; set; }
        public string ReceivePhone { get; set; }
        public string PCC { get; set; }
        public string Detail { get; set;} 
       //public List<string> CardCode { get; set; }

    }
}
