using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Application.BLMSMoney;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;

namespace Vcyber.BLMS.Application
{
    /// <summary>
    /// 交易港口
    /// </summary>
    public interface ITradePort
    {
        /// <summary>
        /// 交易机场服务
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="integralValue">积分值</param>
        /// <returns>true:兑换成功</returns>
        bool TradeService(string userId, int integralValue);

        /// <summary>
        /// 交易非商品
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="integralValue">消耗积分</param>
        /// <param name="orderMode">订单形式</param>
        /// <returns></returns>
        bool TradeService(string userId, int integralValue, EOrderMode orderMode);

        /// <summary>
        /// 创建订单
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="products">订单商品信息</param>
        /// <param name="address">订单地址</param>
        /// <param name="payType">支付方式</param>
        /// <param name="messageStatus">消息状态(99:交易成功；03：商品不存在；04：商品已经下架；07：库存不足；06：超出兑换次数；00：交易失败)</param>
        /// <returns>成功返回：订单编号</returns>
        string CreateOrder(string userId, List<OrderProduct> products, OrderAddress address, EPayType payType, out string messageStatus);

        /// <summary>
        /// 订单支付
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="orderId">订单编号</param>
        /// <param name="payType">支付类型</param>
        /// <returns>true:支付成功</returns>
        bool OrderPay(string userId, string orderId, EPayType payType);

        /// <summary>
        /// 创建订单信息
        /// </summary>
        /// <param name="createInfo">创建数据</param>
        /// <param name="messageStatus">消息状态(99:交易成功；03：商品不存在；04：商品已经下架；07：库存不足；06：超出兑换次数；00：交易失败)</param>
        /// <returns></returns>
        Order CreateOrder(OrderCreateInfoBase createInfo, out string messageStatus);

        /// <summary>
        /// 订单支付
        /// </summary>
        /// <param name="payInfo">订单支付信息</param>
        /// <returns></returns>
        bool OrderPay(OrderPayInfoBase payInfo);

        /// <summary>
        /// 消费通知
        /// </summary>
        /// <returns></returns>
        ReturnResult ConsumerNotice(string userId, string remark);
    }
}
