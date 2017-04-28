using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    /// <summary>
    ///用户经验记录 
    /// </summary>
    public class UserEmpiricRecord
    {
        #region ==== 构造函数 ====

        public UserEmpiricRecord() { }

        #endregion

        #region ==== 公共属性 ====

        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 数据来源Id
        /// </summary>
        public string SourceId { get; set; }

        /// <summary>
        /// 经验值
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// 累计使用经验值
        /// </summary>
        public int UseValue { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 数据是否有效0：有效；1：删除
        /// </summary>
        public int DataState { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        #endregion
    }
}
