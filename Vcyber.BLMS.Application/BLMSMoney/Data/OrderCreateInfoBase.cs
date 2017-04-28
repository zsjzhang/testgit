using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.Application.BLMSMoney
{

    /// <summary>
    /// 订单创建信息抽象类型
    /// </summary>
    public abstract class OrderCreateInfoBase
    {
        #region ====公共属性 ====

        /// <summary>
        /// 用户Id
        /// </summary>
        public string userId { get; set; }

        /// <summary>
        /// 购物车Id
        /// </summary>
        public List<int> shoppingIds { get; set; }

        /// <summary>
        /// 收货地址Id
        /// </summary>
        public int addressId { get; set; }

        /// <summary>
        /// 数据渠道(blms:前台网站；blms_web：手机app;blms_wechat：微信)
        /// </summary>
        public string DataSource { get; set; }

        #endregion
        
         /// <summary>
        /// 验证订单创建数据
        /// </summary>
        /// <param name="orderData">订单数据</param>
        /// <param name="meessageStatus">验证状态（）</param>
        /// <returns></returns>
        public abstract bool ValidateCreateData(out Order orderData, out string meessageStatus);

    }
}
