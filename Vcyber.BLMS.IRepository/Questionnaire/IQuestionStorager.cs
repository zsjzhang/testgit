using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.IRepository
{
    public interface IQuestionStorager
    {
        #region ===============前台操作=================

        /// <summary>
        /// 根据问卷编号获取问题列表
        /// </summary>
        /// <param name="pid">问卷编号</param>
        /// <returns></returns>
        IEnumerable<Vcyber.BLMS.Entity.Question> GetQuestionsByPId(int pid);

        /// <summary>
        /// 获取所有的问题列表
        /// </summary>
        /// <returns></returns>
        IEnumerable<Vcyber.BLMS.Entity.Question> GetAllQuestion();

        #endregion

        #region ==== 后台操作 ====
        /// <summary>
        /// 添加问题
        /// </summary>
        /// <param name="entity">问题实体</param>
        /// <returns></returns>
        int Create(Question entity);

        /// <summary>
        /// 修改问题
        /// </summary>
        /// <param name="entity">修改的数据</param>
        /// <returns></returns>
        bool Edit(Question entity);

        /// <summary>
        /// 修改问题内容
        /// </summary>
        /// <param name="content">修改后的内容</param>
        /// <param name="id">问题id</param>
        /// <returns></returns>
        bool EditContent(string content, int id);

        /// <summary>
        /// 修改排序
        /// </summary>
        /// <param name="sort">修改后的排序</param>
        /// <param name="id">问题id</param>
        /// <returns></returns>
        bool EditSort(int sort, int id);

        /// <summary>
        /// 批量修改排序（拖动后，修改前位置和现位置之间问题的排序）
        /// </summary>
        /// <param name="sort">现排序</param>
        /// <param name="ySort">原排序</param>
        /// <param name="parentId">问卷id</param>
        /// <returns></returns>
        bool EditBatchSort(int sort, int ySort, int parentId);

        /// <summary>
        /// 修改问题是否为必填项
        /// </summary>
        /// <param name="isRequired">是否为必填</param>
        /// <param name="id">问题id</param>
        /// <returns></returns>
        bool EditIsRequired(bool isRequired, int id);

        /// <summary>
        /// 删除问题（不删除数据，只修改状态）
        /// </summary>
        /// <param name="id">删除数据ID</param>
        /// <returns></returns>
        bool Delete(int id);

        /// <summary>
        /// 删除时批量排序
        /// </summary>
        /// <param name="sort">删除问题的所在排序</param>
        /// <param name="parentId">问卷id</param>
        /// <returns></returns>
        bool DelBatchSort(int sort, int parentId);

        /// <summary>
        /// 根据id获取问题
        /// </summary>
        /// <param name="id">问题id</param>
        /// <returns></returns>
        Question GetById(int id);

        /// <summary>
        /// 获取矩阵题目的子问题
        /// </summary>
        /// <param name="parentId">父问题id</param>
        /// <returns></returns>
        IEnumerable<Question> GetJzChildrenQuestion(int parentId);

        /// <summary>
        /// 修改矩阵填空题的循环次数
        /// </summary>
        /// <param name="id">问题id</param>
        /// <param name="count">循环次数</param>
        /// <returns></returns>
        bool AddCycleOption(int id, int count);

        bool EditQuestionTextIsBefore(bool textIsBefore, int id);

        bool EditQuestionIsChecked(bool isChecked, int id);

        #endregion
    }
}
