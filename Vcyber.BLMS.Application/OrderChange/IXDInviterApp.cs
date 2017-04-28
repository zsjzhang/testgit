using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.Application
{
    public interface IXDInviterApp
    {
        bool AddXDInviter(XDInviter xDInviter);

        int IsExistXDInviter(string inviterUserId, int activityId);
        bool IsInvited(string mobile, int activityId);
    }
}
