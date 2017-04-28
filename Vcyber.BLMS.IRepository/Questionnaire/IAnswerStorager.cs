using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.IRepository
{
    public interface IAnswerStorager
    {
        /// <summary>
        /// 添加一条答案记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int AddAnswer(Answer entity);

        /// <summary>
        /// 添加一个答案集合
        /// </summary>
        /// <param name="entities"></param>
        /// <returns>影响的行数</returns>
        int AddAnswerRang(List<Answer> entities);

        /// <summary>
        /// 判断用户是否完成过问卷
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        bool IsAnswer(string memberId, int pid);
    }
}
