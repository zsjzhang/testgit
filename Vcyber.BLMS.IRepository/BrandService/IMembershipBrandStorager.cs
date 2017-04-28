using System;
using System.Collections.Generic;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.IRepository
{
    public interface IMembershipBrandStorager
    {
        int AddMembershipBrand(MembershipBrand item);

        IEnumerable<MembershipBrand> SelectMembershipBrandByUserId(string userId);
    }
}
