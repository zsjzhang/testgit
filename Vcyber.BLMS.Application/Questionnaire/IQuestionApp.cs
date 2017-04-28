using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.Application
{
    public interface IQuestionApp
    {
        /// <summary>
        /// 获取全部问题列表
        /// </summary>
        /// <returns></returns>
        List<Vcyber.BLMS.Entity.Question> GetAllQuestions();
        
        /// <summary>
        /// 根据问卷编号获取问题列表
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        List<Vcyber.BLMS.Entity.Question> GetQuestionByPId(int pid);

        /// <summary>
        /// 创建问题
        /// </summary>
        /// <param name="qEntity">问题实体</param>
        /// <returns></returns>
        int Create(Question qEntity);

        /// <summary>
        /// 修改问题
        /// </summary>
        /// <param name="qEntity">问题实体</param>
        /// <returns></returns>
        bool Edit(Question qEntity);

        /// <summary>
        /// 删除问题
        /// </summary>
        /// <param name="Id">问题id</param>
        /// <param name="parentId">问卷id</param>
        /// <param name="sort">该问题排序</param>
        /// <returns></returns>
        bool Delete(int Id,int parentId,int sort);

        /// <summary>
        /// 修改问题内容
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="id">问题id</param>
        /// <returns></returns>
        bool EditContent(string content, int id);

        /// <summary>
        /// 修改问题排序
        /// </summary>
        /// <param name="sort">排序</param>
        /// <param name="id">问题id</param>
        /// <param name="parentId">所属问卷id</param>
        /// <param name="ySort">该问题的原排序</param>
        /// <returns></returns>
        bool EditSort(int sort, int id, int parentId, int ySort);

        /// <summary>
        /// 修改该问题是否必填
        /// </summary>
        /// <param name="isRequired">是否必填</param>
        /// <param name="id">问题id</param>
        /// <returns></returns>
        bool EditIsRequired(bool isRequired, int id);

        /// <summary>
        /// 根据id获取问题
        /// </summary>
        /// <param name="id">问题id</param>
        /// <returns></returns>
        Question GetById(int id);

        /// <summary>
        /// 获取矩阵问题的子问题列表
        /// </summary>
        /// <param name="parentId">父问题id</param>
        /// <returns></returns>
        List<Question> GetJzChildrenQuestion(int parentId);

        /// <summary>
        /// 修改矩阵填空题的循环次数
        /// </summary>
        /// <param name="id">问题id</param>
        /// <param name="count">循环次数</param>
        /// <returns></returns>
        bool AddCycleOption(int id, int count);

        bool EditQuestionTextIsBefore(bool textIsBefore, int id);

        bool EditQuestionIsChecked(bool isChecked, int id);
    }
}
