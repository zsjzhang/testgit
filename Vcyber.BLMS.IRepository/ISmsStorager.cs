using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;

namespace Vcyber.BLMS.IRepository
{
    public interface ISmsStorager
    {
        #region ==== 公共方法 ====

        /// <summary>
        /// 获取短信内容
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        SmsMessage SelectContent(ESmsType type);

        #endregion
    }
}
