using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    /// <summary>
    /// 订单物流信息
    /// </summary>
    public class OrderShipping
    {
        /// <summary>
        /// ID
        /// </summary>	
        public int ID { get; set; }
        /// <summary>
        /// 订单编号
        /// </summary>	
        public string OrderID { get; set; }
        /// <summary>
        /// Type
        /// </summary>	
        public int Type { get; set; }
        /// <summary>
        ///物流公司名称
        /// </summary>	
        public string Name { get; set; }

        /// <summary>
        /// 快递单号
        /// </summary>	
        public string Number { get; set; }
        /// <summary>
        /// 发货时间
        /// </summary>	
        public DateTime DeliveryTime { get; set; }
        /// <summary>
        /// 收货时间
        /// </summary>	
        public DateTime ReceiveTime { get; set; }
    }
}
