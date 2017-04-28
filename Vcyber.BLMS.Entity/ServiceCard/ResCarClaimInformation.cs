using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Entity
{
    /// <summary>
    /// 卡券核销索赔
    /// </summary>
   public class ResCarClaimInformation
    {
        /// <summary>
        /// 车架号
        /// </summary>
        public string VIN { get; set; }

        /// <summary>
        /// 活动代码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 活动编号
        /// </summary>
        public string ActivityCode { get; set; }

        /// <summary>
        /// 卡券核销时间
        /// </summary>
        public string CreateTime { get; set; }
    }
}
