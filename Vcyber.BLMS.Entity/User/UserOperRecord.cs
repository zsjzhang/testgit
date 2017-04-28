using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    /// <summary>
    /// 用户操作记录
    /// </summary>
    public class UserOperRecord
    {
        #region ==== 构造函数 ====

        public UserOperRecord() { }

        #endregion

        #region ==== 公共属性 ====

        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///  用户Id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 用户操作类型（1=修改登录密码、2=找回登录密码、3=获取手机验证码、4=修改邮箱、5=修改密保、6=修改手机绑定）
        /// </summary>
        public int OperType { get; set; }

        /// <summary>
        /// 用户操作内容
        /// </summary>
        public string Content { get; set; }


        public DateTime CreateTime { get; set; }


        #endregion
    }
}
