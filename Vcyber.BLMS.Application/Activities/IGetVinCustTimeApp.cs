using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.Application
{
   public interface IGetVinCustTimeApp
    {
       IEnumerable<GetVinCustTimeEF> GetVinCustTime();
       IEnumerable<GetVinCustTimeEF> GetVinCustTime(string start, string end);
    }

   
}
