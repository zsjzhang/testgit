using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Common;

namespace Vcyber.BLMS.Entity.Enum
{
    public enum EUserStatus
    {
        /// <summary>
        /// 启用
        /// </summary>
        [EnumDescribe("启用")]
        Enable = 0,

        /// <summary>
        /// 未启用
        /// </summary>
        [EnumDescribe("未启用")]
        NoEnable = 1
    }

    public enum LoginUserStatus
    {
       
        [EnumDescribe("用户不存在")]
        NoExistsUser = 0,

        [EnumDescribe("用户或密码错误")]
         NameOrPassWordError = 1,

        [EnumDescribe("登录失败")]
        Failed=2,

        [EnumDescribe("登录成功")]
        Sucessed=3,

        [EnumDescribe("需要修改密码")]
        NeedModfiyPassWord = 4,


    }
}
