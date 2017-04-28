using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vcyber.BLMS.Entity;

namespace Vcyber.BLMS.Application
{
    public interface IQuestionnaireWinningApp
    {
        List<QuestionnaireWinning> GetQuestionnaireWinning(int id);

        /// <summary>
        /// 添加获奖名单
        /// </summary>
        /// <param name="entity">获奖名单实体</param>
        /// <returns></returns>
        int Create(QuestionnaireWinning entity);

        IEnumerable<QuestionnaireWinning> SelectQuestionnaireWinning(int id, int pageIndex, int pageSize, out int total);
    }
}
