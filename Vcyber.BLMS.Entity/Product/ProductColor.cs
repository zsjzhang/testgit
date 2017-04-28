using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    /// <summary>
    /// 商品颜色
    /// </summary>
    public class ProductColor
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 显示值
        /// </summary>
        public string Text { get; set; }

       /// <summary>
       /// value
       /// </summary>
        public string Value { get; set; }
    }
}
