using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.Application
{
    public interface IUserSecurityApp
    {
        //获取用户安全概况
        UserSecurityInfo GetSecurityInfo(string userGuid);


        #region 密码相关

        //找回密码
        ReturnResult FindPassword(string  phoneNumber,string dataSource);

        //修改密码
        ReturnResult ChangePassword(string  phoneNumber,string dataSource);

        ReturnResult RegisterAcount(string phoneNumber, string dataSource);
        /// <summary>
        /// 预留－验证支付密码
        /// </summary>
        /// <param name="userId">用户名</param>
        /// <param name="payPassword">支付密码</param>
        /// <returns>验证结果信息</returns>
        ReturnResult ValidatePayPassword(string userId, string payPassword);

        #endregion



        #region 手机绑定

        //绑定手机
        ReturnResult BindMobile();

        //修改绑定手机
        ReturnResult ChangeBindMobile();

        #endregion



        #region 邮箱绑定

        //绑定邮箱
        ReturnResult BindEmail();

        //修改绑定邮箱
        ReturnResult ChangeBindEmail();

        //发送确认邮件
        ReturnResult SendConfirmEmail(string emailAddress);

        #endregion







        #region 密保相关

        //创建或修改密保
        ReturnResult CreateQuestionAndAnswer(List<UserPwQuestion> questions, int userId);

        //验证密保
        ReturnResult ValidateQuestionAndAnswer(List<UserPwQuestion> questions, int userId);

        //查询用户设定的密保
        IEnumerable<UserPwQuestion> SelectQuestionAndAnswer(int userId);

        //查询密保问题
        IEnumerable<PwQuestion> SelectQuestion();

        #endregion

        #region 验证码相关

        //发送手机验证码
        ReturnResult SendMobileVerifyCode(string phoneNumber, int number, string dataSource);
        //保存手机验证码
        bool SaveMobileVerifyCode(string phoneNumber, string code, string dataSource);

        //验证手机验证码是否正确
        ReturnResult ValidateMobileVerifyCode(string phoneNumber, string verifyCode);

        ReturnResult SendVerifyCodeWithMessage(string phoneNumber, int number, string message);

        #endregion


    }
}
