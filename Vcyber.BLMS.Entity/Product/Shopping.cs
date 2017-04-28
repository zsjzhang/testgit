using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    /// <summary>
    /// 购物车信息
    /// </summary>
    public class Shopping
    {
        #region ==== 构造函数 ====

        public Shopping() { }

        #endregion

        #region ==== 公共属性 ====

        /// <summary>
        /// 
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// 商品ID
        /// </summary>
        public int ProductID { get; set; }

        /// <summary>
        /// 购买数量
        /// </summary>
        public int Qty { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 0未删除，1：删除
        /// </summary>
        public int DateState { get; set; }

        /// <summary>
        ///商品积分价
        /// </summary>
        public int Integral { get; set; }

        /// <summary>
        ///商品蓝豆价
        /// </summary>
        public int BlueBean { get; set; }

        /// <summary>
        /// 商品颜色
        /// </summary>
        public string ProductColor { get; set; }

        /// <summary>
        /// 商品类型
        /// </summary>
        public string ProductType { get; set; }

        #endregion

        #region ==== 附件属性 ====

        /// <summary>
        /// 商品名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 商品默认图片
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// 1：积分；2：蓝豆;3：异常
        /// </summary>
        public int BuyMode
        {
            get
            {
                if (this.Integral>0&&this.BlueBean<=0)
                {
                    return 1;
                }

                if (this.Integral <= 0 && this.BlueBean > 0)
                {
                    return 2;
                }

                return 3;
            }
        }
        #endregion
    }
}
