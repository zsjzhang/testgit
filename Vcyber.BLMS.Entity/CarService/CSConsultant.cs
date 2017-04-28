using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    /// <summary>
    /// 服务顾问信息
    /// </summary>
    public class CSConsultant
    {
        #region ==== 构造函数 ====

        public CSConsultant() { }

        #endregion

        #region ==== 公共属性 ====

        /// <summary>
        /// 顾问Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 经销商Id
        /// </summary>
        public string DealerId { get; set; }

        /// <summary>
        /// 经销商名称
        /// </summary>
        public string DealerName { get; set; }

        /// <summary>
        /// 照片
        /// </summary>
        public string Photo { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string Tel { get; set; }

        /// <summary>
        /// 性别（0：女;1:男）
        /// </summary>
        public int Sex { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Comment { get; set; }


        #endregion
    }
}
