using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.IRepository;

namespace Vcyber.BLMS.Domain
{
    /// <summary>
    /// 手机验证码逻辑
    /// </summary>
    public class ValidateCodeApp : IValidateCodeApp
    {
        #region ==== 构造函数 ====

        public ValidateCodeApp() { }

        #endregion

        #region ==== 公共方法 ====

        /// <summary>
        /// 保存手机验证码
        /// </summary>
        /// <param name="phoneNumber"></param>
        public void Add(string phoneNumber)
        {
            ValidateCode data = new ValidateCode() { PhoneNumber=phoneNumber,CreateTime=DateTime.Now };
            data.Code = SMSHelper.GetValidateCode(5);

            for (int i = 1; i <= 8; i++)
            {
                //发送验证码
                var result = SMSHelper.SendSMS(new SmsSendMessageDTO { Mobile = phoneNumber, SmsContent = string.Format("您的验证码是： {0}， 请立即使用", data.Code), Type = SmsType.Captcha });

                if (result!=null&&result.Code.Equals("200"))
                {
                    break;
                }
            }

            _DbSession.ValidateCodeStorager.Add(data);
        }

        /// <summary>
        /// 手机号是否验证通过
        /// </summary>
        /// <param name="phoneNumber">手机号</param>
        /// <param name="code">验证码</param>
        /// <param name="maxMin">验证码失效最大分钟数</param>
        /// <returns></returns>
        public bool IsSuccess(string phoneNumber,string code, int maxMin)
        {
           var data= _DbSession.ValidateCodeStorager.Select(phoneNumber);

           if (data!=null&&data.Code.Equals(code)&&System.Math.Abs((DateTime.Now-data.CreateTime).Minutes)<=maxMin)
           {
               return true;
           }

           return false;
        }

        /// <summary>
        /// 手机号当天验证次数
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        public int GetCount(string phoneNumber)
        {
            return _DbSession.ValidateCodeStorager.ValidateCount(phoneNumber);
        }

        public int InHourValidateCount(string phoneNumber)
        {
            return _DbSession.ValidateCodeStorager.InHourValidateCount(phoneNumber);
        }

        public int InMinuteValidateCount(string phoneNumber)
        {
            return _DbSession.ValidateCodeStorager.InMinuteValidateCount(phoneNumber);
        }

        public int GetValidateCountByPhoneNumber(string phoneNumber)
        {
            return _DbSession.ValidateCodeStorager.GetValidateCountByPhoneNumber(phoneNumber);
        }
        #endregion
    }
}
