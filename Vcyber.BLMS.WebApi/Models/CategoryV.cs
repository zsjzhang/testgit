using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vcyber.BLMS.WebApi.Models
{
    /// <summary>
    /// 商品类型
    /// </summary>
    public class CategoryV
    {
        /// <summary>
        /// 商品类型Id
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 类型名称
        /// </summary>
        public string Name { get; set; }
    }

    public class CategoryCollection
    {
        /// <summary>
        /// 应答状态 99：成功；00：失败
        /// </summary>
        public string status { get; set; }

        /// <summary>
        /// 商品类型集合
        /// </summary>
        public List<CategoryV> datas { get; set; }
    }
}