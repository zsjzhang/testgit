using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Vcyber.BLMS.Common;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.Entity.SelectCondition;
using Vcyber.BLMS.IRepository;

namespace Vcyber.BLMS.Repository
{
    /// <summary>
    /// 会员登陆操作
    /// </summary>
    public class LoginMemRecordStorager : ILoginMemRecordStorager
    {
        #region ==== 构造函数 ====

        public LoginMemRecordStorager() { }

        #endregion

        #region ==== 公共方法 ====

        /// <summary>
        /// 添加会员记录
        /// </summary>
        /// <param name="data"></param>
        public void Add(LoginMemberRecord data)
        {
            data.Id = Guid.NewGuid().ToString();

            StringBuilder sql = new StringBuilder();
            sql.Append(" insert into LoginMemberRecord(Id,MemberId,MemberName,DataSource,CreateTime,Remark) ");
            sql.Append(" values(@Id,@MemberId,@MemberName,@DataSource,@CreateTime,@Remark)");
            DbHelp.Execute(sql.ToString(), data);
        }

        /// <summary>
        /// 添加管理员登陆记录
        /// </summary>
        /// <param name="data"></param>
        public void ManagerAdd(LoginManagerRecord data)
        {
            data.Id = Guid.NewGuid().ToString();

            StringBuilder sql = new StringBuilder();
            sql.Append(" insert into LoginManagerRecord(Id,ManagerId,ManagerName,CreateTime,Remark)");
            sql.Append(" values(@Id,@ManagerId,@ManagerName,@CreateTime,@Remark)");
            DbHelp.Execute(sql.ToString(), data);
        }

      
      /// <summary>
      /// 查询最后登录时间
      /// </summary>
      /// <param name="MemberId"></param>
      /// <returns></returns>
        public string GetNewLoginTime(string MemberId)
        {
            return DbHelp.ExecuteScalar<string>("select top 1 CreateTime from LoginMemberRecord  where MemberId=@MemberId order by CreateTime desc", new
            {
                @MemberId = MemberId
            });
        }

        public bool IsReMemberShipRequest(string memberId)
        {

          var userid=  DbHelp.ExecuteScalar<string>("select   userid from xiaowei where userid =@memberId", new { @memberId=memberId });

          if (string.IsNullOrEmpty(userid))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        #endregion
    }
}
