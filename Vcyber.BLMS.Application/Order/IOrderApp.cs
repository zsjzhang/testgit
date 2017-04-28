using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.Entity.SelectCondition;

namespace Vcyber.BLMS.Application
{
    /// <summary>
    /// 订单业务
    /// </summary>
    public interface IOrderApp
    {
        #region ==== 前台逻辑 ====

        /// <summary>
        /// 获取订单与发货信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pageData"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        IEnumerable<OrderProduct> GetOrdersAndShipping(string userId, PageData pageData, out int totalCount);
        /// <summary>
        /// 获取用户积分兑换订单
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        List<Order> GetIntegralOrderList(string userID, PageData pageData, out int total);
        List<Order> GetUserOrderPageList(int pageOrderType, string userID, int pageIndex, int pageSixe, out int total);

        /// <summary>
        /// 获取购买商品信息
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="pageData"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        IEnumerable<OrderProduct> GetUserProduct(string userId, PageData pageData, out int totalCount);

        /// <summary>
        /// 获取用户单个积分订单信息
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        Order GetIntegralOrderOne(string orderID);

        /// <summary>
        /// 修改订单交易状态
        /// </summary>
        /// <param name="orderID"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        bool UpdateTradeState(string orderID, ETradeState state);

        /// <summary>
        /// 修改订单物流收货时间
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        bool UpdateShippingTime(OrderShipping orderShipping);

        /// <summary>
        /// 获取订单地址
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        OrderAddress GetOrderAddress(string orderID);

        /// <summary>
        /// 获取订单物流信息
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        OrderShipping GetOrderShipping(string orderID);

        /// <summary>
        /// 获取用户 对某个商品的购买次数
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <param name="productID">商品ID</param>
        /// <returns></returns>
        int GetUserOderCount(int userID, int productID);

        /// <summary>
        /// 获取用户最新积分订单 数据更新次数
        /// </summary>
        /// <param name="date"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        int RecentlyOrder(DateTime date, int userId);

        #endregion

        #region ==== 后台逻辑 ====

        /// <summary>
        /// 获取订单信息
        /// </summary>
        /// <param name="oid"></param>
        /// <returns></returns>
        Order GetOrder(string oid);

        /// <summary>
        /// 获取订单商品信息
        /// </summary>
        /// <param name="oid"></param>
        /// <returns></returns>
        IEnumerable<OrderProduct> GetOrderProduct(string oid);

        /// <summary>
        /// 分页获取订单信息
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="pageData"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        IEnumerable<Order> GetPageOrder(OrderCondition condition, PageData pageData, out int total);

        /// <summary>
        /// 取消订单
        /// </summary>
        /// <param name="oid"></param>
        /// <returns></returns>
        bool Cancel(string oid);

        /// <summary>
        /// 确认收货
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <returns></returns>
        bool ConfirmOrder(string orderId);

        /// <summary>
        /// 订单发货
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool DeliveryOrder(OrderShipping entity);

        /// <summary>
        /// 修改订单支付状态
        /// </summary>
        /// <param name="oid"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        bool EditPayState(string oid, EPayState state);

        /// <summary>
        /// 修改交易状态
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="tradeState"></param>
        /// <returns></returns>
        bool EditTradeState(string orderId, ETradeState tradeState);

        int AddOrderShipping(OrderShipping entity);

        bool EditOrderShipping(OrderShipping entity);

        /// <summary>
        /// 添加订单轨迹
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int AddOrderTrack(OrderTrack entity);

        /// <summary>
        /// 分页获取订单轨迹
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="pageData"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        IEnumerable<OrderTrack> GetPageOrderTrack(string orderId, PageData pageData, out int total);

        /// <summary>
        /// 获取物流公司信息
        /// </summary>
        /// <param name="type">物流公司类型</param>
        /// <returns></returns>
        Shipping GetShipping(int type);

        /// <summary>
        /// 获取全部物流信息
        /// </summary>
        /// <returns></returns>
        IEnumerable<Shipping> GetAllShipping();

        #endregion

        bool BMCancelOrder(Order entity, IEnumerable<OrderProduct> op);

        /// <summary>
        /// 获取所有订单
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        IEnumerable<Order> GetAllOrderList(OrderCondition condition, out int total);

        bool CancelOrder(Order entity);

        bool EditOrderCardCode(string orderId, string cardCode, int productId);


        string GetOrderProductCategoryCardCode(int productId);


        string GetOrderProductCardCode(int productId, string orderId);

        /// <summary>
        /// 设置收货时间
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <returns></returns>
        bool SetReceiveTime(string orderId);




        /// <summary>
        /// 兑换记录分页列表
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="pageData"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        IEnumerable<OrderProduct> GetUserOrderProductList(string userId, PageData pageData, out int totalCount);

    }
}
