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
    public class XDLotteryRecordStorager : IXDLotteryRecordStorager
    {
        public XDLotteryRecordStorager() { }
        public int AddXDLotteryRecord(XDLotteryRecord xDLotteryRecord)
        {
            string sql = "INSERT INTO [XD_LotteryRecord]([LotteryId],[ActivityId],[ActivityName],[LotteryName],[UserId],[UserName],[LotteryRecordTime],[UserIp],[IsValid],[CreaterId],[CreaterName],[CreaterTime],[UpdaterId],[UpdaterName],[UpdaterTime],[LotteryType],[LotteryRecordSource],[UserMobile])VALUES(@LotteryId,@ActivityId,@ActivityName,@LotteryName,@UserId,@UserName,@LotteryRecordTime,@UserIp,@IsValid,@CreaterId,@CreaterName,@CreaterTime,@UpdaterId,@UpdaterName,@UpdaterTime,@LotteryType,@LotteryRecordSource,@UserMobile);SELECT @@IDENTITY;";
            return DbHelp.ExecuteScalar<int>(sql, xDLotteryRecord);
        }

        public IEnumerable<XDLotteryRecord> GetXDLotteryRecordList(int activityId, int lotteryType, int lotteryRecordSource)
        {
            string sql = string.Empty;
            if (lotteryType == 0)
            {
                sql = @"SELECT * FROM XD_LotteryRecord WHERE ActivityId=@ActivityId ORDER BY CreaterTime DESC";
            }
            else if (lotteryRecordSource == 0)
            {
                sql = @"SELECT * FROM XD_LotteryRecord 
                            WHERE ActivityId=@ActivityId and LotteryType=@LotteryType ORDER BY CreaterTime DESC";
            }
            else
            {
                sql = @"SELECT * FROM XD_LotteryRecord 
                            WHERE ActivityId=@ActivityId and LotteryType=@LotteryType and LotteryRecordSource=@LotteryRecordSource ORDER BY CreaterTime DESC";
            }
            return DbHelp.Query<XDLotteryRecord>(sql, new { ActivityId = activityId, LotteryType = lotteryType, LotteryRecordSource=lotteryRecordSource });
        }


        public bool IsExistLotteryRecordByUserId(string userId, int activityId, int lotteryType)
        {
            string sql = string.Empty;
            if (lotteryType == 0)
            {
                sql = "SELECT COUNT(1) FROM XD_LotteryRecord WHERE UserId=@UserId AND ActivityId=@ActivityId AND IsValid=1";
            }
            else
            {
                sql = "SELECT COUNT(1) FROM XD_LotteryRecord WHERE UserId=@UserId AND ActivityId=@ActivityId AND IsValid=1 and LotteryType=@LotteryType";
            }
            return DbHelp.ExecuteScalar<int>(sql, new { UserId = userId, ActivityId = activityId, LotteryType =lotteryType})>0;
        }

        public bool IsExistLotteryRecordByUserMobile(string mobile, int activityId, int lotteryType)
        {
            string sql = string.Empty;
            if (lotteryType == 0)
            {
                sql = "SELECT COUNT(1) FROM XD_LotteryRecord WHERE UserMobile=@UserMobile AND ActivityId=@ActivityId AND IsValid=1";
            }
            else
            {
                sql = "SELECT COUNT(1) FROM XD_LotteryRecord WHERE UserMobile=@UserMobile AND ActivityId=@ActivityId AND IsValid=1 and LotteryType=@LotteryType";
            }
            return DbHelp.ExecuteScalar<int>(sql, new { UserMobile = mobile, ActivityId = activityId, LotteryType = lotteryType }) > 0;
        }
    }
}
