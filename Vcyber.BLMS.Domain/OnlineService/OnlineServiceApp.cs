using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.IRepository;

namespace Vcyber.BLMS.Domain
{
    public class OnlineServiceApp : IOnlineServiceApp
    {
        public int AddOnLineService(Entity.OnlineServiceRecord service)
        {            
             return _DbSession.OnlineServiceStorager.AddOnLineService(service);
        }
    }
}
