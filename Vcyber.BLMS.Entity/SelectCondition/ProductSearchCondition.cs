using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;

namespace Vcyber.BLMS.Entity.SelectCondition
{
    /// <summary>
    /// 前端商品搜索条件
    /// </summary>
    public class ProductSearchCondition
    {
        #region ==== 构造函数 ====

        public ProductSearchCondition() { }

        #endregion

        #region ==== 公共属性 ====

        /// <summary>
        /// 商品类型ID
        /// </summary>
        public int CategoryID { get; set; }

        /// <summary>
        /// 支付方式
        /// </summary>
        public EPayType? PayType { get; set; }

        /// <summary>
        /// 排序方式
        /// </summary>
        public EPaiXuMode? PXModel { get; set; }

        #endregion

        #region ==== 公共方法 =====

        public string ToJoin()
        {
           
                return " join productcategory on product.ID=productcategory.ProductID join category on category.ID=productcategory.CategoryID";
        }

        public string ToWhereJoin()
        {
            if (this.CategoryID>0)
            {
                return " and productcategory.Datastate=@Datastate and category.Datastate=@Datastate and ( category.ID=" + this.CategoryID + " or category.ParentID=" + this.CategoryID+" ) ";
            }else if(this.CategoryID == 0){
                return " and productcategory.Datastate=@Datastate and category.Datastate=@Datastate";

            }

            return "";
        }

        public string ToWherePayType()
        {
            if (this.PayType==null)
            {
                return "";
            }

            if (this.PayType==EPayType.BlueBean)
            {
                return " and product.BlueBean!=0 ";
            }

            if (this.PayType==EPayType.Integral)
            {
                return " and product.Integral!=0 ";
            }

            return "";
        }

        public string ToWherePaiXu()
        {
            if (this.PXModel==null)
            {
                return "";
            }

            if (this.PXModel==EPaiXuMode.JF)
            {
                return "product.Integral desc,";
            }

            if (this.PXModel==EPaiXuMode.LD)
            {
                return "product.BlueBean desc,";
            }

            if (this.PXModel==EPaiXuMode.XL)
            {
                return "product.Sales desc,";
            }

            return "";
        }

        #endregion
    }


}
