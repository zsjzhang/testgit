using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.IRepository;

namespace Vcyber.BLMS.Domain
{
    public class QuestionnaireVisitorApp : IQuestionnaireVisitorApp
    {
        public bool GetAllQuestionnaireVisitor()
        {
            
            throw new NotImplementedException();
        }

        public int AddQuestionnaireVisitor(QuestionnaireVisitor entity)
        {
            return _DbSession.QuestionnaireVisitorStorager.AddQuestionnaireVisitor(entity);
        }

        public bool UpdateQuestionnaireVisitor(QuestionnaireVisitor entity)
        {
            throw new NotImplementedException();
        }

        public bool DeleteQuestionnaireVisitor(QuestionnaireVisitor entity)
        {
            throw new NotImplementedException();
        }
    }
}
