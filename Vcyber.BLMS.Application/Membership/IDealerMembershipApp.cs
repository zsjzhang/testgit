using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.Application
{
    public interface IDealerMembershipApp
    {
        bool CreateMembership(UserModel user);
        bool IsPersonalUser(string idNumber);
        IEnumerable<Membership> SelectMemberList(string phoneNumber, string identityNumber, string vin, string dealerId, string startTime, string endTime, string userType,string CarCategory,string PaperWork, int pageIndex, int pageSize, out int totalCount);

        IEnumerable<MemberSonata> SelectMemberListNoJoin(string custName, string phoneNumber, string identityNumber, string vin, string startTime, string endTime, string dealerId, int pageIndex, int pageSize, out int totalCount);

        IEnumerable<MemberSonata> SelectMemberListAll(string status, string custName, string phoneNumber, string identityNumber, string vin, string startTime, string endTime, string dealerId, string selectCanJoin, string userType,string PaperWork, int pageIndex, int pageSize, out int totalCount, int IsPay, decimal Amount);

        /// <summary>
        /// 定级（临时）
        /// </summary>
        /// <param name="pid">身份证号</param>
        /// <returns>级别</returns>
        string GetLevel(string pid);

        /// <summary>
        /// 根据身份证号返回会员级别
        /// </summary>
        /// <param name="idNumber">身份证号</param>
        /// <returns></returns>
        string GetFirstRegistMLevelByIdNumber(string idNumber);
        /// <summary>
        /// 根据身份证号判断是否D+S
        /// </summary>
        /// <param name="idNumber"></param>
        /// <returns></returns>
        bool IsDsCarTypeByIdNumber(string idNumber);

        /// <summary>
        /// 根据手机号码获取会员信息
        /// </summary>
        /// <param name="phoneNumber">手机号码</param>
        /// <returns></returns>
        IEnumerable<Membership> SelectMemberListByphoneNumber(string phoneNumber);



        /// <summary>
        /// 根据用户ID获取用户信息
        /// </summary>
        /// <param name="userId">用户iD</param>
        /// <returns></returns>
        Membership GetMembershipByUserId(string userId);



        /// <summary>
        /// 根据证件号码获取会员信息
        /// </summary>
        /// <param name="idNumber">证件号码</param>
        /// <returns></returns>
        IEnumerable<Membership> GetMemberListByIdentityNumber(string idNumber);

        /// <summary>
        /// 根据userId获取用户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Membership GetMembershipMsgByUserId(string userId);
    }
}
