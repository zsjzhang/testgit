using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vcyber.BLMS.FrontWeb.Models
{
    /// <summary>
    /// 用户账户关联信息
    /// </summary>
    public class UserRelevanceV
    {
        #region ==== 构造函数 ====

        public UserRelevanceV() { }

        #endregion

        #region ==== 公共属性 ====

        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }

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
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string PhoneNumber { get; set; }


        /// <summary>
        /// 商城平台 图片
        /// </summary>
        public string MallImg { get; set; }

        #endregion

        #region ==== 附件属性 ====

        /// <summary>
        /// 用户登录信息
        /// </summary>
        public string Password { get; set; }

        #endregion

        #region ==== 公共方法 ====

        public bool ValidateRelevance(out string message)
        {
            message = string.Empty;

            if (string.IsNullOrEmpty(this.MallCode))
            {
                message = "请选择关联平台";
                return false;
            }

            if (string.IsNullOrEmpty(this.UserName)||string.IsNullOrEmpty(this.Password))
            {
                message = "会员名和密码不能为空";
                return false;
            }

            return true;
        }

        #endregion
    }
}