using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vcyber.BLMS.WebApi.Models
{
    /// <summary>
    /// 市信息
    /// </summary>
    public class CityV
    {
        #region ==== 构造函数 ====

        public CityV() { }

        #endregion

        #region ==== 公共属性 ====

        /// <summary>
        /// 市Id
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 市Code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 市名称
        /// </summary>
        public string Name { get; set; }
        
        #endregion

      
    }
}