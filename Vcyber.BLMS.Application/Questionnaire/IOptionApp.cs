using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.Application
{
    public interface IOptionApp
    {
        /// <summary>
        /// 获取全部选项
        /// </summary>
        /// <returns></returns>
        List<Vcyber.BLMS.Entity.Option> GetAllOptions();
        /// <summary>
        /// 根据题号获取选项列表
        /// </summary>
        /// <param name="qid"></param>
        /// <returns></returns>
        List<Vcyber.BLMS.Entity.Option> GetOptionByQId(int qid);
        /// <summary>
        /// 根据题号获取选项列表
        /// </summary>
        /// <param name="qid"></param>
        /// <returns></returns>
        List<Vcyber.BLMS.Entity.Option> GetOptionsByQIdAndOType(int qid, int oType);
        /// <summary>
        /// 创建选项
        /// </summary>
        /// <param name="entity">选项实体</param>
        /// <returns></returns>
        int Create(Option entity);

        /// <summary>
        /// 修改选项
        /// </summary>
        /// <param name="entity">选项实体</param>
        /// <returns></returns>
        bool Edit(Option entity);

        /// <summary>
        /// 删除选项
        /// </summary>
        /// <param name="Id">选项id</param>
        /// <param name="parentId">所属问题id</param>
        /// <param name="sort">排序</param>
        /// <returns></returns>
        bool Delete(int Id,int parentId,int sort);

        /// <summary>
        /// 修改选项内容
        /// </summary>
        /// <param name="content">选项内容</param>
        /// <param name="id">选项id</param>
        /// <returns></returns>
        bool EditContent(string content, int id);

        /// <summary>
        /// 修改选项排序
        /// </summary>
        /// <param name="sort">排序</param>
        /// <param name="id">选项id</param>
        /// <param name="parentId">所属问题id</param>
        /// <param name="ySort">原排序</param>
        /// <returns></returns>
        bool EditSort(int sort, int id,int parentId,int ySort);

        /// <summary>
        /// 查询出满意度题的最大选项
        /// </summary>
        /// <param name="parentId">问题id</param>
        /// <returns></returns>
        int MaxOption(int parentId);

        /// <summary>
        /// 删除满意度题的最大选项
        /// </summary>
        /// <param name="parentId">问题id</param>
        /// <returns></returns>
        bool DeleteMaxOption(int parentId);

        bool EditType(int type, int id);

        bool EditValueType(int vType, int id);


    }
}
