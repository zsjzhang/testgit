using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    /// <summary>
    /// 商品与商品类型关系
    /// </summary>
    public class ProductCategory
    {
        #region ==== 构造函数 ====

        public ProductCategory() { }

        #endregion

        #region ==== 公共属性 ====

        public int ID { get; set; }
        /// <summary>
        /// 商品ID
        /// </summary>	
        public int ProductID { get; set; }
        /// <summary>
        /// 分类ID
        /// </summary>	
        public int CategoryID { get; set; }
        /// <summary>
        /// 分类名称
        /// </summary>	
        public string CategoryName { get; set; }
        /// <summary>
        /// 记录状态
        /// </summary>	
        public short Datastate { get; set; }

        #endregion
    }
}
