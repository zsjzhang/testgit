using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.Entity.Enum;

namespace Vcyber.BLMS.Application
{
    public interface ISmsApp
    {
        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="type">短信类型</param>
        /// <param name="phoneNumber">手机号</param>
        /// <param name="data">短信数据</param>
        /// <returns></returns>
        ReturnResult SendSMS(ESmsType type, string phoneNumber, string[] datas, bool isValidSendCount = true);

        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="phoneNumber">手机号</param>
        /// <param name="message">短信内容</param>
        /// <returns>执行结果</returns>
        ReturnResult SendSMS(string phoneNumber, string message, bool isValidSendCount = true);

        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="type">短信类型</param>
        /// <param name="data">短信数据</param>
        /// <returns></returns>
        ReturnResult SendSMS(ESmsType type, SmsData data, bool isValidSendCount = true);
    }
}
