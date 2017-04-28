using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.IRepository
{
   public interface IXDCarClaimInformationStorager
    {
        IEnumerable<ResCarClaimInformation> GetCarClaimInformation(string starttime, string endtime);
    }
}
