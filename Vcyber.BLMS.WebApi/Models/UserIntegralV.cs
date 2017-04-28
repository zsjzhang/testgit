using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vcyber.BLMS.WebApi.Models
{
    /// <summary>
    /// 用户积分信息
    /// </summary>
    public class UserIntegralV
    {
        #region ==== 构造函数 ====

        public UserIntegralV() { }

        #endregion

        #region ==== 公共属性 ====

        /// <summary>
        /// 用户id
        /// </summary>
        public string userId { get; set; }

        /// <summary>
        /// 积分来源
        /// </summary>
        public string integralSource { get; set; }

        /// <summary>
        /// 积分值
        /// </summary>
        public int value { get; set; }

        /// <summary>
        /// (0:有效；2：失效)
        /// </summary>
        public int datastate { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }

        /// <summary>
        /// 获取时间
        /// </summary>
        public string CreateTime { get; set; }

        #endregion
    }
}