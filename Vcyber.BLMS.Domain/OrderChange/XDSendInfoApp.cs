using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.IRepository;

namespace Vcyber.BLMS.Domain
{
    public class XDSendInfoApp : IXDSendInfoApp
    {
        public XDSendInfoApp() { }
        public bool AddXDSendInfo(Entity.XDSendInfo xDSendInfo)
        {
            return _DbSession.XDSendInfoStorager.AddXDSendInfo(xDSendInfo);
        }
    }
}
