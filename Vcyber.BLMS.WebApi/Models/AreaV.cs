using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vcyber.BLMS.WebApi.Models
{
    /// <summary>
    /// 区/县信息
    /// </summary>
    public class AreaV
    {
        #region ==== 构造函数 ====

        public AreaV() { }

        #endregion

        #region ==== 公共属性 ====

        /// <summary>
        ///  区/县Id
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        ///  区/县Code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        ///  区/县名称
        /// </summary>
        public string Name { get; set; }

        #endregion
    }
}