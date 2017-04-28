using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    /// <summary>
    /// 用户关联账户
    /// </summary>
    public class UserAccountRelevance
    {
        #region ==== public constructor ====

        public UserAccountRelevance() { }

        #endregion

        #region === public method ====

        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 车音通用户Id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        ///  商城编号
        /// </summary>
        public string MallCode { get; set; }

        /// <summary>
        /// 商城用户编号
        /// </summary>
        public string MallUserCode { get; set; }

        /// <summary>
        /// 商城名称
        /// </summary>
        public string MallName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime UpdateTime { get; set; }

        #endregion
    }
}
