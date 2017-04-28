using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Common.Payment
{
    /// <summary>
    /// 支付类型
    /// </summary>
    public enum PaymentType
    {
        alipay = 1,
        tenpay = 2,
        integral = 3,
        tmall = 4,
        weixin = 5
    }
    /// <summary>
    /// 支付工厂
    /// </summary>
    public class PaymentFactory
    {
        public static IPayment GetInstance(PaymentType type)
        {
            IPayment obj = null;
            switch (type)
            {
                case PaymentType.alipay:
                    obj = null;
                    break;
                case PaymentType.tenpay:
                    obj = new PaymentForTen();
                    break;
                case PaymentType.tmall:
                    obj = new PaymentForTmall();
                    break;
                case PaymentType.weixin:
                    obj = new PaymentForWeixin();
                    break;
            }
            return obj;
        }
    }
}
