using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.IRepository
{
    /// <summary>
    /// 会员登陆操作
    /// </summary>
    public interface ILoginMemRecordStorager
    {
        /// <summary>
        /// 添加会员记录
        /// </summary>
        /// <param name="data"></param>
        void Add(LoginMemberRecord data);

        /// 添加管理员登陆记录
        /// </summary>
        /// <param name="data"></param>
        void ManagerAdd(LoginManagerRecord data);

        /// <summary>
        /// 查询最后登录时间
        /// </summary>
        /// <param name="MemberId"></param>
        /// <returns></returns>
        string GetNewLoginTime(string MemberId);

        bool IsReMemberShipRequest(string memberId);
    }
}
