using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    /// <summary>
    /// 功能 树结构
    /// </summary>
    public class FunctionTreeView
    {
        #region ==== 构造函数 ====

        public FunctionTreeView()
        { }

        #endregion

        #region ==== 公共属性 ====

        /// <summary>
        /// 功能
        /// </summary>
        public Function FuncData { get; set; }

        /// <summary>
        /// 孩子功能
        /// </summary>
        public List<FunctionTreeView> ChildeFuncs { get; set; }

        #endregion
    }
}
