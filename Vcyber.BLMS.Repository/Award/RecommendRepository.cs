using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;

namespace Vcyber.BLMS.Repository
{
    public class RecommendRepository
    {
        public int Add(Recommend recommend)
        {
            string sql = @"
insert into Recommend 
(
OpenId,
ActivityType,
Name,
PhoneNumber,
DataSource,
Created
)
values
(
@OpenId,
@ActivityType,
@Name,
@PhoneNumber,
@DataSource,
@Created
)
";
            return DbHelp.Execute(sql, new { @OpenId = recommend.OpenId, @ActivityType = recommend.ActivityType, @Name = recommend.Name, @PhoneNumber = recommend.PhoneNumber, @DataSource = recommend.DataSource, @Created = DateTime.Now });
        }

        public IEnumerable<Recommend> GetByOpenId(string activityType, string openid)
        {
            string sql = @"
select * from Recommend
where ActivityType=@ActivityType and OpenId=@OpenId
";
            return DbHelp.Query<Recommend>(sql, new { @ActivityType = activityType, @OpenId = openid });
        }

        public Recommend GetByPhone4Wap(string activityType, string phoneNumber)
        {
            string sql = @"
select * from Recommend
where ActivityType=@ActivityType and OpenId=@PhoneNumber
";
            return DbHelp.QueryOne<Recommend>(sql, new { @ActivityType = activityType, @PhoneNumber = phoneNumber });
        }

        public bool CheckRecommendNewDriver(string openId)
        {
            string sql = @"
select * from Recommend r
inner join IF_Customer cus on r.PhoneNumber=cus.CustMobile
inner join IF_Car car on cus.CustId=car.CustId
where SUBSTRING(car.VIN,4,2)='AD' and r.OpenId=@OpenId
";
            return DbHelp.Execute(sql, new { @OpenId = openId }) > 0;
        }

        public bool CheckTestDriveNewDriver(string openId)
        {
            string sql = @"
select * from CS_TestDrive t
inner join IF_Customer cus on t.Phone=cus.CustMobile
inner join IF_Car car on cus.CustId=car.CustId
where SUBSTRING(car.VIN,4,2)='AD' and t.OpenId=@OpenId and OrderType=@OrderType
";
            return DbHelp.Execute(sql, new { @OpenId = openId, @OrderType = EBMServiceType.LingDong }) > 0;
        }
    }
}
