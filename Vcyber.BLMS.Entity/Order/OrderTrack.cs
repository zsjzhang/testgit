using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    /// <summary>
    /// 订单轨迹
    /// </summary>
    public class OrderTrack
    {
        #region ==== 构造函数 ====

        public OrderTrack() { }

        #endregion

        #region ==== 公共属性 ====

        public int ID { get; set; }
        /// <summary>
        /// TypeID
        /// </summary>	
        public int TypeID { get; set; }
        /// <summary>
        /// OrderID
        /// </summary>	
        public string TypeName { get; set; }
        /// <summary>
        /// OrderID
        /// </summary>	
        public string OrderID { get; set; }
        /// <summary>
        /// Content
        /// </summary>	
        public string Content { get; set; }
        /// <summary>
        /// OperateUser
        /// </summary>	
        public string OperateUser { get; set; }
        /// <summary>
        /// OperateTime
        /// </summary>	
        public DateTime OperateTime { get; set; }

        #endregion
    }
}
