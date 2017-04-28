using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.IRepository
{
    public interface IMembershipLoginNotifyStorager
    {
        int Insert(MembershipLoginNotify membershipLoginNotify);
        bool IsExists(string userId, LoginNotifyType loginNotifyType);
    }
}
