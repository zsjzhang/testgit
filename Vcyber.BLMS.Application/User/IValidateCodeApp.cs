using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Application
{
    /// <summary>
    /// 手机验证码操作
    /// </summary>
    public interface IValidateCodeApp
    {
        /// <summary>
        /// 保存手机验证码
        /// </summary>
        /// <param name="phoneNumber"></param>
        void Add(string phoneNumber);

        /// <summary>
        /// 手机号是否验证通过
        /// </summary>
        /// <param name="phoneNumber">手机号</param>
        /// <param name="code">验证码</param>
        /// <param name="maxMin">验证码失效最大分钟数</param>
        /// <returns></returns>
        bool IsSuccess(string phoneNumber, string code, int maxMin);

        /// <summary>
        /// 手机号当天验证次数
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        int GetCount(string phoneNumber);

        int InHourValidateCount(string phoneNumber);

        int InMinuteValidateCount(string phoneNumber);

        int GetValidateCountByPhoneNumber(string phoneNumber);
    }
}
