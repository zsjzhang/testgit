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
    /// <summary>
    /// 用户账户关联操作
    /// </summary>
    public class UserAccountRelevanceStorager : IUserAccountRelevanceStorager
    {
        #region ==== public constructor ====

        public UserAccountRelevanceStorager() { }

        #endregion

        #region ==== public method ====

        /// <summary>
        /// 添加账户关联
        /// </summary>
        /// <param name="data"></param>
        public void Add(UserAccountRelevance data)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(" INSERT INTO useraccountrelevance( MallCode, MallUserCode, UserId, MallName, CreateTime, UpdateTime, IsDel)");
            sql.Append(" VALUES(@MallCode, @MallUserCode, @UserId, @MallName, @CreateTime, @UpdateTime, @IsDel);");
            DbHelp.Execute(sql.ToString(), data);
        }

        /// <summary>
        /// 车音通用户是否关联某个商城账户
        /// </summary>
        /// <param name="mallCode">商城Code</param>
        /// <param name="mallUserCode"></param>
        /// <returns>true:已经关联</returns>
        public bool IsRelevance( string mallCode, string mallUserCode)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(" SELECT COUNT(1) FROM useraccountrelevance WHERE ");
            sql.Append(" AND useraccountrelevance.MallCode=@MallCode AND useraccountrelevance.MallUserCode=@MallUserCode AND useraccountrelevance.IsDel=@IsDel");
            return DbHelp.ExecuteScalar<int>(sql.ToString(), new { MallCode = mallCode, MallUserCode = mallUserCode, IsDel = (int)EDataStatus.NoDelete }) > 0;
        }

        /// <summary>
        /// 获取车音通用户关联商城账户信息
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        public IEnumerable<UserAccountRelevance> SelectList(int userId)
        {
            string sql = "SELECT * FROM useraccountrelevance WHERE useraccountrelevance.UserId=@UserId AND useraccountrelevance.IsDel=0";
            return DbHelp.Query<UserAccountRelevance>(sql, new { UserId=userId });
        }

        /// <summary>
        /// 删除用户的某个商城关联账户
        /// </summary>
        /// <param name="relevanceId">关联账户Id</param>
        /// <returns></returns>
        public bool Delete(int relevanceId)
        {
            string sql = "UPDATE useraccountrelevance SET useraccountrelevance.IsDel=1 WHERE useraccountrelevance.Id=@Id";
            return DbHelp.Execute(sql, new { Id = relevanceId }) > 0;
        }

        #endregion
    }
}
