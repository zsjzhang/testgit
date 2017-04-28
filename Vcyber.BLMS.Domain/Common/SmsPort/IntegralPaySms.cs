using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.IRepository;

namespace Vcyber.BLMS.Domain.Common
{
    /// <summary>
    /// 积分支付短信
    /// </summary>
    internal class IntegralPaySms : SmsSendBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        protected override string CreateContent(SmsMessage message, SmsData data)
        {
            return string.Format(message.SmsContent, data.UserName, "一次", data.IntegralPayValue);
        }
    }
}
