using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    /// <summary>
    /// 功能信息表
    /// </summary>
    public class Function
    {
        #region ==== 构造函数 ====

        public Function()
        { }

        #endregion

        #region ==== 公共属性 ====

        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 功能名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 父Id
        /// </summary>
        public int ParentId { get; set; }

        /// <summary>
        /// 功能类型
        /// </summary>
        public int FType { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public int IsDel { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Describe { get; set; }

        /// <summary>
        /// 默认Url
        /// </summary>
        public string DefaultUrl { get; set; }


        public string Action { get; set; }
        public string Controller { get; set; }

        public int Rate { get; set; }

        public string RouteSelection { get; set; }

        public string ChildId { get; set; }

        #endregion
    }
}
