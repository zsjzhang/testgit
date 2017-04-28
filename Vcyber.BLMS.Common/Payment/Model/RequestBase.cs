using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Common.Payment.Model
{
    [DataContract]
    /// <summary>
    /// 请求类Model基类
    /// </summary>
    public class RequestBase
    {        
        private string _serviceVersion = "1.0";
        private string _inputCharset = "GBK";
        private int _signKeyIndex = 1;
        /// <summary>
        /// 版本号，默认为1.0
        /// </summary>
        public string ServiceVersion
        {
            get { return _serviceVersion; }
            set { _serviceVersion = value; }
        }
        /// <summary>
        /// 字符编码,取值：GBK、UTF-8，默认：GBK
        /// </summary>
        public string InputCharset
        {
            get { return _inputCharset; }
            set { _inputCharset = value; }
        }
        /// <summary>
        /// 多密钥支持的密钥序号，默认1
        /// </summary>
        public int SignKeyIndex
        {
            get { return _signKeyIndex; }
            set { _signKeyIndex = value; }
        }
    }
}
