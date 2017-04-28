using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Common.Payment.Model
{
    /// <summary>
    /// 响应Model基类
    /// </summary>
    public class ResponseBase
    {
        /// <summary>
        /// 返回状态码，0表示成功，其它未定义。如果retcode非0，其他参数无意义。
        /// </summary>
        public int Retcode { get; set; }
        /// <summary>
        /// 返回信息，如非空，为错误原因。
        /// </summary>
        public string Retmsg { get; set; }
    }
}
