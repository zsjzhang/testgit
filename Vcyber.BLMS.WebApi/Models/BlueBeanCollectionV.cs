using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vcyber.BLMS.WebApi.Models
{
    /// <summary>
    /// 蓝豆信息集合
    /// </summary>
    public class BlueBeanCollectionV
    {
        #region ==== 构造函数 ====

        public BlueBeanCollectionV() { }

        #endregion

        #region ==== 公共属性 ====

        /// <summary>
        /// 结果记录总数
        /// </summary>
        public int totalrecord { get; set; }

        /// <summary>
        /// 当前返回记录数
        /// </summary>
        public int record { get; set; }

        /// <summary>
        /// 当前页
        /// </summary>
        public int page { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public int totalpage { get; set; }

        /// <summary>
        /// 会员蓝豆集合
        /// </summary>
        public List<BlueBeanV> datas { get; set; }

        #endregion
    }
}