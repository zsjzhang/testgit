using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.IRepository;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.Domain
{
    /// <summary>
    /// 卡券核销索赔
    /// </summary>
   public class XDCarClaimInformationApp : IXDCarClaimInformationApp
    {
        public IEnumerable<Entity.ResCarClaimInformation> GetCarClaimInformation(string starttime, string endtime)
        {
            return _DbSession.XDCarClaimInformationStorager.GetCarClaimInformation(starttime, endtime);
        }
    }
}
