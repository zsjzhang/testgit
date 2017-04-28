using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    /// <summary>
    /// 退款图片
    /// </summary>
    public class RebackImage
    {
        #region ==== 构造函数 ====

        public RebackImage() { }

        #endregion

        #region ==== 公共属性 ====

        public int ID { get; set; }

        public int RebackID { get; set; }

        public string Image { get; set; }

        #endregion
    }
}
