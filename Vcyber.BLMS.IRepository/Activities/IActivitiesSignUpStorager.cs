using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.IRepository
{
    public interface IActivitiesSignUpStorager
    {
        int SignUpActivities(ActivitiesSignUp entity);
        IEnumerable<ActivitiesSignUp> GetSignUpList(int id, string userName, int? start, int? count, out int total);
        IEnumerable<ActivitiesSignUp> GetSignUpListByUserId(string userId, int? start, int? count, out int total);
        bool CancelSignUp(int id, int userId);
        ActivitiesSignUp GetItemByUserIdAndActivitiesId(string userId, int activitiesId);
    }
}
