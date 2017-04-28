using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.IRepository;
using Vcyber.BLMS.Application;
using System.Transactions;


namespace Vcyber.BLMS.Domain
{
    using Vcyber.BLMS.Entity.Generated;

    /// <summary>
    /// 用户业务逻辑
    /// </summary>
    public class UserInfoApp : IUserInfoApp
    {
        #region ==== public constructor ====

        public UserInfoApp() { }

        #endregion

        #region ==== public method ====

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="data"></param>
        public void Register(UserInfo data)
        {
            data.Password = CommonUtilitys.EncodeMD5(data.Password);
            _DbSession.UserStorager.Add(data, EUserStatus.Enable);
        }

        /// <summary>
        /// 保存用户基本信息
        /// </summary>
        /// <param name="data"></param>
        public bool SaveBaseInfo(UserBaseData data)
        {
            return _DbSession.UserStorager.SaveBaseInfo(data);
        }

        /// <summary>
        /// 保存用户身份认证信息
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool SaveIdentity(UserIdentity data)
        {
            return _DbSession.UserStorager.SaveIdentity(data);
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="userGuid">用户Guid</param>
        /// <returns></returns>
        public UserInfo GetOne(string userGuid)
        {
            return _DbSession.UserStorager.SelectOne(userGuid);
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pw"></param>
        /// <returns></returns>
        public UserInfo GetOne(string name, string pw)
        {
            return _DbSession.UserStorager.SelectOne(name, CommonUtilitys.EncodeMD5(pw));
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="data">分页数据</param>
        /// <param name="totalCount">数据总个数</param>
        /// <returns></returns>
        public IEnumerable<UserInfo> GetList(PageData data, out int totalCount)
        {
            return _DbSession.UserStorager.SelectList(data, out totalCount);
        }

        /// <summary>
        /// 验证用户名称是否重复
        /// </summary>
        /// <param name="name">用户名称</param>
        /// <returns>true:重复</returns>
        public bool IsName(string name)
        {
            return _DbSession.UserStorager.IsName(name);
        }

        /// <summary>
        /// 是否存在相同的身份证
        /// </summary>
        /// <param name="identityNumber">身份证编号</param>
        /// <returns>true:存在</returns>
        public bool IsIdentityNumber(string identityNumber)
        {
            return _DbSession.UserStorager.IsIdentityNumber(identityNumber);
        }

        /// <summary>
        /// 修改用户状态
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="status"></param>
        /// <returns>true:成功</returns>
        public bool UpdateStatus(int userId, EUserStatus status)
        {
            return _DbSession.UserStorager.UpdateStatus(userId, status);
        }

        /// <summary>
        /// 根据手机号查询用户
        /// </summary>
        /// <param name="phoneNumber">手机号</param>
        /// <returns>用户</returns>
        public UserInfo SelectOneByPhone(string phoneNumber)
        {
            return _DbSession.UserStorager.SelectOneByPhone(phoneNumber);
        }

        /// <summary>
        /// 根据身份证号查询用户
        /// </summary>
        /// <param name="phoneNumber">身份证号</param>
        /// <returns>用户</returns>
        public UserInfo SelectOneByIdentityNumber(string identityNumber)
        {
            return _DbSession.UserStorager.SelectOneByIdentityNumber(identityNumber);
        }

        /// <summary>
        /// 根据手机号+身份证号查询用户
        /// </summary>
        /// <param name="phoneNumber">手机号</param>
        /// <param name="identityNumber">身份证号</param>
        /// <returns>用户</returns>
        public UserInfo SelectOneByPhoneAndIdentity(string phoneNumber, string identityNumber)
        {
            return _DbSession.UserStorager.SelectOneByPhoneAndIdentity(phoneNumber, identityNumber);
        }

        /// <summary>
        /// 修改用户登录密码
        /// </summary>
        /// <param name="userGuid">用户标识</param>
        /// <param name="loginPassword">登陆密码</param>
        /// <returns>执行结果</returns>
        public bool ChangeLoginPassword(string userGuid, string loginPassword)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                //1.修改登录密码
                bool isSuccess = _DbSession.UserStorager.ChangeLoginPassword(userGuid, loginPassword);

                //2.查询用户信息
                var userInfo = _DbSession.UserStorager.SelectOne(userGuid);


                //3.添加操作记录
                var record = new UserOperRecord
                {
                    UserId = userInfo.UserGuid,
                    UserName = userInfo.UserName,
                    OperType = (int)EUserOperateType.ChangeLoginPassword,
                    CreateTime = DateTime.Now
                };

                if (!isSuccess)
                    return false;

                _DbSession.UserRecordStorager.AddOperate(record);

                scope.Complete();
                return isSuccess;
            }
        }

        public IFCustomer GetInfoWhenBuyCar(string identityNumber)
        {
            return _DbSession.UserStorager.GetInfoWhenBuyCar(identityNumber);
        }

        Membership IUserInfoApp.SelectOneByVin(string vin)
        {
            return _DbSession.UserStorager.SelectOneByVin(vin);
        }

        public Membership SelectOneByTel(string tel)
        {
            return _DbSession.UserStorager.SelectOneByTel(tel);
        }

        public int UpdateTel(Membership user)
        {
            return _DbSession.UserStorager.UpdateTel(user);
        }

        /// <summary>
        /// 查询客户信息
        /// </summary>
        /// <param name="identityNumber">证件号</param>
        /// <returns></returns>
        public IFCustomer FindCustome(string identityNumber)
        {
            return _DbSession.UserStorager.FindCustome(identityNumber);
        }

        #endregion


        /// <summary>
        /// 获取用户信息（users表）
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        public UsersModel GetUserById(string userId)
        {
            return _DbSession.UserStorager.GetUserById(userId);
        }
    }
}
