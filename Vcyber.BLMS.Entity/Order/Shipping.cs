using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    /// <summary>
    /// 物流公司信息
    /// </summary>
    public class Shipping
    {
        #region ==== 构造函数 ====

        public Shipping() { }

        #endregion

        #region ==== 公共属性 ====

        public int ID { get; set; }
        /// <summary>
        /// 类型
        /// </summary>	
        public int Type { get; set; }
        /// <summary>
        /// 值
        /// </summary>	
        public int Value { get; set; }
        /// <summary>
        /// 名称
        /// </summary>	
        public string Name { get; set; }

        #endregion
    }
}
