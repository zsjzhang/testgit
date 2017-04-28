using AspNet.Identity.SQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Application.BLMSMoney;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.IRepository;

namespace Vcyber.BLMS.Domain.BLMSMoney
{
    /// <summary>
    /// 订单创建信息
    /// </summary>
    public class OrderCreateInfo : OrderCreateInfoBase
    {
        #region ==== 构造函数 ====

        public OrderCreateInfo() { }

        #endregion

        #region ==== 公共属性 ====


        #endregion

        #region ==== 公共方法 ====

        /// <summary>
        /// 验证订单创建数据
        /// </summary>
        /// <param name="orderData">订单数据</param>
        /// <param name="meessageStatus">验证状态（）</param>
        /// <returns></returns>
        public override bool ValidateCreateData(out Order orderData, out string meessageStatus)
        {
            meessageStatus = "00";
            orderData = null;
            List<OrderProduct> products = new List<OrderProduct>(5);
            OrderAddress address = null;

            if (this.shoppingIds == null || this.shoppingIds.Count <= 0)
            {
                meessageStatus = "97";
                return false;
            }

           //add 积分不够返回错误
            int Integral = 0;

            foreach (int shoppingId in this.shoppingIds)
            {
                Shopping data = _AppContext.ShoppingApp.ShoppingOne(userId, shoppingId);

                if (data.BuyMode == 1) //积分购买
                {
                    Integral += data.Qty * data.Integral;
                }

                #region  在线商城电子卡券库存处理，如果卡券库存不足，不提交订单;
                var cardType = _AppContext.OrderApp.GetOrderProductCategoryCardCode(data.ProductID);
                if (!string.IsNullOrEmpty(cardType))
                {
                    var cardInfo = _AppContext.CustomCardInfoApp.GetCustomCardQuantityByCardType(cardType);
                    if (cardInfo != null && cardInfo.IsSuccess)
                    {
                        int temp = cardInfo.Data == null ? 0 : (int)cardInfo.Data;
                        if (temp <= 0 || temp < data.Qty)
                        {
                            meessageStatus = "07";
                            return false;
                        }
                    }
                }
                #endregion

                if (data == null || data.Integral < 0)
                {
                    meessageStatus = "97";
                    return false;
                }

                products.Add(new OrderProduct() { ProductID = data.ProductID, BlueBean = data.BlueBean, Integral = data.Integral, Qty = data.Qty, ProductType = data.ProductType, ProductColor = data.ProductColor, Name = data.Name });
            }
            if (Integral > _AppContext.UserIntegralApp.GetTotalIntegral(userId))
            {
               meessageStatus = "904";
               return false;
           }

            Address addressData = _AppContext.AddressApp.GetOne(addressId);

            if (addressData != null)
            {
                address = new OrderAddress() { City = addressData.City, County = addressData.County, Detail = addressData.Detail, PCC = addressData.PCC, Phone = addressData.Phone, Province = addressData.Province, ReceiveName = addressData.ReceiveName, ZipCode = addressData.ZipCode };
            }


            int totalIntegral = 0;
            int totalBlueBean = 0;

            if (products == null || products.Count == 0)
            {
                meessageStatus = "97";
                return false;
            }

            bool result = this.ValidateProduct(this.userId, products, out totalIntegral, out totalBlueBean, out meessageStatus);

            if (!result)
            {
                return false;
            }

            EPayType payType;

            if (!this.SelectPayType(totalBlueBean, totalIntegral, out payType))
            {
                return false;
            }

            this.CreateRecordData(userId, products, address, totalBlueBean, totalIntegral, payType, out orderData);
            return true;
        }

        #endregion

        #region ==== 私有方法 ====

        /// <summary>
        /// 创建交易记录数据
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="products"></param>
        /// <param name="address"></param>
        /// <param name="totalIntegral"></param>
        /// <param name="orderData"></param>
        /// <param name="flowData"></param>
        private void CreateRecordData(string userId, List<OrderProduct> products, OrderAddress address, int blueBean, int integral, EPayType payType, out Order orderData)
        {
            orderData = new Order()
            {
                OrderId = this.CreateNumber("O"),
                PayState = EPayState.ZFZ.ToInt32(),
                OrderProduct = products,
                DataSource = "blms_pc_web",
                TradeState = ETradeState.JYZ.ToInt32(),
                Mode = EOrderMode.YJSW.ToInt32(),
                Type = payType.ToInt32(),
                OrderAddress = address,
                UserID = userId,
                Createtime = DateTime.Now,
                Updatetime = DateTime.Now,
                Integral = integral,
                BlueBean = blueBean
            };
        }

        /// <summary>
        /// 选择支付方式
        /// </summary>
        /// <param name="blueBean"></param>
        /// <param name="integral"></param>
        /// <param name="payType"></param>
        /// <returns></returns>
        private bool SelectPayType(int blueBean, int integral, out EPayType payType)
        {
            payType = EPayType.Money;
            if (blueBean == 0 && integral == 0)
            {
                payType = EPayType.Integral;
                return true;
            }
            else

                if (blueBean > 0 && integral > 0)
                {
                    payType = EPayType.IntegralAndBlueBean;
                    return true;
                }
                else

                    if (blueBean > 0 && integral <= 0)
                    {
                        payType = EPayType.BlueBean;
                        return true;
                    }
                    else

                        if (blueBean <= 0 && integral > 0)
                        {
                            payType = EPayType.Integral;
                            return true;
                        }
                        else
                        {
                            return false;
                        }

            return false;
        }


        /// <summary>
        /// 验证商品
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="products"></param>
        /// <param name="totalIntegral"></param>
        /// <param name="totalBlueBean"></param>
        /// <param name="messageStatus"></param>
        /// <returns></returns>
        private bool ValidateProduct(string userId, List<OrderProduct> products, out int totalIntegral, out int totalBlueBean, out string messageStatus)
        {
            messageStatus = "00";
            totalIntegral = 0;
            totalBlueBean = 0;
            FrontUserStore<FrontIdentityUser> userstore = new FrontUserStore<FrontIdentityUser>();
            var user = userstore.FindByIdAsync(userId);
            foreach (var productItem in products)
            {
                Product product = _DbSession.ProductStorager.GetProductById(productItem.ProductID);
                var categoryId = _AppContext.CategoryApp.selectIdByName("生日特权");//获取生日特权的类型ID
                var productTypeId = _AppContext.ProductApp.GetProductCategory(productItem.ProductID).CategoryID;//获取该商品的类型
                if (product == null)
                {
                    messageStatus = "03";
                    return false;
                }

                if (product.State == EProductState.SoldOut.ToInt32())
                {
                    messageStatus = "04";
                    return false;
                }

                if (product.Qty < productItem.Qty)
                {
                    messageStatus = "07";
                    return false;
                }

                if (product.MaxUser != 0)
                {
                    int count = _DbSession.ProductStorager.TraderCount(userId, productItem.ProductID);

                    if (count > product.MaxUser)
                    {
                        messageStatus = "06";
                        return false;
                    }
                }

                if (product.MaxPer != 0)
                {
                    if (productItem.Qty > product.MaxPer)
                    {
                        messageStatus = "06";
                        return false;
                    }
                }

                if (productItem.Integral <= 0)
                {
                    //判断该产品是不是生日特权商品
                    if (categoryId != productTypeId)
                    {
                        return false;
                    }
                }
                productItem.Integral = product.Integral;
                if (user.Result.MLevel == 10)
                {
                    productItem.Integral = product.Integral;
                }
                else
                    if (user.Result.MLevel == 11)
                    {
                        productItem.Integral = (int)product.FrontSilverMemberIntegral;
                    }
                    else
                        if (user.Result.MLevel == 12)
                        {
                            productItem.Integral = (int)product.FrontGoldMemberIntegral;
                        }
                //如果该商品的类信息等于了生日特权类型就把订单支付积分赋值为0
                if (categoryId == productTypeId)
                {
                    productItem.Integral = 0;
                    productItem.BlueBean = 0;
                }
                totalIntegral += productItem.Integral == 0 ? 0 : productItem.Integral * productItem.Qty;
                totalBlueBean += productItem.BlueBean == 0 ? 0 : product.BlueBean * productItem.Qty;
            }

            return true;
        }

        /// <summary>
        /// 创建编号
        /// </summary>
        /// <param name="type">编号类型</param>
        /// <returns></returns>
        private string CreateNumber(string type)
        {
            int number = Guid.NewGuid().ToString("N").GetHashCode();
            number = Math.Abs(number);
            return type + number;
        }

        #endregion
    }
}
