using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.Entity.Generated;

namespace Vcyber.BLMS.Repository
{
    public class TestDriveRepository
    {
        public CSSonataService GetOne(string activityName, string openId)
        {
            string sql = @"
select * from CS_TestDrive
where (activityName=@activityName or activityName='lingding') and OpenId=@OpenId
";
            return DbHelp.QueryOne<CSSonataService>(sql, new { @activityName = activityName, @OpenId = openId });
        }

        public CSTestDrive GetOne4Wap(string activityName, string phoneNumber, string dataSource)
        {
            string sql = @"
select * from CS_TestDrive
where (activityName=@activityName or activityName='lingding') and Phone=@Phone 
";
            return DbHelp.QueryOne<CSTestDrive>(sql, new { @activityName = activityName, @Phone = phoneNumber, @DataSource = dataSource });
        }
    }
}
