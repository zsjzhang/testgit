using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.IRepository;

namespace Vcyber.BLMS.Repository
{
    public class QuestionnaireManageStorager:IQuestionnaireManageStorager
    {
        /// <summary>
        /// 新增用户与问卷关系表数据
        /// </summary>
        /// <param name="entity">用户与问卷关系实体</param>
        /// <returns>用户与问卷关系id</returns>
        public int Create(QuestionnaireManage entity)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(" insert into CS_QuestionnaireManage (UserId,IsFirst,State,ParentId)");
            sql.Append(" values (@UserId,@IsFirst,@State,@ParentId);select @@identity;");
            return DbHelp.ExecuteScalar<int>(sql.ToString(), entity);
        }
    }
}
