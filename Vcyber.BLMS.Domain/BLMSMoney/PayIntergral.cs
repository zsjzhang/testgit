using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Vcyber.BLMS.Application;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.IRepository;

namespace Vcyber.BLMS.Domain
{
    /// <summary>
    /// 积分支付
    /// </summary>
    internal class PayIntergral : PayBase
    {
        #region ==== 私有方法 ====

        #endregion

        #region ==== 公共方法 ====

        /// <summary>
        /// 积分扣款
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="integralValue">积分值</param>
        /// <param name="orderData"></param>
        /// <param name="flowData"></param>
        /// <returns></returns>
        public override bool SubMoney(string userId, int integralValue, Tradeflow flowData)
        {
            var integralDatas = _AppContext.UserIntegralApp.GetList(userId);

            if (integralDatas != null && integralDatas.Count() > 0)
            {
                int overIntegral = integralValue;//剩余积分

                //using (TransactionScope scope = new TransactionScope())
                //{
                foreach (var integralItem in integralDatas)
                {
                    int realerIntegral = integralItem.value - integralItem.usevalue;
                    int subIntegral = overIntegral > realerIntegral ? realerIntegral : overIntegral;

                    if (overIntegral > realerIntegral)
                    {
                        bool result = _DbSession.UserIntegralStorager.SubIntegral(integralItem.Id, userId,Math.Abs(subIntegral));


                        if (!result)
                        {
                            return false;
                        }

                        overIntegral = overIntegral - subIntegral;
                    }
                    else
                    {
                        bool result = _DbSession.UserIntegralStorager.SubIntegral(integralItem.Id, userId, Math.Abs(subIntegral));

                        if (result)
                        {
                            if (flowData!=null)
                            {
                                _DbSession.TradeFlowStorager.Add(flowData, Entity.Enum.ETradeType.OrderPay);
                            }
                           
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                // }
            }

            return false;
        }

        #endregion
    }
}
