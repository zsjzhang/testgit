using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.Repository
{
    public class RecommendStorager
    {
        public void Add(Recommend recmd)
        {
            if (DbHelp.Execute("select * from Recommend where OpenId=@OpenId and ActivityType=@ActivityType and PhoneNumber=@PhoneNumber", new { OpenId = recmd.OpenId, ActivityType = recmd.ActivityType, PhoneNumber = recmd.PhoneNumber }) > 0)
            {
                string sql_recommendInfo = @"
Insert into RecommendInfo
(
OpenId,
ActivityType,
Name,
PhoneNumber,
Created 
)
values
(
@OpenId,
@ActivityType,
@Name,
@PhoneNumber,
@Created
) 
";
                DbHelp.ExecuteScalar<int>(sql_recommendInfo, new { @Name = recmd.Name, @PhoneNumber = recmd.PhoneNumber, @Created = DateTime.Now });
            }
        }
    }
}
