using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.IRepository
{
    public interface IValidateCodeStorager
    {
        /// <summary>
        /// 添加验证码记录
        /// </summary>
        /// <param name="code">数据实体</param>
        /// <param name="id">返回记录ID</param>
        /// <returns>执行结果</returns>
        bool Add(ValidateCode code);

        /// <summary>
        /// 查询用户的验证码
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>验证码</returns>
        ValidateCode Select(string phoneNumber);

        /// <summary>
        /// 获取手机号当天的验证次数
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        int ValidateCount(string phoneNumber);

        /// <summary>
        /// 获取1分钟内是否发送过短信
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        int InHourValidateCount(string phoneNumber);

        int InMinuteValidateCount(string phoneNumber);

        int GetValidateCountByPhoneNumber(string phoneNumber);
    }
}
