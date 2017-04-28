using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.IRepository;
using Vcyber.BLMS.Application;

namespace Vcyber.BLMS.Domain
{
   //public class GetVinCustTimeApp:IGetVinCustTime
   // {
   //    public IEnumerable<GetVinCustTimeEF> GetVinCustTime()
   //    {
   //        return _DbSession.GetVinCustTime.GetVinCustTime();
   //    }
   // }

   public class GetVinCustTimeApp : IGetVinCustTimeApp
   {
       public IEnumerable<GetVinCustTimeEF> GetVinCustTime()
       {
           var result = _DbSession.GetVinCustTimeStorager.GetVinCustTime();
           return result;
       }
       public IEnumerable<GetVinCustTimeEF> GetVinCustTime(string start ,string  end)
       {
           var result = _DbSession.GetVinCustTimeStorager.GetVinCustTime(start , end);
           return result;
       }
   }
}
