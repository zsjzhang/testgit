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
    public class QuestionnaireWinningApp : IQuestionnaireWinningApp
    {
        public List<Entity.QuestionnaireWinning> GetQuestionnaireWinning(int id)
        {
            return _DbSession.QuestionnaireWinningStorager.GetQuestionnaireWinning(id).ToList();
        }

        public int Create(QuestionnaireWinning entity)
        {
            return _DbSession.QuestionnaireWinningStorager.Create(entity);
        }


        public IEnumerable<QuestionnaireWinning> SelectQuestionnaireWinning(int id, int pageIndex, int pageSize, out int total)
        {
            return _DbSession.QuestionnaireWinningStorager.SelectQuestionnaireWinning(id, pageIndex, pageSize, out total);
        }
    }
}
