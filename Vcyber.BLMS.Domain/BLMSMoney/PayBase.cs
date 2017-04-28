using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Vcyber.BLMS.Common;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.IRepository;

namespace Vcyber.BLMS.Domain
{
    /// <summary>
    /// 支付服务
    /// </summary>
    internal abstract class PayBase
    {
        #region ==== 私有字段 ====

        #endregion

        #region ==== 保护方法 ====

        /// <summary>
        /// 添加交易记录
        /// </summary>
        /// <param name="orderData">订单信息</param>
        /// <param name="flowData">交易流水信息</param>
        /// <returns>true:成功</returns>
        internal static bool AddTradeRecord(Order orderData)
        {
            //using (TransactionScope scope = new TransactionScope())
            //{
            try
            {   
                if (orderData != null)
                {
                    _DbSession.OrderStorager.AddOrder(orderData);

                    if (orderData.OrderProduct != null)
                    {
                        foreach (var productItem in orderData.OrderProduct)
                        {
                            bool result = _DbSession.ProductStorager.SubtractionQty(productItem.ProductID, productItem.Qty);

                            if (!result)
                            {
                                return false;
                            }

                            productItem.OrderID = orderData.OrderId;
                            _DbSession.OrderStorager.AddOrderProduct(productItem);
                        }

                        DelShopping(orderData.UserID, orderData.OrderProduct);
                    }

                    if (orderData.OrderAddress != null)
                    {
                        orderData.OrderAddress.OrderID = orderData.OrderId;
                        _DbSession.OrderStorager.AddOrderAddress(orderData.OrderAddress);
                    }
                }

                //scope.Complete();
                return true;
            }
            catch (Exception ex)
            {
                LogService.Instance.Error(ex.Message, ex);
                return false;
            }
            //}
        }

        #endregion

        #region ==== 抽象方法 ====

        /// <summary>
        /// 扣除货币金额
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="moneyValue">交易货币金额</param>
        /// <param name="flowData"></param>
        /// <returns></returns>
        public abstract bool SubMoney(string userId, int moneyValue, Tradeflow flowData);

        #endregion

        #region ==== 私有方法 ====

        /// <summary>
        /// 删除购物车信息
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="products">订单商品信息</param>
        private static void DelShopping(string userID, List<OrderProduct> products)
        {
            var productIDs = products.Select<OrderProduct, int>((d) => { return d.ProductID; });
            _DbSession.ShoppingStorager.DeleteList(userID, productIDs.ToList<int>());
        }


        #endregion
    }
}
