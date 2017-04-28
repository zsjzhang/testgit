using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    /// <summary>
    /// 订单地址
    /// </summary>
    public class OrderAddress
    {
        #region ==== 构造函数 ====

        public OrderAddress()
        { }

        #endregion

        #region ==== 公共属性 ====

        public int ID { get; set; }
        /// <summary>
        /// 订单ID
        /// </summary>

        public string OrderID { get; set; }
        /// <summary>
        /// 收货人姓名
        /// </summary>
        public string ReceiveName { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 省
        /// </summary>
        public int Province { get; set; }
        /// <summary>
        /// 市
        /// </summary>
        public int City { get; set; }
        /// <summary>
        /// 县
        /// </summary>
        public int County { get; set; }
        /// <summary>
        /// 省市县中文
        /// </summary>
        public string PCC { get; set; }
        /// <summary>
        /// 邮编
        /// </summary>
        public string ZipCode { get; set; }
        /// <summary>
        /// 详细地址
        /// </summary>
        public string Detail { get; set; }

        /// <summary>
        /// 数据渠道(blms:前台网站；blms_web：手机app;blms_wechat：微信)
        /// </summary>
        public string DataSource { get; set; }

        #endregion
    }
}
