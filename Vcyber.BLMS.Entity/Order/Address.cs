using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    /// <summary>
    /// 用户常用地址
    /// </summary>
    public class Address
    {
        #region 公共属性

        public int ID { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserID { get; set; }

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
        /// 是否是默认
        /// </summary>
        public bool IsDefault { get; set; }

        /// <summary>
        /// 记录状态
        /// </summary>
        public short Datastate { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime UpdateTime { get; set; }

        #endregion

    }
}
