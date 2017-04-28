using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.Application
{
    public interface IQuestionnaireRecordApp
    {
        /// <summary>
        /// 判断用户是否已经完成过指定的问卷
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="questionnaireId"></param>
        /// <returns></returns>
        bool IsQuestionnaire(string memberId, int questionnaireId);

        /// <summary>
        /// 添加问卷完成记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool AddQuestionnaireRecord(QuestionnaireRecord entity);
    }
}
