using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity.Enum;

namespace Vcyber.BLMS.Application
{
    /// <summary>
    /// 会员登陆记录业务
    /// </summary>
    public interface ILoginMemRecordApp
    {
        /// <summary>
        /// 添加会员登陆记录
        /// </summary>
        /// <param name="memberId">会员id</param>
        /// <param name="memberName">会员呢称</param>
        /// <param name="dataSource">登陆来源</param>
        void Add(string memberId, string memberName, EDataSource dataSource);

        /// 添加管理员登陆记录
        /// </summary>
        /// <param name="data"></param>
        void ManagerAdd(string managerId, string managerName);


        /// <summary>
        /// 查询最后登录时间
        /// </summary>
        /// <param name="MemberId"></param>
        /// <returns></returns>
        string GetNewLoginTime(string MemberId);

        bool IsReMemberShipRequest(string memberId);
    }
}
