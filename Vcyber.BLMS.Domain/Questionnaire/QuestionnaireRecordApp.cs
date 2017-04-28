using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.IRepository;

namespace Vcyber.BLMS.Domain
{
    public class QuestionnaireRecordApp : IQuestionnaireRecordApp
    {
        public bool IsQuestionnaire(string memberId, int questionnaireId)
        {
            return _DbSession.QuestionnaireRecordStorager.IsQuestionnaire(memberId, questionnaireId);
        }

        public bool AddQuestionnaireRecord(Entity.QuestionnaireRecord entity)
        {
            int id= _DbSession.QuestionnaireRecordStorager.AddQuestionnaireRecord(entity);
            if (id != null && id > 0)
                return true;
            else
                return false;
        }
    }
}
