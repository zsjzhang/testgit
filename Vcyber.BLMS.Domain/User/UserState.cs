using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Domain.User
{
    using System.Configuration;
    using System.Web.Caching;

    using Newtonsoft.Json;

    using Vcyber.BLMS.Application.User;
    using Vcyber.BLMS.Common;
    using Vcyber.BLMS.Entity.Generated;
    using Vcyber.BLMS.Entity.User;
    using Vcyber.BLMS.IRepository;

    public class UserStateApp:IUserStateApp
    {
        public string Add(UserStateEntity entity, string owner)
        {
            string id = Guid.NewGuid().ToString().Replace("-", string.Empty);
            DateTime expiration = DateTime.Now.AddHours(int.Parse(ConfigurationManager.AppSettings["StateExpiration"]));
            UserState state = new UserState { Id = id, Value = JsonConvert.SerializeObject(entity), Expiration = expiration, Owner = owner, Entity = entity, CreateTime = DateTime.Now, UpdateTime = DateTime.Now };
            VcyberCache.Add(
                id,
                state,
                null,
                state.Expiration,
                Cache.NoSlidingExpiration,
                CacheItemPriority.Default,
                null);
            _DbSession.UserStateStorager.Add(state);
            return id;
        }

        public UserState Get(string id)
        {
            UserState state = (UserState)VcyberCache.Get(id);
            if (state == null)
            {
                state = _DbSession.UserStateStorager.Get(id);
                if (state != null)
                {
                    state.Entity = JsonConvert.DeserializeObject<UserStateEntity>(state.Value);
                    VcyberCache.Add(id, state, null, state.Expiration, Cache.NoSlidingExpiration, CacheItemPriority.Default, null);
                }
            }
            return state;
        }

        public void Remove(string id)
        {
            VcyberCache.Remove(id);
        }

        public bool IsExpired(string id)
        {
            return _DbSession.UserStateStorager.IsExpired(id);
        }
    }
}
