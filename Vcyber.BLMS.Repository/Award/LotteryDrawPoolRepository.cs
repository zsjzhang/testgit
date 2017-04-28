using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.Repository
{
    /// <summary>
    /// 奖池
    /// </summary>
    public class LotteryDrawPoolRepository
    {
        public IEnumerable<LotteryDrawPool> GetLotteryDrawPool(LotteryDrawPoolType type)
        {
            string sql = @"
select * from LotteryDrawPool 
where PoolType=@PoolType
and ISNULL(IsPrivate,0)=0 
and ExpireDate>@ExpireDate
";

            IEnumerable<LotteryDrawPool> results = DbHelp.Query<LotteryDrawPool>(sql, new { @PoolType = type, @ExpireDate = DateTime.Now });
            foreach (var p in results)
            {
                string sql_award = @"select * from Award where id=@id";
                p.Award = DbHelp.QueryOne<Award>(sql_award, new { @id = p.AwardId });
            }
            return results;
        }

        /// <summary>
        /// 更新奖品池奖品数量
        /// </summary>
        /// <param name="PoolId">PoolId</param>
        /// <param name="VersionNumber">版本号</param>
        /// <returns></returns>
        internal int UpdateLottery(int Id, int VersionNumber)
        {
            string sql = @"
update LotteryDrawPool
set  Amount=Amount-1, VersionNumber=VersionNumber+1
where Id=@Id and Amount>0 and VersionNumber=@VersionNumber
";
            return DbHelp.Execute(sql, new { @Id = Id, @VersionNumber = VersionNumber });
        }

        internal LotteryDrawPool GetById(int id)
        {
            string sql = @"
select VersionNumber from LotteryDrawPool 
where Id=@Id
";
            return DbHelp.QueryOne<LotteryDrawPool>(sql, new { @Id = id });
        }

        public Award GetWxCardByCardId(string cardId)
        {
            string sql = @"
select a.* from LotteryDrawPool ldp
inner join Award a on ldp.awardid=a.id
where ldp.CardId=@CardId
";
            return DbHelp.QueryOne<Award>(sql, new { @CardId = cardId });
        }
    }
}
