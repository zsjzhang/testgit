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
    public class ActivitiesSignUpApp : IActivitiesSignUpApp
    {
       public int SignUpActivities(ActivitiesSignUp entity)
       {
           return _DbSession.ActivitiesSignUpStorager.SignUpActivities(entity);
       }
       public IEnumerable<ActivitiesSignUp> GetSignUpList(int id, string userName, int? start, int? count, out int total)
       {
           return _DbSession.ActivitiesSignUpStorager.GetSignUpList(id, userName,start, count, out total);
       }

        public IEnumerable<ActivitiesSignUp> GetSignUpListByUserId(string userId, int? start, int? count, out int total)
       {
           return _DbSession.ActivitiesSignUpStorager.GetSignUpListByUserId(userId, start, count, out total);
      
        }

        public bool CancelSignUp(int id, int userId)
       {
           return _DbSession.ActivitiesSignUpStorager.CancelSignUp(id,userId);
       }

        public ActivitiesSignUp GetItemByUserIdAndActivitiesId(string userId, int activitiesId)
        {
            return _DbSession.ActivitiesSignUpStorager.GetItemByUserIdAndActivitiesId(userId, activitiesId);
        }
    }
}
