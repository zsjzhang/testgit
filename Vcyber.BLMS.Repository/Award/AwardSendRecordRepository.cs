using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.Repository
{
    public class AwardSendRecordRepository
    {

        public int AddRecord(AwardSendRecord record)
        {
            string sql = @"
insert into AwardSendRecord
(
OpenId,
AwardId,
LotteryDrawPoolId,
PostAddressId,
SendState,
SendStateMemo,
Created
)
values
(
@OpenId,
@AwardId,
@LotteryDrawPoolId,
@PostAddressId,
@SendState,
@SendStateMemo,
@Created
)
";
            return DbHelp.Execute(sql, new { @OpenId = record.OpenId, @AwardId = record.Award.Id, @LotteryDrawPoolId = record.LotteryDrawPoolId, @PostAddressId = record.PostAddressId, @SendState = record.SendState, @SendStateMemo = record.SendStateMemo, @Created = DateTime.Now });

        }

        public IEnumerable<Award> GetAward(string openId, LotteryDrawPoolType type)
        {
            string sql = @"
select * from AwardSendRecord asr
inner join Award a on asr.AwardId=a.id
left join LotteryDrawPool ldp on asr.LotteryDrawPoolId=ldp.id
where ldp.PoolType=@PoolType and asr.OpenId=@OpenId
";
            return DbHelp.Query<Award>(sql, new { @PoolType = type, @OpenId = openId });
        }

        public Dictionary<string, List<LotteryModel>> GetAllLottery(string phoneNumber)
        {
            Dictionary<string, List<LotteryModel>> re = new Dictionary<string, List<LotteryModel>>();

            string sql_testDeive = @"
select 
a.Name as AwardName,
a.AwardType as AwardType,
testrecord.Phone as PhoneNumber
from AwardSendRecord asr
inner join Award a on asr.AwardId=a.id
inner join CS_TestDrive testrecord on asr.openid=testrecord.openid
left join LotteryDrawPool ldp on asr.LotteryDrawPoolId=ldp.id
where 1=1 and ldp.PoolType=@PoolType 
";

            string sql_recommend = @"
select 
a.Name as AwardName,
a.AwardType as AwardType, 
wx.Tel as PhoneNumber 
from AwardSendRecord asr
inner join Award a on asr.AwardId=a.id
inner join WXBind wx on wx.OpenId=asr.OpenId
left join LotteryDrawPool ldp on asr.LotteryDrawPoolId=ldp.id
where 1=1 and ldp.PoolType=@PoolType
";
            if (!string.IsNullOrEmpty(phoneNumber))
            {
                sql_testDeive += "and testrecord.Phone=@PhoneNumber";
                sql_recommend += "and wx.Tel=@PhoneNumber";
            }
            var result1 = DbHelp.Query<LotteryModel>(sql_testDeive, new { @PoolType = LotteryDrawPoolType.LingDong_TestDeive, @PhoneNumber = phoneNumber }).ToList();

            var result2 = DbHelp.Query<LotteryModel>(sql_recommend, new { @PoolType = LotteryDrawPoolType.LingDong_Recommend, @PhoneNumber = phoneNumber }).ToList();

            re.Add("LingDong_TestDeive", result1);
            re.Add("LingDong_Recommend", result2);
            return re;
        }

        public void UpdateSendRecordForWxCard(LotteryDrawPool LDP, string openid)
        {
            string sql = @"
update AwardSendRecord 
set SendState=@SendState
where OpenId=@OpenId and AwardId=@AwardId and LotteryDrawPoolId=@LotteryDrawPoolId 
";
            DbHelp.Execute(sql, new { @OpenId = openid, @AwardId = LDP.AwardId, @LotteryDrawPoolId = LDP.Id });
        }
    }
}
