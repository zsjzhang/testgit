using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.Application
{
    public interface IActivitiesSignUpApp
    {
        int SignUpActivities(ActivitiesSignUp entity);
        IEnumerable<ActivitiesSignUp> GetSignUpList(int id, string userName, int? start, int? count, out int total);

        bool CancelSignUp(int id, int userId);
        IEnumerable<ActivitiesSignUp> GetSignUpListByUserId(string userId, int? start, int? count, out int total);
        ActivitiesSignUp GetItemByUserIdAndActivitiesId(string userId, int activitiesId);
    }
}
