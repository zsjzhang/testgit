using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class UserInfo
    {
        #region ==== 构造函数 ====

        public UserInfo() { }

        #endregion

        #region ==== 公共属性 ====

        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 用户登录名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 登录密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 支付密码
        /// </summary>
        public string PayPw { get; set; }

        /// <summary>
        /// 用户状态（0：启用、1=冻结）
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// UpdateTime
        /// </summary>
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public string UserGuid { get; set; }

        /// <summary>
        /// 用户手机号
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 用户姓名
        /// </summary>
        public string RealName { get; set; }

        /// <summary>
        /// 性别（2=女、1=男、3=保密、4=其他）
        /// </summary>
        public int Sex { get; set; }

        /// <summary>
        /// 出生日期
        /// </summary>
        public DateTime BirthTime { get; set; }

        /// <summary>
        /// 爱好（多个爱好已“;”分割）
        /// </summary>
        public string Hobby { get; set; }

        /// <summary>
        /// 用户邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 用户邮箱是否激活（1=激活、2=未激活）
        /// </summary>
        public int EmailConfirmed { get; set; }

        /// <summary>
        ///  用户手机是否激活（1=激活、2=未激活）
        /// </summary>
        public int PhoneNumberConfirmed { get; set; }

        /// <summary>
        /// 身份是否确认（1=确认、2=未确认）
        /// </summary>
        public int IdentityConfirmed { get; set; }

        /// <summary>
        /// 用户身份证编号
        /// </summary>
        public string IdentityNumber { get; set; }

        /// <summary>
        /// 用户证件有效时间
        /// </summary>
        public string IdentityValidTime { get; set; }

        /// <summary>
        /// 身份证图片
        /// </summary>
        public string IdentityImg { get; set; }

        #endregion
    }
}
