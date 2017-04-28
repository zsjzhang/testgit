using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Vcyber.BLMS.IRepository;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Common;

namespace Vcyber.BLMS.Repository
{
    public class UserPwQuestionStorager : IUserPwQuestionStorager
    {
        /// <summary>;
        /// 添加用户问题及答案
        /// </summary>
        /// <param name="data">数据实体</param>
        /// <returns>执行结果</returns>
        public bool Add(UserPwQuestion data)
        {
            const string sql = @"INSERT INTO userpwquestion(UserId,PwId,Answer,CreateTime) VALUES(@UserId,@PwId,@Answer,NOW());SELECT @@identity";

            int id = DbHelp.ExecuteScalar<int>(sql, data);

            return id > 0;
        }

        /// <summary>
        /// 查询用户的密保问题及答案
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>密保问题及答案</returns>
        public IEnumerable<UserPwQuestion> Select(int userId)
        {
            string sql = "SELECT u.*,p.Content AS Question FROM userpwquestion u LEFT JOIN pwquestion p ON u.PwId = p.Id WHERE UserId = @UserId";

            return DbHelp.Query<UserPwQuestion>(sql.ToString(), new { UserId = userId });
        }

        /// <summary>
        /// 删除用户设定的所有密保
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool Delete(int userId)
        {
            string sql = "DELETE FROM userpwquestion WHERE UserId = @UserId";

            return DbHelp.Execute(sql, new { UserId = userId }) > 0;
        }
    }
}
