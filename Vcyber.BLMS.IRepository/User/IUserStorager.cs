using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;

namespace Vcyber.BLMS.IRepository
{
    using Vcyber.BLMS.Entity.Generated;

    /// <summary>
    /// 用户操作，与userinfo相关的函数无效
    /// </summary>
    public interface IUserStorager
    {
        #region ==== public method ====

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="data"></param>
        /// <param name="status">用户状态</param>
        void Add(UserInfo data, EUserStatus status);

        /// <summary>
        /// 保存用户基本信息
        /// </summary>
        /// <param name="data"></param>
        bool SaveBaseInfo(UserBaseData data);

        /// <summary>
        /// 保存用户身份认证信息
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool SaveIdentity(UserIdentity data);

        /// <summary>
        /// 用户名称是否重复
        /// </summary>
        /// <param name="name">添加用户</param>
        /// <returns>true:重复</returns>
        bool IsName(string name);

        /// <summary>
        /// 是否存在相同的身份证
        /// </summary>
        /// <param name="identityNumber">身份证编号</param>
        /// <returns>true:存在</returns>
        bool IsIdentityNumber(string identityNumber);

        /// <summary>
        /// 根据用户Guid获取
        /// </summary>
        /// <param name="userGuid"></param>
        /// <returns></returns>
        UserInfo SelectOne(string userGuid);

        /// <summary>
        /// 根据用户名称，密码获取用户信息
        /// </summary>
        /// <param name="name">用户名称</param>
        /// <param name="pw">密码</param>
        /// <returns></returns>
        UserInfo SelectOne(string name, string pw);

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="data">分页数据</param>
        /// <param name="totalCount">数据总个数</param>
        /// <returns></returns>
        IEnumerable<UserInfo> SelectList(PageData data, out int totalCount);

        /// <summary>
        /// 修改用户状态
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="status">用户状态</param>
        /// <returns></returns>
        bool UpdateStatus(int userId, EUserStatus status);

        /// <summary>
        /// 根据手机号查询用户
        /// </summary>
        /// <param name="phoneNumber">手机号</param>
        /// <returns>用户</returns>
        [Obsolete]
        UserInfo SelectOneByPhone(string phoneNumber);

        /// <summary>
        /// 根据身份证号查询用户
        /// </summary>
        /// <param name="phoneNumber">身份证号</param>
        /// <returns>用户</returns>
        UserInfo SelectOneByIdentityNumber(string identityNumber);

        /// <summary>
        /// 根据手机号+身份证号查询用户
        /// </summary>
        /// <param name="phoneNumber">手机号</param>
        /// <param name="identityNumber">身份证号</param>
        /// <returns>用户</returns>
        UserInfo SelectOneByPhoneAndIdentity(string phoneNumber, string identityNumber);

        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <param name="userGuid">用户标识</param>
        /// <param name="loginPassword">登陆密码</param>
        /// <returns>执行结果</returns>
        bool ChangeLoginPassword(string userGuid, string loginPassword);

        /// <summary>
        /// 查询客户信息
        /// </summary>
        /// <param name="identityNumber">证件号</param>
        /// <returns></returns>
        IFCustomer FindCustome(string identityNumber);

        #endregion

        IFCustomer GetInfoWhenBuyCar(string identityNumber);

        /// <summary>
        /// 根据Vin查询用户信息
        /// </summary>
        /// <param name="vin">Vin</param>
        /// <returns>UserInfo</returns>
        Membership SelectOneByVin(string vin);

        /// <summary>
        /// 根据手机查询用户信息
        /// </summary>
        /// <param name="Tel">手机号码</param>
        /// <returns>UserInfo</returns>
        Membership SelectOneByTel(string Tel);

        int UpdateTel(Membership user);

        /// <summary>
        /// 获取用户信息（users表）
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        UsersModel GetUserById(string userId);

    }
}
