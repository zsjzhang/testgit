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
    public class QuestionApp : IQuestionApp
    {
        public List<Question> GetAllQuestions()
        {
            return _DbSession.QuesStorager.GetAllQuestion().ToList();
        }

        public List<Question> GetQuestionByPId(int pid)
        {
            return _DbSession.QuesStorager.GetQuestionsByPId(pid).ToList();
        }

        public int Create(Question qEntity)
        {
            return _DbSession.QuesStorager.Create(qEntity);

        }

        public bool Edit(Question qEntity)
        {
            return _DbSession.QuesStorager.Edit(qEntity);
        }

        public bool Delete(int Id,int parentId,int sort)
        {
            bool result = false;
            if (_DbSession.QuesStorager.Delete(Id))
            {
                result = _DbSession.QuesStorager.DelBatchSort(sort, parentId);
            }
            return result;
        }



        public bool EditContent(string content, int id)
        {
            return _DbSession.QuesStorager.EditContent(content, id);
        }

        public bool EditSort(int sort, int id, int parentId, int ySort)
        {
            bool result = false;
            if(_DbSession.QuesStorager.EditBatchSort(sort,ySort,parentId))
            {
                result=_DbSession.QuesStorager.EditSort(sort, id);
            }
            return result;
        }

        public bool EditIsRequired(bool isRequired, int id)
        {
            return _DbSession.QuesStorager.EditIsRequired(isRequired, id);
        }


        public Question GetById(int id)
        {
            return _DbSession.QuesStorager.GetById(id);
        }

        public List<Question> GetJzChildrenQuestion(int parentId)
        {
            return _DbSession.QuesStorager.GetJzChildrenQuestion(parentId).ToList();
        }

        public bool AddCycleOption(int id, int count)
        {
            return _DbSession.QuesStorager.AddCycleOption(id, count);
        }

        public bool EditQuestionTextIsBefore(bool textIsBefore, int id)
        {
            return _DbSession.QuesStorager.EditQuestionTextIsBefore(textIsBefore, id);
        }

        public bool EditQuestionIsChecked(bool isChecked, int id)
        {
            return _DbSession.QuesStorager.EditQuestionIsChecked(isChecked, id);
        }
    }
}
