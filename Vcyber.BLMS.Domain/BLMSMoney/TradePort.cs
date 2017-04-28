using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

using Vcyber.BLMS.Application;
using Vcyber.BLMS.IRepository;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.Domain.BLMSMoney;
using Vcyber.BLMS.Application.BLMSMoney;
using System.Security.Cryptography;

using Newtonsoft.Json;

namespace Vcyber.BLMS.Domain
{
    /// <summary>
    /// 交易港口
    /// </summary>
    public class TradePort : ITradePort
    {
        #region ==== 私有字段 ====

        #endregion

        #region ==== 构造函数 ====

        public TradePort() { }

        #endregion
#warning ==== 写的有点烂，有待后来人重写 ====

        #region ==== 公共方法 ====

        /// <summary>
        /// 交易机场服务
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="integralValue">积分值</param>
        /// <returns>true:兑换成功</returns>
        public bool TradeService(string userId, int integralValue)
        {
            return this.TradeService(userId, integralValue, EOrderMode.JCService);
        }

        /// <summary>
        /// 交易非商品
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="integralValue">消耗积分</param>
        /// <param name="orderMode">订单形式</param>
        /// <returns></returns>
        public bool TradeService(string userId, int integralValue, EOrderMode orderMode)
        {
            int totalIntegral = _AppContext.UserIntegralApp.GetTotalIntegral(userId);

            if (totalIntegral < integralValue || integralValue <= 0)
            {
                return false;
            }

            Order orderData = new Order()
            {
                OrderId = this.CreateNumber("O"),
                PayState = EPayState.ZFZ.ToInt32(),
                OrderProduct = null,

                TradeState = ETradeState.JYZ.ToInt32(),
                Mode = orderMode.ToInt32(),
                Type = EPayType.Integral.ToInt32(),
                OrderAddress = null,
                UserID = userId,
                Createtime = DateTime.Now,
                Updatetime = DateTime.Now,
                Integral = integralValue
            };

            PayBase.AddTradeRecord(orderData);
            bool result = this.OrderPay(userId, orderData.OrderId, EPayType.Integral);

            if (result)
            {
                this.UpdateOrderStatus(orderData.OrderId, EPayState.ZFWC, ETradeState.JYWC);

                //消费通知
                ConsumerNotice(orderData.UserID, "TradeService");

                return true;
            }

            return false;
        }

        /// <summary>
        /// 创建订单信息
        /// </summary>
        /// <param name="createInfo">创建数据</param>
        /// <param name="messageStatus">消息状态(99:交易成功；03：商品不存在；04：商品已经下架；07：库存不足；06：超出兑换次数；00：交易失败;97：数据异常;)</param>
        /// <returns></returns>
        public Order CreateOrder(OrderCreateInfoBase createInfo, out string messageStatus)
        {
            messageStatus = "00";
            Order orderData;
            if (createInfo == null)
            {
                return null;
            }
            if (!createInfo.ValidateCreateData(out orderData, out messageStatus))
            {
                return null ;
            }
            bool result = PayBase.AddTradeRecord(orderData);
            messageStatus = result ? "99" : "07";
            return orderData;
        }

        /// <summary>
        /// 订单支付
        /// </summary>
        /// <param name="payInfo">订单支付信息</param>
        /// <returns></returns>
        public bool OrderPay(OrderPayInfoBase payInfo)
        {
            Tradeflow flowData;
            EPayType payType;
           
            if (payInfo == null || !payInfo.ValidatePayData(out flowData, out payType))
            {
                return false;
            }

            bool result = false;

            if (payType == EPayType.IntegralAndBlueBean)
            {
                result = this.PayFactory(EPayType.Integral).SubMoney(payInfo.userId, flowData.tradeintegral, null);

                if (result)
                {
                    result = this.PayFactory(EPayType.BlueBean).SubMoney(payInfo.userId, flowData.BlueBean, flowData);
                }
            }
            else
            {
                int value = payType == EPayType.Integral ? flowData.tradeintegral : flowData.BlueBean;
                result = this.PayFactory(payType).SubMoney(payInfo.userId, value, flowData);
            }

             var orderProducts = _DbSession.OrderStorager.GetOrderProduct(payInfo.orderId);//获取订单的商品
            var categoryId = _AppContext.CategoryApp.selectIdByName("生日特权");//获取生日特权的类型ID
            //只要订单商品中包含了有生日特权的，直接设置为支付成功。前台生日特权商品不可能和其他商品一同购买
            var productTypeId=0;
            foreach (var item in orderProducts)
	        {
		        productTypeId = _AppContext.ProductApp.GetProductCategory(item.ProductID).CategoryID;//获取该商品的类型
                if (productTypeId == categoryId)
                {
                    result = true;//直接支付成功
                    break;
                }
	        }

            if (result)
            {
                this.UpdateOrderStatus(payInfo.orderId, EPayState.ZFWC, ETradeState.DFH);
                this.SubLQty(payInfo.orderId);
                this.SmsSend(payInfo.orderId);

                //消费通知
                ConsumerNotice(payInfo.userId, "OrderPay");
            }

            return result;
        }

        #region ==== 旧逻辑 ====

        /// <summary>
        /// 创建订单
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="products">订单商品信息</param>
        /// <param name="address">订单地址</param>
        /// <param name="payType">支付方式</param>
        /// <param name="messageStatus">消息状态(99:交易成功；03：商品不存在；04：商品已经下架；07：库存不足；06：超出兑换次数；00：交易失败;97：数据异常)</param>
        /// <returns>成功返回：订单编号</returns>
        public string CreateOrder(string userId, List<OrderProduct> products, OrderAddress address, EPayType payType, out string messageStatus)
        {
            messageStatus = "00";
            int totalIntegral = 0;
            int totalBlueBean = 0;

            if (products == null || products.Count == 0)
            {
                return null;
            }

            bool result = this.ValidateProduct(userId, products, payType, out totalIntegral, out totalBlueBean, out messageStatus);

            if (!result)
            {
                return null;
            }

            Order orderData;
            int moneyValue = payType == EPayType.Integral ? totalIntegral : totalBlueBean;

            //todo:tsnuen 为生日特权而修改如果没有积分也可以兑换--begin            
            var categoryId = _AppContext.CategoryApp.selectIdByName("生日特权");//获取生日特权的类型ID
            //只要订单商品中包含了有生日特权的，直接设置为支付成功。前台生日特权商品不可能和其他商品一同购买
            foreach (var item in products)
            {
                var productTypeId = _AppContext.ProductApp.GetProductCategory(item.ProductID).CategoryID;//获取该商品的类型
                if (productTypeId == categoryId)
                {
                    moneyValue = 0;                    
                    break;
                }
            }
            //--end

            this.CreateRecordData(userId, products, address, moneyValue, payType, out orderData);
            orderData.DataSource = address.DataSource;
            result = PayBase.AddTradeRecord(orderData);

            messageStatus = result ? "99" : "07";
            return result ? orderData.OrderId : null;
        }

        /// <summary>
        /// 订单支付
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="orderId">订单编号</param>
        /// <param name="payType">支付类型</param>
        /// <returns>true:支付成功</returns>
        public bool OrderPay(string userId, string orderId, EPayType payType)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(orderId))
            {
                return false;
            }

            var orderData = _DbSession.OrderStorager.GetOrder(orderId);

            if (orderData == null || orderData.Type != payType.ToInt32() || orderData.PayState != EPayState.ZFZ.ToInt32() || orderData.TradeState != ETradeState.JYZ.ToInt32())
            {
                return false;
            }

            Tradeflow flowData;
            int moneyValue = payType == EPayType.Integral ? orderData.Integral : orderData.BlueBean;
            this.CreateRecordData(userId, moneyValue, orderId, payType, out flowData);
            flowData.remark = ((EOrderMode)orderData.Mode).GetDiscribe();

            if (!this.ValidateMoney(userId, payType, moneyValue))
            {
                return false;
            }

            bool result = false;

            //todo:tsnuen 为生日特权而修改如果没有积分也可以兑换--begin
            var orderProducts = _DbSession.OrderStorager.GetOrderProduct(orderId);//获取订单的商品
            var categoryId = _AppContext.CategoryApp.selectIdByName("生日特权");//获取生日特权的类型ID
            //只要订单商品中包含了有生日特权的，直接设置为支付成功。前台生日特权商品不可能和其他商品一同购买
            var productTypeId = 0;
            foreach (var item in orderProducts)
            {
                productTypeId = _AppContext.ProductApp.GetProductCategory(item.ProductID).CategoryID;//获取该商品的类型
                if (productTypeId == categoryId)
                {
                    result = true;//直接支付成功
                    break;
                }
            }
            //--end
            if (!result)
            {
                result = this.PayFactory(payType).SubMoney(userId, moneyValue, flowData);
            }            
            if (result)
            {
                this.UpdateOrderStatus(orderId, EPayState.ZFWC, ETradeState.DFH);
                this.SubLQty(orderId);
                this.SmsSend(orderId);
                //消费通知
                ConsumerNotice(userId, "OrderPay");
            }

            return result;
        }


        #endregion

        #endregion


        public ReturnResult ConsumerNotice(string userId, string remark)
        {
            ReturnResult result = new ReturnResult { IsSuccess = true };
            return result;//取消该功能。因为event.bluemembers.com.cn报错
            //密钥
            string scCode = "fdsrwqred21434fre43rgkljkln_aj" + DateTime.Now.ToString("yyyy-MM-dd");

            //参数
            string key = "uid=" + userId + "&act=" + remark + "&dotime=" + DateTime.Now.ToString("yyyyMMddHHmmss") + "&code=" + scCode;
            
            //地址
            string url = "http://event.bluemembers.com.cn/memberstrip/api/notice.html";

            string sign = "";

            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                sign = BitConverter.ToString(md5.ComputeHash(UTF8Encoding.Default.GetBytes(key))).Replace("-", "");
            }

            var postData = new EV_EventPar { 
                uid = userId,
                act = remark,
                dotime = DateTime.Now.ToString("yyyyMMddHHmmss"),
                key = sign
            };

            string json =  JsonConvert.SerializeObject(postData);

            //调用服务
            var data = Vcyber.BLMS.Common.Web.WebUtils.JsonToObj<EV_EventReturnData>(Vcyber.BLMS.Common.Web.WebUtils.POST_HttpWebRequestHTML("utf-8", url, json), null);
            
            return result;
        }



        #region ==== 私有方法 ====

        /// <summary>
        /// 支付工厂
        /// </summary>
        /// <param name="payType">支付类型</param>
        /// <returns></returns>
        private PayBase PayFactory(EPayType payType)
        {
            PayBase instance = null;

            switch (payType)
            {
                case EPayType.Money:
                    break;
                case EPayType.Integral: instance = new PayIntergral();
                    break;
                case EPayType.Prize:
                    break;
                case EPayType.BlueBean: instance = new PayBlueBean();
                    break;
                default:
                    break;
            }

            return instance;
        }

        /// <summary>
        /// 验证用户支付金额是否足够
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="payType"></param>
        /// <param name="payMoney"></param>
        /// <returns></returns>
        private bool ValidateMoney(string userId, EPayType payType, int payMoney)
        {
            if (payType == EPayType.Integral)
            {
                int value = _DbSession.UserIntegralStorager.GetTotalIntegral(userId);

                if (value < payMoney)
                {
                    return false;
                }
            }
            else
            {
                if (payType == EPayType.BlueBean)
                {
                  
                        return false;
             
                }
            }

            return true;
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
        private bool ValidateProduct(string userId, List<OrderProduct> products, EPayType payType, out int totalIntegral, out int totalBlueBean, out string messageStatus)
        {
            messageStatus = "";
            totalIntegral = 0;
            totalBlueBean = 0;
            int productLengt = products.Count();
            var store = new AspNet.Identity.SQL.FrontUserStore<AspNet.Identity.SQL.FrontIdentityUser>();
            var user = store.FindByIdAsync(userId).Result ;
            
            for (int i = 0; i < productLengt; i++)
            {
                OrderProduct productItem = products[i];
                Product product = _DbSession.ProductStorager.GetProductById(productItem.ProductID);

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

                if (payType == EPayType.BlueBean)
                {
                    productItem.BlueBean = product.BlueBean;
                }

                if (payType == EPayType.Integral)
                {
                    productItem.Integral = product.Integral;

                    if (user != null)
                    {
                        if (user.MLevel == (int)MemshipLevel.GoldCard)
                        {
                            productItem.Integral =  (int)product.FrontGoldMemberIntegral;

                        }
                        if (user.MLevel == (int)MemshipLevel.SilverCard)
                        {
                            productItem.Integral = (int)product.FrontSilverMemberIntegral;
                        }

                    }
                }
                totalIntegral += productItem.Integral * productItem.Qty;
                totalBlueBean += product.BlueBean * productItem.Qty;
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

        /// <summary>
        /// 创建交易记录数据
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="products"></param>
        /// <param name="address"></param>
        /// <param name="totalIntegral"></param>
        /// <param name="orderData"></param>
        /// <param name="flowData"></param>
        private void CreateRecordData(string userId, List<OrderProduct> products, OrderAddress address, int moneyValue, EPayType payType, out Order orderData)
        {
            orderData = new Order()
            {
                OrderId = this.CreateNumber("O"),
                PayState = EPayState.ZFZ.ToInt32(),
                OrderProduct = products,

                TradeState = ETradeState.JYZ.ToInt32(),
                Mode = EOrderMode.YJSW.ToInt32(),
                Type = payType.ToInt32(),
                OrderAddress = address,
                UserID = userId,
                Createtime = DateTime.Now,
                Updatetime = DateTime.Now
            };

            if (payType == EPayType.Integral)
            {
                orderData.Integral = moneyValue;
            }

            if (payType == EPayType.BlueBean)
            {
                orderData.BlueBean = moneyValue;
            }
        }

        /// <summary>
        /// 创建交易记录数据
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="products"></param>
        /// <param name="address"></param>
        /// <param name="totalIntegral"></param>
        /// <param name="orderData"></param>
        /// <param name="flowData"></param>
        private void CreateRecordData(string userId, int moneyValue, string orderId, EPayType payType, out Tradeflow flowData)
        {
            flowData = new Tradeflow()
            {
                UserId = userId,
                orderId = orderId,
                TradeType = ETradeType.OrderPay.ToInt32(),

                FlowCode = this.CreateNumber("F"),
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now,
                remark = "商品兑换"
            };

            if (payType == EPayType.Integral)
            {
                flowData.tradeintegral = moneyValue;
            }

            if (payType == EPayType.BlueBean)
            {
                flowData.BlueBean = moneyValue;
            }
        }

        /// <summary>
        /// 修改订单状态
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="payState"></param>
        /// <param name="tradState"></param>
        /// <returns></returns>
        private bool UpdateOrderStatus(string orderId, EPayState payState, ETradeState tradState)
        {
            bool result = _DbSession.OrderStorager.EditPayState(orderId, payState);

            if (result)
            {
                result = _DbSession.OrderStorager.EditTradeState(orderId, tradState);
            }

            return result;
        }

        /// <summary>
        /// 减去订单商品锁定库存
        /// </summary>
        /// <param name="orderId"></param>
        private void SubLQty(string orderId)
        {
            var orderProducts = _DbSession.OrderStorager.GetOrderProduct(orderId);

            if (orderProducts != null && orderProducts.Count() > 0)
            {
                foreach (var item in orderProducts)
                {
                    _DbSession.ProductStorager.SubLockQty(item.ProductID, item.Qty);
                }
            }
        }

        public void SmsSend(string orderId)
        {
            var orderProducts = _AppContext.OrderApp.GetOrderProduct(orderId);
            var orderAddress = _AppContext.OrderApp.GetOrderAddress(orderId);
            var orderData = _AppContext.OrderApp.GetOrder(orderId);

            if (orderProducts != null && orderAddress != null)
            {
                var productNames = orderProducts.Select<OrderProduct, string>((d) => { return d.Name; });
                string productName = string.Join(",", productNames);
                string integaral = orderData.Integral.ToString();
                string blueBean = orderData.BlueBean.ToString();
                string totalIntegral = _AppContext.UserIntegralApp.GetTotalIntegral(orderData.UserID).ToString();
                string totalBlueBean = _AppContext.UserBlueBeanApp.GetTotalBlueBean(orderData.UserID).ToString();

                _AppContext.SMSApp.SendSMS(ESmsType.支付完成_bluemembers银卡会员, orderAddress.Phone, new string[] { productName, integaral, blueBean, totalIntegral, totalBlueBean });
            }
        }

        #endregion
    }
}
