using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.IRepository;

namespace Vcyber.BLMS.Repository
{
    public class XDInviterStorager : IXDInviterStorager
    {
        public bool AddXDInviter(XDInviter xDInviter)
        {
            string sql = "INSERT INTO [XD_Inviter]([ActivityId],[InviterName],[InviterMobile],[InviterUserId],[InviteredName],[InviteredMobile],[InviteredCar],[InviterTime],[InviterType],[InviterSource],[IsValid],[CreaterId],[CreaterName],[CreaterTime],[UpdaterId],[UpdaterName],[UpdaterTime])VALUES(@ActivityId,@InviterName,@InviterMobile,@InviterUserId,@InviteredName,@InviteredMobile,@InviteredCar,@InviterTime,@InviterType,@InviterSource,@IsValid,@CreaterId,@CreaterName,@CreaterTime,@UpdaterId,@UpdaterName,@UpdaterTime)";
            return DbHelp.Execute(sql, xDInviter) > 0;
        }
        public int IsExistXDInviter(string inviterUserId, int activityId)
        {
            string sql = "SELECT COUNT(1) FROM XD_Inviter WHERE InviterUserId=@InviterUserId AND ActivityId=@ActivityId AND IsValid=1";

            return DbHelp.ExecuteScalar<int>(sql, new { InviterUserId = inviterUserId, ActivityId=activityId });
        }


        public bool IsInvited(string mobile, int activityId)
        {
            string sql = "SELECT COUNT(1) FROM XD_Inviter WHERE InviteredMobile=@InviteredMobile AND ActivityId=@ActivityId AND IsValid=1";

            return DbHelp.ExecuteScalar<int>(sql, new { InviteredMobile = mobile, ActivityId = activityId }) > 0;
        }
    }



   
}
