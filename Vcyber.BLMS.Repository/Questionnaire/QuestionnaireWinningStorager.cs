using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.IRepository;
using Vcyber.BLMS.Common;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.Repository
{
    public class QuestionnaireWinningStorager : IQuestionnaireWinningStorager
    {
        public IEnumerable<QuestionnaireWinning> GetQuestionnaireWinning(int id)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("Select * from CS_QuestionnaireWinning where QuestionnaireId={0}", id);
            return DbHelp.Query<QuestionnaireWinning>(sql.ToString());
        }


        public int Create(QuestionnaireWinning entity)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(" insert into CS_QuestionnaireWinning (QuestionnaireId,WName,WPhoneNumber,Prize)");
            sql.Append(" values (@QuestionnaireId,@WName,@WPhoneNumber,@Prize);select @@identity;");

            return DbHelp.ExecuteScalar<int>(sql.ToString(), entity);
        }


        public IEnumerable<QuestionnaireWinning> SelectQuestionnaireWinning(int id, int pageIndex, int pageSize, out int total)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("select count(*) from CS_QuestionnaireWinning where QuestionnaireId = @QuestionnaireId");
            total = DbHelp.ExecuteScalar<int>(sql.ToString(), new { QuestionnaireId = id });
            sql.Clear();

            sql.AppendFormat("select top {0} * from CS_QuestionnaireWinning", pageSize);
            sql.AppendLine(" where QuestionnaireId = @QuestionnaireId and CS_QuestionnaireWinning.Id not in (");
            sql.AppendFormat(" select top {0} CS_QuestionnaireWinning.Id from CS_QuestionnaireWinning where CS_QuestionnaireWinning.QuestionnaireId = @QuestionnaireId order by CS_QuestionnaireWinning.Id desc", (pageIndex - 1) * pageSize);
            sql.AppendLine(" )");
            sql.AppendLine(" order by CS_QuestionnaireWinning.Id desc");
            return DbHelp.Query<QuestionnaireWinning>(sql.ToString(), new { QuestionnaireId = id });
        }
    }
}
