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
    /// 短信发送 鸡肋
    /// </summary>
    internal abstract class SmsSendBase
    {
        #region ==== 抽象方法 ====

        /// <summary>
        /// 转换短信信息
        /// </summary>
        /// <param name="type">短信类型</param>
        /// <param name="data">短信数据尸体</param>
        /// <returns></returns>
        public static string ConvertContent(ESmsType type, string[] datas)
        {            
            SmsMessage content = _DbSession.SmsStorager.SelectContent(type);

            if (content != null)
            {
                return string.Format(content.SmsContent, datas);
            }

            return string.Empty;
        }


        /// <summary>
        /// 转换短信信息
        /// </summary>
        /// <param name="type">短信类型</param>
        /// <param name="data">短信数据尸体</param>
        /// <returns></returns>
        public string ConvertContent(ESmsType type, SmsData data)
        {
            SmsMessage content = _DbSession.SmsStorager.SelectContent(type);

            if (content != null)
            {
                return this.CreateContent(content, data);
            }

            return string.Empty;
        }

        protected abstract string CreateContent(SmsMessage message, SmsData data);

        #endregion
    }
}
