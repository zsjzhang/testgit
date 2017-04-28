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
    public class UserMessageRecordStorager : IUserMessageRecordStorager
    {
        
        public int Insert(BLMS.Entity.UserMessageRecord userMessageRecord)
        {
            string sql = @"
                INSERT INTO dbo.UserMessageRecord
                (UserId
                ,MsgContent
                ,MsgType
                )
                VALUES
                (@UserId
                ,@MsgContent
                ,@MsgType
                )";
            return DbHelp.Execute(sql,
               userMessageRecord);
        }

        public int UpdateState(int id)
        {
            string sql = @"update UserMessageRecord set MsgState=1  where Id=@Id";
            return DbHelp.Execute(sql, new
            {
                Id = id
            });
        }

        public int GetUnReadMessage(string userId)
        {
            string sql = @"select count(1) from UserMessageRecord where MsgState=0 and UserId=@UserId and IsDeleted=0";
            return DbHelp.ExecuteScalar<int>(sql, new { UserId = userId });
        }

        /// <summary>
        /// 更具userid和msgtype获取消息列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="msgtype"></param>
        /// <returns></returns>
        public IEnumerable<UserMessageRecord> GetMsgList(string userId, int msgtype)
        {
            string sql = @"select Id,UserId,MsgContent,MsgType,MsgState,CreateTime,UpdateTime from UserMessageRecord where UserId=@UserId and IsDeleted=0 and MsgType=@msgtype order by MsgState desc,CreateTime desc";
            return DbHelp.Query<UserMessageRecord>(sql, new { @UserId = userId, @msgtype = msgtype });
        }

        public IEnumerable<UnReadMsgStatistics> StatisticsUnReadMsgType(string userId)
        {
            string sql =
                "  select MsgType as MessageType ,count(1) as MsgCount from  UserMessageRecord where MsgState=0 and IsDeleted=0 and UserId=@UserId group by MsgType ";
            return DbHelp.Query<UnReadMsgStatistics>(sql, new { UserId = userId });
        }

        public IEnumerable<UserMessageRecord> GetUserMessageRecords(int msgType, string userId, int pageIndex, int pageSize, out int totalCount)
        {
            StringBuilder sb=new StringBuilder();
            sb.AppendFormat("select count(1) from UserMessageRecord where IsDeleted=0 and UserId=@userId {0};",
                msgType > 0 ? "and MsgType=@MsgType" : "");
            sb.AppendFormat(@"
  select * from (
  select MsgContent,CreateTime, ROW_NUMBER() over(order by Id desc) as RowNumber from UserMessageRecord where IsDeleted=0 and UserId=@userId {2})as RowNumber_Table where RowNumber between {0}  and {1};", (pageIndex - 1) * pageSize + 1, pageSize * pageIndex, msgType > 0 ? "and MsgType=@MsgType" : "");

            return DbHelp.QueryMultiple<UserMessageRecord>(sb.ToString(),out totalCount, new { UserId = userId, MsgType = msgType });


        }
        public  void UpdateState(string userId, int msgType)
        {
            string sql = @"update UserMessageRecord set MsgState=1  where UserId=@userId and MsgType=@MsgType";
            DbHelp.Execute(sql, new { UserId = userId, MsgType = msgType });
        }

        public IEnumerable<PaymentAccessPoint> GetPaymentAccessPoint(string dealerid, string starttime, string endtime)
        {
            string sql =
                @"select * from (select row_number() over (order by ms.id) as row_num,ms.id as MembershipId, msr.Status,msr.createtime as createtime,ms.RealName as name,
                   cust.CustName as RealName, case when ms.IsPay = 1 then '已缴费' when ms.IsPay = 2  then '审核中' else '未缴费' end IsPay,
                   ms.PayNumber, ms.PhoneNumber as Tel, ms.IdentityNumber, msr.DealerId,msr.id as ApprovalId, msr.createtime as SubmitTime, msr.status as ApprStatus,
                    msr.memo,ms.City,ms.Address,ms.Amount,kz.PaperWork FROM membership ms  inner join MembershipRequest msr on ms.id = msr.membershipid
                    left join IF_Customer cust on cust.IdentityNumber = ms.IdentityNumber and cust.IdentityNumber is not null and cust.IdentityNumber <> ''
                    left join Membership_Schedule kz on ms.id = kz.membershipid
                    where 1 = 1 and msr.Status = 1 and msr.DealerId = @DealerId {0} {1}) u where 1 = 1 order by u.SubmitTime desc";
            var whereExp = new StringBuilder();
            var whereExp1 = new StringBuilder();
            if (!string.IsNullOrEmpty(starttime))
            {
                whereExp.Append(" and msr.createtime >= @Starttime ");
            }
            if (!string.IsNullOrEmpty(endtime))
            {
                whereExp1.Append(" and msr.createtime <= @Endtime ");
            }
            sql = string.Format(sql, whereExp.ToString(), whereExp1.ToString());
            return DbHelp.Query<PaymentAccessPoint>(sql, new { @DealerId = dealerid, @Starttime = starttime,@Endtime = endtime});
        }



        /// <summary>
        /// 获取个人中心未读消息数目
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        //public NoReadMsgModel NoReadList(string userId)
        //{
        //    NoReadMsgModel model = new NoReadMsgModel();

        //    //获取未读系统消息数
        //    string sql1 = @"select count(id) from UserMessageRecord where UserId=@UserId and IsDeleted=0 and MsgType=1 and MsgState=0";
        //    model.SysMsgCount = DbHelp.Execute(sql1, new { @UserId = userId });

        //    //获取未读积分变动消息数
        //    string sql2 = @"select count(id) from UserMessageRecord where UserId=@UserId and IsDeleted=0 and MsgType=3 and MsgState=0";
        //    model.IntegralchangeMsgCount = DbHelp.Execute(sql2, new { @UserId = userId });

        //    //获取未读卡券消息数
        //    string sql3 = @"select count(id) from UserMessageRecord where UserId=@UserId and IsDeleted=0 and MsgType=4 and MsgState=0";
        //    model.CardcouponMsgCount = DbHelp.Execute(sql3, new { @UserId = userId });

        //    //获取未读服务和活动消息数
        //    string sql4 = @"select count(id) from UserMessageRecord where UserId=@UserId and IsDeleted=0 and MsgType=5 and MsgState=0";
        //    model.ActAndServerCount = DbHelp.Execute(sql4, new { @UserId = userId });

        //    return model;
        //}

        public IEnumerable<UserMessageRecord> GetNoReadMsg(int msgType, string userId)
        {
            string sql1 = @"select * from UserMessageRecord where UserId=@UserId and IsDeleted=0 and MsgType=@msgType and MsgState=0";
            return DbHelp.Query<UserMessageRecord>(sql1, new { @msgType = msgType, @UserId = userId });
        }
    }
}
