using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vcyber.BLMS.WebApi.Models
{
    /// <summary>
    /// 省信息
    /// </summary>
    public class ProvincesV
    {
        #region ==== 构造函数 ====

        public ProvincesV() { }

        #endregion

        #region ==== 公共属性 ====

        /// <summary>
        /// 省Id
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 省Code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 省名称
        /// </summary>
        public string Name { get; set; }

        #endregion
    }
}