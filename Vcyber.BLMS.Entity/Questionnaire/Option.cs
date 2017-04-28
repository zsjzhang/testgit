using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    public class Option
    {
        #region ==== 构造函数 ====

        public Option() { }
        #endregion

        #region ==== 公用属性 ====

        public int Id { get; set; }

        /// <summary>
        /// 所属问题Id 
        /// </summary>
        public int PartentId { get; set; }

        /// <summary>
        /// 选项内容
        /// </summary>
        public string OContent { get; set; }

        /// <summary>
        /// 该选项在问题下的排序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 选项当前状态：0 删除，1 正常
        /// </summary>
        public int State { get; set; }

        /// <summary>
        /// 选项类型 0：普通选线 1：填空选项
        /// </summary>
        public int OType { get; set; }

        /// <summary>
        /// 填空题的值类型 0:不限制  1：整数 2：浮点数
        /// </summary>
        public int OValueType { get; set; }
        #endregion
    }
}
