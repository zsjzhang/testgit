using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.IRepository;

namespace Vcyber.BLMS.Repository
{
    using PetaPoco;

    using Vcyber.BLMS.Entity.Generated;
    using Vcyber.BLMS.Repository.Entity.Generated;

    /// <summary>
    /// 用户操作
    /// </summary>
    public class UserStorager : IUserStorager
    {
        #region ==== public constructor ====

        public UserStorager() { }

        #endregion

        #region ==== public method ====

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="data"></param>
        /// <param name="status">用户状态</param>
        [Obsolete]
        public void Add(UserInfo data, EUserStatus status)
        {
            data.Status = (int)status;
            string sql = "INSERT INTO userinfo('NAME','PASSWORD','STATUS',CREATETime,UpdateTime) VALUES(@NAME,@PASSWORD,@STATUS,@CREATETime,@UpdateTime);";
            DbHelp.Execute(sql, data);
        }

        /// <summary>
        /// 保存用户基本信息
        /// </summary>
        /// <param name="data"></param>
        [Obsolete]
        public bool SaveBaseInfo(UserBaseData data)
        {
            string sql = "UPDATE userinfo SET userinfo.BirthTime=@BirthTime,userinfo.Sex=@Sex,userinfo.Hobby=@Hobby WHERE userinfo.UserGuid=@UserGuid";
            return DbHelp.Execute(sql, new { BirthTime = data.BirthTime, Sex = data.Sex, Hobby = data.ConvertHobbys(), UserGuid = data.UserGuid }) > 0;
        }

        /// <summary>
        /// 保存用户身份认证信息
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Obsolete]
        public bool SaveIdentity(UserIdentity data)
        {
            string sql = "UPDATE userinfo SET IdentityNumber=@IdentityNumber,IdentityImg=@IdentityImg,IdentityValidTime=@IdentityValidTime,IdentityConfirmed=@IdentityConfirmed WHERE userinfo.Id=@Id";
            return DbHelp.Execute(sql, new { IdentityNumber = data.IdentityNumber, IdentityImg = data.ConvertImg(), IdentityValidTime = data.IdentityValidTime, IdentityConfirmed = data.IdentityConfirmed, Id = data.UserId }) > 0;
        }

        /// <summary>
        /// 用户名称是否重复
        /// </summary>
        /// <param name="name">添加用户</param>
        /// <returns>true:重复</returns>
        [Obsolete]
        public bool IsName(string name)
        {
            string sql = "SELECT COUNT(1) FROM userinfo WHERE LOWER(userinfo.UserName)=@Name";
            return DbHelp.ExecuteScalar<int>(sql, new { Name = name.ToLower() }) > 0;
        }

        /// <summary>
        /// 是否存在相同的身份证
        /// </summary>
        /// <param name="identityNumber">身份证编号</param>
        /// <returns>true:存在</returns>
        [Obsolete]
        public bool IsIdentityNumber(string identityNumber)
        {
            identityNumber = identityNumber.ToUpper();
            string sql = "SELECT COUNT(1) FROM userinfo WHERE userinfo.IdentityNumber=@IdentityNumber";
            return DbHelp.ExecuteScalar<int>(sql, new { IdentityNumber = identityNumber }) > 0;
        }

        /// <summary>
        /// 根据用户Guid获取
        /// </summary>
        /// <param name="userGuid"></param>
        /// <returns></returns>
        [Obsolete]
        public UserInfo SelectOne(string userGuid)
        {
            string sql = "SELECT * FROM userinfo WHERE userinfo.UserGuid=@UserGuid";
            return DbHelp.QueryOne<UserInfo>(sql, new { UserGuid = userGuid });
        }

        /// <summary>
        /// 根据用户名称，密码获取用户信息
        /// </summary>
        /// <param name="name">用户名称</param>
        /// <param name="pw">密码</param>
        /// <returns></returns>
        [Obsolete]
        public UserInfo SelectOne(string name, string pw)
        {
            string sql = "SELECT * FROM userinfo WHERE userinfo.UserName=@Name AND userinfo.Password=@pw";
            return DbHelp.QueryOne<UserInfo>(sql, new { Name = name, pw = pw });
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="data">分页数据</param>
        /// <param name="totalCount">数据总个数</param>
        /// <returns></returns>
        [Obsolete]
        public IEnumerable<UserInfo> SelectList(PageData data, out int totalCount)
        {
            totalCount = DbHelp.ExecuteScalar<int>("SELECT COUNT(1) FROM userinfo");
            return DbHelp.Query<UserInfo>("SELECT * FROM userinfo ORDER BY userinfo.CreateTime DESC LIMIT @Index,@Size", data);
        }

        /// <summary>
        /// 修改用户状态
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="status">用户状态</param>
        /// <returns></returns>
        [Obsolete]
        public bool UpdateStatus(int userId, EUserStatus status)
        {
            string sql = "UPDATE userinfo SET userinfo.Status=@Status WHERE userinfo.Id=@Id";
            return DbHelp.Execute(sql, new { Status = (int)status, Id = userId }) > 0;
        }

        /// <summary>
        /// 根据手机号查询用户
        /// </summary>
        /// <param name="phoneNumber">手机号</param>
        /// <returns>用户</returns>
        [Obsolete]
        public UserInfo SelectOneByPhone(string phoneNumber)
        {
            string sql = "SELECT * FROM userinfo WHERE userinfo.PhoneNumber=@PhoneNumber";
            return DbHelp.QueryOne<UserInfo>(sql, new { PhoneNumber = phoneNumber });
        }

        /// <summary>
        /// 根据身份证号查询用户
        /// </summary>
        /// <param name="phoneNumber">身份证号</param>
        /// <returns>用户</returns>
        [Obsolete]
        public UserInfo SelectOneByIdentityNumber(string identityNumber)
        {
            string sql = "SELECT * FROM userinfo WHERE userinfo.IdentityNumber=@IdentityNumber";
            return DbHelp.QueryOne<UserInfo>(sql, new { IdentityNumber = identityNumber });
        }

        /// <summary>
        /// 根据手机号+身份证号查询用户
        /// </summary>
        /// <param name="phoneNumber">手机号</param>
        /// <param name="identityNumber">身份证号</param>
        /// <returns>用户</returns>
        [Obsolete]
        public UserInfo SelectOneByPhoneAndIdentity(string phoneNumber, string identityNumber)
        {
            string sql = "SELECT * FROM userinfo WHERE userinfo.PhoneNumber=@PhoneNumber AND userinfo.IdentityNumber=@IdentityNumber";
            return DbHelp.QueryOne<UserInfo>(sql, new { PhoneNumber = phoneNumber, IdentityNumber = identityNumber });
        }

        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <param name="userGuid">用户标识</param>
        /// <param name="loginPassword">登陆密码</param>
        /// <returns>执行结果</returns>
        [Obsolete]
        public bool ChangeLoginPassword(string userGuid, string loginPassword)
        {
            string sql = "UPDATE userinfo SET Password = @Password WHERE UserGuid = @UserGuid";
            return DbHelp.Execute(sql, new { UserGuid = userGuid, Password = loginPassword }) > 0;
        }

        /// <summary>
        /// 查询客户信息
        /// </summary>
        /// <param name="identityNumber">证件号</param>
        /// <returns></returns>
        public IFCustomer FindCustome(string identityNumber)
        {
            StringBuilder sql = new StringBuilder();

            sql.Append(" select cus.*,sche.PaperWork ");
            sql.Append(" from IF_Customer cus left join Membership mem on cus.IdentityNumber=mem.IdentityNumber");
            sql.Append(" join Membership_Schedule sche on mem.Id=sche.MembershipId");
            sql.Append(" where cus.IdentityNumber=@IdentityNumber");

            return DbHelp.QueryOne<IFCustomer>(sql.ToString(), new { IdentityNumber = identityNumber });
        }

        public IFCustomer GetInfoWhenBuyCar(string identityNumber)
        {
            return PocoHelper.CurrentDb().FirstOrDefault<IFCustomer>("where IdentityNumber=@0", identityNumber);
        }

        public Membership SelectOneByVin(string vin)
        {
            Sql sql = new Sql(@"SELECT	a.* FROM Membership a
join	if_customer b
on	a.IdentityNumber = b.IdentityNumber
join	IF_Car c
on	b.CustId = c.CustId
where	c.VIN=@0", vin);
            return PocoHelper.CurrentDb().FirstOrDefault<Membership>(sql);
        }

        public Membership SelectOneByTel(string Tel)
        {
            return PocoHelper.CurrentDb()
                .FirstOrDefault<Membership>("select * from membership where phonenumber=@0", Tel);
        }
        public int UpdateTel(Membership user)
        {
            return PocoHelper.CurrentDb().Update(user, "PhoneNumber");
        }

        #endregion


        public UsersModel GetUserById(string userId)
        {
            string sql = " SELECT  ID,PhoneNumber,DealerId,DealerName,Status  FROM Users  where ID=@ID";
            return DbHelp.QueryOne<UsersModel>(sql, new { ID = userId });
        }
    }
}
