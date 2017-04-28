using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.IRepository;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.Application.BLMSMoney;

namespace Vcyber.BLMS.Domain.BLMSMoney
{
    /// <summary>
    /// 订单支付信息
    /// </summary>
    public class OrderPayInfo : OrderPayInfoBase
    {
        #region ==== 构造函数 ====

        public OrderPayInfo() { }

        #endregion

        #region ==== 公共属性 ====

       

        #endregion

        #region ==== 公共方法 ====

        /// <summary>
        /// 验证订单支付数据是否合法
        /// </summary>
        /// <param name="flowData"></param>
        /// <returns></returns>
        public override bool ValidatePayData(out Tradeflow flowData,out EPayType payType)
        {
            flowData = null;
            payType = EPayType.Money;

            if (string.IsNullOrEmpty(this.userId) || string.IsNullOrEmpty(this.orderId))
            {
                return false;
            }

            var orderData = _DbSession.OrderStorager.GetOrder(orderId);

            if (orderData == null || orderData.PayState != EPayState.ZFZ.ToInt32() || orderData.TradeState != ETradeState.JYZ.ToInt32())
            {
                return false;
            }

            payType = (EPayType)orderData.Type;
            this.CreateFlow(orderData.BlueBean, orderData.Integral, out flowData);
            flowData.remark = ((EOrderMode)orderData.Mode).GetDiscribe();

            if (!this.ValidateMoney(payType, orderData.BlueBean, orderData.Integral))
            {
                return false;
            }

            return true;
        }

        #endregion

        #region ==== 私有方法 ====

        /// <summary>
        /// 创建交易流水
        /// </summary>
        /// <param name="blueBaean">蓝豆值</param>
        /// <param name="integral">积分值</param>
        /// <param name="flowData">交易流水</param>
        private void CreateFlow(int blueBaean, int integral, out Tradeflow flowData)
        {
            flowData = new Tradeflow()
            {
                UserId = this.userId,
                orderId = this.orderId,
                TradeType = ETradeType.OrderPay.ToInt32(),

                FlowCode = this.CreateNumber("F"),
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now,
                remark = "商品兑换",
                BlueBean = blueBaean,
                tradeintegral = integral
            };
        }

        /// <summary>
        /// 验证用户积分和蓝豆是否足够支付
        /// </summary>
        /// <param name="payType"></param>
        /// <param name="blueBean"></param>
        /// <param name="integral"></param>
        /// <returns></returns>
        private bool ValidateMoney(EPayType payType, int blueBean, int integral)
        {
            switch (payType)
            {
                case EPayType.Money: return false;
                    break;
                case EPayType.Integral:
                    {
                        int value = _DbSession.UserIntegralStorager.GetTotalIntegral(this.userId);

                        if (value < integral)
                        {
                            return false;
                        }
                    }
                    break;
                case EPayType.Prize: return false;
                    break;
                //case EPayType.BlueBean:
                //    {
                //        int value = _DbSession.UserBlueBeanStorager.GetTotalBlueBean(this.userId);

                //        if (value < blueBean)
                //        {
                //            return false;
                //        }
                //    }
                //    break;
                case EPayType.IntegralAndBlueBean:
                    {
                        int value1 = _DbSession.UserIntegralStorager.GetTotalIntegral(this.userId);
                       // int value2 = _DbSession.UserBlueBeanStorager.GetTotalBlueBean(this.userId);

                        if (value1 < integral)
                        {
                            return false;
                        }
                    }
                    break;
                default: return false;
                    break;
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
