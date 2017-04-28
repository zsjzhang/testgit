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
    /// 支付蓝豆
    /// </summary>
    internal class PayBlueBean : PayBase
    {
        #region ==== 私有字段 ====

        #endregion

        #region ==== 公共方法 ====

        public override bool SubMoney(string userId, int blueBeanValue, Tradeflow flowData)
        {
            var blueBeanDatas = _AppContext.UserBlueBeanApp.GetList(userId);

            if (blueBeanDatas != null && blueBeanDatas.Count() > 0)
            {
                int overblueBean = blueBeanValue;//剩余蓝豆

                //using (TransactionScope scope = new TransactionScope())
                //{
                foreach (var blueBeanItem in blueBeanDatas)
                {
                    int realerblueBean = blueBeanItem.value - blueBeanItem.usevalue;
                    int subblueBean = overblueBean > realerblueBean ? realerblueBean : overblueBean;

                    if (overblueBean > realerblueBean)
                    {
                        bool result = _DbSession.UserBlueBeanStorager.SubBlueBean(blueBeanItem.Id, userId, subblueBean);

                        if (!result)
                        {
                            return false;
                        }

                        overblueBean=overblueBean-subblueBean;
                    }
                    else
                    {
                        bool result = _DbSession.UserBlueBeanStorager.SubBlueBean(blueBeanItem.Id, userId, Math.Abs(subblueBean));

                        if (result)
                        {
                            if (flowData != null)
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
                    // }
                }
            }

            return false;
        }

        #endregion
    }
}
