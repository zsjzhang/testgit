using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.IRepository;
using Vcyber.BLMS.Application;

namespace Vcyber.BLMS.Domain
{
    /// <summary>
    /// 会员登陆记录业务
    /// </summary>
    public class LoginMemRecordApp : ILoginMemRecordApp
    {
        #region ==== 构造函数 ====

        public LoginMemRecordApp() { }

        #endregion

        #region ==== 公共方法 ====

        /// <summary>
        /// 添加会员登陆记录
        /// </summary>
        /// <param name="memberId">会员id</param>
        /// <param name="memberName">会员呢称</param>
        /// <param name="dataSource">登陆来源</param>
        public void Add(string memberId, string memberName, EDataSource dataSource)
        {
            Task.Factory.StartNew(() =>
            {
                _DbSession.LoginMemRecordStorager.Add(new LoginMemberRecord() { MemberId = memberId, MemberName = memberName, DataSource = dataSource.ToString(), CreateTime = DateTime.Now });
            }); 
        }

        /// 添加管理员登陆记录
        /// </summary>
        /// <param name="data"></param>
        public void ManagerAdd(string managerId,string managerName)
        {
            _DbSession.LoginMemRecordStorager.ManagerAdd(new LoginManagerRecord() {  ManagerId=managerId, ManagerName=managerName,CreateTime=DateTime.Now, Remark=""});
        }

        /// <summary>
        /// 查询最后登录时间
        /// </summary>
        /// <param name="MemberId"></param>
        /// <returns></returns>
        public string GetNewLoginTime(string MemberId)
        {
           return _DbSession.LoginMemRecordStorager.GetNewLoginTime(MemberId);
        }


        public bool IsReMemberShipRequest(string memberId)
        {
            return _DbSession.LoginMemRecordStorager.IsReMemberShipRequest(memberId);
        }
        #endregion
    }
}
