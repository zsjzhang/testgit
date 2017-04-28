using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Vcyber.BLMS.Application;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.Entity.SelectCondition;
using Vcyber.BLMS.IRepository;


namespace Vcyber.BLMS.Domain
{
    /// <summary>
    /// 订单业务
    /// </summary>
    public class OrderApp : IOrderApp
    {
        #region ==== 构造函数 ====

        public OrderApp() { }

        #endregion

        #region ==== 前台逻辑 ====

        /// <summary>
        /// 获取订单与发货信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pageData"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
       public  IEnumerable<OrderProduct> GetOrdersAndShipping(string userId, PageData pageData, out int totalCount)
        {
            return _DbSession.OrderStorager.GetOrdersAndShipping(userId, pageData, out totalCount);
        }


       public List<Order> GetUserOrderPageList(int pageOrderType,   string userID, int pageIndex, int pageSize, out int total)
       {
           List<Order> dataResult = new List<Order>();
           IEnumerable<Order> orders = _DbSession.OrderStorager.GetUserOrderPageList( pageOrderType, userID, pageIndex, pageSize, out total);

           if (orders == null || orders.Count() <= 0)
           {
               return dataResult;
           }

           foreach (var orderItem in orders)
           {
               orderItem.Products = new List<Product>(3);
               IEnumerable<OrderProduct> orderProducts = _DbSession.OrderStorager.GetOrderProduct(orderItem.OrderId);

               if (orderProducts != null && orderProducts.Count() > 0)
               {
                   foreach (var productItem in orderProducts)
                   {
                       Product product = _DbSession.ProductStorager.GetProductById(productItem.ProductID);

                       if (product != null)
                       {
                           orderItem.Products.Add(product);
                       }
                   }
               }
             OrderShipping ordershipping = _DbSession.OrderStorager.GetOrderShipping(orderItem.OrderId);
             orderItem.Shipping = ordershipping;
              
               dataResult.Add(orderItem);
           }

           return dataResult;
       }
        /// <summary>
        /// 获取用户积分兑换订单
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<Order> GetIntegralOrderList(string userID, PageData pageData, out int total)
        {
            List<Order> dataResult = new List<Order>();
            IEnumerable<Order> orders = _DbSession.OrderStorager.GetUserOrderList(userID, pageData, out total);

            if (orders == null || orders.Count() <= 0)
            {
                return dataResult;
            }

            foreach (var orderItem in orders)
            {
                orderItem.Products = new List<Product>(3);
                IEnumerable<OrderProduct> orderProducts = _DbSession.OrderStorager.GetOrderProduct(orderItem.OrderId);

                if (orderProducts != null && orderProducts.Count() > 0)
                {
                    foreach (var productItem in orderProducts)
                    {
                        Product product = _DbSession.ProductStorager.GetProductById(productItem.ProductID);

                        if (product != null)
                        {
                            orderItem.Products.Add(product);
                        }
                    }
                }

                dataResult.Add(orderItem);
            }

            return dataResult;
        }

        /// <summary>
        /// 获取购买商品信息
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="pageData"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public IEnumerable<OrderProduct> GetUserProduct(string userId, PageData pageData, out int totalCount)
        {
            return _DbSession.OrderStorager.SelectUserProduct(userId, pageData, out totalCount);
        }


        /// <summary>
        /// 获取用户单个积分订单信息
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        public Order GetIntegralOrderOne(string orderID)
        {
            Order order = _DbSession.OrderStorager.GetOrder(orderID);

            if (order != null)
            {
                IEnumerable<OrderProduct> orderProducts = _DbSession.OrderStorager.GetOrderProduct(order.OrderId);

                if (orderProducts != null && orderProducts.Count() > 0)
                {
                    foreach (var productItem in orderProducts)
                    {
                        Product product = _DbSession.ProductStorager.GetProductById(productItem.ProductID);

                        if (product != null)
                        {
                            order.Products.Add(product);
                        }
                    }
                }

                return order;
            }

            return null;
        }

        /// <summary>
        /// 修改订单交易状态
        /// </summary>
        /// <param name="orderID"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public bool UpdateTradeState(string orderID, ETradeState state)
        {
            return _DbSession.OrderStorager.EditTradeState(orderID, state);
        }

        /// <summary>
        /// 修改订单物流收货时间
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        public bool UpdateShippingTime(OrderShipping orderShipping)
        {
            return _DbSession.OrderStorager.EditOrderShipping(orderShipping);
        }

        /// <summary>
        /// 获取订单地址
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        public OrderAddress GetOrderAddress(string orderID)
        {
            return _DbSession.OrderStorager.GetOrderAddress(orderID);
        }

        /// <summary>
        /// 获取订单物流信息
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        public OrderShipping GetOrderShipping(string orderID)
        {
            return _DbSession.OrderStorager.GetOrderShipping(orderID);
        }

        /// <summary>
        /// 获取用户 对某个商品的购买次数
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <param name="productID">商品ID</param>
        /// <returns></returns>
        public int GetUserOderCount(int userID, int productID)
        {
            return _DbSession.OrderStorager.GetUserOderCount(userID, productID);
        }

        /// <summary>
        /// 获取用户最新积分订单 数据更新次数
        /// </summary>
        /// <param name="date"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int RecentlyOrder(DateTime date, int userId)
        {
            return _DbSession.OrderStorager.RecentlyOrder(date, userId);
        }

        #endregion

        #region ==== 后台逻辑 ====

        /// <summary>
        /// 获取订单信息
        /// </summary>
        /// <param name="oid"></param>
        /// <returns></returns>
        public Order GetOrder(string oid)
        {
            return _DbSession.OrderStorager.GetOrder(oid);
        }

        /// <summary>
        /// 获取订单商品信息
        /// </summary>
        /// <param name="oid"></param>
        /// <returns></returns>
        public IEnumerable<OrderProduct> GetOrderProduct(string oid)
        {
            return _DbSession.OrderStorager.GetOrderProduct(oid);
        }

        /// <summary>
        /// 分页获取订单信息
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="pageData"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public IEnumerable<Order> GetPageOrder(OrderCondition condition, PageData pageData, out int total)
        {
            return _DbSession.OrderStorager.GetPageOrder(condition, pageData, out total);
        }

        /// <summary>
        /// 确认收货
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <returns></returns>
        public bool ConfirmOrder(string orderId)
        {
            bool result = _DbSession.OrderStorager.EditTradeState(orderId, ETradeState.JYWC);

            if (result)
            {
                result = _DbSession.OrderStorager.SetReceiveTime(orderId);
            }

            return result;
        }

        /// <summary>
        /// 取消订单
        /// </summary>
        /// <param name="oid"></param>
        /// <returns></returns>
        public bool Cancel(string oid)
        {
            //using (TransactionScope tran = new TransactionScope())
            //{
            try
            {
                bool bol = _DbSession.OrderStorager.EditTradeState(oid, ETradeState.JYQX);

                if (bol)
                {
                    IEnumerable<OrderProduct> list = _DbSession.OrderStorager.GetOrderProduct(oid);

                    if (list != null && list.Count() > 0)
                    {
                        foreach (OrderProduct item in list)
                        {
                            _DbSession.ProductStorager.UnLQty(item.ProductID, item.Qty);
                        }

                        // tran.Complete();
                        return true;
                    }
                }

                return false;
            }
            catch
            {
                return false;
            }
            //}
        }

        /// <summary>
        /// 订单发货
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool DeliveryOrder(OrderShipping entity)
        {
            //using (TransactionScope tran = new TransactionScope())
            //{
            //判断是否存在物流信息
            bool bol = _DbSession.OrderStorager.EditOrderShipping(entity);

            if (bol == false)
            {
                int id = _DbSession.OrderStorager.AddOrderShipping(entity);
            }

            bol = _DbSession.OrderStorager.EditTradeState(entity.OrderID, ETradeState.YFH);

            if (bol)
            {
                //tran.Complete();
                return true;
            }

            return false;
            // }
        }

        /// <summary>
        /// 修改订单支付状态
        /// </summary>
        /// <param name="oid"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public bool EditPayState(string oid, EPayState state)
        {
            bool bol = _DbSession.OrderStorager.EditPayState(oid, state);
            return bol;
        }

        /// <summary>
        /// 修改交易状态
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="tradeState"></param>
        /// <returns></returns>
        public bool EditTradeState(string orderId, ETradeState tradeState)
        {
            return _DbSession.OrderStorager.EditTradeState(orderId, tradeState);
        }

        public int AddOrderShipping(OrderShipping entity)
        {
            return _DbSession.OrderStorager.AddOrderShipping(entity);
        }
        public bool EditOrderShipping(OrderShipping entity)
        {
            return _DbSession.OrderStorager.EditOrderShipping(entity);
        }

        /// <summary>
        /// 添加订单轨迹
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int AddOrderTrack(OrderTrack entity)
        {
            return _DbSession.OrderStorager.AddOrderTrack(entity);
        }

        /// <summary>
        /// 分页获取订单轨迹
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="pageData"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public IEnumerable<OrderTrack> GetPageOrderTrack(string orderId, PageData pageData, out int total)
        {
            return _DbSession.OrderStorager.GetPageOrderTrack(orderId, pageData, out total);
        }

        /// <summary>
        /// 获取物流公司信息
        /// </summary>
        /// <param name="type">物流公司类型</param>
        /// <returns></returns>
        public Shipping GetShipping(int type)
        {
            return _DbSession.OrderStorager.GetShipping(type);
        }

        /// <summary>
        /// 获取全部物流信息
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Shipping> GetAllShipping()
        {
            return _DbSession.OrderStorager.GetAllShipping();
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public IEnumerable<Order> GetAllOrderList(OrderCondition condition, out int total)
        {
            return _DbSession.OrderStorager.GetAllOrderList(condition, out total);
        }

        /// <summary>
        /// 取消订单
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool CancelOrder(Order entity)
        {
            return _DbSession.OrderStorager.CancelOrder(entity);
        }

        public bool BMCancelOrder(Order entity, IEnumerable<OrderProduct> op)
        {
            return _DbSession.OrderStorager.BMCancelOrder(entity,op);
        }



        public bool EditOrderCardCode(string orderId, string cardCode, int productId)
        {
            return _DbSession.OrderStorager.EditOrderCardCode(orderId, cardCode, productId);
        }


        public string GetOrderProductCategoryCardCode(int productId)
        {
            return _DbSession.OrderStorager.GetOrderProductCategoryCardCode(productId);
        }


        public string GetOrderProductCardCode(int productId, string orderId)
        {
            return _DbSession.OrderStorager.GetOrderProductCardCode(productId, orderId);
        }


        public bool SetReceiveTime(string orderId)
        {
            return _DbSession.OrderStorager.SetReceiveTime(orderId);
        }


        public IEnumerable<OrderProduct> GetUserOrderProductList(string userId, PageData pageData, out int totalCount)
        {
            return _DbSession.OrderStorager.GetUserOrderProductList(userId, pageData, out totalCount);
        }
    }
}
