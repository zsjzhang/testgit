using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.IRepository;
using Vcyber.BLMS.Common;

namespace Vcyber.BLMS.Repository
{
    public class QuestionnaireRecordStorager : IQuestionnaireRecordStorager
    {
        public bool IsQuestionnaire(string memberId, int questionnaireId)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("select COUNT(*) from CS_MemberForQuestionnaireManage where MemberId='{0}'", memberId);
            sql.AppendFormat(" and QuestionnaireId={0}  and State=2", questionnaireId);
            int count = DbHelp.ExecuteScalar<int>(sql.ToString());
            if (count > 0)
                return true;
            else
                return false;
        }

        public int AddQuestionnaireRecord(BLMS.Entity.QuestionnaireRecord entity)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("INSERT INTO CS_MemberForQuestionnaireManage(MemberId,QuestionnaireId,State,CreateTime,ContactId) VALUES");
            sql.Append("(@MemberId,@QuestionnaireId,@State,@CreateTime,@ContactId);SELECT @@identity");
            int id = DbHelp.ExecuteScalar<int>(sql.ToString(), entity);
            return id;
        }


        public int UpdateQuestionnaireRecord(QuestionnaireRecord entity)
        {
            throw new NotImplementedException();
        }

        public int DeleteQuestionnaireRecord(QuestionnaireRecord entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<QuestionnaireRecord> GetQuestionnaireRecordsByMemberId(string memberId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<QuestionnaireRecord> GetQuestionnaireRecordsByQId(int qid)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<QuestionnaireRecord> GetAllQuestionnaireRecord()
        {
            throw new NotImplementedException();
        }
    }
}
