using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    /// <summary>
    /// 缴费获取待审核列表请求参数
    /// </summary>
    public class RequestPaymentAccessPoint
    {
        /// <summary>
        /// 经销商ID
        /// </summary>
        public string dealerid { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public string starttime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public string endtime { get; set; }
    }
}
