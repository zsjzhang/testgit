using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.IRepository
{
    public interface IQuestionnaireRecordStorager
    {/// <summary>
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
        int AddQuestionnaireRecord(QuestionnaireRecord entity);

        /// <summary>
        /// 修改问卷完成记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int UpdateQuestionnaireRecord(QuestionnaireRecord entity);

        /// <summary>
        /// 删除问卷完成记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int DeleteQuestionnaireRecord(QuestionnaireRecord entity);

        /// <summary>
        /// 根据用户编号获取问卷完成记录
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        IEnumerable<QuestionnaireRecord> GetQuestionnaireRecordsByMemberId(string memberId);

        /// <summary>
        /// 获取某一期问卷的完成记录信息
        /// </summary>
        /// <param name="qid"></param>
        /// <returns></returns>
        IEnumerable<QuestionnaireRecord> GetQuestionnaireRecordsByQId(int qid);

        /// <summary>
        /// 获取全部的问卷完成记录信息
        /// </summary>
        /// <returns></returns>
        IEnumerable<QuestionnaireRecord> GetAllQuestionnaireRecord();
    }
}
