using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    public class IFCarDealerShipInfo
    {

        /// <summary>
        /// 数据标识ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 4S店ID
        /// </summary>
        public string DealerId { get; set; }

        /// <summary>
        /// 地理坐标
        /// </summary>
        public string Position { get; set; }

        /// <summary>
        /// 所属省份
        /// </summary>
        public string Province { get; set; }

        /// <summary>
        /// 所属市区
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 办事处
        /// </summary>
        public string WorkName { get; set; }

        /// <summary>
        /// 事业部
        /// </summary>
        public string BU { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 淘宝账号
        /// </summary>
        public string TBAccount { get; set; }

        /// <summary>
        /// 支付宝账号
        /// </summary>
        public string AlipayAccount { get; set; }

        /// <summary>
        /// 售后电话
        /// </summary>
        public string AfterSalesPhone { get; set; }

        /// <summary>
        /// 电子邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 网站
        /// </summary>
        public string WebSite { get; set; }

        /// <summary>
        /// 简称
        /// </summary>
        public string Abbreviation { get; set; }
    }

}
