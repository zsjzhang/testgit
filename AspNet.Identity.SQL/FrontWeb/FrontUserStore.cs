using AspNet.Identity.SQL.FrontWeb;
using AspNet.Identity.SQL.FrontWeb.QueryEntity;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Entity.Enum;

namespace AspNet.Identity.SQL
{
    /// <summary>
    /// Class that implements the key ASP.NET Identity user store iterfaces
    /// </summary>
    public class FrontUserStore<TUser> :
        IUserPasswordStore<TUser>,
        IUserSecurityStampStore<TUser>,
        IQueryableUserStore<TUser>,
        IUserEmailStore<TUser>,
        IUserPhoneNumberStore<TUser>,
        IUserTwoFactorStore<TUser, string>,
        IUserLockoutStore<TUser, string>,
        IUserStore<TUser>, IUserLoginStore<TUser>
        where TUser : FrontIdentityUser
    {
        private FrontUserTable<TUser> userTable;

        public SQLDatabase Database { get; private set; }

        public IQueryable<TUser> Users
        {
            get
            {
                throw new NotImplementedException();
            }
        }


        /// <summary>
        /// Default constructor that initializes a new SQLDatabase
        /// instance using the Default Connection string
        /// </summary>
        public FrontUserStore()
            : this(new SQLDatabase())
        {
        }

        /// <summary>
        /// Constructor that takes a SQLDatabase as argument 
        /// </summary>
        /// <param name="database"></param>
        public FrontUserStore(SQLDatabase database)
        {
            Database = database;
            userTable = new FrontUserTable<TUser>(database);

        }

        /// <summary>
        /// Insert a new TUser in the UserTable
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task CreateAsync(TUser user)
        {
            string id = "";
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            if (!string.IsNullOrEmpty(user.IdentityNumber) && IsIdentityNumberRepeate(user.IdentityNumber))
                throw new Exception("您输入的证件号码已被注册");
            if (CheckNickNameIsExist(user.NickName))
                throw new Exception("用户名已存在");
            if (CheckUserNameIsExist(user.UserName))
                throw new Exception("手机号已存在");

            userTable.Insert(user);
            if (!string.IsNullOrEmpty(user.PaperWork))
            {
                userTable.AddPaperworkToMembership_Schedule(user);
            }
            //新会员注册时为未通过验证状态，需要通过中间表来验证是否为真实车主
            //if (userTable.CheckMembershipValid(user))
            //{
            //    const string status = "1";
            //    userTable.UpdateMembershipStatus(user, status);
            //}
            return Task.FromResult<object>(null);
        }


        public bool CreateMembership(TUser user)
        {
            bool flag = false;
            string id = "";
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            if (!string.IsNullOrEmpty(user.IdentityNumber) && IsIdentityNumberRepeate(user.IdentityNumber))
                throw new Exception("身份证号码已存在");
            if (CheckNickNameIsExist(user.NickName))
                throw new Exception("用户名已存在");
            if (CheckUserNameIsExist(user.UserName))
                throw new Exception("手机号已存在");

            flag=userTable.Insert(user)>0;
            if (!string.IsNullOrEmpty(user.PaperWork))
            {
                userTable.AddPaperworkToMembership_Schedule(user);
            }
            //新会员注册时为未通过验证状态，需要通过中间表来验证是否为真实车主
            //if (userTable.CheckMembershipValid(user))
            //{
            //    const string status = "1";
            //    userTable.UpdateMembershipStatus(user, status);
            //}
            return flag;

        }

        public bool IsIdentityNumberRepeate(string identityNumber,string id)
        {
            if (string.IsNullOrEmpty(identityNumber))
                return false;
            return userTable.IsIdentityNumberRepeate(identityNumber,id);
        }
        public bool IsIdentityNumberId(string identityNumber,string Id)
        {
            if (string.IsNullOrEmpty(identityNumber) && string.IsNullOrEmpty(Id))
                return false;
            return userTable.IsIdentityNumberId(identityNumber,Id);
        }
        public void AddPaperworkToMembership_Schedule(TUser user)
        {
            userTable.AddPaperworkToMembership_Schedule(user);
        }

        public bool IsIdentityNumberRepeate(string identityNumber)
        {
            if (string.IsNullOrEmpty(identityNumber))
                return false;
            return userTable.IsIdentityNumberRepeate(identityNumber);
        }
        //判断用户名称是否存在
        public bool CheckNickNameIsExists(string NickName)
        {
            if (string.IsNullOrEmpty(NickName))
                return false;
            return userTable.CheckNickNameIsExist(NickName);
        }

        //判断手机号是否存在
        public bool CheckPhoneNumberIsExist(string PhoneNumber, string id)
        {
            if (string.IsNullOrEmpty(PhoneNumber))
                return false;
            return userTable.CheckPhoneNumberIsExist(PhoneNumber, id);
        }
        public bool CheckPhoneNumberIsExist(string PhoneNumber)
        {
            if (string.IsNullOrEmpty(PhoneNumber))
                return false;
            return userTable.CheckPhoneNumberIsExist(PhoneNumber);
        }

        //判断邮箱是否存在
        public bool CheckEmailIsExist(string email,string id)
        {
            if (string.IsNullOrEmpty(email))
                return false;
            return userTable.CheckEmailIsExist(email,id);
        }
        //判断邮箱是否存在
        public bool CheckEmailIsExist(string email)
        {
            if (string.IsNullOrEmpty(email))
                return false;
            return userTable.CheckEmailIsExist(email);
        }

        public bool IsLoginExist(string nickName, string Password)
        {
            return userTable.CheckLoginIsExist(nickName, Password);
        }
        /// <summary>
        /// Returns an TUser instance based on a userId query 
        /// </summary>
        /// <param name="userId">The user's Id</param>
        /// <returns></returns>
        public Task<TUser> FindByIdAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentException("Null or empty argument: userId");
            }

            TUser result = userTable.GetUserById(userId) as TUser;
            if (result != null)
            {
                return Task.FromResult<TUser>(result);
            }

            return Task.FromResult<TUser>(null);
        }

        /// <summary>
        /// Returns an TUser instance based on a identitynumber query 
        /// </summary>
        /// <param name="userId">The user's Id</param>
        /// <returns></returns>
        public Task<TUser> FindByIdentityNumber(string identityNumber)
        {
            if (string.IsNullOrEmpty(identityNumber))
            {
                throw new ArgumentException("Null or empty argument: identityNumber");
            }

            TUser result = userTable.GetUserByIdentityNunmber(identityNumber) as TUser;
            if (result != null)
            {
                return Task.FromResult<TUser>(result);
            }

            return Task.FromResult<TUser>(null);
        }
        //查找用户积分状态
        public Task<TUser> FindByIdUserintegral(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentException("Null or empty argument: userId");
            }

            TUser result = userTable.FindByIdUserintegral(userId) as TUser;
            if (result != null)
            {
                return Task.FromResult<TUser>(result);
            }

            return Task.FromResult<TUser>(null);
        }


        /// <summary>
        /// Returns an TUser instance based on a userName query 
        /// </summary>
        /// <param name="userName">The user's name</param>
        /// <returns></returns>
        public Task<TUser> FindByNameAsync(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                throw new ArgumentException("用户名为空");
            }

            List<TUser> result = userTable.GetUserByName(userName) as List<TUser>;

            // Should I throw if > 1 user?
            if (result != null && result.Count == 1)
            {
                return Task.FromResult<TUser>(result[0]);
            }

            return Task.FromResult<TUser>(null);
        }

        public TUser FindByName(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                throw new ArgumentException("用户名为空");
            }

            List<TUser> result = userTable.GetUserByName(userName) as List<TUser>;

            // Should I throw if > 1 user?
            if (result != null && result.Count == 1)
            {
                return result[0];
            }

            return null;
        }


        public Task<TUser> FindByNickNameAsync(string nickName)
        {
            if (string.IsNullOrEmpty(nickName))
            {
                throw new ArgumentException("用户名为空");
            }
            List<TUser> result = userTable.GetUserByNickName(nickName) as List<TUser>;
            if (result != null && result.Count == 1)
            {
                return Task.FromResult<TUser>(result[0]);
            }
            return Task.FromResult<TUser>(null);
        }

        //检查用户名密码是否存在,返回用户id
        public Task<TUser> GetUserByLogin(string nickName, string Password)
        {
            if (string.IsNullOrEmpty(nickName))
            {
                throw new ArgumentException("用户名为空");
            }
            List<TUser> result = userTable.GetUserByLogin(nickName, Password) as List<TUser>;
            if (result != null && result.Count == 1)
            {
                return Task.FromResult<TUser>(result[0]);
            }
            return Task.FromResult<TUser>(null);
        }

        /// <summary>
        /// Updates the UsersTable with the TUser instance values
        /// </summary>
        /// <param name="user">TUser to be updated</param>
        /// <returns></returns>
        public Task UpdateAsync(TUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            userTable.Update(user);

            return Task.FromResult<object>(null);
        }
        //冻结积分
        public Task UpdateUserintegral(TUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            userTable.UpdateUserintegral(user);

            return Task.FromResult<object>(null);
        }

        public Task UpdateAsync2(TUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            userTable.Update2(user);

            return Task.FromResult<object>(null);
        }

        public void Dispose()
        {
            if (Database != null)
            {
                Database.Dispose();
                Database = null;
            }
        }

        /// <summary>
        /// Deletes a user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task DeleteAsync(TUser user)
        {
            if (user != null)
            {
                userTable.Delete(user);
            }

            return Task.FromResult<Object>(null);
        }

        /// <summary>
        /// Returns the PasswordHash for a given TUser
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<string> GetPasswordHashAsync(TUser user)
        {
            string passwordHash = userTable.GetPasswordHash(user.Id);

            return Task.FromResult<string>(passwordHash);
        }

        /// <summary>
        /// Verifies if user has password
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<bool> HasPasswordAsync(TUser user)
        {
            var hasPassword = !string.IsNullOrEmpty(userTable.GetPasswordHash(user.Id));

            return Task.FromResult<bool>(Boolean.Parse(hasPassword.ToString()));
        }

        /// <summary>
        /// Sets the password hash for a given TUser
        /// </summary>
        /// <param name="user"></param>
        /// <param name="passwordHash"></param>
        /// <returns></returns>
        public Task SetPasswordHashAsync(TUser user, string passwordHash)
        {
            user.PasswordHash = passwordHash;

            return Task.FromResult<Object>(null);
        }

        /// <summary>
        ///  Set security stamp
        /// </summary>
        /// <param name="user"></param>
        /// <param name="stamp"></param>
        /// <returns></returns>
        public Task SetSecurityStampAsync(TUser user, string stamp)
        {
            user.SecurityStamp = stamp;

            return Task.FromResult(0);

        }

        /// <summary>
        /// Get security stamp
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<string> GetSecurityStampAsync(TUser user)
        {
            return Task.FromResult(user.SecurityStamp);
        }

        /// <summary>
        /// Set email on user
        /// </summary>
        /// <param name="user"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public Task SetEmailAsync(TUser user, string email)
        {
            user.Email = email;
            userTable.Update(user);

            return Task.FromResult(0);

        }

        

        /// <summary>
        /// Get email from user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<string> GetEmailAsync(TUser user)
        {
            return Task.FromResult(user.Email);
        }

        /// <summary>
        /// Get if user email is confirmed
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<bool> GetEmailConfirmedAsync(TUser user)
        {
            return Task.FromResult(user.EmailConfirmed);
        }

        /// <summary>
        /// Set when user email is confirmed
        /// </summary>
        /// <param name="user"></param>
        /// <param name="confirmed"></param>
        /// <returns></returns>
        public Task SetEmailConfirmedAsync(TUser user, bool confirmed)
        {
            user.EmailConfirmed = confirmed;
            userTable.Update(user);

            return Task.FromResult(0);
        }

        /// <summary>
        /// Get user by email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public Task<TUser> FindByEmailAsync(string email)
        {
            if (String.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException("邮箱为空");
            }

            List<TUser> result = userTable.GetUserByEmail(email) as List<TUser>;
            if (result != null && result.Count() > 0)
            {
                return Task.FromResult<TUser>(result[0]);
            }

            return Task.FromResult<TUser>(null);
        }

        public List<Dictionary<string,string>> FindLevel(string vin)
        {
            if (string.IsNullOrEmpty(vin))
                return null;

            return userTable.FindLevel(vin);
        }

        /// <summary>
        /// 查询匹配的登陆人信息
        /// </summary>
        /// <param name="value">登陆值</param>
        /// <returns></returns>
        public List<TUser> FindLogin(string value)
        {
            return userTable.FindLogin(value);
        }

        /// <summary>
        /// Set user phone number
        /// </summary>
        /// <param name="user"></param>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        public Task SetPhoneNumberAsync(TUser user, string phoneNumber)
        {
            user.PhoneNumber = phoneNumber;
            userTable.Update(user);

            return Task.FromResult(0);
        }

        /// <summary>
        /// Get user phone number
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<string> GetPhoneNumberAsync(TUser user)
        {
            return Task.FromResult(user.PhoneNumber);
        }

        /// <summary>
        /// Get if user phone number is confirmed
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<bool> GetPhoneNumberConfirmedAsync(TUser user)
        {
            return Task.FromResult(user.PhoneNumberConfirmed);
        }

        /// <summary>
        /// Set phone number if confirmed
        /// </summary>
        /// <param name="user"></param>
        /// <param name="confirmed"></param>
        /// <returns></returns>
        public Task SetPhoneNumberConfirmedAsync(TUser user, bool confirmed)
        {
            user.PhoneNumberConfirmed = confirmed;
            userTable.Update(user);

            return Task.FromResult(0);
        }

        /// <summary>
        /// Set two factor authentication is enabled on the user
        /// </summary>
        /// <param name="user"></param>
        /// <param name="enabled"></param>
        /// <returns></returns>
        public Task SetTwoFactorEnabledAsync(TUser user, bool enabled)
        {
            user.TwoFactorEnabled = enabled;
            userTable.Update(user);

            return Task.FromResult(0);
        }

        /// <summary>
        /// Get if two factor authentication is enabled on the user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<bool> GetTwoFactorEnabledAsync(TUser user)
        {
            return Task.FromResult(user.TwoFactorEnabled);
        }

        /// <summary>
        /// Get user lock out end date
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<DateTimeOffset> GetLockoutEndDateAsync(TUser user)
        {
            return
                Task.FromResult(user.LockoutEndDateUtc.HasValue
                    ? new DateTimeOffset(DateTime.SpecifyKind(user.LockoutEndDateUtc.Value, DateTimeKind.Utc))
                    : new DateTimeOffset());
        }


        /// <summary>
        /// Set user lockout end date
        /// </summary>
        /// <param name="user"></param>
        /// <param name="lockoutEnd"></param>
        /// <returns></returns>
        public Task SetLockoutEndDateAsync(TUser user, DateTimeOffset lockoutEnd)
        {
            user.LockoutEndDateUtc = lockoutEnd.UtcDateTime;
            userTable.Update(user);

            return Task.FromResult(0);
        }

        /// <summary>
        /// Increment failed access count
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<int> IncrementAccessFailedCountAsync(TUser user)
        {
            user.AccessFailedCount++;
            userTable.Update(user);

            return Task.FromResult(user.AccessFailedCount);
        }

        /// <summary>
        /// Reset failed access count
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task ResetAccessFailedCountAsync(TUser user)
        {
            user.AccessFailedCount = 0;
            userTable.Update(user);

            return Task.FromResult(0);
        }

        /// <summary>
        /// Get failed access count
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<int> GetAccessFailedCountAsync(TUser user)
        {
            return Task.FromResult(user.AccessFailedCount);
        }

        /// <summary>
        /// Get if lockout is enabled for the user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<bool> GetLockoutEnabledAsync(TUser user)
        {
            return Task.FromResult(user.LockoutEnabled);
        }

        /// <summary>
        /// Set lockout enabled for user
        /// </summary>
        /// <param name="user"></param>
        /// <param name="enabled"></param>
        /// <returns></returns>
        public Task SetLockoutEnabledAsync(TUser user, bool enabled)
        {
            user.LockoutEnabled = enabled;
            userTable.Update(user);

            return Task.FromResult(0);
        }



        public Task AddLoginAsync(TUser user, UserLoginInfo login)
        {
            throw new NotImplementedException();
        }

        public Task<TUser> FindAsync(UserLoginInfo login)
        {
            throw new NotImplementedException();
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(TUser user)
        {
            throw new NotImplementedException();
        }

        public Task RemoveLoginAsync(TUser user, UserLoginInfo login)
        {
            throw new NotImplementedException();
        }

        public Task<List<TUser>> GetUsers(FrontUserQueryEntity query, out int totalCount)
        {
            var userList = userTable.GetUsers(query, out totalCount) as List<TUser>;

            return Task.FromResult<List<TUser>>(userList);
        }

        public Task<List<TUser>> ExportGetUsers(FrontUserQueryEntity query, out int totalCount)
        {
            var userList = userTable.ExportGetUsers(query, out totalCount) as List<TUser>;

            return Task.FromResult<List<TUser>>(userList);
        }

        public Task<List<TUser>> GetExtraUsers(FrontUserQueryEntity query, out int totalCount)
        {
            var userList = userTable.GetExtraUsers(query, out totalCount) as List<TUser>;

            return Task.FromResult<List<TUser>>(userList);
        }

        /// <summary>
        /// 获取经销商入会的需要审核的信息
        /// </summary>
        /// <param name="dealerId">经销商ID</param>
        /// <param name="identityNumber">会员证件号码</param>
        /// <returns>经销商入会的需要审核的信息列表</returns>
        public Task<List<MembershipRequestApproval>> GetApprovingMembershipList( string dealerId, string identityNumber)
        {
            var userList = userTable.GetApprovingMembershipList(dealerId, identityNumber);
            return Task.FromResult(userList);
        }

        public Task<List<MembershipRequestApproval>> FindApprovingMembership(string PayNumber, string phoneNumber, string dealerId, string IdentityNumber, string ApproveType, int skip, int count, out int totalCount, int IsPay, decimal Amount,string PaperWork,string VINNumber,string No)
        {
            var userList = userTable.FindApprovingMembership(PayNumber, phoneNumber, dealerId, IdentityNumber, ApproveType, skip, count, out totalCount, IsPay, Amount, PaperWork, VINNumber,No);
            return Task.FromResult(userList);
        }

        public Task<bool> ManagerMembershipRequest(string id, out string message, string phone, string SubmitTime)
        {
            var result = userTable.ManagerMembershipRequest(id, out message, phone, SubmitTime);

            return Task.FromResult<bool>(result);
        }

        public Task<bool> ApprovalMembershipRequest(string id, out string message, string phone)
        {
            var result = userTable.ApprovalMembershipRequestman(id, out message, phone);

            return Task.FromResult<bool>(result);
        }
        public Task<bool> RejectMembershipRequest(string id, out string message)
        {
            var result = userTable.RejectMembershipRequest(id, out message);
            return Task.FromResult<bool>(result);
        }

        /// <summary>
        /// 如果会员/用户在某一经销商消费,记录消费关系
        /// </summary>
        /// <param name="membershipId"></param>
        /// <param name="dealerId"></param>
        /// <returns></returns>
        public bool AddMembershipDealerRecord(string membershipId, string dealerId)
        {
            return userTable.AddMembershipDealerRecord(membershipId, dealerId);
        }

        //删除消费关系
        public bool DeleteMembershipDealerRecord(string membershipId)
        {
            return userTable.DeleteMembershipDealerRecord(membershipId);
        }

        public Task<List<MembershipRequestFailed>> GetMembershipRequestFailedList(int? status, string phone, int start, int count, out int totalCount)
        {
            var userList = userTable.GetMembershipRequestFailedList(status, phone, start, count, out totalCount) as List<MembershipRequestFailed>;

            return Task.FromResult<List<MembershipRequestFailed>>(userList);
        }

        public Task<List<MembershipRequestFailed>> GetMembershipRequestFailedListAll(int? status)
        {
            var userList = userTable.GetMembershipRequestFailedListAll(status) as List<MembershipRequestFailed>;

            return Task.FromResult<List<MembershipRequestFailed>>(userList);
        }

        public Task<bool> UpdateMembershipRequestStatus(string id, string operatorName, out string message)
        {
            var result = userTable.UpdateMembershipRequestStatus(id, operatorName, out message);
            return Task.FromResult<bool>(result);
        }
        public Task<bool> Activate(string id, string operatorName, out string message)
        {
            var result = userTable.Activate(id, operatorName, out message);
            return Task.FromResult<bool>(result);
        }
        public Task<bool> UpdateIdentityNumber(string id, string identityNumber, string operatorName, out string message)
        {
            var result = userTable.UpdateIdentityNumber(id, identityNumber, operatorName, out message);
            return Task.FromResult<bool>(result);
        }

        public Task<bool> UpdateIdentityNumberBy4S(string id, string identityNumber, string operatorName, out string message)
        {
            var result = userTable.UpdateIdentityNumberBy4S(id, identityNumber, operatorName, out message);
            return Task.FromResult<bool>(result);
        }

        public bool CreateMembershipRequest(string membershipId, string identityNumber, string dealerId, string memo, string dataSource)
        {
            return userTable.CreateMembershipRequest(membershipId, identityNumber, dealerId, memo,dataSource);
        }

        public bool CheckNickNameIsExist(string nickName,string id)
        {
            return userTable.CheckNickNameIsExist(nickName,id);
        }
        public bool CheckNickNameIsExist(string nickName)
        {
            return userTable.CheckNickNameIsExist(nickName);
        }
        //手机号
        public bool CheckUserNameIsExist(string phoneNumber)
        {
            return userTable.CheckUserNameIsExist(phoneNumber);
        }
        //用户名，id
        public bool CheckUserNameIsExist(string UserName, string id)
        {
            return userTable.CheckUserNameIsExist1(UserName,id);
        }

        public Task<bool> updatePhoneNumberModal(string id, string phoneNumber, string operatorName, out string message)
        {
            var result = userTable.updatePhoneNumberModal(id, phoneNumber, operatorName, out message);
            return Task.FromResult<bool>(result);
        }

        public Task<bool> CheckMembershipIdIsExist(string MembershipId)
        {
            var result = userTable.CheckMembershipIdIsExist(MembershipId);
            return Task.FromResult<bool>(result);
        }

        public Task Update_Or_Insert_Membership_Schedule(TUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            userTable.Update_Or_Insert_Membership_Schedule(user);
            return Task.FromResult<object>(null);
        }

    }
}
