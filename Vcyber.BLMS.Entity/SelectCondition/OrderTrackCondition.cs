using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity.SelectCondition
{
    /// <summary>
    /// 订单轨迹查询条件
    /// </summary>
    public class OrderTrackCondition:Condition
    {
        #region ==== 构造函数 ====

        public OrderTrackCondition() { }

        #endregion

        #region ==== 公共属性 ====



        #endregion

        #region ==== 公共方法 ====

        public string ToWhere()
        {
            return string.Empty;
        }

        #endregion
    }
}
