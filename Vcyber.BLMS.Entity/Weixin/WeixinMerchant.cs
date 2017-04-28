using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    /// <summary>
    /// 微信支付商户表
    /// </summary>
    public class WeixinMerchant
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 收款人姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 收款人手机号
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 收款人OpenId
        /// </summary>
        public string OpenId { get; set; }
        /// <summary>
        /// 所属经销商
        /// </summary>
        public string DealerId { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
