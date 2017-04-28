using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    /// <summary>
    /// 用户审核信息
    /// </summary>
    public class UserVerify
    {
        #region ==== 构造函数 ====

        public UserVerify() { }

        #endregion

        #region ==== 公共属性 ====

        /// <summary>
        /// 用户Id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 用户审核类型（2=二代身份证）
        /// </summary>
        public int VerifyType { get; set; }

        /// <summary>
        /// 审核证件号
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// 用户安全验证状态（1=验证通过、2=验证失败、3=验证中）
        /// </summary>
        public int State { get; set; }

        /// <summary>
        /// 用户验证信息有效时间
        /// </summary>
        public DateTime ValidTime { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        public string RealName { get; set; }

        /// <summary>
        /// 用户验证图片
        /// </summary>
        public string Img { get; set; }

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
