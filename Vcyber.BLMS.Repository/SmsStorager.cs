using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.IRepository;
using Vcyber.BLMS.Entity.SelectCondition;

namespace Vcyber.BLMS.Repository
{
    /// <summary>
    /// 短信操作
    /// </summary>
    public class SmsStorager : ISmsStorager
    {
        #region ==== 公共方法 ====

        /// <summary>
        /// 获取短信内容
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public SmsMessage SelectContent(ESmsType type)
        {
            string sql = "select * from Sms where code=@Id";
            return DbHelp.QueryOne<SmsMessage>(sql, new { Id=(int)type});
        }

        #endregion
    }
}
