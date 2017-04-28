using System;
using System.Collections.Generic;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.IRepository;

namespace Vcyber.BLMS.Repository
{
    public class BluebeanWinRecordStorager : IBluebeanWinRecordStorager
    {
        public string QueryWinRecord(string phone)
        {
            var sql = @"select  Prize from [BluebeanWinRecord] where Phone=@phone";
            return DbHelp.ExecuteScalar<string>(sql, new { phone });
        }

        public IEnumerable<BluebeanWinRecord> QueryWinRecords(int quantity)
        {
            var sql = string.Format(@"select top {0} [Phone],[Prize] from [BluebeanWinRecord]", quantity);
            return DbHelp.Query<BluebeanWinRecord>(sql);
        }


        public int GetCurrentWinPrizeCount()
        {
            var hour = DateTime.Now.Hour;
            var shortDate = DateTime.Now.ToShortDateString();
            var begin = string.Format("{0} {1}:00:00", shortDate, hour);
            var end = string.Format("{0} {1}:00:00", shortDate, hour + 1);
            var sql =
                string.Format(
                    "select count(1) from [BluebeanActiveRecord] where CreateTime between '{0}' and '{1}' and IsSelected=1",
                    begin, end);
            return DbHelp.ExecuteScalar<int>(sql);
        }


        public IEnumerable<BlueMemberPrize> GetPrizeByType(string prizeType)
        {
            string sql = "select * from  [BlueMemberPrize] where [IsDeleted]=0 and [PrizeType]=@PrizeType";

            return DbHelp.Query<BlueMemberPrize>(sql, new
            {
                PrizeType = prizeType
            });
        }

        public bool IsSelected(string userId)
        {
            string sql = @"select 1 from  [BluebeanActiveRecord] where [UserId]=@UserId and [IsSelected]=1";
            return DbHelp.ExecuteScalar<int>(sql, new { UserId = userId }) > 0;
        }


        public void Insert(BluebeanActiveRecord bluebeanActiveRecord)
        {
            string sql =
                "insert into [BluebeanActiveRecord] (UserId,IsSelected,PrizeName)values(@UserId,@IsSelected,@PrizeName)";
            DbHelp.Execute(sql, bluebeanActiveRecord);
        }


        public int InsertWinRecords(int prizeId, BluebeanWinRecord bluebeanWinRecord)
        {
            string sql =
               string.Format(" update BlueMemberPrize set LeftNum=LeftNum-1 where Id={0};  insert into [BluebeanWinRecord](UserId,Prize)values(@UserId,@Prize) ; select SCOPE_IDENTITY() ;", prizeId);
            return DbHelp.ExecuteScalar<int>(sql, bluebeanWinRecord);
        }

        public BluebeanWinRecord QueryWinRecordByUserId(string userId)
        {
            var sql = @"select  *  from [BluebeanWinRecord] where UserId=@UserId";
            return DbHelp.QueryOne<BluebeanWinRecord>(sql, new { UserId = userId });
        }

        public bool UpdateAddress(BluebeanWinRecord bluebeanWinRecord)
        {
            string sql =
                @"update BluebeanWinRecord set Province=@Province,City=@City,Area=@Area,[Address]=@Address,Contacts=@Contacts,UpdateTime=GETDATE(),Phone=@Phone where Id=@Id";
            return DbHelp.Execute(sql, bluebeanWinRecord) > 0;

        }
    }
}