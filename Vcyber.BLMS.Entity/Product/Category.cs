using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    /// <summary>
    /// 商品类型
    /// </summary>
    public class Category
    {
        #region ==== 构造函数 ====

        public Category() { }

        #endregion

        #region ==== 公共属性 ====

        /// <summary>
        /// 
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int ParentID { get; set; }

        /// <summary>
        /// 类型名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime Createtime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime Updatetime { get; set; }

        public int Datastate { get; set; }


        public string CardType { set; get; }

        #endregion

        #region ==== 附件属性 ====

        /// <summary>
        /// 子节点
        /// </summary>
        public List<Category> Childs { get; set; }

        /// <summary>
        /// 是否存在子节点
        /// </summary>
        public bool IsChild { get; set; }

        #endregion
    }
}
