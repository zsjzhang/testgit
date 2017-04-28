using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;

namespace Vcyber.BLMS.Application
{
    using Vcyber.BLMS.Entity.Generated;

    /// <summary>
    /// 用户业务 逻辑
    /// </summary>
    public interface IUserInfoApp
    {
        #region ==== public method ====

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="data"></param>
        void Register(UserInfo data);

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
        /// 获取用户信息
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pw"></param>
        /// <returns></returns>
        UserInfo GetOne(string name, string pw);

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="userGuid">用户Guid</param>
        /// <returns></returns>
        UserInfo GetOne(string userGuid);

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="data">分页数据</param>
        /// <param name="totalCount">数据总个数</param>
        /// <returns></returns>
        IEnumerable<UserInfo> GetList(PageData data, out int totalCount);

        /// <summary>
        /// 验证用户名称是否重复
        /// </summary>
        /// <param name="name">用户名称</param>
        /// <returns>true:重复</returns>
        bool IsName(string name);

        /// <summary>
        /// 是否存在相同的身份证
        /// </summary>
        /// <param name="identityNumber">身份证编号</param>
        /// <returns>true:存在</returns>
        bool IsIdentityNumber(string identityNumber);

        /// <summary>
        /// 修改用户状态
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="status"></param>
        /// <returns>true:成功</returns>
        bool UpdateStatus(int userId, EUserStatus status);

        /// <summary>
        /// 根据手机号查询用户
        /// </summary>
        /// <param name="phoneNumber">手机号</param>
        /// <returns>用户</returns>
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

        IFCustomer GetInfoWhenBuyCar(string identityNumber);

        /// <summary>
        /// 根据Vin查询用户信息
        /// </summary>
        /// <param name="vin">Vin</param>
        /// <returns>UserInfo</returns>
        Membership SelectOneByVin(string vin);

        Membership SelectOneByTel(string tel);

        [Obsolete("没有使用")]
        int UpdateTel(Membership user);



        /// <summary>
        /// 获取用户信息（users表）
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        UsersModel GetUserById(string userId);

        #endregion
    }
}
