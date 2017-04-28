using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    /// <summary>
    /// 短信信息
    /// </summary>
    public class SmsMessage
    {
        #region ==== 构造函数 ====

        public SmsMessage() { }

        #endregion

        #region ==== 公共属性 ====

        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 短信内容
        /// </summary>
        public string SmsContent { get; set; }


        #endregion
    }
}
