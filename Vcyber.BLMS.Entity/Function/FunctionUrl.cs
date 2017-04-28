using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    /// <summary>
    /// 功能访问Url信息
    /// </summary>
    public class FunctionUrl
    {
        #region ==== 构造函数 ====

        public FunctionUrl() { }

        #endregion

        #region ==== 公共属性 ====

        /// <summary>
        /// UrlId
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 功能Id
        /// </summary>
        public int FunId { get; set; }

        public string Action { get; set; }
        public string Controller { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        public int IsDel { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Describe { get; set; }

        #endregion
    }
}
