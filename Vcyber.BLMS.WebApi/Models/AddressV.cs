using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vcyber.BLMS.WebApi.Models
{
    /// <summary>
    /// 会员地址
    /// </summary>
    public class AddressV
    {
        #region ==== 构造函数 ====

        public AddressV() { }

        #endregion

        #region ==== 公共属性 ====

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
        /// 省id
        /// </summary>
        public int ProvinceID { get; set; }

        /// <summary>
        /// 市id
        /// </summary>
        public int CityID { get; set; }

        /// <summary>
        /// 县id
        /// </summary>
        public int CountyID { get; set; }

        /// <summary>
        /// 省市县中文（数据已，分割）
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
        /// 是否是默认（1：默认）
        /// </summary>
        public bool IsDefault { get; set; }

        #endregion

        #region ==== 公共方法 ===

        public bool ValidateData()
        {
            if (string.IsNullOrEmpty(this.UserID)||string.IsNullOrEmpty(this.ReceiveName)||string.IsNullOrEmpty(this.Phone)||
                string.IsNullOrEmpty(this.ZipCode) || string.IsNullOrEmpty(this.Detail)||string.IsNullOrEmpty(this.PCC))
            {
                return false;
            }

            return true;
        }

        #endregion
    }
}