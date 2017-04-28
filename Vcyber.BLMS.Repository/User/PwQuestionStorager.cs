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
    public class PwQuestionStorager : IPwQuestionStorager
    {
        /// <summary>
        /// 添加问题
        /// </summary>
        /// <param name="question">问题</param>
        /// <param name="id">返回的数据行ID</param>
        /// <returns>执行结果</returns>
        public bool Add(PwQuestion question, out int id)
        {
            const string sql = @"INSERT INTO pwquestion(Content,CreateTime) VALUES(@Content,NOW());SELECT @@identity";

            id = DbHelp.ExecuteScalar<int>(sql, question);

            return id > 0;
        }

        /// <summary>
        /// 查询所有问题
        /// </summary>
        /// <returns>问题集合</returns>
        public IEnumerable<PwQuestion> Select()
        {
            var sql = "SELECT * FROM pwquestion";

            return DbHelp.Query<PwQuestion>(sql, null);
        }
    }
}
