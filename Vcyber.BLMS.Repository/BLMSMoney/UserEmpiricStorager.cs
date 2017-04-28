using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.SelectCondition;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.IRepository;

namespace Vcyber.BLMS.Repository
{
    /// <summary>
    /// 用户经验记录操作
    /// </summary>
    public class UserEmpiricStorager : IUserEmpiricStorager
    {
        #region ==== 构造函数 ====

        public UserEmpiricStorager() { }

        #endregion

        #region ==== 公共方法 ====

        /// <summary>
        /// 添加用户经验记录
        /// </summary>
        /// <param name="data"></param>
        public void Add(UserEmpiricRecord data)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(" insert into UserEmpiricRecord(UserId,SourceId,Value,UseValue,CreateTime,DataState,Remark)");
            sql.Append(" values(@UserId,@SourceId,@Value,@UseValue,@CreateTime,@DataState,@Remark)");
            DbHelp.Execute(sql.ToString(), data);
        }

        /// <summary>
        /// 获取有效的用户经验记录信息
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="pageData"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public IEnumerable<UserEmpiricRecord> SelectList(string userId, PageData pageData, out int total)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("  select count(1) from UserEmpiricRecord where UserId=@UserId and DataState=@DataState");
            total = DbHelp.ExecuteScalar<int>(sql.ToString(), new { UserId = userId, DataState = EDataStatus.NoDelete.ToInt32() });

            sql.Clear();

            sql.AppendFormat(" select top {0} UserEmpiricRecord.* from UserEmpiricRecord where UserId=@UserId ", pageData.Size);
            sql.Append(" and DataState=@DataState and UserEmpiricRecord.Id not in(");
            sql.AppendFormat(" select top {0} UserEmpiricRecord.Id from UserEmpiricRecord where UserId=@UserId ", pageData.Index);
            sql.Append(" and DataState=@DataState order by UserEmpiricRecord.CreateTime desc)");
            sql.Append(" order by UserEmpiricRecord.CreateTime desc");
            return DbHelp.Query<UserEmpiricRecord>(sql.ToString(), new { UserId = userId, DataState = EDataStatus.NoDelete.ToInt32() });
        }

        /// <summary>
        /// 获取用户总经验值
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int TotalValue(string userId)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(" select isnull(SUM(ISNULL(Value,0)-ISNULL(UseValue,0)),0) from UserEmpiricRecord");
            sql.Append("  where UserEmpiricRecord.UserId=@UserId and UserEmpiricRecord.DataState=@DataState");
            return DbHelp.ExecuteScalar<int>(sql.ToString(), new { UserId = userId, DataState = EDataStatus.NoDelete.ToInt32() });
        }

        /// <summary>
        /// 扣除用户经验值
        /// </summary>
        /// <param name="id">经验值Id</param>
        /// <param name="userId">用户Id</param>
        /// <param name="value">需要扣除的经验值</param>
        /// <returns></returns>
        public bool SubValue(int id, string userId, int value)
        {
            if (value <= 0)
            {
                return false;
            }

            StringBuilder sql = new StringBuilder();
            sql.Append(" update UserEmpiricRecord set UseValue=ISNULL(UseValue,0)+@subvalue where UserId=@UserId and ID=@Id");
            sql.Append(" and (ISNULL(Value,0)-ISNULL(UseValue,0))>=@subvalue");
            return DbHelp.Execute(sql.ToString(), new { subvalue = value, UserId = userId, Id=id }) > 0;
        }

        /// <summary>
        /// 统计用户获得某种经验值的次数
        /// </summary>
        /// <param name="rule">经验值规则</param>
        /// <param name="userId">用户Id</param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public int Count(EEmpiricRule rule, string userId, UserEmpiricCondition condition)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select count(1) from UserEmpiricRecord where UserEmpiricRecord.UserId=@UserId and UserEmpiricRecord.SourceId=@SourceId");
            sql.AppendFormat(" and {0}", condition.ToWhere());
            return DbHelp.ExecuteScalar<int>(sql.ToString(), new { UserId = userId, SourceId = (rule.ToInt32()) });
        }

        #endregion
    }
}
