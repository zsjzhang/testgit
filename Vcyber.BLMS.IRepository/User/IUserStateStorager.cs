using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.IRepository.User
{
    using Vcyber.BLMS.Entity.Generated;

    public interface IUserStateStorager
    {
        UserState Get(string id);

        void Add(UserState state);

        void Remove(string id);

        bool IsExpired(string key);
    }
}
