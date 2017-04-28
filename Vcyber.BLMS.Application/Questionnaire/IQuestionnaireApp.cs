using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;

namespace Vcyber.BLMS.Application
{
    public interface IQuestionnaireApp
    {
        bool UpdateMemberShipEmail(string memberId, string emailVal);
        /// <summary>
        /// 获取问卷信息列表
        /// </summary>
        /// <returns></returns>
        IEnumerable<Questionnaire> GetQuestionnaireList(int qType);
        Questionnaire GetCurQuestionnaireInfo(int qType);
        Questionnaire GetLastQuestionnatire(int qType);
        Questionnaire GetQuestionnatireById(int qid, int qType);
        int SSICreate(SSIQuestion entity);
        int GetCurQuestionnaireState(int qType);

        IEnumerable<Questionnaire> selectQuestionnaire(EQuestionnaireType qType, string qName, string order, int pageIndex, int pageSize, out int total);

        /// <summary>
        /// 添加问卷
        /// </summary>
        /// <param name="questionnaire">问卷实体</param>
        /// <param name="questionnaireManage">问卷管理员关系实体</param>
        /// <returns></returns>
        int Create(Questionnaire questionnaire, QuestionnaireManage questionnaireManage);

        /// <summary>
        /// 创建问卷是查询是否有问卷时间冲突
        /// </summary>
        /// <param name="beginTime">现问卷开始时间</param>
        /// <param name="id">现问卷id</param>
        /// <returns></returns>
        bool CreateSelect(DateTime beginTime, int id, int qType);

        /// <summary>
        /// 获取指定的问卷
        /// </summary>
        /// <param name="id">问卷id</param>
        /// <returns></returns>
        Questionnaire SelectSingle(int id);

        /// <summary>
        /// 删除问卷
        /// </summary>
        /// <param name="id">问卷id</param>
        /// <returns></returns>
        bool Delete(int id);

        /// <summary>
        /// 更新问卷状态
        /// </summary>
        /// <param name="id">问卷id</param>
        /// <param name="state">问卷状态</param>
        /// <returns></returns>
        bool UpdateState(int id, int state);

        /// <summary>
        /// 修改问卷
        /// </summary>
        /// <param name="questionnaire">问卷实体</param>
        /// <returns></returns>
        bool Edit(Questionnaire questionnaire);

        /// <summary>
        /// 问卷模块验证是否为CS管理员
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="roleName">权限名称</param>
        /// <returns></returns>
        bool IsCSManager(string userId, string roleName);

    }
}
