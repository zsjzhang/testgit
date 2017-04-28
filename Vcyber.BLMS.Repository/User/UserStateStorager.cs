using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vcyber.BLMS.Repository.User
{
    using PetaPoco;

    using Vcyber.BLMS.Entity.Generated;
    using Vcyber.BLMS.IRepository.User;
    using Vcyber.BLMS.Repository.Entity.Generated;

    public class UserStateStorager : IUserStateStorager
    {
        public static UserState Get(string id)
        {
            return PocoHelper.CurrentDb().SingleOrDefault<UserState>("where Id=@0 and expiration>@1", new AnsiString(id), DateTime.Now.ToString("yyyy-MM-dd HH24:mm:ss"));

        }
        void IUserStateStorager.Add(UserState state)
        {
            Add(state);
        }

        void IUserStateStorager.Remove(string id)
        {
            Remove(id);
        }

        bool IUserStateStorager.IsExpired(string key)
        {
            return IsExpired(key);
        }

        UserState IUserStateStorager.Get(string id)
        {
            return Get(id);
        }

        public new static void Add(UserState state)
        {
            using (Database db = PocoHelper.CurrentDb())
            {
                UserState newState = db.SingleOrDefault<UserState>("where Id=@0", state.Id);
                if (newState != null)
                {
                    newState.Value = state.Value;
                    newState.Expiration = state.Expiration;
                    db.Update(newState);
                }
                else
                {
                    state.Insert();
                }
                if (DateTime.Now.Hour % 3 == 0)
                    db.Delete<UserState>("where expiration<@0", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            }
        }

        public static void Remove(string id)
        {

            PocoHelper.CurrentDb().Delete<UserState>(id);

        }


        public static bool IsExpired(string key)
        {

            UserState state = Get(key);
            return state.Expiration < DateTime.Now;

        }
    }
}
