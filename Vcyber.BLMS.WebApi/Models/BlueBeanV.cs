using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vcyber.BLMS.WebApi.Models
{
    /// <summary>
    /// 蓝豆信息
    /// </summary>
    public class BlueBeanV
    {
        #region ==== 构造函数 ====

        public BlueBeanV() { }

        #endregion

        #region ==== 公共属性 ====

        /// <summary>
        /// 蓝豆值
        /// </summary>
        public int value { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CreateTime { get; set; }

        #endregion
    }
}