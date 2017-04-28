using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Vcyber.BLMS.Application;

namespace Vcyber.BLMS.FrontWeb.Models
{
    /// <summary>
    /// 注册信息
    /// </summary>
    public class RegisterInfo
    {
        #region ==== 构造函数 ====

        public RegisterInfo() { }

        #endregion

        #region ==== 公共属性 ====

        /// <summary>
        /// 用户名称
        /// </summary>
        public string userName { get; set; }

        /// <summary>
        /// 登录密码
        /// </summary>
        [DataType(DataType.Password)]
        public string password { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        public string realName { get; set; }

        /// <summary>
        /// 身份证编号
        /// </summary>
        public string identityNumber { get; set; }

        #endregion

        #region ==== 公共方法 ====

        public bool ValidatePara(out string message)
        {
            message = string.Empty;

            if (string.IsNullOrEmpty(this.userName) || string.IsNullOrEmpty(this.password) || string.IsNullOrEmpty(this.realName) ||
                string.IsNullOrEmpty(this.identityNumber))
            {
                message = "请输入必填的注册信息。";
                return false;
            }

            bool result = _AppContext.UserInfoApp.IsName(this.userName);

            if (result)
            {
                message = "手机号已经使用。";
                return false;
            }

            result = _AppContext.UserInfoApp.IsIdentityNumber(identityNumber);

            if (result)
            {
                message = "身份证编号已经使用。";
                return false;
            }

            return true;
        }

        #endregion
    }
}