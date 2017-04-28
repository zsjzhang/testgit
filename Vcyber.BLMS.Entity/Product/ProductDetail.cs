using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    /// <summary>
    /// 商品详细信息
    /// </summary>
    public class ProductDetail
    {
        #region ==== 构造函数 ====

        public ProductDetail() { }

        #endregion

        #region ==== 公共属性 ====

        public int ID { get; set; }

        /// <summary>
        /// 商品ID
        /// </summary>
        public int ProductID { get; set; }

        /// <summary>
        /// 详情描述
        /// </summary>
        public string Description { get; set; }

        #endregion
    }
}
