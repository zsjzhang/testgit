using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.IRepository
{
    public interface IPwQuestionStorager
    {
        /// <summary>
        /// 添加问题
        /// </summary>
        /// <param name="question">问题</param>
        /// <param name="id">返回的数据行ID</param>
        /// <returns>执行结果</returns>
        bool Add(PwQuestion question, out int id);

        /// <summary>
        /// 查询所有问题
        /// </summary>
        /// <returns>问题集合</returns>
        IEnumerable<PwQuestion> Select();
    }
}
