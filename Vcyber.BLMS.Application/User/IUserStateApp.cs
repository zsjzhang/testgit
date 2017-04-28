using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Application.User
{
    using Vcyber.BLMS.Entity.Generated;
    using Vcyber.BLMS.Entity.User;

    public interface IUserStateApp
    {
        string Add(UserStateEntity entity, string owner);

        UserState Get(string id);

        void Remove(string id);

        bool IsExpired(string id);
    }
}
