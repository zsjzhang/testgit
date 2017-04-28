using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.Entity.Generated;
using Vcyber.BLMS.Entity.SelectCondition;

namespace Vcyber.BLMS.IRepository
{
    /// <summary>
    /// 订单操作
    /// </summary>
    public interface IOrderStorager
    {
        #region ==== 前台逻辑 ====

        #region ==== Order ====


        IEnumerable<Order> GetUserOrderPageList(int pageOrderType, string userID, int pageIndex, int pageSize, out int total);
        /// <summary>
        /// 获取用户订单
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="pageData"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        IEnumerable<Order> GetUserOrderList(string userID, PageData pageData, out int total);

        /// <summary>
        /// 添加订单信息
        /// </summary>
        /// <param name="instance">订单信息</param>
        /// <returns></returns>
        void AddOrder(Order instance);

        /// <summary>
        /// 获取用户订单信息
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="pageData"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        IEnumerable<Order> SelectList(string userId, PageData pageData, out int totalCount);

        #endregion

        #region ==== OrderProduct =====

        /// <summary>
        /// 添加订单商品
        /// </summary>
        /// <param name="instance">商品信息</param>
        /// <returns></returns>
        void AddOrderProduct(OrderProduct instance);

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

        /// <summary>
        /// 获取购买商品信息
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="pageData"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        IEnumerable<OrderProduct> SelectUserProduct(string userId, PageData pageData, out int totalCount);

        #endregion

        #region ==== OrderAddress ====

        /// <summary>
        /// 添加订单地址
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        void AddOrderAddress(OrderAddress instance);

        /// <summary>
        /// 修改订单地址
        /// </summary>
        /// <param name="orderID"></param>
        /// <param name="instance"></param>
        /// <returns></returns>
        bool UpdateOrderAddress(string orderID, OrderAddress instance);

        /// <summary>
        /// 获取订单地址
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        OrderAddress GetOrderAddress(string orderID);

        #endregion

        #region ==== OrderShipping ====

        /// <summary>
        /// 设置收货时间
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <returns></returns>
        bool SetReceiveTime(string orderId);

        /// <summary>
        /// 获取订单及发货字段
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pageData"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        IEnumerable<OrderProduct> GetOrdersAndShipping(string userId, PageData pageData, out int totalCount);



        #endregion

        #endregion

        bool BMCancelOrder(Order entity, IEnumerable<OrderProduct> op);

        #region ==== 后台逻辑 ====

        /// <summary>
        /// 获取订单信息
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        Order GetOrder(string orderId);

        /// <summary>
        /// 修改订单支付状态
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        bool EditPayState(string orderId, EPayState state);

        /// <summary>
        /// 修改订单交易状态
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="traderState"></param>
        /// <returns></returns>
        bool EditTradeState(string orderId, ETradeState traderState);

        /// <summary>
        /// 获取订单商品信息
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        IEnumerable<OrderProduct> GetOrderProduct(string orderId);

        /// <summary>
        /// 分页获取订单信息
        /// </summary>
        /// <param name="conditon"></param>
        /// <param name="pageData"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        IEnumerable<Order> GetPageOrder(OrderCondition conditon, PageData pageData, out int total);


        /// <summary>
        /// 订单邮寄信息
        /// </summary>
        /// <param name="oid"></param>
        /// <returns></returns>
        OrderShipping GetOrderShipping(string oid);

        /// <summary>
        /// 添加订单邮寄信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int AddOrderShipping(OrderShipping entity);

        /// <summary>
        /// 修改订单邮寄信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool EditOrderShipping(OrderShipping entity);

        /// <summary>
        /// 添加订单轨迹
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>返回轨迹Id</returns>
        int AddOrderTrack(OrderTrack entity);

        /// <summary>
        /// 分页获取订单轨迹信息
        /// </summary>
        /// <param name="orderID"></param>
        /// <param name="pageData"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        IEnumerable<OrderTrack> GetPageOrderTrack(string orderID, PageData pageData, out int total);


        /// <summary>
        /// 获取物流公司信息
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        Shipping GetShipping(int shippingType);

        /// <summary>
        /// 获取全部物流公司信息
        /// </summary>
        /// <returns></returns>
        IEnumerable<Shipping> GetAllShipping();


        #endregion

        IEnumerable<Order> GetAllOrderList(OrderCondition condition, out int total);

        bool CancelOrder(Order entity);


        bool EditOrderCardCode(string orderId, string cardCode, int productId);


        string GetOrderProductCategoryCardCode(int productId);

        string GetOrderProductCardCode(int productId, string orderId);



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
