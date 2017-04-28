using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    /// <summary>
    /// 商品图片
    /// </summary>
    public class ProductImage
    {
        #region ==== 构造函数 ====

        public ProductImage() { }

        #endregion

        #region ==== 公共属性 ====

        public int ID { get; set; }

        /// <summary>
        /// 商品ID
        /// </summary>
        public int ProductID { get; set; }

        private string image;

        /// <summary>
        /// 图片路径
        /// </summary>
        public string Image { get {return this.image; } set { this.image = value; } }

        /// <summary>
        /// '是否是默认'(0:不默认;1默认),
        /// </summary>
        public int IsDefault { get; set; }

        public int Datastate { get; set; }

        #endregion
    }
}
