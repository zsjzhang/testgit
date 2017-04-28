using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Vcyber.BLMS.Entity.Enum;

namespace Vcyber.BLMS.Entity.SelectCondition
{
    /// <summary>
    /// 商品搜索条件
    /// </summary>
    public class ProductCondition
    {
        #region ==== 构造函数 ====

        public ProductCondition() { }

        #endregion

        #region ==== 公共属性 ====

        /// <summary>
        /// 商品名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 商品类型Id
        /// </summary>
        public int CategoryID { get; set; }

        /// <summary>
        /// 商品推荐
        /// </summary>
        public EProductRecommend? Recommend { get; set; }

        /// <summary>
        /// 商品状态
        /// </summary>
        public EProductState? State { get; set; }

        /// <summary>
        /// 库存预警项
        /// </summary>
        public int Yjx { get; set; }

        #endregion

        #region ==== 公共方法 ====

        public string ToWhere()
        {
            StringBuilder where = new StringBuilder();
            where.Append(" 1=1 ");

            if (this.CategoryID != 0)
            {
                where.AppendFormat(" and tempTable.CategoryID=@CategoryID");
            }

            if (this.Recommend!=null)
            {
                where.AppendFormat(" and  tempTable.IsRecommend={0}",((EProductRecommend)this.Recommend).ToInt32());
            }

            if (this.State != null)
            {
                where.AppendFormat(" and  tempTable.State={0}", ((EProductState)this.State).ToInt32());
            }

            if (!string.IsNullOrEmpty(this.Name))
            {
                this.Name = string.Format("%{0}%", Name);
                where.AppendFormat(" and tempTable.Name like @Name");
            }

            if (this.Yjx==1)
            {
                where.Append(" and Yjx<30 ");
            }
            if (this.Yjx ==2)
            {
                where.Append(" and Yjx>30 ");
            }
            return where.ToString();
        }

        #endregion
    }
}
