using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.IRepository;

namespace Vcyber.BLMS.Repository
{
    public class MembershipLoginNotifyStorager : IMembershipLoginNotifyStorager
    {
        public int Insert(BLMS.Entity.MembershipLoginNotify membershipLoginNotify)
        {
            string sql = @"INSERT INTO MembershipLoginNotify
           (UserId
           ,DataSource
           ,NotifyType)
     VALUES
           (@UserId
           ,@DataSource
           ,@NotifyType)";

            return DbHelp.Execute(sql, membershipLoginNotify);
        }

        public bool IsExists(string userId, BLMS.Entity.LoginNotifyType loginNotifyType)
        {
            bool result = false;
            string sql = @"
		   select  1 from MembershipLoginNotify where UserId=@UserId and NotifyType=@NotifyType";
            var record = DbHelp.ExecuteScalar<int>(sql, new
            {
                UserId = userId,
                NotifyType = (int)loginNotifyType
            });
            if (record > 0)
            {
                result = true;
            }
            return result;
        }
    }
}
