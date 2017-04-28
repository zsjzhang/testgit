using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Application;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;
using Vcyber.BLMS.IRepository;

namespace Vcyber.BLMS.Domain
{
    public class QuestionnaireApp : IQuestionnaireApp
    {
        public bool UpdateMemberShipEmail(string memberId, string emailVal)
        {
            return _DbSession.QuestionnaireStorager.UpdateMemberShipEmail(memberId, emailVal);
        }
        /// <summary>
        /// 获取全部问卷信息列表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Questionnaire> GetQuestionnaireList(int qType)
        {
            return _DbSession.QuestionnaireStorager.GetQuestionnaireList(qType);
        }

        public Questionnaire GetQuestionnatireById(int qid,int qType)
        {
            return _DbSession.QuestionnaireStorager.GetQuestionnatireById(qid,qType);
        }
        public IEnumerable<Questionnaire> selectQuestionnaire(EQuestionnaireType qType, string qName, string order, int pageIndex, int pageSize, out int total)
        {
            return _DbSession.QuestionnaireStorager.selectQuestionnaire(qType,qName, order, pageIndex, pageSize, out total);
        }

        public int SSICreate(SSIQuestion entity)
        {
            return _DbSession.QuestionnaireStorager.SSICreate(entity);
        }

        public Questionnaire GetCurQuestionnaireInfo(int qType)
        {
            return _DbSession.QuestionnaireStorager.GetCurrentQuestionnatire(qType);
        }

        public Questionnaire GetLastQuestionnatire(int qType)
        {
            return _DbSession.QuestionnaireStorager.GetLastQuestionnatire(qType);
        }

        public int Create(Questionnaire questionnaire, QuestionnaireManage questionnaireManage)
        {
            int result = 0;
            questionnaire.State = Convert.ToInt32(EQuestionnaireState.Ready);
            int qResult = _DbSession.QuestionnaireStorager.Create(questionnaire);

            if (qResult > 0)
            {
                questionnaireManage.ParentId = qResult;
                int qmResult = _DbSession.QuestionnaireManageStorager.Create(questionnaireManage);
                if (qmResult > 0)
                {
                    result = qResult;
                }
            }
            return result;
        }

        public bool CreateSelect(DateTime beginTime, int id, int qType)
        {
            return _DbSession.QuestionnaireStorager.CreateSelect(beginTime, id, qType);
        }

        public Questionnaire SelectSingle(int id)
        {
            return _DbSession.QuestionnaireStorager.SelectSingle(id);
        }


        public int GetCurQuestionnaireState(int qType)
        {
            return _DbSession.QuestionnaireStorager.GetCurrentQuestionnaireState(qType);
        }

        public bool Delete(int id)
        {
            return _DbSession.QuestionnaireStorager.Delete(id);
        }

        public bool UpdateState(int id, int state)
        {
            return _DbSession.QuestionnaireStorager.UpdateState(id, state);
        }

        public bool Edit(Questionnaire questionnaire)
        {
            return _DbSession.QuestionnaireStorager.Edit(questionnaire);
        }

        public bool IsCSManager(string userId, string roleName)
        {
            return _DbSession.QuestionnaireStorager.IsCSManager(userId, roleName);
        }
    }
}
