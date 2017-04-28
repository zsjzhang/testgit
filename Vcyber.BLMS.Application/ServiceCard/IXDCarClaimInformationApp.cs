using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.Application
{
   public interface IXDCarClaimInformationApp
    {
        /// <summary>
        /// 卡券核销索赔
        /// </summary>
        /// <param name="activitycode">活动代码</param>
        /// <param name="createtime">卡券核销日期</param>
        /// <returns></returns>
        IEnumerable<ResCarClaimInformation> GetCarClaimInformation(string starttime, string endtime);
    }
}
