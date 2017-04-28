using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    /// <summary>
    /// 退款信息
    /// </summary>
    public class Reback
    {
        #region 公共属性

        public int ID { get; set; }

        /// <summary>
        /// 订单ID
        /// </summary>
        public string OrderID { get; set; }

        ///<summary>
        ///商品ID
        ///</summary>
        public int ProductID { get; set; }

        ///<summary>
        ///库存
        ///</summary>
        public int Qty { get; set; }

        ///<summary>
        ///退货原因
        ///</summary>
        public string Reason { get; set; }

        ///<summary>
        ///回复
        ///</summary>
        public string Reply { get; set; }

        ///<summary>
        ///状态
        ///</summary>
        public int State { get; set; }

        public string StateName { get; set; }

        ///<summary>
        ///申请时间
        ///</summary>
        public DateTime ApplyTime { get; set; }

        ///<summary>
        ///审核时间
        ///</summary>
        public DateTime AuditTime { get; set; }

        /// <summary>
        /// 退货图片
        /// </summary>
        public List<string> Image { get; set; }

        /// <summary>
        /// 价格
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 商品积分
        /// </summary>
        public int Integral { get; set; }

        /// <summary>
        /// 退货商品名称
        /// </summary>
        public string Name { get; set; }

        public int MaxCount { get; set; }

        public int RebackQty { get; set; }
        #endregion


    }
}
