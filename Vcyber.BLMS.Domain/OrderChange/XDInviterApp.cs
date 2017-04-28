using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.IRepository;

namespace Vcyber.BLMS.Domain
{
    public class XDInviterApp : IXDInviterApp
    {
        public XDInviterApp() { }
        public bool AddXDInviter(XDInviter xDInviter)
        {
            return _DbSession.XDInviterStorager.AddXDInviter(xDInviter);
        }

        public int IsExistXDInviter(string inviterUserId, int activityId)
        {
            return _DbSession.XDInviterStorager.IsExistXDInviter(inviterUserId, activityId);
        }


        public bool IsInvited(string mobile, int activityId)
        {
            return _DbSession.XDInviterStorager.IsInvited(mobile, activityId);
        }
    }
}
