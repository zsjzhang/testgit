using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    /// <summary>
    /// 卡券核销索赔请求参数
    /// </summary>
   public class RequestCarClaimInformation
    {
        /// <summary>
        /// 请求开始时间
        /// </summary>
        public string starttime { get; set; }

        /// <summary>
        /// 请求结束时间
        /// </summary>
        public string endtime { get; set; }
    }
}
