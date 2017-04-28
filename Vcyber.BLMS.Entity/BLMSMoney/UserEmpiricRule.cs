using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    /// <summary>
    /// 用户经验规则
    /// </summary>
    public class UserEmpiricRule
    {
        #region ==== 构造函数 ====

        public UserEmpiricRule() { }

        #endregion

        #region ==== 公共属性 ====

        /// <summary>
        /// 规则Guid
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 获得经验值
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// 规则类型1:注册；2:登陆；3:连续登陆；4：修改密码.........
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 获取方式；1:一次性；2：每日；3：每月;4:每次;5:双月
        /// </summary>
        public int AcquireMode { get; set; }

        /// <summary>
        /// 最大获取次数
        /// </summary>
        public int MaxCount { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 规程是否失效：0：启用；1：失效
        /// </summary>
        public int DataState { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        #endregion
    }
}
