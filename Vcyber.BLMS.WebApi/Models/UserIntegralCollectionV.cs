using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vcyber.BLMS.WebApi.Models
{
    /// <summary>
    /// 用户积分集合
    /// </summary>
    public class UserIntegralCollectionV
    {
        #region ==== 构造函数 ====

        public UserIntegralCollectionV() { }

        #endregion

        #region ==== 公共属性 ====

        /// <summary>
        /// 总数据个数
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 积分集合
        /// </summary>
        public List<UserIntegralV> Datas { get; set; }

        #endregion
    }
}