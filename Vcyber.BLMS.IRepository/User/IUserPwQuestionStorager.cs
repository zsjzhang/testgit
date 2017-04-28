using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.IRepository
{
    public interface IUserPwQuestionStorager
    {
        /// <summary>
        /// 添加用户问题及答案
        /// </summary>
        /// <param name="data">数据实体</param>
        /// <returns>执行结果</returns>
        bool Add(UserPwQuestion data);

        /// <summary>
        /// 查询用户的密保问题及答案
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>密保问题及答案</returns>
        IEnumerable<UserPwQuestion> Select(int userId);

        /// <summary>
        /// 删除用户设定的所有密保
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        bool Delete(int userId);
    }
}
