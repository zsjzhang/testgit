using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vcyber.BLMS.WebApi.Models
{
    /// <summary>
    /// 会员等级信息
    /// </summary>
    public class MembershipLevelV
    {
        #region ==== 构造函数 ====

        public MembershipLevelV() { }

        #endregion

        #region ==== 公共属性 ====

        /// <summary>
        /// 会员等级(1:一星；2：二星；3：三星；0：数据为空)
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// 会员类型(1:非车主；2：车主；3：索九；0：数据为空)
        /// </summary>
        public int Type
        {
            get;
            set;
        }

        #endregion
    }
}