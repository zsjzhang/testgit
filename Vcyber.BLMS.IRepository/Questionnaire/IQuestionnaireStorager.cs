using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;
using Vcyber.BLMS.Entity.Enum;

namespace Vcyber.BLMS.IRepository
{
    public interface IQuestionnaireStorager
    {

        #region ============前台操作=============
        bool UpdateMemberShipEmail(string memberId, string emailVal);
        /// <summary>
        /// 获取全部问卷信息
        /// </summary>
        /// <returns></returns>
        IEnumerable<Questionnaire> GetQuestionnaireList(int qType);
        /// <summary>
        /// 获取当前问卷信息
        /// </summary>
        /// <returns></returns>
        Questionnaire GetCurrentQuestionnatire(int qType);
        Questionnaire GetQuestionnatireById(int qid, int qType);

        Questionnaire GetLastQuestionnatire(int qType);

        int GetCurrentQuestionnaireState(int qType);

        #endregion

        #region ==== 后台操作 ====
        /// <summary>
        /// 分页查询问卷列表
        /// </summary>
        /// <param name="qName"></param>
        /// <param name="order"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        IEnumerable<Questionnaire> selectQuestionnaire(EQuestionnaireType qType, string qName, string order, int pageIndex, int pageSize,out int total);


        int SSICreate(SSIQuestion entity);
        /// <summary>
        /// 新增问卷
        /// </summary>
        /// <param name="entity">问卷实体</param>
        /// <returns></returns>
        int Create(Questionnaire entity);

        /// <summary>
        /// 新增时，查询是否存在时间重叠的问卷
        /// </summary>
        /// <param name="beginTime">开始时间</param>
        /// <returns></returns>
        bool CreateSelect(DateTime beginTime, int id,int qType);

        /// <summary>
        /// 修改问卷
        /// </summary>
        /// <param name="entity">问卷实体</param>
        /// <returns></returns>
        bool Edit(Questionnaire entity);

        /// <summary>
        /// 删除问卷（不删除数据，只修改状态）
        /// </summary>
        /// <param name="id">删除数据的ID</param>
        /// <returns></returns>
        bool Delete(int id);

        /// <summary>
        /// 根据id查询指定问卷
        /// </summary>
        /// <param name="id">问卷id</param>
        /// <returns></returns>
        Questionnaire SelectSingle(int id);

        /// <summary>
        /// 更新问卷状态
        /// </summary>
        /// <param name="id">问卷id</param>
        /// <param name="state">当前问卷状态</param>
        /// <returns></returns>
        bool UpdateState(int id, int state);

        /// <summary>
        /// 问卷模块验证是否为CS管理员
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="roleName">权限名称</param>
        /// <returns></returns>
        bool IsCSManager(string userId, string roleName);
        #endregion
    }
}
