using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspNet.Identity.SQL;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.IRepository;

namespace Vcyber.BLMS.Domain
{
    public class DealerMembershipApp : IDealerMembershipApp
    {
        public IEnumerable<Membership> SelectMemberList(string phoneNumber, string identityNumber, string vin, string dealerId, string startTime, string endTime, string userType, string CarCategory, string PaperWork, int pageIndex, int pageSize, out int totalCount)
        {
            return _DbSession.DealerMembershipStorager.SelectMemberList(phoneNumber, identityNumber, vin, dealerId, startTime, endTime, userType, CarCategory, PaperWork, pageIndex, pageSize, out totalCount);
        }

        public IEnumerable<MemberSonata> SelectMemberListNoJoin(string custName, string phoneNumber, string identityNumber, string vin, string dealerId, string startTime, string endTime, int pageIndex, int pageSize, out int totalCount)
        {
            return _DbSession.DealerMembershipStorager.SelectMemberListNoJoin(custName, phoneNumber, identityNumber, vin, dealerId, startTime, endTime, pageIndex, pageSize, out totalCount);
        }

        public IEnumerable<MemberSonata> SelectMemberListAll(string status, string custName, string phoneNumber, string identityNumber, string vin, string startTime, string endTime, string dealerId, string selectCanJoin, string userType, string PaperWork, int pageIndex, int pageSize, out int totalCount, int IsPay, decimal Amount)
        {
            return _DbSession.DealerMembershipStorager.SelectMemberListAll(status, custName, phoneNumber, identityNumber, vin, startTime, endTime, dealerId, selectCanJoin, userType, PaperWork, pageIndex, pageSize, out totalCount, IsPay, Amount);
        }

        public bool IsPersonalUser(string idNumber)
        {
            return _DbSession.DealerMembershipStorager.IsPersonalUser(idNumber);
        }

        /// <summary>
        /// 定级（临时）
        /// </summary>
        /// <param name="pid">身份证号</param>
        /// <returns>级别</returns>
        public string GetLevel(string pid)
        {
            if (string.IsNullOrEmpty(pid))
                throw new ArgumentNullException("idNumber身份证号不能为空");
            return _DbSession.DealerMembershipStorager.GetLevel(pid);            
        }

        public string GetFirstRegistMLevelByIdNumber(string idNumber)
        {
            if (string.IsNullOrEmpty(idNumber))
                throw new ArgumentNullException("idNumber身份证号不能为空");
            return _DbSession.DealerMembershipStorager.GetFirstRegistMLevelByIdNumber(idNumber);
        }

        public bool AddMembershipDealerRecord(string membershipId, string dealerId)
        {
           return  _DbSession.DealerMembershipStorager.AddMembershipDealerRecord(membershipId,dealerId);
        }


        public bool CreateMembership(UserModel user)
        {
            bool flag = false;
            string id = "";


            flag = _DbSession.DealerMembershipStorager.InsertUser(user) > 0;
            if (!string.IsNullOrEmpty(user.PaperWork))
            {
                _DbSession.DealerMembershipStorager.AddPaperworkToMembership_Schedule(user);
            }
            //新会员注册时为未通过验证状态，需要通过中间表来验证是否为真实车主
            //if (userTable.CheckMembershipValid(user))
            //{
            //    const string status = "1";
            //    userTable.UpdateMembershipStatus(user, status);
            //}
            return flag;

        }
        public bool IsDsCarTypeByIdNumber(string idNumber)
        {
            return _DbSession.DealerMembershipStorager.IsDsCarTypeByIdNumber(idNumber);

        }

        /// <summary>
        /// 根据手机号码获取会员信息
        /// </summary>
        /// <param name="phoneNumber">手机号码</param>
        /// <returns></returns>
        public IEnumerable<Membership> SelectMemberListByphoneNumber(string phoneNumber)
        {
            return _DbSession.DealerMembershipStorager.SelectMemberListByphoneNumber(phoneNumber);
        }


        public Membership GetMembershipByUserId(string userId)
        {
            return _DbSession.DealerMembershipStorager.GetMembershipByUserId(userId);
        }


        public IEnumerable<Membership> GetMemberListByIdentityNumber(string idNumber)
        {
            return _DbSession.DealerMembershipStorager.GetMemberListByIdentityNumber(idNumber);
        }

        /// <summary>
        /// 根据userId获取用户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Membership GetMembershipMsgByUserId(string userId)
        {
            return _DbSession.DealerMembershipStorager.GetMembershipMsgByUserId(userId);
        }
    }
}
